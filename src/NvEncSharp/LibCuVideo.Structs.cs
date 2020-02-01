using System;
using System.Runtime.InteropServices;
using static Lennox.NvEncSharp.LibCuda;

// ReSharper disable UnusedMember.Global

namespace Lennox.NvEncSharp
{
    /// <summary>
    /// \struct CUVIDDECODECAPS;
    /// This structure is used in cuvidGetDecoderCaps API
    ///</summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct CuVideoDecodeCaps
    {
        /// <summary>IN: CuVideoCodec_XXX</summary>
        public CuVideoCodec CodecType;
        /// <summary>IN: CuVideoChromaFormat_XXX</summary>
        public CuVideoChromaFormat ChromaFormat;
        /// <summary>IN: The Value "BitDepth minus 8"</summary>
        public int BitDepthMinus8;
        /// <summary>Reserved for future use - set to zero</summary>
        private fixed uint _reserved1[3];

        /// <summary>OUT: 1 if codec supported, 0 if not supported</summary>
        public ByteBool IsSupported;
        /// <summary>Reserved for future use - set to zero</summary>
        private byte _reserved2;
        /// <summary>OUT: each bit represents corresponding CuVideoSurfaceFormat enum</summary>
        public CuVideoSurfaceFormat OutputFormatMask
        {
            get => (CuVideoSurfaceFormat)_outputFormatMask;
            set => _outputFormatMask = (short)value;
        }
        private short _outputFormatMask;
        /// <summary>OUT: Max supported coded width in pixels</summary>
        public int MaxWidth;
        /// <summary>OUT: Max supported coded height in pixels</summary>
        public int MaxHeight;
        /// <summary>OUT: Max supported macroblock count
        /// CodedWidth*CodedHeight/256 must be <= nMaxMBCount</summary>
        public int MaxMBCount;
        /// <summary>OUT: Min supported coded width in pixels</summary>
        public short MinWidth;
        /// <summary>OUT: Min supported coded height in pixels</summary>
        public short MinHeight;
        /// <summary>Reserved for future use - set to zero</summary>
        private fixed uint _reserved3[11];

        public static CuVideoDecodeCaps GetDecoderCaps(
            CuVideoCodec codecType,
            CuVideoChromaFormat chromaFormat,
            int bitDepthMinus8)
        {
            var caps = new CuVideoDecodeCaps
            {
                CodecType = codecType,
                ChromaFormat = chromaFormat,
                BitDepthMinus8 = bitDepthMinus8
            };

            var result = LibCuVideo.GetDecoderCaps(ref caps);
            CheckResult(result);

            return caps;
        }
    }

    /// <summary>
    /// \struct CUVIDDECODECREATEINFO
    /// This structure is used in cuvidCreateDecoder API
    ///</summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct CuVideoDecodeCreateInfo
    {
        /// <summary>IN: Coded sequence width in pixels</summary>
        public int Width;
        /// <summary>IN: Coded sequence height in pixels</summary>
        public int Height;
        /// <summary>IN: Maximum number of internal decode surfaces</summary>
        public int NumDecodeSurfaces;
        /// <summary>IN: CuVideoCodec_XXX</summary>
        public CuVideoCodec CodecType;
        /// <summary>IN: CuVideoChromaFormat_XXX</summary>
        public CuVideoChromaFormat ChromaFormat;
        /// <summary>IN: Decoder creation flags (cudaVideoCreateFlags_XXX)</summary>
        public CuVideoCreateFlags CreationFlags;
        /// <summary>IN: The value "BitDepth minus 8"</summary>
        public int BitDepthMinus8;
        /// <summary>IN: Set 1 only if video has all intra frames (default value is 0). This will
        /// optimize video memory for Intra frames only decoding. The support is limited
        /// to specific codecs - H264, HEVC, VP9, the flag will be ignored for codecs which
        /// are not supported. However decoding might fail if the flag is enabled in case
        /// of supported codecs for regular bit streams having P and/or B frames.</summary>
        public int IntraDecodeOnly;
        /// <summary>IN: Coded sequence max width in pixels used with reconfigure Decoder</summary>
        public int MaxWidth;
        /// <summary>IN: Coded sequence max height in pixels used with reconfigure Decoder</summary>
        public int MaxHeight;
        /// <summary>Reserved for future use - set to zero</summary>
        private uint _reserved1;
        /// <summary>IN: area of the frame that should be displayed</summary>
        public CuRectangleShort DisplayArea;

        /// <summary>IN: CuVideoSurfaceFormat_XXX</summary>
        public CuVideoSurfaceFormat OutputFormat;
        /// <summary>IN: CuVideoDeinterlaceMode_XXX</summary>
        public CuVideoDeinterlaceMode DeinterlaceMode;
        /// <summary>IN: Post-processed output width (Should be aligned to 2)</summary>
        public int TargetWidth;
        /// <summary>IN: Post-processed output height (Should be aligned to 2)</summary>
        public int TargetHeight;
        /// <summary>IN: Maximum number of output surfaces simultaneously mapped</summary>
        public int NumOutputSurfaces;
        /// <summary>IN: If non-NULL, context lock used for synchronizing ownership of
        /// the Cu context. Needed for CuVideoCreate_PreferCUDA decode</summary>
        public CuVideoContextLock VideoLock;
        /// <summary>IN: target rectangle in the output frame (for aspect ratio conversion)</summary>
        public CuRectangleShort TargetRect;
        private fixed uint _reserved2[5];

        public int GetBytesPerPixel()
        {
            return BitDepthMinus8 > 0 ? 2 : 1;
        }

        public IntPtr GetFrameByteSize(int pitch, out int chromaHeight)
        {
            var chromaInfo = new CuVideoChromaFormatInformation(ChromaFormat);
            chromaHeight = (int)(Height * chromaInfo.HeightFactor);
            var height = Height + chromaHeight * chromaInfo.PlaneCount;
            return new IntPtr((long)(pitch * height * GetBytesPerPixel()));
        }

        public int GetBitmapFrameSize(int bytesPerPixel)
        {
            return Height * Width * bytesPerPixel;
        }

        public YuvInformation GetYuvInformation(int pitch)
        {
            var chromaInfo = new CuVideoChromaFormatInformation(ChromaFormat);
            var bytesPerPixel = GetBytesPerPixel();

            var frameByteSize = GetFrameByteSize(pitch, out var chromaHeight);

            // y = luma

            var lumaPitch = Width;
            var lumaHeight = Height;

            return new YuvInformation
            {
                BytesPerPixel = bytesPerPixel,
                FrameByteSize = frameByteSize.ToInt32(),
                LumaHeight = lumaHeight,
                LumaPitch = lumaPitch,
                ChromaHeight = chromaHeight,
                ChromaOffset = bytesPerPixel * lumaPitch * lumaHeight,
                ChromaPitch = (int)(lumaPitch * chromaInfo.HeightFactor) * 2,
                ChromaPlaneCount = chromaInfo.PlaneCount
            };
        }
    }

