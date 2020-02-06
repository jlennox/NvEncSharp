using System;
using System.Runtime.InteropServices;

// ReSharper disable UnusedMember.Global

namespace Lennox.NvEncSharp
{
    /// \defgroup CUDA_OCCUPANCY Occupancy
    ///
    /// ___MANBRIEF___ occupancy calculation functions of the low-level CUDA driver
    /// API (___CURRENT_FILE___) ___ENDMANBRIEF___
    ///
    /// This section describes the occupancy calculation functions of the low-level CUDA
    /// driver application programming interface.
    public partial class LibCuda
    {
        /// <summary>Returns occupancy of a function
        ///
        /// Returns in *<paramref name="numBlocks"/> the number of the maximum active blocks per
        /// streaming multiprocessor.</summary>
        ///
        /// <param name="numBlocks">Returned occupancy</param>
        /// <param name="func">Kernel for which occupancy is calculated</param>
        /// <param name="blockSize">Block size the kernel is intended to be launched with</param>
        /// <param name="dynamicSMemSize">Per-block dynamic shared memory usage intended, in bytes</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_UNKNOWN
        /// </returns>
        /// \notefnerr
        ///
        /// \sa
        /// ::cudaOccupancyMaxActiveBlocksPerMultiprocessor
        /// CUresult CUDAAPI cuOccupancyMaxActiveBlocksPerMultiprocessor(int *numBlocks, CUfunction func, int blockSize, size_t dynamicSMemSize);
        [DllImport(_dllpath, EntryPoint = "cuOccupancyMaxActiveBlocksPerMultiprocessor")]
        public static extern CuResult OccupancyMaxActiveBlocksPerMultiprocessor(out int numBlocks, CuFunction func, int blockSize, IntPtr dynamicSMemSize);

        /// <summary>Returns occupancy of a function
        ///
        /// Returns in *<paramref name="numBlocks"/> the number of the maximum active blocks per
        /// streaming multiprocessor.
        ///
        /// The <paramref name="flags"/> parameter controls how special cases are handled. The
        /// valid flags are:
        ///
        /// - ::CU_OCCUPANCY_DEFAULT, which maintains the default behavior as
        ///   ::cuOccupancyMaxActiveBlocksPerMultiprocessor;
        ///
        /// - ::CU_OCCUPANCY_DISABLE_CACHING_OVERRIDE, which suppresses the
        ///   default behavior on platform where global caching affects
        ///   occupancy. On such platforms, if caching is enabled, but
        ///   per-block SM resource usage would result in zero occupancy, the
        ///   occupancy calculator will calculate the occupancy as if caching
        ///   is disabled. Setting ::CU_OCCUPANCY_DISABLE_CACHING_OVERRIDE makes
        ///   the occupancy calculator to return 0 in such cases. More information
        ///   can be found about this feature in the "Unified L1/Texture Cache"
        ///   section of the Maxwell tuning guide.</summary>
        ///
        /// <param name="numBlocks">Returned occupancy</param>
        /// <param name="func">Kernel for which occupancy is calculated</param>
        /// <param name="blockSize">Block size the kernel is intended to be launched with</param>
        /// <param name="dynamicSMemSize">Per-block dynamic shared memory usage intended, in bytes</param>
        /// <param name="flags">Requested behavior for the occupancy calculator</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_UNKNOWN
        /// </returns>
        /// \notefnerr
        ///
        /// \sa
        /// ::cudaOccupancyMaxActiveBlocksPerMultiprocessorWithFlags
        /// CUresult CUDAAPI cuOccupancyMaxActiveBlocksPerMultiprocessorWithFlags(int *numBlocks, CUfunction func, int blockSize, size_t dynamicSMemSize, unsigned int flags);
        [DllImport(_dllpath, EntryPoint = "cuOccupancyMaxActiveBlocksPerMultiprocessorWithFlags")]
        public static extern CuResult OccupancyMaxActiveBlocksPerMultiprocessorWithFlags(out int numBlocks, CuFunction func, int blockSize, IntPtr dynamicSMemSize, OccupancyFlags flags);

