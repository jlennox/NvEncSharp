using System;
using System.IO;
using System.Linq;
using System.Threading;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using static Lennox.NvEncSharp.LibNvEnc;

namespace Lennox.NvEncSharp.Sample.ScreenCapture;

internal class Program
{
    private bool _initialized = false;
    private NvEncoder _encoder;
    private Guid _codecGuid;
    private NvEncCreateBitstreamBuffer _bitstreamBuffer;
    private readonly object _writeMutex = new object();

    private const int _fps = 30;
    private const int _frameDuration = 1000 / _fps;

    // This program captures the full frames of a display using directX,
    // then uses the hardware NvEnc encoder on Nvidia GPUs to encode
    // H.264, HEVC or AV1 video directly from the GPU texture.
    // The output is written as containerless frames. Most software
    // does not support playback of containerless formats but ffplay can:
    // ffplay.exe -f h264 sample.264
    // ffplay.exe -f hevc sample.hevc
    // ffplay.exe -f obu sample.obu
    public static void Main(string[] args)
    {
        var program = new Program();
        program.Run(new ProgramArguments(args));
    }

    private void Run(ProgramArguments args)
    {
        _codecGuid = args.CodecGuid;
        using var duplicate = GetDisplayDuplicate(
            args.DisplayName, out var outputDescription);
        using var output = File.Open(args.OutputPath, FileMode.Create);

        Console.WriteLine($"Process: {(Environment.Is64BitProcess ? "64" : "32")} bits");
        Console.WriteLine($"Display: {outputDescription.DeviceName}");
        Console.WriteLine($"Output: {output.Name}");
        Console.WriteLine($"Codec: {(_codecGuid == NvEncCodecGuids.Av1 ? "AV1" : _codecGuid == NvEncCodecGuids.Hevc ? "HEVC" : "H.264")}");

        try
        {
            CaptureFrames(duplicate, output);
        }
        finally
        {
            if (_encoder.Handle != IntPtr.Zero)
            {
                try
                {
                    if (_bitstreamBuffer.BitstreamBuffer.Handle != IntPtr.Zero)
                        _encoder.DestroyBitstreamBuffer(_bitstreamBuffer.BitstreamBuffer);
                }
                finally
                {
                    _encoder.DestroyEncoder();
                }
            }
        }
    }

    private void CaptureFrames(OutputDuplication duplicate, Stream output)
    {
        while (true)
        {
            // Get the next screen image.
            duplicate.AcquireNextFrame(500,
                out var frameInfo, out var resourceOut);

            try
            {
                using (resourceOut)
                {
                    // If the frame has not changed, there's no reason to encode it.
                    if (frameInfo.LastPresentTime != 0)
                    {
                        using var desktopTexture = resourceOut.QueryInterface<Texture2D>();
                        EncodeFrame(desktopTexture, frameInfo.LastPresentTime, output);
                    }
                }
            }
            finally
            {
                duplicate.ReleaseFrame();
            }

            Thread.Sleep(_frameDuration);
        }

        // ReSharper disable once FunctionNeverReturns
    }

