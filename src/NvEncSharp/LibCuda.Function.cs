using System.Runtime.InteropServices;

// ReSharper disable UnusedMember.Global

namespace Lennox.NvEncSharp
{
    /// \defgroup CUDA_EXEC Execution Control
    ///
    /// ___MANBRIEF___ execution control functions of the low-level CUDA driver API
    /// (___CURRENT_FILE___) ___ENDMANBRIEF___
    ///
    /// This section describes the execution control functions of the low-level CUDA
    /// driver application programming interface.
    public unsafe partial class LibCuda
    {
        /// <summary>Returns information about a function
        ///
        /// Returns in *<paramref name="pi"/> the integer value of the attribute <paramref name="attrib"/> on the kernel
        /// given by <paramref name="hfunc"/>. The supported attributes are:
        /// - ::CU_FUNC_ATTRIBUTE_MAX_THREADS_PER_BLOCK: The maximum number of threads
        ///   per block, beyond which a launch of the function would fail. This number
        ///   depends on both the function and the device on which the function is
        ///   currently loaded.
        /// - ::CU_FUNC_ATTRIBUTE_SHARED_SIZE_BYTES: The size in bytes of
        ///   statically-allocated shared memory per block required by this function.
        ///   This does not include dynamically-allocated shared memory requested by
        ///   the user at runtime.
        /// - ::CU_FUNC_ATTRIBUTE_CONST_SIZE_BYTES: The size in bytes of user-allocated
        ///   constant memory required by this function.
        /// - ::CU_FUNC_ATTRIBUTE_LOCAL_SIZE_BYTES: The size in bytes of local memory
        ///   used by each thread of this function.
        /// - ::CU_FUNC_ATTRIBUTE_NUM_REGS: The number of registers used by each thread
        ///   of this function.
        /// - ::CU_FUNC_ATTRIBUTE_PTX_VERSION: The PTX virtual architecture version for
        ///   which the function was compiled. This value is the major PTX version * 10
        ///   + the minor PTX version, so a PTX version 1.3 function would return the
        ///   value 13. Note that this may return the undefined value of 0 for cubins
        ///   compiled prior to CUDA 3.0.
        /// - ::CU_FUNC_ATTRIBUTE_BINARY_VERSION: The binary architecture version for
        ///   which the function was compiled. This value is the major binary
        ///   version * 10 + the minor binary version, so a binary version 1.3 function
        ///   would return the value 13. Note that this will return a value of 10 for
        ///   legacy cubins that do not have a properly-encoded binary architecture
        ///   version.
        /// - ::CU_FUNC_CACHE_MODE_CA: The attribute to indicate whether the function has
        ///   been compiled with user specified option "-Xptxas --dlcm=ca" set .
        /// - ::CU_FUNC_ATTRIBUTE_MAX_DYNAMIC_SHARED_SIZE_BYTES: The maximum size in bytes of
        ///   dynamically-allocated shared memory.
        /// - ::CU_FUNC_ATTRIBUTE_PREFERRED_SHARED_MEMORY_CARVEOUT: Preferred shared memory-L1
        ///   cache split ratio in percent of shared memory.</summary>
        ///
        /// <param name="pi">Returned attribute value</param>
        /// <param name="attrib">Attribute requested</param>
        /// <param name="hfunc">Function to query attribute of</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuCtxGetCacheConfig,
        /// ::cuCtxSetCacheConfig,
        /// ::cuFuncSetCacheConfig,
        /// ::cuLaunchKernel,
        /// ::cudaFuncGetAttributes
        /// ::cudaFuncSetAttribute
        /// CUresult CUDAAPI cuFuncGetAttribute(int *pi, CUfunction_attribute attrib, CUfunction hfunc);
        [DllImport(_dllpath, EntryPoint = "cuFuncGetAttribute")]
        public static extern CuResult FuncGetAttribute(out int pi, FunctionAttribute attrib, CuFunction hfunc);

