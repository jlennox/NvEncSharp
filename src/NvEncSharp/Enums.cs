namespace Lennox.NvEncSharp
{
    /// <summary>NV_ENC_PARAMS_FRAME_FIELD_MODE</summary>
    public enum NvEncParamsFrameFieldMode
    {
        /// <summary>NV_ENC_PARAMS_FRAME_FIELD_MODE_FRAME: Frame mode</summary>
        Frame = 0x01,
        /// <summary>NV_ENC_PARAMS_FRAME_FIELD_MODE_FIELD: Field mode</summary>
        Field = 0x02,
        /// <summary>NV_ENC_PARAMS_FRAME_FIELD_MODE_MBAFF: MB adaptive frame/field</summary>
        Mbaff = 0x03,
    }

    /// <summary>NV_ENC_PARAMS_RC_MODE</summary>
    public enum NvEncParamsRcMode
    {
        /// <summary>NV_ENC_PARAMS_RC_CONSTQP: Constant QP mode</summary>
        Constqp = 0x0,
        /// <summary>NV_ENC_PARAMS_RC_VBR: Variable bitrate mode</summary>
        Vbr = 0x1,
        /// <summary>NV_ENC_PARAMS_RC_CBR: Constant bitrate mode</summary>
        Cbr = 0x2,
        /// <summary>NV_ENC_PARAMS_RC_CBR_LOWDELAY_HQ: low-delay CBR, high quality</summary>
        CbrLowdelayHq = 0x8,
        /// <summary>NV_ENC_PARAMS_RC_CBR_HQ: CBR, high quality (slower)</summary>
        CbrHq = 0x10,
        /// <summary>NV_ENC_PARAMS_RC_VBR_HQ: VBR, high quality (slower)</summary>
        VbrHq = 0x20,
    }

    /// <summary>NV_ENC_EMPHASIS_MAP_LEVEL</summary>
    public enum NvEncEmphasisMapLevel
    {
        /// <summary>NV_ENC_EMPHASIS_MAP_LEVEL_0: Emphasis Map Level 0, for zero Delta QP value</summary>
        Level0 = 0x0,
        /// <summary>NV_ENC_EMPHASIS_MAP_LEVEL_1: Emphasis Map Level 1, for very low Delta QP value</summary>
        Level1 = 0x1,
        /// <summary>NV_ENC_EMPHASIS_MAP_LEVEL_2: Emphasis Map Level 2, for low Delta QP value</summary>
        Level2 = 0x2,
        /// <summary>NV_ENC_EMPHASIS_MAP_LEVEL_3: Emphasis Map Level 3, for medium Delta QP value</summary>
        Level3 = 0x3,
        /// <summary>NV_ENC_EMPHASIS_MAP_LEVEL_4: Emphasis Map Level 4, for high Delta QP value</summary>
        Level4 = 0x4,
        /// <summary>NV_ENC_EMPHASIS_MAP_LEVEL_5: Emphasis Map Level 5, for very high Delta QP value</summary>
        Level5 = 0x5,
    }

    /// <summary>NV_ENC_QP_MAP_MODE</summary>
    public enum NvEncQpMapMode
    {
        /// <summary>NV_ENC_QP_MAP_DISABLED: Value in NV_ENC_PIC_PARAMS::qpDeltaMap have no effect.</summary>
        Disabled = 0x0,
        /// <summary>NV_ENC_QP_MAP_EMPHASIS: Value in NV_ENC_PIC_PARAMS::qpDeltaMap will be treated as Empasis level. Currently this is only supported for H264</summary>
        Emphasis = 0x1,
        /// <summary>NV_ENC_QP_MAP_DELTA: Value in NV_ENC_PIC_PARAMS::qpDeltaMap will be treated as QP delta map.</summary>
        Delta = 0x2,
        /// <summary>NV_ENC_QP_MAP: Currently This is not supported. Value in NV_ENC_PIC_PARAMS::qpDeltaMap will be treated as QP value.</summary>
        QpMap = 0x3,
    }

    /// <summary>NV_ENC_PIC_STRUCT</summary>
    public enum NvEncPicStruct
    {
        /// <summary>NV_ENC_PIC_STRUCT_FRAME: Progressive frame</summary>
        Frame = 0x01,
        /// <summary>NV_ENC_PIC_STRUCT_FIELD_TOP_BOTTOM: Field encoding top field first</summary>
        FieldTopBottom = 0x02,
        /// <summary>NV_ENC_PIC_STRUCT_FIELD_BOTTOM_TOP: Field encoding bottom field first</summary>
        FieldBottomTop = 0x03,
    }

    /// <summary>NV_ENC_PIC_TYPE</summary>
    public enum NvEncPicType
    {
        /// <summary>NV_ENC_PIC_TYPE_P: Forward predicted</summary>
        P = 0x0,
        /// <summary>NV_ENC_PIC_TYPE_B: Bi-directionally predicted picture</summary>
        B = 0x01,
        /// <summary>NV_ENC_PIC_TYPE_I: Intra predicted picture</summary>
        I = 0x02,
        /// <summary>NV_ENC_PIC_TYPE_IDR: IDR picture</summary>
        Idr = 0x03,
        /// <summary>NV_ENC_PIC_TYPE_BI: Bi-directionally predicted with only Intra MBs</summary>
        Bi = 0x04,
        /// <summary>NV_ENC_PIC_TYPE_SKIPPED: Picture is skipped</summary>
        Skipped = 0x05,
        /// <summary>NV_ENC_PIC_TYPE_INTRA_REFRESH: First picture in intra refresh cycle</summary>
        IntraRefresh = 0x06,
        /// <summary>NV_ENC_PIC_TYPE_NONREF_P: Non reference P picture</summary>
        NonrefP = 0x07,
        /// <summary>NV_ENC_PIC_TYPE_UNKNOWN: Picture type unknown</summary>
        Unknown = 0xFF,
    }

    /// <summary>NV_ENC_MV_PRECISION</summary>
    public enum NvEncMvPrecision
    {
        /// <summary>NV_ENC_MV_PRECISION_DEFAULT: Driver selects QuarterPel motion vector precision by default</summary>
        Default = 0x0,
        /// <summary>NV_ENC_MV_PRECISION_FULL_PEL: FullPel motion vector precision</summary>
        FullPel = 0x01,
        /// <summary>NV_ENC_MV_PRECISION_HALF_PEL: HalfPel motion vector precision</summary>
        HalfPel = 0x02,
        /// <summary>NV_ENC_MV_PRECISION_QUARTER_PEL: QuarterPel motion vector precision</summary>
        QuarterPel = 0x03,
    }

    /// <summary>NV_ENC_BUFFER_FORMAT</summary>
    public enum NvEncBufferFormat
    {
        /// <summary>NV_ENC_BUFFER_FORMAT_UNDEFINED: Undefined buffer format</summary>
        Undefined = 0x00000000,
        /// <summary>NV_ENC_BUFFER_FORMAT_NV12: Semi-Planar YUV [Y plane followed by interleaved UV plane]</summary>
        Nv12 = 0x00000001,
        /// <summary>NV_ENC_BUFFER_FORMAT_YV12: Planar YUV [Y plane followed by V and U planes]</summary>
        Yv12 = 0x00000010,
        /// <summary>NV_ENC_BUFFER_FORMAT_IYUV: Planar YUV [Y plane followed by U and V planes]</summary>
        Iyuv = 0x00000100,
        /// <summary>NV_ENC_BUFFER_FORMAT_YUV444: Planar YUV [Y plane followed by U and V planes]</summary>
        Yuv444 = 0x00001000,
        /// <summary>NV_ENC_BUFFER_FORMAT_YUV420_10BIT: 10 bit Semi-Planar YUV [Y plane followed by interleaved UV plane]. Each pixel of size 2 bytes. Most Significant 10 bits contain pixel data.</summary>
        Yuv42010bit = 0x00010000,
        /// <summary>NV_ENC_BUFFER_FORMAT_YUV444_10BIT: 10 bit Planar YUV444 [Y plane followed by U and V planes]. Each pixel of size 2 bytes. Most Significant 10 bits contain pixel data.</summary>
        Yuv44410bit = 0x00100000,
        /// <summary>NV_ENC_BUFFER_FORMAT_ARGB: 8 bit Packed A8R8G8B8. This is a word-ordered format
        ///  where a pixel is represented by a 32-bit word with B
        ///  in the lowest 8 bits, G in the next 8 bits, R in the
        ///  8 bits after that and A in the highest 8 bits.</summary>
        Argb = 0x01000000,
        /// <summary>NV_ENC_BUFFER_FORMAT_ARGB10: 10 bit Packed A2R10G10B10. This is a word-ordered format
        ///  where a pixel is represented by a 32-bit word with B
        ///  in the lowest 10 bits, G in the next 10 bits, R in the
        ///  10 bits after that and A in the highest 2 bits.</summary>
        Argb10 = 0x02000000,
        /// <summary>NV_ENC_BUFFER_FORMAT_AYUV: 8 bit Packed A8Y8U8V8. This is a word-ordered format
        ///  where a pixel is represented by a 32-bit word with V
        ///  in the lowest 8 bits, U in the next 8 bits, Y in the
        ///  8 bits after that and A in the highest 8 bits.</summary>
        Ayuv = 0x04000000,
        /// <summary>NV_ENC_BUFFER_FORMAT_ABGR: 8 bit Packed A8B8G8R8. This is a word-ordered format
        ///  where a pixel is represented by a 32-bit word with R
        ///  in the lowest 8 bits, G in the next 8 bits, B in the
        ///  8 bits after that and A in the highest 8 bits.</summary>
        Abgr = 0x10000000,
        /// <summary>NV_ENC_BUFFER_FORMAT_ABGR10: 10 bit Packed A2B10G10R10. This is a word-ordered format
        ///  where a pixel is represented by a 32-bit word with R
        ///  in the lowest 10 bits, G in the next 10 bits, B in the
        ///  10 bits after that and A in the highest 2 bits.</summary>
        Abgr10 = 0x20000000,
        /// <summary>NV_ENC_BUFFER_FORMAT_U8: Buffer format representing one-dimensional buffer.
        ///  This format should be used only when registering the
        ///  resource as output buffer, which will be used to write
        ///  the encoded bit stream or H.264 ME only mode output.</summary>
        U8 = 0x40000000,
    }

    /// <summary>NV_ENC_LEVEL</summary>
    public enum NvEncLevel
    {
        /// <summary>NV_ENC_LEVEL_AUTOSELECT</summary>
        Autoselect = 0,
        /// <summary>NV_ENC_LEVEL_H264_1</summary>
        H2641 = 10,
        /// <summary>NV_ENC_LEVEL_H264_1b</summary>
        H2641b = 9,
        /// <summary>NV_ENC_LEVEL_H264_11</summary>
        H26411 = 11,
        /// <summary>NV_ENC_LEVEL_H264_12</summary>
        H26412 = 12,
        /// <summary>NV_ENC_LEVEL_H264_13</summary>
        H26413 = 13,
        /// <summary>NV_ENC_LEVEL_H264_2</summary>
        H2642 = 20,
        /// <summary>NV_ENC_LEVEL_H264_21</summary>
        H26421 = 21,
        /// <summary>NV_ENC_LEVEL_H264_22</summary>
        H26422 = 22,
        /// <summary>NV_ENC_LEVEL_H264_3</summary>
        H2643 = 30,
        /// <summary>NV_ENC_LEVEL_H264_31</summary>
        H26431 = 31,
        /// <summary>NV_ENC_LEVEL_H264_32</summary>
        H26432 = 32,
        /// <summary>NV_ENC_LEVEL_H264_4</summary>
        H2644 = 40,
        /// <summary>NV_ENC_LEVEL_H264_41</summary>
        H26441 = 41,
        /// <summary>NV_ENC_LEVEL_H264_42</summary>
        H26442 = 42,
        /// <summary>NV_ENC_LEVEL_H264_5</summary>
        H2645 = 50,
        /// <summary>NV_ENC_LEVEL_H264_51</summary>
        H26451 = 51,
        /// <summary>NV_ENC_LEVEL_H264_52</summary>
        H26452 = 52,
        /// <summary>NV_ENC_LEVEL_HEVC_1</summary>
        Hevc1 = 30,
        /// <summary>NV_ENC_LEVEL_HEVC_2</summary>
        Hevc2 = 60,
        /// <summary>NV_ENC_LEVEL_HEVC_21</summary>
        Hevc21 = 63,
        /// <summary>NV_ENC_LEVEL_HEVC_3</summary>
        Hevc3 = 90,
        /// <summary>NV_ENC_LEVEL_HEVC_31</summary>
        Hevc31 = 93,
        /// <summary>NV_ENC_LEVEL_HEVC_4</summary>
        Hevc4 = 120,
        /// <summary>NV_ENC_LEVEL_HEVC_41</summary>
        Hevc41 = 123,
        /// <summary>NV_ENC_LEVEL_HEVC_5</summary>
        Hevc5 = 150,
        /// <summary>NV_ENC_LEVEL_HEVC_51</summary>
        Hevc51 = 153,
        /// <summary>NV_ENC_LEVEL_HEVC_52</summary>
        Hevc52 = 156,
        /// <summary>NV_ENC_LEVEL_HEVC_6</summary>
        Hevc6 = 180,
        /// <summary>NV_ENC_LEVEL_HEVC_61</summary>
        Hevc61 = 183,
        /// <summary>NV_ENC_LEVEL_HEVC_62</summary>
        Hevc62 = 186,
        /// <summary>NV_ENC_TIER_HEVC_MAIN</summary>
        TierHevcMain = 0,
        /// <summary>NV_ENC_TIER_HEVC_HIGH</summary>
        TierHevcHigh = 1,
    }

    /// <summary>NVENCSTATUS</summary>
    public enum NvEncStatus
    {
        /// <summary>NV_ENC_SUCCESS:  This indicates that API call returned with no errors.</summary>
        Success,
        /// <summary>NV_ENC_ERR_NO_ENCODE_DEVICE:  This indicates that no encode capable devices were detected.</summary>
        NoEncodeDevice,
        /// <summary>NV_ENC_ERR_UNSUPPORTED_DEVICE:  This indicates that devices pass by the client is not supported.</summary>
        UnsupportedDevice,
        /// <summary>NV_ENC_ERR_INVALID_ENCODERDEVICE:  This indicates that the encoder device supplied by the client is not
        ///  * valid.</summary>
        InvalidEncoderdevice,
        /// <summary>NV_ENC_ERR_INVALID_DEVICE:  This indicates that device passed to the API call is invalid.</summary>
        InvalidDevice,
        /// <summary>NV_ENC_ERR_DEVICE_NOT_EXIST:  This indicates that device passed to the API call is no longer available and
        ///  * needs to be reinitialized. The clients need to destroy the current encoder
        ///  * session by freeing the allocated input output buffers and destroying the device
        ///  * and create a new encoding session.</summary>
        DeviceNotExist,
        /// <summary>NV_ENC_ERR_INVALID_PTR:  This indicates that one or more of the pointers passed to the API call
        ///  * is invalid.</summary>
        InvalidPtr,
        /// <summary>NV_ENC_ERR_INVALID_EVENT:  This indicates that completion event passed in ::NvEncEncodePicture() call
        ///  * is invalid.</summary>
        InvalidEvent,
        /// <summary>NV_ENC_ERR_INVALID_PARAM:  This indicates that one or more of the parameter passed to the API call
        ///  * is invalid.</summary>
        InvalidParam,
        /// <summary>NV_ENC_ERR_INVALID_CALL:  This indicates that an API call was made in wrong sequence/order.</summary>
        InvalidCall,
        /// <summary>NV_ENC_ERR_OUT_OF_MEMORY:  This indicates that the API call failed because it was unable to allocate
        ///  * enough memory to perform the requested operation.</summary>
        OutOfMemory,
        /// <summary>NV_ENC_ERR_ENCODER_NOT_INITIALIZED:  This indicates that the encoder has not been initialized with
        ///  * ::NvEncInitializeEncoder() or that initialization has failed.
        ///  * The client cannot allocate input or output buffers or do any encoding
        ///  * related operation before successfully initializing the encoder.</summary>
        EncoderNotInitialized,
        /// <summary>NV_ENC_ERR_UNSUPPORTED_PARAM:  This indicates that an unsupported parameter was passed by the client.</summary>
        UnsupportedParam,
        /// <summary>NV_ENC_ERR_LOCK_BUSY:  This indicates that the ::NvEncLockBitstream() failed to lock the output
        ///  * buffer. This happens when the client makes a non blocking lock call to
        ///  * access the output bitstream by passing NV_ENC_LOCK_BITSTREAM::doNotWait flag.
        ///  * This is not a fatal error and client should retry the same operation after
        ///  * few milliseconds.</summary>
        LockBusy,
        /// <summary>NV_ENC_ERR_NOT_ENOUGH_BUFFER:  This indicates that the size of the user buffer passed by the client is
        ///  * insufficient for the requested operation.</summary>
        NotEnoughBuffer,
        /// <summary>NV_ENC_ERR_INVALID_VERSION:  This indicates that an invalid struct version was used by the client.</summary>
        InvalidVersion,
        /// <summary>NV_ENC_ERR_MAP_FAILED:  This indicates that ::NvEncMapInputResource() API failed to map the client
        ///  * provided input resource.</summary>
        MapFailed,
        /// <summary>NV_ENC_ERR_NEED_MORE_INPUT:  This indicates encode driver requires more input buffers to produce an output
        ///  * bitstream. If this error is returned from ::NvEncEncodePicture() API, this
        ///  * is not a fatal error. If the client is encoding with B frames then,
        ///  * ::NvEncEncodePicture() API might be buffering the input frame for re-ordering.
        ///  *
        ///  * A client operating in synchronous mode cannot call ::NvEncLockBitstream()
        ///  * API on the output bitstream buffer if ::NvEncEncodePicture() returned the
        ///  * ::NV_ENC_ERR_NEED_MORE_INPUT error code.
        ///  * The client must continue providing input frames until encode driver returns
        ///  * ::NV_ENC_SUCCESS. After receiving ::NV_ENC_SUCCESS status the client can call
        ///  * ::NvEncLockBitstream() API on the output buffers in the same order in which
        ///  * it has called ::NvEncEncodePicture().</summary>
        NeedMoreInput,
        /// <summary>NV_ENC_ERR_ENCODER_BUSY:  This indicates that the HW encoder is busy encoding and is unable to encode
        ///  * the input. The client should call ::NvEncEncodePicture() again after few
        ///  * milliseconds.</summary>
        EncoderBusy,
        /// <summary>NV_ENC_ERR_EVENT_NOT_REGISTERD:  This indicates that the completion event passed in ::NvEncEncodePicture()
        ///  * API has not been registered with encoder driver using ::NvEncRegisterAsyncEvent().</summary>
        EventNotRegisterd,
        /// <summary>NV_ENC_ERR_GENERIC:  This indicates that an unknown internal error has occurred.</summary>
        Generic,
        /// <summary>NV_ENC_ERR_INCOMPATIBLE_CLIENT_KEY:  This indicates that the client is attempting to use a feature
        ///  * that is not available for the license type for the current system.</summary>
        IncompatibleClientKey,
        /// <summary>NV_ENC_ERR_UNIMPLEMENTED:  This indicates that the client is attempting to use a feature
        ///  * that is not implemented for the current version.</summary>
        Unimplemented,
        /// <summary>NV_ENC_ERR_RESOURCE_REGISTER_FAILED:  This indicates that the ::NvEncRegisterResource API failed to register the resource.</summary>
        ResourceRegisterFailed,
        /// <summary>NV_ENC_ERR_RESOURCE_NOT_REGISTERED:  This indicates that the client is attempting to unregister a resource
        ///  * that has not been successfully registered.</summary>
        ResourceNotRegistered,
        /// <summary>NV_ENC_ERR_RESOURCE_NOT_MAPPED:  This indicates that the client is attempting to unmap a resource
        ///  * that has not been successfully mapped.</summary>
        ResourceNotMapped,
    }

    /// <summary>NV_ENC_PIC_FLAGS</summary>
    public enum NvEncPicFlags
    {
        /// <summary>NV_ENC_PIC_FLAG_FORCEINTRA: Encode the current picture as an Intra picture</summary>
        FlagForceintra = 0x1,
        /// <summary>NV_ENC_PIC_FLAG_FORCEIDR: Encode the current picture as an IDR picture.
        ///  This flag is only valid when Picture type decision is taken by the Encoder
        ///  [_NV_ENC_INITIALIZE_PARAMS::enablePTD == 1].</summary>
        FlagForceidr = 0x2,
        /// <summary>NV_ENC_PIC_FLAG_OUTPUT_SPSPPS: Write the sequence and picture header in encoded bitstream of the current picture</summary>
        FlagOutputSpspps = 0x4,
        /// <summary>NV_ENC_PIC_FLAG_EOS: Indicates end of the input stream</summary>
        FlagEos = 0x8,
    }

    /// <summary>NV_ENC_MEMORY_HEAP</summary>
    public enum NvEncMemoryHeap
    {
        /// <summary>NV_ENC_MEMORY_HEAP_AUTOSELECT: Memory heap to be decided by the encoder driver based on the usage</summary>
        Autoselect = 0,
        /// <summary>NV_ENC_MEMORY_HEAP_VID: Memory heap is in local video memory</summary>
        Vid = 1,
        /// <summary>NV_ENC_MEMORY_HEAP_SYSMEM_CACHED: Memory heap is in cached system memory</summary>
        SysmemCached = 2,
        /// <summary>NV_ENC_MEMORY_HEAP_SYSMEM_UNCACHED: Memory heap is in uncached system memory</summary>
        SysmemUncached = 3,
    }

    /// <summary>NV_ENC_BFRAME_REF_MODE</summary>
    public enum NvEncBframeRefMode
    {
        /// <summary>NV_ENC_BFRAME_REF_MODE_DISABLED: B frame is not used for reference</summary>
        Disabled = 0x0,
        /// <summary>NV_ENC_BFRAME_REF_MODE_EACH: Each B-frame will be used for reference. currently not supported for H.264</summary>
        Each = 0x1,
        /// <summary>NV_ENC_BFRAME_REF_MODE_MIDDLE: Only(Number of B-frame)/2 th B-frame will be used for reference</summary>
        Middle = 0x2,
    }

    /// <summary>NV_ENC_H264_ENTROPY_CODING_MODE</summary>
    public enum NvEncH264EntropyCodingMode
    {
        /// <summary>NV_ENC_H264_ENTROPY_CODING_MODE_AUTOSELECT: Entropy coding mode is auto selected by the encoder driver</summary>
        Autoselect = 0x0,
        /// <summary>NV_ENC_H264_ENTROPY_CODING_MODE_CABAC: Entropy coding mode is CABAC</summary>
        Cabac = 0x1,
        /// <summary>NV_ENC_H264_ENTROPY_CODING_MODE_CAVLC: Entropy coding mode is CAVLC</summary>
        Cavlc = 0x2,
    }

    /// <summary>NV_ENC_H264_BDIRECT_MODE</summary>
    public enum NvEncH264BdirectMode
    {
        /// <summary>NV_ENC_H264_BDIRECT_MODE_AUTOSELECT: BDirect mode is auto selected by the encoder driver</summary>
        Autoselect = 0x0,
        /// <summary>NV_ENC_H264_BDIRECT_MODE_DISABLE: Disable BDirect mode</summary>
        Disable = 0x1,
        /// <summary>NV_ENC_H264_BDIRECT_MODE_TEMPORAL: Temporal BDirect mode</summary>
        Temporal = 0x2,
        /// <summary>NV_ENC_H264_BDIRECT_MODE_SPATIAL: Spatial BDirect mode</summary>
        Spatial = 0x3,
    }

    /// <summary>NV_ENC_H264_FMO_MODE</summary>
    public enum NvEncH264FmoMode
    {
        /// <summary>NV_ENC_H264_FMO_AUTOSELECT: FMO usage is auto selected by the encoder driver</summary>
        Autoselect = 0x0,
        /// <summary>NV_ENC_H264_FMO_ENABLE: Enable FMO</summary>
        Enable = 0x1,
        /// <summary>NV_ENC_H264_FMO_DISABLE: Disble FMO</summary>
        Disable = 0x2,
    }

    /// <summary>NV_ENC_H264_ADAPTIVE_TRANSFORM_MODE</summary>
    public enum NvEncH264AdaptiveTransformMode
    {
        /// <summary>NV_ENC_H264_ADAPTIVE_TRANSFORM_AUTOSELECT: Adaptive Transform 8x8 mode is auto selected by the encoder driver</summary>
        Autoselect = 0x0,
        /// <summary>NV_ENC_H264_ADAPTIVE_TRANSFORM_DISABLE: Adaptive Transform 8x8 mode disabled</summary>
        Disable = 0x1,
        /// <summary>NV_ENC_H264_ADAPTIVE_TRANSFORM_ENABLE: Adaptive Transform 8x8 mode should be used</summary>
        Enable = 0x2,
    }

    /// <summary>NV_ENC_STEREO_PACKING_MODE</summary>
    public enum NvEncStereoPackingMode
    {
        /// <summary>NV_ENC_STEREO_PACKING_MODE_NONE: No Stereo packing required</summary>
        None = 0x0,
        /// <summary>NV_ENC_STEREO_PACKING_MODE_CHECKERBOARD: Checkerboard mode for packing stereo frames</summary>
        Checkerboard = 0x1,
        /// <summary>NV_ENC_STEREO_PACKING_MODE_COLINTERLEAVE: Column Interleave mode for packing stereo frames</summary>
        Colinterleave = 0x2,
        /// <summary>NV_ENC_STEREO_PACKING_MODE_ROWINTERLEAVE: Row Interleave mode for packing stereo frames</summary>
        Rowinterleave = 0x3,
        /// <summary>NV_ENC_STEREO_PACKING_MODE_SIDEBYSIDE: Side-by-side mode for packing stereo frames</summary>
        Sidebyside = 0x4,
        /// <summary>NV_ENC_STEREO_PACKING_MODE_TOPBOTTOM: Top-Bottom mode for packing stereo frames</summary>
        Topbottom = 0x5,
        /// <summary>NV_ENC_STEREO_PACKING_MODE_FRAMESEQ: Frame Sequential mode for packing stereo frames</summary>
        Frameseq = 0x6,
    }

    /// <summary>NV_ENC_INPUT_RESOURCE_TYPE</summary>
    public enum NvEncInputResourceType
    {
        /// <summary>NV_ENC_INPUT_RESOURCE_TYPE_DIRECTX: input resource type is a directx9 surface</summary>
        Directx = 0x0,
        /// <summary>NV_ENC_INPUT_RESOURCE_TYPE_CUDADEVICEPTR: input resource type is a cuda device pointer surface</summary>
        Cudadeviceptr = 0x1,
        /// <summary>NV_ENC_INPUT_RESOURCE_TYPE_CUDAARRAY: input resource type is a cuda array surface.
        ///  This array must be a 2D array and the CUDA_ARRAY3D_SURFACE_LDST
        ///  flag must have been specified when creating it.</summary>
        Cudaarray = 0x2,
        /// <summary>NV_ENC_INPUT_RESOURCE_TYPE_OPENGL_TEX: input resource type is an OpenGL texture</summary>
        OpenglTex = 0x3,
    }

    /// <summary>NV_ENC_BUFFER_USAGE</summary>
    public enum NvEncBufferUsage
    {
        /// <summary>NV_ENC_INPUT_IMAGE: Registered surface will be used for input image</summary>
        NvEncInputImage = 0x0,
        /// <summary>NV_ENC_OUTPUT_MOTION_VECTOR: Registered surface will be used for output of H.264 ME only mode.
        ///  This buffer usage type is not supported for HEVC ME only mode.</summary>
        NvEncOutputMotionVector = 0x1,
        /// <summary>NV_ENC_OUTPUT_BITSTREAM: Registered surface will be used for output bitstream in encoding</summary>
        NvEncOutputBitstream = 0x2,
    }

    /// <summary>NV_ENC_DEVICE_TYPE</summary>
    public enum NvEncDeviceType
    {
        /// <summary>NV_ENC_DEVICE_TYPE_DIRECTX: encode device type is a directx9 device</summary>
        Directx = 0x0,
        /// <summary>NV_ENC_DEVICE_TYPE_CUDA: encode device type is a cuda device</summary>
        Cuda = 0x1,
        /// <summary>NV_ENC_DEVICE_TYPE_OPENGL: encode device type is an OpenGL device.
        ///  Use of this device type is supported only on Linux</summary>
        Opengl = 0x2,
    }

    /// <summary>NV_ENC_NUM_REF_FRAMES</summary>
    public enum NvEncNumRefFrames
    {
        /// <summary>NV_ENC_NUM_REF_FRAMES_AUTOSELECT: Number of reference frames is auto selected by the encoder driver</summary>
        Autoselect = 0x0,
        /// <summary>NV_ENC_NUM_REF_FRAMES_1: Number of reference frames equal to 1</summary>
        Frames1 = 0x1,
        /// <summary>NV_ENC_NUM_REF_FRAMES_2: Number of reference frames equal to 2</summary>
        Frames2 = 0x2,
        /// <summary>NV_ENC_NUM_REF_FRAMES_3: Number of reference frames equal to 3</summary>
        Frames3 = 0x3,
        /// <summary>NV_ENC_NUM_REF_FRAMES_4: Number of reference frames equal to 4</summary>
        Frames4 = 0x4,
        /// <summary>NV_ENC_NUM_REF_FRAMES_5: Number of reference frames equal to 5</summary>
        Frames5 = 0x5,
        /// <summary>NV_ENC_NUM_REF_FRAMES_6: Number of reference frames equal to 6</summary>
        Frames6 = 0x6,
        /// <summary>NV_ENC_NUM_REF_FRAMES_7: Number of reference frames equal to 7</summary>
        Frames7 = 0x7,
    }

    /// <summary>NV_ENC_CAPS</summary>
    public enum NvEncCaps
    {
        /// <summary>NV_ENC_CAPS_NUM_MAX_BFRAMES:  Maximum number of B-Frames supported.</summary>
        NumMaxBframes,
        /// <summary>NV_ENC_CAPS_SUPPORTED_RATECONTROL_MODES:  Rate control modes supported.
        ///  * \n The API return value is a bitmask of the values in NV_ENC_PARAMS_RC_MODE.</summary>
        SupportedRatecontrolModes,
        /// <summary>NV_ENC_CAPS_SUPPORT_FIELD_ENCODING:  Indicates HW support for field mode encoding.
        ///  * \n 0 : Interlaced mode encoding is not supported.
        ///  * \n 1 : Interlaced field mode encoding is supported.
        ///  * \n 2 : Interlaced frame encoding and field mode encoding are both supported.</summary>
        SupportFieldEncoding,
        /// <summary>NV_ENC_CAPS_SUPPORT_MONOCHROME:  Indicates HW support for monochrome mode encoding.
        ///  * \n 0 : Monochrome mode not supported.
        ///  * \n 1 : Monochrome mode supported.</summary>
        SupportMonochrome,
        /// <summary>NV_ENC_CAPS_SUPPORT_FMO:  Indicates HW support for FMO.
        ///  * \n 0 : FMO not supported.
        ///  * \n 1 : FMO supported.</summary>
        SupportFmo,
        /// <summary>NV_ENC_CAPS_SUPPORT_QPELMV:  Indicates HW capability for Quarter pel motion estimation.
        ///  * \n 0 : QuarterPel Motion Estimation not supported.
        ///  * \n 1 : QuarterPel Motion Estimation supported.</summary>
        SupportQpelmv,
        /// <summary>NV_ENC_CAPS_SUPPORT_BDIRECT_MODE:  H.264 specific. Indicates HW support for BDirect modes.
        ///  * \n 0 : BDirect mode encoding not supported.
        ///  * \n 1 : BDirect mode encoding supported.</summary>
        SupportBdirectMode,
        /// <summary>NV_ENC_CAPS_SUPPORT_CABAC:  H264 specific. Indicates HW support for CABAC entropy coding mode.
        ///  * \n 0 : CABAC entropy coding not supported.
        ///  * \n 1 : CABAC entropy coding supported.</summary>
        SupportCabac,
        /// <summary>NV_ENC_CAPS_SUPPORT_ADAPTIVE_TRANSFORM:  Indicates HW support for Adaptive Transform.
        ///  * \n 0 : Adaptive Transform not supported.
        ///  * \n 1 : Adaptive Transform supported.</summary>
        SupportAdaptiveTransform,
        /// <summary>NV_ENC_CAPS_SUPPORT_STEREO_MVC:  Indicates HW support for Multi View Coding.
        ///  * \n 0 : Multi View Coding not supported.
        ///  * \n 1 : Multi View Coding supported.</summary>
        SupportStereoMvc,
        /// <summary>NV_ENC_CAPS_NUM_MAX_TEMPORAL_LAYERS:  Indicates HW support for encoding Temporal layers.
        ///  * \n 0 : Encoding Temporal layers not supported.
        ///  * \n 1 : Encoding Temporal layers supported.</summary>
        NumMaxTemporalLayers,
        /// <summary>NV_ENC_CAPS_SUPPORT_HIERARCHICAL_PFRAMES:  Indicates HW support for Hierarchical P frames.
        ///  * \n 0 : Hierarchical P frames not supported.
        ///  * \n 1 : Hierarchical P frames supported.</summary>
        SupportHierarchicalPframes,
        /// <summary>NV_ENC_CAPS_SUPPORT_HIERARCHICAL_BFRAMES:  Indicates HW support for Hierarchical B frames.
        ///  * \n 0 : Hierarchical B frames not supported.
        ///  * \n 1 : Hierarchical B frames supported.</summary>
        SupportHierarchicalBframes,
        /// <summary>NV_ENC_CAPS_LEVEL_MAX:  Maximum Encoding level supported (See ::NV_ENC_LEVEL for details).</summary>
        LevelMax,
        /// <summary>NV_ENC_CAPS_LEVEL_MIN:  Minimum Encoding level supported (See ::NV_ENC_LEVEL for details).</summary>
        LevelMin,
        /// <summary>NV_ENC_CAPS_SEPARATE_COLOUR_PLANE:  Indicates HW support for separate colour plane encoding.
        ///  * \n 0 : Separate colour plane encoding not supported.
        ///  * \n 1 : Separate colour plane encoding supported.</summary>
        SeparateColourPlane,
        /// <summary>NV_ENC_CAPS_WIDTH_MAX:  Maximum output width supported.</summary>
        WidthMax,
        /// <summary>NV_ENC_CAPS_HEIGHT_MAX:  Maximum output height supported.</summary>
        HeightMax,
        /// <summary>NV_ENC_CAPS_SUPPORT_TEMPORAL_SVC:  Indicates Temporal Scalability Support.
        ///  * \n 0 : Temporal SVC encoding not supported.
        ///  * \n 1 : Temporal SVC encoding supported.</summary>
        SupportTemporalSvc,
        /// <summary>NV_ENC_CAPS_SUPPORT_DYN_RES_CHANGE:  Indicates Dynamic Encode Resolution Change Support.
        ///  * Support added from NvEncodeAPI version 2.0.
        ///  * \n 0 : Dynamic Encode Resolution Change not supported.
        ///  * \n 1 : Dynamic Encode Resolution Change supported.</summary>
        SupportDynResChange,
        /// <summary>NV_ENC_CAPS_SUPPORT_DYN_BITRATE_CHANGE:  Indicates Dynamic Encode Bitrate Change Support.
        ///  * Support added from NvEncodeAPI version 2.0.
        ///  * \n 0 : Dynamic Encode bitrate change not supported.
        ///  * \n 1 : Dynamic Encode bitrate change supported.</summary>
        SupportDynBitrateChange,
        /// <summary>NV_ENC_CAPS_SUPPORT_DYN_FORCE_CONSTQP:  Indicates Forcing Constant QP On The Fly Support.
        ///  * Support added from NvEncodeAPI version 2.0.
        ///  * \n 0 : Forcing constant QP on the fly not supported.
        ///  * \n 1 : Forcing constant QP on the fly supported.</summary>
        SupportDynForceConstqp,
        /// <summary>NV_ENC_CAPS_SUPPORT_DYN_RCMODE_CHANGE:  Indicates Dynamic rate control mode Change Support.
        ///  * \n 0 : Dynamic rate control mode change not supported.
        ///  * \n 1 : Dynamic rate control mode change supported.</summary>
        SupportDynRcmodeChange,
        /// <summary>NV_ENC_CAPS_SUPPORT_SUBFRAME_READBACK:  Indicates Subframe readback support for slice-based encoding.
        ///  * \n 0 : Subframe readback not supported.
        ///  * \n 1 : Subframe readback supported.</summary>
        SupportSubframeReadback,
        /// <summary>NV_ENC_CAPS_SUPPORT_CONSTRAINED_ENCODING:  Indicates Constrained Encoding mode support.
        ///  * Support added from NvEncodeAPI version 2.0.
        ///  * \n 0 : Constrained encoding mode not supported.
        ///  * \n 1 : Constarined encoding mode supported.
        ///  * If this mode is supported client can enable this during initialisation.
        ///  * Client can then force a picture to be coded as constrained picture where
        ///  * each slice in a constrained picture will have constrained_intra_pred_flag set to 1
        ///  * and disable_deblocking_filter_idc will be set to 2 and prediction vectors for inter
        ///  * macroblocks in each slice will be restricted to the slice region.</summary>
        SupportConstrainedEncoding,
        /// <summary>NV_ENC_CAPS_SUPPORT_INTRA_REFRESH:  Indicates Intra Refresh Mode Support.
        ///  * Support added from NvEncodeAPI version 2.0.
        ///  * \n 0 : Intra Refresh Mode not supported.
        ///  * \n 1 : Intra Refresh Mode supported.</summary>
        SupportIntraRefresh,
        /// <summary>NV_ENC_CAPS_SUPPORT_CUSTOM_VBV_BUF_SIZE:  Indicates Custom VBV Bufer Size support. It can be used for capping frame size.
        ///  * Support added from NvEncodeAPI version 2.0.
        ///  * \n 0 : Custom VBV buffer size specification from client, not supported.
        ///  * \n 1 : Custom VBV buffer size specification from client, supported.</summary>
        SupportCustomVbvBufSize,
        /// <summary>NV_ENC_CAPS_SUPPORT_DYNAMIC_SLICE_MODE:  Indicates Dynamic Slice Mode Support.
        ///  * Support added from NvEncodeAPI version 2.0.
        ///  * \n 0 : Dynamic Slice Mode not supported.
        ///  * \n 1 : Dynamic Slice Mode supported.</summary>
        SupportDynamicSliceMode,
        /// <summary>NV_ENC_CAPS_SUPPORT_REF_PIC_INVALIDATION:  Indicates Reference Picture Invalidation Support.
        ///  * Support added from NvEncodeAPI version 2.0.
        ///  * \n 0 : Reference Picture Invalidation not supported.
        ///  * \n 1 : Reference Picture Invalidation supported.</summary>
        SupportRefPicInvalidation,
        /// <summary>NV_ENC_CAPS_PREPROC_SUPPORT:  Indicates support for PreProcessing.
        ///  * The API return value is a bitmask of the values defined in ::NV_ENC_PREPROC_FLAGS</summary>
        PreprocSupport,
        /// <summary>NV_ENC_CAPS_ASYNC_ENCODE_SUPPORT:  Indicates support Async mode.
        ///  * \n 0 : Async Encode mode not supported.
        ///  * \n 1 : Async Encode mode supported.</summary>
        AsyncEncodeSupport,
        /// <summary>NV_ENC_CAPS_MB_NUM_MAX:  Maximum MBs per frame supported.</summary>
        MbNumMax,
        /// <summary>NV_ENC_CAPS_MB_PER_SEC_MAX:  Maximum aggregate throughput in MBs per sec.</summary>
        MbPerSecMax,
        /// <summary>NV_ENC_CAPS_SUPPORT_YUV444_ENCODE:  Indicates HW support for YUV444 mode encoding.
        ///  * \n 0 : YUV444 mode encoding not supported.
        ///  * \n 1 : YUV444 mode encoding supported.</summary>
        SupportYuv444Encode,
        /// <summary>NV_ENC_CAPS_SUPPORT_LOSSLESS_ENCODE:  Indicates HW support for lossless encoding.
        ///  * \n 0 : lossless encoding not supported.
        ///  * \n 1 : lossless encoding supported.</summary>
        SupportLosslessEncode,
        /// <summary>NV_ENC_CAPS_SUPPORT_SAO:  Indicates HW support for Sample Adaptive Offset.
        ///  * \n 0 : SAO not supported.
        ///  * \n 1 : SAO encoding supported.</summary>
        SupportSao,
        /// <summary>NV_ENC_CAPS_SUPPORT_MEONLY_MODE:  Indicates HW support for MEOnly Mode.
        ///  * \n 0 : MEOnly Mode not supported.
        ///  * \n 1 : MEOnly Mode supported for I and P frames.
        ///  * \n 2 : MEOnly Mode supported for I, P and B frames.</summary>
        SupportMeonlyMode,
        /// <summary>NV_ENC_CAPS_SUPPORT_LOOKAHEAD:  Indicates HW support for lookahead encoding (enableLookahead=1).
        ///  * \n 0 : Lookahead not supported.
        ///  * \n 1 : Lookahead supported.</summary>
        SupportLookahead,
        /// <summary>NV_ENC_CAPS_SUPPORT_TEMPORAL_AQ:  Indicates HW support for temporal AQ encoding (enableTemporalAQ=1).
        ///  * \n 0 : Temporal AQ not supported.
        ///  * \n 1 : Temporal AQ supported.</summary>
        SupportTemporalAq,
        /// <summary>NV_ENC_CAPS_SUPPORT_10BIT_ENCODE:  Indicates HW support for 10 bit encoding.
        ///  * \n 0 : 10 bit encoding not supported.
        ///  * \n 1 : 10 bit encoding supported.</summary>
        Support10bitEncode,
        /// <summary>NV_ENC_CAPS_NUM_MAX_LTR_FRAMES:  Maximum number of Long Term Reference frames supported</summary>
        NumMaxLtrFrames,
        /// <summary>NV_ENC_CAPS_SUPPORT_WEIGHTED_PREDICTION:  Indicates HW support for Weighted Predicition.
        ///  * \n 0 : Weighted Predicition not supported.
        ///  * \n 1 : Weighted Predicition supported.</summary>
        SupportWeightedPrediction,
        /// <summary>NV_ENC_CAPS_DYNAMIC_QUERY_ENCODER_CAPACITY:  On managed (vGPU) platforms (Windows only), this API, in conjunction with other GRID Management APIs, can be used
        ///  * to estimate the residual capacity of the hardware encoder on the GPU as a percentage of the total available encoder capacity.
        ///  * This API can be called at any time; i.e. during the encode session or before opening the encode session.
        ///  * If the available encoder capacity is returned as zero, applications may choose to switch to software encoding
        ///  * and continue to call this API (e.g. polling once per second) until capacity becomes available.
        ///  *
        ///  * On baremetal (non-virtualized GPU) and linux platforms, this API always returns 100.</summary>
        DynamicQueryEncoderCapacity,
        /// <summary>NV_ENC_CAPS_SUPPORT_BFRAME_REF_MODE:  Indicates B as refererence support.
        ///  * \n 0 : B as reference is not supported.
        ///  * \n 1 : each B-Frame as reference is supported.
        ///  * \n 2 : only Middle B-frame as reference is supported.</summary>
        SupportBframeRefMode,
        /// <summary>NV_ENC_CAPS_SUPPORT_EMPHASIS_LEVEL_MAP:  Indicates HW support for Emphasis Level Map based delta QP computation.
        ///  * \n 0 : Emphasis Level Map based delta QP not supported.
        ///  * \n 1 : Emphasis Level Map based delta QP is supported.</summary>
        SupportEmphasisLevelMap,
        /// <summary>NV_ENC_CAPS_WIDTH_MIN:  Minimum input width supported.</summary>
        WidthMin,
        /// <summary>NV_ENC_CAPS_HEIGHT_MIN:  Minimum input height supported.</summary>
        HeightMin,
        /// <summary>NV_ENC_CAPS_SUPPORT_MULTIPLE_REF_FRAMES:  Indicates HW support for multiple reference frames.</summary>
        SupportMultipleRefFrames,
    }

    /// <summary>NV_ENC_HEVC_CUSIZE</summary>
    public enum NvEncHevcCusize
    {
        /// <summary>NV_ENC_HEVC_CUSIZE_AUTOSELECT</summary>
        Autoselect = 0,
        /// <summary>NV_ENC_HEVC_CUSIZE_8x8</summary>
        Cusize8x8 = 1,
        /// <summary>NV_ENC_HEVC_CUSIZE_16x16</summary>
        Cusize16x16 = 2,
        /// <summary>NV_ENC_HEVC_CUSIZE_32x32</summary>
        Cusize32x32 = 3,
        /// <summary>NV_ENC_HEVC_CUSIZE_64x64</summary>
        Cusize64x64 = 4,
    }
}
