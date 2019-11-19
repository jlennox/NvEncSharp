using System;
using System.Runtime.InteropServices;

namespace Lennox.NvEncSharp
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)] public struct NvEncInputPtr { public IntPtr Handle; }
    [StructLayout(LayoutKind.Sequential, Pack = 1)] public struct NvEncOutputPtr { public IntPtr Handle; }
    [StructLayout(LayoutKind.Sequential, Pack = 1)] public struct NvEncRegisteredPtr { public IntPtr Handle; }
    [StructLayout(LayoutKind.Sequential, Pack = 1)] public struct NvEncCustreamPtr { public IntPtr Handle; }
    [StructLayout(LayoutKind.Sequential, Pack = 1)] public struct NvEncoder { public IntPtr Handle; }

    /// <summary>NV_ENCODE_API_FUNCTION_LIST</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncApiFunctionList
    {
        public uint Version;
        public uint Reserved;
        public delegate NvEncStatus OpenEncodeSessionFn(IntPtr device, uint deviceType, out NvEncoder encoder);
        public OpenEncodeSessionFn OpenEncodeSession;
        public delegate NvEncStatus GetEncodeGuidCountFn(NvEncoder encoder, ref uint encodeGuidCount);
        public GetEncodeGuidCountFn GetEncodeGuidCount;
        public delegate NvEncStatus GetEncodeProfileGuidCountFn(NvEncoder encoder, Guid encodeGuid, ref uint encodeProfileGuidCount);
        public GetEncodeProfileGuidCountFn GetEncodeProfileGuidCount;
        public delegate NvEncStatus GetEncodeProfileGuidsFn(NvEncoder encoder, Guid encodeGuid, Guid* profileGuids, uint guidArraySize, ref uint guidCount);
        public GetEncodeProfileGuidsFn GetEncodeProfileGuids;
        public delegate NvEncStatus GetEncodeGuidsFn(NvEncoder encoder, Guid* guids, uint guidArraySize, ref uint guidCount);
        public GetEncodeGuidsFn GetEncodeGuids;
        public delegate NvEncStatus GetInputFormatCountFn(NvEncoder encoder, Guid encodeGuid, ref uint inputFmtCount);
        public GetInputFormatCountFn GetInputFormatCount;
        public delegate NvEncStatus GetInputFormatsFn(NvEncoder encoder, Guid encodeGuid, ref NvEncBufferFormat inputFmts, uint inputFmtArraySize, ref uint inputFmtCount);
        public GetInputFormatsFn GetInputFormats;
        public delegate NvEncStatus GetEncodeCapsFn(NvEncoder encoder, Guid encodeGuid, ref NvEncCapsParam capsParam, ref int capsVal);
        public GetEncodeCapsFn GetEncodeCaps;
        public delegate NvEncStatus GetEncodePresetCountFn(NvEncoder encoder, Guid encodeGuid, ref uint encodePresetGuidCount);
        public GetEncodePresetCountFn GetEncodePresetCount;
        public delegate NvEncStatus GetEncodePresetGuidsFn(NvEncoder encoder, Guid encodeGuid, Guid* presetGuids, uint guidArraySize, ref uint encodePresetGuidCount);
        public GetEncodePresetGuidsFn GetEncodePresetGuids;
        public delegate NvEncStatus GetEncodePresetConfigFn(NvEncoder encoder, Guid encodeGuid, Guid presetGuid, ref NvEncPresetConfig presetConfig);
        public GetEncodePresetConfigFn GetEncodePresetConfig;
        public delegate NvEncStatus InitializeEncoderFn(NvEncoder encoder, ref NvEncInitializeParams createEncodeParams);
        public InitializeEncoderFn InitializeEncoder;
        public delegate NvEncStatus CreateInputBufferFn(NvEncoder encoder, ref NvEncCreateInputBuffer createInputBufferParams);
        public CreateInputBufferFn CreateInputBuffer;
        public delegate NvEncStatus DestroyInputBufferFn(NvEncoder encoder, NvEncInputPtr inputBuffer);
        public DestroyInputBufferFn DestroyInputBuffer;
        public delegate NvEncStatus CreateBitstreamBufferFn(NvEncoder encoder, ref NvEncCreateBitstreamBuffer createBitstreamBufferParams);
        public CreateBitstreamBufferFn CreateBitstreamBuffer;
        public delegate NvEncStatus DestroyBitstreamBufferFn(NvEncoder encoder, NvEncOutputPtr bitstreamBuffer);
        public DestroyBitstreamBufferFn DestroyBitstreamBuffer;
        public delegate NvEncStatus EncodePictureFn(NvEncoder encoder, ref NvEncPicParams encodePicParams);
        public EncodePictureFn EncodePicture;
        public delegate NvEncStatus LockBitstreamFn(NvEncoder encoder, ref NvEncLockBitstream lockBitstreamBufferParams);
        public LockBitstreamFn LockBitstream;
        public delegate NvEncStatus UnlockBitstreamFn(NvEncoder encoder, NvEncOutputPtr bitstreamBuffer);
        public UnlockBitstreamFn UnlockBitstream;
        public delegate NvEncStatus LockInputBufferFn(NvEncoder encoder, ref NvEncLockInputBuffer lockInputBufferParams);
        public LockInputBufferFn LockInputBuffer;
        public delegate NvEncStatus UnlockInputBufferFn(NvEncoder encoder, NvEncInputPtr inputBuffer);
        public UnlockInputBufferFn UnlockInputBuffer;
        public delegate NvEncStatus GetEncodeStatsFn(NvEncoder encoder, ref NvEncStat encodeStats);
        public GetEncodeStatsFn GetEncodeStats;
        public delegate NvEncStatus GetSequenceParamsFn(NvEncoder encoder, ref NvEncSequenceParamPayload sequenceParamPayload);
        public GetSequenceParamsFn GetSequenceParams;
        public delegate NvEncStatus RegisterAsyncEventFn(NvEncoder encoder, ref NvEncEventParams eventParams);
        public RegisterAsyncEventFn RegisterAsyncEvent;
        public delegate NvEncStatus UnregisterAsyncEventFn(NvEncoder encoder, ref NvEncEventParams eventParams);
        public UnregisterAsyncEventFn UnregisterAsyncEvent;
        public delegate NvEncStatus MapInputResourceFn(NvEncoder encoder, ref NvEncMapInputResource mapInputResParams);
        public MapInputResourceFn MapInputResource;
        public delegate NvEncStatus UnmapInputResourceFn(NvEncoder encoder, NvEncInputPtr mappedInputBuffer);
        public UnmapInputResourceFn UnmapInputResource;
        public delegate NvEncStatus DestroyEncoderFn(NvEncoder encoder);
        public DestroyEncoderFn DestroyEncoder;
        public delegate NvEncStatus InvalidateRefFramesFn(NvEncoder encoder, ulong invalidRefFrameTimeStamp);
        public InvalidateRefFramesFn InvalidateRefFrames;
        public delegate NvEncStatus OpenEncodeSessionExFn(ref NvEncOpenEncodeSessionExParams openSessionExParams, out NvEncoder encoder);
        public OpenEncodeSessionExFn OpenEncodeSessionEx;
        public delegate NvEncStatus RegisterResourceFn(NvEncoder encoder, ref NvEncRegisterResource registerResParams);
        public RegisterResourceFn RegisterResource;
        public delegate NvEncStatus UnregisterResourceFn(NvEncoder encoder, NvEncRegisteredPtr registeredRes);
        public UnregisterResourceFn UnregisterResource;
        public delegate NvEncStatus ReconfigureEncoderFn(NvEncoder encoder, ref NvEncReconfigureParams reInitEncodeParams);
        public ReconfigureEncoderFn ReconfigureEncoder;
        public IntPtr Reserved1;
        public delegate NvEncStatus CreateMvBufferFn(NvEncoder encoder, ref NvEncCreateMvBuffer createMvBufferParams);
        public CreateMvBufferFn CreateMvBuffer;
        public delegate NvEncStatus DestroyMvBufferFn(NvEncoder encoder, NvEncOutputPtr mvBuffer);
        public DestroyMvBufferFn DestroyMvBuffer;
        public delegate NvEncStatus RunMotionEstimationOnlyFn(NvEncoder encoder, ref NvEncMeonlyParams meOnlyParams);
        public RunMotionEstimationOnlyFn RunMotionEstimationOnly;
        public delegate IntPtr GetLastErrorFn(NvEncoder encoder);
        public GetLastErrorFn GetLastError;
        public delegate NvEncStatus SetIOCudaStreamsFn(NvEncoder encoder, NvEncCustreamPtr inputStream, NvEncCustreamPtr outputStream);
        public SetIOCudaStreamsFn SetIOCudaStreams;
        public fixed long Reserved2[279];
    }
}