        /// <summary>Sets information about a function
        ///
        /// This call sets the value of a specified attribute <paramref name="attrib"/> on the kernel given
        /// by <paramref name="hfunc"/> to an integer value specified by <paramref name="value"/>
        /// This function returns CUDA_SUCCESS if the new value of the attribute could be
        /// successfully set. If the set fails, this call will return an error.
        /// Not all attributes can have values set. Attempting to set a value on a read-only
        /// attribute will result in an error (CUDA_ERROR_INVALID_VALUE)
        ///
        /// Supported attributes for the cuFuncSetAttribute call are:
        /// - ::CU_FUNC_ATTRIBUTE_MAX_DYNAMIC_SHARED_SIZE_BYTES: This maximum size in bytes of
        ///   dynamically-allocated shared memory. The value should contain the requested
        ///   maximum size of dynamically-allocated shared memory. The sum of this value and
        ///   the function attribute ::CU_FUNC_ATTRIBUTE_SHARED_SIZE_BYTES cannot exceed the
        ///   device attribute ::CU_DEVICE_ATTRIBUTE_MAX_SHARED_MEMORY_PER_BLOCK_OPTIN.
        ///   The maximal size of requestable dynamic shared memory may differ by GPU
        ///   architecture.
        /// - ::CU_FUNC_ATTRIBUTE_PREFERRED_SHARED_MEMORY_CARVEOUT: On devices where the L1
        ///   cache and shared memory use the same hardware resources, this sets the shared memory
        ///   carveout preference, in percent of the total resources. This is only a hint, and the
        ///   driver can choose a different ratio if required to execute the function.</summary>
        ///
        /// <param name="hfunc">Function to query attribute of</param>
        /// <param name="attrib">Attribute requested</param>
        /// <param name="value">The value to set</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuCtxGetCacheConfig,
        /// ::cuCtxSetCacheConfig,
        /// ::cuFuncSetCacheConfig,
        /// ::cuLaunchKernel,
        /// ::cudaFuncGetAttributes
        /// ::cudaFuncSetAttribute
        /// CUresult CUDAAPI cuFuncSetAttribute(CUfunction hfunc, CUfunction_attribute attrib, int value);
        [DllImport(_dllpath, EntryPoint = "cuFuncSetAttribute")]
        public static extern CuResult FuncSetAttribute(CuFunction hfunc, FunctionAttribute attrib, int value);

        /// <summary>Sets the preferred cache configuration for a device function
        ///
        /// On devices where the L1 cache and shared memory use the same hardware
        /// resources, this sets through <paramref name="config"/> the preferred cache configuration for
        /// the device function <paramref name="hfunc"/>. This is only a preference. The driver will use
        /// the requested configuration if possible, but it is free to choose a different
        /// configuration if required to execute <paramref name="hfunc"/>.  Any context-wide preference
        /// set via ::cuCtxSetCacheConfig() will be overridden by this per-function
        /// setting unless the per-function setting is ::CU_FUNC_CACHE_PREFER_NONE. In
        /// that case, the current context-wide setting will be used.
        ///
        /// This setting does nothing on devices where the size of the L1 cache and
        /// shared memory are fixed.
        ///
        /// Launching a kernel with a different preference than the most recent
        /// preference setting may insert a device-side synchronization point.
        ///
        ///
        /// The supported cache configurations are:
        /// - ::CU_FUNC_CACHE_PREFER_NONE: no preference for shared memory or L1 (default)
        /// - ::CU_FUNC_CACHE_PREFER_SHARED: prefer larger shared memory and smaller L1 cache
        /// - ::CU_FUNC_CACHE_PREFER_L1: prefer larger L1 cache and smaller shared memory
        /// - ::CU_FUNC_CACHE_PREFER_EQUAL: prefer equal sized L1 cache and shared memory</summary>
        ///
        /// <param name="hfunc">Kernel to configure cache for</param>
        /// <param name="config">Requested cache configuration</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuCtxGetCacheConfig,
        /// ::cuCtxSetCacheConfig,
        /// ::cuFuncGetAttribute,
        /// ::cuLaunchKernel,
        /// ::cudaFuncSetCacheConfig
        /// CUresult CUDAAPI cuFuncSetCacheConfig(CUfunction hfunc, CUfunc_cache config);
        [DllImport(_dllpath, EntryPoint = "cuFuncSetCacheConfig")]
        public static extern CuResult FuncSetCacheConfig(CuFunction hfunc, CuFunctionCache config);

