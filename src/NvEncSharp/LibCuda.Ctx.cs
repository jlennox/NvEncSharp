using System;
using System.Runtime.InteropServices;

// ReSharper disable UnusedMember.Global

namespace Lennox.NvEncSharp
{
    public static partial class LibCuda
    {
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
        /// <param name="pctx"></param>
        /// <param name="dev">Device for which the primary context flags are set</param>
        /// <param name="flags">New flags for the device</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_DEVICE,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_PRIMARY_CONTEXT_ACTIVE
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuDevicePrimaryCtxRetain,
        /// ::cuDevicePrimaryCtxGetState,
        /// ::cuCtxCreate,
        /// ::cuCtxGetFlags,
        /// ::cudaSetDeviceFlags
        /// CUresult CUDAAPI cuCtxCreate(CUcontext *pctx, unsigned int flags, CUdevice dev);
        [DllImport(_dllpath, EntryPoint = "cuCtxCreate" + _ver)]
        public static extern CuResult CtxCreate(out CuContext pctx, CuContextFlags flags, CuDevice dev);

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
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
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
        [DllImport(_dllpath, EntryPoint = "cuCtxDestroy" + _ver)]
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
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
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
        [DllImport(_dllpath, EntryPoint = "cuCtxPushCurrent" + _ver)]
        public static extern CuResult CtxPushCurrent(CuContext ctx);

        /// <summary>Pops the current CUDA context from the current CPU thread.
        ///
        /// Pops the current CUDA context from the CPU thread and passes back the
        /// old context handle in *<paramref name="pctx"/>. That context may then be made current
        /// to a different CPU thread by calling ::cuCtxPushCurrent().
        ///
        /// If a context was current to the CPU thread before ::cuCtxCreate() or
        /// ::cuCtxPushCurrent() was called, this function makes that context current to
        /// the CPU thread again.</summary>
        ///
        /// <param name="pctx">Returned new context handle</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT
        /// </returns>
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
        [DllImport(_dllpath, EntryPoint = "cuCtxPopCurrent" + _ver)]
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
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT
        /// </returns>
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
        /// Returns in *<paramref name="pctx"/> the CUDA context bound to the calling CPU thread.
        /// If no context is bound to the calling CPU thread then *<paramref name="pctx"/> is
        /// set to NULL and ::CUDA_SUCCESS is returned.</summary>
        ///
        /// <param name="pctx">Returned context handle</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// </returns>
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