    public struct YuvInformation
    {
        public int BytesPerPixel { get; set; }
        public int FrameByteSize { get; set; }
        public int LumaHeight { get; set; }
        public int LumaPitch { get; set; }
        public int ChromaHeight { get; set; }
        public int ChromaOffset { get; set; }
        public int ChromaPitch { get; set; }
        public int ChromaPlaneCount { get; set; }
    }

    public struct CuVideoChromaFormatInformation
    {
        public float HeightFactor;
        public int PlaneCount;

        public CuVideoChromaFormatInformation(CuVideoChromaFormat format)
        {
            HeightFactor = 0.5f;
            PlaneCount = 1;

            switch (format)
            {
                case CuVideoChromaFormat.Monochrome:
                    HeightFactor = 0.0f;
                    PlaneCount = 0;
                    break;
                case CuVideoChromaFormat.YUV420:
                    break;
                case CuVideoChromaFormat.YUV422:
                    HeightFactor = 1.0f;
                    break;
                case CuVideoChromaFormat.YUV444:
                    HeightFactor = 1.0f;
                    PlaneCount = 2;
                    break;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CuRectangle
    {
        public static CuRectangle Null { get; } = new CuRectangle(0, 0, 0, 0);

        public int Width => Right - Left;
        public int Height => Bottom - Top;

        /// <summary>left position of rect</summary>
        public int Left;
        /// <summary>top position of rect</summary>
        public int Top;
        /// <summary>right position of rect</summary>
        public int Right;
        /// <summary>bottom position of rect</summary>
        public int Bottom;

        public CuRectangle(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CuRectangleShort
    {
        public static CuRectangleShort Null { get; } = new CuRectangleShort(0, 0, 0, 0);

        public short Width => (short)(Right - Left);
        public short Height => (short)(Bottom - Top);

        /// <summary>left position of rect</summary>
        public short Left;
        /// <summary>top position of rect</summary>
        public short Top;
        /// <summary>right position of rect</summary>
        public short Right;
        /// <summary>bottom position of rect</summary>
        public short Bottom;

        public CuRectangleShort(short left, short top, short right, short bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
    }

    /// <summary>
    /// \struct CUVIDEOFORMAT
    /// Video format
    /// Used in cuvidGetSourceVideoFormat API
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct CuVideoFormat
    {
        /// <summary>OUT: Compression format</summary>
        public CuVideoCodec Codec;
        /// <summary>OUT: frame rate numerator   (0 = unspecified or variable frame rate)</summary>
        public int FrameRateNumerator;
        /// <summary>OUT: frame rate denominator (0 = unspecified or variable frame rate)</summary>
        public int FrameRateDenominator;
        /// <summary>OUT: 0=interlaced, 1=progressive</summary>
        public ByteBool ProgressiveSequence;
        /// <summary>OUT: high bit depth luma. E.g, 2 for 10-bitdepth, 4 for 12-bitdepth</summary>
        public byte BitDepthLumaMinus8;
        /// <summary>OUT: high bit depth chroma. E.g, 2 for 10-bitdepth, 4 for 12-bitdepth</summary>
        public byte BitDepthChromaMinus8;
        /// <summary>OUT: Minimum number of decode surfaces to be allocated for correct
        /// decoding. The client can send this value in ulNumDecodeSurfaces
        /// (in CUVIDDECODECREATEINFO structure).
        /// This guarantees correct functionality and optimal video memory
        /// usage but not necessarily the best performance, which depends on
        /// the design of the overall application. The optimal number of
        /// decode surfaces (in terms of performance and memory utilization)
        /// should be decided by experimentation for each application, but it
        /// cannot go below min_num_decode_surfaces.
        /// If this value is used for ulNumDecodeSurfaces then it must be
        /// returned to parser during sequence callback.</summary>
        public byte MinNumDecodeSurfaces;
        /// <summary>OUT: coded frame width in pixels</summary>
        public int CodedWidth;
        /// <summary>OUT: coded frame height in pixels</summary>
        public int CodedHeight;
        public CuRectangle DisplayArea;
        /// <summary>OUT:  Chroma format</summary>
        public CuVideoChromaFormat ChromaFormat;
        /// <summary>OUT: video bitrate (bps, 0=unknown)</summary>
        public int Bitrate;
        /// <summary>OUT: Display Aspect Ratio = x:y (4:3, 16:9, etc)</summary>
        public int DisplayAspectRatioX;
        /// <summary>OUT: Display Aspect Ratio = x:y (4:3, 16:9, etc)</summary>
        public int DisplayAspectRatioY;
        internal fixed byte BitField1[1];

        /// <summary>OUT: 0-Component, 1-PAL, 2-NTSC, 3-SECAM, 4-MAC, 5-Unspecified</summary>
        public byte VideoFormat
        {
            get { fixed (byte* ptr = &BitField1[0]) { return (byte)((*(byte*)ptr >> 0) & 3); } }
            set => BitField1[0] = (byte)((BitField1[0] & ~24) | (((value) << 0) & 24));
        }

        /// <summary>OUT: indicates the black level and luma and chroma range</summary>
        public bool VideoFullRange
        {
            get => (BitField1[0] & 4) != 0;
            set => BitField1[0] = value ? (byte)(BitField1[0] | 4) : (byte)(BitField1[0] & -5);
        }

        /// <summary>OUT: chromaticity coordinates of source primaries</summary>
        public byte ColorPrimaries;
        /// <summary>OUT: opto-electronic transfer characteristic of the source picture</summary>
        public byte TransferCharacteristics;
        /// <summary>OUT: used in deriving luma and chroma signals from RGB primaries</summary>
        public byte MatrixCoefficients;
        /// <summary>UT: Additional bytes following (CUVIDEOFORMATEX)</summary>
        public int SeqHdrDataLength;

        public CuVideoSurfaceFormat GetSurfaceFormat()
        {
            switch (ChromaFormat)
            {
                case CuVideoChromaFormat.YUV420:
                    return BitDepthLumaMinus8 > 0
                        ? CuVideoSurfaceFormat.P016
                        : CuVideoSurfaceFormat.NV12;
                case CuVideoChromaFormat.YUV444:
                    return BitDepthLumaMinus8 > 0
                        ? CuVideoSurfaceFormat.YUV444_16Bit
                        : CuVideoSurfaceFormat.YUV444;
            }

            return CuVideoSurfaceFormat.Default;
        }

        public bool IsSupportedByDecoder(out string error)
        {
            return IsSupportedByDecoder(out error, out _);
        }

        public bool IsSupportedByDecoder(
            out string error,
            out CuVideoDecodeCaps caps)
        {
            caps = CuVideoDecodeCaps.GetDecoderCaps(
                Codec, ChromaFormat, BitDepthLumaMinus8);

            if (!caps.IsSupported)
            {
                error = $"Codec {Codec} is not supported.";
                return false;
            }

            if (CodedWidth > caps.MaxWidth ||
                CodedHeight > caps.MaxHeight)
            {
                error = $"Unsupported video dimentions. Requested: {CodedWidth}x{CodedHeight}. Supported max: {caps.MaxWidth}x{caps.MaxHeight}.";
                return false;
            }

            error = null;
            return true;
        }
    }

    /// <summary>
    /// \struct CUVIDEOFORMATEX
    /// Video format including raw sequence header information
    /// Used in cuvidGetSourceVideoFormat API
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct CuVideoFormatEx
    {
        /// <summary>OUT: CUVIDEOFORMAT structure</summary>
        public CuVideoFormat Format;
        /// <summary>OUT: Sequence header data</summary>
        public fixed byte RawSeqHdrData[1024];
    }

    /// <summary>
    /// \struct CUVIDPARSERPARAMS
    /// Used in cuvidCreateVideoParser API
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct CuVideoParserParams
    {
        /// <summary>IN: cudaVideoCodec_XXX</summary>
        public CuVideoCodec CodecType;
        /// <summary>IN: Max # of decode surfaces (parser will cycle through these)</summary>
        public uint MaxNumDecodeSurfaces;
        /// <summary>IN: Timestamp units in Hz (0=default=10000000Hz)</summary>
        public uint ClockRate;
        /// <summary>IN: % Error threshold (0-100) for calling pfnDecodePicture (100=always
        /// IN: call pfnDecodePicture even if picture bitstream is fully corrupted)</summary>
        public uint ErrorThreshold;
        /// <summary>IN: Max display queue delay (improves pipelining of decode with display)
        /// 0=no delay (recommended values: 2..4)</summary>
        public uint MaxDisplayDelay;
        /// <summary>IN: Reserved for future use - set to 0</summary>
        public fixed uint Reserved1[5];
        /// <summary>IN: User data for callbacks</summary>
        public IntPtr UserData;
        /// <summary>IN: Called before decoding frames and/or whenever there is a fmt change</summary>
        public LibCuVideo.VideoSequenceCallback SequenceCallback;
        /// <summary>IN: Called when a picture is ready to be decoded (decode order)</summary>
        public LibCuVideo.VideoDecodeCallback DecodePicture;
        /// <summary>IN: Called whenever a picture is ready to be displayed (display order)</summary>
        public LibCuVideo.VideoDisplayCallback DisplayPicture;
        private IntPtr _reserved21;
        private IntPtr _reserved22;
        private IntPtr _reserved23;
        private IntPtr _reserved24;
        private IntPtr _reserved25;
        private IntPtr _reserved26;
        private IntPtr _reserved27;
        /// <summary>IN: [Optional] sequence header data from system layer </summary>
        public CuVideoFormatEx* ExtVideoInfo;
    }

    /// <summary>
    /// \struct CUAUDIOFORMAT
    /// Audio formats
    /// Used in cuvidGetSourceAudioFormat API
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CuAudioFormat
    {
        /// <summary>OUT: Compression format</summary>
        public CuAudioCodec Codec;
        /// <summary>OUT: number of audio channels</summary>
        public uint Channels;
        /// <summary>OUT: sampling frequency</summary>
        public uint Samplespersec;
        /// <summary>OUT: For uncompressed, can also be used to determine bits per sample</summary>
        public uint Bitrate;
        /// <summary> Reserved for future use</summary>
        private uint _reserved1;
        /// <summary> Reserved for future use</summary>
        private uint _reserved2;
    }

    /// <summary>
    /// \struct CUVIDSOURCEDATAPACKET
    /// Data Packet
    /// Used in cuvidParseVideoData API
    /// IN for cuvidParseVideoData
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct CuVideoSourceDataPacket
    {
        /// <summary>IN: Combination of CUVID_PKT_XXX flags</summary>
        public CuVideoPacketFlags Flags;
        /// <summary>IN: number of bytes in the payload (may be zero if EOS flag is set)</summary>
        public uint PayloadSize;
        /// <summary>IN: Pointer to packet payload data (may be NULL if EOS flag is set)</summary>
        public byte* Payload;
        /// <summary>IN: Presentation time stamp (10MHz clock), only valid if
        /// CUVID_PKT_TIMESTAMP flag is set</summary>
        public long Timestamp;
    }

    /// <summary>
    /// \struct CUVIDSOURCEPARAMS
    /// Describes parameters needed in cuvidCreateVideoSource API
    /// NVDECODE API is intended for HW accelerated video decoding so CUvideosource doesn't have audio demuxer for all supported
    /// containers. It's recommended to clients to use their own or third party demuxer if audio support is needed.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct CuVideoSourceParams
    {
        /// <summary>IN: Time stamp units in Hz (0=default=10000000Hz)</summary>
        public int ClockRate;
        /// <summary>Reserved for future use - set to zero</summary>
        private fixed uint _reserved1[7];
        /// <summary>IN: User private data passed in to the data handlers</summary>
        public IntPtr UserData;
        /// <summary>IN: Called to deliver video packets</summary>
        // TODO: Fix non-delegate type.
        public LibCuVideo.VideoSourceCallback VideoDataHandler;
        /// <summary>IN: Called to deliver audio packets.</summary>
        public LibCuVideo.VideoSourceCallback AudioDataHandler;
        private IntPtr _reserved21;
        private IntPtr _reserved22;
        private IntPtr _reserved23;
        private IntPtr _reserved24;
        private IntPtr _reserved25;
        private IntPtr _reserved26;
        private IntPtr _reserved27;
        private IntPtr _reserved28;
    }

    /// <summary>
    /// \struct CUVIDMPEG2PicParams
    /// MPEG-2 picture parameters
    /// This structure is used in CUVIDPicParams structure
    ///</summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct CuVideoMpeg2PicParams
    {
        /// <summary>Picture index of forward reference (P/B-frames)</summary>
        public int ForwardRefIdx;
        /// <summary>Picture index of backward reference (B-frames)</summary>
        public int BackwardRefIdx;
        public int PictureCodingType;
        public int FullPelForwardVector;
        public int FullPelBackwardVector;
        public fixed int FCode[2 * 2];
        public int IntraDcPrecision;
        public int FramePredFrameDct;
        public int ConcealmentMotionVectors;
        public int QScaleType;
        public int IntraVlcFormat;
        public int AlternateScan;
        public int TopFieldFirst;
        // Quantization matrices (raster order)
        public fixed byte QuantMatrixIntra[64];
        public fixed byte QuantMatrixInter[64];
    }

    /// <summary>
    /// \struct CUVIDMPEG4PicParams
    /// MPEG-4 picture parameters
    /// This structure is used in CUVIDPicParams structure
    ///</summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct CuVideoMpeg4PicParams
    {
        public int ForwardRefIdx; // Picture index of forward reference (P/B-frames)
        public int BackwardRefIdx; // Picture index of backward reference (B-frames)
        // VOL
        public int VideoObjectLayerWidth;
        public int VideoObjectLayerHeight;
        public int VopTimeIncrementBitcount;
        public int TopFieldFirst;
        public int ResyncMarkerDisable;
        public int QuantType;
        public int QuarterSample;
        public int ShortVideoHeader;
        public int DivxFlags;
        // VOP
        public int VopCodingType;
        public int VopCoded;
        public int VopRoundingType;
        public int AlternateVerticalScanFlag;
        public int Interlaced;
        public int VopFcodeForward;
        public int VopFcodeBackward;
        public fixed int Trd[2];
        public fixed int Trb[2];
        // Quantization matrices (raster order)
        public fixed byte QuantMatrixIntra[64];
        public fixed byte QuantMatrixInter[64];
        public int GmcEnabled;
    }

    /// <summary>
    /// \struct CUVIDVC1PicParams
    /// VC1 picture parameters
    /// This structure is used in CUVIDPicParams structure
    ///</summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct CuVideoVC1PicParams
    {
        /// <summary>Picture index of forward reference (P/B-frames)</summary>
        public int ForwardRefIdx;
        /// <summary>Picture index of backward reference (B-frames)</summary>
        public int BackwardRefIdx;
        /// <summary>Actual frame width</summary>
        public int FrameWidth;
        /// <summary>Actual frame height</summary>
        public int FrameHeight;
        // PICTURE
        /// <summary>Set to 1 for I,BI frames</summary>
        public int IntraPicFlag;
        /// <summary>Set to 1 for I,P frames</summary>
        public int RefPicFlag;
        /// <summary>Progressive frame</summary>
        public int ProgressiveFcm;
        // SEQUENCE
        public int Profile;
        public int Postprocflag;
        public int Pulldown;
        public int Interlace;
        public int Tfcntrflag;
        public int Finterpflag;
        public int Psf;
        public int Multires;
        public int Syncmarker;
        public int Rangered;
        public int Maxbframes;
        // ENTRYPOINT
        public int PanscanFlag;
        public int RefdistFlag;
        public int ExtendedMv;
        public int Dquant;
        public int Vstransform;
        public int Loopfilter;
        public int Fastuvmc;
        public int Overlap;
        public int Quantizer;
        public int ExtendedDmv;
        public int RangeMapyFlag;
        public int RangeMapy;
        public int RangeMapuvFlag;
        public int RangeMapuv;
        public int Rangeredfrm; // range reduction state
    }

    /// <summary>
    /// \struct CUVIDJPEGPicParams
    /// JPEG picture parameters
    /// This structure is used in CUVIDPicParams structure
    ///</summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct CuVideoJpegPicParams
    {
        private int _reserved;
    }

    /// <summary>
    /// \struct CUVIDHEVCPicParams
    /// HEVC picture parameters
    /// This structure is used in CUVIDPicParams structure
    ///</summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct CuVideoHevcPicParams
    {
        // sps
        public int PicWidthInLumaSamples;
        public int PicHeightInLumaSamples;
        public byte Log2MinLumaCodingBlockSizeMinus3;
        public byte Log2DiffMaxMinLumaCodingBlockSize;
        public byte Log2MinTransformBlockSizeMinus2;
        public byte Log2DiffMaxMinTransformBlockSize;
        public byte PcmEnabledFlag;
        public byte Log2MinPcmLumaCodingBlockSizeMinus3;
        public byte Log2DiffMaxMinPcmLumaCodingBlockSize;
        public byte PcmSampleBitDepthLumaMinus1;

        public byte PcmSampleBitDepthChromaMinus1;
        public byte PcmLoopFilterDisabledFlag;
        public byte StrongIntraSmoothingEnabledFlag;
        public byte MaxTransformHierarchyDepthIntra;
        public byte MaxTransformHierarchyDepthInter;
        public byte AmpEnabledFlag;
        public byte SeparateColourPlaneFlag;
        public byte Log2MaxPicOrderCntLsbMinus4;

        public byte NumShortTermRefPicSets;
        public byte LongTermRefPicsPresentFlag;
        public byte NumLongTermRefPicsSps;
        public byte SpsTemporalMvpEnabledFlag;
        public byte SampleAdaptiveOffsetEnabledFlag;
        public byte ScalingListEnableFlag;
        public byte IrapPicFlag;
        public byte IdrPicFlag;

        public byte BitDepthLumaMinus8;
        public byte BitDepthChromaMinus8;
        //sps/pps extension fields
        public byte Log2MaxTransformSkipBlockSizeMinus2;
        public byte Log2SaoOffsetScaleLuma;
        public byte Log2SaoOffsetScaleChroma;
        public byte HighPrecisionOffsetsEnabledFlag;
        private fixed byte _reserved1[10];

        // pps
        public byte DependentSliceSegmentsEnabledFlag;
        public byte SliceSegmentHeaderExtensionPresentFlag;
        public byte SignDataHidingEnabledFlag;
        public byte CuQpDeltaEnabledFlag;
        public byte DiffCuQpDeltaDepth;
        public sbyte InitQpMinus26;
        public sbyte PpsCbQpOffset;
        public sbyte PpsCrQpOffset;

        public byte ConstrainedIntraPredFlag;
        public byte WeightedPredFlag;
        public byte WeightedBipredFlag;
        public byte TransformSkipEnabledFlag;
        public byte TransquantBypassEnabledFlag;
        public byte EntropyCodingSyncEnabledFlag;
        public byte Log2ParallelMergeLevelMinus2;
        public byte NumExtraSliceHeaderBits;

        public byte LoopFilterAcrossTilesEnabledFlag;
        public byte LoopFilterAcrossSlicesEnabledFlag;
        public byte OutputFlagPresentFlag;
        public byte NumRefIdxL0DefaultActiveMinus1;
        public byte NumRefIdxL1DefaultActiveMinus1;
        public byte ListsModificationPresentFlag;
        public byte CabacInitPresentFlag;
        public byte PpsSliceChromaQpOffsetsPresentFlag;

        public byte DeblockingFilterOverrideEnabledFlag;
        public byte PpsDeblockingFilterDisabledFlag;
        public sbyte PpsBetaOffsetDiv2;
        public sbyte PpsTcOffsetDiv2;
        public byte TilesEnabledFlag;
        public byte UniformSpacingFlag;
        public byte NumTileColumnsMinus1;
        public byte NumTileRowsMinus1;

        public fixed ushort ColumnWidthMinus1[21];
        public fixed ushort RowHeightMinus1[21];

        // sps and pps extension HEVC-main 444
        public byte SpsRangeExtensionFlag;
        public byte TransformSkipRotationEnabledFlag;
        public byte TransformSkipContextEnabledFlag;
        public byte ImplicitRdpcmEnabledFlag;

        public byte ExplicitRdpcmEnabledFlag;
        public byte ExtendedPrecisionProcessingFlag;
        public byte IntraSmoothingDisabledFlag;
        public byte PersistentRiceAdaptationEnabledFlag;

        public byte CabacBypassAlignmentEnabledFlag;
        public byte PpsRangeExtensionFlag;
        public byte CrossComponentPredictionEnabledFlag;
        public byte ChromaQpOffsetListEnabledFlag;

        public byte DiffCuChromaQpOffsetDepth;
        public byte ChromaQpOffsetListLenMinus1;
        public fixed sbyte CbQpOffsetList[6];

        public fixed sbyte CrQpOffsetList[6];
        private fixed byte _reserved2[2];

        private fixed uint _reserved3[8];

        // RefPicSets
        public int NumBitsForShortTermRpsInSlice;
        public int NumDeltaPocsOfRefRpsIdx;
        public int NumPocTotalCurr;
        public int NumPocStCurrBefore;
        public int NumPocStCurrAfter;
        public int NumPocLtCurr;
        public int CurrPicOrderCntVal;
        /// <summary>[refpic] Indices of valid reference pictures (-1 if unused for reference)</summary>
        public fixed int RefPicIdx[16];
        /// <summary>[refpic]</summary>
        public fixed int PicOrderCntVal[16];
        /// <summary>[refpic] 0=not a long-term reference, 1=long-term reference</summary>
        public fixed byte IsLongTerm[16];
        /// <summary>[0..NumPocStCurrBefore-1] -> refpic (0..15)</summary>
        public fixed byte RefPicSetStCurrBefore[8];
        /// <summary>[0..NumPocStCurrAfter-1] -> refpic (0..15)</summary>
        public fixed byte RefPicSetStCurrAfter[8];
        /// <summary>[0..NumPocLtCurr-1] -> refpic (0..15)</summary>
        public fixed byte RefPicSetLtCurr[8];
        public fixed byte RefPicSetInterLayer0[8];
        public fixed byte RefPicSetInterLayer1[8];
        private fixed uint _reserved4[12];

        // scaling lists (diag order)
        public fixed byte ScalingList4X4[6 * 16]; // [matrixId*i]
        public fixed byte ScalingList8X8[6 * 64]; // [matrixId*i]
        public fixed byte ScalingList16X16[6 * 64]; // [matrixId*i]
        public fixed byte ScalingList32X32[2 * 64]; // [matrixId*i]
        public fixed byte ScalingListDcCoeff16X16[6]; // [matrixId]
        public fixed byte ScalingListDcCoeff32X32[2]; // [matrixId]
    }

    /// <summary>
    /// \struct CUVIDVP8PicParams
    /// VP8 picture parameters
    /// This structure is used in CUVIDPicParams structure
    ///</summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct CuVideoVP8PicParams
    {
        public int Width;
        public int Height;
        public uint FirstPartitionSize;
        //Frame Indexes
        public byte LastRefIdx;
        public byte GoldenRefIdx;
        public byte AltRefIdx;
        // TODO: Fix.
        // union {
        //    struct {
        //        /// <summary>0 = KEYFRAME, 1 = INTERFRAME</summary>
        //        public byte frame_type : 1;
        //        public byte version : 3;
        //        public byte show_frame : 1;
        //        /// <summary>Must be 0 if segmentation is not enabled</summary>
        //        public byte update_mb_segmentation_data : 1;
        //        private byte _reserved2Bits : 2;
        //    }
        // vp8_frame_tag;

        public byte FrameTagFlags;

        private fixed byte _reserved1[4];
        private fixed uint _reserved2[3];
    }

    /// <summary>
    /// \struct CUVIDVP9PicParams
    /// VP9 picture parameters
    /// This structure is used in CUVIDPicParams structure
    ///</summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct CuVideoVP9PicParams
    {
        public uint Width;
        public uint Height;

        //Frame Indices
        public byte LastRefIdx;
        public byte GoldenRefIdx;
        public byte AltRefIdx;
        public byte ColorSpace;

        // TODO: Fix.
        public fixed byte FrameFlags[2];
        // public ushort profile : 3;
        // public ushort frameContextIdx : 2;
        // public ushort frameType : 1;
        // public ushort showFrame : 1;
        // public ushort errorResilient : 1;
        // public ushort frameParallelDecoding : 1;
        // public ushort subSamplingX : 1;
        // public ushort subSamplingY : 1;
        // public ushort intraOnly : 1;
        // public ushort allow_high_precision_mv : 1;
        // public ushort refreshEntropyProbs : 1;
        // private ushort _reserved2Bits : 2;

        private ushort _reserved16Bits;

        public fixed byte RefFrameSignBias[4];

        public byte BitDepthMinus8Luma;
        public byte BitDepthMinus8Chroma;
        public byte LoopFilterLevel;
        public byte LoopFilterSharpness;

        public byte ModeRefLfEnabled;
        public byte Log2TileColumns;
        public byte Log2TileRows;

        // TODO: Fix
        public byte SegmentFlags;
        // public byte segmentEnabled : 1;
        // public byte segmentMapUpdate : 1;
        // public byte segmentMapTemporalUpdate : 1;
        // public byte segmentFeatureMode : 1;
        // private byte _reserved4Bits : 4;

        public fixed byte SegmentFeatureEnable[8 * 4];
        public fixed short SegmentFeatureData[8 * 4];
        public fixed byte MbSegmentTreeProbs[7];
        public fixed byte SegmentPredProbs[3];
        private fixed byte _reservedSegment16Bits[2];

        public int QpYAc;
        public int QpYDc;
        public int QpChDc;
        public int QpChAc;

        public fixed uint ActiveRefIdx[3];
        public uint ResetFrameContext;
        public uint McompFilterType;
        public fixed uint MbRefLfDelta[4];
        public fixed uint MbModeLfDelta[2];
        public uint FrameTagSize;
        public uint OffsetToDctParts;
        private fixed uint _reserved128Bits[4];
    }

    /// <summary>
    /// \struct CUVIDH264DPBENTRY
    /// H.264 DPB entry
    /// This structure is used in CUVIDH264PicParams structure
    ///</summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct CuVideoH264DpbEntry
    {
        /// <summary>picture index of reference frame</summary>
        public int PicIdx;
        /// <summary>frame_num(short-term) or LongTermFrameIdx(long-term)</summary>
        public int FrameIdx;
        /// <summary>0=short term reference, 1=long term reference</summary>
        public int IsLongTerm;
        /// <summary>non-existing reference frame (corresponding PicIdx should be set to -1)</summary>
        public int NotExisting;
        /// <summary>0=unused, 1=top_field, 2=bottom_field, 3=both_fields</summary>
        public int UsedForReference;
        /// <summary>field order count of top and bottom fields</summary>
        public fixed int FieldOrderCnt[2];
    }

    /// <summary>
    /// \struct CUVIDH264MVCEXT
    /// H.264 MVC picture parameters ext
    /// This structure is used in CUVIDH264PicParams structure
    ///</summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct CuVideoH264MvcExt
    {
        /// <summary>Max number of coded views minus 1 in video : Range - 0 to 1023</summary>
        public int NumViewsMinus1;
        /// <summary>view identifier</summary>
        public int ViewId;
        /// <summary>1 if used for inter-view prediction, 0 if not</summary>
        public byte InterViewFlag;
        /// <summary>number of inter-view ref pics in RefPicList0</summary>
        public byte NumInterViewRefsL0;
        /// <summary>number of inter-view ref pics in RefPicList1</summary>
        public byte NumInterViewRefsL1;
        /// <summary>Reserved bits</summary>
        public byte MvcReserved8Bits;
        /// <summary>view id of the i-th view component for inter-view prediction in RefPicList0</summary>
        public fixed int InterViewRefsL0[16];
        /// <summary>view id of the i-th view component for inter-view prediction in RefPicList1</summary>
        public fixed int InterViewRefsL1[16];
    }

    /// <summary>
    /// \struct CUVIDH264SVCEXT
    /// H.264 SVC picture parameters ext
    /// This structure is used in CUVIDH264PicParams structure
    ///</summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct CuVideoH264SvcExt
    {
        public byte ProfileIdc;
        public byte LevelIdc;
        public byte DQId;
        public byte DQIdMax;
        public byte DisableInterLayerDeblockingFilterIdc;
        public byte RefLayerChromaPhaseYPlus1;
        public sbyte InterLayerSliceAlphaC0OffsetDiv2;
        public sbyte InterLayerSliceBetaOffsetDiv2;

        public ushort DPBEntryValidFlag;
        public byte InterLayerDeblockingFilterControlPResentFlag;
        public byte ExtendedSpatialScalabilityIdc;
        public byte AdaptiveTcoeffLevelPredictionFlag;
        public byte SliceHeaderRestrictionFlag;
        public byte ChromaPhaseXPlus1Flag;
        public byte ChromaPhaseTPlus1;

        public byte TcoeffLevelPredictionFlag;
        public byte ConstrainedIntraResamplingFlag;
        public byte RefLayerChromaPhaseXPlus1Flag;
        public byte StoreRefBasePicFlag;
        private byte _reserved8BitsA;
        private byte _reserved8BitsB;

        public short ScaledRefLayerLeftOffet;
        public short ScaledRefLayerTopOffset;
        public short ScaledRefLayerRightOffset;
        public short ScaledRefLayerBottomOffset;
        private ushort _reserved16Bits;
        /// <summary>Points to the PicParams for the next layer to be decoded.
        /// Linked list ends at the target layer.</summary>
        public CuVideoPicParams* NextLayer;
        /// <summary>whether to store ref base pic</summary>
        public int bRefBaseLayer;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct CuVideoH264PicParamsExt
    {
        [FieldOffset(0)]
        public CuVideoH264MvcExt MvcExt;

        [FieldOffset(0)]
        public CuVideoH264SvcExt SvcExt;
    }

    /// <summary>
    /// \struct CUVIDH264PicParams
    /// H.264 picture parameters
    /// This structure is used in CUVIDPicParams structure
    ///</summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct CuVideoH264PicParams
    {
        // SPS
        public int Log2MaxFrameNumMinus4;
        public int PicOrderCntType;
        public int Log2MaxPicOrderCntLsbMinus4;
        public int DeltaPicOrderAlwaysZeroFlag;
        public int FrameMbsOnlyFlag;
        public int Direct8X8InferenceFlag;
        /// <summary>NOTE: shall meet level 4.1 restrictions</summary>
        public int NumRefFrames;
        public byte ResidualColourTransformFlag;
        /// <summary>Must be 0 (only 8-bit supported)</summary>
        public byte BitDepthLumaMinus8;
        /// <summary>Must be 0 (only 8-bit supported)</summary>
        public byte BitDepthChromaMinus8;
        public byte QpprimeYZeroTransformBypassFlag;
        // PPS
        public int EntropyCodingModeFlag;
        public int PicOrderPresentFlag;
        public int NumRefIdxL0ActiveMinus1;
        public int NumRefIdxL1ActiveMinus1;
        public int WeightedPredFlag;
        public int WeightedBipredIdc;
        public int PicInitQpMinus26;
        public int DeblockingFilterControlPresentFlag;
        public int RedundantPicCntPresentFlag;
        public int Transform8X8ModeFlag;
        public int MbaffFrameFlag;
        public int ConstrainedIntraPredFlag;
        public int ChromaQpIndexOffset;
        public int SecondChromaQpIndexOffset;
        public int RefPicFlag;
        public int FrameNum;
        public fixed int CurrFieldOrderCnt[2];
        // DPB
        // List of reference frames within the DPB
        public CuVideoH264DpbEntry Dpb01;
        public CuVideoH264DpbEntry Dpb02;
        public CuVideoH264DpbEntry Dpb03;
        public CuVideoH264DpbEntry Dpb04;
        public CuVideoH264DpbEntry Dpb05;
        public CuVideoH264DpbEntry Dpb06;
        public CuVideoH264DpbEntry Dpb07;
        public CuVideoH264DpbEntry Dpb08;
        public CuVideoH264DpbEntry Dpb09;
        public CuVideoH264DpbEntry Dpb10;
        public CuVideoH264DpbEntry Dpb11;
        public CuVideoH264DpbEntry Dpb12;
        public CuVideoH264DpbEntry Dpb13;
        public CuVideoH264DpbEntry Dpb14;
        public CuVideoH264DpbEntry Dpb15;
        public CuVideoH264DpbEntry Dpb16;
        // Quantization Matrices (raster-order)
        public fixed byte WeightScale4X4[6 * 16];
        public fixed byte WeightScale8X8[2 * 64];
        // FMO/ASO
        public byte FmoAsoEnable;
        public byte NumSliceGroupsMinus1;
        public byte SliceGroupMapType;
        public sbyte PicInitQsMinus26;
        public uint SliceGroupChangeRateMinus1;

        public byte** Mb2SliceGroupMap;
        //union
        //{
        //    public ulong slice_group_map_addr;
        //    public byte* *pMb2SliceGroupMap;
        //}
        private fixed uint _reserved[12];
        // SVC/MVC

        public CuVideoH264PicParamsExt Ext;
    }

    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct CuVideoPicCodecParams
    {
        /// <summary>Also used for MPEG-1</summary>
        [FieldOffset(0)]
        public CuVideoMpeg2PicParams Mpeg2;
        [FieldOffset(0)]
        public CuVideoH264PicParams H264;
        [FieldOffset(0)]
        public CuVideoVC1PicParams VC1;
        [FieldOffset(0)]
        public CuVideoMpeg4PicParams Mpeg4;
        [FieldOffset(0)]
        public CuVideoJpegPicParams Jpeg;
        [FieldOffset(0)]
        public CuVideoHevcPicParams Hevc;
        [FieldOffset(0)]
        public CuVideoVP8PicParams VP8;
        [FieldOffset(0)]
        public CuVideoVP9PicParams VP9;
        [FieldOffset(0)]
        private fixed uint CodecReserved[1024];
    }

    /// <summary>
    /// \struct CUVIDPicParams
    /// Picture parameters for decoding
    /// This structure is used in cuvidDecodePicture API
    /// IN for cuvidDecodePicture
    ///</summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct CuVideoPicParams
    {
        /// <summary>IN: Coded frame size in macroblocks</summary>
        public int PicWidthInMbs;
        /// <summary>IN: Coded frame height in macroblocks</summary>
        public int FrameHeightInMbs;
        /// <summary>IN: Output index of the current picture</summary>
        public int CurrPicIdx;
        /// <summary>IN: 0=frame picture, 1=field picture</summary>
        public int FieldPicFlag;
        /// <summary>IN: 0=top field, 1=bottom field (ignored if field_pic_flag=0)</summary>
        public int BottomFieldFlag;
        /// <summary>IN: Second field of a complementary field pair</summary>
        public int SecondField;
        // Bitstream data
        /// <summary>IN: Number of bytes in bitstream data buffer</summary>
        public uint BitstreamDataLen;
        /// <summary>IN: Ptr to bitstream data for this picture (slice-layer)</summary>
        public IntPtr BitstreamData;
        /// <summary>IN: Number of slices in this picture</summary>
        public uint NumSlices;
        /// <summary>IN: nNumSlices entries, contains offset of each slice within the bitstream data buffer</summary>
        public IntPtr SliceDataOffsets;
        /// <summary>IN: This picture is a reference picture</summary>
        public int RefPicFlag;
        /// <summary>IN: This picture is entirely intra coded</summary>
        public int IntraPicFlag;
        /// <summary>Reserved for future use</summary>
        private fixed uint _reserved[30];
        /// <summary>IN: Codec-specific data</summary>
        public CuVideoPicCodecParams CodecParams;
    }

    /// <summary>
    /// \struct CUVIDPROCPARAMS
    /// Picture parameters for postprocessing
    /// This structure is used in cuvidMapVideoFrame API
    ///</summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct CuVideoProcParams
    {
        /// <summary>IN: Input is progressive (deinterlace_mode will be ignored)</summary>
        public bool ProgressiveFrame;
        /// <summary>IN: Output the second field (ignored if deinterlace mode is Weave)</summary>
        public int SecondField;
        /// <summary>IN: Input frame is top field first (1st field is top, 2nd field is bottom)</summary>
        public int TopFieldFirst;
        /// <summary>IN: Input only contains one field (2nd field is invalid)</summary>
        public int UnpairedField;
        // The fields below are used for raw YUV input
        /// <summary>Reserved for future use (set to zero)</summary>
        private uint _reservedFlags;
        /// <summary>Reserved (set to zero)</summary>
        private uint _reservedZero;
        /// <summary>IN: Input CUdeviceptr for raw YUV extensions</summary>
        public ulong RawInputDptr;
        /// <summary>IN: pitch in bytes of raw YUV input (should be aligned appropriately)</summary>
        public int RawInputPitch;
        /// <summary>IN: Input YUV format (cudaVideoCodec_enum)</summary>
        public CuVideoCodec RawInputFormat;
        /// <summary>IN: Output CUdeviceptr for raw YUV extensions</summary>
        public ulong RawOutputDptr;
        /// <summary>IN: pitch in bytes of raw YUV output (should be aligned appropriately)</summary>
        public int RawOutputPitch;
        /// <summary>Reserved for future use (set to zero)</summary>
        private uint _reserved1;
        /// <summary>IN: stream object used by cuvidMapVideoFrame</summary>
        //public CUstream OutputStream;
        // TODO: fix.
        public CuStream OutputStream;
        /// <summary>Reserved for future use (set to zero)</summary>
        private fixed uint _reserved[46];
        private IntPtr _reserved21;
        private IntPtr _reserved22;
    }

    /// <summary>
    /// \struct CUVIDGETDECODESTATUS
    /// Struct for reporting decode status.
    /// This structure is used in cuvidGetDecodeStatus API.
    ///</summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct CuVideoGetDecodeStatus
    {
        public CuVideoDecodeStatus DecodeStatus;
        private fixed uint reserved[31];
        private IntPtr _reserved1;
        private IntPtr _reserved2;
        private IntPtr _reserved3;
        private IntPtr _reserved4;
        private IntPtr _reserved5;
        private IntPtr _reserved6;
        private IntPtr _reserved7;
        private IntPtr _reserved8;
    }

    /// <summary>
    /// \struct CUVIDRECONFIGUREDECODERINFO
    /// Struct for decoder reset
    /// This structure is used in cuvidReconfigureDecoder() API
    ///</summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct CuVideoReconfigureDecoderInfo
    {
        /// <summary>IN: Coded sequence width in pixels, MUST be &lt;= ulMaxWidth defined at CUVIDDECODECREATEINFO</summary>
        public int Width;
        /// <summary>IN: Coded sequence height in pixels, MUST be &lt;= ulMaxHeight defined at CUVIDDECODECREATEINFO</summary>
        public int Height;
        /// <summary>IN: Post processed output width</summary>
        public int TargetWidth;
        /// <summary>IN: Post Processed output height</summary>
        public int TargetHeight;
        /// <summary>IN: Maximum number of internal decode surfaces</summary>
        public int NumDecodeSurfaces;
        /// <summary>Reserved for future use. Set to Zero</summary>
        private fixed uint _reserved1[12];
        /// <summary>* IN: Area of frame to be displayed. Use-case : Source Cropping</summary>
        public CuRectangleShort DisplayArea;
        /// <summary>IN: Target Rectangle in the OutputFrame. Use-case : Aspect ratio Conversion</summary>
        public CuRectangleShort TargetRect;
        /// <summary>Reserved for future use. Set to Zero</summary>
        private fixed uint _reserved2[11];
    }

    /// <summary>
    /// \struct CUVIDPARSERDISPINFO
    /// Used in cuvidParseVideoData API with PFNVIDDISPLAYCALLBACK pfnDisplayPicture
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct CuVideoParseDisplayInfo
    {
        /// <summary>OUT: Index of the current picture</summary>
        public int PictureIndex;
        /// <summary>OUT: 1 if progressive frame; 0 otherwise</summary>
        public bool ProgressiveFrame;
        /// <summary>OUT: 1 if top field is displayed first; 0 otherwise</summary>
        public int TopFieldFirst;
        /// <summary>OUT: Number of additional fields (1=ivtc, 2=frame doubling, 4=frame tripling, -1=unpaired field</summary>
        public int RepeatFirstField;
        /// <summary>OUT: Presentation time stamp</summary>
        public long Timestamp;

        /// Determine if <param name="infoPtr" /> is the final frame. If not,
        /// the CuVideoParseDisplayInfo is returned as <param name="info" />.
        public static bool IsFinalFrame(
            IntPtr infoPtr,
            out CuVideoParseDisplayInfo info)
        {
            if (infoPtr == IntPtr.Zero)
            {
                info = default;
                return true;
            }

            info = Marshal.PtrToStructure<CuVideoParseDisplayInfo>(infoPtr);

            return false;
        }
    }
}
