using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Channels;

namespace Lennox.NvEncSharp.Sample.VideoDecode
{
    internal unsafe class Program : IDisposable
    {
        public static void Main(string[] args)
        {
            using var program = new Program();
            var result = program.Run(new ProgramArguments(args));
            Environment.Exit(result);
        }

        private static void PrintInformation(string title, Dictionary<string, object> info)
        {
            Console.WriteLine(title);
            foreach (var (key, value) in info)
            {
                Console.WriteLine($"  + {key}: {value}");
            }

            Console.WriteLine();
        }

        private CuContext _context;
        private CuVideoDecoder _decoder;
        private CuVideoContextLock _contextLock;
        private Channel<FrameInformation> _framesChannel;
        private CuVideoDecodeCreateInfo _info;
        private YuvInformation _yuvInfo;
        private CancellationTokenSource _cts;
        private Thread _displayThread;
        private ProgramArguments _args;
        private int _bitmapIndex = 0;

        private bool _useDeviceMemory => !_args.UseHostMemory;
        private bool _isDisposed = false;

        private class FrameInformation : IDisposable
        {
            public IntPtr Bytes;
            public CuMemoryType Type;
            public CuDeviceMemory DeviceMemory;
            public int Pitch;
            public CuVideoDecodeCreateInfo Info;
            public YuvInformation YuvInfo;

            private int _disposed = 0;

            public void Dispose()
            {
                if (Interlocked.Exchange(ref _disposed, 1) != 0) return;

                switch (Type)
                {
                    case CuMemoryType.Device:
                        DeviceMemory.Dispose();
                        break;
                    case CuMemoryType.Host:
                        // TODO: put buffer back into the pool here.
                        Marshal.FreeHGlobal(Bytes);
                        break;
                }

            }
        }

        private int Run(ProgramArguments args)
        {
            _args = args;
            _framesChannel = Channel.CreateBounded<FrameInformation>(
                new BoundedChannelOptions(10)
                {
                    FullMode = BoundedChannelFullMode.DropOldest,
                    SingleReader = true
                });

            _cts = new CancellationTokenSource();
            _displayThread = new Thread(DisplayThread)
            {
                IsBackground = true,
                Name = nameof(DisplayThread)
            };

            _displayThread.Start();

            LibCuda.Initialize();

            var descriptions = CuDevice.GetDescriptions().ToArray();

            if (descriptions.Length == 0)
            {
                Console.Error.WriteLine("No CUDA devices found.");
                return -1;
            }

            foreach (var description in descriptions)
            {
                Console.WriteLine($"Device {description.Handle}");
                Console.WriteLine($"  + Memory: {description.TotalMemory}");
                Console.WriteLine($"  + Name: {description.Name}");
                Console.WriteLine($"  + MaxThreadsPerBlock: {description.GetAttribute(CuDeviceAttribute.MaxThreadsPerBlock)}");
                Console.WriteLine($"  + SharedMemoryPerBlock: {description.GetAttribute(CuDeviceAttribute.SharedMemoryPerBlock)}");
            }

            var device = descriptions[0].Device;

            _context = device.CreateContext(CuContextFlags.SchedBlockingSync);
            _contextLock = _context.CreateLock();

            var parserParams = new CuVideoParserParams
            {
                CodecType = CuVideoCodec.H264,
                MaxNumDecodeSurfaces = 1,
                MaxDisplayDelay = 0,
                ErrorThreshold = 100,
                UserData = IntPtr.Zero,
                DisplayPicture = VideoDisplayCallback,
                DecodePicture = DecodePictureCallback,
                SequenceCallback = SequenceCallback
            };

            using var parser = CuVideoParser.Create(ref parserParams);
            using var fs = File.OpenRead(args.InputPath);
            const int bufferSize = 10 * 1024 * 1024;
            var inputBufferPtr = Marshal.AllocHGlobal(bufferSize);
            var count = 0;

            var backbuffer = new byte[10 * 1024 * 1024];
            var backbufferStream = new MemoryStream(backbuffer);

            void SendBackbuffer()
            {
                if (backbufferStream.Position == 0) return;

                var span = new Span<byte>(
                    backbuffer, 0,
                    (int) backbufferStream.Position);

                parser.ParseVideoData(span);

                Array.Fill<byte>(backbuffer, 0);
                backbufferStream = new MemoryStream(backbuffer);
            }

            while (true)
            {
                var inputBuffer = new Span<byte>(
                    (void*) inputBufferPtr, bufferSize);

                var nread = fs.Read(inputBuffer);
                if (nread == 0) break;

                var inputStream = inputBuffer.Slice(0, nread);

                while (true)
                {
                    var packet = NalPacket.ReadNextPacket(ref inputStream);

                    if (packet.PacketPrefix.Length > 0)
                    {
                        backbufferStream.Write(packet.PacketPrefix);
                        SendBackbuffer();
                    }

                    if (packet.Packet.Length == 0)
                    {
                        break;
                    }

                    if (packet.Complete)
                    {
                        parser.ParseVideoData(packet.Packet);
                    }
                    else
                    {
                        backbufferStream.Write(packet.Packet);
                    }

                    ++count;

                    if (inputStream.Length == 0) break;
                }
            }

            parser.SendEndOfStream();
            Marshal.FreeHGlobal(inputBufferPtr);
            Console.WriteLine($"Sent {count} packets.");

            return 0;
        }