        /// <summary>Set resource limits
        ///
        /// Setting <paramref name="limit"/> to <paramref name="value"/> is a request by the application to update
        /// the current limit maintained by the context. The driver is free to
        /// modify the requested value to meet h/w requirements (this could be
        /// clamping to minimum or maximum values, rounding up to nearest element
        /// size, etc). The application can use ::cuCtxGetLimit() to find out exactly
        /// what the limit has been set to.
        ///
        /// Setting each ::CUlimit has its own specific restrictions, so each is
        /// discussed here.
        ///
        /// - ::CU_LIMIT_STACK_SIZE controls the stack size in bytes of each GPU thread.
        ///
        /// - ::CU_LIMIT_PRINTF_FIFO_SIZE controls the size in bytes of the FIFO used
        ///   by the ::printf() device system call. Setting ::CU_LIMIT_PRINTF_FIFO_SIZE
        ///   must be performed before launching any kernel that uses the ::printf()
        ///   device system call, otherwise ::CUDA_ERROR_INVALID_VALUE will be returned.
        ///
        /// - ::CU_LIMIT_MALLOC_HEAP_SIZE controls the size in bytes of the heap used
        ///   by the ::malloc() and ::free() device system calls. Setting
        ///   ::CU_LIMIT_MALLOC_HEAP_SIZE must be performed before launching any kernel
        ///   that uses the ::malloc() or ::free() device system calls, otherwise
        ///   ::CUDA_ERROR_INVALID_VALUE will be returned.
        ///
        /// - ::CU_LIMIT_DEV_RUNTIME_SYNC_DEPTH controls the maximum nesting depth of
        ///   a grid at which a thread can safely call ::cudaDeviceSynchronize(). Setting
        ///   this limit must be performed before any launch of a kernel that uses the
        ///   device runtime and calls ::cudaDeviceSynchronize() above the default sync
        ///   depth, two levels of grids. Calls to ::cudaDeviceSynchronize() will fail
        ///   with error code ::cudaErrorSyncDepthExceeded if the limitation is
        ///   violated. This limit can be set smaller than the default or up the maximum
        ///   launch depth of 24. When setting this limit, keep in mind that additional
        ///   levels of sync depth require the driver to reserve large amounts of device
        ///   memory which can no longer be used for user allocations. If these
        ///   reservations of device memory fail, ::cuCtxSetLimit will return
        ///   ::CUDA_ERROR_OUT_OF_MEMORY, and the limit can be reset to a lower value.
        ///   This limit is only applicable to devices of compute capability 3.5 and
        ///   higher. Attempting to set this limit on devices of compute capability less
        ///   than 3.5 will result in the error ::CUDA_ERROR_UNSUPPORTED_LIMIT being
        ///   returned.
        ///
        /// - ::CU_LIMIT_DEV_RUNTIME_PENDING_LAUNCH_COUNT controls the maximum number of
        ///   outstanding device runtime launches that can be made from the current
        ///   context. A grid is outstanding from the point of launch up until the grid
        ///   is known to have been completed. Device runtime launches which violate
        ///   this limitation fail and return ::cudaErrorLaunchPendingCountExceeded when
        ///   ::cudaGetLastError() is called after launch. If more pending launches than
        ///   the default (2048 launches) are needed for a module using the device
        ///   runtime, this limit can be increased. Keep in mind that being able to
        ///   sustain additional pending launches will require the driver to reserve
        ///   larger amounts of device memory upfront which can no longer be used for
        ///   allocations. If these reservations fail, ::cuCtxSetLimit will return
        ///   ::CUDA_ERROR_OUT_OF_MEMORY, and the limit can be reset to a lower value.
        ///   This limit is only applicable to devices of compute capability 3.5 and
        ///   higher. Attempting to set this limit on devices of compute capability less
        ///   than 3.5 will result in the error ::CUDA_ERROR_UNSUPPORTED_LIMIT being
        ///   returned.</summary>
        ///
        /// <param name="limit">Limit to set</param>
        /// <param name="value">Size of limit</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_UNSUPPORTED_LIMIT,
        /// ::CUDA_ERROR_OUT_OF_MEMORY
        /// </returns>
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
        /// ::cuCtxPushCurrent,
        /// ::cuCtxSetCacheConfig,
        /// ::cuCtxSynchronize,
        /// ::cudaDeviceSetLimit
        /// CUresult CUDAAPI cuCtxSetLimit(CUlimit limit, size_t value);
        [DllImport(_dllpath, EntryPoint = "cuCtxSetLimit")]
        public static extern CuResult CtxSetLimit(CuLimit limit, IntPtr value);

        /// <summary>Returns resource limits
        ///
        /// Returns in *<paramref name="pvalue"/> the current size of <paramref name="limit"/>.  The supported
        /// ::CUlimit values are:
        /// - ::CU_LIMIT_STACK_SIZE: stack size in bytes of each GPU thread.
        /// - ::CU_LIMIT_PRINTF_FIFO_SIZE: size in bytes of the FIFO used by the
        ///   ::printf() device system call.
        /// - ::CU_LIMIT_MALLOC_HEAP_SIZE: size in bytes of the heap used by the
        ///   ::malloc() and ::free() device system calls.
        /// - ::CU_LIMIT_DEV_RUNTIME_SYNC_DEPTH: maximum grid depth at which a thread
        ///   can issue the device runtime call ::cudaDeviceSynchronize() to wait on
        ///   child grid launches to complete.
        /// - ::CU_LIMIT_DEV_RUNTIME_PENDING_LAUNCH_COUNT: maximum number of outstanding
        ///   device runtime launches that can be made from this context.</summary>
        ///
        /// <param name="limit">Limit to query</param>
        /// <param name="pvalue">Returned size of limit</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_UNSUPPORTED_LIMIT
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuCtxCreate,
        /// ::cuCtxDestroy,
        /// ::cuCtxGetApiVersion,
        /// ::cuCtxGetCacheConfig,
        /// ::cuCtxGetDevice,
        /// ::cuCtxGetFlags,
        /// ::cuCtxPopCurrent,
        /// ::cuCtxPushCurrent,
        /// ::cuCtxSetCacheConfig,
        /// ::cuCtxSetLimit,
        /// ::cuCtxSynchronize,
        /// ::cudaDeviceGetLimit
        /// CUresult CUDAAPI cuCtxGetLimit(size_t *pvalue, CUlimit limit);
        [DllImport(_dllpath, EntryPoint = "cuCtxGetLimit")]
        public static extern CuResult CtxGetLimit(out IntPtr pvalue, CuLimit limit);

