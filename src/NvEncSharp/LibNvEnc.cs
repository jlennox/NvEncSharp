using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Lennox.NvEncSharp
{
    public static class LibNvEnc
    {
        private const string _path = "nvEncodeAPI64.dll";

        public const uint NVENCAPI_MAJOR_VERSION = 9;
        public const uint NVENCAPI_MINOR_VERSION = 1;

        public const uint NVENCAPI_VERSION = NVENCAPI_MAJOR_VERSION | (NVENCAPI_MINOR_VERSION << 24);

        public static uint NV_ENCODE_API_FUNCTION_LIST_VER = StructVersion(2);
        public static uint NV_ENC_OPEN_ENCODE_SESSION_EX_PARAMS_VER = StructVersion(1);
        public static uint NV_ENC_INITIALIZE_PARAMS_VER = StructVersion(5, (long)1 << 31);
        public static uint NV_ENC_CONFIG_VER = StructVersion(7, (long)1 << 31);
        public static uint NV_ENC_PRESET_CONFIG_VER = StructVersion(4, (long)1 << 31);


        public static NvEncApiFunctionList FunctionList => GetFunctionList();

        public static uint StructVersion(uint ver, long and = 0)
        {
            // #define NVENCAPI_STRUCT_VERSION(ver) ((uint32_t)NVENCAPI_VERSION | ((ver)<<16) | (0x7 << 28))
            return NVENCAPI_VERSION | (ver << 16) | (0x7 << 28) | (uint)and;
        }

        private static readonly Lazy<NvEncApiFunctionList> _functionList =
            new Lazy<NvEncApiFunctionList>(GetFunctionList);

        [DllImport(_path, SetLastError = true)]
        public static extern NvEncStatus NvEncodeAPICreateInstance(ref NvEncApiFunctionList functionList);

        [DllImport(_path, SetLastError = true)]
        public static extern NvEncStatus NvEncodeAPIGetMaxSupportedVersion(out uint version);

        public static bool CheckSupportedVersion()
        {
            NvEncodeAPIGetMaxSupportedVersion(out var supportedVersion);

            return true;
        }

        public static NvEncoder OpenEncoder(ref NvEncOpenEncodeSessionexParams sessionParams)
        {
            CheckResult(default, _functionList.Value.
                OpenEncodeSessionEx(ref sessionParams, out var encoder));

            return encoder;
        }

        public static NvEncoder OpenEncoderForDirectX(IntPtr deviceHandle)
        {
            var ps = new NvEncOpenEncodeSessionexParams
            {
                Version = NV_ENC_OPEN_ENCODE_SESSION_EX_PARAMS_VER,
                ApiVersion = NVENCAPI_VERSION,
                Device = deviceHandle,
                DeviceType = NvEncDeviceType.Directx
            };

            return OpenEncoder(ref ps);
        }

        public static NvEncApiFunctionList GetFunctionList()
        {
            var functionList = new NvEncApiFunctionList
            {
                Version = NV_ENCODE_API_FUNCTION_LIST_VER
            };

            var status = NvEncodeAPICreateInstance(ref functionList);
            return functionList;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CheckResult(
            NvEncoder encoder,
            NvEncStatus status,
            [CallerMemberName] string callerName = "")
        {
            if (status != NvEncStatus.Success)
            {
                throw new LibNvEncException(
                    callerName, encoder.GetLastError(), status);
            }
        }
    }

}
