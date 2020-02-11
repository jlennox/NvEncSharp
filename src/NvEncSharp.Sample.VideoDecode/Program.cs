using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Channels;

#nullable enable

namespace Lennox.NvEncSharp.Sample.VideoDecode
{
    internal unsafe class Program : IDisposable
    {
        public static void Main(string[] args)
        {
            using var program = new Program(new ProgramArguments(args));
            var result = program.Run();
            Environment.Exit(result);
        }

        private CuContext _context;
        private CuVideoDecoder _decoder;
        private CuVideoContextLock _contextLock;
        private readonly Channel<FrameInformation> _framesChannel;
        private CuVideoDecodeCreateInfo _info;
        private readonly CancellationTokenSource _cts;
        private readonly Thread _displayThread;
        private readonly ProgramArguments _args;
        private int _displayedFrames;
        private readonly Pool<BufferStorage> _nv12BufferPool = new Pool<BufferStorage>(5);
        private readonly ManualResetEventSlim _renderingCompleted = new ManualResetEventSlim(false);
        private DisplayWindow? _window;

        private bool _useHostMemory => _args.UseHostMemory;
        private bool _isDisposed = false;

        public Program(ProgramArguments args)
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
        }

        private int Run()
        {
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
                PrintInformation($"Device {description.Handle}", new Dictionary<string, object> {
                    ["Memory"] = description.TotalMemory,
                    ["Name"] = description.Name,
                    ["PciBusId"] = description.GetPciBusId(),
                    ["MaxThreadsPerBlock"] = description.GetAttribute(CuDeviceAttribute.MaxThreadsPerBlock),
                    ["SharedMemoryPerBlock"] = description.GetAttribute(CuDeviceAttribute.SharedMemoryPerBlock)
                });
            }

            var device = descriptions[0].Device;

            _context = device.CreateContext(CuContextFlags.SchedBlockingSync);
            _contextLock = _context.CreateLock();

            _window = new DisplayWindow(_context, "Decoded video", 800, 600);

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
            using var fs = File.OpenRead(_args.InputPath);
            const int bufferSize = 10 * 1024 * 1024;
            var inputBufferPtr = Marshal.AllocHGlobal(bufferSize);
            var count = 0;

            var backbuffer = new byte[50 * 1024 * 1024];
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

            var watch = Stopwatch.StartNew();

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

            _renderingCompleted.Wait();
            watch.Stop();

            Marshal.FreeHGlobal(inputBufferPtr);
            Console.WriteLine($"Sent {count} packets.");
            Console.WriteLine($"Rendered {_displayedFrames} in {watch.Elapsed}. ~{Math.Round(_displayedFrames / watch.Elapsed.TotalSeconds, 2)} FPS.");