        private CuCallbackResult VideoDisplayCallback(
            IntPtr data, ref CuVideoParseDisplayInfo info)
        {
            using var _ = _context.Push();

            var processingParam = new CuVideoProcParams
            {
                ProgressiveFrame = info.ProgressiveFrame,
                SecondField = info.RepeatFirstField + 1,
                TopFieldFirst = info.TopFieldFirst,
                UnpairedField = info.RepeatFirstField < 0 ? 1 : 0
            };

            using var srcFrame = _decoder.MapVideoFrame(
                info.PictureIndex, ref processingParam,
                out var pitch);

            var status = _decoder.GetDecodeStatus(info.PictureIndex);

            if (status != CuVideoDecodeStatus.Success)
            {
                //throw new Exception(status.ToString());
            }

            var frameSize = _info.GetFrameSize(out var chromaHeight);

            CuDeviceMemory frameDevicePtr = default;
            IntPtr frameLocalPtr = default;
            CuMemoryType destMemoryType;

            if (_useDeviceMemory)
            {
                frameDevicePtr = CuDeviceMemory.Allocate(frameSize);
                destMemoryType = CuMemoryType.Device;
            }
            else
            {
                frameLocalPtr = Marshal.AllocHGlobal((int) frameSize);
                destMemoryType = CuMemoryType.Host;
            }

            var byteWidth = _info.Width * _info.GetBytesPerPixel();

            // Copy luma
            var memcopy = new CudaMemcopy2D
            {
                SrcMemoryType = CuMemoryType.Device,
                SrcDevice = srcFrame,
                SrcPitch = (IntPtr)pitch,
                DstMemoryType = destMemoryType,
                DstDevice = frameDevicePtr,
                DstHost = frameLocalPtr,
                DstPitch = (IntPtr)byteWidth,
                WidthInBytes = (IntPtr)byteWidth,
                Height = (IntPtr)_info.Height
            };

            memcopy.Memcpy2D();

            // Copy chroma
            memcopy.SrcDevice = new CuDevicePtr(srcFrame.Handle.ToInt64() + pitch * _info.Height);
            memcopy.DstDevice = new CuDevicePtr(frameDevicePtr.Handle + byteWidth * _info.Height);
            memcopy.DstHost = (IntPtr)(frameLocalPtr + byteWidth * _info.Height);
            memcopy.Height = (IntPtr)chromaHeight;
            memcopy.Memcpy2D();

            _framesChannel.Writer.TryWrite(new FrameInformation
            {
                Bytes = frameLocalPtr,
                DeviceMemory = frameDevicePtr,
                Type = destMemoryType,
                Pitch = pitch,
                Info = _info,
                YuvInfo = _yuvInfo
            });

            return CuCallbackResult.Success;
        }

        private CuCallbackResult DecodePictureCallback(
            IntPtr data, ref CuVideoPicParams param)
        {
            _decoder.DecodePicture(ref param);
            return CuCallbackResult.Success;
        }

