using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Lennox.NvEncSharp.Sample.Library;

#nullable enable

namespace Lennox.NvEncSharp.Sample.VideoDecode
{
    internal unsafe class Program
    {
        public static void Main(string[] args)
        {
            var program = new Program(new ProgramArguments(args));
            var result = program.Run();
            Environment.Exit(result);
        }

        private VideoDecoder _videoDecoder;
        private readonly ProgramArguments _args;
        private readonly Lazy<DisplayWindow> _window;

        public Program(ProgramArguments args)
        {
            _args = args;
            _window = new Lazy<DisplayWindow>(() => new DisplayWindow(
                _videoDecoder.CuContext,
                "Decoded video", 800, 600));
            _videoDecoder = new VideoDecoder(args);
        }

        private int Run()
        {
            _videoDecoder.FrameArrived += FrameArrived;

            return _videoDecoder.DecodeLoop() ? 0 : -1;
        }

        private void FrameArrived(FrameInformation frame)
        {
            _window.Value.FrameArrived(frame);

            if (_args.WriteBitmap)
            {
                SaveAsBitmap(
                    frame,
                    Path.Combine(_args.BitmapPath, $"output-{_videoDecoder.DisplayedFrames:D8}.bmp"));
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

            frame.DecodeToHostRgba32((byte*)locked.Scan0);

            bitmap.UnlockBits(locked);
            bitmap.Save(filename, ImageFormat.Bmp);
        }
    }
}