        /// <summary>Returns the preferred cache configuration for the current context.
        ///
        /// On devices where the L1 cache and shared memory use the same hardware
        /// resources, this function returns through <paramref name="pconfig"/> the preferred cache configuration
        /// for the current context. This is only a preference. The driver will use
        /// the requested configuration if possible, but it is free to choose a different
        /// configuration if required to execute functions.
        ///
        /// This will return a <paramref name="pconfig"/> of ::CU_FUNC_CACHE_PREFER_NONE on devices
        /// where the size of the L1 cache and shared memory are fixed.
        ///
        /// The supported cache configurations are:
        /// - ::CU_FUNC_CACHE_PREFER_NONE: no preference for shared memory or L1 (default)
        /// - ::CU_FUNC_CACHE_PREFER_SHARED: prefer larger shared memory and smaller L1 cache
        /// - ::CU_FUNC_CACHE_PREFER_L1: prefer larger L1 cache and smaller shared memory
        /// - ::CU_FUNC_CACHE_PREFER_EQUAL: prefer equal sized L1 cache and shared memory</summary>
        ///
        /// <param name="pconfig">Returned cache configuration</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuCtxCreate,
        /// ::cuCtxDestroy,
        /// ::cuCtxGetApiVersion,
        /// ::cuCtxGetDevice,
        /// ::cuCtxGetFlags,
        /// ::cuCtxGetLimit,
        /// ::cuCtxPopCurrent,
        /// ::cuCtxPushCurrent,
        /// ::cuCtxSetCacheConfig,
        /// ::cuCtxSetLimit,
        /// ::cuCtxSynchronize,
        /// ::cuFuncSetCacheConfig,
        /// ::cudaDeviceGetCacheConfig
        /// CUresult CUDAAPI cuCtxGetCacheConfig(CUfunc_cache *pconfig);
        [DllImport(_dllpath, EntryPoint = "cuCtxGetCacheConfig")]
        public static extern CuResult CtxGetCacheConfig(out CuFunctionCache pconfig);

        /// <summary>Sets the preferred cache configuration for the current context.
        ///
        /// On devices where the L1 cache and shared memory use the same hardware
        /// resources, this sets through <paramref name="config"/> the preferred cache configuration for
        /// the current context. This is only a preference. The driver will use
        /// the requested configuration if possible, but it is free to choose a different
        /// configuration if required to execute the function. Any function preference
        /// set via ::cuFuncSetCacheConfig() will be preferred over this context-wide
        /// setting. Setting the context-wide cache configuration to
        /// ::CU_FUNC_CACHE_PREFER_NONE will cause subsequent kernel launches to prefer
        /// to not change the cache configuration unless required to launch the kernel.
        ///
        /// This setting does nothing on devices where the size of the L1 cache and
        /// shared memory are fixed.
        ///
        /// Launching a kernel with a different preference than the most recent
        /// preference setting may insert a device-side synchronization point.
        ///
        /// The supported cache configurations are:
        /// - ::CU_FUNC_CACHE_PREFER_NONE: no preference for shared memory or L1 (default)
        /// - ::CU_FUNC_CACHE_PREFER_SHARED: prefer larger shared memory and smaller L1 cache
        /// - ::CU_FUNC_CACHE_PREFER_L1: prefer larger L1 cache and smaller shared memory
        /// - ::CU_FUNC_CACHE_PREFER_EQUAL: prefer equal sized L1 cache and shared memory</summary>
        ///
        /// <param name="config">Requested cache configuration</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
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
        /// ::cuCtxPushCurrent,
        /// ::cuCtxSetLimit,
        /// ::cuCtxSynchronize,
        /// ::cuFuncSetCacheConfig,
        /// ::cudaDeviceSetCacheConfig
        /// CUresult CUDAAPI cuCtxSetCacheConfig(CUfunc_cache config);
        [DllImport(_dllpath, EntryPoint = "cuCtxSetCacheConfig")]
        public static extern CuResult CtxSetCacheConfig(CuFunctionCache config);

