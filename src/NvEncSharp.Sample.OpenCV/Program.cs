using System;
using System.Linq;
using System.Runtime.InteropServices;
using Alturos.Yolo;
using Alturos.Yolo.Model;
using Lennox.NvEncSharp.Sample.Library;

#nullable enable

namespace NvEncSharp.Sample.OpenCV
{
    internal sealed class Program : IDisposable
    {
        private readonly YoloWrapper _yolo;
        private readonly VideoDecoder _videoDecoder;
        private IntPtr _frameBuffer;
        private int _frameBufferSize;

        static void Main(string[] args)
        {
            using var program = new Program(args[0]);
        }

        public Program(string path)
        {
            var configurationDetector = new ConfigurationDetector();
            var config = configurationDetector.Detect();
            _yolo = new YoloWrapper(config);

            _videoDecoder = new VideoDecoder(new VideoDecoderOptions
            {
                InputPath = path,
                UseHostMemory = true
            });

            _videoDecoder.FrameArrived += FrameArrived;
            _videoDecoder.DecodeLoop();
        }

        private unsafe void FrameArrived(FrameInformation frame)
        {
            var neededSize = frame.Info.Width * frame.Info.Height * 4;
            if (_frameBufferSize < neededSize)
            {
                Marshal.FreeHGlobal(_frameBuffer);
                _frameBuffer = Marshal.AllocHGlobal(neededSize);
                _frameBufferSize = neededSize;
            }

            frame.DecodeToHostRgba32((byte*)_frameBuffer);
            var results = _yolo.Detect(_frameBuffer, neededSize);

            var items = results == null
                ? Array.Empty<YoloItem>()
                : results.ToArray();

            Console.WriteLine($"Found {items.Length} items in frame {frame.FrameNumber}.");

            foreach (var item in items)
            {
                Console.WriteLine($"   Type: {item.Type}");
            }
        }

        public void Dispose()
        {
            _yolo?.Dispose();
            _videoDecoder?.Dispose();
            Marshal.FreeHGlobal(_frameBuffer);
        }
    }
}