        private CuCallbackResult SequenceCallback(
            IntPtr data, ref CuVideoFormat format)
        {
            using var _ = _context.Push();

            PrintInformation("CuVideoFormat", new Dictionary<string, object>
            {
                ["Codec"] = format.Codec,
                ["Bitrate"] = format.Bitrate,
                ["CodedWidth"] = format.CodedWidth,
                ["CodedHeight"] = format.CodedHeight,
                ["Framerate"] = format.FrameRateNumerator / format.FrameRateDenominator,
            });

            if (!format.IsSupportedByDecoder(out var error, out var caps))
            {
                Console.Error.WriteLine(error);
                Environment.Exit(-1);
                return CuCallbackResult.Failure;
            }

            PrintInformation("CuVideoDecodeCaps", new Dictionary<string, object>
            {
                ["MaxWidth"] = caps.MaxWidth,
                ["MaxHeight"] = caps.MaxHeight,
            });

            if (!_decoder.IsEmpty)
            {
                _decoder.Reconfigure(ref format);
                return CuCallbackResult.Success;
            }

            _info = new CuVideoDecodeCreateInfo
            {
                CodecType = format.Codec,
                ChromaFormat = format.ChromaFormat,
                OutputFormat = format.GetSurfaceFormat(),
                BitDepthMinus8 = format.BitDepthLumaMinus8,
                DeinterlaceMode = format.ProgressiveSequence
                    ? CuVideoDeinterlaceMode.Weave
                    : CuVideoDeinterlaceMode.Adaptive,
                NumOutputSurfaces = 2,
                CreationFlags = CuVideoCreateFlags.PreferCUVID,
                NumDecodeSurfaces = format.MinNumDecodeSurfaces,
                VideoLock = _contextLock,
                Width = format.CodedWidth,
                Height = format.CodedHeight,
                MaxWidth = format.CodedWidth,
                MaxHeight = format.CodedHeight,
                TargetWidth = format.CodedWidth,
                TargetHeight = format.CodedHeight
            };

            _yuvInfo = _info.GetYuvInformation();

            _decoder = CuVideoDecoder.Create(ref _info);

            return (CuCallbackResult)format.MinNumDecodeSurfaces;
        }

        private void DisplayThread()
        {
            while (!_isDisposed)
            {
                try
                {
                    DisplayThreadCore();
                }
                catch (OperationCanceledException)
                {
                    // Program is exiting.
                    return;
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                    throw;
                }
            }
        }

        private void DisplayThreadCore()
        {
            using var frame = _framesChannel.Reader
                .ReadAsync(_cts.Token)
                .AsTask().Result;

            using var _ = _context.Push();

            if (_args.WriteBitmap)
            {
                SaveAsBitmap(
                    frame,
                    Path.Combine(_args.BitmapPath, $"output-{_bitmapIndex:D8}.bmp"));

                ++_bitmapIndex;
            }
        }

        private static void SaveAsBitmap(
            FrameInformation frame,
            string filename)
        {
            var width = frame.Info.Width;
            var height = frame.Info.Height;

            using var bitmap = new Bitmap(
                width, height,
                PixelFormat.Format32bppRgb);

            var locked = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.WriteOnly, bitmap.PixelFormat);

            const int rgbBpp = 4;
            var rgbPtr = (byte*)locked.Scan0;
            var rgbSize = width * height * rgbBpp;

            switch (frame.Type)
            {
                case CuMemoryType.Host:
                    LibYuvSharp.LibYuv.NV12ToARGB(
                        (byte*)frame.Bytes, frame.YuvInfo.LumaPitch,
                        (byte*)frame.Bytes + frame.YuvInfo.ChromaOffset,
                        frame.YuvInfo.ChromaPitch,
                        rgbPtr, width * rgbBpp, width, height);

                    break;
                case CuMemoryType.Device:
                    using (var destPtr = CuDeviceMemory.Allocate(rgbSize))
                    {
                        LibCudaLibrary.Nv12ToBGRA32(
                            frame.DeviceMemory.Handle, width,
                            destPtr, width * rgbBpp, width, height);

                        destPtr.CopyToHost(rgbPtr, rgbSize);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(frame.Type));
            }

            bitmap.UnlockBits(locked);
            bitmap.Save(filename, ImageFormat.Bmp);
        }

        public void Dispose()
        {
            _isDisposed = true;
            _context.Dispose();
            _decoder.Dispose();
            _contextLock.Dispose();
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}