        /// <summary>Sets the shared memory configuration for a device function.
        ///
        /// On devices with configurable shared memory banks, this function will
        /// force all subsequent launches of the specified device function to have
        /// the given shared memory bank size configuration. On any given launch of the
        /// function, the shared memory configuration of the device will be temporarily
        /// changed if needed to suit the function's preferred configuration. Changes in
        /// shared memory configuration between subsequent launches of functions,
        /// may introduce a device side synchronization point.
        ///
        /// Any per-function setting of shared memory bank size set via
        /// ::cuFuncSetSharedMemConfig will override the context wide setting set with
        /// ::cuCtxSetSharedMemConfig.
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
        /// - ::CU_SHARED_MEM_CONFIG_DEFAULT_BANK_SIZE: use the context's shared memory
        ///   configuration when launching this function.
        /// - ::CU_SHARED_MEM_CONFIG_FOUR_BYTE_BANK_SIZE: set shared memory bank width to
        ///   be natively four bytes when launching this function.
        /// - ::CU_SHARED_MEM_CONFIG_EIGHT_BYTE_BANK_SIZE: set shared memory bank width to
        ///   be natively eight bytes when launching this function.</summary>
        ///
        /// <param name="hfunc">kernel to be given a shared memory config</param>
        /// <param name="config">requested shared memory configuration</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuCtxGetCacheConfig,
        /// ::cuCtxSetCacheConfig,
        /// ::cuCtxGetSharedMemConfig,
        /// ::cuCtxSetSharedMemConfig,
        /// ::cuFuncGetAttribute,
        /// ::cuLaunchKernel,
        /// ::cudaFuncSetSharedMemConfig
        /// CUresult CUDAAPI cuFuncSetSharedMemConfig(CUfunction hfunc, CUsharedconfig config);
        [DllImport(_dllpath, EntryPoint = "cuFuncSetSharedMemConfig")]
        public static extern CuResult FuncSetSharedMemConfig(CuFunction hfunc, CuSharedConfig config);

