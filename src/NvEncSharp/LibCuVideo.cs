using System;
using System.Runtime.InteropServices;

namespace Lennox.NvEncSharp
{
    public unsafe class LibCuVideo
    {
        private const string _dllpath = "nvcuvid.dll";

        /// <summary>
        /// typedef int (CUDAAPI *PFNVIDSOURCECALLBACK)(void *, CUVIDSOURCEDATAPACKET *);
        /// Callback for packet delivery
        /// </summary>
        public delegate CuCallbackResult VideoSourceCallback(IntPtr data, ref CuVideoSourceDataPacket packet);

        // typedef int (CUDAAPI *PFNVIDSEQUENCECALLBACK)(void *, CUVIDEOFORMAT *);
        public delegate CuCallbackResult VideoSequenceCallback(IntPtr userData, ref CuVideoFormat pvidfmt);
        // typedef int (CUDAAPI* PFNVIDDECODECALLBACK) (void*, CUVIDPICPARAMS*);
        public delegate CuCallbackResult VideoDecodeCallback(IntPtr userData, ref CuVideoPicParams param);
        // typedef int (CUDAAPI* PFNVIDDISPLAYCALLBACK) (void*, CUVIDPARSERDISPINFO*);
        /// <summary>
        /// The callback when a decoded frame is ready for display.
        /// </summary>
        /// <param name="userData">The pointer passed to `CuVideoParserParams.UserData`</param>
        /// <param name="infoPtr">A pointer to a `CuVideoParseDisplayInfo` object or `IntPtr.Zero` if
        /// the end of stream has been reached. Use `CuVideoParseDisplayInfo.IsFinalFrame` to get the
        /// actual structure.</param>
        /// <returns></returns>
        /// <seealso cref="CuVideoParseDisplayInfo.IsFinalFrame(IntPtr, out CuVideoParseDisplayInfo)" />
        public delegate CuCallbackResult VideoDisplayCallback(IntPtr userData, IntPtr infoPtr);