    private void EncodeFrame(Texture2D desktopTexture, long timestamp, Stream output)
    {
        var encoder = _initialized ? _encoder : CreateEncoder(desktopTexture);
        var desc = desktopTexture.Description;
        var reg = new NvEncRegisterResource
        {
            Version = NV_ENC_REGISTER_RESOURCE_VER,
            ResourceType = NvEncInputResourceType.Directx,
            BufferFormat = NvEncBufferFormat.Argb,
            BufferUsage = NvEncBufferUsage.NvEncInputImage,
            ResourceToRegister = desktopTexture.NativePointer,
            Width = (uint)desc.Width,
            Height = (uint)desc.Height,
            Pitch = 0
        };

        // Desktop duplication provides BGRA bytes (NVENC's little-endian ARGB).
        using var registration = encoder.RegisterResource(ref reg);
        var mapped = new NvEncMapInputResource
        {
            Version = NV_ENC_MAP_INPUT_RESOURCE_VER,
            RegisteredResource = reg.RegisteredResource
        };
        encoder.MapInputResource(ref mapped);

        try
        {
            var pic = new NvEncPicParams
            {
                Version = NV_ENC_PIC_PARAMS_VER,
                PictureStruct = NvEncPicStruct.Frame,
                InputBuffer = mapped.MappedResource,
                BufferFmt = mapped.MappedBufferFmt,
                InputWidth = (uint)desc.Width,
                InputHeight = (uint)desc.Height,
                OutputBitstream = _bitstreamBuffer.BitstreamBuffer,
                InputTimeStamp = (ulong)timestamp,
                InputDuration = _frameDuration
            };

            // Do the actual encoding. With this configuration this is done
            // sync (blocking).
            encoder.EncodePicture(ref pic);

            // The output is written to the bitstream, which is now copied
            // to the output file.
            using (var sm = encoder.LockBitstreamAndCreateStream(
                       ref _bitstreamBuffer))
            {
                lock (_writeMutex)
                {
                    sm.CopyTo(output);
                }
            }
        }
        finally
        {
            encoder.UnmapInputResource(mapped.MappedResource);
        }
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

        var dxgiOutput = dxgiAdapter.Outputs.Single(t => t.Description.DeviceName == foundDeviceName);

        using var output1 = dxgiOutput.QueryInterface<Output1>();

        description = output1.Description;

        return output1.DuplicateOutput(device);
    }

    private NvEncoder CreateEncoder(Texture2D texture)
    {
        if (_initialized) return _encoder;

        var desc = texture.Description;
        var encoder = OpenEncoderForDirectX(texture.Device.NativePointer);
        _encoder = encoder; // Retain ownership even if initialization fails.
        if (!encoder.IsValidGuid(_codecGuid))
        {
            throw new NotSupportedException("The selected codec is not supported by this GPU's encoder.");
        }

        var encoderConfig = encoder.GetEncodePresetConfigEx(_codecGuid, NvEncPresetGuids.P1).PresetCfg;
        if (_codecGuid == NvEncCodecGuids.Hevc)
        {
            encoderConfig.ProfileGuid = NvEncProfileGuids.HevcMain;
        }
        else if (_codecGuid == NvEncCodecGuids.Av1)
        {
            encoderConfig.ProfileGuid = NvEncProfileGuids.Av1Main;
            encoderConfig.EncodeCodecConfig.Av1Config.OutputAnnexBFormat = false;
        }
        // Each mapped texture must finish encoding before this iteration releases it.
        encoderConfig.FrameIntervalP = 1;
        encoderConfig.RcParams.EnableLookahead = false;
        encoderConfig.RcParams.AverageBitRate = 4 * (1 << 20); // 4 Mbit
        encoderConfig.RcParams.MaxBitRate = 8 * (1 << 20);
        encoderConfig.RcParams.RateControlMode = NvEncParamsRcMode.Vbr;

        unsafe
        {
            NvEncConfig* p = &encoderConfig;
            var initparams = new NvEncInitializeParams
            {
                Version = NV_ENC_INITIALIZE_PARAMS_VER,
                EncodeGuid = _codecGuid,
                EncodeHeight = (uint)desc.Height,
                EncodeWidth = (uint)desc.Width,
                MaxEncodeHeight = (uint)desc.Height,
                MaxEncodeWidth = (uint)desc.Width,
                DarHeight = (uint)desc.Height,
                DarWidth = (uint)desc.Width,
                FrameRateNum = _fps,
                FrameRateDen = 1,
                ReportSliceOffsets = false,
                EnableSubFrameWrite = false,
                PresetGuid = NvEncPresetGuids.P1,
                EnableEncodeAsync = 0,
                EnablePTD = 1,
                EnableWeightedPrediction = _codecGuid != NvEncCodecGuids.Av1,
                EncodeConfig = p,
                TuningInfo = NvEncTuningInfo.HighQuality,
            };

            encoder.InitializeEncoder(ref initparams);
        }

        _bitstreamBuffer = encoder.CreateBitstreamBuffer();

        _encoder = encoder;
        _initialized = true;
        return encoder;
    }
}