        /// <summary>Launches a CUDA function
        ///
        /// Invokes the kernel <paramref name="f"/> on a <paramref name="gridDimX"/> x <paramref name="gridDimY"/> x <paramref name="gridDimZ"/>
        /// grid of blocks. Each block contains <paramref name="blockDimX"/> x <paramref name="blockDimY"/> x
        /// <paramref name="blockDimZ"/> threads.
        ///
        /// <paramref name="sharedMemBytes"/> sets the amount of dynamic shared memory that will be
        /// available to each thread block.
        ///
        /// Kernel parameters to <paramref name="f"/> can be specified in one of two ways:
        ///
        /// 1) Kernel parameters can be specified via <paramref name="kernelParams"/>.  If <paramref name="f"/>
        /// has N parameters, then <paramref name="kernelParams"/> needs to be an array of N
        /// pointers.  Each of <paramref name="kernelParams[0]"/> through <paramref name="kernelParams[N-1]"/>
        /// must point to a region of memory from which the actual kernel
        /// parameter will be copied.  The number of kernel parameters and their
        /// offsets and sizes do not need to be specified as that information is
        /// retrieved directly from the kernel's image.
        ///
        /// 2) Kernel parameters can also be packaged by the application into
        /// a single buffer that is passed in via the <paramref name="extra"/> parameter.
        /// This places the burden on the application of knowing each kernel
        /// parameter's size and alignment/padding within the buffer.  Here is
        /// an example of using the <paramref name="extra"/> parameter in this manner:
        /// <code>
        /// size_t argBufferSize;
        /// char argBuffer[256];
        ///
        /// // populate argBuffer and argBufferSize
        ///
        /// void *config[] = {
        ///     CU_LAUNCH_PARAM_BUFFER_POINTER, argBuffer,
        ///     CU_LAUNCH_PARAM_BUFFER_SIZE,    &amp;argBufferSize,
        ///     CU_LAUNCH_PARAM_END
        /// };
        /// status = cuLaunchKernel(f, gx, gy, gz, bx, by, bz, sh, s, NULL, config);
        /// </code>
        ///
        /// The <paramref name="extra"/> parameter exists to allow ::cuLaunchKernel to take
        /// additional less commonly used arguments.  <paramref name="extra"/> specifies a list of
        /// names of extra settings and their corresponding values.  Each extra
        /// setting name is immediately followed by the corresponding value.  The
        /// list must be terminated with either NULL or ::CU_LAUNCH_PARAM_END.
        ///
        /// - ::CU_LAUNCH_PARAM_END, which indicates the end of the <paramref name="extra"/>
        ///   array;
        /// - ::CU_LAUNCH_PARAM_BUFFER_POINTER, which specifies that the next
        ///   value in <paramref name="extra"/> will be a pointer to a buffer containing all
        ///   the kernel parameters for launching kernel <paramref name="f;"/>
        /// - ::CU_LAUNCH_PARAM_BUFFER_SIZE, which specifies that the next
        ///   value in <paramref name="extra"/> will be a pointer to a size_t containing the
        ///   size of the buffer specified with ::CU_LAUNCH_PARAM_BUFFER_POINTER;
        ///
        /// The error ::CUDA_ERROR_INVALID_VALUE will be returned if kernel
        /// parameters are specified with both <paramref name="kernelParams"/> and <paramref name="extra"/>
        /// (i.e. both <paramref name="kernelParams"/> and <paramref name="extra"/> are non-NULL).
        ///
        /// Calling ::cuLaunchKernel() sets persistent function state that is
        /// the same as function state set through the following deprecated APIs:
        ///  ::cuFuncSetBlockShape(),
        ///  ::cuFuncSetSharedSize(),
        ///  ::cuParamSetSize(),
        ///  ::cuParamSeti(),
        ///  ::cuParamSetf(),
        ///  ::cuParamSetv().
        ///
        /// When the kernel <paramref name="f"/> is launched via ::cuLaunchKernel(), the previous
        /// block shape, shared size and parameter info associated with <paramref name="f"/>
        /// is overwritten.
        ///
        /// Note that to use ::cuLaunchKernel(), the kernel <paramref name="f"/> must either have
        /// been compiled with toolchain version 3.2 or later so that it will
        /// contain kernel parameter information, or have no kernel parameters.
        /// If either of these conditions is not met, then ::cuLaunchKernel() will
        /// return ::CUDA_ERROR_INVALID_IMAGE.</summary>
        ///
        /// <param name="f">Kernel to launch</param>
        /// <param name="gridDimX">Width of grid in blocks</param>
        /// <param name="gridDimY">Height of grid in blocks</param>
        /// <param name="gridDimZ">Depth of grid in blocks</param>
        /// <param name="blockDimX">X dimension of each thread block</param>
        /// <param name="blockDimY">Y dimension of each thread block</param>
        /// <param name="blockDimZ">Z dimension of each thread block</param>
        /// <param name="sharedMemBytes">Dynamic shared-memory size per thread block in bytes</param>
        /// <param name="hStream">Stream identifier</param>
        /// <param name="kernelParams">Array of pointers to kernel parameters</param>
        /// <param name="extra">Extra options</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_INVALID_IMAGE,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_LAUNCH_FAILED,
        /// ::CUDA_ERROR_LAUNCH_OUT_OF_RESOURCES,
        /// ::CUDA_ERROR_LAUNCH_TIMEOUT,
        /// ::CUDA_ERROR_LAUNCH_INCOMPATIBLE_TEXTURING,
        /// ::CUDA_ERROR_SHARED_OBJECT_INIT_FAILED
        /// \note_null_stream
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuCtxGetCacheConfig,
        /// ::cuCtxSetCacheConfig,
        /// ::cuFuncSetCacheConfig,
        /// ::cuFuncGetAttribute,
        /// ::cudaLaunchKernel
        /// CUresult CUDAAPI cuLaunchKernel(CUfunction f,
        ///     unsigned int gridDimX,
        ///     unsigned int gridDimY,
        ///     unsigned int gridDimZ,
        ///     unsigned int blockDimX,
        ///     unsigned int blockDimY,
        ///     unsigned int blockDimZ,
        ///     unsigned int sharedMemBytes,
        ///     CUstream hStream,
        ///     void **kernelParams,
        ///     void **extra);
        [DllImport(_dllpath, EntryPoint = "cuLaunchKernel")]
        public static extern CuResult LaunchKernel(CuFunction f, int gridDimX, int gridDimY, int gridDimZ, int blockDimX, int blockDimY, int blockDimZ, int sharedMemBytes, CuStream hStream, void** kernelParams, void** extra);

