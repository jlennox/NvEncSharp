using System;
using System.IO;
using System.Linq;
using System.Threading;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using static Lennox.NvEncSharp.LibNvEnc;

namespace Lennox.NvEncSharp.Sample.ScreenCapture
{
    internal class Program
    {
        private bool _initialized = false;
        private NvEncoder _encoder;
        private NvEncCreateBitstreamBuffer _bitstreamBuffer;

        private const int _fps = 30;
        private const int _frameDuration = 1000 / _fps;

        // This program captures the full frames of a display using directX,
        // then uses the hardware NvEnc h264 encoder on Nvidia GPUs to encode
        // h264 video directly from the GPU texture.
        // The output is written as containerless h264 frames. Most software
        // does not support playback of containerless formats but ffplay can:
        // ffplay.exe -f h264 sample.264
        public static void Main(string[] args)
        {
            var program = new Program();
            program.Run(new ProgramArguments(args));
        }

        private unsafe void Run(ProgramArguments args)
        {
            using var duplicate = GetDisplayDuplicate(
                args.DisplayName, out var outputDescription);
            using var output = File.OpenWrite(args.OutputPath);

            Console.WriteLine($"Process: {(Environment.Is64BitProcess ? "64" : "32")} bits");
            Console.WriteLine($"Display: {outputDescription.DeviceName}");
            Console.WriteLine($"Output: {output.Name}");

            while (true)
            {
                // Get the next screen image.
                duplicate.AcquireNextFrame(500,
                    out var frameInfo, out var resourceOut);

                // If the frame has not changed, there's no reason to encode
                // a new frame.
                if (frameInfo.LastPresentTime == 0)
                {
                    duplicate.ReleaseFrame();
                    resourceOut.Dispose();
                    Thread.Sleep(_frameDuration);
                    continue;
                }

                var desktopTexture = resourceOut.QueryInterface<Texture2D>();
                var encoder = _initialized
                    ? _encoder
                    : CreateEncoder(desktopTexture);

                var desc = desktopTexture.Description;

                var reg = new NvEncRegisterResource
                {
                    Version = NV_ENC_REGISTER_RESOURCE_VER,
                    BufferFormat = NvEncBufferFormat.Abgr,
                    BufferUsage = NvEncBufferUsage.NvEncInputImage,
                    ResourceToRegister = desktopTexture.NativePointer,
                    Width = (uint)desc.Width,
                    Height = (uint)desc.Height,
                    Pitch = 0
                };

                // Registers the hardware texture surface as a resource for
                // NvEnc to use.
                _encoder.RegisterResource(ref reg);

                var pic = new NvEncPicParams
                {
                    Version = NV_ENC_PIC_PARAMS_VER,
                    PictureStruct = NvEncPicStruct.Frame,
                    InputBuffer = reg.AsInputPointer(),
                    BufferFmt = NvEncBufferFormat.Abgr,
                    InputWidth = (uint)desc.Width,
                    InputHeight = (uint)desc.Height,
                    OutputBitstream = _bitstreamBuffer.BitstreamBuffer,
                    InputTimeStamp = (ulong)frameInfo.LastPresentTime,
                    InputDuration = _frameDuration
                };

                // Do the actual encoding. With this configuration this is done
                // sync (blocking).
                encoder.EncodePicture(ref pic);

                // The output is written to the bitstream, which is now copied
                // to the output file.
                var lockstruct = encoder.LockBitstream(_bitstreamBuffer);
                using (var sm = new UnmanagedMemoryStream(
                    (byte*)lockstruct.BitstreamBufferPtr,
                    lockstruct.BitstreamSizeInBytes))
                {
                    sm.CopyTo(output);
                }

                // Cleanup.
                encoder.UnlockBitstream(_bitstreamBuffer.BitstreamBuffer);
                encoder.UnregisterResource(reg.RegisteredResource);

                desktopTexture.Dispose();
                duplicate.ReleaseFrame();
                resourceOut.Dispose();

                Thread.Sleep(_frameDuration);
            }

            // ReSharper disable once FunctionNeverReturns
        }

        private static OutputDuplication GetDisplayDuplicate(
            string displayName, out OutputDescription description)
        {
            // This much simpler code will grab an arbitrary display but
            // works in most single output systems. It's useful for enabling
            // debug on a device.
            /*using var device = new SharpDX.Direct3D11.Device(
                DriverType.Hardware, DeviceCreationFlags.Debug);
            using var dxgiDevice = device.QueryInterface<SharpDX.DXGI.Device>();
            using var dxgiAdapter = dxgiDevice.GetParent<Adapter>();
            using var dxgiOutput = dxgiAdapter.GetOutput(0);
            using var output1 = dxgiOutput.QueryInterface<Output1>();
            return output1.DuplicateOutput(device);*/

            using var factory = new Factory4();
            var availableAdaptors = factory.Adapters;

            var output = availableAdaptors
                .SelectMany(t => t.Outputs)
                .FirstOrDefault(t => displayName == null
                    ? t.Description.IsAttachedToDesktop == true
                    : t.Description.DeviceName == displayName);

            if (output == null)
            {
                throw new DriveNotFoundException(displayName);
            }

            var foundDeviceName = output.Description.DeviceName;
            using var dxgiAdapter = output.GetParent<Adapter>();
            using var device = new SharpDX.Direct3D11.Device(dxgiAdapter);

            var dxgiOutput = dxgiAdapter.Outputs
               .Single(t => t.Description.DeviceName == foundDeviceName);

            using var output1 = dxgiOutput.QueryInterface<Output1>();

            description = output1.Description;

            return output1.DuplicateOutput(device);
        }

        private NvEncoder CreateEncoder(Texture2D texture)
        {
            if (_initialized) return _encoder;

            var desc = texture.Description;
            var encoder = OpenEncoderForDirectX(texture.Device.NativePointer);

            var initparams = new NvEncInitializeParams
            {
                Version = NV_ENC_INITIALIZE_PARAMS_VER,
                EncodeGuid = NvEncCodecGuids.H264,
                EncodeHeight = (uint)desc.Height,
                EncodeWidth = (uint)desc.Width,
                MaxEncodeHeight = (uint)desc.Height,
                MaxEncodeWidth = (uint)desc.Width,
                DarHeight = (uint)desc.Height,
                DarWidth = (uint)desc.Width,
                FrameRateNum = _frameDuration,
                FrameRateDen = 1,
                ReportSliceOffsets = false,
                EnableSubFrameWrite = false,
                PresetGuid = NvEncPresetGuids.LowLatencyDefault,
                EnableEncodeAsync = 0
            };

            encoder.InitializeEncoder(ref initparams);

            _bitstreamBuffer = encoder.CreateBitstreamBuffer();

            _encoder = encoder;
            _initialized = true;
            return encoder;
        }
    }
}
