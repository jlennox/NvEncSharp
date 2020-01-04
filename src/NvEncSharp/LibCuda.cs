using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Threading;
using static Lennox.NvEncSharp.LibNvVideo;

namespace Lennox.NvEncSharp
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CuContext : IDisposable
    {
        public IntPtr Handle;

        public struct CuContextPush : IDisposable
        {
            private CuContext _context;
            private int _disposed;

            public CuContextPush(CuContext context)
            {
                _context = context;
                _disposed = 0;
            }

            public void Dispose()
            {
                var disposed = Interlocked.Exchange(ref _disposed, 1);
                if (disposed != 0) return;

                LibCuda.CtxPopCurrent(out _);
            }
        }

        [Pure]
        public CuVideoContextLock CreateLock()
        {
            var result = CtxLockCreate(out var lok, this);
            CheckResult(result);

            return lok;
        }

        [Pure]
        public CuContextPush Push()
        {
            LibCuda.CtxPushCurrent(this);
            return new CuContextPush(this);
        }

        public void Dispose()
        {
            var handle = Interlocked.Exchange(ref Handle, IntPtr.Zero);
            if (handle == IntPtr.Zero) return;
            var obj = new CuContext { Handle = handle };

            LibCuda.CtxDestroy(obj);
        }
    }

    [Flags]
    public enum CuContextFlags
    {
        /// <summary>Sets SchedAuto.</summary>
        Default = 0x00,
        /// <summary>Automatic scheduling</summary>
        SchedAuto = 0x00,

        /// <summary>Set spin as default scheduling</summary>
        SchedSpin = 0x01,

        /// <summary>Set yield as default scheduling</summary>
        SchedYield = 0x02,

        /// <summary>Set blocking synchronization as default scheduling</summary>
        SchedBlockingSync = 0x04,

        /// <summary>Set blocking synchronization as default scheduling
        /// \deprecated This flag was deprecated as of CUDA 4.0
        /// and was replaced with ::CU_CTX_SCHED_BLOCKING_SYNC.</summary>
        BlockingSync = 0x04,

        SchedMask = 0x07,

        /// <summary>Support mapped pinned allocations</summary>
        MapHost = 0x08,

        /// <summary>Keep local memory allocation after launch</summary>
        LmemResizeToMax = 0x10,

        FlagsMask = 0x1f
    }

    public unsafe class LibCuda
    {
        private const string _dllpath = "nvcuda.dll";

        /// <summary>CUresult cuInit(unsigned int Flags)
        /// Initialize the CUDA driver API.</summary>
        [DllImport(_dllpath, EntryPoint = "cuInit")]
        public static extern CuResult Initialize(uint flags);

        public static void Initialize()
        {
            CheckResult(Initialize(0));
        }

        /// <summary>Returns the CUDA driver version
        ///
        /// Returns in \p *driverVersion the version number of the installed CUDA
        /// driver. This function automatically returns ::CUDA_ERROR_INVALID_VALUE if
        /// the <c>driverVersion</c> argument is NULL.</summary>
        ///
        /// <param name="driverVersion">Returns the CUDA driver version</param>
        /// \return
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// \notefnerr
        ///
        /// \sa
        /// ::cudaDriverGetVersion,
        /// ::cudaRuntimeGetVersion

        /// CUresult CUDAAPI cuDriverGetVersion(int *driverVersion);
        [DllImport(_dllpath, EntryPoint = "cuDriverGetVersion")]
        public static extern CuResult DriverGetVersion(out int driverVersion);

        public static int DriverGetVersion()
        {
            CheckResult(DriverGetVersion(out var version));
            return version;
        }

        /// <summary>Gets the string description of an error code
        ///
        /// Sets \p *pStr to the address of a NULL-terminated string description
        /// of the error code <c>error</c>.
        /// If the error code is not recognized, ::CUDA_ERROR_INVALID_VALUE
        /// will be returned and \p *pStr will be set to the NULL address.</summary>
        ///
        /// <param name="error">Error code to convert to string</param>
        /// <param name="str">Address of the string pointer.</param>
        /// \return
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_INVALID_VALUE
        ///
        /// \sa
        /// ::CUresult,
        /// ::cudaGetErrorString

        /// CUresult CUDAAPI cuGetErrorString(CUresult error, const char **pStr);
        [DllImport(_dllpath, EntryPoint = "cuGetErrorString")]
        public static extern CuResult GetErrorString(CuResult error, out IntPtr str);

        public static string GetErrorString(CuResult error)
        {
            CheckResult(GetErrorString(error, out var str));
            return Marshal.PtrToStringAnsi(str);
        }

        /// <summary>Gets the string representation of an error code enum name
        ///
        /// Sets \p *pStr to the address of a NULL-terminated string representation
        /// of the name of the enum error code <c>error</c>.
        /// If the error code is not recognized, ::CUDA_ERROR_INVALID_VALUE
        /// will be returned and \p *pStr will be set to the NULL address.</summary>
        ///
        /// <param name="error">Error code to convert to string</param>
        /// <param name="str">Address of the string pointer.</param>
        /// \return
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_INVALID_VALUE
        ///
        /// \sa
        /// ::CUresult,
        /// ::cudaGetErrorName

        /// CUresult CUDAAPI cuGetErrorName(CUresult error, const char **pStr);
        [DllImport(_dllpath, EntryPoint = "cuGetErrorName")]
        public static extern CuResult GetErrorName(CuResult error, out IntPtr str);

        public static string GetErrorName(CuResult error)
        {
            CheckResult(GetErrorString(error, out var str));
            return Marshal.PtrToStringAnsi(str);
        }

        /// <summary>CUresult cuDeviceGet ( CUdevice* device, int  ordinal )
        /// Returns a handle to a compute device.</summary>
        [DllImport(_dllpath, EntryPoint = "cuDeviceGet")]
        public static extern CuResult DeviceGet(out CuDevice device, int ordinal);

        /// <summary>CUresult cuDeviceGetAttribute ( int* pi, CUdevice_attribute attrib, CUdevice dev )
        /// Returns information about the device.</summary>
        [DllImport(_dllpath, EntryPoint = "cuDeviceGetAttribute")]
        public static extern CuResult DeviceGetAttribute(out int pi, CuDeviceAttribute attrib, CuDevice device);

        /// <summary>CUresult cuDeviceGetCount ( int* count )
        /// Returns the number of compute-capable devices.</summary>
        [DllImport(_dllpath, EntryPoint = "cuDeviceGetCount")]
        public static extern CuResult DeviceGetCount(out int count);

        /// <summary>CUresult cuDeviceGetLuid ( char* luid, unsigned int* deviceNodeMask, CUdevice dev )
        /// Return an LUID and device node mask for the device.</summary>
        [DllImport(_dllpath, EntryPoint = "cuDeviceGetLuid")]
        public static extern CuResult DeviceGetLuid(out byte luid, out uint deviceNodeMask, CuDevice device);

        /// <summary>CUresult cuDeviceGetName ( char* name, int  len, CUdevice dev )
        /// Returns an identifer string for the device.</summary>
        [DllImport(_dllpath, EntryPoint = "cuDeviceGetName")]
        public static extern CuResult DeviceGetName(byte* name, int len, CuDevice device);

        /// <summary>CUresult cuDeviceGetNvSciSyncAttributes ( void* nvSciSyncAttrList, CUdevice dev, int  flags )
        /// Return NvSciSync attributes that this device can support.</summary>
        [DllImport(_dllpath, EntryPoint = "cuDeviceGetNvSciSyncAttributes")]
        public static extern CuResult DeviceGetNvSciSyncAttributes(IntPtr nvSciSyncAttrList, CuDevice device, int flags);

        /// <summary>CUresult cuDeviceGetUuid ( CUuuid* uuid, CUdevice dev )
        /// Return an UUID for the device.</summary>
        // TODO: Does CUuuid == GUID?
        [DllImport(_dllpath, EntryPoint = "cuDeviceGetUuid")]
        public static extern CuResult DeviceGetUuid(out Guid uuid, CuDevice device);

        /// <summary>CUresult cuDeviceTotalMem ( size_t* bytes, CUdevice dev )
        /// Returns the total amount of memory on the device.</summary>
        [DllImport(_dllpath, EntryPoint = "cuDeviceTotalMem")]
        public static extern CuResult DeviceTotalMemory(out IntPtr bytes, CuDevice device);

        #region Context
        /// <summary>Set flags for the primary context
        ///
        /// Sets the flags for the primary context on the device overwriting perviously
        /// set ones. If the primary context is already created
        /// ::CUDA_ERROR_PRIMARY_CONTEXT_ACTIVE is returned.
        ///
        /// The three LSBs of the <c>flags</c> parameter can be used to control how the OS
        /// thread, which owns the CUDA context at the time of an API call, interacts
        /// with the OS scheduler when waiting for results from the GPU. Only one of
        /// the scheduling flags can be set when creating a context.
        ///
        /// - ::CU_CTX_SCHED_SPIN: Instruct CUDA to actively spin when waiting for
        /// results from the GPU. This can decrease latency when waiting for the GPU,
        /// but may lower the performance of CPU threads if they are performing work in
        /// parallel with the CUDA thread.
        ///
        /// - ::CU_CTX_SCHED_YIELD: Instruct CUDA to yield its thread when waiting for
        /// results from the GPU. This can increase latency when waiting for the GPU,
        /// but can increase the performance of CPU threads performing work in parallel
        /// with the GPU.
        ///
        /// - ::CU_CTX_SCHED_BLOCKING_SYNC: Instruct CUDA to block the CPU thread on a
        /// synchronization primitive when waiting for the GPU to finish work.
        ///
        /// - ::CU_CTX_BLOCKING_SYNC: Instruct CUDA to block the CPU thread on a
        /// synchronization primitive when waiting for the GPU to finish work.
        /// <b>Deprecated:</b> This flag was deprecated as of CUDA 4.0 and was
        /// replaced with ::CU_CTX_SCHED_BLOCKING_SYNC.
        ///
        /// - ::CU_CTX_SCHED_AUTO: The default value if the <c>flags</c> parameter is zero,
        /// uses a heuristic based on the number of active CUDA contexts in the
        /// process \e C and the number of logical processors in the system \e P. If
        /// \e C > \e P, then CUDA will yield to other OS threads when waiting for
        /// the GPU (::CU_CTX_SCHED_YIELD), otherwise CUDA will not yield while
        /// waiting for results and actively spin on the processor (::CU_CTX_SCHED_SPIN).
        /// However, on low power devices like Tegra, it always defaults to
        /// ::CU_CTX_SCHED_BLOCKING_SYNC.
        ///
        /// - ::CU_CTX_LMEM_RESIZE_TO_MAX: Instruct CUDA to not reduce local memory
        /// after resizing local memory for a kernel. This can prevent thrashing by
        /// local memory allocations when launching many kernels with high local
        /// memory usage at the cost of potentially increased memory usage.</summary>
        ///
        /// <param name="dev">Device for which the primary context flags are set</param>
        /// <param name="flags">New flags for the device</param>
        ///
        /// \return
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_DEVICE,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_PRIMARY_CONTEXT_ACTIVE
        /// \notefnerr
        ///
        /// \sa ::cuDevicePrimaryCtxRetain,
        /// ::cuDevicePrimaryCtxGetState,
        /// ::cuCtxCreate,
        /// ::cuCtxGetFlags,
        /// ::cudaSetDeviceFlags
        /// CUresult CUDAAPI cuCtxCreate(CUcontext *pctx, unsigned int flags, CUdevice dev);
        [DllImport(_dllpath, EntryPoint = "cuCtxCreate")]
        public static extern CuResult CtxCreate(out CuContext pctx, CuContextFlags flags, CuDevice dev);

        public static CuContext CtxCreate(
            CuDevice dev,
            CuContextFlags flags = CuContextFlags.SchedAuto)
        {
            var result = CtxCreate(out var ctx, flags, dev);
            CheckResult(result);

            return ctx;
        }

        /// <summary>Destroy a CUDA context
        ///
        /// Destroys the CUDA context specified by <c>ctx</c>.  The context <c>ctx</c> will be
        /// destroyed regardless of how many threads it is current to.
        /// It is the responsibility of the calling function to ensure that no API
        /// call issues using <c>ctx</c> while ::cuCtxDestroy() is executing.
        ///
        /// If <c>ctx</c> is current to the calling thread then <c>ctx</c> will also be
        /// popped from the current thread's context stack (as though ::cuCtxPopCurrent()
        /// were called).  If <c>ctx</c> is current to other threads, then <c>ctx</c> will
        /// remain current to those threads, and attempting to access <c>ctx</c> from
        /// those threads will result in the error ::CUDA_ERROR_CONTEXT_IS_DESTROYED.</summary>
        ///
        /// <param name="ctx">Context to destroy</param>
        ///
        /// \return
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// \notefnerr
        ///
        /// \sa ::cuCtxCreate,
        /// ::cuCtxGetApiVersion,
        /// ::cuCtxGetCacheConfig,
        /// ::cuCtxGetDevice,
        /// ::cuCtxGetFlags,
        /// ::cuCtxGetLimit,
        /// ::cuCtxPopCurrent,
        /// ::cuCtxPushCurrent,
        /// ::cuCtxSetCacheConfig,
        /// ::cuCtxSetLimit,
        /// ::cuCtxSynchronize
        /// CUresult CUDAAPI cuCtxDestroy(CUcontext ctx);
        [DllImport(_dllpath, EntryPoint = "cuCtxDestroy")]
        public static extern CuResult CtxDestroy(CuContext ctx);

        /// <summary>Pushes a context on the current CPU thread
        ///
        /// Pushes the given context <c>ctx</c> onto the CPU thread's stack of current
        /// contexts. The specified context becomes the CPU thread's current context, so
        /// all CUDA functions that operate on the current context are affected.
        ///
        /// The previous current context may be made current again by calling
        /// ::cuCtxDestroy() or ::cuCtxPopCurrent().</summary>
        ///
        /// <param name="ctx">Context to push</param>
        /// \return
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// \notefnerr
        ///
        /// \sa ::cuCtxCreate,
        /// ::cuCtxDestroy,
        /// ::cuCtxGetApiVersion,
        /// ::cuCtxGetCacheConfig,
        /// ::cuCtxGetDevice,
        /// ::cuCtxGetFlags,
        /// ::cuCtxGetLimit,
        /// ::cuCtxPopCurrent,
        /// ::cuCtxSetCacheConfig,
        /// ::cuCtxSetLimit,
        /// ::cuCtxSynchronize
        /// CUresult CUDAAPI cuCtxPushCurrent(CUcontext ctx);
        [DllImport(_dllpath, EntryPoint = "cuCtxPushCurrent")]
        public static extern CuResult CtxPushCurrent(CuContext ctx);

        /// <summary>Pops the current CUDA context from the current CPU thread.
        ///
        /// Pops the current CUDA context from the CPU thread and passes back the
        /// old context handle in \p *pctx. That context may then be made current
        /// to a different CPU thread by calling ::cuCtxPushCurrent().
        ///
        /// If a context was current to the CPU thread before ::cuCtxCreate() or
        /// ::cuCtxPushCurrent() was called, this function makes that context current to
        /// the CPU thread again.</summary>
        ///
        /// <param name="pctx">Returned new context handle</param>
        /// \return
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT
        /// \notefnerr
        ///
        /// \sa ::cuCtxCreate,
        /// ::cuCtxDestroy,
        /// ::cuCtxGetApiVersion,
        /// ::cuCtxGetCacheConfig,
        /// ::cuCtxGetDevice,
        /// ::cuCtxGetFlags,
        /// ::cuCtxGetLimit,
        /// ::cuCtxPushCurrent,
        /// ::cuCtxSetCacheConfig,
        /// ::cuCtxSetLimit,
        /// ::cuCtxSynchronize
        /// CUresult CUDAAPI cuCtxPopCurrent(CUcontext *pctx);
        [DllImport(_dllpath, EntryPoint = "cuCtxPopCurrent")]
        public static extern CuResult CtxPopCurrent(out CuContext pctx);


        /// <summary>Binds the specified CUDA context to the calling CPU thread
        ///
        /// Binds the specified CUDA context to the calling CPU thread.
        /// If <c>ctx</c> is NULL then the CUDA context previously bound to the
        /// calling CPU thread is unbound and ::CUDA_SUCCESS is returned.
        ///
        /// If there exists a CUDA context stack on the calling CPU thread, this
        /// will replace the top of that stack with <c>ctx</c>.
        /// If <c>ctx</c> is NULL then this will be equivalent to popping the top
        /// of the calling CPU thread's CUDA context stack (or a no-op if the
        /// calling CPU thread's CUDA context stack is empty).</summary>
        ///
        /// <param name="ctx">Context to bind to the calling CPU thread</param>
        /// \return
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT
        /// \notefnerr
        ///
        /// \sa
        /// ::cuCtxGetCurrent,
        /// ::cuCtxCreate,
        /// ::cuCtxDestroy,
        /// ::cudaSetDevice
        /// CUresult CUDAAPI cuCtxSetCurrent(CUcontext ctx);
        [DllImport(_dllpath, EntryPoint = "cuCtxSetCurrent")]
        public static extern CuResult CtxSetCurrent(CuContext ctx);


        /// <summary>Returns the CUDA context bound to the calling CPU thread.
        ///
        /// Returns in \p *pctx the CUDA context bound to the calling CPU thread.
        /// If no context is bound to the calling CPU thread then \p *pctx is
        /// set to NULL and ::CUDA_SUCCESS is returned.</summary>
        ///
        /// <param name="pctx">Returned context handle</param>
        /// \return
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// \notefnerr
        ///
        /// \sa
        /// ::cuCtxSetCurrent,
        /// ::cuCtxCreate,
        /// ::cuCtxDestroy,
        /// ::cudaGetDevice
        /// CUresult CUDAAPI cuCtxGetCurrent(CUcontext *pctx);
        [DllImport(_dllpath, EntryPoint = "cuCtxGetCurrent")]
        public static extern CuResult CtxGetCurrent(out CuContext pctx);
        #endregion

        /// <summary>Gets free and total memory
        ///
        /// Returns in \p *free and \p *total respectively, the free and total amount of
        /// memory available for allocation by the CUDA context, in bytes.</summary>
        ///
        /// <param name="free">Returned free memory in bytes</param>
        /// <param name="total">Returned total memory in bytes</param>
        /// \return
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// \notefnerr
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D16,
        /// ::cuMemsetD2D32, ::cuMemsetD8, ::cuMemsetD16, ::cuMemsetD32,
        /// ::cudaMemGetInfo
        /// CUresult CUDAAPI cuMemGetInfo(size_t *free, size_t *total);
        [DllImport(_dllpath, EntryPoint = "cuMemGetInfo")]
        public static extern CuResult MemGetInfo(out IntPtr free, out IntPtr total);

        /// <summary>Allocates device memory
        ///
        /// Allocates <c>bytesize</c> bytes of linear memory on the device and returns in
        /// \p *dptr a pointer to the allocated memory. The allocated memory is suitably
        /// aligned for any kind of variable. The memory is not cleared. If <c>bytesize</c>
        /// is 0, ::cuMemAlloc() returns ::CUDA_ERROR_INVALID_VALUE.</summary>
        ///
        /// <param name="dptr">Returned device pointer</param>
        /// <param name="bytesize">Requested allocation size in bytes</param>
        /// \return
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_OUT_OF_MEMORY
        /// \notefnerr
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D16,
        /// ::cuMemsetD2D32, ::cuMemsetD8, ::cuMemsetD16, ::cuMemsetD32,
        /// ::cudaMalloc
        /// CUresult CUDAAPI cuMemAlloc(CUdeviceptr *dptr, size_t bytesize);
        [DllImport(_dllpath, EntryPoint = "cuMemAlloc")]
        public static extern CuResult MemAlloc(out CuDevicePtr dptr, IntPtr bytesize);

        /// <summary>Allocates pitched device memory
        ///
        /// Allocates at least <c>WidthInBytes</c> * <c>Height</c> bytes of linear memory on
        /// the device and returns in \p *dptr a pointer to the allocated memory. The
        /// function may pad the allocation to ensure that corresponding pointers in
        /// any given row will continue to meet the alignment requirements for
        /// coalescing as the address is updated from row to row. <c>ElementSizeBytes</c>
        /// specifies the size of the largest reads and writes that will be performed
        /// on the memory range. <c>ElementSizeBytes</c> may be 4, 8 or 16 (since coalesced
        /// memory transactions are not possible on other data sizes). If
        /// <c>ElementSizeBytes</c> is smaller than the actual read/write size of a kernel,
        /// the kernel will run correctly, but possibly at reduced speed. The pitch
        /// returned in \p *pPitch by ::cuMemAllocPitch() is the width in bytes of the
        /// allocation. The intended usage of pitch is as a separate parameter of the
        /// allocation, used to compute addresses within the 2D array. Given the row
        /// and column of an array element of type \b T, the address is computed as:
        /// <code>
        /// T* pElement = (T*)((char*)BaseAddress + Row * Pitch) + Column;
        /// </code>
        ///
        /// The pitch returned by ::cuMemAllocPitch() is guaranteed to work with
        /// ::cuMemcpy2D() under all circumstances. For allocations of 2D arrays, it is
        /// recommended that programmers consider performing pitch allocations using
        /// ::cuMemAllocPitch(). Due to alignment restrictions in the hardware, this is
        /// especially true if the application will be performing 2D memory copies
        /// between different regions of device memory (whether linear memory or CUDA
        /// arrays).
        ///
        /// The byte alignment of the pitch returned by ::cuMemAllocPitch() is guaranteed
        /// to match or exceed the alignment requirement for texture binding with
        /// ::cuTexRefSetAddress2D().</summary>
        ///
        /// <param name="dptr">Returned device pointer</param>
        /// <param name="pitch">Returned pitch of allocation in bytes</param>
        /// <param name="widthInBytes">Requested allocation width in bytes</param>
        /// <param name="height">Requested allocation height in rows</param>
        /// <param name="elementSizeBytes">Size of largest reads/writes for range</param>
        /// \return
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_OUT_OF_MEMORY
        /// \notefnerr
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D16,
        /// ::cuMemsetD2D32, ::cuMemsetD8, ::cuMemsetD16, ::cuMemsetD32,
        /// ::cudaMallocPitch

        /// CUresult CUDAAPI cuMemAllocPitch(CUdeviceptr *dptr, size_t *pPitch, size_t WidthInBytes, size_t Height, unsigned int ElementSizeBytes);
        [DllImport(_dllpath, EntryPoint = "cuMemAllocPitch")]
        public static extern CuResult MemAllocPitch(out CuDevicePtr dptr, out IntPtr pitch, IntPtr widthInBytes, IntPtr height, uint elementSizeBytes);

        /// <summary>Frees device memory
        ///
        /// Frees the memory space pointed to by <c>dptr</c>, which must have been returned
        /// by a previous call to ::cuMemAlloc() or ::cuMemAllocPitch().</summary>
        ///
        /// <param name="dptr">Pointer to memory to free</param>
        /// \return
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// \notefnerr
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D16,
        /// ::cuMemsetD2D32, ::cuMemsetD8, ::cuMemsetD16, ::cuMemsetD32,
        /// ::cudaFree

        /// CUresult CUDAAPI cuMemFree(CUdeviceptr dptr);
        [DllImport(_dllpath, EntryPoint = "cuMemFree")]
        public static extern CuResult MemFree(CuDevicePtr dptr);

        #region Memcpy
        /// <summary>Copies memory
        ///
        /// Copies data between two pointers.
        /// <c>dst</c> and <c>src</c> are base pointers of the destination and source, respectively.
        /// <c>ByteCount</c> specifies the number of bytes to copy.
        /// Note that this function infers the type of the transfer (host to host, host to
        ///   device, device to device, or device to host) from the pointer values.  This
        ///   function is only allowed in contexts which support unified addressing.</summary>
        ///
        /// <param name="dst">Destination unified virtual address space pointer</param>
        /// <param name="src">Source unified virtual address space pointer</param>
        /// <param name="byteCount">Size of memory copy in bytes</param>
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </return>
        /// \notefnerr
        /// \note_sync
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D16,
        /// ::cuMemsetD2D32, ::cuMemsetD8, ::cuMemsetD16, ::cuMemsetD32,
        /// ::cudaMemcpy,
        /// ::cudaMemcpyToSymbol,
        /// ::cudaMemcpyFromSymbol
        /// CUresult CUDAAPI cuMemcpy(CUdeviceptr dst, CUdeviceptr src, size_t ByteCount);
        [DllImport(_dllpath, EntryPoint = "cuMemcpy")]
        public static extern CuResult Memcpy(CuDevicePtr dst, CuDevicePtr src, IntPtr byteCount);

        /// <summary>Copies device memory between two contexts
        ///
        /// Copies from device memory in one context to device memory in another
        /// context. <c>dstDevice</c> is the base device pointer of the destination memory
        /// and <c>dstContext</c> is the destination context.  <c>srcDevice</c> is the base
        /// device pointer of the source memory and <c>srcContext</c> is the source pointer.
        /// <c>ByteCount</c> specifies the number of bytes to copy.</summary>
        ///
        /// <param name="dstDevice">Destination device pointer</param>
        /// <param name="dstContext">Destination context</param>
        /// <param name="srcDevice">Source device pointer</param>
        /// <param name="srcContext">Source context</param>
        /// <param name="byteCount">Size of memory copy in bytes</param>
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </return>
        /// \notefnerr
        /// \note_sync
        ///
        /// \sa ::cuMemcpyDtoD, ::cuMemcpy3DPeer, ::cuMemcpyDtoDAsync, ::cuMemcpyPeerAsync,
        /// ::cuMemcpy3DPeerAsync,
        /// ::cudaMemcpyPeer
        /// CUresult CUDAAPI cuMemcpyPeer(CUdeviceptr dstDevice, CUcontext dstContext, CUdeviceptr srcDevice, CUcontext srcContext, size_t ByteCount);
        [DllImport(_dllpath, EntryPoint = "cuMemcpyPeer")]
        public static extern CuResult MemcpyPeer(CuDevicePtr dstDevice, CuContext dstContext, CuDevicePtr srcDevice, CuContext srcContext, IntPtr byteCount);


        /// <summary>Copies memory from Host to Device
        ///
        /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are
        /// the base addresses of the destination and source, respectively. <c>ByteCount</c>
        /// specifies the number of bytes to copy.</summary>
        ///
        /// <param name="dstDevice">Destination device pointer</param>
        /// <param name="srcHost">Source host pointer</param>
        /// <param name="byteCount">Size of memory copy in bytes</param>
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </return>
        /// \notefnerr
        /// \note_sync
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D16,
        /// ::cuMemsetD2D32, ::cuMemsetD8, ::cuMemsetD16, ::cuMemsetD32,
        /// ::cudaMemcpy,
        /// ::cudaMemcpyToSymbol
        /// CUresult CUDAAPI cuMemcpyHtoD(CUdeviceptr dstDevice, const void *srcHost, size_t ByteCount);
        [DllImport(_dllpath, EntryPoint = "cuMemcpyHtoD")]
        public static extern CuResult MemcpyHtoD(CuDevicePtr dstDevice, IntPtr srcHost, IntPtr byteCount);

        /// <summary>Copies memory from Device to Host
        ///
        /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the
        /// base pointers of the destination and source, respectively. <c>ByteCount</c>
        /// specifies the number of bytes to copy.</summary>
        ///
        /// <param name="dstHost">Destination host pointer</param>
        /// <param name="srcDevice">Source device pointer</param>
        /// <param name="byteCount">Size of memory copy in bytes</param>
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </return>
        /// \notefnerr
        /// \note_sync
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D16,
        /// ::cuMemsetD2D32, ::cuMemsetD8, ::cuMemsetD16, ::cuMemsetD32,
        /// ::cudaMemcpy,
        /// ::cudaMemcpyFromSymbol
        /// CUresult CUDAAPI cuMemcpyDtoH(void *dstHost, CUdeviceptr srcDevice, size_t ByteCount);
        [DllImport(_dllpath, EntryPoint = "cuMemcpyDtoH")]
        public static extern CuResult MemcpyDtoH(IntPtr dstHost, CuDevicePtr srcDevice, IntPtr byteCount);

        /// <summary>Copies memory from Device to Device
        ///
        /// Copies from device memory to device memory. <c>dstDevice</c> and <c>srcDevice</c>
        /// are the base pointers of the destination and source, respectively.
        /// <c>ByteCount</c> specifies the number of bytes to copy.</summary>
        ///
        /// <param name="dstDevice">Destination device pointer</param>
        /// <param name="srcDevice">Source device pointer</param>
        /// <param name="byteCount">Size of memory copy in bytes</param>
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </return>
        /// \notefnerr
        /// \note_sync
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D16,
        /// ::cuMemsetD2D32, ::cuMemsetD8, ::cuMemsetD16, ::cuMemsetD32,
        /// ::cudaMemcpy,
        /// ::cudaMemcpyToSymbol,
        /// ::cudaMemcpyFromSymbol
        /// CUresult CUDAAPI cuMemcpyDtoD(CUdeviceptr dstDevice, CUdeviceptr srcDevice, size_t ByteCount);
        [DllImport(_dllpath, EntryPoint = "cuMemcpyDtoD")]
        public static extern CuResult MemcpyDtoD(CuDevicePtr dstDevice, CuDevicePtr srcDevice, IntPtr byteCount);

        /// <summary>Copies memory from Device to Array
        ///
        /// Copies from device memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c>
        /// specify the CUDA array handle and starting index of the destination data.
        /// <c>srcDevice</c> specifies the base pointer of the source. <c>ByteCount</c>
        /// specifies the number of bytes to copy.</summary>
        ///
        /// <param name="dstArray">Destination array</param>
        /// <param name="dstOffset">Offset in bytes of destination array</param>
        /// <param name="srcDevice">Source device pointer</param>
        /// <param name="byteCount">Size of memory copy in bytes</param>
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </return>
        /// \notefnerr
        /// \note_sync
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D16,
        /// ::cuMemsetD2D32, ::cuMemsetD8, ::cuMemsetD16, ::cuMemsetD32,
        /// ::cudaMemcpyToArray
        /// CUresult CUDAAPI cuMemcpyDtoA(CUarray dstArray, size_t dstOffset, CUdeviceptr srcDevice, size_t ByteCount);
        [DllImport(_dllpath, EntryPoint = "cuMemcpyDtoA")]
        public static extern CuResult MemcpyDtoA(CuArray dstArray, IntPtr dstOffset, CuDevicePtr srcDevice, IntPtr byteCount);

        /// <summary>Copies memory from Array to Device
        ///
        /// Copies from one 1D CUDA array to device memory. <c>dstDevice</c> specifies the
        /// base pointer of the destination and must be naturally aligned with the CUDA
        /// array elements. <c>srcArray</c> and <c>srcOffset</c> specify the CUDA array handle
        /// and the offset in bytes into the array where the copy is to begin.
        /// <c>ByteCount</c> specifies the number of bytes to copy and must be evenly
        /// divisible by the array element size.</summary>
        ///
        /// <param name="dstDevice">Destination device pointer</param>
        /// <param name="srcArray">Source array</param>
        /// <param name="srcOffset">Offset in bytes of source array</param>
        /// <param name="byteCount">Size of memory copy in bytes</param>
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </return>
        /// \notefnerr
        /// \note_sync
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D16,
        /// ::cuMemsetD2D32, ::cuMemsetD8, ::cuMemsetD16, ::cuMemsetD32,
        /// ::cudaMemcpyFromArray
        /// CUresult CUDAAPI cuMemcpyAtoD(CUdeviceptr dstDevice, CUarray srcArray, size_t srcOffset, size_t ByteCount);
        [DllImport(_dllpath, EntryPoint = "cuMemcpyAtoD")]
        public static extern CuResult MemcpyAtoD(CuDevicePtr dstDevice, CuArray srcArray, IntPtr srcOffset, IntPtr byteCount);

        /// <summary>Copies memory from Host to Array
        ///
        /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c>
        /// specify the CUDA array handle and starting offset in bytes of the destination
        /// data.  <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
        /// the number of bytes to copy.</summary>
        ///
        /// <param name="dstArray">Destination array</param>
        /// <param name="dstOffset">Offset in bytes of destination array</param>
        /// <param name="srcHost">Source host pointer</param>
        /// <param name="byteCount">Size of memory copy in bytes</param>
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </return>
        /// \notefnerr
        /// \note_sync
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D16,
        /// ::cuMemsetD2D32, ::cuMemsetD8, ::cuMemsetD16, ::cuMemsetD32,
        /// ::cudaMemcpyToArray
        /// CUresult CUDAAPI cuMemcpyHtoA(CUarray dstArray, size_t dstOffset, const void *srcHost, size_t ByteCount);
        [DllImport(_dllpath, EntryPoint = "cuMemcpyHtoA")]
        public static extern CuResult MemcpyHtoA(CuArray dstArray, IntPtr dstOffset, IntPtr srcHost, IntPtr byteCount);

        /// <summary>Copies memory from Array to Host
        ///
        /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base
        /// pointer of the destination. <c>srcArray</c> and <c>srcOffset</c> specify the CUDA
        /// array handle and starting offset in bytes of the source data.
        /// <c>ByteCount</c> specifies the number of bytes to copy.</summary>
        ///
        /// <param name="dstHost">Destination device pointer</param>
        /// <param name="srcArray">Source array</param>
        /// <param name="srcOffset">Offset in bytes of source array</param>
        /// <param name="byteCount">Size of memory copy in bytes</param>
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </return>
        /// \notefnerr
        /// \note_sync
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D16,
        /// ::cuMemsetD2D32, ::cuMemsetD8, ::cuMemsetD16, ::cuMemsetD32,
        /// ::cudaMemcpyFromArray
        /// CUresult CUDAAPI cuMemcpyAtoH(void *dstHost, CUarray srcArray, size_t srcOffset, size_t ByteCount);
        [DllImport(_dllpath, EntryPoint = "cuMemcpyAtoH")]
        public static extern CuResult MemcpyAtoH(IntPtr dstHost, CuArray srcArray, IntPtr srcOffset, IntPtr byteCount);

        /// <summary>Copies memory from Array to Array
        ///
        /// Copies from one 1D CUDA array to another. <c>dstArray</c> and <c>srcArray</c>
        /// specify the handles of the destination and source CUDA arrays for the copy,
        /// respectively. <c>dstOffset</c> and <c>srcOffset</c> specify the destination and
        /// source offsets in bytes into the CUDA arrays. <c>ByteCount</c> is the number of
        /// bytes to be copied. The size of the elements in the CUDA arrays need not be
        /// the same format, but the elements must be the same size; and count must be
        /// evenly divisible by that size.</summary>
        ///
        /// <param name="dstArray">Destination array</param>
        /// <param name="dstOffset">Offset in bytes of destination array</param>
        /// <param name="srcArray">Source array</param>
        /// <param name="srcOffset">Offset in bytes of source array</param>
        /// <param name="byteCount">Size of memory copy in bytes</param>
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </return>
        /// \notefnerr
        /// \note_sync
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D16,
        /// ::cuMemsetD2D32, ::cuMemsetD8, ::cuMemsetD16, ::cuMemsetD32,
        /// ::cudaMemcpyArrayToArray
        /// CUresult CUDAAPI cuMemcpyAtoA(CUarray dstArray, size_t dstOffset, CUarray srcArray, size_t srcOffset, size_t ByteCount);
        [DllImport(_dllpath, EntryPoint = "cuMemcpyAtoA")]
        public static extern CuResult MemcpyAtoA(CuArray dstArray, IntPtr dstOffset, CuArray srcArray, IntPtr srcOffset, IntPtr byteCount);

        /// <summary>Copies memory for 2D arrays
        ///
        /// Perform a 2D memory copy according to the parameters specified in <paramref name="pCopy"/>.
        /// The ::CUDA_MEMCPY2D structure is defined as:
        ///
        /// <code>
        ///   typedef struct CUDA_MEMCPY2D_st {
        ///      unsigned int srcXInBytes, srcY;
        ///      CUmemorytype srcMemoryType;
        ///          const void *srcHost;
        ///          CUdeviceptr srcDevice;
        ///          CUarray srcArray;
        ///          unsigned int srcPitch;
        ///
        ///      unsigned int dstXInBytes, dstY;
        ///      CUmemorytype dstMemoryType;
        ///          void *dstHost;
        ///          CUdeviceptr dstDevice;
        ///          CUarray dstArray;
        ///          unsigned int dstPitch;
        ///
        ///      unsigned int WidthInBytes;
        ///      unsigned int Height;
        ///   } CUDA_MEMCPY2D;
        /// </code>
        /// where:
        /// - ::srcMemoryType and ::dstMemoryType specify the type of memory of the
        ///   source and destination, respectively; ::CUmemorytype_enum is defined as:
        ///
        /// <code>
        ///   typedef enum CUmemorytype_enum {
        ///      CU_MEMORYTYPE_HOST = 0x01,
        ///      CU_MEMORYTYPE_DEVICE = 0x02,
        ///      CU_MEMORYTYPE_ARRAY = 0x03,
        ///      CU_MEMORYTYPE_UNIFIED = 0x04
        ///   } CUmemorytype;
        /// </code>
        ///
        /// <para>
        /// If ::srcMemoryType is ::CU_MEMORYTYPE_UNIFIED, ::srcDevice and ::srcPitch
        ///   specify the (unified virtual address space) base address of the source data
        ///   and the bytes per row to apply.  ::srcArray is ignored.
        /// This value may be used only if unified addressing is supported in the calling
        ///   context.
        /// </para>
        /// <para>
        /// If ::srcMemoryType is ::CU_MEMORYTYPE_HOST, ::srcHost and ::srcPitch
        /// specify the (host) base address of the source data and the bytes per row to
        /// apply. ::srcArray is ignored.
        /// </para>
        /// <para>
        /// If ::srcMemoryType is ::CU_MEMORYTYPE_DEVICE, ::srcDevice and ::srcPitch
        /// specify the (device) base address of the source data and the bytes per row
        /// to apply. ::srcArray is ignored.
        /// </para>
        /// <para>
        /// If ::srcMemoryType is ::CU_MEMORYTYPE_ARRAY, ::srcArray specifies the
        /// handle of the source data. ::srcHost, ::srcDevice and ::srcPitch are
        /// ignored.
        /// </para>
        /// <para>
        /// If ::dstMemoryType is ::CU_MEMORYTYPE_HOST, ::dstHost and ::dstPitch
        /// specify the (host) base address of the destination data and the bytes per
        /// row to apply. ::dstArray is ignored.
        /// </para>
        /// <para>
        /// If ::dstMemoryType is ::CU_MEMORYTYPE_UNIFIED, ::dstDevice and ::dstPitch
        ///   specify the (unified virtual address space) base address of the source data
        ///   and the bytes per row to apply.  ::dstArray is ignored.
        /// This value may be used only if unified addressing is supported in the calling
        ///   context.
        /// </para>
        /// <para>
        /// If ::dstMemoryType is ::CU_MEMORYTYPE_DEVICE, ::dstDevice and ::dstPitch
        /// specify the (device) base address of the destination data and the bytes per
        /// row to apply. ::dstArray is ignored.
        /// </para>
        /// <para>
        /// If ::dstMemoryType is ::CU_MEMORYTYPE_ARRAY, ::dstArray specifies the
        /// handle of the destination data. ::dstHost, ::dstDevice and ::dstPitch are
        /// ignored.
        ///
        /// - ::srcXInBytes and ::srcY specify the base address of the source data for
        ///   the copy.
        /// </para>
        /// <para>
        /// For host pointers, the starting address is
        /// </para>
        /// <code>
        ///  void* Start = (void*)((char*)srcHost+srcY*srcPitch + srcXInBytes);
        /// </code>
        ///
        /// <para>
        /// For device pointers, the starting address is
        /// </para>
        /// <code>
        ///  CUdeviceptr Start = srcDevice+srcY*srcPitch+srcXInBytes;
        /// </code>
        ///
        /// <para>
        /// For CUDA arrays, ::srcXInBytes must be evenly divisible by the array
        /// element size.
        ///
        /// - ::dstXInBytes and ::dstY specify the base address of the destination data
        ///   for the copy.
        /// </para>
        /// <para>
        /// For host pointers, the base address is
        /// </para>
        /// <code>
        /// void* dstStart = (void*)((char*)dstHost+dstY*dstPitch + dstXInBytes);
        /// </code>
        ///
        /// <para>
        /// For device pointers, the starting address is
        /// </para>
        /// <code>
        ///  CUdeviceptr dstStart = dstDevice+dstY*dstPitch+dstXInBytes;
        /// </code>
        ///
        /// <para>
        /// For CUDA arrays, ::dstXInBytes must be evenly divisible by the array
        /// element size.
        ///
        /// - ::WidthInBytes and ::Height specify the width (in bytes) and height of
        ///   the 2D copy being performed.
        /// - If specified, ::srcPitch must be greater than or equal to ::WidthInBytes +
        ///   ::srcXInBytes, and ::dstPitch must be greater than or equal to
        ///   ::WidthInBytes + dstXInBytes.
        /// </para>
        /// <para>
        /// ::cuMemcpy2D() returns an error if any pitch is greater than the maximum
        /// allowed (::CU_DEVICE_ATTRIBUTE_MAX_PITCH). ::cuMemAllocPitch() passes back
        /// pitches that always work with ::cuMemcpy2D(). On intra-device memory copies
        /// (device to device, CUDA array to device, CUDA array to CUDA array),
        /// ::cuMemcpy2D() may fail for pitches not computed by ::cuMemAllocPitch().
        /// ::cuMemcpy2DUnaligned() does not have this restriction, but may run
        /// significantly slower in the cases where ::cuMemcpy2D() would have returned
        /// an error code.</para></summary>
        ///
        /// <param name="pCopy">Parameters for the memory copy</param>
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </return>
        ///
        /// \notefnerr
        /// \note_sync
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D16,
        /// ::cuMemsetD2D32, ::cuMemsetD8, ::cuMemsetD16, ::cuMemsetD32,
        /// ::cudaMemcpy2D,
        /// ::cudaMemcpy2DToArray,
        /// ::cudaMemcpy2DFromArray
        /// CUresult CUDAAPI cuMemcpy2D(const CUDA_MEMCPY2D *pCopy);
        [DllImport(_dllpath, EntryPoint = "cuMemcpy2D")]
        public static extern CuResult Memcpy2D(ref CudaMemcopy2D pCopy);

        /// <summary>Copies memory for 2D arrays
        ///
        /// Perform a 2D memory copy according to the parameters specified in <paramref name="pCopy"/>.
        /// The ::CUDA_MEMCPY2D structure is defined as:
        ///
        /// <code>
        ///   typedef struct CUDA_MEMCPY2D_st {
        ///      unsigned int srcXInBytes, srcY;
        ///      CUmemorytype srcMemoryType;
        ///      const void *srcHost;
        ///      CUdeviceptr srcDevice;
        ///      CUarray srcArray;
        ///      unsigned int srcPitch;
        ///      unsigned int dstXInBytes, dstY;
        ///      CUmemorytype dstMemoryType;
        ///      void *dstHost;
        ///      CUdeviceptr dstDevice;
        ///      CUarray dstArray;
        ///      unsigned int dstPitch;
        ///      unsigned int WidthInBytes;
        ///      unsigned int Height;
        ///   } CUDA_MEMCPY2D;
        /// </code>
        /// where:
        /// - ::srcMemoryType and ::dstMemoryType specify the type of memory of the
        ///   source and destination, respectively; ::CUmemorytype_enum is defined as:
        ///
        /// <code>
        ///   typedef enum CUmemorytype_enum {
        ///      CU_MEMORYTYPE_HOST = 0x01,
        ///      CU_MEMORYTYPE_DEVICE = 0x02,
        ///      CU_MEMORYTYPE_ARRAY = 0x03,
        ///      CU_MEMORYTYPE_UNIFIED = 0x04
        ///   } CUmemorytype;
        /// </code>
        ///
        /// <para>
        /// If ::srcMemoryType is ::CU_MEMORYTYPE_UNIFIED, ::srcDevice and ::srcPitch
        ///   specify the (unified virtual address space) base address of the source data
        ///   and the bytes per row to apply.  ::srcArray is ignored.
        /// This value may be used only if unified addressing is supported in the calling
        ///   context.
        /// </para>
        /// <para>
        /// If ::srcMemoryType is ::CU_MEMORYTYPE_HOST, ::srcHost and ::srcPitch
        /// specify the (host) base address of the source data and the bytes per row to
        /// apply. ::srcArray is ignored.
        /// </para>
        /// <para>
        /// If ::srcMemoryType is ::CU_MEMORYTYPE_DEVICE, ::srcDevice and ::srcPitch
        /// specify the (device) base address of the source data and the bytes per row
        /// to apply. ::srcArray is ignored.
        /// </para>
        /// <para>
        /// If ::srcMemoryType is ::CU_MEMORYTYPE_ARRAY, ::srcArray specifies the
        /// handle of the source data. ::srcHost, ::srcDevice and ::srcPitch are
        /// ignored.
        /// </para>
        /// <para>
        /// If ::dstMemoryType is ::CU_MEMORYTYPE_UNIFIED, ::dstDevice and ::dstPitch
        ///   specify the (unified virtual address space) base address of the source data
        ///   and the bytes per row to apply.  ::dstArray is ignored.
        /// This value may be used only if unified addressing is supported in the calling
        ///   context.
        /// </para>
        /// <para>
        /// If ::dstMemoryType is ::CU_MEMORYTYPE_HOST, ::dstHost and ::dstPitch
        /// specify the (host) base address of the destination data and the bytes per
        /// row to apply. ::dstArray is ignored.
        /// </para>
        /// <para>
        /// If ::dstMemoryType is ::CU_MEMORYTYPE_DEVICE, ::dstDevice and ::dstPitch
        /// specify the (device) base address of the destination data and the bytes per
        /// row to apply. ::dstArray is ignored.
        /// </para>
        /// <para>
        /// If ::dstMemoryType is ::CU_MEMORYTYPE_ARRAY, ::dstArray specifies the
        /// handle of the destination data. ::dstHost, ::dstDevice and ::dstPitch are
        /// ignored.
        ///
        /// - ::srcXInBytes and ::srcY specify the base address of the source data for
        ///   the copy.
        /// </para>
        /// <para>
        /// For host pointers, the starting address is
        /// </para>
        /// <code>
        ///  void* Start = (void*)((char*)srcHost+srcY*srcPitch + srcXInBytes);
        /// </code>
        ///
        /// <para>
        /// For device pointers, the starting address is
        /// </para>
        /// <code>
        ///  CUdeviceptr Start = srcDevice+srcY*srcPitch+srcXInBytes;
        /// </code>
        ///
        /// <para>
        /// For CUDA arrays, ::srcXInBytes must be evenly divisible by the array
        /// element size.
        ///
        /// - ::dstXInBytes and ::dstY specify the base address of the destination data
        ///   for the copy.
        /// </para>
        /// <para>
        /// For host pointers, the base address is
        /// </para>
        /// <code>
        ///  void* dstStart = (void*)((char*)dstHost+dstY*dstPitch + dstXInBytes);
        /// </code>
        ///
        /// <para>
        /// For device pointers, the starting address is
        /// </para>
        /// <code>
        ///  CUdeviceptr dstStart = dstDevice+dstY*dstPitch+dstXInBytes;
        /// </code>
        ///
        /// <para>
        /// For CUDA arrays, ::dstXInBytes must be evenly divisible by the array
        /// element size.
        ///
        /// - ::WidthInBytes and ::Height specify the width (in bytes) and height of
        ///   the 2D copy being performed.
        /// - If specified, ::srcPitch must be greater than or equal to ::WidthInBytes +
        ///   ::srcXInBytes, and ::dstPitch must be greater than or equal to
        ///   ::WidthInBytes + dstXInBytes.
        /// </para>
        /// <para>
        /// ::cuMemcpy2D() returns an error if any pitch is greater than the maximum
        /// allowed (::CU_DEVICE_ATTRIBUTE_MAX_PITCH). ::cuMemAllocPitch() passes back
        /// pitches that always work with ::cuMemcpy2D(). On intra-device memory copies
        /// (device to device, CUDA array to device, CUDA array to CUDA array),
        /// ::cuMemcpy2D() may fail for pitches not computed by ::cuMemAllocPitch().
        /// ::cuMemcpy2DUnaligned() does not have this restriction, but may run
        /// significantly slower in the cases where ::cuMemcpy2D() would have returned
        /// an error code.</para></summary>
        ///
        /// <param name="pCopy">Parameters for the memory copy</param>
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </return>
        ///
        /// \notefnerr
        /// \note_sync
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D16,
        /// ::cuMemsetD2D32, ::cuMemsetD8, ::cuMemsetD16, ::cuMemsetD32,
        /// ::cudaMemcpy2D,
        /// ::cudaMemcpy2DToArray,
        /// ::cudaMemcpy2DFromArray
        /// CUresult CUDAAPI cuMemcpy2DUnaligned(const CUDA_MEMCPY2D *pCopy);
        [DllImport(_dllpath, EntryPoint = "cuMemcpy2DUnaligned")]
        public static extern CuResult Memcpy2DUnaligned(ref CudaMemcopy2D pCopy);

        /// <summary>Copies memory for 3D arrays
        ///
        /// Perform a 3D memory copy according to the parameters specified in
        /// <paramref name="pCopy"/>. The ::CUDA_MEMCPY3D structure is defined as:
        ///
        /// <code>
        ///        typedef struct CUDA_MEMCPY3D_st {
        ///
        ///            unsigned int srcXInBytes, srcY, srcZ;
        ///            unsigned int srcLOD;
        ///            CUmemorytype srcMemoryType;
        ///                const void *srcHost;
        ///                CUdeviceptr srcDevice;
        ///                CUarray srcArray;
        ///                unsigned int srcPitch;  // ignored when src is array
        ///                unsigned int srcHeight; // ignored when src is array; may be 0 if Depth==1
        ///
        ///            unsigned int dstXInBytes, dstY, dstZ;
        ///            unsigned int dstLOD;
        ///            CUmemorytype dstMemoryType;
        ///                void *dstHost;
        ///                CUdeviceptr dstDevice;
        ///                CUarray dstArray;
        ///                unsigned int dstPitch;  // ignored when dst is array
        ///                unsigned int dstHeight; // ignored when dst is array; may be 0 if Depth==1
        ///
        ///            unsigned int WidthInBytes;
        ///            unsigned int Height;
        ///            unsigned int Depth;
        ///        } CUDA_MEMCPY3D;
        /// </code>
        /// where:
        /// - ::srcMemoryType and ::dstMemoryType specify the type of memory of the
        ///   source and destination, respectively; ::CUmemorytype_enum is defined as:
        ///
        /// <code>
        ///   typedef enum CUmemorytype_enum {
        ///      CU_MEMORYTYPE_HOST = 0x01,
        ///      CU_MEMORYTYPE_DEVICE = 0x02,
        ///      CU_MEMORYTYPE_ARRAY = 0x03,
        ///      CU_MEMORYTYPE_UNIFIED = 0x04
        ///   } CUmemorytype;
        /// </code>
        ///
        /// <para>
        /// If ::srcMemoryType is ::CU_MEMORYTYPE_UNIFIED, ::srcDevice and ::srcPitch
        ///   specify the (unified virtual address space) base address of the source data
        ///   and the bytes per row to apply.  ::srcArray is ignored.
        /// This value may be used only if unified addressing is supported in the calling
        ///   context.
        /// </para>
        /// <para>
        /// If ::srcMemoryType is ::CU_MEMORYTYPE_HOST, ::srcHost, ::srcPitch and
        /// ::srcHeight specify the (host) base address of the source data, the bytes
        /// per row, and the height of each 2D slice of the 3D array. ::srcArray is
        /// ignored.
        /// </para>
        /// <para>
        /// If ::srcMemoryType is ::CU_MEMORYTYPE_DEVICE, ::srcDevice, ::srcPitch and
        /// ::srcHeight specify the (device) base address of the source data, the bytes
        /// per row, and the height of each 2D slice of the 3D array. ::srcArray is
        /// ignored.
        /// </para>
        /// <para>
        /// If ::srcMemoryType is ::CU_MEMORYTYPE_ARRAY, ::srcArray specifies the
        /// handle of the source data. ::srcHost, ::srcDevice, ::srcPitch and
        /// ::srcHeight are ignored.
        /// </para>
        /// <para>
        /// If ::dstMemoryType is ::CU_MEMORYTYPE_UNIFIED, ::dstDevice and ::dstPitch
        ///   specify the (unified virtual address space) base address of the source data
        ///   and the bytes per row to apply.  ::dstArray is ignored.
        /// This value may be used only if unified addressing is supported in the calling
        ///   context.
        /// </para>
        /// <para>
        /// If ::dstMemoryType is ::CU_MEMORYTYPE_HOST, ::dstHost and ::dstPitch
        /// specify the (host) base address of the destination data, the bytes per row,
        /// and the height of each 2D slice of the 3D array. ::dstArray is ignored.
        /// </para>
        /// <para>
        /// If ::dstMemoryType is ::CU_MEMORYTYPE_DEVICE, ::dstDevice and ::dstPitch
        /// specify the (device) base address of the destination data, the bytes per
        /// row, and the height of each 2D slice of the 3D array. ::dstArray is ignored.
        /// </para>
        /// <para>
        /// If ::dstMemoryType is ::CU_MEMORYTYPE_ARRAY, ::dstArray specifies the
        /// handle of the destination data. ::dstHost, ::dstDevice, ::dstPitch and
        /// ::dstHeight are ignored.
        ///
        /// - ::srcXInBytes, ::srcY and ::srcZ specify the base address of the source
        ///   data for the copy.
        /// </para>
        /// <para>
        /// For host pointers, the starting address is
        /// </para>
        /// <code>
        ///  void* Start = (void*)((char*)srcHost+(srcZ*srcHeight+srcY)*srcPitch + srcXInBytes);
        /// </code>
        ///
        /// <para>
        /// For device pointers, the starting address is
        /// </para>
        /// <code>
        ///  CUdeviceptr Start = srcDevice+(srcZ*srcHeight+srcY)*srcPitch+srcXInBytes;
        /// </code>
        ///
        /// <para>
        /// For CUDA arrays, ::srcXInBytes must be evenly divisible by the array
        /// element size.
        ///
        /// - dstXInBytes, ::dstY and ::dstZ specify the base address of the
        ///   destination data for the copy.
        /// </para>
        /// <para>
        /// For host pointers, the base address is
        /// </para>
        /// <code>
        ///  void* dstStart = (void*)((char*)dstHost+(dstZ*dstHeight+dstY)*dstPitch + dstXInBytes);
        /// </code>
        ///
        /// <para>
        /// For device pointers, the starting address is
        /// </para>
        /// <code>
        ///  CUdeviceptr dstStart = dstDevice+(dstZ*dstHeight+dstY)*dstPitch+dstXInBytes;
        /// </code>
        ///
        /// <para>
        /// For CUDA arrays, ::dstXInBytes must be evenly divisible by the array
        /// element size.
        ///
        /// - ::WidthInBytes, ::Height and ::Depth specify the width (in bytes), height
        ///   and depth of the 3D copy being performed.
        /// - If specified, ::srcPitch must be greater than or equal to ::WidthInBytes +
        ///   ::srcXInBytes, and ::dstPitch must be greater than or equal to
        ///   ::WidthInBytes + dstXInBytes.
        /// - If specified, ::srcHeight must be greater than or equal to ::Height +
        ///   ::srcY, and ::dstHeight must be greater than or equal to ::Height + ::dstY.
        /// </para>
        /// <para>
        /// ::cuMemcpy3D() returns an error if any pitch is greater than the maximum
        /// allowed (::CU_DEVICE_ATTRIBUTE_MAX_PITCH).
        ///
        /// The ::srcLOD and ::dstLOD members of the ::CUDA_MEMCPY3D structure must be
        /// set to 0.</para></summary>
        ///
        /// <param name="pCopy">Parameters for the memory copy</param>
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </return>
        ///
        /// \notefnerr
        /// \note_sync
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D16,
        /// ::cuMemsetD2D32, ::cuMemsetD8, ::cuMemsetD16, ::cuMemsetD32,
        /// ::cudaMemcpy3D
        /// CUresult CUDAAPI cuMemcpy3D(const CUDA_MEMCPY3D *pCopy);
        [DllImport(_dllpath, EntryPoint = "cuMemcpy3D")]
        public static extern CuResult Memcpy3D(ref CudaMemcpy3D pCopy);

        /// <summary>Copies memory between contexts
        ///
        /// Perform a 3D memory copy according to the parameters specified in
        /// <paramref name="pCopy"/>.  See the definition of the ::CUDA_MEMCPY3D_PEER structure
        /// for documentation of its parameters.</summary>
        ///
        /// <param name="pCopy">Parameters for the memory copy</param>
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </return>
        /// \notefnerr
        /// \note_sync
        ///
        /// \sa ::cuMemcpyDtoD, ::cuMemcpyPeer, ::cuMemcpyDtoDAsync, ::cuMemcpyPeerAsync,
        /// ::cuMemcpy3DPeerAsync,
        /// ::cudaMemcpy3DPeer
        /// CUresult CUDAAPI cuMemcpy3DPeer(const CUDA_MEMCPY3D_PEER *pCopy);
        [DllImport(_dllpath, EntryPoint = "cuMemcpy3DPeer")]
        public static extern CuResult Memcpy3DPeer(ref CudaMemcpy3D pCopy);

        /// <summary>Copies memory asynchronously
        ///
        /// Copies data between two pointers.
        /// <c>dst</c> and <c>src</c> are base pointers of the destination and source, respectively.
        /// <c>ByteCount</c> specifies the number of bytes to copy.
        /// Note that this function infers the type of the transfer (host to host, host to
        ///   device, device to device, or device to host) from the pointer values.  This
        ///   function is only allowed in contexts which support unified addressing.</summary>
        ///
        /// <param name="dst">Destination unified virtual address space pointer</param>
        /// <param name="src">Source unified virtual address space pointer</param>
        /// <param name="byteCount">Size of memory copy in bytes</param>
        /// <param name="hStream">Stream identifier</param>
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </return>
        /// \notefnerr
        /// \note_async
        /// \note_null_stream
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D8Async,
        /// ::cuMemsetD2D16, ::cuMemsetD2D16Async, ::cuMemsetD2D32, ::cuMemsetD2D32Async,
        /// ::cuMemsetD8, ::cuMemsetD8Async, ::cuMemsetD16, ::cuMemsetD16Async,
        /// ::cuMemsetD32, ::cuMemsetD32Async,
        /// ::cudaMemcpyAsync,
        /// ::cudaMemcpyToSymbolAsync,
        /// ::cudaMemcpyFromSymbolAsync
        /// CUresult CUDAAPI cuMemcpyAsync(CUdeviceptr dst, CUdeviceptr src, size_t ByteCount, CUstream hStream);
        [DllImport(_dllpath, EntryPoint = "cuMemcpyAsync")]
        public static extern CuResult MemcpyAsync(CuDevicePtr dst, CuDevicePtr src, IntPtr byteCount, CuStream hStream);

        /// <summary>Copies device memory between two contexts asynchronously.
        ///
        /// Copies from device memory in one context to device memory in another
        /// context. <c>dstDevice</c> is the base device pointer of the destination memory
        /// and <c>dstContext</c> is the destination context.  <c>srcDevice</c> is the base
        /// device pointer of the source memory and <c>srcContext</c> is the source pointer.
        /// <c>ByteCount</c> specifies the number of bytes to copy.</summary>
        ///
        /// <param name="dstDevice">Destination device pointer</param>
        /// <param name="dstContext">Destination context</param>
        /// <param name="srcDevice">Source device pointer</param>
        /// <param name="srcContext">Source context</param>
        /// <param name="byteCount">Size of memory copy in bytes</param>
        /// <param name="hStream">Stream identifier</param>
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </return>
        /// \notefnerr
        /// \note_async
        /// \note_null_stream
        ///
        /// \sa ::cuMemcpyDtoD, ::cuMemcpyPeer, ::cuMemcpy3DPeer, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpy3DPeerAsync,
        /// ::cudaMemcpyPeerAsync
        /// CUresult CUDAAPI cuMemcpyPeerAsync(CUdeviceptr dstDevice, CUcontext dstContext, CUdeviceptr srcDevice, CUcontext srcContext, size_t ByteCount, CUstream hStream);
        [DllImport(_dllpath, EntryPoint = "cuMemcpyPeerAsync")]
        public static extern CuResult MemcpyPeerAsync(CuDevicePtr dstDevice, CuContext dstContext, CuDevicePtr srcDevice, CuContext srcContext, IntPtr byteCount, CuStream hStream);

        /// <summary>Copies memory from Host to Device
        ///
        /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are
        /// the base addresses of the destination and source, respectively. <c>ByteCount</c>
        /// specifies the number of bytes to copy.</summary>
        ///
        /// <param name="dstDevice">Destination device pointer</param>
        /// <param name="srcHost">Source host pointer</param>
        /// <param name="byteCount">Size of memory copy in bytes</param>
        /// <param name="hStream">Stream identifier</param>
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </return>
        /// \notefnerr
        /// \note_async
        /// \note_null_stream
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D8Async,
        /// ::cuMemsetD2D16, ::cuMemsetD2D16Async, ::cuMemsetD2D32, ::cuMemsetD2D32Async,
        /// ::cuMemsetD8, ::cuMemsetD8Async, ::cuMemsetD16, ::cuMemsetD16Async,
        /// ::cuMemsetD32, ::cuMemsetD32Async,
        /// ::cudaMemcpyAsync,
        /// ::cudaMemcpyToSymbolAsync
        /// CUresult CUDAAPI cuMemcpyHtoDAsync(CUdeviceptr dstDevice, const void *srcHost, size_t ByteCount, CUstream hStream);
        [DllImport(_dllpath, EntryPoint = "cuMemcpyHtoDAsync")]
        public static extern CuResult MemcpyHtoDAsync(CuDevicePtr dstDevice, IntPtr srcHost, IntPtr byteCount, CuStream hStream);

        /// <summary>Copies memory from Device to Host
        ///
        /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the
        /// base pointers of the destination and source, respectively. <c>ByteCount</c>
        /// specifies the number of bytes to copy.</summary>
        ///
        /// <param name="dstHost">Destination host pointer</param>
        /// <param name="srcDevice">Source device pointer</param>
        /// <param name="byteCount">Size of memory copy in bytes</param>
        /// <param name="hStream">Stream identifier</param>
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </return>
        /// \notefnerr
        /// \note_async
        /// \note_null_stream
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D8Async,
        /// ::cuMemsetD2D16, ::cuMemsetD2D16Async, ::cuMemsetD2D32, ::cuMemsetD2D32Async,
        /// ::cuMemsetD8, ::cuMemsetD8Async, ::cuMemsetD16, ::cuMemsetD16Async,
        /// ::cuMemsetD32, ::cuMemsetD32Async,
        /// ::cudaMemcpyAsync,
        /// ::cudaMemcpyFromSymbolAsync
        /// CUresult CUDAAPI cuMemcpyDtoHAsync(void *dstHost, CUdeviceptr srcDevice, size_t ByteCount, CUstream hStream);
        [DllImport(_dllpath, EntryPoint = "cuMemcpyDtoHAsync")]
        public static extern CuResult MemcpyDtoHAsync(IntPtr dstHost, CuDevicePtr srcDevice, IntPtr byteCount, CuStream hStream);

        /// <summary>Copies memory from Device to Device
        ///
        /// Copies from device memory to device memory. <c>dstDevice</c> and <c>srcDevice</c>
        /// are the base pointers of the destination and source, respectively.
        /// <c>ByteCount</c> specifies the number of bytes to copy.</summary>
        ///
        /// <param name="dstDevice">Destination device pointer</param>
        /// <param name="srcDevice">Source device pointer</param>
        /// <param name="byteCount">Size of memory copy in bytes</param>
        /// <param name="hStream">Stream identifier</param>
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </return>
        /// \notefnerr
        /// \note_async
        /// \note_null_stream
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D8Async,
        /// ::cuMemsetD2D16, ::cuMemsetD2D16Async, ::cuMemsetD2D32, ::cuMemsetD2D32Async,
        /// ::cuMemsetD8, ::cuMemsetD8Async, ::cuMemsetD16, ::cuMemsetD16Async,
        /// ::cuMemsetD32, ::cuMemsetD32Async,
        /// ::cudaMemcpyAsync,
        /// ::cudaMemcpyToSymbolAsync,
        /// ::cudaMemcpyFromSymbolAsync
        /// CUresult CUDAAPI cuMemcpyDtoDAsync(CUdeviceptr dstDevice, CUdeviceptr srcDevice, size_t ByteCount, CUstream hStream);
        [DllImport(_dllpath, EntryPoint = "cuMemcpyDtoDAsync")]
        public static extern CuResult MemcpyDtoDAsync(CuDevicePtr dstDevice, CuDevicePtr srcDevice, IntPtr byteCount, CuStream hStream);

        /// <summary>Copies memory from Host to Array
        ///
        /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c>
        /// specify the CUDA array handle and starting offset in bytes of the
        /// destination data. <c>srcHost</c> specifies the base address of the source.
        /// <c>ByteCount</c> specifies the number of bytes to copy.</summary>
        ///
        /// <param name="dstArray">Destination array</param>
        /// <param name="dstOffset">Offset in bytes of destination array</param>
        /// <param name="srcHost">Source host pointer</param>
        /// <param name="byteCount">Size of memory copy in bytes</param>
        /// <param name="hStream">Stream identifier</param>
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </return>
        /// \notefnerr
        /// \note_async
        /// \note_null_stream
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D8Async,
        /// ::cuMemsetD2D16, ::cuMemsetD2D16Async, ::cuMemsetD2D32, ::cuMemsetD2D32Async,
        /// ::cuMemsetD8, ::cuMemsetD8Async, ::cuMemsetD16, ::cuMemsetD16Async,
        /// ::cuMemsetD32, ::cuMemsetD32Async,
        /// ::cudaMemcpyToArrayAsync
        /// CUresult CUDAAPI cuMemcpyHtoAAsync(CUarray dstArray, size_t dstOffset, const void *srcHost, size_t ByteCount, CUstream hStream);
        [DllImport(_dllpath, EntryPoint = "cuMemcpyHtoAAsync")]
        public static extern CuResult MemcpyHtoAAsync(CuArray dstArray, IntPtr dstOffset, IntPtr srcHost, IntPtr byteCount, CuStream hStream);

        /// <summary>Copies memory from Array to Host
        ///
        /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base
        /// pointer of the destination. <c>srcArray</c> and <c>srcOffset</c> specify the CUDA
        /// array handle and starting offset in bytes of the source data.
        /// <c>ByteCount</c> specifies the number of bytes to copy.</summary>
        ///
        /// <param name="dstHost">Destination pointer</param>
        /// <param name="srcArray">Source array</param>
        /// <param name="srcOffset">Offset in bytes of source array</param>
        /// <param name="byteCount">Size of memory copy in bytes</param>
        /// <param name="hStream">Stream identifier</param>
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </return>
        /// \notefnerr
        /// \note_async
        /// \note_null_stream
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D8Async,
        /// ::cuMemsetD2D16, ::cuMemsetD2D16Async, ::cuMemsetD2D32, ::cuMemsetD2D32Async,
        /// ::cuMemsetD8, ::cuMemsetD8Async, ::cuMemsetD16, ::cuMemsetD16Async,
        /// ::cuMemsetD32, ::cuMemsetD32Async,
        /// ::cudaMemcpyFromArrayAsync
        /// CUresult CUDAAPI cuMemcpyAtoHAsync(void *dstHost, CUarray srcArray, size_t srcOffset, size_t ByteCount, CUstream hStream);
        [DllImport(_dllpath, EntryPoint = "cuMemcpyAtoHAsync")]
        public static extern CuResult MemcpyAtoHAsync(IntPtr dstHost, CuArray srcArray, IntPtr srcOffset, IntPtr byteCount, CuStream hStream);

        /// <summary>Copies memory for 2D arrays</summary>
        ///
        /// <remarks>
        /// Perform a 2D memory copy according to the parameters specified in <paramref name="pCopy"/>.
        ///
        /// <para>
        /// If ::srcMemoryType is ::CU_MEMORYTYPE_HOST, ::srcHost and ::srcPitch
        /// specify the (host) base address of the source data and the bytes per row to
        /// apply. ::srcArray is ignored.
        /// </para>
        /// <para>
        /// If ::srcMemoryType is ::CU_MEMORYTYPE_UNIFIED, ::srcDevice and ::srcPitch
        ///   specify the (unified virtual address space) base address of the source data
        ///   and the bytes per row to apply.  ::srcArray is ignored.
        /// This value may be used only if unified addressing is supported in the calling
        ///   context.
        /// </para>
        /// <para>
        /// If ::srcMemoryType is ::CU_MEMORYTYPE_DEVICE, ::srcDevice and ::srcPitch
        /// specify the (device) base address of the source data and the bytes per row
        /// to apply. ::srcArray is ignored.
        /// </para>
        /// <para>
        /// If ::srcMemoryType is ::CU_MEMORYTYPE_ARRAY, ::srcArray specifies the
        /// handle of the source data. ::srcHost, ::srcDevice and ::srcPitch are
        /// ignored.
        /// </para>
        /// <para>
        /// If ::dstMemoryType is ::CU_MEMORYTYPE_UNIFIED, ::dstDevice and ::dstPitch
        ///   specify the (unified virtual address space) base address of the source data
        ///   and the bytes per row to apply.  ::dstArray is ignored.
        /// This value may be used only if unified addressing is supported in the calling
        ///   context.
        /// </para>
        /// <para>
        /// If ::dstMemoryType is ::CU_MEMORYTYPE_HOST, ::dstHost and ::dstPitch
        /// specify the (host) base address of the destination data and the bytes per
        /// row to apply. ::dstArray is ignored.
        /// </para>
        /// <para>
        /// If ::dstMemoryType is ::CU_MEMORYTYPE_DEVICE, ::dstDevice and ::dstPitch
        /// specify the (device) base address of the destination data and the bytes per
        /// row to apply. ::dstArray is ignored.
        /// </para>
        /// <para>
        /// If ::dstMemoryType is ::CU_MEMORYTYPE_ARRAY, ::dstArray specifies the
        /// handle of the destination data. ::dstHost, ::dstDevice and ::dstPitch are
        /// ignored.
        ///
        /// - ::srcXInBytes and ::srcY specify the base address of the source data for
        ///   the copy.
        /// </para>
        ///
        /// <para>
        /// For host pointers, the starting address is
        /// <code>
        ///  void* Start = (void*)((char*)srcHost+srcY*srcPitch + srcXInBytes);
        /// </code>
        /// </para>
        ///
        /// <para>
        /// For device pointers, the starting address is
        /// <code>
        ///  CUdeviceptr Start = srcDevice+srcY*srcPitch+srcXInBytes;
        /// </code>
        /// </para>
        ///
        /// <para>
        /// For CUDA arrays, ::srcXInBytes must be evenly divisible by the array
        /// element size.
        ///
        /// - ::dstXInBytes and ::dstY specify the base address of the destination data
        ///   for the copy.
        /// </para>
        ///
        /// <para>
        /// For host pointers, the base address is
        /// <code>
        ///  void* dstStart = (void*)((char*)dstHost+dstY*dstPitch + dstXInBytes);
        /// </code>
        ///</para>
        ///
        /// <para>
        /// For device pointers, the starting address is
        /// <code>
        ///  CUdeviceptr dstStart = dstDevice+dstY*dstPitch+dstXInBytes;
        /// </code>
        /// </para>
        ///
        /// <para>
        /// For CUDA arrays, ::dstXInBytes must be evenly divisible by the array
        /// element size.
        ///
        /// - ::WidthInBytes and ::Height specify the width (in bytes) and height of
        ///   the 2D copy being performed.
        /// - If specified, ::srcPitch must be greater than or equal to ::WidthInBytes +
        ///   ::srcXInBytes, and ::dstPitch must be greater than or equal to
        ///   ::WidthInBytes + dstXInBytes.
        /// - If specified, ::srcPitch must be greater than or equal to ::WidthInBytes +
        ///   ::srcXInBytes, and ::dstPitch must be greater than or equal to
        ///   ::WidthInBytes + dstXInBytes.
        /// - If specified, ::srcHeight must be greater than or equal to ::Height +
        ///   ::srcY, and ::dstHeight must be greater than or equal to ::Height + ::dstY.
        /// </para>
        ///
        /// <para>
        /// ::cuMemcpy2DAsync() returns an error if any pitch is greater than the maximum
        /// allowed (::CU_DEVICE_ATTRIBUTE_MAX_PITCH). ::cuMemAllocPitch() passes back
        /// pitches that always work with ::cuMemcpy2D(). On intra-device memory copies
        /// (device to device, CUDA array to device, CUDA array to CUDA array),
        /// ::cuMemcpy2DAsync() may fail for pitches not computed by ::cuMemAllocPitch().
        /// </para>
        /// </remarks>
        ///
        /// <param name="pCopy">Parameters for the memory copy</param>
        /// <param name="hStream">Stream identifier</param>
        ///
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </return>
        ///
        /// \notefnerr
        /// \note_async
        /// \note_null_stream
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D8Async,
        /// ::cuMemsetD2D16, ::cuMemsetD2D16Async, ::cuMemsetD2D32, ::cuMemsetD2D32Async,
        /// ::cuMemsetD8, ::cuMemsetD8Async, ::cuMemsetD16, ::cuMemsetD16Async,
        /// ::cuMemsetD32, ::cuMemsetD32Async,
        /// ::cudaMemcpy2DAsync,
        /// ::cudaMemcpy2DToArrayAsync,
        /// ::cudaMemcpy2DFromArrayAsync
        /// CUresult CUDAAPI cuMemcpy2DAsync(const CUDA_MEMCPY2D *pCopy, CUstream hStream);
        [DllImport(_dllpath, EntryPoint = "cuMemcpy2DAsync")]
        public static extern CuResult Memcpy2DAsync(ref CudaMemcopy2D pCopy, CuStream hStream);

        /// <summary>Copies memory for 3D arrays</summary>
        ///
        /// <remarks>
        /// Perform a 3D memory copy according to the parameters specified in
        /// <paramref name="pCopy"/>.
        ///
        /// <para>
        /// If ::srcMemoryType is ::CU_MEMORYTYPE_UNIFIED, ::srcDevice and ::srcPitch
        ///   specify the (unified virtual address space) base address of the source data
        ///   and the bytes per row to apply.  ::srcArray is ignored.
        /// This value may be used only if unified addressing is supported in the calling
        ///   context.
        /// </para>
        ///
        /// <para>
        /// If ::srcMemoryType is ::CU_MEMORYTYPE_HOST, ::srcHost, ::srcPitch and
        /// ::srcHeight specify the (host) base address of the source data, the bytes
        /// per row, and the height of each 2D slice of the 3D array. ::srcArray is
        /// ignored.
        /// </para>
        ///
        /// <para>
        /// If ::srcMemoryType is ::CU_MEMORYTYPE_DEVICE, ::srcDevice, ::srcPitch and
        /// ::srcHeight specify the (device) base address of the source data, the bytes
        /// per row, and the height of each 2D slice of the 3D array. ::srcArray is
        /// ignored.
        /// </para>
        ///
        /// <para>
        /// If ::srcMemoryType is ::CU_MEMORYTYPE_ARRAY, ::srcArray specifies the
        /// handle of the source data. ::srcHost, ::srcDevice, ::srcPitch and
        /// ::srcHeight are ignored.
        /// </para>
        ///
        /// <para>
        /// If ::dstMemoryType is ::CU_MEMORYTYPE_UNIFIED, ::dstDevice and ::dstPitch
        ///   specify the (unified virtual address space) base address of the source data
        ///   and the bytes per row to apply.  ::dstArray is ignored.
        /// This value may be used only if unified addressing is supported in the calling
        ///   context.
        /// </para>
        ///
        /// <para>
        /// If ::dstMemoryType is ::CU_MEMORYTYPE_HOST, ::dstHost and ::dstPitch
        /// specify the (host) base address of the destination data, the bytes per row,
        /// and the height of each 2D slice of the 3D array. ::dstArray is ignored.
        /// </para>
        ///
        /// <para>
        /// If ::dstMemoryType is ::CU_MEMORYTYPE_DEVICE, ::dstDevice and ::dstPitch
        /// specify the (device) base address of the destination data, the bytes per
        /// row, and the height of each 2D slice of the 3D array. ::dstArray is ignored.
        /// </para>
        ///
        /// <para>
        /// If ::dstMemoryType is ::CU_MEMORYTYPE_ARRAY, ::dstArray specifies the
        /// handle of the destination data. ::dstHost, ::dstDevice, ::dstPitch and
        /// ::dstHeight are ignored.
        ///
        /// - ::srcXInBytes, ::srcY and ::srcZ specify the base address of the source
        ///   data for the copy.
        /// </para>
        ///
        /// <para>
        /// For host pointers, the starting address is
        /// <code>
        ///  void* Start = (void*)((char*)srcHost+(srcZ*srcHeight+srcY)*srcPitch + srcXInBytes);
        /// </code>
        /// </para>
        ///
        /// <para>
        /// For device pointers, the starting address is
        /// <code>
        ///  CUdeviceptr Start = srcDevice+(srcZ*srcHeight+srcY)*srcPitch+srcXInBytes;
        /// </code>
        /// </para>
        ///
        /// <para>
        /// For CUDA arrays, ::srcXInBytes must be evenly divisible by the array
        /// element size.
        ///
        /// - dstXInBytes, ::dstY and ::dstZ specify the base address of the
        ///   destination data for the copy.
        /// </para>
        ///
        /// <para>
        /// For host pointers, the base address is
        /// <code>
        ///  void* dstStart = (void*)((char*)dstHost+(dstZ*dstHeight+dstY)*dstPitch + dstXInBytes);
        /// </code>
        /// </para>
        ///
        /// <para>
        /// For device pointers, the starting address is
        /// <code>
        ///  CUdeviceptr dstStart = dstDevice+(dstZ*dstHeight+dstY)*dstPitch+dstXInBytes;
        /// </code>
        /// </para>
        ///
        /// <para>
        /// For CUDA arrays, ::dstXInBytes must be evenly divisible by the array
        /// element size.
        ///
        /// - ::WidthInBytes, ::Height and ::Depth specify the width (in bytes), height
        ///   and depth of the 3D copy being performed.
        /// - If specified, ::srcPitch must be greater than or equal to ::WidthInBytes +
        ///   ::srcXInBytes, and ::dstPitch must be greater than or equal to
        ///   ::WidthInBytes + dstXInBytes.
        /// - If specified, ::srcHeight must be greater than or equal to ::Height +
        ///   ::srcY, and ::dstHeight must be greater than or equal to ::Height + ::dstY.
        /// </para>
        ///
        /// <para>
        /// ::cuMemcpy3DAsync() returns an error if any pitch is greater than the maximum
        /// allowed (::CU_DEVICE_ATTRIBUTE_MAX_PITCH).
        ///
        /// The ::srcLOD and ::dstLOD members of the ::CUDA_MEMCPY3D structure must be
        /// set to 0.
        /// </para>
        /// </remarks>
        ///
        /// <param name="pCopy">Parameters for the memory copy</param>
        /// <param name="hStream">Stream identifier</param>
        ///
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </return>
        ///
        /// \notefnerr
        /// \note_async
        /// \note_null_stream
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D8Async,
        /// ::cuMemsetD2D16, ::cuMemsetD2D16Async, ::cuMemsetD2D32, ::cuMemsetD2D32Async,
        /// ::cuMemsetD8, ::cuMemsetD8Async, ::cuMemsetD16, ::cuMemsetD16Async,
        /// ::cuMemsetD32, ::cuMemsetD32Async,
        /// ::cudaMemcpy3DAsync
        /// CUresult CUDAAPI cuMemcpy3DAsync(const CUDA_MEMCPY3D *pCopy, CUstream hStream);
        [DllImport(_dllpath, EntryPoint = "cuMemcpy3DAsync")]
        public static extern CuResult Memcpy3DAsync(ref CudaMemcpy3D pCopy, CuStream hStream);

        /// <summary>Copies memory between contexts asynchronously.</summary>
        ///
        /// <remarks>
        /// Perform a 3D memory copy according to the parameters specified in
        /// <paramref name="pCopy"/>.  See the definition of the ::CUDA_MEMCPY3D_PEER structure
        /// for documentation of its parameters.
        /// </remarks>
        ///
        /// <param name="pCopy">Parameters for the memory copy</param>
        /// <param name="hStream">Stream identifier</param>
        ///
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </return>
        ///
        /// \notefnerr
        /// \note_async
        /// \note_null_stream
        ///
        /// \sa ::cuMemcpyDtoD, ::cuMemcpyPeer, ::cuMemcpyDtoDAsync, ::cuMemcpyPeerAsync,
        /// ::cuMemcpy3DPeerAsync,
        /// ::cudaMemcpy3DPeerAsync
        /// CUresult CUDAAPI cuMemcpy3DPeerAsync(const CUDA_MEMCPY3D_PEER *pCopy, CUstream hStream);
        [DllImport(_dllpath, EntryPoint = "cuMemcpy3DPeerAsync")]
        public static extern CuResult Memcpy3DPeerAsync(ref CudaMemcpy3DPeer pCopy, CuStream hStream);
        #endregion

        #region Stream

        /// <summary>CUDA stream callback</summary>
        ///
        /// <param name="hStream">The stream the callback was added to, as passed to ::cuStreamAddCallback.  May be NULL.</param>
        /// <param name="status">::CUDA_SUCCESS or any persistent error on the stream.</param>
        /// <param name="userData">User parameter provided at registration.</param>
        public delegate void CuStreamCallback(CuStream hStream, CuResult status, IntPtr userData);

        /// <summary>Create a stream
        ///
        /// Creates a stream and returns a handle in <c>phStream</c>.  The <c>Flags</c> argument
        /// determines behaviors of the stream.  Valid values for <c>Flags</c> are:
        /// - ::CU_STREAM_DEFAULT: Default stream creation flag.
        /// - ::CU_STREAM_NON_BLOCKING: Specifies that work running in the created
        ///   stream may run concurrently with work in stream 0 (the NULL stream), and that
        ///   the created stream should perform no implicit synchronization with stream 0.</summary>
        ///
        /// <param name="phStream">Returned newly created stream</param>
        /// <param name="flags">Parameters for stream creation</param>
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_OUT_OF_MEMORY
        /// </return>
        /// \notefnerr
        ///
        /// \sa ::cuStreamDestroy,
        /// ::cuStreamCreateWithPriority,
        /// ::cuStreamGetPriority,
        /// ::cuStreamGetFlags,
        /// ::cuStreamWaitEvent,
        /// ::cuStreamQuery,
        /// ::cuStreamSynchronize,
        /// ::cuStreamAddCallback,
        /// ::cudaStreamCreate,
        /// ::cudaStreamCreateWithFlags
        /// CUresult CUDAAPI cuStreamCreate(CUstream *phStream, unsigned int Flags);
        [DllImport(_dllpath, EntryPoint = "cuStreamCreate")]
        public static extern CuResult StreamCreate(out CuStream phStream, CuStreamFlags flags);

        /// <summary>Create a stream with the given priority
        ///
        /// Creates a stream with the specified priority and returns a handle in <c>phStream</c>.
        /// This API alters the scheduler priority of work in the stream. Work in a higher
        /// priority stream may preempt work already executing in a low priority stream.
        ///
        /// <c>priority</c> follows a convention where lower numbers represent higher priorities.
        /// '0' represents default priority. The range of meaningful numerical priorities can
        /// be queried using ::cuCtxGetStreamPriorityRange. If the specified priority is
        /// outside the numerical range returned by ::cuCtxGetStreamPriorityRange,
        /// it will automatically be clamped to the lowest or the highest number in the range.</summary>
        ///
        /// <param name="phStream">Returned newly created stream</param>
        /// <param name="flags">Flags for stream creation. See ::cuStreamCreate for a list of
        ///                      valid flags</param>
        /// <param name="priority">Stream priority. Lower numbers represent higher priorities.
        ///                      See ::cuCtxGetStreamPriorityRange for more information about
        ///                      meaningful stream priorities that can be passed.</param>
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_OUT_OF_MEMORY
        /// </return>
        /// \notefnerr
        ///
        /// \note Stream priorities are supported only on GPUs
        /// with compute capability 3.5 or higher.
        ///
        /// \note In the current implementation, only compute kernels launched in
        /// priority streams are affected by the stream's priority. Stream priorities have
        /// no effect on host-to-device and device-to-host memory operations.
        ///
        /// \sa ::cuStreamDestroy,
        /// ::cuStreamCreate,
        /// ::cuStreamGetPriority,
        /// ::cuCtxGetStreamPriorityRange,
        /// ::cuStreamGetFlags,
        /// ::cuStreamWaitEvent,
        /// ::cuStreamQuery,
        /// ::cuStreamSynchronize,
        /// ::cuStreamAddCallback,
        /// ::cudaStreamCreateWithPriority
        /// CUresult CUDAAPI cuStreamCreateWithPriority(CUstream *phStream, unsigned int flags, int priority);
        [DllImport(_dllpath, EntryPoint = "cuStreamCreateWithPriority")]
        public static extern CuResult StreamCreateWithPriority(out CuStream phStream, CuStreamFlags flags, int priority);

        /// <summary>Query the priority of a given stream
        ///
        /// Query the priority of a stream created using ::cuStreamCreate or ::cuStreamCreateWithPriority
        /// and return the priority in <c>priority</c>. Note that if the stream was created with a
        /// priority outside the numerical range returned by ::cuCtxGetStreamPriorityRange,
        /// this function returns the clamped priority.
        /// See ::cuStreamCreateWithPriority for details about priority clamping.</summary>
        ///
        /// <param name="hStream">Handle to the stream to be queried</param>
        /// <param name="priority">Pointer to a signed integer in which the stream's priority is returned</param>
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_OUT_OF_MEMORY
        /// </return>
        /// \notefnerr
        ///
        /// \sa ::cuStreamDestroy,
        /// ::cuStreamCreate,
        /// ::cuStreamCreateWithPriority,
        /// ::cuCtxGetStreamPriorityRange,
        /// ::cuStreamGetFlags,
        /// ::cudaStreamGetPriority
        /// CUresult CUDAAPI cuStreamGetPriority(CUstream hStream, int *priority);
        [DllImport(_dllpath, EntryPoint = "cuStreamGetPriority")]
        public static extern CuResult StreamGetPriority(CuStream hStream, out int priority);

        /// <summary>Query the flags of a given stream
        ///
        /// Query the flags of a stream created using ::cuStreamCreate or ::cuStreamCreateWithPriority
        /// and return the flags in <c>flags</c>.</summary>
        ///
        /// <param name="hStream">Handle to the stream to be queried</param>
        /// <param name="flags">Pointer to an unsigned integer in which the stream's flags are returned
        ///                     The value returned in <c>flags</c> is a logical 'OR' of all flags that
        ///                     were used while creating this stream. See ::cuStreamCreate for the list
        ///                     of valid flags</param>
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_OUT_OF_MEMORY
        /// </return>
        /// \notefnerr
        ///
        /// \sa ::cuStreamDestroy,
        /// ::cuStreamCreate,
        /// ::cuStreamGetPriority,
        /// ::cudaStreamGetFlags
        /// CUresult CUDAAPI cuStreamGetFlags(CUstream hStream, unsigned int *flags);
        [DllImport(_dllpath, EntryPoint = "cuStreamGetFlags")]
        public static extern CuResult StreamGetFlags(CuStream hStream, out CuStreamFlags flags);

        /// <summary>Make a compute stream wait on an event
        ///
        /// Makes all future work submitted to <c>hStream</c> wait until <c>hEvent</c>
        /// reports completion before beginning execution.  This synchronization
        /// will be performed efficiently on the device.  The event <c>hEvent</c> may
        /// be from a different context than <c>hStream</c>, in which case this function
        /// will perform cross-device synchronization.
        ///
        /// The stream <c>hStream</c> will wait only for the completion of the most recent
        /// host call to ::cuEventRecord() on <c>hEvent</c>.  Once this call has returned,
        /// any functions (including ::cuEventRecord() and ::cuEventDestroy()) may be
        /// called on <c>hEvent</c> again, and subsequent calls will not have any
        /// effect on <c>hStream</c>.
        ///
        /// If ::cuEventRecord() has not been called on <c>hEvent</c>, this call acts as if
        /// the record has already completed, and so is a functional no-op.</summary>
        ///
        /// <param name="hStream">Stream to wait</param>
        /// <param name="hEvent">Event to wait on (may not be NULL)</param>
        /// <param name="flags">Parameters for the operation (must be 0)</param>
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// \note_null_stream
        /// </return>
        /// \notefnerr
        ///
        /// \sa ::cuStreamCreate,
        /// ::cuEventRecord,
        /// ::cuStreamQuery,
        /// ::cuStreamSynchronize,
        /// ::cuStreamAddCallback,
        /// ::cuStreamDestroy,
        /// ::cudaStreamWaitEvent
        /// CUresult CUDAAPI cuStreamWaitEvent(CUstream hStream, CUevent hEvent, unsigned int Flags);
        [DllImport(_dllpath, EntryPoint = "cuStreamWaitEvent")]
        public static extern CuResult StreamWaitEvent(CuStream hStream, CuEvent hEvent, uint flags = 0);

        /// <summary>Add a callback to a compute stream
        ///
        /// Adds a callback to be called on the host after all currently enqueued
        /// items in the stream have completed.  For each
        /// cuStreamAddCallback call, the callback will be executed exactly once.
        /// The callback will block later work in the stream until it is finished.
        ///
        /// The callback may be passed ::CUDA_SUCCESS or an error code.  In the event
        /// of a device error, all subsequently executed callbacks will receive an
        /// appropriate ::CUresult.
        ///
        /// Callbacks must not make any CUDA API calls.  Attempting to use a CUDA API
        /// will result in ::CUDA_ERROR_NOT_PERMITTED.  Callbacks must not perform any
        /// synchronization that may depend on outstanding device work or other callbacks
        /// that are not mandated to run earlier.  Callbacks without a mandated order
        /// (in independent streams) execute in undefined order and may be serialized.
        ///
        /// For the purposes of Unified Memory, callback execution makes a number of
        /// guarantees:
        /// <ul>
        ///   <li>The callback stream is considered idle for the duration of the
        ///   callback.  Thus, for example, a callback may always use memory attached
        ///   to the callback stream.</li>
        ///   <li>The start of execution of a callback has the same effect as
        ///   synchronizing an event recorded in the same stream immediately prior to
        ///   the callback.  It thus synchronizes streams which have been "joined"
        ///   prior to the callback.</li>
        ///   <li>Adding device work to any stream does not have the effect of making
        ///   the stream active until all preceding callbacks have executed.  Thus, for
        ///   example, a callback might use global attached memory even if work has
        ///   been added to another stream, if it has been properly ordered with an
        ///   event.</li>
        ///   <li>Completion of a callback does not cause a stream to become
        ///   active except as described above.  The callback stream will remain idle
        ///   if no device work follows the callback, and will remain idle across
        ///   consecutive callbacks without device work in between.  Thus, for example,
        ///   stream synchronization can be done by signaling from a callback at the
        ///   end of the stream.</li>
        /// </ul></summary>
        ///
        /// <param name="hStream">Stream to add callback to</param>
        /// <param name="callback">The function to call once preceding stream operations are complete</param>
        /// <param name="userData">User specified data to be passed to the callback function</param>
        /// <param name="flags">Reserved for future use, must be 0</param>
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_NOT_SUPPORTED
        /// \note_null_stream
        /// </return>
        /// \notefnerr
        ///
        /// \sa ::cuStreamCreate,
        /// ::cuStreamQuery,
        /// ::cuStreamSynchronize,
        /// ::cuStreamWaitEvent,
        /// ::cuStreamDestroy,
        /// ::cuMemAllocManaged,
        /// ::cuStreamAttachMemAsync,
        /// ::cudaStreamAddCallback
        /// CUresult CUDAAPI cuStreamAddCallback(CUstream hStream, CUstreamCallback callback, void *userData, unsigned int flags);
        [DllImport(_dllpath, EntryPoint = "cuStreamAddCallback")]
        public static extern CuResult StreamAddCallback(CuStream hStream, CuStreamCallback callback, IntPtr userData, uint flags = 0);

        /// <summary>Attach memory to a stream asynchronously
        ///
        /// Enqueues an operation in <c>hStream</c> to specify stream association of
        /// <c>length</c> bytes of memory starting from <c>dptr</c>. This function is a
        /// stream-ordered operation, meaning that it is dependent on, and will
        /// only take effect when, previous work in stream has completed. Any
        /// previous association is automatically replaced.
        ///
        /// <c>dptr</c> must point to an address within managed memory space declared
        /// using the __managed__ keyword or allocated with ::cuMemAllocManaged.
        ///
        /// <c>length</c> must be zero, to indicate that the entire allocation's
        /// stream association is being changed. Currently, it's not possible
        /// to change stream association for a portion of an allocation.
        ///
        /// The stream association is specified using <c>flags</c> which must be
        /// one of ::CUmemAttach_flags.
        /// If the ::CU_MEM_ATTACH_GLOBAL flag is specified, the memory can be accessed
        /// by any stream on any device.
        /// If the ::CU_MEM_ATTACH_HOST flag is specified, the program makes a guarantee
        /// that it won't access the memory on the device from any stream on a device that
        /// has a zero value for the device attribute ::CU_DEVICE_ATTRIBUTE_CONCURRENT_MANAGED_ACCESS.
        /// If the ::CU_MEM_ATTACH_SINGLE flag is specified and <c>hStream</c> is associated with
        /// a device that has a zero value for the device attribute ::CU_DEVICE_ATTRIBUTE_CONCURRENT_MANAGED_ACCESS,
        /// the program makes a guarantee that it will only access the memory on the device
        /// from <c>hStream</c>. It is illegal to attach singly to the NULL stream, because the
        /// NULL stream is a virtual global stream and not a specific stream. An error will
        /// be returned in this case.
        ///
        /// When memory is associated with a single stream, the Unified Memory system will
        /// allow CPU access to this memory region so long as all operations in <c>hStream</c>
        /// have completed, regardless of whether other streams are active. In effect,
        /// this constrains exclusive ownership of the managed memory region by
        /// an active GPU to per-stream activity instead of whole-GPU activity.
        ///
        /// Accessing memory on the device from streams that are not associated with
        /// it will produce undefined results. No error checking is performed by the
        /// Unified Memory system to ensure that kernels launched into other streams
        /// do not access this region.
        ///
        /// It is a program's responsibility to order calls to ::cuStreamAttachMemAsync
        /// via events, synchronization or other means to ensure legal access to memory
        /// at all times. Data visibility and coherency will be changed appropriately
        /// for all kernels which follow a stream-association change.
        ///
        /// If <c>hStream</c> is destroyed while data is associated with it, the association is
        /// removed and the association reverts to the default visibility of the allocation
        /// as specified at ::cuMemAllocManaged. For __managed__ variables, the default
        /// association is always ::CU_MEM_ATTACH_GLOBAL. Note that destroying a stream is an
        /// asynchronous operation, and as a result, the change to default association won't
        /// happen until all work in the stream has completed.</summary>
        ///
        /// <param name="hStream">Stream in which to enqueue the attach operation</param>
        /// <param name="dptr">Pointer to memory (must be a pointer to managed memory)</param>
        /// <param name="length">Length of memory (must be zero)</param>
        /// <param name="flags">Must be one of ::CUmemAttach_flags</param>
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_NOT_SUPPORTED
        /// \note_null_stream
        /// </return>
        /// \notefnerr
        ///
        /// \sa ::cuStreamCreate,
        /// ::cuStreamQuery,
        /// ::cuStreamSynchronize,
        /// ::cuStreamWaitEvent,
        /// ::cuStreamDestroy,
        /// ::cuMemAllocManaged,
        /// ::cudaStreamAttachMemAsync
        /// CUresult CUDAAPI cuStreamAttachMemAsync(CUstream hStream, CUdeviceptr dptr, size_t length, unsigned int flags);
        [DllImport(_dllpath, EntryPoint = "cuStreamAttachMemAsync")]
        public static extern CuResult StreamAttachMemAsync(CuStream hStream, CuDevicePtr dptr, IntPtr length, CuMemAttachFlags flags);

        /// <summary>Determine status of a compute stream
        ///
        /// Returns ::CUDA_SUCCESS if all operations in the stream specified by
        /// <c>hStream</c> have completed, or ::CUDA_ERROR_NOT_READY if not.
        ///
        /// For the purposes of Unified Memory, a return value of ::CUDA_SUCCESS
        /// is equivalent to having called ::cuStreamSynchronize().</summary>
        ///
        /// <param name="hStream">Stream to query status of</param>
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_NOT_READY
        /// \note_null_stream
        /// </return>
        /// \notefnerr
        ///
        /// \sa ::cuStreamCreate,
        /// ::cuStreamWaitEvent,
        /// ::cuStreamDestroy,
        /// ::cuStreamSynchronize,
        /// ::cuStreamAddCallback,
        /// ::cudaStreamQuery
        /// CUresult CUDAAPI cuStreamQuery(CUstream hStream);
        [DllImport(_dllpath, EntryPoint = "cuStreamQuery")]
        public static extern CuResult StreamQuery(CuStream hStream);

        /// <summary>Wait until a stream's tasks are completed
        ///
        /// Waits until the device has completed all operations in the stream specified
        /// by <c>hStream</c>. If the context was created with the
        /// ::CU_CTX_SCHED_BLOCKING_SYNC flag, the CPU thread will block until the
        /// stream is finished with all of its tasks.</summary>
        ///
        /// <param name="hStream">Stream to wait for</param>
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_HANDLE
        /// \note_null_stream
        /// </return>
        /// \notefnerr
        ///
        /// \sa ::cuStreamCreate,
        /// ::cuStreamDestroy,
        /// ::cuStreamWaitEvent,
        /// ::cuStreamQuery,
        /// ::cuStreamAddCallback,
        /// ::cudaStreamSynchronize
        /// CUresult CUDAAPI cuStreamSynchronize(CUstream hStream);
        [DllImport(_dllpath, EntryPoint = "cuStreamSynchronize")]
        public static extern CuResult StreamSynchronize(CuStream hStream);

        /// <summary>Destroys a stream
        ///
        /// Destroys the stream specified by <c>hStream</c>.
        ///
        /// In case the device is still doing work in the stream <c>hStream</c>
        /// when ::cuStreamDestroy() is called, the function will return immediately
        /// and the resources associated with <c>hStream</c> will be released automatically
        /// once the device has completed all work in <c>hStream</c>.</summary>
        ///
        /// <param name="hStream">Stream to destroy</param>
        /// <return>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </return>
        /// \notefnerr
        ///
        /// \sa ::cuStreamCreate,
        /// ::cuStreamWaitEvent,
        /// ::cuStreamQuery,
        /// ::cuStreamSynchronize,
        /// ::cuStreamAddCallback,
        /// ::cudaStreamDestroy
        /// CUresult CUDAAPI cuStreamDestroy(CUstream hStream);
        [DllImport(_dllpath, EntryPoint = "cuStreamDestroy")]
        public static extern CuResult StreamDestroy(CuStream hStream);
        #endregion
    }

    /// <summary>\enum CUdevice_attribute</summary>
    public enum CuDeviceAttribute
    {
        /// <summary>CU_DEVICE_ATTRIBUTE_MAX_THREADS_PER_BLOCK = 1,</summary>
        MaxThreadsPerBlock = 1,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAX_BLOCK_DIM_X = 2,</summary>
        MaxBlockDimX = 2,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAX_BLOCK_DIM_Y = 3,</summary>
        MaxBlockDimY = 3,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAX_BLOCK_DIM_Z = 4,</summary>
        MaxBlockDimZ = 4,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAX_GRID_DIM_X = 5,</summary>
        MaxGridDimX = 5,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAX_GRID_DIM_Y = 6,</summary>
        MaxGridDimY = 6,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAX_GRID_DIM_Z = 7,</summary>
        MaxGridDimZ = 7,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAX_SHARED_MEMORY_PER_BLOCK = 8,</summary>
        MaxSharedMemoryPerBlock = 8,
        /// <summary>CU_DEVICE_ATTRIBUTE_SHARED_MEMORY_PER_BLOCK = 8,</summary>
        SharedMemoryPerBlock = 8,
        /// <summary>CU_DEVICE_ATTRIBUTE_TOTAL_CONSTANT_MEMORY = 9,</summary>
        TotalConstantMemory = 9,
        /// <summary>CU_DEVICE_ATTRIBUTE_WARP_SIZE = 10,</summary>
        WarpSize = 10,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAX_PITCH = 11,</summary>
        MaxPitch = 11,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAX_REGISTERS_PER_BLOCK = 12,</summary>
        MaxRegistersPerBlock = 12,
        /// <summary>CU_DEVICE_ATTRIBUTE_REGISTERS_PER_BLOCK = 12,</summary>
        RegistersPerBlock = 12,
        /// <summary>CU_DEVICE_ATTRIBUTE_CLOCK_RATE = 13,</summary>
        ClockRate = 13,
        /// <summary>CU_DEVICE_ATTRIBUTE_TEXTURE_ALIGNMENT = 14,</summary>
        TextureAlignment = 14,
        /// <summary>CU_DEVICE_ATTRIBUTE_GPU_OVERLAP = 15,</summary>
        GpuOverlap = 15,
        /// <summary>CU_DEVICE_ATTRIBUTE_MULTIPROCESSOR_COUNT = 16,</summary>
        MultiprocessorCount = 16,
        /// <summary>CU_DEVICE_ATTRIBUTE_KERNEL_EXEC_TIMEOUT = 17,</summary>
        KernelExecTimeout = 17,
        /// <summary>CU_DEVICE_ATTRIBUTE_INTEGRATED = 18,</summary>
        Integrated = 18,
        /// <summary>CU_DEVICE_ATTRIBUTE_CAN_MAP_HOST_MEMORY = 19,</summary>
        CanMapHostMemory = 19,
        /// <summary>CU_DEVICE_ATTRIBUTE_COMPUTE_MODE = 20,</summary>
        ComputeMode = 20,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE1D_WIDTH = 21,</summary>
        MaximumTexture1DWidth = 21,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_WIDTH = 22,</summary>
        MaximumTexture2DWidth = 22,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_HEIGHT = 23,</summary>
        MaximumTexture2DHeight = 23,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE3D_WIDTH = 24,</summary>
        MaximumTexture3DWidth = 24,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE3D_HEIGHT = 25,</summary>
        MaximumTexture3DHeight = 25,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE3D_DEPTH = 26,</summary>
        MaximumTexture3DDepth = 26,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_LAYERED_WIDTH = 27,</summary>
        MaximumTexture2DLayeredWidth = 27,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_LAYERED_HEIGHT = 28,</summary>
        MaximumTexture2DLayeredHeight = 28,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_LAYERED_LAYERS = 29,</summary>
        MaximumTexture2DLayeredLayers = 29,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_ARRAY_WIDTH = 27,</summary>
        MaximumTexture2DArrayWidth = 27,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_ARRAY_HEIGHT = 28,</summary>
        MaximumTexture2DArrayHeight = 28,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_ARRAY_NUMSLICES = 29,</summary>
        MaximumTexture2DArrayNumslices = 29,
        /// <summary>CU_DEVICE_ATTRIBUTE_SURFACE_ALIGNMENT = 30,</summary>
        SurfaceAlignment = 30,
        /// <summary>CU_DEVICE_ATTRIBUTE_CONCURRENT_KERNELS = 31,</summary>
        ConcurrentKernels = 31,
        /// <summary>CU_DEVICE_ATTRIBUTE_ECC_ENABLED = 32,</summary>
        EccEnabled = 32,
        /// <summary>CU_DEVICE_ATTRIBUTE_PCI_BUS_ID = 33,</summary>
        PciBusId = 33,
        /// <summary>CU_DEVICE_ATTRIBUTE_PCI_DEVICE_ID = 34,</summary>
        PciDeviceId = 34,
        /// <summary>CU_DEVICE_ATTRIBUTE_TCC_DRIVER = 35,</summary>
        TccDriver = 35,
        /// <summary>CU_DEVICE_ATTRIBUTE_MEMORY_CLOCK_RATE = 36,</summary>
        MemoryClockRate = 36,
        /// <summary>CU_DEVICE_ATTRIBUTE_GLOBAL_MEMORY_BUS_WIDTH = 37,</summary>
        GlobalMemoryBusWidth = 37,
        /// <summary>CU_DEVICE_ATTRIBUTE_L2_CACHE_SIZE = 38,</summary>
        L2CacheSize = 38,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAX_THREADS_PER_MULTIPROCESSOR = 39,</summary>
        MaxThreadsPerMultiprocessor = 39,
        /// <summary>CU_DEVICE_ATTRIBUTE_ASYNC_ENGINE_COUNT = 40,</summary>
        AsyncEngineCount = 40,
        /// <summary>CU_DEVICE_ATTRIBUTE_UNIFIED_ADDRESSING = 41,</summary>
        UnifiedAddressing = 41,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE1D_LAYERED_WIDTH = 42,</summary>
        MaximumTexture1DLayeredWidth = 42,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE1D_LAYERED_LAYERS = 43,</summary>
        MaximumTexture1DLayeredLayers = 43,
        /// <summary>CU_DEVICE_ATTRIBUTE_CAN_TEX2D_GATHER = 44,</summary>
        CanTex2DGather = 44,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_GATHER_WIDTH = 45,</summary>
        MaximumTexture2DGatherWidth = 45,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_GATHER_HEIGHT = 46,</summary>
        MaximumTexture2DGatherHeight = 46,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE3D_WIDTH_ALTERNATE = 47,</summary>
        MaximumTexture3DWidthAlternate = 47,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE3D_HEIGHT_ALTERNATE = 48,</summary>
        MaximumTexture3DHeightAlternate = 48,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE3D_DEPTH_ALTERNATE = 49,</summary>
        MaximumTexture3DDepthAlternate = 49,
        /// <summary>CU_DEVICE_ATTRIBUTE_PCI_DOMAIN_ID = 50,</summary>
        PciDomainId = 50,
        /// <summary>CU_DEVICE_ATTRIBUTE_TEXTURE_PITCH_ALIGNMENT = 51,</summary>
        TexturePitchAlignment = 51,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURECUBEMAP_WIDTH = 52,</summary>
        MaximumTexturecubemapWidth = 52,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURECUBEMAP_LAYERED_WIDTH = 53,</summary>
        MaximumTexturecubemapLayeredWidth = 53,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURECUBEMAP_LAYERED_LAYERS = 54,</summary>
        MaximumTexturecubemapLayeredLayers = 54,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_SURFACE1D_WIDTH = 55,</summary>
        MaximumSurface1DWidth = 55,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_SURFACE2D_WIDTH = 56,</summary>
        MaximumSurface2DWidth = 56,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_SURFACE2D_HEIGHT = 57,</summary>
        MaximumSurface2DHeight = 57,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_SURFACE3D_WIDTH = 58,</summary>
        MaximumSurface3DWidth = 58,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_SURFACE3D_HEIGHT = 59,</summary>
        MaximumSurface3DHeight = 59,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_SURFACE3D_DEPTH = 60,</summary>
        MaximumSurface3DDepth = 60,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_SURFACE1D_LAYERED_WIDTH = 61,</summary>
        MaximumSurface1DLayeredWidth = 61,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_SURFACE1D_LAYERED_LAYERS = 62,</summary>
        MaximumSurface1DLayeredLayers = 62,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_SURFACE2D_LAYERED_WIDTH = 63,</summary>
        MaximumSurface2DLayeredWidth = 63,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_SURFACE2D_LAYERED_HEIGHT = 64,</summary>
        MaximumSurface2DLayeredHeight = 64,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_SURFACE2D_LAYERED_LAYERS = 65,</summary>
        MaximumSurface2DLayeredLayers = 65,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_SURFACECUBEMAP_WIDTH = 66,</summary>
        MaximumSurfacecubemapWidth = 66,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_SURFACECUBEMAP_LAYERED_WIDTH = 67,</summary>
        MaximumSurfacecubemapLayeredWidth = 67,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_SURFACECUBEMAP_LAYERED_LAYERS = 68,</summary>
        MaximumSurfacecubemapLayeredLayers = 68,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE1D_LINEAR_WIDTH = 69,</summary>
        MaximumTexture1DLinearWidth = 69,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_LINEAR_WIDTH = 70,</summary>
        MaximumTexture2DLinearWidth = 70,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_LINEAR_HEIGHT = 71,</summary>
        MaximumTexture2DLinearHeight = 71,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_LINEAR_PITCH = 72,</summary>
        MaximumTexture2DLinearPitch = 72,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_MIPMAPPED_WIDTH = 73,</summary>
        MaximumTexture2DMipmappedWidth = 73,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_MIPMAPPED_HEIGHT = 74,</summary>
        MaximumTexture2DMipmappedHeight = 74,
        /// <summary>CU_DEVICE_ATTRIBUTE_COMPUTE_CAPABILITY_MAJOR = 75,</summary>
        ComputeCapabilityMajor = 75,
        /// <summary>CU_DEVICE_ATTRIBUTE_COMPUTE_CAPABILITY_MINOR = 76,</summary>
        ComputeCapabilityMinor = 76,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE1D_MIPMAPPED_WIDTH = 77,</summary>
        MaximumTexture1DMipmappedWidth = 77,
        /// <summary>CU_DEVICE_ATTRIBUTE_STREAM_PRIORITIES_SUPPORTED = 78,</summary>
        StreamPrioritiesSupported = 78,
        /// <summary>CU_DEVICE_ATTRIBUTE_GLOBAL_L1_CACHE_SUPPORTED = 79,</summary>
        GlobalL1CacheSupported = 79,
        /// <summary>CU_DEVICE_ATTRIBUTE_LOCAL_L1_CACHE_SUPPORTED = 80,</summary>
        LocalL1CacheSupported = 80,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAX_SHARED_MEMORY_PER_MULTIPROCESSOR = 81,</summary>
        MaxSharedMemoryPerMultiprocessor = 81,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAX_REGISTERS_PER_MULTIPROCESSOR = 82,</summary>
        MaxRegistersPerMultiprocessor = 82,
        /// <summary>CU_DEVICE_ATTRIBUTE_MANAGED_MEMORY = 83,</summary>
        ManagedMemory = 83,
        /// <summary>CU_DEVICE_ATTRIBUTE_MULTI_GPU_BOARD = 84,</summary>
        MultiGpuBoard = 84,
        /// <summary>CU_DEVICE_ATTRIBUTE_MULTI_GPU_BOARD_GROUP_ID = 85,</summary>
        MultiGpuBoardGroupId = 85,
        /// <summary>CU_DEVICE_ATTRIBUTE_HOST_NATIVE_ATOMIC_SUPPORTED = 86,</summary>
        HostNativeAtomicSupported = 86,
        /// <summary>CU_DEVICE_ATTRIBUTE_SINGLE_TO_DOUBLE_PRECISION_PERF_RATIO = 87,</summary>
        SingleToDoublePrecisionPerfRatio = 87,
        /// <summary>CU_DEVICE_ATTRIBUTE_PAGEABLE_MEMORY_ACCESS = 88,</summary>
        PageableMemoryAccess = 88,
        /// <summary>CU_DEVICE_ATTRIBUTE_CONCURRENT_MANAGED_ACCESS = 89,</summary>
        ConcurrentManagedAccess = 89,
        /// <summary>CU_DEVICE_ATTRIBUTE_COMPUTE_PREEMPTION_SUPPORTED = 90,</summary>
        ComputePreemptionSupported = 90,
        /// <summary>CU_DEVICE_ATTRIBUTE_CAN_USE_HOST_POINTER_FOR_REGISTERED_MEM = 91,</summary>
        CanUseHostPointerForRegisteredMem = 91,
        /// <summary>CU_DEVICE_ATTRIBUTE_CAN_USE_STREAM_MEM_OPS = 92,</summary>
        CanUseStreamMemOps = 92,
        /// <summary>CU_DEVICE_ATTRIBUTE_CAN_USE_64_BIT_STREAM_MEM_OPS = 93,</summary>
        CanUse64BitStreamMemOps = 93,
        /// <summary>CU_DEVICE_ATTRIBUTE_CAN_USE_STREAM_WAIT_VALUE_NOR = 94,</summary>
        CanUseStreamWaitValueNor = 94,
        /// <summary>CU_DEVICE_ATTRIBUTE_COOPERATIVE_LAUNCH = 95,</summary>
        CooperativeLaunch = 95,
        /// <summary>CU_DEVICE_ATTRIBUTE_COOPERATIVE_MULTI_DEVICE_LAUNCH = 96,</summary>
        CooperativeMultiDeviceLaunch = 96,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAX_SHARED_MEMORY_PER_BLOCK_OPTIN = 97</summary>
        MaxSharedMemoryPerBlockOptin = 97
    }

    /// <summary>Memory types</summary>
    public enum CuMemoryType
    {
        /// <summary>Host memory</summary>
        Host = 0x01,
        /// <summary>Device memory</summary>
        Device = 0x02,
        /// <summary>Array memory</summary>
        Array = 0x03,
        /// <summary>Unified device or host memory</summary>
        Unified = 0x04
    }

    /// <summary>2D memory copy parameters</summary>
    public struct CudaMemcopy2D
    {
        /// <summary>Source X in bytes</summary>
        public IntPtr SrcXInBytes;
        /// <summary>Source Y</summary>
        public IntPtr SrcY;

        /// <summary>Source memory type (host, device, array)</summary>
        public CuMemoryType SrcMemoryType;
        /// <summary>Source host pointer</summary>
        public IntPtr SrcHost;
        /// <summary>Source device pointer</summary>
        public CuDevicePtr SrcDevice;
        /// <summary>Source array reference</summary>
        public CuArray SrcArray;
        /// <summary>Source pitch (ignored when src is array)</summary>
        public IntPtr SrcPitch;

        /// <summary>Destination X in bytes</summary>
        public IntPtr DstXInBytes;
        /// <summary>Destination Y</summary>
        public IntPtr DstY;

        /// <summary>Destination memory type (host, device, array)</summary>
        public CuMemoryType DstMemoryType;
        /// <summary>Destination host pointer</summary>
        public IntPtr DstHost;
        /// <summary>Destination device pointer</summary>
        public CuDevicePtr DstDevice;
        /// <summary>Destination array reference</summary>
        public CuArray DstArray;
        /// <summary>Destination pitch (ignored when dst is array)</summary>
        public IntPtr DstPitch;

        /// <summary>Width of 2D memory copy in bytes</summary>
        public IntPtr WidthInBytes;
        /// <summary>Height of 2D memory copy</summary>
        public IntPtr Height;
    }

    /// <summary>Occupancy calculator flag</summary>
    [Flags]
    public enum CuOccupancyFlags
    {
        /// <summary>Default behavior</summary>
        Default = 0x0,
        /// <summary>Assume global caching is enabled and cannot be automatically turned off</summary>
        DisableCachingOverride = 0x1
    }

    /// <summary>Array formats</summary>
    public enum CuArrayFormat
    {
        /// <summary>Unsigned 8-bit integers</summary>
        UnsignedInt8 = 0x01,
        /// <summary>Unsigned 16-bit integers</summary>
        UnsignedInt16 = 0x02,
        /// <summary>Unsigned 32-bit integers</summary>
        UnsignedInt32 = 0x03,
        /// <summary>Signed 8-bit integers</summary>
        SignedInt8 = 0x08,
        /// <summary>Signed 16-bit integers</summary>
        SignedInt16 = 0x09,
        /// <summary>Signed 32-bit integers</summary>
        SignedInt32 = 0x0a,
        /// <summary>16-bit floating point</summary>
        Half = 0x10,
        /// <summary>32-bit floating point</summary>
        Float = 0x20
    }

    /// <summary>Texture reference addressing modes</summary>
    public enum CuAddressMode
    {
        /// <summary>Wrapping address mode</summary>
        Wrap = 0,
        /// <summary>Clamp to edge address mode</summary>
        Clamp = 1,
        /// <summary>Mirror address mode</summary>
        Mirror = 2,
        /// <summary>Border address mode</summary>
        Border = 3
    }

    /// <summary>Texture reference filtering modes</summary>
    public enum CuFilterMode
    {
        /// <summary>Point filter mode</summary>
        Point = 0,
        /// <summary>Linear filter mode</summary>
        Linear = 1
    }

    public struct CudaMemcpy3D
    {
        /// <summary>Source X in bytes</summary>
        public uint SrcXInBytes;
        /// <summary>Source Y</summary>
        public uint SrcY;
        /// <summary>Source Z</summary>
        public uint SrcZ;
        /// <summary>Source LOD</summary>
        public uint SrcLod;
        /// <summary>Source memory type (host, device, array)</summary>
        public CuMemoryType SrcMemoryType;
        /// <summary>Source host pointer</summary>
        public IntPtr SrcHost;
        /// <summary>Source device pointer</summary>
        public CuDevicePtr SrcDevice;
        /// <summary>Source array reference</summary>
        public CuArray SrcArray;
        /// <summary>Must be NULL</summary>
        private IntPtr _reserved0;
        /// <summary>Source pitch (ignored when src is array)</summary>
        public uint SrcPitch;
        /// <summary>Source height (ignored when src is array; may be 0 if Depth==1)</summary>
        public uint SrcHeight;

        /// <summary>Destination X in bytes</summary>
        public uint DstXInBytes;
        /// <summary>Destination Y</summary>
        public uint DstY;
        /// <summary>Destination Z</summary>
        public uint DstZ;
        /// <summary>Destination LOD</summary>
        public uint DstLod;
        /// <summary>Destination memory type (host, device, array)</summary>
        public CuMemoryType DstMemoryType;
        /// <summary>Destination host pointer</summary>
        public IntPtr DstHost;
        /// <summary>Destination device pointer</summary>
        public CuDevicePtr DstDevice;
        /// <summary>Destination array reference</summary>
        public CuArray DstArray;
        /// <summary>Must be NULL</summary>
        public IntPtr Reserved1;
        /// <summary>Destination pitch (ignored when dst is array)</summary>
        public uint DstPitch;
        /// <summary>Destination height (ignored when dst is array; may be 0 if Depth==1)</summary>
        public uint DstHeight;

        /// <summary>Width of 3D memory copy in bytes</summary>
        public uint WidthInBytes;
        /// <summary>Height of 3D memory copy</summary>
        public uint Height;
        /// <summary>Depth of 3D memory copy</summary>
        public uint Depth;
    }

    public struct CudaMemcpy3DPeer
    {
        /// <summary>Source X in bytes</summary>
        public IntPtr SrcXInBytes;
        /// <summary>Source Y</summary>
        public IntPtr SrcY;
        /// <summary>Source Z</summary>
        public IntPtr SrcZ;
        /// <summary>Source LOD</summary>
        public IntPtr SrcLod;
        /// <summary>Source memory type (host, device, array)</summary>
        public CuMemoryType SrcMemoryType;
        /// <summary>Source host pointer</summary>
        public IntPtr SrcHost;
        /// <summary>Source device pointer</summary>
        public CuDevicePtr SrcDevice;
        /// <summary>Source array reference</summary>
        public CuArray SrcArray;
        /// <summary>Source context (ignored with srcMemoryType is ::CU_MEMORYTYPE_ARRAY)</summary>
        public CuContext SrcContext;
        /// <summary>Source pitch (ignored when src is array)</summary>
        public IntPtr SrcPitch;
        /// <summary>Source height (ignored when src is array; may be 0 if Depth==1)</summary>
        public IntPtr SrcHeight;

        /// <summary>Destination X in bytes</summary>
        public IntPtr DstXInBytes;
        /// <summary>Destination Y</summary>
        public IntPtr DstY;
        /// <summary>Destination Z</summary>
        public IntPtr DstZ;
        /// <summary>Destination LOD</summary>
        public IntPtr DstLod;
        /// <summary>Destination memory type (host, device, array)</summary>
        public CuMemoryType DstMemoryType;
        /// <summary>Destination host pointer</summary>
        public IntPtr DstHost;
        /// <summary>Destination device pointer</summary>
        public CuDevicePtr DstDevice;
        /// <summary>Destination array reference</summary>
        public CuArray DstArray;
        /// <summary>Destination context (ignored with dstMemoryType is ::CU_MEMORYTYPE_ARRAY)</summary>
        public CuContext DstContext;
        /// <summary>Destination pitch (ignored when dst is array)</summary>
        public IntPtr DstPitch;
        /// <summary>Destination height (ignored when dst is array; may be 0 if Depth==1)</summary>
        public IntPtr DstHeight;

        /// <summary>Width of 3D memory copy in bytes</summary>
        public IntPtr WidthInBytes;
        /// <summary>Height of 3D memory copy</summary>
        public IntPtr Height;
        /// <summary>Depth of 3D memory copy</summary>
        public IntPtr Depth;
    }

    public struct CudaArrayDescription
    {
        /// <summary>Width of array</summary>
        public uint Width;
        /// <summary>Height of array</summary>
        public uint Height;

        /// <summary>Array format</summary>
        public CuArrayFormat Format;
        /// <summary>Channels per array element</summary>
        public uint NumChannels;
    }

    public struct CudaArray3DDescription
    {
        /// <summary>Width of 3D array</summary>
        public uint Width;
        /// <summary>Height of 3D array</summary>
        public uint Height;
        /// <summary>Depth of 3D array</summary>
        public uint Depth;

        /// <summary>Array format</summary>
        public CuArrayFormat Format;
        /// <summary>Channels per array element</summary>
        public uint NumChannels;
        /// <summary>Flags</summary>
        public uint Flags;
    }

    /// <summary>Stream creation flags</summary>
    [Flags]
    public enum CuStreamFlags
    {
        /// <summary>Default stream flag</summary>
        Default = 0x0,
        /// <summary>Stream does not synchronize with stream 0 (the NULL stream)</summary>
        NonBlocking = 0x1
    }

    /// <summary>Event creation flags</summary>
    [Flags]
    public enum CuEventFlags
    {
        /// <summary>Default event flag</summary>
        Default = 0x0,
        /// <summary>Event uses blocking synchronization</summary>
        BlockingSync = 0x1,
        /// <summary>Event will not record timing data</summary>
        DisableTiming = 0x2,
        /// <summary>Event is suitable for interprocess use. CU_EVENT_DISABLE_TIMING must be set</summary>
        Interprocess = 0x4
    }

    /// <summary>CUDA Ipc Mem Flags</summary>
    [Flags]
    public enum CuIpcMemFlags
    {
        /// <summary>Automatically enable peer access between remote devices as needed</summary>
        LazyEnablePeerAccess = 0x1
    }

    /// <summary>CUDA Mem Attach Flags</summary>
    [Flags]
    public enum CuMemAttachFlags
    {
        /// <summary>Memory can be accessed by any stream on any device</summary>
        Global = 0x1,
        /// <summary>Memory cannot be accessed by any stream on any device</summary>
        Host = 0x2,
        /// <summary>Memory can only be accessed by a single stream on the associated device</summary>
        Single = 0x4
    }
}