        /// <summary>Returns the current shared memory configuration for the current context.
        ///
        /// This function will return in <paramref name="pConfig"/> the current size of shared memory banks
        /// in the current context. On devices with configurable shared memory banks,
        /// ::cuCtxSetSharedMemConfig can be used to change this setting, so that all
        /// subsequent kernel launches will by default use the new bank size. When
        /// ::cuCtxGetSharedMemConfig is called on devices without configurable shared
        /// memory, it will return the fixed bank size of the hardware.
        ///
        /// The returned bank configurations can be either:
        /// - ::CU_SHARED_MEM_CONFIG_FOUR_BYTE_BANK_SIZE:  shared memory bank width is
        ///   four bytes.
        /// - ::CU_SHARED_MEM_CONFIG_EIGHT_BYTE_BANK_SIZE: shared memory bank width will
        ///   eight bytes.</summary>
        ///
        /// <param name="pConfig">returned shared memory configuration</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
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
        /// ::cuCtxPushCurrent,
        /// ::cuCtxSetLimit,
        /// ::cuCtxSynchronize,
        /// ::cuCtxGetSharedMemConfig,
        /// ::cuFuncSetCacheConfig,
        /// ::cudaDeviceGetSharedMemConfig
        /// CUresult CUDAAPI cuCtxGetSharedMemConfig(CUsharedconfig *pConfig);
        [DllImport(_dllpath, EntryPoint = "cuCtxGetSharedMemConfig")]
        public static extern CuResult CtxGetSharedMemConfig(out SharedMemoryConfig pConfig);

        /// <summary>Sets the shared memory configuration for the current context.
        ///
        /// On devices with configurable shared memory banks, this function will set
        /// the context's shared memory bank size which is used for subsequent kernel
        /// launches.
        ///
        /// Changed the shared memory configuration between launches may insert a device
        /// side synchronization point between those launches.
        ///
        /// Changing the shared memory bank size will not increase shared memory usage
        /// or affect occupancy of kernels, but may have major effects on performance.
        /// Larger bank sizes will allow for greater potential bandwidth to shared memory,
        /// but will change what kinds of accesses to shared memory will result in bank
        /// conflicts.
        ///
        /// This function will do nothing on devices with fixed shared memory bank size.
        ///
        /// The supported bank configurations are:
        /// - ::CU_SHARED_MEM_CONFIG_DEFAULT_BANK_SIZE: set bank width to the default initial
        ///   setting (currently, four bytes).
        /// - ::CU_SHARED_MEM_CONFIG_FOUR_BYTE_BANK_SIZE: set shared memory bank width to
        ///   be natively four bytes.
        /// - ::CU_SHARED_MEM_CONFIG_EIGHT_BYTE_BANK_SIZE: set shared memory bank width to
        ///   be natively eight bytes.</summary>
        ///
        /// <param name="config">requested shared memory configuration</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
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
        /// ::cuCtxPushCurrent,
        /// ::cuCtxSetLimit,
        /// ::cuCtxSynchronize,
        /// ::cuCtxGetSharedMemConfig,
        /// ::cuFuncSetCacheConfig,
        /// ::cudaDeviceSetSharedMemConfig
        /// CUresult CUDAAPI cuCtxSetSharedMemConfig(CUsharedconfig config);
        [DllImport(_dllpath, EntryPoint = "cuCtxSetSharedMemConfig")]
        public static extern CuResult CtxSetSharedMemConfig(SharedMemoryConfig config);