        #region Obsolete
        /// <summary>
        /// \fn CUresult CUDAAPI cuvidCreateVideoSource(CUvideosource *pObj, const char *pszFileName, CUVIDSOURCEPARAMS *pParams)
        /// Create CUvideosource object. CUvideosource spawns demultiplexer thread that provides two callbacks:
        /// pfnVideoDataHandler() and pfnAudioDataHandler()
        /// NVDECODE API is intended for HW accelerated video decoding so CUvideosource doesn't have audio demuxer for all supported
        /// containers. It's recommended to clients to use their own or third party demuxer if audio support is needed.
        /// </summary>
        [Obsolete]
        [DllImport(_dllpath, EntryPoint = "cuvidCreateVideoSource", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern CuResult CreateVideoSource(CuVideoSource* pObj, string pszFileName, ref CuVideoSourceParams pParams);

        /// <summary>
        /// \fn CUresult CUDAAPI cuvidCreateVideoSourceW(CUvideosource *pObj, const wchar_t *pwszFileName, CUVIDSOURCEPARAMS *pParams)
        /// Create video source
        /// </summary>
        [Obsolete]
        [DllImport(_dllpath, EntryPoint = "cuvidCreateVideoSourceW", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern CuResult CreateVideoSourceW(CuVideoSource* pObj, string pszFileName, ref CuVideoSourceParams pParams);

        /// <summary>
        /// \fn CUresult CUDAAPI cuvidDestroyVideoSource(CUvideosource obj)
        /// Destroy video source
        /// </summary>
        [Obsolete]
        [DllImport(_dllpath, EntryPoint = "cuvidDestroyVideoSource", SetLastError = true)]
        public static extern CuResult DestroyVideoSource(CuVideoSource obj);

        /// <summary>
        /// \fn CUresult CUDAAPI cuvidSetVideoSourceState(CUvideosource obj, cudaVideoState state)
        /// Set video source state to:
        /// cudaVideoState_Started - to signal the source to run and deliver data
        /// cudaVideoState_Stopped - to stop the source from delivering the data
        /// cudaVideoState_Error   - invalid source
        /// </summary>
        [Obsolete]
        [DllImport(_dllpath, EntryPoint = "cuvidSetVideoSourceState", SetLastError = true)]
        public static extern CuResult SetVideoSourceState(CuVideoSource obj, CuVideoState state);

        /// <summary>
        /// \fn cudaVideoState CUDAAPI cuvidGetVideoSourceState(CUvideosource obj)
        /// Get video source state
        /// </summary>
        /// <returns>
        /// cudaVideoState_Started - if Source is running and delivering data
        /// cudaVideoState_Stopped - if Source is stopped or reached end-of-stream
        /// cudaVideoState_Error   - if Source is in error state
        /// </returns>
        [Obsolete]
        [DllImport(_dllpath, EntryPoint = "cuvidGetVideoSourceState", SetLastError = true)]
        public static extern CuVideoState GetVideoSourceState(CuVideoSource obj);

        /// <summary>
        /// \fn CUresult CUDAAPI cuvidGetSourceVideoFormat(CUvideosource obj, CUVIDEOFORMAT *pvidfmt, unsigned int flags)
        /// Gets video source format in pvidfmt, flags is set to combination of CUvideosourceformat_flags as per requirement
        /// </summary>
        [Obsolete]
        [DllImport(_dllpath, EntryPoint = "cuvidGetSourceVideoFormat", SetLastError = true)]
        public static extern CuResult GetSourceVideoFormat(CuVideoSource obj, ref CuVideoFormat pvidfmt, uint flags);

        /// <summary>
        /// \fn CUresult CUDAAPI cuvidGetSourceAudioFormat(CUvideosource obj, CUAUDIOFORMAT *paudfmt, unsigned int flags)
        /// Get audio source format
        /// NVDECODE API is intended for HW accelerated video decoding so CUvideosource doesn't have audio demuxer for all supported
        /// containers. It's recommended to clients to use their own or third party demuxer if audio support is needed.
        /// </summary>
        [Obsolete]
        [DllImport(_dllpath, EntryPoint = "cuvidGetSourceAudioFormat", SetLastError = true)]
        public static extern CuResult GetSourceAudioFormat(CuVideoSource obj, ref CuAudioFormat paudfmt, uint flags);
        #endregion

        /// <summary>
        /// \fn CUresult CUDAAPI cuvidCreateVideoParser(CUvideoparser *pObj, CUVIDPARSERPARAMS *pParams)
        /// Create video parser object and initialize
        /// </summary>
        [DllImport(_dllpath, EntryPoint = "cuvidCreateVideoParser", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern CuResult CreateVideoParser(out CuVideoParser parser, ref CuVideoParserParams @params);

        /// <summary>
        /// \fn CUresult CUDAAPI cuvidParseVideoData(CUvideoparser obj, CUVIDSOURCEDATAPACKET *pPacket)
        /// Parse the video data from source data packet in pPacket
        /// Extracts parameter sets like SPS, PPS, bitstream etc. from pPacket and
        /// calls back pfnDecodePicture with CUVIDPICPARAMS data for kicking of HW decoding
        /// calls back pfnSequenceCallback with CUVIDEOFORMAT data for initial sequence header or when
        /// the decoder encounters a video format change
        /// calls back pfnDisplayPicture with CUVIDPARSERDISPINFO data to display a video frame
        /// </summary>
        /// CUresult CUDAAPI cuvidParseVideoData(CUvideoparser obj, CUVIDSOURCEDATAPACKET *pPacket);
        [DllImport(_dllpath, EntryPoint = "cuvidParseVideoData")]
        public static extern CuResult ParseVideoData(CuVideoParser obj, ref CuVideoSourceDataPacket packet);

        /// <summary>
        /// \fn CUresult CUDAAPI cuvidDestroyVideoParser(CUvideoparser obj)
        /// Destroy the video parser
        /// </summary>
        /// CUresult CUDAAPI cuvidDestroyVideoParser(CUvideoparser obj);
        [DllImport(_dllpath, EntryPoint = "cuvidDestroyVideoParser")]
        public static extern CuResult DestroyVideoParser(CuVideoParser obj);

        /// <summary>
        /// \fn CUresult CuAPI cuvidCtxLockCreate(CUvideoctxlock *pLock, CUcontext ctx)
        /// This API is used to create CtxLock object
        ///
        /// Context-locking: to facilitate multi-threaded implementations, the following 4 functions
        /// provide a simple mutex-style host synchronization. If a non-NULL context is specified
        /// in CUVIDDECODECREATEINFO, the codec library will acquire the mutex associated with the given
        /// context before making any Cu calls.
        /// A multi-threaded application could create a lock associated with a context handle so that
        /// multiple threads can safely share the same Cu context:
        /// - use cuCtxPopCurrent immediately after context creation in order to create a 'floating' context
        /// that can be passed to cuvidCtxLockCreate.
        /// - When using a floating context, all Cu calls should only be made within a cuvidCtxLock/cuvidCtxUnlock section.
        ///
        /// NOTE: This is a safer alternative to cuCtxPushCurrent and cuCtxPopCurrent, and is not related to video
        /// decoder in any way (implemented as a critical section associated with cuCtx{Push|Pop}Current calls).
        /// </summary>
        [DllImport(_dllpath, EntryPoint = "cuvidCtxLockCreate")]
        public static extern CuResult CtxLockCreate(out CuVideoContextLock pLock, CuContext ctx);

        /// <summary>
        /// \fn CUresult CuAPI cuvidCtxLockDestroy(CUvideoctxlock lck)
        /// This API is used to free CtxLock object
        /// </summary>
        [DllImport(_dllpath, EntryPoint = "cuvidCtxLockDestroy")]
        public static extern CuResult CtxLockDestroy(CuVideoContextLock lck);

        /// <summary>
        /// \fn CUresult CuAPI cuvidCtxLock(CUvideoctxlock lck, unsigned int reserved_flags)
        /// This API is used to acquire ctxlock
        /// </summary>
        [DllImport(_dllpath, EntryPoint = "cuvidCtxLock")]
        public static extern CuResult CtxLock(CuVideoContextLock lck, uint reservedFlags);

        /// <summary>
        /// \fn CUresult CuAPI cuvidCtxUnlock(CUvideoctxlock lck, unsigned int reserved_flags)
        /// This API is used to release ctxlock
        /// </summary>
        [DllImport(_dllpath, EntryPoint = "cuvidCtxUnlock")]
        public static extern CuResult CtxUnlock(CuVideoContextLock lck, uint reservedFlags);

        /// <summary>
        /// \fn CUresult CuAPI cuvidGetDecoderCaps(CUVIDDECODECAPS *pdc)
        /// Queries decode capabilities of NVDEC-HW based on CodecType, ChromaFormat and BitDepthMinus8 parameters.
        /// 1. Application fills IN parameters CodecType, ChromaFormat and BitDepthMinus8 of CUVIDDECODECAPS structure
        /// 2. On calling cuvidGetDecoderCaps, driver fills OUT parameters if the IN parameters are supported
        /// If IN parameters passed to the driver are not supported by NVDEC-HW, then all OUT params are set to 0.
        /// E.g. on Geforce GTX 960:
        /// App fills - eCodecType = CuVideoCodec_H264; eChromaFormat = CuVideoChromaFormat_420; nBitDepthMinus8 = 0;
        /// Given IN parameters are supported, hence driver fills: bIsSupported = 1; nMinWidth = 48; nMinHeight = 16;
        /// nMaxWidth = 4096; nMaxHeight = 4096; nMaxMBCount = 65536;
        /// CodedWidth*CodedHeight/256 must be less than or equal to nMaxMBCount
        /// </summary>
        [DllImport(_dllpath, EntryPoint = "cuvidGetDecoderCaps")]
        public static extern CuResult GetDecoderCaps(ref CuVideoDecodeCaps pdc);

        /// <summary>
        /// \fn CUresult CuAPI cuvidCreateDecoder(CUvideodecoder *phDecoder, CUVIDDECODECREATEINFO *pdci)
        /// Create the decoder object based on pdci. A handle to the created decoder is returned
        /// </summary>
        [DllImport(_dllpath, EntryPoint = "cuvidCreateDecoder")]
        public static extern CuResult CreateDecoder(out CuVideoDecoder decoder, ref CuVideoDecodeCreateInfo pdci);

        /// <summary>
        /// \fn CUresult CuAPI cuvidDestroyDecoder(CUvideodecoder hDecoder)
        /// Destroy the decoder object
        /// </summary>
        [DllImport(_dllpath, EntryPoint = "cuvidDestroyDecoder")]
        public static extern CuResult DestroyDecoder(CuVideoDecoder decoder);

        /// <summary>
        /// \fn CUresult CuAPI cuvidDecodePicture(CUvideodecoder hDecoder, CUVIDPicParams *pPicParams)
        /// Decode a single picture (field or frame)
        /// Kicks off HW decoding
        /// </summary>
        [DllImport(_dllpath, EntryPoint = "cuvidDecodePicture")]
        public static extern CuResult DecodePicture(CuVideoDecoder decoder, ref CuVideoPicParams picParams);

        /// <summary>
        /// \fn CUresult CuAPI cuvidGetDecodeStatus(CUvideodecoder hDecoder, int nPicIdx);
        /// Get the decode status for frame corresponding to nPicIdx
        /// </summary>
        [DllImport(_dllpath, EntryPoint = "cuvidGetDecodeStatus")]
        public static extern CuResult GetDecodeStatus(CuVideoDecoder decoder, int nPicIdx, out CuVideoDecodeStatus decodeStatus);

        /// <summary>
        /// \fn CUresult CuAPI cuvidReconfigureDecoder(CUvideodecoder hDecoder, CUVIDRECONFIGUREDECODERINFO *pDecReconfigParams)
        /// Used to reuse single decoder for multiple clips. Currently supports resolution change, resize params, display area
        /// params, target area params change for same codec. Must be called during CUVIDPARSERPARAMS::pfnSequenceCallback
        /// </summary>
        [DllImport(_dllpath, EntryPoint = "cuvidReconfigureDecoder")]
        public static extern CuResult ReconfigureDecoder(CuVideoDecoder decoder, ref CuVideoReconfigureDecoderInfo decReconfigParams);

        /// <summary>
        /// \fn CUresult CuAPI cuvidMapVideoFrame(CUvideodecoder hDecoder, int nPicIdx, unsigned int *pDevPtr,
        /// uint *pPitch, CUVIDPROCPARAMS *pVPP);
        /// Post-process and map video frame corresponding to nPicIdx for use in Cu. Returns Cu device pointer and associated
        /// pitch of the video frame
        /// </summary>
        [DllImport(_dllpath, EntryPoint = "cuvidMapVideoFrame")]
        public static extern CuResult MapVideoFrame(CuVideoDecoder decoder, int picIdx,
            out CuDevicePtr devPtr, out int pitch, ref CuVideoProcParams vpp);

        /// <summary>
        /// \fn CUresult CuAPI cuvidUnmapVideoFrame(CUvideodecoder hDecoder, unsigned int DevPtr)
        /// Unmap a previously mapped video frame
        /// </summary>
        [DllImport(_dllpath, EntryPoint = "cuvidUnmapVideoFrame")]
        public static extern CuResult UnmapVideoFrame(CuVideoDecoder decoder, CuDevicePtr devPtr);

        /// <summary>
        /// \fn CUresult CuAPI cuvidMapVideoFrame64(CUvideodecoder hDecoder, int nPicIdx, unsigned long long *pDevPtr,
        /// uint * pPitch, CUVIDPROCPARAMS *pVPP);
        /// Post-process and map video frame corresponding to nPicIdx for use in Cu. Returns Cu device pointer and associated
        /// pitch of the video frame
        /// </summary>
        [DllImport(_dllpath, EntryPoint = "cuvidMapVideoFrame64")]
        public static extern CuResult MapVideoFrame64(CuVideoDecoder decoder, int nPicIdx,
            out CuDevicePtr devPtr, out int pitch, ref CuVideoProcParams vpp);

        /// <summary>
        /// \fn CUresult CuAPI cuvidUnmapVideoFrame64(CUvideodecoder hDecoder, unsigned long long DevPtr);
        /// Unmap a previously mapped video frame
        /// </summary>
        [DllImport(_dllpath, EntryPoint = "cuvidUnmapVideoFrame64")]
        public static extern CuResult UnmapVideoFrame64(CuVideoDecoder decoder, CuDevicePtr devPtr);
    }
}