using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using static Lennox.NvEncSharp.LibNvVideo;

// ReSharper disable PureAttributeOnVoidMethod

namespace Lennox.NvEncSharp
{
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{" + nameof(Handle) + "}")]
    public struct CuVideoSource : IDisposable
    {
        public static readonly CuVideoSource Empty = new CuVideoSource { Handle = IntPtr.Zero };

        public IntPtr Handle;

        public bool IsEmpty => Handle == IntPtr.Zero;

        public void Dispose()
        {
            var handle = Interlocked.Exchange(ref Handle, IntPtr.Zero);
            if (handle == IntPtr.Zero) return;
            var obj = new CuVideoSource { Handle = handle };

            CheckResult(DestroyVideoSource(obj));
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{" + nameof(Handle) + "}")]
    public struct CuStream : IDisposable
    {
        public static readonly CuStream Empty = new CuStream { Handle = IntPtr.Zero };

        public IntPtr Handle;

        public bool IsEmpty => Handle == IntPtr.Zero;

        public static CuStream Create(
            CuStreamFlags flags = CuStreamFlags.Default)
        {
            var result = LibCuda.StreamCreate(out var stream, flags);
            CheckResult(result);

            return stream;
        }

        public static CuStream Create(
            int priority,
            CuStreamFlags flags = CuStreamFlags.Default)
        {
            var result = LibCuda.StreamCreateWithPriority(
                out var stream, flags, priority);
            CheckResult(result);

            return stream;
        }

        public CuResult Query()
        {
            return LibCuda.StreamQuery(this);
        }

        public bool HasPendingOperations()
        {
            return LibCuda.StreamQuery(this) == CuResult.ErrorNotReady;
        }

        public void Synchronize()
        {
            var result = LibCuda.StreamSynchronize(this);
            CheckResult(result);
        }

        public void Dispose()
        {
            var handle = Interlocked.Exchange(ref Handle, IntPtr.Zero);
            if (handle == IntPtr.Zero) return;
            var obj = new CuStream { Handle = handle };

            CheckResult(LibCuda.StreamDestroy(obj));
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{" + nameof(Handle) + "}")]
    public struct CuEvent
    {
        public static readonly CuEvent Empty = new CuEvent { Handle = IntPtr.Zero };

        public IntPtr Handle;

        public bool IsEmpty => Handle == IntPtr.Zero;
    }

    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{" + nameof(Handle) + "}")]
    public struct CuVideoDecoder : IDisposable
    {
        public static readonly CuVideoDecoder Empty = new CuVideoDecoder { Handle = IntPtr.Zero };

        public IntPtr Handle;

        public bool IsEmpty => Handle == IntPtr.Zero;

        public static CuVideoDecoder Create(ref CuVideoDecodeCreateInfo pdci)
        {
            var result = CreateDecoder(out var decoder, ref pdci);
            CheckResult(result);
            return decoder;
        }

        public CuVideoDecodeStatus GetDecodeStatus(int picIndex = 0)
        {
            var result = LibNvVideo.GetDecodeStatus(
                this, picIndex, out var status);
            CheckResult(result);
            return status;
        }

        public void Reconfigure(
            ref CuVideoReconfigureDecoderInfo decReconfigParams)
        {
            var result = ReconfigureDecoder(this, ref decReconfigParams);
            CheckResult(result);
        }

        public void Reconfigure(ref CuVideoFormat format)
        {
            // TODO
            var info = new CuVideoReconfigureDecoderInfo {
                Width = format.CodedWidth,
                Height = format.CodedHeight,
            };

            Reconfigure(ref info);
        }

        public void DecodePicture(ref CuVideoPicParams param)
        {
            var result = LibNvVideo.DecodePicture(this, ref param);
            CheckResult(result);
        }

        public CuDevicePtr MapVideoFrame(
            int picIndex, ref CuVideoProcParams param,
            out uint pitch)
        {
            CuDevicePtr devicePtr;

            var result = Environment.Is64BitProcess
                ? LibNvVideo.MapVideoFrame64(
                    this, picIndex, out devicePtr,
                    out pitch, ref param)
                : LibNvVideo.MapVideoFrame(
                    this, picIndex, out devicePtr,
                    out pitch, ref param);

            CheckResult(result);

            return devicePtr;
        }

        public void Dispose()
        {
            var handle = Interlocked.Exchange(ref Handle, IntPtr.Zero);
            if (handle == IntPtr.Zero) return;
            var obj = new CuVideoDecoder { Handle = handle };

            CheckResult(DestroyDecoder(obj));
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{" + nameof(Handle) + "}")]
    public struct CuVideoFrame
    {
        public IntPtr Handle;

        public bool IsEmpty => Handle == IntPtr.Zero;
    }

    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{" + nameof(Handle) + "}")]
    public struct CuVideoFrame64 //: IDisposable
    {
        public long Handle;

        public bool IsEmpty => Handle == 0;

        /*public void Dispose()
        {
            var handle = Interlocked.Exchange(ref Handle, 0);
            if (handle == 0) return;
            var obj = new CuVideoFrame64 { Handle = handle };

            CheckResult(UnmapVideoFrame64(obj));
        }*/
    }

    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{" + nameof(Handle) + "}")]
    public unsafe struct CuVideoParser : IDisposable
    {
        public IntPtr Handle;

        public bool IsEmpty => Handle == IntPtr.Zero;

        [Pure]
        public void ParseVideoData(ref CuVideoSourceDataPacket packet)
        {
            var result = LibNvVideo.ParseVideoData(this, ref packet);
            CheckResult(result);
        }

        [Pure]
        public void ParseVideoData(
            Span<byte> payload,
            CuVideoPacketFlags flags = CuVideoPacketFlags.None,
            long timestamp = 0)
        {
            fixed (byte* payloadPtr = payload)
            {
                var packet = new CuVideoSourceDataPacket
                {
                    Flags = flags,
                    Payload = payloadPtr,
                    PayloadSize = (uint)payload.Length,
                    Timestamp = timestamp
                };

                ParseVideoData(ref packet);
            }
        }

        [Pure]
        public void ParseVideoData(
            IntPtr payload,
            int payloadLength,
            CuVideoPacketFlags flags = CuVideoPacketFlags.None,
            long timestamp = 0)
        {
            var packet = new CuVideoSourceDataPacket
            {
                Flags = flags,
                Payload = (byte*)payload,
                PayloadSize = (uint)payloadLength,
                Timestamp = timestamp
            };

            ParseVideoData(ref packet);
        }

        [Pure]
        public void SendEndOfStream()
        {
            var eosPacket = new CuVideoSourceDataPacket
            {
                Flags = CuVideoPacketFlags.EndOfStream
            };

            ParseVideoData(ref eosPacket);
        }

        public static CuVideoParser Create(ref CuVideoParserParams @params)
        {
            var result = CreateVideoParser(out var parser, ref @params);
            CheckResult(result);
            return parser;
        }

        public void Dispose()
        {
            var handle = Interlocked.Exchange(ref Handle, IntPtr.Zero);
            if (handle == IntPtr.Zero) return;
            var obj = new CuVideoParser { Handle = handle };

            CheckResult(DestroyVideoParser(obj));
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{" + nameof(Handle) + "}")]
    public partial struct CuDevice
    {
        public int Handle;

        public bool IsEmpty => Handle == 0;

        public CuDevice(int handle)
        {
            Handle = handle;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{" + nameof(Handle) + "}")]
    public unsafe struct CuDevicePtr
    {
        public IntPtr Handle;

        public bool IsEmpty => Handle == IntPtr.Zero;

        public CuDevicePtr(IntPtr handle)
        {
            Handle = handle;
        }

        public CuDevicePtr(byte* handle)
        {
            Handle = (IntPtr)handle;
        }

        public static implicit operator ulong(CuDevicePtr d) => (ulong)d.Handle.ToInt64();
    }

    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{" + nameof(Handle) + "}")]
    public struct CuArray
    {
        public IntPtr Handle;

        public bool IsEmpty => Handle == IntPtr.Zero;

        public CuArray(IntPtr handle)
        {
            Handle = handle;
        }
    }

    [DebuggerDisplay("{" + nameof(DevicePtr) + "}")]
    public struct CuDevicePtrSafe
    {
        public CuDevicePtr DevicePtr;
        private CuDevicePtrDeallocator _deallocator;

        public delegate CuResult CuDevicePtrDeallocator(CuDevicePtr dptr);

        public CuDevicePtrSafe(
            CuDevicePtr devicePtr,
            CuDevicePtrDeallocator deallocator)
        {
            DevicePtr = devicePtr;
            _deallocator = deallocator;
        }

        public static implicit operator CuDevicePtr(CuDevicePtrSafe d) => d.DevicePtr;
    }

    /// <summary>
    /// Wraps the CuDevicePtr with a safe memory deallocator.
    /// </summary>
    [DebuggerDisplay("{" + nameof(Handle) + "}")]
    public struct CuDeviceMemory : IDisposable
    {
        public IntPtr Handle;

        public bool IsEmpty => Handle == IntPtr.Zero;

        private CuDeviceMemory(CuDevicePtr devicePtr)
        {
            Handle = devicePtr.Handle;
        }

        public static CuDeviceMemory Allocate(IntPtr bytesize)
        {
            var result = LibCuda.MemAlloc(out var device, bytesize);
            CheckResult(result);

            return new CuDeviceMemory(device);
        }

        public static CuDeviceMemory AllocatePitch(
            out IntPtr pitch, IntPtr widthInBytes,
            IntPtr height, uint elementSizeBytes)
        {
            var result = LibCuda.MemAllocPitch(
                out var device, out pitch,
                widthInBytes, height, elementSizeBytes);

            CheckResult(result);

            return new CuDeviceMemory(device);
        }

        public void Dispose()
        {
            var handle = Interlocked.Exchange(ref Handle, IntPtr.Zero);
            if (handle == IntPtr.Zero) return;
            var obj = new CuDevicePtr { Handle = handle };

            LibCuda.MemFree(obj);
        }

        public static implicit operator CuDevicePtr(CuDeviceMemory d) => new CuDevicePtr(d.Handle);
        public static implicit operator IntPtr(CuDeviceMemory d) => d.Handle;
    }

    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{" + nameof(Value) + "}")]
    public struct ByteBool
    {
        public byte Value;

        public ByteBool(bool b)
        {
            Value = (byte) (b ? 1 : 0);
        }

        public static implicit operator bool(ByteBool d) => d.Value != 0;
        public static implicit operator ByteBool(bool d) => new ByteBool(d);
    }

    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{" + nameof(Handle) + "}")]
    public struct CuVideoContextLock : IDisposable
    {
        public IntPtr Handle;

        public bool IsEmpty => Handle == IntPtr.Zero;

        public struct AutoCuVideoContextLock : IDisposable
        {
            private readonly CuVideoContextLock _lock;

            public AutoCuVideoContextLock(CuVideoContextLock lok)
            {
                _lock = lok;
            }

            public void Dispose()
            {
                _lock.Unlock();
            }
        }

        [Pure]
        public AutoCuVideoContextLock Lock()
        {
            CheckResult(CtxLock(this, 0));
            return new AutoCuVideoContextLock(this);
        }

        [Pure]
        public void Unlock()
        {
            CheckResult(CtxUnlock(this, 0));
        }

        public void Dispose()
        {
            var handle = Interlocked.Exchange(ref Handle, IntPtr.Zero);
            if (handle == IntPtr.Zero) return;
            var obj = new CuVideoContextLock { Handle = handle };

            CheckResult(CtxLockDestroy(obj));
        }
    }

    public unsafe class LibNvVideo
    {
        private const string _dllpath = "nvcuvid.dll";

        /// <summary>
        /// typedef int (CUDAAPI *PFNVIDSOURCECALLBACK)(void *, CUVIDSOURCEDATAPACKET *);
        /// Callback for packet delivery
        /// </summary>
        public delegate int VideoSourceCallback(byte* data, CuVideoSourceDataPacket* packet);

        // typedef int (CUDAAPI *PFNVIDSEQUENCECALLBACK)(void *, CUVIDEOFORMAT *);
        public delegate int VideoSequenceCallback(byte* data, ref CuVideoFormat pvidfmt);
        // typedef int (CUDAAPI* PFNVIDDECODECALLBACK) (void*, CUVIDPICPARAMS*);
        public delegate int VideoDecodeCallback(byte* data, ref CuVideoPicParams param);
        // typedef int (CUDAAPI* PFNVIDDISPLAYCALLBACK) (void*, CUVIDPARSERDISPINFO*);
        public delegate int VideoDisplayCallback(byte* data, ref CuVideoParseDisplayInfo info);

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
            out CuDevicePtr devPtr, out uint pitch, ref CuVideoProcParams vpp);

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
            out CuDevicePtr devPtr, out uint pitch, ref CuVideoProcParams vpp);

        /// <summary>
        /// \fn CUresult CuAPI cuvidUnmapVideoFrame64(CUvideodecoder hDecoder, unsigned long long DevPtr);
        /// Unmap a previously mapped video frame
        /// </summary>
        [DllImport(_dllpath, EntryPoint = "cuvidUnmapVideoFrame64")]
        public static extern CuResult UnmapVideoFrame64(CuVideoDecoder decoder, CuDevicePtr devPtr);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CheckResult(
            CuResult result,
            [CallerMemberName] string callerName = "")
        {
            if (result != CuResult.Success)
            {
                throw new LibNvEncException(callerName, result);
            }
        }
    }

    public enum CuResult
    {
        /// <summary>
        /// The API call returned with no errors. In the case of query calls, this
        /// can also mean that the operation being queried is complete (see
        /// ::cuEventQuery() and ::cuStreamQuery()).
        /// </summary>
        Success = 0,

        /// <summary>
        /// This indicates that one or more of the parameters passed to the API call
        /// is not within an acceptable range of values.
        /// </summary>
        ErrorInvalidValue = 1,

        /// <summary>
        /// The API call failed because it was unable to allocate enough memory to
        /// perform the requested operation.
        /// </summary>
        ErrorOutOfMemory = 2,

        /// <summary>
        /// This indicates that the CUDA driver has not been initialized with
        /// ::cuInit() or that initialization has failed.
        /// </summary>
        ErrorNotInitialized = 3,

        /// <summary>
        /// This indicates that the CUDA driver is in the process of shutting down.
        /// </summary>
        ErrorDeinitialized = 4,

        /// <summary>
        /// This indicates profiler is not initialized for this run. This can
        /// happen when the application is running with external profiling tools
        /// like visual profiler.
        /// </summary>
        ErrorProfilerDisabled = 5,

        /// <summary>
        /// \deprecated
        /// This error return is deprecated as of CUDA 5.0. It is no longer an error
        /// to attempt to enable/disable the profiling via ::cuProfilerStart or
        /// ::cuProfilerStop without initialization.
        /// </summary>
        ErrorProfilerNotInitialized = 6,

        /// <summary>
        /// \deprecated
        /// This error return is deprecated as of CUDA 5.0. It is no longer an error
        /// to call cuProfilerStart() when profiling is already enabled.
        /// </summary>
        ErrorProfilerAlreadyStarted = 7,

        /// <summary>
        /// \deprecated
        /// This error return is deprecated as of CUDA 5.0. It is no longer an error
        /// to call cuProfilerStop() when profiling is already disabled.
        /// </summary>
        ErrorProfilerAlreadyStopped = 8,

        /// <summary>
        /// This indicates that no CUDA-capable devices were detected by the installed
        /// CUDA driver.
        /// </summary>
        ErrorNoDevice = 100,

        /// <summary>
        /// This indicates that the device ordinal supplied by the user does not
        /// correspond to a valid CUDA device.
        /// </summary>
        ErrorInvalidDevice = 101,

        /// <summary>
        /// This indicates that the device kernel image is invalid. This can also
        /// indicate an invalid CUDA module.
        /// </summary>
        ErrorInvalidImage = 200,

        /// <summary>
        /// This most frequently indicates that there is no context bound to the
        /// current thread. This can also be returned if the context passed to an
        /// API call is not a valid handle (such as a context that has had
        /// ::cuCtxDestroy() invoked on it). This can also be returned if a user
        /// mixes different API versions (i.e. 3010 context with 3020 API calls).
        /// See ::cuCtxGetApiVersion() for more details.
        /// </summary>
        ErrorInvalidContext = 201,

        /// <summary>
        /// This indicated that the context being supplied as a parameter to the
        /// API call was already the active context.
        /// \deprecated
        /// This error return is deprecated as of CUDA 3.2. It is no longer an
        /// error to attempt to push the active context via ::cuCtxPushCurrent().
        /// </summary>
        ErrorContextAlreadyCurrent = 202,

        /// <summary>
        /// This indicates that a map or register operation has failed.
        /// </summary>
        ErrorMapFailed = 205,

        /// <summary>
        /// This indicates that an unmap or unregister operation has failed.
        /// </summary>
        ErrorUnmapFailed = 206,

        /// <summary>
        /// This indicates that the specified array is currently mapped and thus
        /// cannot be destroyed.
        /// </summary>
        ErrorArrayIsMapped = 207,

        /// <summary>
        /// This indicates that the resource is already mapped.
        /// </summary>
        ErrorAlreadyMapped = 208,

        /// <summary>
        /// This indicates that there is no kernel image available that is suitable
        /// for the device. This can occur when a user specifies code generation
        /// options for a particular CUDA source file that do not include the
        /// corresponding device configuration.
        /// </summary>
        ErrorNoBinaryForGpu = 209,

        /// <summary>
        /// This indicates that a resource has already been acquired.
        /// </summary>
        ErrorAlreadyAcquired = 210,

        /// <summary>
        /// This indicates that a resource is not mapped.
        /// </summary>
        ErrorNotMapped = 211,

        /// <summary>
        /// This indicates that a mapped resource is not available for access as an
        /// array.
        /// </summary>
        ErrorNotMappedAsArray = 212,

        /// <summary>
        /// This indicates that a mapped resource is not available for access as a
        /// pointer.
        /// </summary>
        ErrorNotMappedAsPointer = 213,

        /// <summary>
        /// This indicates that an uncorrectable ECC error was detected during
        /// execution.
        /// </summary>
        ErrorEccUncorrectable = 214,

        /// <summary>
        /// This indicates that the ::CUlimit passed to the API call is not
        /// supported by the active device.
        /// </summary>
        ErrorUnsupportedLimit = 215,

        /// <summary>
        /// This indicates that the ::CUcontext passed to the API call can
        /// only be bound to a single CPU thread at a time but is already
        /// bound to a CPU thread.
        /// </summary>
        ErrorContextAlreadyInUse = 216,

        /// <summary>
        /// This indicates that peer access is not supported across the given
        /// devices.
        /// </summary>
        ErrorPeerAccessUnsupported = 217,

        /// <summary>
        /// This indicates that the device kernel source is invalid.
        /// </summary>
        ErrorInvalidSource = 300,

        /// <summary>
        /// This indicates that the file specified was not found.
        /// </summary>
        ErrorFileNotFound = 301,

        /// <summary>
        /// This indicates that a link to a shared object failed to resolve.
        /// </summary>
        ErrorSharedObjectSymbolNotFound = 302,

        /// <summary>
        /// This indicates that initialization of a shared object failed.
        /// </summary>
        ErrorSharedObjectInitFailed = 303,

        /// <summary>
        /// This indicates that an OS call failed.
        /// </summary>
        ErrorOperatingSystem = 304,

        /// <summary>
        /// This indicates that a resource handle passed to the API call was not
        /// valid. Resource handles are opaque types like ::CUstream and ::CUevent.
        /// </summary>
        ErrorInvalidHandle = 400,

        /// <summary>
        /// This indicates that a named symbol was not found. Examples of symbols
        /// are global/constant variable names, texture names, and surface names.
        /// </summary>
        ErrorNotFound = 500,

        /// <summary>
        /// This indicates that asynchronous operations issued previously have not
        /// completed yet. This result is not actually an error, but must be indicated
        /// differently than ::CUDA_SUCCESS (which indicates completion). Calls that
        /// may return this value include ::cuEventQuery() and ::cuStreamQuery().
        /// </summary>
        ErrorNotReady = 600,

        /// <summary>
        /// An exception occurred on the device while executing a kernel. Common
        /// causes include dereferencing an invalid device pointer and accessing
        /// out of bounds shared memory. The context cannot be used, so it must
        /// be destroyed (and a new one should be created). All existing device
        /// memory allocations from this context are invalid and must be
        /// reconstructed if the program is to continue using CUDA.
        /// </summary>
        ErrorLaunchFailed = 700,

        /// <summary>
        /// This indicates that a launch did not occur because it did not have
        /// appropriate resources. This error usually indicates that the user has
        /// attempted to pass too many arguments to the device kernel, or the
        /// kernel launch specifies too many threads for the kernel's register
        /// count. Passing arguments of the wrong size (i.e. a 64-bit pointer
        /// when a 32-bit int is expected) is equivalent to passing too many
        /// arguments and can also result in this error.
        /// </summary>
        ErrorLaunchOutOfResources = 701,

        /// <summary>
        /// This indicates that the device kernel took too long to execute. This can
        /// only occur if timeouts are enabled - see the device attribute
        /// ::CU_DEVICE_ATTRIBUTE_KERNEL_EXEC_TIMEOUT for more information. The
        /// context cannot be used (and must be destroyed similar to
        /// ::CUDA_ERROR_LAUNCH_FAILED). All existing device memory allocations from
        /// this context are invalid and must be reconstructed if the program is to
        /// continue using CUDA.
        /// </summary>
        ErrorLaunchTimeout = 702,

        /// <summary>
        /// This error indicates a kernel launch that uses an incompatible texturing
        /// mode.
        /// </summary>
        ErrorLaunchIncompatibleTexturing = 703,

        /// <summary>
        /// This error indicates that a call to ::cuCtxEnablePeerAccess() is
        /// trying to re-enable peer access to a context which has already
        /// had peer access to it enabled.
        /// </summary>
        ErrorPeerAccessAlreadyEnabled = 704,

        /// <summary>
        /// This error indicates that ::cuCtxDisablePeerAccess() is
        /// trying to disable peer access which has not been enabled yet
        /// via ::cuCtxEnablePeerAccess().
        /// </summary>
        ErrorPeerAccessNotEnabled = 705,

        /// <summary>
        /// This error indicates that the primary context for the specified device
        /// has already been initialized.
        /// </summary>
        ErrorPrimaryContextActive = 708,

        /// <summary>
        /// This error indicates that the context current to the calling thread
        /// has been destroyed using ::cuCtxDestroy, or is a primary context which
        /// has not yet been initialized.
        /// </summary>
        ErrorContextIsDestroyed = 709,

        /// <summary>
        /// A device-side assert triggered during kernel execution. The context
        /// cannot be used anymore, and must be destroyed. All existing device
        /// memory allocations from this context are invalid and must be
        /// reconstructed if the program is to continue using CUDA.
        /// </summary>
        ErrorAssert = 710,

        /// <summary>
        /// This error indicates that the hardware resources required to enable
        /// peer access have been exhausted for one or more of the devices
        /// passed to ::cuCtxEnablePeerAccess().
        /// </summary>
        ErrorTooManyPeers = 711,

        /// <summary>
        /// This error indicates that the memory range passed to ::cuMemHostRegister()
        /// has already been registered.
        /// </summary>
        ErrorHostMemoryAlreadyRegistered = 712,

        /// <summary>
        /// This error indicates that the pointer passed to ::cuMemHostUnregister()
        /// does not correspond to any currently registered memory region.
        /// </summary>
        ErrorHostMemoryNotRegistered = 713,

        /// <summary>
        /// This error indicates that the attempted operation is not permitted.
        /// </summary>
        ErrorNotPermitted = 800,

        /// <summary>
        /// This error indicates that the attempted operation is not supported
        /// on the current system or device.
        /// </summary>
        ErrorNotSupported = 801,

        /// <summary>
        /// This indicates that an unknown internal error has occurred.
        /// </summary>
        ErrorUnknown = 999
    }

    /// <summary>
    /// \enum cudaVideoState
    /// Video source state enums
    /// Used in cuvidSetVideoSourceState and cuvidGetVideoSourceState APIs
    /// </summary>
    public enum CuVideoState
    {
        /// <summary>Error state (invalid source)</summary>
        Error = -1,
        /// <summary>Source is stopped (or reached end-of-stream)</summary>
        Stopped = 0,
        /// <summary>Source is running and delivering data</summary>
        Started = 1
    }

    /// <summary>
    /// \enum cudaVideoCodec
    /// Video codec enums
    /// These enums are used in CUVIDDECODECREATEINFO and CUVIDDECODECAPS structures
    /// </summary>
    public enum CuVideoCodec
    {
        MPEG1 = 0,
        MPEG2,
        MPEG4,
        VC1,
        H264,
        JPEG,
        H264_SVC,
        H264_MVC,
        HEVC,
        VP8,
        VP9,
        NumCodecs,
        /// <summary>Uncompressed: Y,U,V (4:2:0)</summary>
        YUV420 = (('I' << 24) | ('Y' << 16) | ('U' << 8) | ('V')),
        /// <summary>Uncompressed: Y,V,U (4:2:0)</summary>
        YV12 = (('Y' << 24) | ('V' << 16) | ('1' << 8) | ('2')),
        /// <summary>Uncompressed: Y,UV  (4:2:0)</summary>
        NV12 = (('N' << 24) | ('V' << 16) | ('1' << 8) | ('2')),
        /// <summary>Uncompressed: YUYV/YUY2 (4:2:2)</summary>
        YUYV = (('Y' << 24) | ('U' << 16) | ('Y' << 8) | ('V')),
        /// <summary>Uncompressed: UYVY (4:2:2)</summary>
        UYVY = (('U' << 24) | ('Y' << 16) | ('V' << 8) | ('Y'))
    }

    /// <summary>
    /// \enum cudaVideoSurfaceFormat
    /// Video surface format enums used for output format of decoded output
    /// These enums are used in CUVIDDECODECREATEINFO structure
    ///</summary>
    public enum CuVideoSurfaceFormat
    {
        /// <summary>NV12</summary>
        Default = 0,
        /// <summary>Semi-Planar YUV [Y plane followed by interleaved UV plane]</summary>
        NV12 = 0,
        /// <summary>16 bit Semi-Planar YUV [Y plane followed by interleaved UV plane].
        /// Can be used for 10 bit(6LSB bits 0), 12 bit (4LSB bits 0)</summary>
        P016 = 1,
        /// <summary>Planar YUV [Y plane followed by U and V planes]</summary>
        YUV444 = 2,
        /// <summary>16 bit Planar YUV [Y plane followed by U and V planes].
        /// Can be used for 10 bit(6LSB bits 0), 12 bit (4LSB bits 0)</summary>
        YUV444_16Bit = 3,
    }

    /// <summary>
    /// \enum cudaVideoDeinterlaceMode
    /// Deinterlacing mode enums
    /// These enums are used in CUVIDDECODECREATEINFO structure
    /// Use CuVideoDeinterlaceMode_Weave for progressive content and for content that doesn't need deinterlacing
    /// CuVideoDeinterlaceMode_Adaptive needs more video memory than other DImodes
    ///</summary>
    public enum CuVideoDeinterlaceMode
    {
        /// <summary>Weave both fields (no deinterlacing)</summary>
        Weave = 0,
        /// <summary>Drop one field</summary>
        Bob,
        /// <summary>Adaptive deinterlacing</summary>
        Adaptive
    }

    /// <summary>
    /// \enum cudaVideoChromaFormat
    /// Chroma format enums
    /// These enums are used in CUVIDDECODECREATEINFO and CUVIDDECODECAPS structures
    /// </summary>
    public enum CuVideoChromaFormat
    {
        /// <summary>MonoChrome</summary>
        Monochrome = 0,
        /// <summary>YUV 4:2:0</summary>
        YUV420,
        /// <summary>YUV 4:2:2</summary>
        YUV422,
        /// <summary>YUV 4:4:4</summary>
        YUV444
    }

    /// <summary>
    /// \enum CuVideoCreateFlags
    /// Decoder flag enums to select preferred decode path
    /// CuVideoCreate_Default and CuVideoCreate_PreferCUVID are most optimized, use these whenever possible
    ///</summary>
    [Flags]
    public enum CuVideoCreateFlags
    {
        /// <summary>Default operation mode: use dedicated video engines</summary>
        Default = 0x00,
        /// <summary>Use Cu-based decoder (requires valid vidLock object for multi-threading)</summary>
        PreferCUDA = 0x01,
        /// <summary>Go through DXVA internally if possible (requires D3D9 interop)</summary>
        PreferDXVA = 0x02,
        /// <summary>Use dedicated video engines directly</summary>
        PreferCUVID = 0x04
    }

    /// <summary>
    /// \enum cuvidDecodeStatus
    /// Decode status enums
    /// These enums are used in CUVIDGETDECODESTATUS structure
    ///</summary>
    public enum CuVideoDecodeStatus
    {
        /// <summary>Decode status is not valid</summary>
        Invalid = 0,
        /// <summary>Decode is in progress</summary>
        InProgress = 1,
        /// <summary>Decode is completed without any errors</summary>
        Success = 2,
        // 3 to 7 enums are reserved for future use
        /// <summary>Decode is completed with an error (error is not concealed)</summary>
        Error = 8,
        /// <summary>Decode is completed with an error and error is concealed</summary>
        ErrorConcealed = 9
    }

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
        public uint BitDepthMinus8;
        /// <summary>Reserved for future use - set to zero</summary>
        private fixed uint _reserved1[3];

        /// <summary>OUT: 1 if codec supported, 0 if not supported</summary>
        public bool IsSupported;
        /// <summary>Reserved for future use - set to zero</summary>
        private byte _reserved2;
        /// <summary>OUT: each bit represents corresponding CuVideoSurfaceFormat enum</summary>
        public ushort OutputFormatMask;
        /// <summary>OUT: Max supported coded width in pixels</summary>
        public uint MaxWidth;
        /// <summary>OUT: Max supported coded height in pixels</summary>
        public uint MaxHeight;
        /// <summary>OUT: Max supported macroblock count
        /// CodedWidth*CodedHeight/256 must be <= nMaxMBCount</summary>
        public uint MaxMBCount;
        /// <summary>OUT: Min supported coded width in pixels</summary>
        public ushort MinWidth;
        /// <summary>OUT: Min supported coded height in pixels</summary>
        public ushort MinHeight;
        /// <summary>Reserved for future use - set to zero</summary>
        private fixed uint _reserved3[11];
    }

    /// <summary>
    /// \struct CUVIDDECODECREATEINFO
    /// This structure is used in cuvidCreateDecoder API
    ///</summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct CuVideoDecodeCreateInfo
    {
        /// <summary>IN: Coded sequence width in pixels</summary>
        public uint Width;
        /// <summary>IN: Coded sequence height in pixels</summary>
        public uint Height;
        /// <summary>IN: Maximum number of internal decode surfaces</summary>
        public uint NumDecodeSurfaces;
        /// <summary>IN: CuVideoCodec_XXX</summary>
        public CuVideoCodec CodecType;
        /// <summary>IN: CuVideoChromaFormat_XXX</summary>
        public CuVideoChromaFormat ChromaFormat;
        /// <summary>IN: Decoder creation flags (cudaVideoCreateFlags_XXX)</summary>
        public CuVideoCreateFlags CreationFlags;
        /// <summary>IN: The value "BitDepth minus 8"</summary>
        public uint BitDepthMinus8;
        /// <summary>IN: Set 1 only if video has all intra frames (default value is 0). This will
        /// optimize video memory for Intra frames only decoding. The support is limited
        /// to specific codecs - H264, HEVC, VP9, the flag will be ignored for codecs which
        /// are not supported. However decoding might fail if the flag is enabled in case
        /// of supported codecs for regular bit streams having P and/or B frames.</summary>
        public uint IntraDecodeOnly;
        /// <summary>IN: Coded sequence max width in pixels used with reconfigure Decoder</summary>
        public uint MaxWidth;
        /// <summary>IN: Coded sequence max height in pixels used with reconfigure Decoder</summary>
        public uint MaxHeight;
        /// <summary>Reserved for future use - set to zero</summary>
        private uint _reserved1;
        /// <summary>IN: area of the frame that should be displayed</summary>
        public CuRectangleShort DisplayArea;

        /// <summary>IN: CuVideoSurfaceFormat_XXX</summary>
        public CuVideoSurfaceFormat OutputFormat;
        /// <summary>IN: CuVideoDeinterlaceMode_XXX</summary>
        public CuVideoDeinterlaceMode DeinterlaceMode;
        /// <summary>IN: Post-processed output width (Should be aligned to 2)</summary>
        public uint TargetWidth;
        /// <summary>IN: Post-processed output height (Should be aligned to 2)</summary>
        public uint TargetHeight;
        /// <summary>IN: Maximum number of output surfaces simultaneously mapped</summary>
        public uint NumOutputSurfaces;
        /// <summary>IN: If non-NULL, context lock used for synchronizing ownership of
        /// the Cu context. Needed for CuVideoCreate_PreferCUDA decode</summary>
        public CuVideoContextLock VideoLock;
        /// <summary>IN: target rectangle in the output frame (for aspect ratio conversion)</summary>
        public CuRectangleShort TargetRect;
        private fixed uint _reserved2[5];

        public int GetBitsPerPixel()
        {
            return BitDepthMinus8 > 0 ? 2 : 1;
        }

        public IntPtr GetFrameSize()
        {
            var chromaInfo = new CuVideoChromaFormatInformation(ChromaFormat);
            var chromaHeight = Height * chromaInfo.HeightFactor;
            var height = Height + chromaHeight * chromaInfo.PlaneCount;
            return new IntPtr((long)(Width * height * GetBitsPerPixel()));
        }
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
        public uint FrameRateNumerator;
        /// <summary>OUT: frame rate denominator (0 = unspecified or variable frame rate)</summary>
        public uint FrameRateDenominator;
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
        public uint CodedWidth;
        /// <summary>OUT: coded frame height in pixels</summary>
        public uint CodedHeight;
        public CuRectangle DisplayArea;
        /// <summary>OUT:  Chroma format</summary>
        public CuVideoChromaFormat ChromaFormat;
        /// <summary>OUT: video bitrate (bps, 0=unknown)</summary>
        public uint Bitrate;
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
        public uint SeqhdrDataLength;

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
        public fixed byte RawSeqhdrData[1024];
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
        //[MarshalAs(UnmanagedType.FunctionPtr)]
        public LibNvVideo.VideoSequenceCallback SequenceCallback;
        /// <summary>IN: Called when a picture is ready to be decoded (decode order)</summary>
        //[MarshalAs(UnmanagedType.FunctionPtr)]
        public LibNvVideo.VideoDecodeCallback DecodePicture;
        /// <summary>IN: Called whenever a picture is ready to be displayed (display order)</summary>
        //[MarshalAs(UnmanagedType.FunctionPtr)]
        public LibNvVideo.VideoDisplayCallback DisplayPicture;
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
    /// \struct CUAUDIOFORMAT
    /// Audio formats
    /// Used in cuvidGetSourceAudioFormat API
    /// </summary>
    public enum CuAudioCodec
    {
        /// <summary>MPEG-1 Audio</summary>
        MPEG1 = 0,
        /// <summary>MPEG-2 Audio</summary>
        MPEG2,
        /// <summary>MPEG-1 Layer III Audio</summary>
        MP3,
        /// <summary>Dolby Digital (AC3) Audio</summary>
        AC3,
        /// <summary>PCM Audio</summary>
        LPCM,
        /// <summary>AAC Audio</summary>
        AAC,
    }

    /// <summary>
    /// \enum CUvideopacketflags
    /// Data packet flags
    /// Used in CUVIDSOURCEDATAPACKET structure
    /// </summary>
    [Flags]
    public enum CuVideoPacketFlags
    {
        None = 0,
        /// <summary>CUVID_PKT_ENDOFSTREAM: Set when this is the last packet for this stream</summary>
        EndOfStream = 0x01,
        /// <summary>CUVID_PKT_TIMESTAMP: Timestamp is valid</summary>
        Timestamp = 0x02,
        /// <summary>CUVID_PKT_DISCONTINUITY: Set when a discontinuity has to be signalled </summary>
        Discontinuity = 0x04,
        /// <summary>CUVID_PKT_ENDOFPICTURE: Set when the packet contains exactly one frame or one field</summary>
        EndOfPicture = 0x08,
        /// <summary>CUVID_PKT_NOTIFY_EOS: If this flag is set along with CUVID_PKT_ENDOFSTREAM, an additional (dummy)
        /// display callback will be invoked with null value of CUVIDPARSERDISPINFO which
        /// should be interpreted as end of the stream.</summary>
        NotifyEos = 0x10,
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
        public uint ClockRate;
        /// <summary>Reserved for future use - set to zero</summary>
        private fixed uint _reserved1[7];
        /// <summary>IN: User private data passed in to the data handlers</summary>
        public IntPtr UserData;
        /// <summary>IN: Called to deliver video packets</summary>
        // TODO: Fix non-delegate type.
        public LibNvVideo.VideoSourceCallback VideoDataHandler;
        /// <summary>IN: Called to deliver audio packets.</summary>
        public LibNvVideo.VideoSourceCallback AudioDataHandler;
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
    /// \enum cudaVideosourceformat_flags
    /// CUvideosourceformat_flags
    /// Used in cuvidGetSourceVideoFormat API
    /// </summary>
    public enum CuVideoSourceFormat
    {
        Default = 0,
        /// <summary>Return extended format structure (CUVIDEOFORMATEX)</summary>
        ExtFormatInfo = 0x100
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
        public byte profile_idc;
        public byte level_idc;
        public byte DQId;
        public byte DQIdMax;
        public byte disable_inter_layer_deblocking_filter_idc;
        public byte ref_layer_chroma_phase_y_plus1;
        public sbyte inter_layer_slice_alpha_c0_offset_div2;
        public sbyte inter_layer_slice_beta_offset_div2;

        public ushort DPBEntryValidFlag;
        public byte inter_layer_deblocking_filter_control_present_flag;
        public byte extended_spatial_scalability_idc;
        public byte adaptive_tcoeff_level_prediction_flag;
        public byte slice_header_restriction_flag;
        public byte chroma_phase_x_plus1_flag;
        public byte chroma_phase_y_plus1;

        public byte tcoeff_level_prediction_flag;
        public byte constrained_intra_resampling_flag;
        public byte ref_layer_chroma_phase_x_plus1_flag;
        public byte store_ref_base_pic_flag;
        private byte _reserved8BitsA;
        private byte _reserved8BitsB;

        public short scaled_ref_layer_left_offset;
        public short scaled_ref_layer_top_offset;
        public short scaled_ref_layer_right_offset;
        public short scaled_ref_layer_bottom_offset;
        private ushort _reserved16Bits;
        /// <summary>Points to the PicParams for the next layer to be decoded.
        /// Linked list ends at the target layer.</summary>
        public CuVideoPicParams* pNextLayer;
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
        public CuVideoH264DpbEntry Dpb1;
        public CuVideoH264DpbEntry Dpb2;
        public CuVideoH264DpbEntry Dpb3;
        public CuVideoH264DpbEntry Dpb4;
        public CuVideoH264DpbEntry Dpb5;
        public CuVideoH264DpbEntry Dpb6;
        public CuVideoH264DpbEntry Dpb7;
        public CuVideoH264DpbEntry Dpb8;
        public CuVideoH264DpbEntry Dpb9;
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
        public char* BitstreamData;
        /// <summary>IN: Number of slices in this picture</summary>
        public uint NumSlices;
        /// <summary>IN: nNumSlices entries, contains offset of each slice within the bitstream data buffer</summary>
        public int* SliceDataOffsets;
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
        public uint RawInputPitch;
        /// <summary>IN: Input YUV format (cudaVideoCodec_enum)</summary>
        public uint RawInputFormat;
        /// <summary>IN: Output CUdeviceptr for raw YUV extensions</summary>
        public ulong RawOutputDptr;
        /// <summary>IN: pitch in bytes of raw YUV output (should be aligned appropriately)</summary>
        public uint RawOutputPitch;
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
        public uint Width;
        /// <summary>IN: Coded sequence height in pixels, MUST be &lt;= ulMaxHeight defined at CUVIDDECODECREATEINFO</summary>
        public uint Height;
        /// <summary>IN: Post processed output width</summary>
        public uint TargetWidth;
        /// <summary>IN: Post Processed output height</summary>
        public uint TargetHeight;
        /// <summary>IN: Maximum number of internal decode surfaces</summary>
        public uint NumDecodeSurfaces;
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
    }
}