        /// <summary>Suggest a launch configuration with reasonable occupancy
        ///
        /// Returns in *<paramref name="blockSize"/> a reasonable block size that can achieve
        /// the maximum occupancy (or, the maximum number of active warps with
        /// the fewest blocks per multiprocessor), and in *<paramref name="minGridSize"/> the
        /// minimum grid size to achieve the maximum occupancy.
        ///
        /// If <paramref name="blockSizeLimit"/> is 0, the configurator will use the maximum
        /// block size permitted by the device / function instead.
        ///
        /// If per-block dynamic shared memory allocation is not needed, the
        /// user should leave both <paramref name="blockSizeToDynamicSMemSize"/> and \p
        /// dynamicSMemSize as 0.
        ///
        /// If per-block dynamic shared memory allocation is needed, then if
        /// the dynamic shared memory size is constant regardless of block
        /// size, the size should be passed through <paramref name="dynamicSMemSize"/>, and \p
        /// blockSizeToDynamicSMemSize should be NULL.
        ///
        /// Otherwise, if the per-block dynamic shared memory size varies with
        /// different block sizes, the user needs to provide a unary function
        /// through <paramref name="blockSizeToDynamicSMemSize"/> that computes the dynamic
        /// shared memory needed by <paramref name="func"/> for any given block size. \p
        /// dynamicSMemSize is ignored. An example signature is:
        ///
        /// <code>
        ///    // Take block size, returns dynamic shared memory needed
        ///    size_t blockToSmem(int blockSize);
        /// </code></summary>
        ///
        /// <param name="minGridSize">Returned minimum grid size needed to achieve the maximum occupancy</param>
        /// <param name="blockSize">Returned maximum block size that can achieve the maximum occupancy</param>
        /// <param name="func">Kernel for which launch configuration is calculated</param>
        /// <param name="blockSizeToDynamicSMemSize">A function that calculates how much per-block dynamic shared memory <paramref name="func"/> uses based on the block size</param>
        /// <param name="dynamicSMemSize">Dynamic shared memory usage intended, in bytes</param>
        /// <param name="blockSizeLimit">The maximum block size <paramref name="func"/> is designed to handle</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_UNKNOWN
        /// </returns>
        /// \notefnerr
        ///
        /// \sa
        /// ::cudaOccupancyMaxPotentialBlockSize
        /// CUresult CUDAAPI cuOccupancyMaxPotentialBlockSize(int *minGridSize, int *blockSize, CUfunction func, CUoccupancyB2DSize blockSizeToDynamicSMemSize, size_t dynamicSMemSize, int blockSizeLimit);
        [DllImport(_dllpath, EntryPoint = "cuOccupancyMaxPotentialBlockSize")]
        public static extern CuResult OccupancyMaxPotentialBlockSize(out int minGridSize, out int blockSize, CuFunction func, CuOccupancyB2DSize blockSizeToDynamicSMemSize, IntPtr dynamicSMemSize, int blockSizeLimit);

        /// <summary>Suggest a launch configuration with reasonable occupancy
        ///
        /// An extended version of ::cuOccupancyMaxPotentialBlockSize. In
        /// addition to arguments passed to ::cuOccupancyMaxPotentialBlockSize,
        /// ::cuOccupancyMaxPotentialBlockSizeWithFlags also takes a <paramref name="flags"/>
        /// parameter.
        ///
        /// The <paramref name="flags"/> parameter controls how special cases are handled. The
        /// valid flags are:
        ///
        /// - ::CU_OCCUPANCY_DEFAULT, which maintains the default behavior as
        ///   ::cuOccupancyMaxPotentialBlockSize;
        ///
        /// - ::CU_OCCUPANCY_DISABLE_CACHING_OVERRIDE, which suppresses the
        ///   default behavior on platform where global caching affects
        ///   occupancy. On such platforms, the launch configurations that
        ///   produces maximal occupancy might not support global
        ///   caching. Setting ::CU_OCCUPANCY_DISABLE_CACHING_OVERRIDE
        ///   guarantees that the the produced launch configuration is global
        ///   caching compatible at a potential cost of occupancy. More information
        ///   can be found about this feature in the "Unified L1/Texture Cache"
        ///   section of the Maxwell tuning guide.</summary>
        ///
        /// <param name="minGridSize">Returned minimum grid size needed to achieve the maximum occupancy</param>
        /// <param name="blockSize">Returned maximum block size that can achieve the maximum occupancy</param>
        /// <param name="func">Kernel for which launch configuration is calculated</param>
        /// <param name="blockSizeToDynamicSMemSize">A function that calculates how much per-block dynamic shared memory <paramref name="func"/> uses based on the block size</param>
        /// <param name="dynamicSMemSize">Dynamic shared memory usage intended, in bytes</param>
        /// <param name="blockSizeLimit">The maximum block size <paramref name="func"/> is designed to handle</param>
        /// <param name="flags">Options</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_UNKNOWN
        /// </returns>
        /// \notefnerr
        ///
        /// \sa
        /// ::cudaOccupancyMaxPotentialBlockSizeWithFlags
        /// CUresult CUDAAPI cuOccupancyMaxPotentialBlockSizeWithFlags(int *minGridSize, int *blockSize, CUfunction func, CUoccupancyB2DSize blockSizeToDynamicSMemSize, size_t dynamicSMemSize, int blockSizeLimit, unsigned int flags);
        [DllImport(_dllpath, EntryPoint = "cuOccupancyMaxPotentialBlockSizeWithFlags")]
        public static extern CuResult OccupancyMaxPotentialBlockSizeWithFlags(out int minGridSize, out int blockSize, CuFunction func, CuOccupancyB2DSize blockSizeToDynamicSMemSize, IntPtr dynamicSMemSize, int blockSizeLimit, OccupancyFlags flags);
    }
}
