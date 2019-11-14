﻿using System;
using System.Runtime.InteropServices;

namespace Lennox.NvEncSharp
{
    /// <summary>NV_ENC_CODEC_CONFIG</summary>
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public unsafe struct NvEncCodecConfig
    {
        /// <summary>h264Config: [in]: Specifies the H.264-specific encoder configuration.</summary>
        [FieldOffset(0)]
        public NvEncConfigH264 H264Config;
        /// <summary>hevcConfig: [in]: Specifies the HEVC-specific encoder configuration.</summary>
        [FieldOffset(0)]
        public NvEncConfigHevc HevcConfig;
        /// <summary>h264MeOnlyConfig: [in]: Specifies the H.264-specific ME only encoder configuration.</summary>
        [FieldOffset(0)]
        public NvEncConfigH264Meonly H264MeOnlyConfig;
        /// <summary>hevcMeOnlyConfig: [in]: Specifies the HEVC-specific ME only encoder configuration.</summary>
        [FieldOffset(0)]
        public NvEncConfigHevcMeonly HevcMeOnlyConfig;
        /// <summary>reserved[320]: [in]: Reserved and must be set to 0</summary>
        [FieldOffset(0)]
        public fixed uint Reserved[320];
    }

    /// <summary>NV_ENC_PIC_PARAMS_H264_EXT</summary>
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public unsafe struct NvEncPicParamsH264Ext
    {
        /// <summary>mvcPicParams: [in]: Specifies the MVC picture parameters.</summary>
        [FieldOffset(0)]
        public NvEncPicParamsMvc MvcPicParams;
        /// <summary>reserved1[32]: [in]: Reserved and must be set to 0.</summary>
        [FieldOffset(0)]
        public fixed uint Reserved1[32];
    }

    /// <summary>NV_ENC_CODEC_PIC_PARAMS</summary>
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public unsafe struct NvEncCodecPicParams
    {
        /// <summary>h264PicParams: [in]: H264 encode picture params.</summary>
        [FieldOffset(0)]
        public NvEncPicParamsH264 H264PicParams;
        /// <summary>hevcPicParams: [in]: HEVC encode picture params.</summary>
        [FieldOffset(0)]
        public NvEncPicParamsHevc HevcPicParams;
        /// <summary>reserved[256]: [in]: Reserved and must be set to 0.</summary>
        [FieldOffset(0)]
        public fixed uint Reserved[256];
    }

    /// <summary>NVENC_RECT</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncRect
    {
        /// <summary>left: [in]: X coordinate of the upper left corner of rectangular area to be specified.</summary>
        public uint Left;
        /// <summary>top: [in]: Y coordinate of the upper left corner of the rectangular area to be specified.</summary>
        public uint Top;
        /// <summary>right: [in]: X coordinate of the bottom right corner of the rectangular area to be specified.</summary>
        public uint Right;
        /// <summary>bottom: [in]: Y coordinate of the bottom right corner of the rectangular area to be specified.</summary>
        public uint Bottom;
    }

    /// <summary>NV_ENC_CAPS_PARAM</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncCapsParam
    {
        /// <summary>version: [in]: Struct version. Must be set to ::NV_ENC_CAPS_PARAM_VER</summary>
        public uint Version;
        /// <summary>capsToQuery: [in]: Specifies the encode capability to be queried. Client should pass a member for ::NV_ENC_CAPS enum.</summary>
        public NvEncCaps CapsToQuery;
        /// <summary>reserved[62]: [in]: Reserved and must be set to 0</summary>
        public fixed uint Reserved[62];
    }

    /// <summary>NV_ENC_ENCODE_OUT_PARAMS</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncEncodeOutParams
    {
        /// <summary>version: [out]: Struct version.</summary>
        public uint Version;
        /// <summary>bitstreamSizeInBytes: [out]: Encoded bitstream size in bytes</summary>
        public uint BitstreamSizeInBytes;
        /// <summary>reserved[62]: [out]: Reserved and must be set to 0</summary>
        public fixed uint Reserved[62];
    }

    /// <summary>NV_ENC_CREATE_INPUT_BUFFER</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncCreateInputBuffer
    {
        /// <summary>version: [in]: Struct version. Must be set to ::NV_ENC_CREATE_INPUT_BUFFER_VER</summary>
        public uint Version;
        /// <summary>width: [in]: Input buffer width</summary>
        public uint Width;
        /// <summary>height: [in]: Input buffer width</summary>
        public uint Height;
        /// <summary>memoryHeap: [in]: Deprecated. Do not use</summary>
        public NvEncMemoryHeap MemoryHeap;
        /// <summary>bufferFmt: [in]: Input buffer format</summary>
        public NvEncBufferFormat BufferFmt;
        /// <summary>reserved: [in]: Reserved and must be set to 0</summary>
        public uint Reserved;
        /// <summary>inputBuffer: [out]: Pointer to input buffer</summary>
        public IntPtr InputBuffer;
        /// <summary>pSysMemBuffer: [in]: Pointer to existing sysmem buffer</summary>
        public IntPtr PSysMemBuffer;
        /// <summary>reserved1[57]: [in]: Reserved and must be set to 0</summary>
        public fixed uint Reserved1[57];
        /// <summary>reserved2[63]: [in]: Reserved and must be set to NULL</summary>

        public fixed ulong Reserved2[63];
    }

    /// <summary>NV_ENC_CREATE_BITSTREAM_BUFFER</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncCreateBitstreamBuffer
    {
        /// <summary>version: [in]: Struct version. Must be set to ::NV_ENC_CREATE_BITSTREAM_BUFFER_VER</summary>
        public uint Version;
        /// <summary>size: [in]: Deprecated. Do not use</summary>
        public uint Size;
        /// <summary>memoryHeap: [in]: Deprecated. Do not use</summary>
        public NvEncMemoryHeap MemoryHeap;
        /// <summary>reserved: [in]: Reserved and must be set to 0</summary>
        public uint Reserved;
        /// <summary>bitstreamBuffer: [out]: Pointer to the output bitstream buffer</summary>
        public IntPtr BitstreamBuffer;
        /// <summary>bitstreamBufferPtr: [out]: Reserved and should not be used</summary>
        public IntPtr BitstreamBufferPtr;
        /// <summary>reserved1[58]: [in]: Reserved and should be set to 0</summary>
        public fixed uint Reserved1[58];
        /// <summary>reserved2[64]: [in]: Reserved and should be set to NULL</summary>

        public fixed ulong Reserved2[64];
    }

    /// <summary>NV_ENC_MVECTOR</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncMvector
    {
        /// <summary>mvx: the x component of MV in qpel units</summary>
        public short Mvx;
        /// <summary>mvy: the y component of MV in qpel units</summary>
        public short Mvy;
    }

    /// <summary>NV_ENC_H264_MV_DATA</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncH264MvData
    {
        /// <summary>mv[4]: up to 4 vectors for 8x8 partition</summary>
        public NvEncMvector Mv0;
        public NvEncMvector Mv1;
        public NvEncMvector Mv2;
        public NvEncMvector Mv3;
        /// <summary>mbType: 0 (I), 1 (P), 2 (IPCM), 3 (B)</summary>
        public byte MbType;
        /// <summary>partitionType: Specifies the block partition type. 0:16x16, 1:8x8, 2:16x8, 3:8x16</summary>
        public byte PartitionType;
        /// <summary>reserved: reserved padding for alignment</summary>
        public ushort Reserved;
        /// <summary>mbCost</summary>
        public uint MbCost;
    }

    /// <summary>NV_ENC_HEVC_MV_DATA</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncHevcMvData
    {
        /// <summary>mv[4]: up to 4 vectors within a CU</summary>
        public NvEncMvector Mv0;
        public NvEncMvector Mv1;
        public NvEncMvector Mv2;
        public NvEncMvector Mv3;
        /// <summary>cuType: 0 (I), 1(P)</summary>
        public byte CuType;
        /// <summary>cuSize: 0: 8x8, 1: 16x16, 2: 32x32, 3: 64x64</summary>
        public byte CuSize;
        /// <summary>partitionMode: The CU partition mode
        ///  0 (2Nx2N), 1 (2NxN), 2(Nx2N), 3 (NxN),
        ///  4 (2NxnU), 5 (2NxnD), 6(nLx2N), 7 (nRx2N)</summary>
        public byte PartitionMode;
        /// <summary>lastCUInCTB: Marker to separate CUs in the current CTB from CUs in the next CTB</summary>
        public byte LastCUInCTB;
    }

    /// <summary>NV_ENC_CREATE_MV_BUFFER</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncCreateMvBuffer
    {
        /// <summary>version: [in]: Struct version. Must be set to NV_ENC_CREATE_MV_BUFFER_VER</summary>
        public uint Version;
        /// <summary>mvBuffer: [out]: Pointer to the output motion vector buffer</summary>
        public IntPtr MvBuffer;
        /// <summary>reserved1[255]: [in]: Reserved and should be set to 0</summary>
        public fixed uint Reserved1[255];
        /// <summary>reserved2[63]: [in]: Reserved and should be set to NULL</summary>

        public fixed ulong Reserved2[63];
    }

    /// <summary>NV_ENC_QP</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncQp
    {
        /// <summary>qpInterP: [in]: Specifies QP value for P-frame. Even though this field is uint32_t for legacy reasons, the client should treat this as a signed parameter(int32_t) for cases in which negative QP values are to be specified.</summary>
        public uint QpInterP;
        /// <summary>qpInterB: [in]: Specifies QP value for B-frame. Even though this field is uint32_t for legacy reasons, the client should treat this as a signed parameter(int32_t) for cases in which negative QP values are to be specified.</summary>
        public uint QpInterB;
        /// <summary>qpIntra: [in]: Specifies QP value for Intra Frame. Even though this field is uint32_t for legacy reasons, the client should treat this as a signed parameter(int32_t) for cases in which negative QP values are to be specified.</summary>
        public uint QpIntra;
    }

