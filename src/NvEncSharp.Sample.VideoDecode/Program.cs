using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using static Lennox.NvEncSharp.LibNvVideo;

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

        private bool _isDeviceFramePitched = false;
        private bool _useDeviceFrame = true;

        private int Run(ProgramArguments args)
        {
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

            _context = device.CreateContext();
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

            Console.WriteLine($"Sent {count} packets.");

            Marshal.FreeHGlobal(inputBufferPtr);

            return 0;
        }

        private int VideoDisplayCallback(
            byte* data, ref CuVideoParseDisplayInfo info)
        {
            using var _ = _context.Push();

            var stream = CuStream.Create();

            var processingParam = new CuVideoProcParams
            {
                ProgressiveFrame = info.ProgressiveFrame,
                SecondField = info.RepeatFirstField + 1,
                TopFieldFirst = info.TopFieldFirst,
                UnpairedField = info.RepeatFirstField < 0 ? 1 : 0,
                OutputStream = stream
            };

            var srcFrame = _decoder.MapVideoFrame(
                info.PictureIndex, ref processingParam,
                out var pitch);

            var status = _decoder.GetDecodeStatus(info.PictureIndex);

            {
                var frameSize = _info.GetFrameSize();

                var frame = CuDeviceMemory.Allocate(frameSize);
                var frameLocal = new byte[(int)frameSize];
                fixed (byte* frameLocalPtr = frameLocal)
                {
                    var bitWidth = _info.Width * _info.GetBitsPerPixel();

                    // Copy luma
                    var memcopy = new CudaMemcopy2D
                    {
                        SrcMemoryType = CuMemoryType.Device,
                        SrcDevice = srcFrame,
                        SrcPitch = (IntPtr)pitch,
                        DstMemoryType = CuMemoryType.Host,
                        DstDevice = new CuDevicePtr(frameLocalPtr),
                        DstHost = (IntPtr)frameLocalPtr,
                        DstPitch = (IntPtr)bitWidth,
                        WidthInBytes = (IntPtr)bitWidth,
                        Height = (IntPtr)_info.Height
                    };

                    var result = LibCuda.Memcpy2D(ref memcopy);
                    CheckResult(result);
                }
            }

            return 1;
        }

        private int DecodePictureCallback(
            byte* data, ref CuVideoPicParams param)
        {
            _decoder.DecodePicture(ref param);
            return 1;
        }

        private CuVideoDecodeCreateInfo _info;

        private int SequenceCallback(
            byte* data, ref CuVideoFormat format)
        {
            PrintInformation("CuVideoFormat", new Dictionary<string, object>
            {
                ["Codec"] = format.Codec,
                ["Bitrate"] = format.Bitrate,
                ["CodedWidth"] = format.CodedWidth,
                ["CodedHeight"] = format.CodedHeight,
                ["Framerate"] = format.FrameRateNumerator / format.FrameRateDenominator,
            });

            var caps = new CuVideoDecodeCaps
            {
                CodecType = format.Codec,
                ChromaFormat = format.ChromaFormat,
                BitDepthMinus8 = format.BitDepthLumaMinus8
            };

            using (_context.Push())
            {
                CheckResult(GetDecoderCaps(ref caps));
            }

            if (!caps.IsSupported)
            {
                Console.Error.WriteLine($"Codec {caps.CodecType} is not supported.");
                Environment.Exit(-1);
            }

            if (format.CodedWidth > caps.MaxWidth ||
                format.CodedHeight > caps.MaxHeight)
            {
                Console.Error.WriteLine($"Unsupported video dimentions. Requested: {format.CodedWidth}x{format.CodedHeight}. Supported max: {caps.MaxWidth}x{caps.MaxHeight}");
                Environment.Exit(-1);
            }

            Console.WriteLine($"CuVideoDecodeCaps:");
            Console.WriteLine($"  + MaxWidth: {caps.MaxWidth}");
            Console.WriteLine($"  + MaxHeight: {caps.MaxHeight}");

            if (!_decoder.IsEmpty)
            {
                using (_context.Push())
                {
                    _decoder.Reconfigure(ref format);
                }

                return 1;
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

            using (_context.Push())
            {
                _decoder = CuVideoDecoder.Create(ref _info);
            }

            return format.MinNumDecodeSurfaces;
        }

        public void Dispose()
        {
            _context.Dispose();
            _decoder.Dispose();
            _contextLock.Dispose();
        }
    }
}
