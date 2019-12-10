using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Lennox.NvEncSharp
{
    public enum LibNcEncInitializeStatus
    {
        Success,
        DllNotFound,
        UnsupportedVersion,
        Failure
    }

    public static class LibNvEnc
    {
        private const string _path64 = "nvEncodeAPI64.dll";
        private const string _path32 = "nvEncodeAPI.dll";

        // ReSharper disable InconsistentNaming
        // ReSharper disable UnusedMember.Global
        public const uint NVENCAPI_MAJOR_VERSION = 9;
        public const uint NVENCAPI_MINOR_VERSION = 1;

        public const uint NVENCAPI_VERSION = NVENCAPI_MAJOR_VERSION | (NVENCAPI_MINOR_VERSION << 24);

        public static readonly uint NV_ENC_CAPS_PARAM_VER = StructVersion(1);
        public static readonly uint NV_ENC_ENCODE_OUT_PARAMS_VER = StructVersion(1);
        public static readonly uint NV_ENC_CREATE_INPUT_BUFFER_VER = StructVersion(1);
        public static readonly uint NV_ENC_CREATE_BITSTREAM_BUFFER_VER = StructVersion(1);
        public static readonly uint NV_ENC_CREATE_MV_BUFFER_VER = StructVersion(1);
        public static readonly uint NV_ENC_RC_PARAMS_VER = StructVersion(1);
        public static readonly uint NV_ENC_CONFIG_VER = StructVersion(7, (uint)1 << 31);
        public static readonly uint NV_ENC_INITIALIZE_PARAMS_VER = StructVersion(5, (uint)1 << 31);
        public static readonly uint NV_ENC_RECONFIGURE_PARAMS_VER = StructVersion(1, (uint)1 << 31);
        public static readonly uint NV_ENC_PRESET_CONFIG_VER = StructVersion(4, (uint)1 << 31);
        public static readonly uint NV_ENC_PIC_PARAMS_MVC_VER = StructVersion(1);
        public static readonly uint NV_ENC_PIC_PARAMS_VER = StructVersion(4, (uint)1 << 31);
        public static readonly uint NV_ENC_MEONLY_PARAMS_VER = StructVersion(3);
        public static readonly uint NV_ENC_LOCK_BITSTREAM_VER = StructVersion(1);
        public static readonly uint NV_ENC_LOCK_INPUT_BUFFER_VER = StructVersion(1);
        public static readonly uint NV_ENC_MAP_INPUT_RESOURCE_VER = StructVersion(4);
        public static readonly uint NV_ENC_REGISTER_RESOURCE_VER = StructVersion(3);
        public static readonly uint NV_ENC_STAT_VER = StructVersion(1);
        public static readonly uint NV_ENC_SEQUENCE_PARAM_PAYLOAD_VER = StructVersion(1);
        public static readonly uint NV_ENC_EVENT_PARAMS_VER = StructVersion(1);
        public static readonly uint NV_ENC_OPEN_ENCODE_SESSION_EX_PARAMS_VER = StructVersion(1);
        public static readonly uint NV_ENCODE_API_FUNCTION_LIST_VER = StructVersion(2);
        // ReSharper restore UnusedMember.Global
        // ReSharper restore InconsistentNaming

        public static NvEncApiFunctionList FunctionList;

        private static bool _isInitialized = false;

        [DllImport(_path64, SetLastError = true, EntryPoint = "NvEncodeAPICreateInstance")]
        private static extern NvEncStatus NvEncodeAPICreateInstance64(ref NvEncApiFunctionList functionList);

        [DllImport(_path64, SetLastError = true, EntryPoint = "NvEncodeAPIGetMaxSupportedVersion")]
        private static extern NvEncStatus NvEncodeAPIGetMaxSupportedVersion64(out uint version);

        [DllImport(_path32, SetLastError = true, EntryPoint = "NvEncodeAPICreateInstance")]
        private static extern NvEncStatus NvEncodeAPICreateInstance32(ref NvEncApiFunctionList functionList);

        [DllImport(_path32, SetLastError = true, EntryPoint = "NvEncodeAPIGetMaxSupportedVersion")]
        private static extern NvEncStatus NvEncodeAPIGetMaxSupportedVersion32(out uint version);

        // ReSharper disable InconsistentNaming
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NvEncStatus NvEncodeAPICreateInstance(ref NvEncApiFunctionList functionList) =>
            Environment.Is64BitProcess
                ? NvEncodeAPICreateInstance64(ref functionList)
                : NvEncodeAPICreateInstance32(ref functionList);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NvEncStatus NvEncodeAPIGetMaxSupportedVersion(out uint version) =>
            Environment.Is64BitProcess
                ? NvEncodeAPIGetMaxSupportedVersion64(out version)
                : NvEncodeAPIGetMaxSupportedVersion32(out version);
        // ReSharper restore InconsistentNaming

        private static uint StructVersion(uint ver, uint and = 0)
        {
            // #define NVENCAPI_STRUCT_VERSION(ver) ((uint32_t)NVENCAPI_VERSION | ((ver)<<16) | (0x7 << 28))
            return NVENCAPI_VERSION | (ver << 16) | (0x7 << 28) | and;
        }

        public static LibNcEncInitializeStatus TryInitialize(out string failedDescription)
        {
            failedDescription = null;

            // Thread safety isn't an issue because there's no loss on
            // successful re-entrance.
            if (_isInitialized) return LibNcEncInitializeStatus.Success;

            uint version;
            NvEncStatus status;

            try
            {
                status = NvEncodeAPIGetMaxSupportedVersion(out version);
            }
            catch (DllNotFoundException e)
            {
                failedDescription = e.ToString();
                return LibNcEncInitializeStatus.DllNotFound;
            }
            catch (Exception e)
            {
                failedDescription = e.ToString();
                return LibNcEncInitializeStatus.Failure;
            }

            if (status != NvEncStatus.Success)
            {
                failedDescription = $"{nameof(NvEncodeAPIGetMaxSupportedVersion)} returned unexpected status, {status}";
                return LibNcEncInitializeStatus.Failure;
            }

            const uint currentVersion = (NVENCAPI_MAJOR_VERSION << 4) | NVENCAPI_MINOR_VERSION;

            if (currentVersion > version)
            {
                failedDescription = $"Installed NvEnc version is {version >> 4}.{version & 0xF}, version must be at least {NVENCAPI_MAJOR_VERSION}.{NVENCAPI_MINOR_VERSION}. Please upgrade the nvidia display drivers.";
                return LibNcEncInitializeStatus.UnsupportedVersion;
            }

            var functionList = new NvEncApiFunctionList
            {
                Version = NV_ENCODE_API_FUNCTION_LIST_VER
            };

            status = NvEncodeAPICreateInstance(ref functionList);

            if (status != NvEncStatus.Success)
            {
                failedDescription = $"{nameof(NvEncodeAPICreateInstance)} returned unexpected status, {status}";
                return LibNcEncInitializeStatus.Failure;
            }

            FunctionList = functionList;

            _isInitialized = true;

            return LibNcEncInitializeStatus.Success;
        }

        public static void Initialize()
        {
            if (_isInitialized) return;

            if (TryInitialize(out var description) !=
                LibNcEncInitializeStatus.Success)
            {
                throw new NotSupportedException(description);
            }
        }

        public static NvEncoder OpenEncoder(
            ref NvEncOpenEncodeSessionExParams sessionParams)
        {
            Initialize();

            CheckResult(default, FunctionList
                .OpenEncodeSessionEx(ref sessionParams, out var encoder));

            return encoder;
        }

        public static NvEncoder OpenEncoderForDirectX(IntPtr deviceHandle)
        {
            var ps = new NvEncOpenEncodeSessionExParams
            {
                Version = NV_ENC_OPEN_ENCODE_SESSION_EX_PARAMS_VER,
                ApiVersion = NVENCAPI_VERSION,
                Device = deviceHandle,
                DeviceType = NvEncDeviceType.Directx
            };

            return OpenEncoder(ref ps);
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