    /// <summary>NV_ENC_RC_PARAMS</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncRcParams
    {
        /// <summary>version</summary>
        public uint Version;
        /// <summary>rateControlMode: [in]: Specifies the rate control mode. Check support for various rate control modes using ::NV_ENC_CAPS_SUPPORTED_RATECONTROL_MODES caps.</summary>
        public NvEncParamsRcMode RateControlMode;
        /// <summary>constQP: [in]: Specifies the initial QP to be used for encoding, these values would be used for all frames if in Constant QP mode.</summary>
        public NvEncQp ConstQP;
        /// <summary>averageBitRate: [in]: Specifies the average bitrate(in bits/sec) used for encoding.</summary>
        public uint AverageBitRate;
        /// <summary>maxBitRate: [in]: Specifies the maximum bitrate for the encoded output. This is used for VBR and ignored for CBR mode.</summary>
        public uint MaxBitRate;
        /// <summary>vbvBufferSize: [in]: Specifies the VBV(HRD) buffer size. in bits. Set 0 to use the default VBV buffer size.</summary>
        public uint VbvBufferSize;
        /// <summary>vbvInitialDelay: [in]: Specifies the VBV(HRD) initial delay in bits. Set 0 to use the default VBV initial delay .</summary>
        public uint VbvInitialDelay;
        internal fixed byte BitField1[1];
        /// <summary>enableMinQP: [in]: Set this to 1 if minimum QP used for rate control.</summary>
        public bool EnableMinQP
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 0) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~1) | (((value ? 1 : 0) << 0) & 1));
            }
        }
        /// <summary>enableMaxQP: [in]: Set this to 1 if maximum QP used for rate control.</summary>
        public bool EnableMaxQP
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 1) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~2) | (((value ? 1 : 0) << 1) & 2));
            }
        }
        /// <summary>enableInitialRCQP: [in]: Set this to 1 if user suppplied initial QP is used for rate control.</summary>
        public bool EnableInitialRCQP
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 2) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~4) | (((value ? 1 : 0) << 2) & 4));
            }
        }
        /// <summary>enableAQ: [in]: Set this to 1 to enable adaptive quantization (Spatial).</summary>
        public bool EnableAQ
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 3) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~8) | (((value ? 1 : 0) << 3) & 8));
            }
        }
        /// <summary>reservedBitField1: [in]: Reserved bitfields and must be set to 0.</summary>
        /// <summary>enableLookahead: [in]: Set this to 1 to enable lookahead with depth <lookaheadDepth> (if lookahead is enabled, input frames must remain available to the encoder until encode completion)</summary>
        public bool EnableLookahead
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 4) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~16) | (((value ? 1 : 0) << 4) & 16));
            }
        }
        /// <summary>disableIadapt: [in]: Set this to 1 to disable adaptive I-frame insertion at scene cuts (only has an effect when lookahead is enabled)</summary>
        public bool DisableIadapt
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 5) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~32) | (((value ? 1 : 0) << 5) & 32));
            }
        }
        /// <summary>disableBadapt: [in]: Set this to 1 to disable adaptive B-frame decision (only has an effect when lookahead is enabled)</summary>
        public bool DisableBadapt
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 6) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~64) | (((value ? 1 : 0) << 6) & 64));
            }
        }
        /// <summary>enableTemporalAQ: [in]: Set this to 1 to enable temporal AQ</summary>
        public bool EnableTemporalAQ
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 7) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~128) | (((value ? 1 : 0) << 7) & 128));
            }
        }
        internal fixed byte BitField2[2];
        /// <summary>zeroReorderDelay: [in]: Set this to 1 to indicate zero latency operation (no reordering delay, num_reorder_frames=0)</summary>
        public bool ZeroReorderDelay
        {
            get { fixed (byte* ptr = &BitField2[0]) { return ((*(byte*)ptr >> 0) & 1) == 1; } }
            set
            {
                BitField2[0] = (byte)((BitField2[0] & ~1) | (((value ? 1 : 0) << 0) & 1));
            }
        }
        /// <summary>enableNonRefP: [in]: Set this to 1 to enable automatic insertion of non-reference P-frames (no effect if enablePTD=0)</summary>
        public bool EnableNonRefP
        {
            get { fixed (byte* ptr = &BitField2[0]) { return ((*(byte*)ptr >> 1) & 1) == 1; } }
            set
            {
                BitField2[0] = (byte)((BitField2[0] & ~2) | (((value ? 1 : 0) << 1) & 2));
            }
        }
        /// <summary>strictGOPTarget: [in]: Set this to 1 to minimize GOP-to-GOP rate fluctuations</summary>
        public bool StrictGOPTarget
        {
            get { fixed (byte* ptr = &BitField2[0]) { return ((*(byte*)ptr >> 2) & 1) == 1; } }
            set
            {
                BitField2[0] = (byte)((BitField2[0] & ~4) | (((value ? 1 : 0) << 2) & 4));
            }
        }
        /// <summary>aqStrength: [in]: When AQ (Spatial) is enabled (i.e. NV_ENC_RC_PARAMS::enableAQ is set), this field is used to specify AQ strength. AQ strength scale is from 1 (low) - 15 (aggressive). If not set, strength is autoselected by driver.</summary>
        public uint AqStrength
        {
            get { fixed (byte* ptr = &BitField2[0]) { return ((*(uint*)ptr >> 3) & 4); } }
            set
            {
                BitField2[0] = (byte)((BitField2[0] & ~32) | (((value) << 3) & 32));
            }
        }
        /// <summary>reservedBitFields: [in]: Reserved bitfields and must be set to 0</summary>
        /// <summary>minQP: [in]: Specifies the minimum QP used for rate control. Client must set NV_ENC_CONFIG::enableMinQP to 1.</summary>
        public NvEncQp MinQP;
        /// <summary>maxQP: [in]: Specifies the maximum QP used for rate control. Client must set NV_ENC_CONFIG::enableMaxQP to 1.</summary>
        public NvEncQp MaxQP;
        /// <summary>initialRCQP: [in]: Specifies the initial QP used for rate control. Client must set NV_ENC_CONFIG::enableInitialRCQP to 1.</summary>
        public NvEncQp InitialRCQP;
        /// <summary>temporallayerIdxMask: [in]: Specifies the temporal layers (as a bitmask) whose QPs have changed. Valid max bitmask is [2^NV_ENC_CAPS_NUM_MAX_TEMPORAL_LAYERS - 1]</summary>
        public uint TemporallayerIdxMask;
        /// <summary>temporalLayerQP[8]: [in]: Specifies the temporal layer QPs used for rate control. Temporal layer index is used as as the array index</summary>
        public fixed byte TemporalLayerQP[8];
        /// <summary>targetQuality: [in]: Target CQ (Constant Quality) level for VBR mode (range 0-51 with 0-automatic)</summary>
        public byte TargetQuality;
        /// <summary>targetQualityLSB: [in]: Fractional part of target quality (as 8.8 fixed point format)</summary>
        public byte TargetQualityLSB;
        /// <summary>lookaheadDepth: [in]: Maximum depth of lookahead with range 0-32 (only used if enableLookahead=1)</summary>
        public ushort LookaheadDepth;
        /// <summary>reserved1</summary>
        public uint Reserved1;
        /// <summary>qpMapMode: [in]: This flag is used to interpret values in array specified by NV_ENC_PIC_PARAMS::qpDeltaMap.
        ///  Set this to NV_ENC_QP_MAP_EMPHASIS to treat values specified by NV_ENC_PIC_PARAMS::qpDeltaMap as Emphasis Level Map.
        ///  Emphasis Level can be assigned any value specified in enum NV_ENC_EMPHASIS_MAP_LEVEL.
        ///  Emphasis Level Map is used to specify regions to be encoded at varying levels of quality.
        ///  The hardware encoder adjusts the quantization within the image as per the provided emphasis map,
        ///  by adjusting the quantization parameter (QP) assigned to each macroblock. This adjustment is commonly called “Delta QP”.
        ///  The adjustment depends on the absolute QP decided by the rate control algorithm, and is applied after the rate control has decided each macroblock’s QP.
        ///  Since the Delta QP overrides rate control, enabling Emphasis Level Map may violate bitrate and VBV buffer size constraints.
        ///  Emphasis Level Map is useful in situations where client has a priori knowledge of the image complexity (e.g. via use of NVFBC's Classification feature) and encoding those high-complexity areas at higher quality (lower QP) is important, even at the possible cost of violating bitrate/VBV buffer size constraints
        ///  This feature is not supported when AQ( Spatial/Temporal) is enabled.
        ///  This feature is only supported for H264 codec currently.
        ///
        ///  Set this to NV_ENC_QP_MAP_DELTA to treat values specified by NV_ENC_PIC_PARAMS::qpDeltaMap as QPDelta. This specifies QP modifier to be applied on top of the QP chosen by rate control
        ///
        ///  Set this to NV_ENC_QP_MAP_DISABLED to ignore NV_ENC_PIC_PARAMS::qpDeltaMap values. In this case, qpDeltaMap should be set to NULL.
        ///
        ///  Other values are reserved for future use.</summary>
        public NvEncQpMapMode QpMapMode;
        /// <summary>reserved[7]</summary>
        public fixed uint Reserved[7];
    }

    /// <summary>NVENC_EXTERNAL_ME_HINT_COUNTS_PER_BLOCKTYPE</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncExternalMeHintCountsPerBlocktype
    {
        internal fixed byte BitField1[1];
        /// <summary>numCandsPerBlk16x16: [in]: Supported for H264,HEVC.It Specifies the number of candidates per 16x16 block.</summary>
        public uint NumCandsPerBlk16x16
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(uint*)ptr >> 0) & 4); } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~4) | (((value) << 0) & 4));
            }
        }
        /// <summary>numCandsPerBlk16x8: [in]: Supported for H264 only.Specifies the number of candidates per 16x8 block.</summary>
        public uint NumCandsPerBlk16x8
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(uint*)ptr >> 4) & 4); } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~64) | (((value) << 4) & 64));
            }
        }
        internal fixed byte BitField2[1];
        /// <summary>numCandsPerBlk8x16: [in]: Supported for H264 only.Specifies the number of candidates per 8x16 block.</summary>
        public uint NumCandsPerBlk8x16
        {
            get { fixed (byte* ptr = &BitField2[0]) { return ((*(uint*)ptr >> 0) & 4); } }
            set
            {
                BitField2[0] = (byte)((BitField2[0] & ~4) | (((value) << 0) & 4));
            }
        }
        /// <summary>numCandsPerBlk8x8: [in]: Supported for H264,HEVC.Specifies the number of candidates per 8x8 block.</summary>
        public uint NumCandsPerBlk8x8
        {
            get { fixed (byte* ptr = &BitField2[0]) { return ((*(uint*)ptr >> 4) & 4); } }
            set
            {
                BitField2[0] = (byte)((BitField2[0] & ~64) | (((value) << 4) & 64));
            }
        }
        internal fixed byte BitField3[2];
        /// <summary>reserved: [in]: Reserved for padding.</summary>
        /// <summary>reserved1[3]: [in]: Reserved for future use.</summary>
        public fixed uint Reserved1[3];
    }

    /// <summary>NVENC_EXTERNAL_ME_HINT</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncExternalMeHint
    {
        internal fixed byte BitField1[4];
        /// <summary>mvx: [in]: Specifies the x component of integer pixel MV (relative to current MB) S12.0.</summary>
        public int Mvx
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(int*)ptr >> 0) & 12); } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~4) | (((value) << 0) & 4));
                BitField1[0] = (byte)((BitField1[0] & ~128) | (((value) << 4) & 128));
            }
        }
        /// <summary>mvy: [in]: Specifies the y component of integer pixel MV (relative to current MB) S10.0 .</summary>
        public int Mvy
        {
            get { fixed (byte* ptr = &BitField1[1]) { return ((*(int*)ptr >> 4) & 10); } }
            set
            {
                BitField1[1] = (byte)((BitField1[1] & ~8192) | (((value) << 4) & 32));
                BitField1[1] = (byte)((BitField1[1] & ~131072) | (((value) << 6) & 512));
            }
        }
        /// <summary>refidx: [in]: Specifies the reference index (31=invalid). Current we support only 1 reference frame per direction for external hints, so \p refidx must be 0.</summary>
        public int Refidx
        {
            get { fixed (byte* ptr = &BitField1[2]) { return ((*(int*)ptr >> 6) & 5); } }
            set
            {
                BitField1[2] = (byte)((BitField1[2] & ~20971520) | (((value) << 6) & 320));
            }
        }
        /// <summary>dir: [in]: Specifies the direction of motion estimation . 0=L0 1=L1.</summary>
        public bool Dir
        {
            get { fixed (byte* ptr = &BitField1[3]) { return ((*(byte*)ptr >> 3) & 1) == 1; } }
            set
            {
                BitField1[3] = (byte)((BitField1[3] & ~134217728) | (((value ? 1 : 0) << 3) & 8));
            }
        }
        /// <summary>partType: [in]: Specifies the block partition type.0=16x16 1=16x8 2=8x16 3=8x8 (blocks in partition must be consecutive).</summary>
        public int PartType
        {
            get { fixed (byte* ptr = &BitField1[3]) { return ((*(int*)ptr >> 4) & 2); } }
            set
            {
                BitField1[3] = (byte)((BitField1[3] & ~536870912) | (((value) << 4) & 32));
            }
        }
        /// <summary>lastofPart: [in]: Set to 1 for the last MV of (sub) partition</summary>
        public bool LastofPart
        {
            get { fixed (byte* ptr = &BitField1[3]) { return ((*(byte*)ptr >> 6) & 1) == 1; } }
            set
            {
                BitField1[3] = (byte)((BitField1[3] & ~1073741824) | (((value ? 1 : 0) << 6) & 64));
            }
        }
        /// <summary>lastOfMB: [in]: Set to 1 for the last MV of macroblock.</summary>
        public bool LastOfMB
        {
            get { fixed (byte* ptr = &BitField1[3]) { return ((*(byte*)ptr >> 7) & 1) == 1; } }
            set
            {
                BitField1[3] = (byte)((BitField1[3] & ~-2147483648) | (((value ? 1 : 0) << 7) & 128));
            }
        }
    }

    /// <summary>NV_ENC_CONFIG_H264</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncConfigH264
    {
        internal fixed byte BitField1[1];
        /// <summary>reserved: [in]: Reserved and must be set to 0</summary>
        /// <summary>enableStereoMVC: [in]: Set to 1 to enable stereo MVC</summary>
        public bool EnableStereoMVC
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 0) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~1) | (((value ? 1 : 0) << 0) & 1));
            }
        }
        /// <summary>hierarchicalPFrames: [in]: Set to 1 to enable hierarchical PFrames</summary>
        public bool HierarchicalPFrames
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 1) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~2) | (((value ? 1 : 0) << 1) & 2));
            }
        }
        /// <summary>hierarchicalBFrames: [in]: Set to 1 to enable hierarchical BFrames</summary>
        public bool HierarchicalBFrames
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 2) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~4) | (((value ? 1 : 0) << 2) & 4));
            }
        }
        /// <summary>outputBufferingPeriodSEI: [in]: Set to 1 to write SEI buffering period syntax in the bitstream</summary>
        public bool OutputBufferingPeriodSEI
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 3) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~8) | (((value ? 1 : 0) << 3) & 8));
            }
        }
        /// <summary>outputPictureTimingSEI: [in]: Set to 1 to write SEI picture timing syntax in the bitstream. When set for following rateControlMode : NV_ENC_PARAMS_RC_CBR, NV_ENC_PARAMS_RC_CBR_LOWDELAY_HQ,
        ///  NV_ENC_PARAMS_RC_CBR_HQ, filler data is inserted if needed to achieve hrd bitrate</summary>
        public bool OutputPictureTimingSEI
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 4) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~16) | (((value ? 1 : 0) << 4) & 16));
            }
        }
        /// <summary>outputAUD: [in]: Set to 1 to write access unit delimiter syntax in bitstream</summary>
        public bool OutputAUD
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 5) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~32) | (((value ? 1 : 0) << 5) & 32));
            }
        }
        /// <summary>disableSPSPPS: [in]: Set to 1 to disable writing of Sequence and Picture parameter info in bitstream</summary>
        public bool DisableSPSPPS
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 6) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~64) | (((value ? 1 : 0) << 6) & 64));
            }
        }
        /// <summary>outputFramePackingSEI: [in]: Set to 1 to enable writing of frame packing arrangement SEI messages to bitstream</summary>
        public bool OutputFramePackingSEI
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 7) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~128) | (((value ? 1 : 0) << 7) & 128));
            }
        }
        internal fixed byte BitField2[1];
        /// <summary>outputRecoveryPointSEI: [in]: Set to 1 to enable writing of recovery point SEI message</summary>
        public bool OutputRecoveryPointSEI
        {
            get { fixed (byte* ptr = &BitField2[0]) { return ((*(byte*)ptr >> 0) & 1) == 1; } }
            set
            {
                BitField2[0] = (byte)((BitField2[0] & ~1) | (((value ? 1 : 0) << 0) & 1));
            }
        }
        /// <summary>enableIntraRefresh: [in]: Set to 1 to enable gradual decoder refresh or intra refresh. If the GOP structure uses B frames this will be ignored</summary>
        public bool EnableIntraRefresh
        {
            get { fixed (byte* ptr = &BitField2[0]) { return ((*(byte*)ptr >> 1) & 1) == 1; } }
            set
            {
                BitField2[0] = (byte)((BitField2[0] & ~2) | (((value ? 1 : 0) << 1) & 2));
            }
        }
        /// <summary>enableConstrainedEncoding: [in]: Set this to 1 to enable constrainedFrame encoding where each slice in the constarined picture is independent of other slices
        ///  Check support for constrained encoding using ::NV_ENC_CAPS_SUPPORT_CONSTRAINED_ENCODING caps.</summary>
        public bool EnableConstrainedEncoding
        {
            get { fixed (byte* ptr = &BitField2[0]) { return ((*(byte*)ptr >> 2) & 1) == 1; } }
            set
            {
                BitField2[0] = (byte)((BitField2[0] & ~4) | (((value ? 1 : 0) << 2) & 4));
            }
        }
        /// <summary>repeatSPSPPS: [in]: Set to 1 to enable writing of Sequence and Picture parameter for every IDR frame</summary>
        public bool RepeatSPSPPS
        {
            get { fixed (byte* ptr = &BitField2[0]) { return ((*(byte*)ptr >> 3) & 1) == 1; } }
            set
            {
                BitField2[0] = (byte)((BitField2[0] & ~8) | (((value ? 1 : 0) << 3) & 8));
            }
        }
        /// <summary>enableVFR: [in]: Set to 1 to enable variable frame rate.</summary>
        public bool EnableVFR
        {
            get { fixed (byte* ptr = &BitField2[0]) { return ((*(byte*)ptr >> 4) & 1) == 1; } }
            set
            {
                BitField2[0] = (byte)((BitField2[0] & ~16) | (((value ? 1 : 0) << 4) & 16));
            }
        }
        /// <summary>enableLTR: [in]: Set to 1 to enable LTR (Long Term Reference) frame support. LTR can be used in two modes: "LTR Trust" mode and "LTR Per Picture" mode.
        ///  LTR Trust mode: In this mode, ltrNumFrames pictures after IDR are automatically marked as LTR. This mode is enabled by setting ltrTrustMode = 1.
        ///  Use of LTR Trust mode is strongly discouraged as this mode may be deprecated in future.
        ///  LTR Per Picture mode: In this mode, client can control whether the current picture should be marked as LTR. Enable this mode by setting
        ///  ltrTrustMode = 0 and ltrMarkFrame = 1 for the picture to be marked as LTR. This is the preferred mode
        ///  for using LTR.
        ///  Note that LTRs are not supported if encoding session is configured with B-frames</summary>
        public bool EnableLTR
        {
            get { fixed (byte* ptr = &BitField2[0]) { return ((*(byte*)ptr >> 5) & 1) == 1; } }
            set
            {
                BitField2[0] = (byte)((BitField2[0] & ~32) | (((value ? 1 : 0) << 5) & 32));
            }
        }
        /// <summary>qpPrimeYZeroTransformBypassFlag: [in]: To enable lossless encode set this to 1, set QP to 0 and RC_mode to NV_ENC_PARAMS_RC_CONSTQP and profile to HIGH_444_PREDICTIVE_PROFILE.
        ///  Check support for lossless encoding using ::NV_ENC_CAPS_SUPPORT_LOSSLESS_ENCODE caps.</summary>
        public bool QpPrimeYZeroTransformBypassFlag
        {
            get { fixed (byte* ptr = &BitField2[0]) { return ((*(byte*)ptr >> 6) & 1) == 1; } }
            set
            {
                BitField2[0] = (byte)((BitField2[0] & ~64) | (((value ? 1 : 0) << 6) & 64));
            }
        }
        /// <summary>useConstrainedIntraPred: [in]: Set 1 to enable constrained intra prediction.</summary>
        public bool UseConstrainedIntraPred
        {
            get { fixed (byte* ptr = &BitField2[0]) { return ((*(byte*)ptr >> 7) & 1) == 1; } }
            set
            {
                BitField2[0] = (byte)((BitField2[0] & ~128) | (((value ? 1 : 0) << 7) & 128));
            }
        }
        internal fixed byte BitField3[1];
        /// <summary>enableFillerDataInsertion: [in]: Set to 1 to enable insertion of filler data in the bitstream.
        ///  This flag will take effect only when one of the CBR rate
        ///  control modes (NV_ENC_PARAMS_RC_CBR, NV_ENC_PARAMS_RC_CBR_HQ,
        ///  NV_ENC_PARAMS_RC_CBR_LOWDELAY_HQ) is in use and both
        ///  NV_ENC_INITIALIZE_PARAMS::frameRateNum and
        ///  NV_ENC_INITIALIZE_PARAMS::frameRateDen are set to non-zero
        ///  values. Setting this field when
        ///  NV_ENC_INITIALIZE_PARAMS::enableOutputInVidmem is also set
        ///  is currently not supported and will make ::NvEncInitializeEncoder()
        ///  return an error.</summary>
        public bool EnableFillerDataInsertion
        {
            get { fixed (byte* ptr = &BitField3[0]) { return ((*(byte*)ptr >> 0) & 1) == 1; } }
            set
            {
                BitField3[0] = (byte)((BitField3[0] & ~1) | (((value ? 1 : 0) << 0) & 1));
            }
        }
        /// <summary>reservedBitFields: [in]: Reserved bitfields and must be set to 0</summary>
        /// <summary>level: [in]: Specifies the encoding level. Client is recommended to set this to NV_ENC_LEVEL_AUTOSELECT in order to enable the NvEncodeAPI interface to select the correct level.</summary>
        public uint Level;
        /// <summary>idrPeriod: [in]: Specifies the IDR interval. If not set, this is made equal to gopLength in NV_ENC_CONFIG.Low latency application client can set IDR interval to NVENC_INFINITE_GOPLENGTH so that IDR frames are not inserted automatically.</summary>
        public uint IdrPeriod;
        /// <summary>separateColourPlaneFlag: [in]: Set to 1 to enable 4:4:4 separate colour planes</summary>
        public uint SeparateColourPlaneFlag;
        /// <summary>disableDeblockingFilterIDC: [in]: Specifies the deblocking filter mode. Permissible value range: [0,2]</summary>
        public uint DisableDeblockingFilterIDC;
        /// <summary>numTemporalLayers: [in]: Specifies max temporal layers to be used for hierarchical coding. Valid value range is [1,::NV_ENC_CAPS_NUM_MAX_TEMPORAL_LAYERS]</summary>
        public uint NumTemporalLayers;
        /// <summary>spsId: [in]: Specifies the SPS id of the sequence header</summary>
        public uint SpsId;
        /// <summary>ppsId: [in]: Specifies the PPS id of the picture header</summary>
        public uint PpsId;
        /// <summary>adaptiveTransformMode: [in]: Specifies the AdaptiveTransform Mode. Check support for AdaptiveTransform mode using ::NV_ENC_CAPS_SUPPORT_ADAPTIVE_TRANSFORM caps.</summary>
        public NvEncH264AdaptiveTransformMode AdaptiveTransformMode;
        /// <summary>fmoMode: [in]: Specified the FMO Mode. Check support for FMO using ::NV_ENC_CAPS_SUPPORT_FMO caps.</summary>
        public NvEncH264FmoMode FmoMode;
        /// <summary>bdirectMode: [in]: Specifies the BDirect mode. Check support for BDirect mode using ::NV_ENC_CAPS_SUPPORT_BDIRECT_MODE caps.</summary>
        public NvEncH264BdirectMode BdirectMode;
        /// <summary>entropyCodingMode: [in]: Specifies the entropy coding mode. Check support for CABAC mode using ::NV_ENC_CAPS_SUPPORT_CABAC caps.</summary>
        public NvEncH264EntropyCodingMode EntropyCodingMode;
        /// <summary>stereoMode: [in]: Specifies the stereo frame packing mode which is to be signalled in frame packing arrangement SEI</summary>
        public NvEncStereoPackingMode StereoMode;
        /// <summary>intraRefreshPeriod: [in]: Specifies the interval between successive intra refresh if enableIntrarefresh is set. Requires enableIntraRefresh to be set.
        ///  Will be disabled if NV_ENC_CONFIG::gopLength is not set to NVENC_INFINITE_GOPLENGTH.</summary>
        public uint IntraRefreshPeriod;
        /// <summary>intraRefreshCnt: [in]: Specifies the length of intra refresh in number of frames for periodic intra refresh. This value should be smaller than intraRefreshPeriod</summary>
        public uint IntraRefreshCnt;
        /// <summary>maxNumRefFrames: [in]: Specifies the DPB size used for encoding. Setting it to 0 will let driver use the default dpb size.
        ///  The low latency application which wants to invalidate reference frame as an error resilience tool
        ///  is recommended to use a large DPB size so that the encoder can keep old reference frames which can be used if recent
        ///  frames are invalidated.</summary>
        public uint MaxNumRefFrames;
        /// <summary>sliceMode: [in]: This parameter in conjunction with sliceModeData specifies the way in which the picture is divided into slices
        ///  sliceMode = 0 MB based slices, sliceMode = 1 Byte based slices, sliceMode = 2 MB row based slices, sliceMode = 3 numSlices in Picture.
        ///  When forceIntraRefreshWithFrameCnt is set it will have priority over sliceMode setting
        ///  When sliceMode == 0 and sliceModeData == 0 whole picture will be coded with one slice</summary>
        public uint SliceMode;
        /// <summary>sliceModeData: [in]: Specifies the parameter needed for sliceMode. For:
        ///  sliceMode = 0, sliceModeData specifies # of MBs in each slice (except last slice)
        ///  sliceMode = 1, sliceModeData specifies maximum # of bytes in each slice (except last slice)
        ///  sliceMode = 2, sliceModeData specifies # of MB rows in each slice (except last slice)
        ///  sliceMode = 3, sliceModeData specifies number of slices in the picture. Driver will divide picture into slices optimally</summary>
        public uint SliceModeData;
        /// <summary>h264VUIParameters: [in]: Specifies the H264 video usability info pamameters</summary>
        /// <summary>ltrNumFrames: [in]: Specifies the number of LTR frames. This parameter has different meaning in two LTR modes.
        ///  In "LTR Trust" mode (ltrTrustMode = 1), encoder will mark the first ltrNumFrames base layer reference frames within each IDR interval as LTR.
        ///  In "LTR Per Picture" mode (ltrTrustMode = 0 and ltrMarkFrame = 1), ltrNumFrames specifies maximum number of LTR frames in DPB.</summary>
        public uint LtrNumFrames;
        /// <summary>ltrTrustMode: [in]: Specifies the LTR operating mode. See comments near NV_ENC_CONFIG_H264::enableLTR for description of the two modes.
        ///  Set to 1 to use "LTR Trust" mode of LTR operation. Clients are discouraged to use "LTR Trust" mode as this mode may
        ///  be deprecated in future releases.
        ///  Set to 0 when using "LTR Per Picture" mode of LTR operation.</summary>
        public uint LtrTrustMode;
        /// <summary>chromaFormatIDC: [in]: Specifies the chroma format. Should be set to 1 for yuv420 input, 3 for yuv444 input.
        ///  Check support for YUV444 encoding using ::NV_ENC_CAPS_SUPPORT_YUV444_ENCODE caps.</summary>
        public uint ChromaFormatIDC;
        /// <summary>maxTemporalLayers: [in]: Specifies the max temporal layer used for hierarchical coding.</summary>
        public uint MaxTemporalLayers;
        /// <summary>useBFramesAsRef: [in]: Specifies the B-Frame as reference mode. Check support for useBFramesAsRef mode using ::NV_ENC_CAPS_SUPPORT_BFRAME_REF_MODE caps.</summary>
        public NvEncBframeRefMode UseBFramesAsRef;
        /// <summary>numRefL0: [in]: Specifies max number of reference frames in reference picture list L0, that can be used by hardware for prediction of a frame.
        ///  Check support for numRefL0 using ::NV_ENC_CAPS_SUPPORT_MULTIPLE_REF_FRAMES caps.</summary>
        public NvEncNumRefFrames NumRefL0;
        /// <summary>numRefL1: [in]: Specifies max number of reference frames in reference picture list L1, that can be used by hardware for prediction of a frame.
        ///  Check support for numRefL1 using ::NV_ENC_CAPS_SUPPORT_MULTIPLE_REF_FRAMES caps.</summary>
        public NvEncNumRefFrames NumRefL1;
        /// <summary>reserved1[267]: [in]: Reserved and must be set to 0</summary>
        public fixed uint Reserved1[267];
        /// <summary>reserved2[64]: [in]: Reserved and must be set to NULL</summary>

        public fixed ulong Reserved2[64];
    }

    /// <summary>NV_ENC_CONFIG_HEVC</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncConfigHevc
    {
        /// <summary>level: [in]: Specifies the level of the encoded bitstream.</summary>
        public uint Level;
        /// <summary>tier: [in]: Specifies the level tier of the encoded bitstream.</summary>
        public uint Tier;
        /// <summary>minCUSize: [in]: Specifies the minimum size of luma coding unit.</summary>
        public NvEncHevcCusize MinCUSize;
        /// <summary>maxCUSize: [in]: Specifies the maximum size of luma coding unit. Currently NVENC SDK only supports maxCUSize equal to NV_ENC_HEVC_CUSIZE_32x32.</summary>
        public NvEncHevcCusize MaxCUSize;
        internal fixed byte BitField1[1];
        /// <summary>useConstrainedIntraPred: [in]: Set 1 to enable constrained intra prediction.</summary>
        public bool UseConstrainedIntraPred
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 0) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~1) | (((value ? 1 : 0) << 0) & 1));
            }
        }
        /// <summary>disableDeblockAcrossSliceBoundary: [in]: Set 1 to disable in loop filtering across slice boundary.</summary>
        public bool DisableDeblockAcrossSliceBoundary
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 1) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~2) | (((value ? 1 : 0) << 1) & 2));
            }
        }
        /// <summary>outputBufferingPeriodSEI: [in]: Set 1 to write SEI buffering period syntax in the bitstream</summary>
        public bool OutputBufferingPeriodSEI
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 2) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~4) | (((value ? 1 : 0) << 2) & 4));
            }
        }
        /// <summary>outputPictureTimingSEI: [in]: Set 1 to write SEI picture timing syntax in the bitstream</summary>
        public bool OutputPictureTimingSEI
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 3) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~8) | (((value ? 1 : 0) << 3) & 8));
            }
        }
        /// <summary>outputAUD: [in]: Set 1 to write Access Unit Delimiter syntax.</summary>
        public bool OutputAUD
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 4) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~16) | (((value ? 1 : 0) << 4) & 16));
            }
        }
        /// <summary>enableLTR: [in]: Set to 1 to enable LTR (Long Term Reference) frame support. LTR can be used in two modes: "LTR Trust" mode and "LTR Per Picture" mode.
        ///  LTR Trust mode: In this mode, ltrNumFrames pictures after IDR are automatically marked as LTR. This mode is enabled by setting ltrTrustMode = 1.
        ///  Use of LTR Trust mode is strongly discouraged as this mode may be deprecated in future releases.
        ///  LTR Per Picture mode: In this mode, client can control whether the current picture should be marked as LTR. Enable this mode by setting
        ///  ltrTrustMode = 0 and ltrMarkFrame = 1 for the picture to be marked as LTR. This is the preferred mode
        ///  for using LTR.
        ///  Note that LTRs are not supported if encoding session is configured with B-frames</summary>
        public bool EnableLTR
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 5) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~32) | (((value ? 1 : 0) << 5) & 32));
            }
        }
        /// <summary>disableSPSPPS: [in]: Set 1 to disable VPS,SPS and PPS signalling in the bitstream.</summary>
        public bool DisableSPSPPS
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 6) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~64) | (((value ? 1 : 0) << 6) & 64));
            }
        }
        /// <summary>repeatSPSPPS: [in]: Set 1 to output VPS,SPS and PPS for every IDR frame.</summary>
        public bool RepeatSPSPPS
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 7) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~128) | (((value ? 1 : 0) << 7) & 128));
            }
        }
        internal fixed byte BitField2[3];
        /// <summary>enableIntraRefresh: [in]: Set 1 to enable gradual decoder refresh or intra refresh. If the GOP structure uses B frames this will be ignored</summary>
        public bool EnableIntraRefresh
        {
            get { fixed (byte* ptr = &BitField2[0]) { return ((*(byte*)ptr >> 0) & 1) == 1; } }
            set
            {
                BitField2[0] = (byte)((BitField2[0] & ~1) | (((value ? 1 : 0) << 0) & 1));
            }
        }
        /// <summary>chromaFormatIDC: [in]: Specifies the chroma format. Should be set to 1 for yuv420 input, 3 for yuv444 input.</summary>
        public uint ChromaFormatIDC
        {
            get { fixed (byte* ptr = &BitField2[0]) { return ((*(uint*)ptr >> 1) & 2); } }
            set
            {
                BitField2[0] = (byte)((BitField2[0] & ~4) | (((value) << 1) & 4));
            }
        }
        /// <summary>pixelBitDepthMinus8: [in]: Specifies pixel bit depth minus 8. Should be set to 0 for 8 bit input, 2 for 10 bit input.</summary>
        public uint PixelBitDepthMinus8
        {
            get { fixed (byte* ptr = &BitField2[0]) { return ((*(uint*)ptr >> 3) & 3); } }
            set
            {
                BitField2[0] = (byte)((BitField2[0] & ~24) | (((value) << 3) & 24));
            }
        }
        /// <summary>enableFillerDataInsertion: [in]: Set to 1 to enable insertion of filler data in the bitstream.
        ///  This flag will take effect only when one of the CBR rate
        ///  control modes (NV_ENC_PARAMS_RC_CBR, NV_ENC_PARAMS_RC_CBR_HQ,
        ///  NV_ENC_PARAMS_RC_CBR_LOWDELAY_HQ) is in use and both
        ///  NV_ENC_INITIALIZE_PARAMS::frameRateNum and
        ///  NV_ENC_INITIALIZE_PARAMS::frameRateDen are set to non-zero
        ///  values. Setting this field when
        ///  NV_ENC_INITIALIZE_PARAMS::enableOutputInVidmem is also set
        ///  is currently not supported and will make ::NvEncInitializeEncoder()
        ///  return an error.</summary>
        public bool EnableFillerDataInsertion
        {
            get { fixed (byte* ptr = &BitField2[0]) { return ((*(byte*)ptr >> 6) & 1) == 1; } }
            set
            {
                BitField2[0] = (byte)((BitField2[0] & ~64) | (((value ? 1 : 0) << 6) & 64));
            }
        }
        /// <summary>reserved: [in]: Reserved bitfields.</summary>
        /// <summary>idrPeriod: [in]: Specifies the IDR interval. If not set, this is made equal to gopLength in NV_ENC_CONFIG.Low latency application client can set IDR interval to NVENC_INFINITE_GOPLENGTH so that IDR frames are not inserted automatically.</summary>
        public uint IdrPeriod;
        /// <summary>intraRefreshPeriod: [in]: Specifies the interval between successive intra refresh if enableIntrarefresh is set. Requires enableIntraRefresh to be set.
        ///  Will be disabled if NV_ENC_CONFIG::gopLength is not set to NVENC_INFINITE_GOPLENGTH.</summary>
        public uint IntraRefreshPeriod;
        /// <summary>intraRefreshCnt: [in]: Specifies the length of intra refresh in number of frames for periodic intra refresh. This value should be smaller than intraRefreshPeriod</summary>
        public uint IntraRefreshCnt;
        /// <summary>maxNumRefFramesInDPB: [in]: Specifies the maximum number of references frames in the DPB.</summary>
        public uint MaxNumRefFramesInDPB;
        /// <summary>ltrNumFrames: [in]: This parameter has different meaning in two LTR modes.
        ///  In "LTR Trust" mode (ltrTrustMode = 1), encoder will mark the first ltrNumFrames base layer reference frames within each IDR interval as LTR.
        ///  In "LTR Per Picture" mode (ltrTrustMode = 0 and ltrMarkFrame = 1), ltrNumFrames specifies maximum number of LTR frames in DPB.</summary>
        public uint LtrNumFrames;
        /// <summary>vpsId: [in]: Specifies the VPS id of the video parameter set</summary>
        public uint VpsId;
        /// <summary>spsId: [in]: Specifies the SPS id of the sequence header</summary>
        public uint SpsId;
        /// <summary>ppsId: [in]: Specifies the PPS id of the picture header</summary>
        public uint PpsId;
        /// <summary>sliceMode: [in]: This parameter in conjunction with sliceModeData specifies the way in which the picture is divided into slices
        ///  sliceMode = 0 CTU based slices, sliceMode = 1 Byte based slices, sliceMode = 2 CTU row based slices, sliceMode = 3, numSlices in Picture
        ///  When sliceMode == 0 and sliceModeData == 0 whole picture will be coded with one slice</summary>
        public uint SliceMode;
        /// <summary>sliceModeData: [in]: Specifies the parameter needed for sliceMode. For:
        ///  sliceMode = 0, sliceModeData specifies # of CTUs in each slice (except last slice)
        ///  sliceMode = 1, sliceModeData specifies maximum # of bytes in each slice (except last slice)
        ///  sliceMode = 2, sliceModeData specifies # of CTU rows in each slice (except last slice)
        ///  sliceMode = 3, sliceModeData specifies number of slices in the picture. Driver will divide picture into slices optimally</summary>
        public uint SliceModeData;
        /// <summary>maxTemporalLayersMinus1: [in]: Specifies the max temporal layer used for hierarchical coding.</summary>
        public uint MaxTemporalLayersMinus1;
        /// <summary>hevcVUIParameters: [in]: Specifies the HEVC video usability info pamameters</summary>
        /// <summary>ltrTrustMode: [in]: Specifies the LTR operating mode. See comments near NV_ENC_CONFIG_HEVC::enableLTR for description of the two modes.
        ///  Set to 1 to use "LTR Trust" mode of LTR operation. Clients are discouraged to use "LTR Trust" mode as this mode may
        ///  be deprecated in future releases.
        ///  Set to 0 when using "LTR Per Picture" mode of LTR operation.</summary>
        public uint LtrTrustMode;
        /// <summary>useBFramesAsRef: [in]: Specifies the B-Frame as reference mode. Check support for useBFramesAsRef mode using ::NV_ENC_CAPS_SUPPORT_BFRAME_REF_MODE caps.</summary>
        public NvEncBframeRefMode UseBFramesAsRef;
        /// <summary>numRefL0: [in]: Specifies max number of reference frames in reference picture list L0, that can be used by hardware for prediction of a frame.
        ///  Check support for numRefL0 using ::NV_ENC_CAPS_SUPPORT_MULTIPLE_REF_FRAMES caps.</summary>
        public NvEncNumRefFrames NumRefL0;
        /// <summary>numRefL1: [in]: Specifies max number of reference frames in reference picture list L1, that can be used by hardware for prediction of a frame.
        ///  Check support for numRefL1 using ::NV_ENC_CAPS_SUPPORT_MULTIPLE_REF_FRAMES caps.</summary>
        public NvEncNumRefFrames NumRefL1;
        /// <summary>reserved1[214]: [in]: Reserved and must be set to 0.</summary>
        public fixed uint Reserved1[214];
        /// <summary>reserved2[64]: [in]: Reserved and must be set to NULL</summary>

        public fixed ulong Reserved2[64];
    }

    /// <summary>NV_ENC_CONFIG_H264_MEONLY</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncConfigH264Meonly
    {
        internal fixed byte BitField1[4];
        /// <summary>disablePartition16x16: [in]: Disable MotionEstimation on 16x16 blocks</summary>
        public bool DisablePartition16x16
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 0) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~1) | (((value ? 1 : 0) << 0) & 1));
            }
        }
        /// <summary>disablePartition8x16: [in]: Disable MotionEstimation on 8x16 blocks</summary>
        public bool DisablePartition8x16
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 1) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~2) | (((value ? 1 : 0) << 1) & 2));
            }
        }
        /// <summary>disablePartition16x8: [in]: Disable MotionEstimation on 16x8 blocks</summary>
        public bool DisablePartition16x8
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 2) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~4) | (((value ? 1 : 0) << 2) & 4));
            }
        }
        /// <summary>disablePartition8x8: [in]: Disable MotionEstimation on 8x8 blocks</summary>
        public bool DisablePartition8x8
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 3) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~8) | (((value ? 1 : 0) << 3) & 8));
            }
        }
        /// <summary>disableIntraSearch: [in]: Disable Intra search during MotionEstimation</summary>
        public bool DisableIntraSearch
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 4) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~16) | (((value ? 1 : 0) << 4) & 16));
            }
        }
        /// <summary>bStereoEnable: [in]: Enable Stereo Mode for Motion Estimation where each view is independently executed</summary>
        public bool BStereoEnable
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 5) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~32) | (((value ? 1 : 0) << 5) & 32));
            }
        }
        /// <summary>reserved: [in]: Reserved and must be set to 0</summary>
        /// <summary>reserved1 [255]: [in]: Reserved and must be set to 0</summary>
        public fixed uint Reserved1[255];
        /// <summary>reserved2[64]: [in]: Reserved and must be set to NULL</summary>

        public fixed ulong Reserved2[64];
    }

    /// <summary>NV_ENC_CONFIG_HEVC_MEONLY</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncConfigHevcMeonly
    {
        /// <summary>reserved [256]: [in]: Reserved and must be set to 0</summary>
        public fixed uint Reserved[256];
        /// <summary>reserved1[64]: [in]: Reserved and must be set to NULL</summary>

        public fixed ulong Reserved1[64];
    }

    /// <summary>NV_ENC_CONFIG</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncConfig
    {
        /// <summary>version: [in]: Struct version. Must be set to ::NV_ENC_CONFIG_VER.</summary>
        public uint Version;
        /// <summary>profileGUID: [in]: Specifies the codec profile guid. If client specifies \p NV_ENC_CODEC_PROFILE_AUTOSELECT_GUID the NvEncodeAPI interface will select the appropriate codec profile.</summary>
        public Guid ProfileGUID;
        /// <summary>gopLength: [in]: Specifies the number of pictures in one GOP. Low latency application client can set goplength to NVENC_INFINITE_GOPLENGTH so that keyframes are not inserted automatically.</summary>
        public uint GopLength;
        /// <summary>frameIntervalP: [in]: Specifies the GOP pattern as follows: \p frameIntervalP = 0: I, 1: IPP, 2: IBP, 3: IBBP If goplength is set to NVENC_INFINITE_GOPLENGTH \p frameIntervalP should be set to 1.</summary>
        public int FrameIntervalP;
        /// <summary>monoChromeEncoding: [in]: Set this to 1 to enable monochrome encoding for this session.</summary>
        public uint MonoChromeEncoding;
        /// <summary>frameFieldMode: [in]: Specifies the frame/field mode.
        ///  Check support for field encoding using ::NV_ENC_CAPS_SUPPORT_FIELD_ENCODING caps.
        ///  Using a frameFieldMode other than NV_ENC_PARAMS_FRAME_FIELD_MODE_FRAME for RGB input is not supported.</summary>
        public NvEncParamsFrameFieldMode FrameFieldMode;
        /// <summary>mvPrecision: [in]: Specifies the desired motion vector prediction precision.</summary>
        public NvEncMvPrecision MvPrecision;
        /// <summary>rcParams: [in]: Specifies the rate control parameters for the current encoding session.</summary>
        public NvEncRcParams RcParams;
        /// <summary>encodeCodecConfig: [in]: Specifies the codec specific config parameters through this union.</summary>
        public NvEncCodecConfig EncodeCodecConfig;
        /// <summary>reserved [278]: [in]: Reserved and must be set to 0</summary>
        public fixed uint Reserved[278];
        /// <summary>reserved2[64]: [in]: Reserved and must be set to NULL</summary>

        public fixed ulong Reserved2[64];
    }

    /// <summary>NV_ENC_INITIALIZE_PARAMS</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncInitializeParams
    {
        /// <summary>version: [in]: Struct version. Must be set to ::NV_ENC_INITIALIZE_PARAMS_VER.</summary>
        public uint Version;
        /// <summary>encodeGUID: [in]: Specifies the Encode GUID for which the encoder is being created. ::NvEncInitializeEncoder() API will fail if this is not set, or set to unsupported value.</summary>
        public Guid EncodeGUID;
        /// <summary>presetGUID: [in]: Specifies the preset for encoding. If the preset GUID is set then , the preset configuration will be applied before any other parameter.</summary>
        public Guid PresetGUID;
        /// <summary>encodeWidth: [in]: Specifies the encode width. If not set ::NvEncInitializeEncoder() API will fail.</summary>
        public uint EncodeWidth;
        /// <summary>encodeHeight: [in]: Specifies the encode height. If not set ::NvEncInitializeEncoder() API will fail.</summary>
        public uint EncodeHeight;
        /// <summary>darWidth: [in]: Specifies the display aspect ratio Width.</summary>
        public uint DarWidth;
        /// <summary>darHeight: [in]: Specifies the display aspect ratio height.</summary>
        public uint DarHeight;
        /// <summary>frameRateNum: [in]: Specifies the numerator for frame rate used for encoding in frames per second ( Frame rate = frameRateNum / frameRateDen ).</summary>
        public uint FrameRateNum;
        /// <summary>frameRateDen: [in]: Specifies the denominator for frame rate used for encoding in frames per second ( Frame rate = frameRateNum / frameRateDen ).</summary>
        public uint FrameRateDen;
        /// <summary>enableEncodeAsync: [in]: Set this to 1 to enable asynchronous mode and is expected to use events to get picture completion notification.</summary>
        public uint EnableEncodeAsync;
        /// <summary>enablePTD: [in]: Set this to 1 to enable the Picture Type Decision is be taken by the NvEncodeAPI interface.</summary>
        public uint EnablePTD;
        internal fixed byte BitField1[4];
        /// <summary>reportSliceOffsets: [in]: Set this to 1 to enable reporting slice offsets in ::_NV_ENC_LOCK_BITSTREAM. NV_ENC_INITIALIZE_PARAMS::enableEncodeAsync must be set to 0 to use this feature. Client must set this to 0 if NV_ENC_CONFIG_H264::sliceMode is 1 on Kepler GPUs</summary>
        public bool ReportSliceOffsets
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 0) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~1) | (((value ? 1 : 0) << 0) & 1));
            }
        }
        /// <summary>enableSubFrameWrite: [in]: Set this to 1 to write out available bitstream to memory at subframe intervals</summary>
        public bool EnableSubFrameWrite
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 1) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~2) | (((value ? 1 : 0) << 1) & 2));
            }
        }
        /// <summary>enableExternalMEHints: [in]: Set to 1 to enable external ME hints for the current frame. For NV_ENC_INITIALIZE_PARAMS::enablePTD=1 with B frames, programming L1 hints is optional for B frames since Client doesn't know internal GOP structure.
        ///  NV_ENC_PIC_PARAMS::meHintRefPicDist should preferably be set with enablePTD=1.</summary>
        public bool EnableExternalMEHints
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 2) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~4) | (((value ? 1 : 0) << 2) & 4));
            }
        }
        /// <summary>enableMEOnlyMode: [in]: Set to 1 to enable ME Only Mode .</summary>
        public bool EnableMEOnlyMode
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 3) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~8) | (((value ? 1 : 0) << 3) & 8));
            }
        }
        /// <summary>enableWeightedPrediction: [in]: Set this to 1 to enable weighted prediction. Not supported if encode session is configured for B-Frames( 'frameIntervalP' in NV_ENC_CONFIG is greater than 1).</summary>
        public bool EnableWeightedPrediction
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 4) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~16) | (((value ? 1 : 0) << 4) & 16));
            }
        }
        /// <summary>enableOutputInVidmem: [in]: Set this to 1 to enable output of NVENC in video memory buffer created by application. This feature is not supported for HEVC ME only mode.</summary>
        public bool EnableOutputInVidmem
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 5) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~32) | (((value ? 1 : 0) << 5) & 32));
            }
        }
        /// <summary>reservedBitFields: [in]: Reserved bitfields and must be set to 0</summary>
        /// <summary>privDataSize: [in]: Reserved private data buffer size and must be set to 0</summary>
        public uint PrivDataSize;
        /// <summary>privData: [in]: Reserved private data buffer and must be set to NULL</summary>
        public IntPtr PrivData;
        /// <summary>encodeConfig: [in]: Specifies the advanced codec specific structure. If client has sent a valid codec config structure, it will override parameters set by the NV_ENC_INITIALIZE_PARAMS::presetGUID parameter. If set to NULL the NvEncodeAPI interface will use the NV_ENC_INITIALIZE_PARAMS::presetGUID to set the codec specific parameters.
        ///  Client can also optionally query the NvEncodeAPI interface to get codec specific parameters for a presetGUID using ::NvEncGetEncodePresetConfig() API. It can then modify (if required) some of the codec config parameters and send down a custom config structure as part of ::_NV_ENC_INITIALIZE_PARAMS.
        ///  Even in this case client is recommended to pass the same preset guid it has used in ::NvEncGetEncodePresetConfig() API to query the config structure; as NV_ENC_INITIALIZE_PARAMS::presetGUID. This will not override the custom config structure but will be used to determine other Encoder HW specific parameters not exposed in the API.</summary>
        public NvEncConfig* EncodeConfig;
        /// <summary>maxEncodeWidth: [in]: Maximum encode width to be used for current Encode session.
        ///  Client should allocate output buffers according to this dimension for dynamic resolution change. If set to 0, Encoder will not allow dynamic resolution change.</summary>
        public uint MaxEncodeWidth;
        /// <summary>maxEncodeHeight: [in]: Maximum encode height to be allowed for current Encode session.
        ///  Client should allocate output buffers according to this dimension for dynamic resolution change. If set to 0, Encode will not allow dynamic resolution change.</summary>
        public uint MaxEncodeHeight;
        /// <summary>maxMEHintCountsPerBlock[2]: [in]: If Client wants to pass external motion vectors in NV_ENC_PIC_PARAMS::meExternalHints buffer it must specify the maximum number of hint candidates per block per direction for the encode session.
        ///  The NV_ENC_INITIALIZE_PARAMS::maxMEHintCountsPerBlock[0] is for L0 predictors and NV_ENC_INITIALIZE_PARAMS::maxMEHintCountsPerBlock[1] is for L1 predictors.
        ///  This client must also set NV_ENC_INITIALIZE_PARAMS::enableExternalMEHints to 1.</summary>
        public NvEncExternalMeHintCountsPerBlocktype MaxMEHintCountsPerBlock0;
        public NvEncExternalMeHintCountsPerBlocktype MaxMEHintCountsPerBlock1;
        /// <summary>reserved [289]: [in]: Reserved and must be set to 0</summary>
        public fixed uint Reserved[289];
        /// <summary>reserved2[64]: [in]: Reserved and must be set to NULL</summary>

        public fixed ulong Reserved2[64];
    }

    /// <summary>NV_ENC_RECONFIGURE_PARAMS</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncReconfigureParams
    {
        /// <summary>version: [in]: Struct version. Must be set to ::NV_ENC_RECONFIGURE_PARAMS_VER.</summary>
        public uint Version;
        /// <summary>reInitEncodeParams: [in]: Encoder session re-initialization parameters.
        ///  If reInitEncodeParams.encodeConfig is NULL and
        ///  reInitEncodeParams.presetGUID is the same as the preset
        ///  GUID specified on the call to NvEncInitializeEncoder(),
        ///  EncodeAPI will continue to use the existing encode
        ///  configuration.
        ///  If reInitEncodeParams.encodeConfig is NULL and
        ///  reInitEncodeParams.presetGUID is different from the preset
        ///  GUID specified on the call to NvEncInitializeEncoder(),
        ///  EncodeAPI will try to use the default configuration for
        ///  the preset specified by reInitEncodeParams.presetGUID.
        ///  In this case, reconfiguration may fail if the new
        ///  configuration is incompatible with the existing
        ///  configuration (e.g. the new configuration results in
        ///  a change in the GOP structure).</summary>
        public NvEncInitializeParams ReInitEncodeParams;
        internal fixed byte BitField1[4];
        /// <summary>resetEncoder: [in]: This resets the rate control states and other internal encoder states. This should be used only with an IDR frame.
        ///  If NV_ENC_INITIALIZE_PARAMS::enablePTD is set to 1, encoder will force the frame type to IDR</summary>
        public bool ResetEncoder
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 0) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~1) | (((value ? 1 : 0) << 0) & 1));
            }
        }
        /// <summary>forceIDR: [in]: Encode the current picture as an IDR picture. This flag is only valid when Picture type decision is taken by the Encoder
        ///  [_NV_ENC_INITIALIZE_PARAMS::enablePTD == 1].</summary>
        public bool ForceIDR
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 1) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~2) | (((value ? 1 : 0) << 1) & 2));
            }
        }
        /// <summary>reserved</summary>
    }

    /// <summary>NV_ENC_PRESET_CONFIG</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncPresetConfig
    {
        /// <summary>version: [in]: Struct version. Must be set to ::NV_ENC_PRESET_CONFIG_VER.</summary>
        public uint Version;
        /// <summary>presetCfg: [out]: preset config returned by the Nvidia Video Encoder interface.</summary>
        public NvEncConfig PresetCfg;
        /// <summary>reserved1[255]: [in]: Reserved and must be set to 0</summary>
        public fixed uint Reserved1[255];
        /// <summary>reserved2[64]: [in]: Reserved and must be set to NULL</summary>

        public fixed ulong Reserved2[64];
    }

    /// <summary>NV_ENC_PIC_PARAMS_MVC</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncPicParamsMvc
    {
        /// <summary>version: [in]: Struct version. Must be set to ::NV_ENC_PIC_PARAMS_MVC_VER.</summary>
        public uint Version;
        /// <summary>viewID: [in]: Specifies the view ID associated with the current input view.</summary>
        public uint ViewID;
        /// <summary>temporalID: [in]: Specifies the temporal ID associated with the current input view.</summary>
        public uint TemporalID;
        /// <summary>priorityID: [in]: Specifies the priority ID associated with the current input view. Reserved and ignored by the NvEncodeAPI interface.</summary>
        public uint PriorityID;
        /// <summary>reserved1[12]: [in]: Reserved and must be set to 0.</summary>
        public fixed uint Reserved1[12];
        /// <summary>reserved2[8]: [in]: Reserved and must be set to NULL.</summary>

        public fixed ulong Reserved2[8];
    }

    /// <summary>NV_ENC_SEI_PAYLOAD</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncSeiPayload
    {
        /// <summary>payloadSize: [in] SEI payload size in bytes. SEI payload must be byte aligned, as described in Annex D</summary>
        public uint PayloadSize;
        /// <summary>payloadType: [in] SEI payload types and syntax can be found in Annex D of the H.264 Specification.</summary>
        public uint PayloadType;
        /// <summary>*payload: [in] pointer to user data</summary>
        public byte* payload;
    }

    /// <summary>NV_ENC_PIC_PARAMS_H264</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncPicParamsH264
    {
        /// <summary>displayPOCSyntax: [in]: Specifies the display POC syntax This is required to be set if client is handling the picture type decision.</summary>
        public uint DisplayPOCSyntax;
        /// <summary>reserved3: [in]: Reserved and must be set to 0</summary>
        public uint Reserved3;
        /// <summary>refPicFlag: [in]: Set to 1 for a reference picture. This is ignored if NV_ENC_INITIALIZE_PARAMS::enablePTD is set to 1.</summary>
        public uint RefPicFlag;
        /// <summary>colourPlaneId: [in]: Specifies the colour plane ID associated with the current input.</summary>
        public uint ColourPlaneId;
        /// <summary>forceIntraRefreshWithFrameCnt: [in]: Forces an intra refresh with duration equal to intraRefreshFrameCnt.
        ///  When outputRecoveryPointSEI is set this is value is used for recovery_frame_cnt in recovery point SEI message
        ///  forceIntraRefreshWithFrameCnt cannot be used if B frames are used in the GOP structure specified</summary>
        public uint ForceIntraRefreshWithFrameCnt;
        internal fixed byte BitField1[4];
        /// <summary>constrainedFrame: [in]: Set to 1 if client wants to encode this frame with each slice completely independent of other slices in the frame.
        ///  NV_ENC_INITIALIZE_PARAMS::enableConstrainedEncoding should be set to 1</summary>
        public bool ConstrainedFrame
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 0) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~1) | (((value ? 1 : 0) << 0) & 1));
            }
        }
        /// <summary>sliceModeDataUpdate: [in]: Set to 1 if client wants to change the sliceModeData field to specify new sliceSize Parameter
        ///  When forceIntraRefreshWithFrameCnt is set it will have priority over sliceMode setting</summary>
        public bool SliceModeDataUpdate
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 1) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~2) | (((value ? 1 : 0) << 1) & 2));
            }
        }
        /// <summary>ltrMarkFrame: [in]: Set to 1 if client wants to mark this frame as LTR</summary>
        public bool LtrMarkFrame
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 2) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~4) | (((value ? 1 : 0) << 2) & 4));
            }
        }
        /// <summary>ltrUseFrames: [in]: Set to 1 if client allows encoding this frame using the LTR frames specified in ltrFrameBitmap</summary>
        public bool LtrUseFrames
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 3) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~8) | (((value ? 1 : 0) << 3) & 8));
            }
        }
        /// <summary>reservedBitFields: [in]: Reserved bit fields and must be set to 0</summary>
        /// <summary>sliceTypeData: [in]: Deprecated.</summary>
        public byte* SliceTypeData;
        /// <summary>sliceTypeArrayCnt: [in]: Deprecated.</summary>
        public uint SliceTypeArrayCnt;
        /// <summary>seiPayloadArrayCnt: [in]: Specifies the number of elements allocated in seiPayloadArray array.</summary>
        public uint SeiPayloadArrayCnt;
        /// <summary>seiPayloadArray: [in]: Array of SEI payloads which will be inserted for this frame.</summary>
        public NvEncSeiPayload* SeiPayloadArray;
        /// <summary>sliceMode: [in]: This parameter in conjunction with sliceModeData specifies the way in which the picture is divided into slices
        ///  sliceMode = 0 MB based slices, sliceMode = 1 Byte based slices, sliceMode = 2 MB row based slices, sliceMode = 3, numSlices in Picture
        ///  When forceIntraRefreshWithFrameCnt is set it will have priority over sliceMode setting
        ///  When sliceMode == 0 and sliceModeData == 0 whole picture will be coded with one slice</summary>
        public uint SliceMode;
        /// <summary>sliceModeData: [in]: Specifies the parameter needed for sliceMode. For:
        ///  sliceMode = 0, sliceModeData specifies # of MBs in each slice (except last slice)
        ///  sliceMode = 1, sliceModeData specifies maximum # of bytes in each slice (except last slice)
        ///  sliceMode = 2, sliceModeData specifies # of MB rows in each slice (except last slice)
        ///  sliceMode = 3, sliceModeData specifies number of slices in the picture. Driver will divide picture into slices optimally</summary>
        public uint SliceModeData;
        /// <summary>ltrMarkFrameIdx: [in]: Specifies the long term referenceframe index to use for marking this frame as LTR.</summary>
        public uint LtrMarkFrameIdx;
        /// <summary>ltrUseFrameBitmap: [in]: Specifies the the associated bitmap of LTR frame indices to use when encoding this frame.</summary>
        public uint LtrUseFrameBitmap;
        /// <summary>ltrUsageMode: [in]: Not supported. Reserved for future use and must be set to 0.</summary>
        public uint LtrUsageMode;
        /// <summary>forceIntraSliceCount: [in]: Specfies the number of slices to be forced to Intra in the current picture.
        ///  This option along with forceIntraSliceIdx[] array needs to be used with sliceMode = 3 only</summary>
        public uint ForceIntraSliceCount;
        /// <summary>*forceIntraSliceIdx: [in]: Slice indices to be forced to intra in the current picture. Each slice index should be <= num_slices_in_picture -1. Index starts from 0 for first slice.
        ///  The number of entries in this array should be equal to forceIntraSliceCount</summary>
        public uint* forceIntraSliceIdx;
        /// <summary>h264ExtPicParams: [in]: Specifies the H264 extension config parameters using this config.</summary>
        public NvEncPicParamsH264Ext H264ExtPicParams;
        /// <summary>reserved [210]: [in]: Reserved and must be set to 0.</summary>
        public fixed uint Reserved[210];
        /// <summary>reserved2[61]: [in]: Reserved and must be set to NULL.</summary>

        public fixed ulong Reserved2[61];
    }

    /// <summary>NV_ENC_PIC_PARAMS_HEVC</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncPicParamsHevc
    {
        /// <summary>displayPOCSyntax: [in]: Specifies the display POC syntax This is required to be set if client is handling the picture type decision.</summary>
        public uint DisplayPOCSyntax;
        /// <summary>refPicFlag: [in]: Set to 1 for a reference picture. This is ignored if NV_ENC_INITIALIZE_PARAMS::enablePTD is set to 1.</summary>
        public uint RefPicFlag;
        /// <summary>temporalId: [in]: Specifies the temporal id of the picture</summary>
        public uint TemporalId;
        /// <summary>forceIntraRefreshWithFrameCnt: [in]: Forces an intra refresh with duration equal to intraRefreshFrameCnt.
        ///  When outputRecoveryPointSEI is set this is value is used for recovery_frame_cnt in recovery point SEI message
        ///  forceIntraRefreshWithFrameCnt cannot be used if B frames are used in the GOP structure specified</summary>
        public uint ForceIntraRefreshWithFrameCnt;
        internal fixed byte BitField1[4];
        /// <summary>constrainedFrame: [in]: Set to 1 if client wants to encode this frame with each slice completely independent of other slices in the frame.
        ///  NV_ENC_INITIALIZE_PARAMS::enableConstrainedEncoding should be set to 1</summary>
        public bool ConstrainedFrame
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 0) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~1) | (((value ? 1 : 0) << 0) & 1));
            }
        }
        /// <summary>sliceModeDataUpdate: [in]: Set to 1 if client wants to change the sliceModeData field to specify new sliceSize Parameter
        ///  When forceIntraRefreshWithFrameCnt is set it will have priority over sliceMode setting</summary>
        public bool SliceModeDataUpdate
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 1) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~2) | (((value ? 1 : 0) << 1) & 2));
            }
        }
        /// <summary>ltrMarkFrame: [in]: Set to 1 if client wants to mark this frame as LTR</summary>
        public bool LtrMarkFrame
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 2) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~4) | (((value ? 1 : 0) << 2) & 4));
            }
        }
        /// <summary>ltrUseFrames: [in]: Set to 1 if client allows encoding this frame using the LTR frames specified in ltrFrameBitmap</summary>
        public bool LtrUseFrames
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 3) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~8) | (((value ? 1 : 0) << 3) & 8));
            }
        }
        /// <summary>reservedBitFields: [in]: Reserved bit fields and must be set to 0</summary>
        /// <summary>sliceTypeData: [in]: Array which specifies the slice type used to force intra slice for a particular slice. Currently supported only for NV_ENC_CONFIG_H264::sliceMode == 3.
        ///  Client should allocate array of size sliceModeData where sliceModeData is specified in field of ::_NV_ENC_CONFIG_H264
        ///  Array element with index n corresponds to nth slice. To force a particular slice to intra client should set corresponding array element to NV_ENC_SLICE_TYPE_I
        ///  all other array elements should be set to NV_ENC_SLICE_TYPE_DEFAULT</summary>
        public byte* SliceTypeData;
        /// <summary>sliceTypeArrayCnt: [in]: Client should set this to the number of elements allocated in sliceTypeData array. If sliceTypeData is NULL then this should be set to 0</summary>
        public uint SliceTypeArrayCnt;
        /// <summary>sliceMode: [in]: This parameter in conjunction with sliceModeData specifies the way in which the picture is divided into slices
        ///  sliceMode = 0 CTU based slices, sliceMode = 1 Byte based slices, sliceMode = 2 CTU row based slices, sliceMode = 3, numSlices in Picture
        ///  When forceIntraRefreshWithFrameCnt is set it will have priority over sliceMode setting
        ///  When sliceMode == 0 and sliceModeData == 0 whole picture will be coded with one slice</summary>
        public uint SliceMode;
        /// <summary>sliceModeData: [in]: Specifies the parameter needed for sliceMode. For:
        ///  sliceMode = 0, sliceModeData specifies # of CTUs in each slice (except last slice)
        ///  sliceMode = 1, sliceModeData specifies maximum # of bytes in each slice (except last slice)
        ///  sliceMode = 2, sliceModeData specifies # of CTU rows in each slice (except last slice)
        ///  sliceMode = 3, sliceModeData specifies number of slices in the picture. Driver will divide picture into slices optimally</summary>
        public uint SliceModeData;
        /// <summary>ltrMarkFrameIdx: [in]: Specifies the long term reference frame index to use for marking this frame as LTR.</summary>
        public uint LtrMarkFrameIdx;
        /// <summary>ltrUseFrameBitmap: [in]: Specifies the associated bitmap of LTR frame indices to use when encoding this frame.</summary>
        public uint LtrUseFrameBitmap;
        /// <summary>ltrUsageMode: [in]: Not supported. Reserved for future use and must be set to 0.</summary>
        public uint LtrUsageMode;
        /// <summary>seiPayloadArrayCnt: [in]: Specifies the number of elements allocated in seiPayloadArray array.</summary>
        public uint SeiPayloadArrayCnt;
        /// <summary>reserved: [in]: Reserved and must be set to 0.</summary>
        public uint Reserved;
        /// <summary>seiPayloadArray: [in]: Array of SEI payloads which will be inserted for this frame.</summary>
        public NvEncSeiPayload* SeiPayloadArray;
        /// <summary>reserved2 [244]: [in]: Reserved and must be set to 0.</summary>
        public fixed uint Reserved2[244];
        /// <summary>reserved3[61]: [in]: Reserved and must be set to NULL.</summary>

        public fixed ulong Reserved3[61];
    }

    /// <summary>NV_ENC_PIC_PARAMS</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncPicParams
    {
        /// <summary>version: [in]: Struct version. Must be set to ::NV_ENC_PIC_PARAMS_VER.</summary>
        public uint Version;
        /// <summary>inputWidth: [in]: Specifies the input buffer width</summary>
        public uint InputWidth;
        /// <summary>inputHeight: [in]: Specifies the input buffer height</summary>
        public uint InputHeight;
        /// <summary>inputPitch: [in]: Specifies the input buffer pitch. If pitch value is not known, set this to inputWidth.</summary>
        public uint InputPitch;
        /// <summary>encodePicFlags: [in]: Specifies bit-wise OR`ed encode pic flags. See ::NV_ENC_PIC_FLAGS enum.</summary>
        public uint EncodePicFlags;
        /// <summary>frameIdx: [in]: Specifies the frame index associated with the input frame [optional].</summary>
        public uint FrameIdx;
        /// <summary>inputTimeStamp: [in]: Specifies presentation timestamp associated with the input picture.</summary>
        public ulong InputTimeStamp;
        /// <summary>inputDuration: [in]: Specifies duration of the input picture</summary>
        public ulong InputDuration;
        /// <summary>inputBuffer: [in]: Specifies the input buffer pointer. Client must use a pointer obtained from ::NvEncCreateInputBuffer() or ::NvEncMapInputResource() APIs.</summary>
        public IntPtr InputBuffer;
        /// <summary>outputBitstream: [in]: Specifies the output buffer pointer.
        ///  If NV_ENC_INITIALIZE_PARAMS::enableOutputInVidmem is set to 0, specifies the pointer to output buffer. Client should use a pointer obtained from ::NvEncCreateBitstreamBuffer() API.
        ///  If NV_ENC_INITIALIZE_PARAMS::enableOutputInVidmem is set to 1, client should allocate buffer in video memory for NV_ENC_ENCODE_OUT_PARAMS struct and encoded bitstream data. Client
        ///  should use a pointer obtained from ::NvEncMapInputResource() API, when mapping this output buffer and assign it to NV_ENC_PIC_PARAMS::outputBitstream.
        ///  First 256 bytes of this buffer should be interpreted as NV_ENC_ENCODE_OUT_PARAMS struct followed by encoded bitstream data. Recommended size for output buffer is sum of size of
        ///  NV_ENC_ENCODE_OUT_PARAMS struct and twice the input frame size for lower resolution eg. CIF and 1.5 times the input frame size for higher resolutions. If encoded bitstream size is
        ///  greater than the allocated buffer size for encoded bitstream, then the output buffer will have encoded bitstream data equal to buffer size. All CUDA operations on this buffer must use
        ///  the default stream.</summary>
        public IntPtr OutputBitstream;
        /// <summary>completionEvent: [in]: Specifies an event to be signalled on completion of encoding of this Frame [only if operating in Asynchronous mode]. Each output buffer should be associated with a distinct event pointer.</summary>
        public IntPtr CompletionEvent;
        /// <summary>bufferFmt: [in]: Specifies the input buffer format.</summary>
        public NvEncBufferFormat BufferFmt;
        /// <summary>pictureStruct: [in]: Specifies structure of the input picture.</summary>
        public NvEncPicStruct PictureStruct;
        /// <summary>pictureType: [in]: Specifies input picture type. Client required to be set explicitly by the client if the client has not set NV_ENC_INITALIZE_PARAMS::enablePTD to 1 while calling NvInitializeEncoder.</summary>
        public NvEncPicType PictureType;
        /// <summary>codecPicParams: [in]: Specifies the codec specific per-picture encoding parameters.</summary>
        public NvEncCodecPicParams CodecPicParams;
        /// <summary>meHintCountsPerBlock[2]: [in]: Specifies the number of hint candidates per block per direction for the current frame. meHintCountsPerBlock[0] is for L0 predictors and meHintCountsPerBlock[1] is for L1 predictors.
        ///  The candidate count in NV_ENC_PIC_PARAMS::meHintCountsPerBlock[lx] must never exceed NV_ENC_INITIALIZE_PARAMS::maxMEHintCountsPerBlock[lx] provided during encoder intialization.</summary>
        public NvEncExternalMeHintCountsPerBlocktype MeHintCountsPerBlock0;
        public NvEncExternalMeHintCountsPerBlocktype MeHintCountsPerBlock1;
        /// <summary>*meExternalHints: [in]: Specifies the pointer to ME external hints for the current frame. The size of ME hint buffer should be equal to number of macroblocks * the total number of candidates per macroblock.
        ///  The total number of candidates per MB per direction = 1*meHintCountsPerBlock[Lx].numCandsPerBlk16x16 + 2*meHintCountsPerBlock[Lx].numCandsPerBlk16x8 + 2*meHintCountsPerBlock[Lx].numCandsPerBlk8x8
        ///  + 4*meHintCountsPerBlock[Lx].numCandsPerBlk8x8. For frames using bidirectional ME , the total number of candidates for single macroblock is sum of total number of candidates per MB for each direction (L0 and L1)</summary>
        public NvEncExternalMeHint* meExternalHints;
        /// <summary>reserved1[6]: [in]: Reserved and must be set to 0</summary>
        public fixed uint Reserved1[6];
        /// <summary>reserved2[2]: [in]: Reserved and must be set to NULL</summary>

        public fixed ulong Reserved2[2];
        /// <summary>*qpDeltaMap: [in]: Specifies the pointer to signed byte array containing value per MB in raster scan order for the current picture, which will be interpreted depending on NV_ENC_RC_PARAMS::qpMapMode.
        ///  If NV_ENC_RC_PARAMS::qpMapMode is NV_ENC_QP_MAP_DELTA, qpDeltaMap specifies QP modifier per MB. This QP modifier will be applied on top of the QP chosen by rate control.
        ///  If NV_ENC_RC_PARAMS::qpMapMode is NV_ENC_QP_MAP_EMPHASIS, qpDeltaMap specifies Emphasis Level Map per MB. This level value along with QP chosen by rate control is used to
        ///  compute the QP modifier, which in turn is applied on top of QP chosen by rate control.
        ///  If NV_ENC_RC_PARAMS::qpMapMode is NV_ENC_QP_MAP_DISABLED, value in qpDeltaMap will be ignored.</summary>
        public byte* qpDeltaMap;
        /// <summary>qpDeltaMapSize: [in]: Specifies the size in bytes of qpDeltaMap surface allocated by client and pointed to by NV_ENC_PIC_PARAMS::qpDeltaMap. Surface (array) should be picWidthInMbs * picHeightInMbs</summary>
        public uint QpDeltaMapSize;
        /// <summary>reservedBitFields: [in]: Reserved bitfields and must be set to 0</summary>
        public uint ReservedBitFields;
        /// <summary>meHintRefPicDist[2]: [in]: Specifies temporal distance for reference picture (NVENC_EXTERNAL_ME_HINT::refidx = 0) used during external ME with NV_ENC_INITALIZE_PARAMS::enablePTD = 1 . meHintRefPicDist[0] is for L0 hints and meHintRefPicDist[1] is for L1 hints.
        ///  If not set, will internally infer distance of 1. Ignored for NV_ENC_INITALIZE_PARAMS::enablePTD = 0</summary>
        public fixed ushort MeHintRefPicDist[2];
        /// <summary>reserved3[286]: [in]: Reserved and must be set to 0</summary>
        public fixed uint Reserved3[286];
        /// <summary>reserved4[60]: [in]: Reserved and must be set to NULL</summary>

        public fixed ulong Reserved4[60];
    }

    /// <summary>NV_ENC_MEONLY_PARAMS</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncMeonlyParams
    {
        /// <summary>version: [in]: Struct version. Must be set to NV_ENC_MEONLY_PARAMS_VER.</summary>
        public uint Version;
        /// <summary>inputWidth: [in]: Specifies the input buffer width</summary>
        public uint InputWidth;
        /// <summary>inputHeight: [in]: Specifies the input buffer height</summary>
        public uint InputHeight;
        /// <summary>inputBuffer: [in]: Specifies the input buffer pointer. Client must use a pointer obtained from NvEncCreateInputBuffer() or NvEncMapInputResource() APIs.</summary>
        public IntPtr InputBuffer;
        /// <summary>referenceFrame: [in]: Specifies the reference frame pointer</summary>
        public IntPtr ReferenceFrame;
        /// <summary>mvBuffer: [in]: Specifies the output buffer pointer.
        ///  If NV_ENC_INITIALIZE_PARAMS::enableOutputInVidmem is set to 0, specifies the pointer to motion vector data buffer allocated by NvEncCreateMVBuffer.
        ///  Client must lock mvBuffer using ::NvEncLockBitstream() API to get the motion vector data.
        ///  If NV_ENC_INITIALIZE_PARAMS::enableOutputInVidmem is set to 1, client should allocate buffer in video memory for storing the motion vector data. The size of this buffer must
        ///  be equal to total number of macroblocks multiplied by size of NV_ENC_H264_MV_DATA struct. Client should use a pointer obtained from ::NvEncMapInputResource() API, when mapping this
        ///  output buffer and assign it to NV_ENC_MEONLY_PARAMS::mvBuffer. All CUDA operations on this buffer must use the default stream.</summary>
        public IntPtr MvBuffer;
        /// <summary>bufferFmt: [in]: Specifies the input buffer format.</summary>
        public NvEncBufferFormat BufferFmt;
        /// <summary>completionEvent: [in]: Specifies an event to be signalled on completion of motion estimation
        ///  of this Frame [only if operating in Asynchronous mode].
        ///  Each output buffer should be associated with a distinct event pointer.</summary>
        public IntPtr CompletionEvent;
        /// <summary>viewID: [in]: Specifies left,right viewID if NV_ENC_CONFIG_H264_MEONLY::bStereoEnable is set.
        ///  viewID can be 0,1 if bStereoEnable is set, 0 otherwise.</summary>
        public uint ViewID;
        /// <summary>meHintCountsPerBlock[2]: [in]: Specifies the number of hint candidates per block for the current frame. meHintCountsPerBlock[0] is for L0 predictors.
        ///  The candidate count in NV_ENC_PIC_PARAMS::meHintCountsPerBlock[lx] must never exceed NV_ENC_INITIALIZE_PARAMS::maxMEHintCountsPerBlock[lx] provided during encoder intialization.</summary>
        public NvEncExternalMeHintCountsPerBlocktype MeHintCountsPerBlock0;
        public NvEncExternalMeHintCountsPerBlocktype MeHintCountsPerBlock1;
        /// <summary>*meExternalHints: [in]: Specifies the pointer to ME external hints for the current frame. The size of ME hint buffer should be equal to number of macroblocks * the total number of candidates per macroblock.
        ///  The total number of candidates per MB per direction = 1*meHintCountsPerBlock[Lx].numCandsPerBlk16x16 + 2*meHintCountsPerBlock[Lx].numCandsPerBlk16x8 + 2*meHintCountsPerBlock[Lx].numCandsPerBlk8x8
        ///  + 4*meHintCountsPerBlock[Lx].numCandsPerBlk8x8. For frames using bidirectional ME , the total number of candidates for single macroblock is sum of total number of candidates per MB for each direction (L0 and L1)</summary>
        public NvEncExternalMeHint* meExternalHints;
        /// <summary>reserved1[243]: [in]: Reserved and must be set to 0</summary>
        public fixed uint Reserved1[243];
        /// <summary>reserved2[59]: [in]: Reserved and must be set to NULL</summary>

        public fixed ulong Reserved2[59];
    }

    /// <summary>NV_ENC_LOCK_BITSTREAM</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncLockBitstream
    {
        /// <summary>version: [in]: Struct version. Must be set to ::NV_ENC_LOCK_BITSTREAM_VER.</summary>
        public uint Version;
        internal fixed byte BitField1[4];
        /// <summary>doNotWait: [in]: If this flag is set, the NvEncodeAPI interface will return buffer pointer even if operation is not completed. If not set, the call will block until operation completes.</summary>
        public bool DoNotWait
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 0) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~1) | (((value ? 1 : 0) << 0) & 1));
            }
        }
        /// <summary>ltrFrame: [out]: Flag indicating this frame is marked as LTR frame</summary>
        public bool LtrFrame
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 1) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~2) | (((value ? 1 : 0) << 1) & 2));
            }
        }
        /// <summary>getRCStats: [in]: If this flag is set then lockBitstream call will add additional intra-inter MB count and average MVX, MVY</summary>
        public bool GetRCStats
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 2) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~4) | (((value ? 1 : 0) << 2) & 4));
            }
        }
        /// <summary>reservedBitFields: [in]: Reserved bit fields and must be set to 0</summary>
        /// <summary>outputBitstream: [in]: Pointer to the bitstream buffer being locked.</summary>
        public IntPtr OutputBitstream;
        /// <summary>sliceOffsets: [in,out]: Array which receives the slice offsets. This is not supported if NV_ENC_CONFIG_H264::sliceMode is 1 on Kepler GPUs. Array size must be equal to size of frame in MBs.</summary>
        public uint* SliceOffsets;
        /// <summary>frameIdx: [out]: Frame no. for which the bitstream is being retrieved.</summary>
        public uint FrameIdx;
        /// <summary>hwEncodeStatus: [out]: The NvEncodeAPI interface status for the locked picture.</summary>
        public uint HwEncodeStatus;
        /// <summary>numSlices: [out]: Number of slices in the encoded picture. Will be reported only if NV_ENC_INITIALIZE_PARAMS::reportSliceOffsets set to 1.</summary>
        public uint NumSlices;
        /// <summary>bitstreamSizeInBytes: [out]: Actual number of bytes generated and copied to the memory pointed by bitstreamBufferPtr.</summary>
        public uint BitstreamSizeInBytes;
        /// <summary>outputTimeStamp: [out]: Presentation timestamp associated with the encoded output.</summary>
        public ulong OutputTimeStamp;
        /// <summary>outputDuration: [out]: Presentation duration associates with the encoded output.</summary>
        public ulong OutputDuration;
        /// <summary>bitstreamBufferPtr: [out]: Pointer to the generated output bitstream.
        ///  For MEOnly mode _NV_ENC_LOCK_BITSTREAM::bitstreamBufferPtr should be typecast to
        ///  NV_ENC_H264_MV_DATA/NV_ENC_HEVC_MV_DATA pointer respectively for H264/HEVC</summary>
        public IntPtr BitstreamBufferPtr;
        /// <summary>pictureType: [out]: Picture type of the encoded picture.</summary>
        public NvEncPicType PictureType;
        /// <summary>pictureStruct: [out]: Structure of the generated output picture.</summary>
        public NvEncPicStruct PictureStruct;
        /// <summary>frameAvgQP: [out]: Average QP of the frame.</summary>
        public uint FrameAvgQP;
        /// <summary>frameSatd: [out]: Total SATD cost for whole frame.</summary>
        public uint FrameSatd;
        /// <summary>ltrFrameIdx: [out]: Frame index associated with this LTR frame.</summary>
        public uint LtrFrameIdx;
        /// <summary>ltrFrameBitmap: [out]: Bitmap of LTR frames indices which were used for encoding this frame. Value of 0 if no LTR frames were used.</summary>
        public uint LtrFrameBitmap;
        /// <summary>reserved[13]: [in]: Reserved and must be set to 0</summary>
        public fixed uint Reserved[13];
        /// <summary>intraMBCount: [out]: For H264, Number of Intra MBs in the encoded frame. For HEVC, Number of Intra CTBs in the encoded frame. Supported only if _NV_ENC_LOCK_BITSTREAM::getRCStats set to 1.</summary>
        public uint IntraMBCount;
        /// <summary>interMBCount: [out]: For H264, Number of Inter MBs in the encoded frame, includes skip MBs. For HEVC, Number of Inter CTBs in the encoded frame. Supported only if _NV_ENC_LOCK_BITSTREAM::getRCStats set to 1.</summary>
        public uint InterMBCount;
        /// <summary>averageMVX: [out]: Average Motion Vector in X direction for the encoded frame. Supported only if _NV_ENC_LOCK_BITSTREAM::getRCStats set to 1.</summary>
        public int AverageMVX;
        /// <summary>averageMVY: [out]: Average Motion Vector in y direction for the encoded frame. Supported only if _NV_ENC_LOCK_BITSTREAM::getRCStats set to 1.</summary>
        public int AverageMVY;
        /// <summary>reserved1[219]: [in]: Reserved and must be set to 0</summary>
        public fixed uint Reserved1[219];
        /// <summary>reserved2[64]: [in]: Reserved and must be set to NULL</summary>

        public fixed ulong Reserved2[64];
    }

    /// <summary>NV_ENC_LOCK_INPUT_BUFFER</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncLockInputBuffer
    {
        /// <summary>version: [in]: Struct version. Must be set to ::NV_ENC_LOCK_INPUT_BUFFER_VER.</summary>
        public uint Version;
        internal fixed byte BitField1[4];
        /// <summary>doNotWait: [in]: Set to 1 to make ::NvEncLockInputBuffer() a unblocking call. If the encoding is not completed, driver will return ::NV_ENC_ERR_ENCODER_BUSY error code.</summary>
        public bool DoNotWait
        {
            get { fixed (byte* ptr = &BitField1[0]) { return ((*(byte*)ptr >> 0) & 1) == 1; } }
            set
            {
                BitField1[0] = (byte)((BitField1[0] & ~1) | (((value ? 1 : 0) << 0) & 1));
            }
        }
        /// <summary>reservedBitFields: [in]: Reserved bitfields and must be set to 0</summary>
        /// <summary>inputBuffer: [in]: Pointer to the input buffer to be locked, client should pass the pointer obtained from ::NvEncCreateInputBuffer() or ::NvEncMapInputResource API.</summary>
        public IntPtr InputBuffer;
        /// <summary>bufferDataPtr: [out]: Pointed to the locked input buffer data. Client can only access input buffer using the \p bufferDataPtr.</summary>
        public IntPtr BufferDataPtr;
        /// <summary>pitch: [out]: Pitch of the locked input buffer.</summary>
        public uint Pitch;
        /// <summary>reserved1[251]: [in]: Reserved and must be set to 0</summary>
        public fixed uint Reserved1[251];
        /// <summary>reserved2[64]: [in]: Reserved and must be set to NULL</summary>

        public fixed ulong Reserved2[64];
    }

    /// <summary>NV_ENC_MAP_INPUT_RESOURCE</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncMapInputResource
    {
        /// <summary>version: [in]: Struct version. Must be set to ::NV_ENC_MAP_INPUT_RESOURCE_VER.</summary>
        public uint Version;
        /// <summary>subResourceIndex: [in]: Deprecated. Do not use.</summary>
        public uint SubResourceIndex;
        /// <summary>inputResource: [in]: Deprecated. Do not use.</summary>
        public IntPtr InputResource;
        /// <summary>registeredResource: [in]: The Registered resource handle obtained by calling NvEncRegisterInputResource.</summary>
        public IntPtr RegisteredResource;
        /// <summary>mappedResource: [out]: Mapped pointer corresponding to the registeredResource. This pointer must be used in NV_ENC_PIC_PARAMS::inputBuffer parameter in ::NvEncEncodePicture() API.</summary>
        public IntPtr MappedResource;
        /// <summary>mappedBufferFmt: [out]: Buffer format of the outputResource. This buffer format must be used in NV_ENC_PIC_PARAMS::bufferFmt if client using the above mapped resource pointer.</summary>
        public NvEncBufferFormat MappedBufferFmt;
        /// <summary>reserved1[251]: [in]: Reserved and must be set to 0.</summary>
        public fixed uint Reserved1[251];
        /// <summary>reserved2[63]: [in]: Reserved and must be set to NULL</summary>

        public fixed ulong Reserved2[63];
    }

    /// <summary>NV_ENC_INPUT_RESOURCE_OPENGL_TEX</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncInputResourceOpenglTex
    {
        /// <summary>texture: [in]: The name of the texture to be used.</summary>
        public uint Texture;
        /// <summary>target: [in]: Accepted values are GL_TEXTURE_RECTANGLE and GL_TEXTURE_2D.</summary>
        public uint Target;
    }

    /// <summary>NV_ENC_REGISTER_RESOURCE</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncRegisterResource
    {
        /// <summary>version: [in]: Struct version. Must be set to ::NV_ENC_REGISTER_RESOURCE_VER.</summary>
        public uint Version;
        /// <summary>resourceType: [in]: Specifies the type of resource to be registered.
        ///  Supported values are
        ///  ::NV_ENC_INPUT_RESOURCE_TYPE_DIRECTX,
        ///  ::NV_ENC_INPUT_RESOURCE_TYPE_CUDADEVICEPTR,
        ///  ::NV_ENC_INPUT_RESOURCE_TYPE_OPENGL_TEX</summary>
        public NvEncInputResourceType ResourceType;
        /// <summary>width: [in]: Input buffer Width.</summary>
        public uint Width;
        /// <summary>height: [in]: Input buffer Height.</summary>
        public uint Height;
        /// <summary>pitch: [in]: Input buffer Pitch.
        ///  For ::NV_ENC_INPUT_RESOURCE_TYPE_DIRECTX resources, set this to 0.
        ///  For ::NV_ENC_INPUT_RESOURCE_TYPE_CUDADEVICEPTR resources, set this to
        ///  the pitch as obtained from cuMemAllocPitch(), or to the width in
        ///  bytes (if this resource was created by using cuMemAlloc()). This
        ///  value must be a multiple of 4.
        ///  For ::NV_ENC_INPUT_RESOURCE_TYPE_CUDAARRAY resources, set this to the
        ///  width of the allocation in bytes (i.e.
        ///  CUDA_ARRAY3D_DESCRIPTOR::Width * CUDA_ARRAY3D_DESCRIPTOR::NumChannels).
        ///  For ::NV_ENC_INPUT_RESOURCE_TYPE_OPENGL_TEX resources, set this to the
        ///  texture width multiplied by the number of components in the texture
        ///  format.</summary>
        public uint Pitch;
        /// <summary>subResourceIndex: [in]: Subresource Index of the DirectX resource to be registered. Should be set to 0 for other interfaces.</summary>
        public uint SubResourceIndex;
        /// <summary>resourceToRegister: [in]: Handle to the resource that is being registered.</summary>
        public IntPtr ResourceToRegister;
        /// <summary>registeredResource: [out]: Registered resource handle. This should be used in future interactions with the Nvidia Video Encoder Interface.</summary>
        public IntPtr RegisteredResource;
        /// <summary>bufferFormat: [in]: Buffer format of resource to be registered.</summary>
        public NvEncBufferFormat BufferFormat;
        /// <summary>bufferUsage: [in]: Usage of resource to be registered.</summary>
        public NvEncBufferUsage BufferUsage;
        /// <summary>reserved1[247]: [in]: Reserved and must be set to 0.</summary>
        public fixed uint Reserved1[247];
        /// <summary>reserved2[62]: [in]: Reserved and must be set to NULL.</summary>

        public fixed ulong Reserved2[62];
    }

    /// <summary>NV_ENC_STAT</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncStat
    {
        /// <summary>version: [in]: Struct version. Must be set to ::NV_ENC_STAT_VER.</summary>
        public uint Version;
        /// <summary>reserved: [in]: Reserved and must be set to 0</summary>
        public uint Reserved;
        /// <summary>outputBitStream: [out]: Specifies the pointer to output bitstream.</summary>
        public IntPtr OutputBitStream;
        /// <summary>bitStreamSize: [out]: Size of generated bitstream in bytes.</summary>
        public uint BitStreamSize;
        /// <summary>picType: [out]: Picture type of encoded picture. See ::NV_ENC_PIC_TYPE.</summary>
        public uint PicType;
        /// <summary>lastValidByteOffset: [out]: Offset of last valid bytes of completed bitstream</summary>
        public uint LastValidByteOffset;
        /// <summary>sliceOffsets[16]: [out]: Offsets of each slice</summary>
        public fixed uint SliceOffsets[16];
        /// <summary>picIdx: [out]: Picture number</summary>
        public uint PicIdx;
        /// <summary>reserved1[233]: [in]: Reserved and must be set to 0</summary>
        public fixed uint Reserved1[233];
        /// <summary>reserved2[64]: [in]: Reserved and must be set to NULL</summary>

        public fixed ulong Reserved2[64];
    }

    /// <summary>NV_ENC_SEQUENCE_PARAM_PAYLOAD</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncSequenceParamPayload
    {
        /// <summary>version: [in]: Struct version. Must be set to ::NV_ENC_INITIALIZE_PARAMS_VER.</summary>
        public uint Version;
        /// <summary>inBufferSize: [in]: Specifies the size of the spsppsBuffer provied by the client</summary>
        public uint InBufferSize;
        /// <summary>spsId: [in]: Specifies the SPS id to be used in sequence header. Default value is 0.</summary>
        public uint SpsId;
        /// <summary>ppsId: [in]: Specifies the PPS id to be used in picture header. Default value is 0.</summary>
        public uint PpsId;
        /// <summary>spsppsBuffer: [in]: Specifies bitstream header pointer of size NV_ENC_SEQUENCE_PARAM_PAYLOAD::inBufferSize. It is the client's responsibility to manage this memory.</summary>
        public IntPtr SpsppsBuffer;
        /// <summary>outSPSPPSPayloadSize: [out]: Size of the sequence and picture header in bytes written by the NvEncodeAPI interface to the SPSPPSBuffer.</summary>
        public uint* OutSPSPPSPayloadSize;
        /// <summary>reserved [250]: [in]: Reserved and must be set to 0</summary>
        public fixed uint Reserved[250];
        /// <summary>reserved2[64]: [in]: Reserved and must be set to NULL</summary>

        public fixed ulong Reserved2[64];
    }

    /// <summary>NV_ENC_EVENT_PARAMS</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncEventParams
    {
        /// <summary>version: [in]: Struct version. Must be set to ::NV_ENC_EVENT_PARAMS_VER.</summary>
        public uint Version;
        /// <summary>reserved: [in]: Reserved and must be set to 0</summary>
        public uint Reserved;
        /// <summary>completionEvent: [in]: Handle to event to be registered/unregistered with the NvEncodeAPI interface.</summary>
        public IntPtr CompletionEvent;
        /// <summary>reserved1[253]: [in]: Reserved and must be set to 0</summary>
        public fixed uint Reserved1[253];
        /// <summary>reserved2[64]: [in]: Reserved and must be set to NULL</summary>

        public fixed ulong Reserved2[64];
    }

    /// <summary>NV_ENC_OPEN_ENCODE_SESSIONEX_PARAMS</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NvEncOpenEncodeSessionexParams
    {
        /// <summary>version: [in]: Struct version. Must be set to ::NV_ENC_OPEN_ENCODE_SESSION_EX_PARAMS_VER.</summary>
        public uint Version;
        /// <summary>deviceType: [in]: Specified the device Type</summary>
        public NvEncDeviceType DeviceType;
        /// <summary>device: [in]: Pointer to client device.</summary>
        public IntPtr Device;
        /// <summary>reserved: [in]: Reserved and must be set to 0.</summary>
        public IntPtr Reserved;
        /// <summary>apiVersion: [in]: API version. Should be set to NVENCAPI_VERSION.</summary>
        public uint ApiVersion;
        /// <summary>reserved1[253]: [in]: Reserved and must be set to 0</summary>
        public fixed uint Reserved1[253];
        /// <summary>reserved2[64]: [in]: Reserved and must be set to NULL</summary>

        public fixed ulong Reserved2[64];
    }
}