        /// <summary>Gets the context's API version.
        ///
        /// Returns a version number in <paramref name="version"/> corresponding to the capabilities of
        /// the context (e.g. 3010 or 3020), which library developers can use to direct
        /// callers to a specific API version. If <paramref name="ctx"/> is NULL, returns the API version
        /// used to create the currently bound context.
        ///
        /// Note that new API versions are only introduced when context capabilities are
        /// changed that break binary compatibility, so the API version and driver version
        /// may be different. For example, it is valid for the API version to be 3020 while
        /// the driver version is 4020.</summary>
        ///
        /// <param name="ctx">Context to check</param>
        /// <param name="version">Pointer to version</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_UNKNOWN
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuCtxCreate,
        /// ::cuCtxDestroy,
        /// ::cuCtxGetDevice,
        /// ::cuCtxGetFlags,
        /// ::cuCtxGetLimit,
        /// ::cuCtxPopCurrent,
        /// ::cuCtxPushCurrent,
        /// ::cuCtxSetCacheConfig,
        /// ::cuCtxSetLimit,
        /// ::cuCtxSynchronize
        /// CUresult CUDAAPI cuCtxGetApiVersion(CUcontext ctx, unsigned int *version);
        [DllImport(_dllpath, EntryPoint = "cuCtxGetApiVersion")]
        public static extern CuResult CtxGetApiVersion(CuContext ctx, out uint version);

        /// <summary>Returns numerical values that correspond to the least and
        /// greatest stream priorities.
        ///
        /// Returns in *<paramref name="leastPriority"/> and *<paramref name="greatestPriority"/> the numerical values that correspond
        /// to the least and greatest stream priorities respectively. Stream priorities
        /// follow a convention where lower numbers imply greater priorities. The range of
        /// meaningful stream priorities is given by [*<paramref name="greatestPriority"/>, *<paramref name="leastPriority"/>].
        /// If the user attempts to create a stream with a priority value that is
        /// outside the meaningful range as specified by this API, the priority is
        /// automatically clamped down or up to either *<paramref name="leastPriority"/> or *<paramref name="greatestPriority"/>
        /// respectively. See ::cuStreamCreateWithPriority for details on creating a
        /// priority stream.
        /// A NULL may be passed in for *<paramref name="leastPriority"/> or *<paramref name="greatestPriority"/> if the value
        /// is not desired.
        ///
        /// This function will return '0' in both *<paramref name="leastPriority"/> and *<paramref name="greatestPriority"/> if
        /// the current context's device does not support stream priorities
        /// (see ::cuDeviceGetAttribute).</summary>
        ///
        /// <param name="leastPriority">Pointer to an int in which the numerical value for least
        /// stream priority is returned</param>
        /// <param name="greatestPriority">Pointer to an int in which the numerical value for greatest
        /// stream priority is returned</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuStreamCreateWithPriority,
        /// ::cuStreamGetPriority,
        /// ::cuCtxGetDevice,
        /// ::cuCtxGetFlags,
        /// ::cuCtxSetLimit,
        /// ::cuCtxSynchronize,
        /// ::cudaDeviceGetStreamPriorityRange
        /// CUresult CUDAAPI cuCtxGetStreamPriorityRange(int *leastPriority, int *greatestPriority);
        [DllImport(_dllpath, EntryPoint = "cuCtxGetStreamPriorityRange")]
        public static extern CuResult CtxGetStreamPriorityRange(out int leastPriority, out int greatestPriority);

        /// <summary>Returns the device ID for the current context
        ///
        /// Returns in *<paramref name="device"/> the ordinal of the current context's device.</summary>
        ///
        /// <param name="device">Returned device ID for the current context</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuCtxCreate,
        /// ::cuCtxDestroy,
        /// ::cuCtxGetApiVersion,
        /// ::cuCtxGetCacheConfig,
        /// ::cuCtxGetFlags,
        /// ::cuCtxGetLimit,
        /// ::cuCtxPopCurrent,
        /// ::cuCtxPushCurrent,
        /// ::cuCtxSetCacheConfig,
        /// ::cuCtxSetLimit,
        /// ::cuCtxSynchronize,
        /// ::cudaGetDevice
        /// CUresult CUDAAPI cuCtxGetDevice(CUdevice *device);
        [DllImport(_dllpath, EntryPoint = "cuCtxGetDevice")]
        public static extern CuResult CtxGetDevice(out CuDevice device);

