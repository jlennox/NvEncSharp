using System.Reflection;
using System.Runtime.InteropServices;
using Lennox.NvEncSharp;

var abiOnly = args.Contains("--abi-only");
if (!abiOnly && (!OperatingSystem.IsLinux() || RuntimeInformation.ProcessArchitecture != Architecture.X64))
    throw new PlatformNotSupportedException("Run these checks on Linux x64.");

var assembly = typeof(NvEncoder).Assembly;
int count = 0;
foreach (var line in File.ReadLines(args[0]))
{
    if (line.StartsWith('#')) continue;
    var parts = line.Split(' ');
    var key = parts[0].Split('.');
    var expected = int.Parse(parts[1]);
    int actual;
    if (key.Length == 1)
    {
        var type = key[0] == "Guid" ? typeof(Guid) : assembly.GetType("Lennox.NvEncSharp." + key[0], true);
        actual = Marshal.SizeOf(type);
    }
    else if (key[0].StartsWith("Linux"))
    {
        var type = typeof(LibCuVideo).GetNestedType(key[0], BindingFlags.NonPublic);
        actual = key[1] == "size" ? Marshal.SizeOf(type) : Marshal.OffsetOf(type, key[1]).ToInt32();
    }
    else
    {
        var method = typeof(NvEncApiFunctionList).GetNestedType(key[0]).GetMethod("Invoke");
        var parameters = method.GetParameters();
        actual = key[1] switch
        {
            "count" => parameters.Length,
            "return" => NativeSize(method.ReturnType),
            _ => NativeSize(key.Length == 3
                ? parameters[int.Parse(key[1][3..])].ParameterType.GetElementType()
                : parameters[int.Parse(key[1][3..])].ParameterType)
        };
    }
    if (expected != actual) throw new Exception($"{parts[0]}: native={expected}, managed={actual}");
    count++;
}
if (abiOnly)
{
    Console.WriteLine($"Passed {count} native ABI checks.");
    return;
}

// The script supplies mock driver libraries. These calls exercise the real P/Invoke
// resolver and marshaling paths without an NVIDIA GPU or driver installation.
Check(LibCuda.Initialize(0) == CuResult.Success, "CUDA resolver");
Check(LibNvEnc.TryInitialize(out var error) == LibNcEncInitializeStatus.Success, "NVENC resolver: " + error);
var info = new CuVideoDecodeCreateInfo
{
    Width = 1920, Height = 1080, NumDecodeSurfaces = 8,
    CodecType = CuVideoCodec.H264, ChromaFormat = CuVideoChromaFormat.YUV420,
    CreationFlags = (CuVideoCreateFlags)1, BitDepthMinus8 = 2, IntraDecodeOnly = 1,
    MaxWidth = 3840, MaxHeight = 2160, TargetWidth = 1280, TargetHeight = 720,
    NumOutputSurfaces = 3, VideoLock = new CuVideoContextLock { Handle = (IntPtr)0x1234 }
};
Check(LibCuVideo.CreateDecoder(out var decoder, ref info) == CuResult.Success, "Linux decoder structure translation");
var proc = new CuVideoProcParams();
Check(LibCuVideo.MapVideoFrame(decoder, 7, out var ptr, out var pitch, ref proc) == CuResult.Success,
    "64-bit frame map");
Check(pitch == 2048, "frame pitch");
Check(LibCuVideo.UnmapVideoFrame(decoder, ptr) == CuResult.Success, "64-bit frame pointer preservation");
unsafe
{
    byte payload = 42;
    var packet = new CuVideoSourceDataPacket
    {
        Flags = (CuVideoPacketFlags)2, PayloadSize = 1, Payload = &payload, Timestamp = -1234567890123
    };
    Check(LibCuVideo.ParseVideoData(default, ref packet) == CuResult.Success, "Linux parser packet translation");
}
var pixel = Marshal.AllocHGlobal(4);
try
{
    LibCudaLibrary.Nv12ToBGRA32(IntPtr.Zero, 0, pixel, 4, 1, 1);
    Check(Marshal.ReadInt32(pixel) == 0x12345678, "CUDA helper resolver");
}
finally { Marshal.FreeHGlobal(pixel); }
Console.WriteLine($"Passed {count} native ABI checks and Linux library-loading/marshaling checks.");

static int NativeSize(Type type) => type.IsPointer || type.IsByRef ? IntPtr.Size
    : Marshal.SizeOf(type.IsEnum ? Enum.GetUnderlyingType(type) : type);
static void Check(bool success, string name)
{
    if (!success) throw new Exception(name);
}