        /// <summary>Launches a CUDA function where thread blocks can cooperate and synchronize as they execute
        ///
        /// Invokes the kernel <paramref name="f"/> on a <paramref name="gridDimX"/> x <paramref name="gridDimY"/> x <paramref name="gridDimZ"/>
        /// grid of blocks. Each block contains <paramref name="blockDimX"/> x <paramref name="blockDimY"/> x
        /// <paramref name="blockDimZ"/> threads.
        ///
        /// <paramref name="sharedMemBytes"/> sets the amount of dynamic shared memory that will be
        /// available to each thread block.
        ///
        /// The device on which this kernel is invoked must have a non-zero value for
        /// the device attribute ::CU_DEVICE_ATTRIBUTE_COOPERATIVE_LAUNCH.
        ///
        /// The total number of blocks launched cannot exceed the maximum number of blocks per
        /// multiprocessor as returned by ::cuOccupancyMaxActiveBlocksPerMultiprocessor (or
        /// ::cuOccupancyMaxActiveBlocksPerMultiprocessorWithFlags) times the number of multiprocessors
        /// as specified by the device attribute ::CU_DEVICE_ATTRIBUTE_MULTIPROCESSOR_COUNT.
        ///
        /// The kernel cannot make use of CUDA dynamic parallelism.
        ///
        /// Kernel parameters must be specified via <paramref name="kernelParams"/>.  If <paramref name="f"/>
        /// has N parameters, then <paramref name="kernelParams"/> needs to be an array of N
        /// pointers.  Each of <paramref name="kernelParams[0]"/> through <paramref name="kernelParams[N-1]"/>
        /// must point to a region of memory from which the actual kernel
        /// parameter will be copied.  The number of kernel parameters and their
        /// offsets and sizes do not need to be specified as that information is
        /// retrieved directly from the kernel's image.
        ///
        /// Calling ::cuLaunchCooperativeKernel() sets persistent function state that is
        /// the same as function state set through ::cuLaunchKernel API
        ///
        /// When the kernel <paramref name="f"/> is launched via ::cuLaunchCooperativeKernel(), the previous
        /// block shape, shared size and parameter info associated with <paramref name="f"/>
        /// is overwritten.
        ///
        /// Note that to use ::cuLaunchCooperativeKernel(), the kernel <paramref name="f"/> must either have
        /// been compiled with toolchain version 3.2 or later so that it will
        /// contain kernel parameter information, or have no kernel parameters.
        /// If either of these conditions is not met, then ::cuLaunchCooperativeKernel() will
        /// return ::CUDA_ERROR_INVALID_IMAGE.</summary>
        ///
        /// <param name="f">Kernel to launch</param>
        /// <param name="gridDimX">Width of grid in blocks</param>
        /// <param name="gridDimY">Height of grid in blocks</param>
        /// <param name="gridDimZ">Depth of grid in blocks</param>
        /// <param name="blockDimX">X dimension of each thread block</param>
        /// <param name="blockDimY">Y dimension of each thread block</param>
        /// <param name="blockDimZ">Z dimension of each thread block</param>
        /// <param name="sharedMemBytes">Dynamic shared-memory size per thread block in bytes</param>
        /// <param name="hStream">Stream identifier</param>
        /// <param name="kernelParams">Array of pointers to kernel parameters</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_INVALID_IMAGE,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_LAUNCH_FAILED,
        /// ::CUDA_ERROR_LAUNCH_OUT_OF_RESOURCES,
        /// ::CUDA_ERROR_LAUNCH_TIMEOUT,
        /// ::CUDA_ERROR_LAUNCH_INCOMPATIBLE_TEXTURING,
        /// ::CUDA_ERROR_COOPERATIVE_LAUNCH_TOO_LARGE,
        /// ::CUDA_ERROR_SHARED_OBJECT_INIT_FAILED
        /// \note_null_stream
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuCtxGetCacheConfig,
        /// ::cuCtxSetCacheConfig,
        /// ::cuFuncSetCacheConfig,
        /// ::cuFuncGetAttribute,
        /// ::cuLaunchCooperativeKernelMultiDevice,
        /// ::cudaLaunchCooperativeKernel
        /// CUresult CUDAAPI cuLaunchCooperativeKernel(CUfunction f,
        ///     unsigned int gridDimX,
        ///     unsigned int gridDimY,
        ///     unsigned int gridDimZ,
        ///     unsigned int blockDimX,
        ///     unsigned int blockDimY,
        ///     unsigned int blockDimZ,
        ///     unsigned int sharedMemBytes,
        ///     CUstream hStream,
        ///     void **kernelParams);
        [DllImport(_dllpath, EntryPoint = "cuLaunchCooperativeKernel")]
        public static extern CuResult LaunchCooperativeKernel(CuFunction f, int gridDimX, int gridDimY, int gridDimZ, int blockDimX, int blockDimY, int blockDimZ, int sharedMemBytes, CuStream hStream, void** kernelParams);