            return 0;
        }

        private void AllocateNv12FrameBuffer(
            CuMemoryType type, IntPtr frameByteSize,
            out CuDeviceMemory frameDevicePtr,
            out IntPtr frameLocalPtr)
        {
            frameDevicePtr = default;
            frameLocalPtr = default;

            var pooled = _nv12BufferPool.Get();

            if (pooled != null)
            {
                // Only use exact size matches to allow memory reduction when
                // sizing down.
                if (pooled.Size == frameByteSize &&
                    pooled.MemoryType == type)
                {
                    frameLocalPtr = pooled.Bytes;
                    frameDevicePtr = pooled.DeviceMemory;
                    return;
                }

                // The memory type or size has changed. Deallocate the old
                // buffer and allocate new.
                pooled.Dispose();
            }

            if (type == CuMemoryType.Host)
            {
                frameLocalPtr = Marshal.AllocHGlobal(frameByteSize);
                return;
            }

            frameDevicePtr = CuDeviceMemory.Allocate(frameByteSize);
        }

        private CuCallbackResult VideoDisplayCallback(
            IntPtr data, IntPtr infoPtr)
        {
            using var _ = _context.Push();

            if (CuVideoParseDisplayInfo.IsFinalFrame(infoPtr, out var info))
            {
                if (!_framesChannel.Writer.TryWrite(
                    FrameInformation.FinalFrame))
                {
                    _renderingCompleted.Set();
                }

                return CuCallbackResult.Success;
            }

            var processingParam = new CuVideoProcParams
            {
                ProgressiveFrame = info.ProgressiveFrame,
                SecondField = info.RepeatFirstField + 1,
                TopFieldFirst = info.TopFieldFirst,
                UnpairedField = info.RepeatFirstField < 0 ? 1 : 0
            };

            using var frame = _decoder.MapVideoFrame(
                info.PictureIndex, ref processingParam,
                out var pitch);

            var yuvInfo = _info.GetYuvInformation(pitch);
            var status = _decoder.GetDecodeStatus(info.PictureIndex);

            if (status != CuVideoDecodeStatus.Success)
            {
                // TODO: Determine what to do in this situation. This condition
                // is non-exceptional but may require different handling?
            }

            var frameByteSize = _info.GetFrameByteSize(
                pitch, out var chromaHeight);

            var destMemoryType = _useHostMemory
                ? CuMemoryType.Host
                : CuMemoryType.Device;

            AllocateNv12FrameBuffer(
                destMemoryType, frameByteSize,
                out var frameDevicePtr, out var frameLocalPtr);

            var byteWidth = _info.Width * _info.GetBytesPerPixel();

            // Copy luma
            var memcopy = new CuMemcopy2D
            {
                SrcMemoryType = CuMemoryType.Device,
                SrcDevice = frame,
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
            memcopy.SrcDevice = new CuDevicePtr(frame.Handle + pitch * _info.Height);
            memcopy.DstDevice = new CuDevicePtr(frameDevicePtr.Handle + byteWidth * _info.Height);
            memcopy.DstHost = frameLocalPtr + byteWidth * _info.Height;
            memcopy.Height = (IntPtr)chromaHeight;
            memcopy.Memcpy2D();

            var bufferStorage = new BufferStorage(
                frameLocalPtr, destMemoryType,
                frameDevicePtr, frameByteSize);

            var frameInfo = new FrameInformation(
                bufferStorage, pitch, _info, yuvInfo);

            if (!_framesChannel.Writer.TryWrite(frameInfo))
            {
                _nv12BufferPool.Free(ref bufferStorage);
            }

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
            var frame = _framesChannel.Reader
                .ReadAsync(_cts.Token)
                .AsTask().Result;

            if (frame.IsFinalFrame)
            {
                _renderingCompleted.Set();
                return;
            }

            using var _lease = _nv12BufferPool.Lease(frame.Buffer);
            using var _ = _context.Push();

            _window.FrameArrived(frame);

            if (_args.WriteBitmap)
            {
                SaveAsBitmap(
                    frame,
                    Path.Combine(_args.BitmapPath, $"output-{_displayedFrames:D8}.bmp"));
            }

            ++_displayedFrames;
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

            frame.DecodeToHostRgba32((byte*)locked.Scan0);

            bitmap.UnlockBits(locked);
            bitmap.Save(filename, ImageFormat.Bmp);
        }

        private static void PrintInformation(
            string title,
            Dictionary<string, object> info)
        {
            Console.WriteLine(title);
            foreach (var (key, value) in info)
            {
                Console.WriteLine($"  + {key}: {value}");
            }

            Console.WriteLine();
        }

        public void Dispose()
        {
            _isDisposed = true;
            _context.Dispose();
            _decoder.Dispose();
            _contextLock.Dispose();
            _renderingCompleted.Dispose();
            _cts.Cancel();
            _cts.Dispose();
        }
    }

    internal class BufferStorage : IDisposable
    {
        public IntPtr Bytes { get; set; }
        public CuMemoryType MemoryType { get; set; }
        public CuDeviceMemory DeviceMemory { get; set; }
        public IntPtr Size { get; set; }

        public BufferStorage(
            IntPtr bytes,
            CuMemoryType memoryType,
            CuDeviceMemory deviceMemory,
            IntPtr size)
        {
            Bytes = bytes;
            Size = size;
            DeviceMemory = deviceMemory;
            MemoryType = memoryType;
        }

        private int _disposed = 0;

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _disposed, 1) != 0) return;

            switch (MemoryType)
            {
                case CuMemoryType.Device:
                    DeviceMemory.Dispose();
                    DeviceMemory = CuDeviceMemory.Empty;
                    break;
                case CuMemoryType.Host:
                    Marshal.FreeHGlobal(Bytes);
                    Bytes = IntPtr.Zero;
                    break;
            }
        }
    }

    internal class FrameInformation
    {
        public BufferStorage? Buffer { get; set; }
        public int Pitch { get; set; }
        public CuVideoDecodeCreateInfo Info { get; set; }
        public YuvInformation YuvInfo { get; set; }
        public bool IsFinalFrame { get; set; }

        public static FrameInformation FinalFrame => new FrameInformation(true);

        public FrameInformation(
            BufferStorage buffer,
            int pitch,
            CuVideoDecodeCreateInfo info,
            YuvInformation yuvInfo)
        {
            Buffer = buffer;
            Pitch = pitch;
            Info = info;
            YuvInfo = yuvInfo;
        }

        private FrameInformation(bool isFinalFrame)
        {
            IsFinalFrame = isFinalFrame;
        }

        public int GetRgba32Size()
        {
            return Info.Width * Info.Height * 4;
        }

        public unsafe void DecodeToHostRgba32(byte* destinationPtr)
        {
            var width = Info.Width;
            var height = Info.Height;
            var buffer = Buffer;
            const int rgbBpp = 4;
            var rgbSize = GetRgba32Size();

            switch (buffer.MemoryType)
            {
                case CuMemoryType.Host:
                    LibYuvSharp.LibYuv.NV12ToARGB(
                        (byte*)buffer.Bytes, YuvInfo.LumaPitch,
                        (byte*)buffer.Bytes + YuvInfo.ChromaOffset,
                        YuvInfo.ChromaPitch,
                        destinationPtr, width * rgbBpp, width, height);

                    break;
                case CuMemoryType.Device:
                    using (var destPtr = CuDeviceMemory.Allocate(rgbSize))
                    {
                        LibCudaLibrary.Nv12ToBGRA32(
                            buffer.DeviceMemory.Handle, width,
                            destPtr, width * rgbBpp, width, height);

                        destPtr.CopyToHost(destinationPtr, rgbSize);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(buffer.MemoryType));
            }
        }

        public void DecodeToDeviceRgba32(
            IntPtr destinationPtr,
            Size? resize = null)
        {
            var width = Info.Width;
            var height = Info.Height;
            var buffer = Buffer;
            const int rgbBpp = 4;

            var source = buffer.DeviceMemory;
            var hasNewSource = false;

            try
            {
                // This code path does not appear to properly resize the
                // window.
                if (resize.HasValue)
                {
                    var newWidth = resize.Value.Width;
                    var newHeight = resize.Value.Height;

                    // This buffer size allocation is incorrect but should be
                    // oversized enough to be fine.
                    source = CuDeviceMemory.Allocate(
                        newWidth * newHeight * rgbBpp);
                    hasNewSource = true;

                    LibCudaLibrary.ResizeNv12(
                        source, newWidth, newWidth, newHeight,
                        buffer.DeviceMemory, Pitch, width, height,
                        CuDevicePtr.Empty);

                    width = newWidth;
                    height = newHeight;
                }

                switch (buffer.MemoryType)
                {
                    case CuMemoryType.Device:
                        LibCudaLibrary.Nv12ToBGRA32(
                            source, width,
                            destinationPtr, width * rgbBpp, width, height);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(
                            nameof(buffer.MemoryType), buffer.MemoryType,
                            "Unsupported memory type.");
                }
            }
            finally
            {
                if (hasNewSource)
                {
                    source.Dispose();
                }
            }
        }
    }
}