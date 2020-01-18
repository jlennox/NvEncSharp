using System;
using System.Runtime.InteropServices;

namespace Lennox.NvEncSharp
{
    public static partial class LibCuda
    {
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
        [DllImport(_dllpath, EntryPoint = "cuMemGetInfo" + _ver)]
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
        [DllImport(_dllpath, EntryPoint = "cuMemAlloc" + _ver)]
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
        [DllImport(_dllpath, EntryPoint = "cuMemAllocPitch" + _ver)]
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
        [DllImport(_dllpath, EntryPoint = "cuMemFree" + _ver)]
        public static extern CuResult MemFree(CuDevicePtr dptr);
    }
}