        /// <summary>Launches CUDA functions on multiple devices where thread blocks can cooperate and synchronize as they execute
        ///
        /// Invokes kernels as specified in the <paramref name="launchParamsList"/> array where each element
        /// of the array specifies all the parameters required to perform a single kernel launch.
        /// These kernels can cooperate and synchronize as they execute. The size of the array is
        /// specified by <paramref name="numDevices."/>
        ///
        /// No two kernels can be launched on the same device. All the devices targeted by this
        /// multi-device launch must be identical. All devices must have a non-zero value for the
        /// device attribute ::CU_DEVICE_ATTRIBUTE_COOPERATIVE_MULTI_DEVICE_LAUNCH.
        ///
        /// All kernels launched must be identical with respect to the compiled code. Note that
        /// any __device__, __constant__ or __managed__ variables present in the module that owns
        /// the kernel launched on each device, are independently instantiated on every device.
        /// It is the application's responsiblity to ensure these variables are initialized and
        /// used appropriately.
        ///
        /// The size of the grids as specified in blocks, the size of the blocks themselves
        /// and the amount of shared memory used by each thread block must also match across
        /// all launched kernels.
        ///
        /// The streams used to launch these kernels must have been created via either ::cuStreamCreate
        /// or ::cuStreamCreateWithPriority. The NULL stream or ::CU_STREAM_LEGACY or ::CU_STREAM_PER_THREAD
        /// cannot be used.
        ///
        /// The total number of blocks launched per kernel cannot exceed the maximum number of blocks
        /// per multiprocessor as returned by ::cuOccupancyMaxActiveBlocksPerMultiprocessor (or
        /// ::cuOccupancyMaxActiveBlocksPerMultiprocessorWithFlags) times the number of multiprocessors
        /// as specified by the device attribute ::CU_DEVICE_ATTRIBUTE_MULTIPROCESSOR_COUNT. Since the
        /// total number of blocks launched per device has to match across all devices, the maximum
        /// number of blocks that can be launched per device will be limited by the device with the
        /// least number of multiprocessors.
        ///
        /// The kernels cannot make use of CUDA dynamic parallelism.
        /// where:
        /// - ::CUDA_LAUNCH_PARAMS::function specifies the kernel to be launched. All functions must
        ///   be identical with respect to the compiled code.
        /// - ::CUDA_LAUNCH_PARAMS::gridDimX is the width of the grid in blocks. This must match across
        ///   all kernels launched.
        /// - ::CUDA_LAUNCH_PARAMS::gridDimY is the height of the grid in blocks. This must match across
        ///   all kernels launched.
        /// - ::CUDA_LAUNCH_PARAMS::gridDimZ is the depth of the grid in blocks. This must match across
        ///   all kernels launched.
        /// - ::CUDA_LAUNCH_PARAMS::blockDimX is the X dimension of each thread block. This must match across
        ///   all kernels launched.
        /// - ::CUDA_LAUNCH_PARAMS::blockDimX is the Y dimension of each thread block. This must match across
        ///   all kernels launched.
        /// - ::CUDA_LAUNCH_PARAMS::blockDimZ is the Z dimension of each thread block. This must match across
        ///   all kernels launched.
        /// - ::CUDA_LAUNCH_PARAMS::sharedMemBytes is the dynamic shared-memory size per thread block in bytes.
        ///   This must match across all kernels launched.
        /// - ::CUDA_LAUNCH_PARAMS::hStream is the handle to the stream to perform the launch in. This cannot
        ///   be the NULL stream or ::CU_STREAM_LEGACY or ::CU_STREAM_PER_THREAD. The CUDA context associated
        ///   with this stream must match that associated with ::CUDA_LAUNCH_PARAMS::function.
        /// - ::CUDA_LAUNCH_PARAMS::kernelParams is an array of pointers to kernel parameters. If
        ///   ::CUDA_LAUNCH_PARAMS::function has N parameters, then ::CUDA_LAUNCH_PARAMS::kernelParams
        ///   needs to be an array of N pointers. Each of ::CUDA_LAUNCH_PARAMS::kernelParams[0] through
        ///   ::CUDA_LAUNCH_PARAMS::kernelParams[N-1] must point to a region of memory from which the actual
        ///   kernel parameter will be copied. The number of kernel parameters and their offsets and sizes
        ///   do not need to be specified as that information is retrieved directly from the kernel's image.
        ///
        /// By default, the kernel won't begin execution on any GPU until all prior work in all the specified
        /// streams has completed. This behavior can be overridden by specifying the flag
        /// ::CUDA_COOPERATIVE_LAUNCH_MULTI_DEVICE_NO_PRE_LAUNCH_SYNC. When this flag is specified, each kernel
        /// will only wait for prior work in the stream corresponding to that GPU to complete before it begins
        /// execution.
        ///
        /// Similarly, by default, any subsequent work pushed in any of the specified streams will not begin
        /// execution until the kernels on all GPUs have completed. This behavior can be overridden by specifying
        /// the flag ::CUDA_COOPERATIVE_LAUNCH_MULTI_DEVICE_NO_POST_LAUNCH_SYNC. When this flag is specified,
        /// any subsequent work pushed in any of the specified streams will only wait for the kernel launched
        /// on the GPU corresponding to that stream to complete before it begins execution.
        ///
        /// Calling ::cuLaunchCooperativeKernelMultiDevice() sets persistent function state that is
        /// the same as function state set through ::cuLaunchKernel API when called individually for each
        /// element in <paramref name="launchParamsList."/>
        ///
        /// When kernels are launched via ::cuLaunchCooperativeKernelMultiDevice(), the previous
        /// block shape, shared size and parameter info associated with each ::CUDA_LAUNCH_PARAMS::function
        /// in <paramref name="launchParamsList"/> is overwritten.
        ///
        /// Note that to use ::cuLaunchCooperativeKernelMultiDevice(), the kernels must either have
        /// been compiled with toolchain version 3.2 or later so that it will
        /// contain kernel parameter information, or have no kernel parameters.
        /// If either of these conditions is not met, then ::cuLaunchCooperativeKernelMultiDevice() will
        /// return ::CUDA_ERROR_INVALID_IMAGE.</summary>
        ///
        /// <param name="launchParamsList">List of launch parameters, one per device</param>
        /// <param name="numDevices">Size of the <paramref name="launchParamsList"/> array</param>
        /// <param name="flags">Flags to control launch behavior</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_INVALID_IMAGE,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_LAUNCH_FAILED,
        /// ::CUDA_ERROR_LAUNCH_OUT_OF_RESOURCES,
        /// ::CUDA_ERROR_LAUNCH_TIMEOUT,
        /// ::CUDA_ERROR_LAUNCH_INCOMPATIBLE_TEXTURING,
        /// ::CUDA_ERROR_COOPERATIVE_LAUNCH_TOO_LARGE,
        /// ::CUDA_ERROR_SHARED_OBJECT_INIT_FAILED
        /// \note_null_stream
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuCtxGetCacheConfig,
        /// ::cuCtxSetCacheConfig,
        /// ::cuFuncSetCacheConfig,
        /// ::cuFuncGetAttribute,
        /// ::cuLaunchCooperativeKernel,
        /// ::cudaLaunchCooperativeKernelMultiDevice
        /// CUresult CUDAAPI cuLaunchCooperativeKernelMultiDevice(CUDA_LAUNCH_PARAMS *launchParamsList, unsigned int numDevices, unsigned int flags);
        [DllImport(_dllpath, EntryPoint = "cuLaunchCooperativeKernelMultiDevice")]
        public static extern CuResult LaunchCooperativeKernelMultiDevice(CuLaunchParams* launchParamsList, int numDevices, CuCooperativeLaunchMultiDevice flags);
    }
}