        /// <summary>Returns the flags for the current context
        ///
        /// Returns in *<paramref name="flags"/> the flags of the current context. See ::cuCtxCreate
        /// for flag values.</summary>
        ///
        /// <param name="flags">Pointer to store flags of current context</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuCtxCreate,
        /// ::cuCtxGetApiVersion,
        /// ::cuCtxGetCacheConfig,
        /// ::cuCtxGetCurrent,
        /// ::cuCtxGetDevice
        /// ::cuCtxGetLimit,
        /// ::cuCtxGetSharedMemConfig,
        /// ::cuCtxGetStreamPriorityRange,
        /// ::cudaGetDeviceFlags
        /// CUresult CUDAAPI cuCtxGetFlags(unsigned int *flags);
        [DllImport(_dllpath, EntryPoint = "cuCtxGetFlags")]
        public static extern CuResult CtxGetFlags(out CuContextFlags flags);

        /// <summary>Block for a context's tasks to complete
        ///
        /// Blocks until the device has completed all preceding requested tasks.
        /// ::cuCtxSynchronize() returns an error if one of the preceding tasks failed.
        /// If the context was created with the ::CU_CTX_SCHED_BLOCKING_SYNC flag, the
        /// CPU thread will block until the GPU context has finished its work.</summary>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT
        /// </returns>
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
        /// ::cuCtxPushCurrent,
        /// ::cuCtxSetCacheConfig,
        /// ::cuCtxSetLimit,
        /// ::cudaDeviceSynchronize
        /// CUresult CUDAAPI cuCtxSynchronize(void);
        [DllImport(_dllpath, EntryPoint = "cuCtxSynchronize")]
        public static extern CuResult CtxSynchronize();

        /// <summary>Increment a context's usage-count
        ///
        /// \deprecated
        ///
        /// Note that this function is deprecated and should not be used.
        ///
        /// Increments the usage count of the context and passes back a context handle
        /// in *<paramref name="pctx"/> that must be passed to ::cuCtxDetach() when the application is
        /// done with the context. ::cuCtxAttach() fails if there is no context current
        /// to the thread.
        ///
        /// Currently, the <paramref name="flags"/> parameter must be 0.</summary>
        ///
        /// <param name="pctx">Returned context handle of the current context</param>
        /// <param name="flags">Context attach flags (must be 0)</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuCtxCreate,
        /// ::cuCtxDestroy,
        /// ::cuCtxDetach,
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
        /// CUresult CUDAAPI cuCtxAttach(CUcontext *pctx, unsigned int flags);
        [Obsolete]
        [DllImport(_dllpath, EntryPoint = "cuCtxAttach")]
        public static extern CuResult CtxAttach(out CuContext pctx, CuContextFlags flags);

        /// <summary>Decrement a context's usage-count
        ///
        /// \deprecated
        ///
        /// Note that this function is deprecated and should not be used.
        ///
        /// Decrements the usage count of the context <paramref name="ctx"/>, and destroys the context
        /// if the usage count goes to 0. The context must be a handle that was passed
        /// back by ::cuCtxCreate() or ::cuCtxAttach(), and must be current to the
        /// calling thread.</summary>
        ///
        /// <param name="ctx">Context to destroy</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT
        /// </returns>
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
        /// ::cuCtxPushCurrent,
        /// ::cuCtxSetCacheConfig,
        /// ::cuCtxSetLimit,
        /// ::cuCtxSynchronize
        /// CUresult CUDAAPI cuCtxDetach(CUcontext ctx);
        [Obsolete]
        [DllImport(_dllpath, EntryPoint = "cuCtxDetach")]
        public static extern CuResult CtxDetach(CuContext ctx);
    }
}
