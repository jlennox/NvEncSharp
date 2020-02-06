using System;
using System.Runtime.InteropServices;

// ReSharper disable UnusedMember.Global

namespace Lennox.NvEncSharp
{
    public static unsafe partial class LibCuda
    {
        /// <summary>Gets free and total memory
        ///
        /// Returns in *<paramref name="free"/> and *<paramref name="total"/> respectively, the free and total amount of
        /// memory available for allocation by the CUDA context, in bytes.</summary>
        ///
        /// <param name="free">Returned free memory in bytes</param>
        /// <param name="total">Returned total memory in bytes</param>
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
        /// *<paramref name="dptr"/> a pointer to the allocated memory. The allocated memory is suitably
        /// aligned for any kind of variable. The memory is not cleared. If <c>bytesize</c>
        /// is 0, ::cuMemAlloc() returns ::CUDA_ERROR_INVALID_VALUE.</summary>
        ///
        /// <param name="dptr">Returned device pointer</param>
        /// <param name="bytesize">Requested allocation size in bytes</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_OUT_OF_MEMORY
        /// </returns>
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
        /// the device and returns in *<paramref name="dptr"/> a pointer to the allocated memory. The
        /// function may pad the allocation to ensure that corresponding pointers in
        /// any given row will continue to meet the alignment requirements for
        /// coalescing as the address is updated from row to row. <c>ElementSizeBytes</c>
        /// specifies the size of the largest reads and writes that will be performed
        /// on the memory range. <c>ElementSizeBytes</c> may be 4, 8 or 16 (since coalesced
        /// memory transactions are not possible on other data sizes). If
        /// <c>ElementSizeBytes</c> is smaller than the actual read/write size of a kernel,
        /// the kernel will run correctly, but possibly at reduced speed. The pitch
        /// returned in *<paramref name="pitch"/> by ::cuMemAllocPitch() is the width in bytes of the
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
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_OUT_OF_MEMORY
        /// </returns>
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

        /// <summary>Set attributes on a previously allocated memory region
        ///
        /// The supported attributes are:
        ///
        /// - ::CU_POINTER_ATTRIBUTE_SYNC_MEMOPS:
        ///
        ///      A boolean attribute that can either be set (1) or unset (0). When set,
        ///      the region of memory that <paramref name="ptr"/> points to is guaranteed to always synchronize
        ///      memory operations that are synchronous. If there are some previously initiated
        ///      synchronous memory operations that are pending when this attribute is set, the
        ///      function does not return until those memory operations are complete.
        ///      See further documentation in the section titled "API synchronization behavior"
        ///      to learn more about cases when synchronous memory operations can
        ///      exhibit asynchronous behavior.
        ///      <paramref name="value"/> will be considered as a pointer to an unsigned integer to which this attribute is to be set.</summary>
        ///
        /// <param name="value">Pointer to memory containing the value to be set</param>
        /// <param name="attribute">Pointer attribute to set</param>
        /// <param name="ptr">Pointer to a memory region allocated using CUDA memory allocation APIs</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_INVALID_DEVICE
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuPointerGetAttribute,
        /// ::cuPointerGetAttributes,
        /// ::cuMemAlloc,
        /// ::cuMemFree,
        /// ::cuMemAllocHost,
        /// ::cuMemFreeHost,
        /// ::cuMemHostAlloc,
        /// ::cuMemHostRegister,
        /// ::cuMemHostUnregister
        /// CUresult CUDAAPI cuPointerSetAttribute(const void *value, CUpointer_attribute attribute, CUdeviceptr ptr);
        [DllImport(_dllpath, EntryPoint = "cuPointerSetAttribute")]
        public static extern CuResult PointerSetAttribute(IntPtr value, PointerAttribute attribute, CuDeviceMemory ptr);

        /// <summary>Returns information about a pointer.
        ///
        /// The supported attributes are (refer to ::cuPointerGetAttribute for attribute descriptions and restrictions):
        ///
        /// - ::CU_POINTER_ATTRIBUTE_CONTEXT
        /// - ::CU_POINTER_ATTRIBUTE_MEMORY_TYPE
        /// - ::CU_POINTER_ATTRIBUTE_DEVICE_POINTER
        /// - ::CU_POINTER_ATTRIBUTE_HOST_POINTER
        /// - ::CU_POINTER_ATTRIBUTE_SYNC_MEMOPS
        /// - ::CU_POINTER_ATTRIBUTE_BUFFER_ID
        /// - ::CU_POINTER_ATTRIBUTE_IS_MANAGED</summary>
        ///
        /// <param name="numAttributes">Number of attributes to query</param>
        /// <param name="attributes">An array of attributes to query
        /// (numAttributes and the number of attributes in this array should match)</param>
        /// <param name="data">A two-dimensional array containing pointers to memory
        /// locations where the result of each attribute query will be written to.</param>
        /// <param name="ptr">Pointer to query
        ///
        /// Unlike ::cuPointerGetAttribute, this function will not return an error when the <paramref name="ptr"/>
        /// encountered is not a valid CUDA pointer. Instead, the attributes are assigned default NULL values
        /// and CUDA_SUCCESS is returned.
        ///
        /// If <paramref name="ptr"/> was not allocated by, mapped by, or registered with a ::CUcontext which uses UVA
        /// (Unified Virtual Addressing), ::CUDA_ERROR_INVALID_CONTEXT is returned.</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_INVALID_DEVICE
        /// </returns>
        /// \notefnerr
        ///
        /// \sa
        /// ::cuPointerGetAttribute,
        /// ::cuPointerSetAttribute,
        /// ::cudaPointerGetAttributes
        /// CUresult CUDAAPI cuPointerGetAttributes(unsigned int numAttributes, CUpointer_attribute *attributes, void **data, CUdeviceptr ptr);
        [DllImport(_dllpath, EntryPoint = "cuPointerGetAttributes")]
        public static extern CuResult PointerGetAttributes(int numAttributes, out PointerAttribute[] attributes, out IntPtr[] data, CuDevicePtr ptr);

        /// <summary>Get information on memory allocations
        ///
        /// Returns the base address in *<paramref name="pbase"/> and size in *<paramref name="psize"/> of the
        /// allocation by ::cuMemAlloc() or ::cuMemAllocPitch() that contains the input
        /// pointer <paramref name="dptr"/>. Both parameters <paramref name="pbase"/> and <paramref name="psize"/> are optional. If one
        /// of them is NULL, it is ignored.</summary>
        ///
        /// <param name="pbase">Returned base address</param>
        /// <param name="psize">Returned size of device memory allocation</param>
        /// <param name="dptr">Device pointer to query</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_NOT_FOUND,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D16,
        /// ::cuMemsetD2D32, ::cuMemsetD8, ::cuMemsetD16, ::cuMemsetD32
        /// CUresult CUDAAPI cuMemGetAddressRange(CUdeviceptr *pbase, size_t *psize, CUdeviceptr dptr);
        [DllImport(_dllpath, EntryPoint = "cuMemGetAddressRange")]
        public static extern CuResult MemGetAddressRange(out CuDevicePtr pbase, out IntPtr psize, CuDevicePtr dptr);

        /// <summary>Allocates page-locked host memory
        ///
        /// Allocates <paramref name="bytesize"/> bytes of host memory that is page-locked and
        /// accessible to the device. The driver tracks the virtual memory ranges
        /// allocated with this function and automatically accelerates calls to
        /// functions such as ::cuMemcpy(). Since the memory can be accessed directly by
        /// the device, it can be read or written with much higher bandwidth than
        /// pageable memory obtained with functions such as ::malloc(). Allocating
        /// excessive amounts of memory with ::cuMemAllocHost() may degrade system
        /// performance, since it reduces the amount of memory available to the system
        /// for paging. As a result, this function is best used sparingly to allocate
        /// staging areas for data exchange between host and device.
        ///
        /// Note all host memory allocated using ::cuMemHostAlloc() will automatically
        /// be immediately accessible to all contexts on all devices which support unified
        /// addressing (as may be queried using ::CU_DEVICE_ATTRIBUTE_UNIFIED_ADDRESSING).
        /// The device pointer that may be used to access this host memory from those
        /// contexts is always equal to the returned host pointer <paramref name="pp"/>.
        /// See \ref CUDA_UNIFIED for additional details.</summary>
        ///
        /// <param name="pp">Returned host pointer to page-locked memory</param>
        /// <param name="bytesize">Requested allocation size in bytes</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_OUT_OF_MEMORY
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D16,
        /// ::cuMemsetD2D32, ::cuMemsetD8, ::cuMemsetD16, ::cuMemsetD32,
        /// ::cudaMallocHost
        /// CUresult CUDAAPI cuMemAllocHost(void **pp, size_t bytesize);
        [DllImport(_dllpath, EntryPoint = "cuMemAllocHost")]
        public static extern CuResult MemAllocHost(out CuHostMemory pp, IntPtr bytesize);

        /// <summary>Frees page-locked host memory
        ///
        /// Frees the memory space pointed to by <paramref name="p"/>, which must have been returned by
        /// a previous call to ::cuMemAllocHost().</summary>
        ///
        /// <param name="p">Pointer to memory to free</param>
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
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D16,
        /// ::cuMemsetD2D32, ::cuMemsetD8, ::cuMemsetD16, ::cuMemsetD32,
        /// ::cudaFreeHost
        /// CUresult CUDAAPI cuMemFreeHost(void *p);
        [DllImport(_dllpath, EntryPoint = "cuMemFreeHost")]
        public static extern CuResult MemFreeHost(CuHostMemory p);

        /// <summary>Allocates page-locked host memory
        ///
        /// Allocates <paramref name="bytesize"/> bytes of host memory that is page-locked and accessible
        /// to the device. The driver tracks the virtual memory ranges allocated with
        /// this function and automatically accelerates calls to functions such as
        /// ::cuMemcpyHtoD(). Since the memory can be accessed directly by the device,
        /// it can be read or written with much higher bandwidth than pageable memory
        /// obtained with functions such as ::malloc(). Allocating excessive amounts of
        /// pinned memory may degrade system performance, since it reduces the amount
        /// of memory available to the system for paging. As a result, this function is
        /// best used sparingly to allocate staging areas for data exchange between
        /// host and device.
        ///
        /// The <paramref name="flags"/> parameter enables different options to be specified that
        /// affect the allocation, as follows.
        ///
        /// - ::CU_MEMHOSTALLOC_PORTABLE: The memory returned by this call will be
        ///   considered as pinned memory by all CUDA contexts, not just the one that
        ///   performed the allocation.
        ///
        /// - ::CU_MEMHOSTALLOC_DEVICEMAP: Maps the allocation into the CUDA address
        ///   space. The device pointer to the memory may be obtained by calling
        ///   ::cuMemHostGetDevicePointer().
        ///
        /// - ::CU_MEMHOSTALLOC_WRITECOMBINED: Allocates the memory as write-combined
        ///   (WC). WC memory can be transferred across the PCI Express bus more
        ///   quickly on some system configurations, but cannot be read efficiently by
        ///   most CPUs. WC memory is a good option for buffers that will be written by
        ///   the CPU and read by the GPU via mapped pinned memory or host->device
        ///   transfers.
        ///
        /// All of these flags are orthogonal to one another: a developer may allocate
        /// memory that is portable, mapped and/or write-combined with no restrictions.
        ///
        /// The CUDA context must have been created with the ::CU_CTX_MAP_HOST flag in
        /// order for the ::CU_MEMHOSTALLOC_DEVICEMAP flag to have any effect.
        ///
        /// The ::CU_MEMHOSTALLOC_DEVICEMAP flag may be specified on CUDA contexts for
        /// devices that do not support mapped pinned memory. The failure is deferred
        /// to ::cuMemHostGetDevicePointer() because the memory may be mapped into
        /// other CUDA contexts via the ::CU_MEMHOSTALLOC_PORTABLE flag.
        ///
        /// The memory allocated by this function must be freed with ::cuMemFreeHost().
        ///
        /// Note all host memory allocated using ::cuMemHostAlloc() will automatically
        /// be immediately accessible to all contexts on all devices which support unified
        /// addressing (as may be queried using ::CU_DEVICE_ATTRIBUTE_UNIFIED_ADDRESSING).
        /// Unless the flag ::CU_MEMHOSTALLOC_WRITECOMBINED is specified, the device pointer
        /// that may be used to access this host memory from those contexts is always equal
        /// to the returned host pointer <paramref name="pp"/>.  If the flag ::CU_MEMHOSTALLOC_WRITECOMBINED
        /// is specified, then the function ::cuMemHostGetDevicePointer() must be used
        /// to query the device pointer, even if the context supports unified addressing.
        /// See \ref CUDA_UNIFIED for additional details.</summary>
        ///
        /// <param name="pp">Returned host pointer to page-locked memory</param>
        /// <param name="bytesize">Requested allocation size in bytes</param>
        /// <param name="flags">Flags for allocation request</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_OUT_OF_MEMORY
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D16,
        /// ::cuMemsetD2D32, ::cuMemsetD8, ::cuMemsetD16, ::cuMemsetD32,
        /// ::cudaHostAlloc
        /// CUresult CUDAAPI cuMemHostAlloc(void **pp, size_t bytesize, unsigned int Flags);
        [DllImport(_dllpath, EntryPoint = "cuMemHostAlloc")]
        public static extern CuResult MemHostAlloc(out CuHostMemory pp, IntPtr bytesize, MemHostAllocFlags flags);

        /// <summary>Passes back device pointer of mapped pinned memory
        ///
        /// Passes back the device pointer <paramref name="pdptr"/> corresponding to the mapped, pinned
        /// host buffer <paramref name="p"/> allocated by ::cuMemHostAlloc.
        ///
        /// ::cuMemHostGetDevicePointer() will fail if the ::CU_MEMHOSTALLOC_DEVICEMAP
        /// flag was not specified at the time the memory was allocated, or if the
        /// function is called on a GPU that does not support mapped pinned memory.
        ///
        /// For devices that have a non-zero value for the device attribute
        /// ::CU_DEVICE_ATTRIBUTE_CAN_USE_HOST_POINTER_FOR_REGISTERED_MEM, the memory
        /// can also be accessed from the device using the host pointer <paramref name="p"/>.
        /// The device pointer returned by ::cuMemHostGetDevicePointer() may or may not
        /// match the original host pointer <paramref name="p"/> and depends on the devices visible to the
        /// application. If all devices visible to the application have a non-zero value for the
        /// device attribute, the device pointer returned by ::cuMemHostGetDevicePointer()
        /// will match the original pointer <paramref name="p"/>. If any device visible to the application
        /// has a zero value for the device attribute, the device pointer returned by
        /// ::cuMemHostGetDevicePointer() will not match the original host pointer <paramref name="p"/>,
        /// but it will be suitable for use on all devices provided Unified Virtual Addressing
        /// is enabled. In such systems, it is valid to access the memory using either pointer
        /// on devices that have a non-zero value for the device attribute. Note however that
        /// such devices should access the memory using only of the two pointers and not both.
        ///
        /// <paramref name="flags"/> provides for future releases. For now, it must be set to 0.</summary>
        ///
        /// <param name="pdptr">Returned device pointer</param>
        /// <param name="p">Host pointer</param>
        /// <param name="flags">Options (must be 0)</param>
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
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemsetD2D8, ::cuMemsetD2D16,
        /// ::cuMemsetD2D32, ::cuMemsetD8, ::cuMemsetD16, ::cuMemsetD32,
        /// ::cudaHostGetDevicePointer
        /// CUresult CUDAAPI cuMemHostGetDevicePointer(CUdeviceptr *pdptr, void *p, unsigned int Flags);
        [DllImport(_dllpath, EntryPoint = "cuMemHostGetDevicePointer")]
        public static extern CuResult MemHostGetDevicePointer(out CuDevicePtr pdptr, CuHostMemory p, int flags = 0);

        /// <summary>Passes back flags that were used for a pinned allocation
        ///
        /// Passes back the flags <paramref name="pFlags"/> that were specified when allocating
        /// the pinned host buffer <paramref name="p"/> allocated by ::cuMemHostAlloc.
        ///
        /// ::cuMemHostGetFlags() will fail if the pointer does not reside in
        /// an allocation performed by ::cuMemAllocHost() or ::cuMemHostAlloc().</summary>
        ///
        /// <param name="pFlags">Returned flags word</param>
        /// <param name="p">Host pointer</param>
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
        /// \sa
        /// ::cuMemAllocHost,
        /// ::cuMemHostAlloc,
        /// ::cudaHostGetFlags
        /// CUresult CUDAAPI cuMemHostGetFlags(unsigned int *pFlags, void *p);
        [DllImport(_dllpath, EntryPoint = "cuMemHostGetFlags")]
        public static extern CuResult MemHostGetFlags(out MemHostAllocFlags pFlags, CuHostMemory p);

        /// <summary>Allocates memory that will be automatically managed by the Unified Memory system
        ///
        /// Allocates <paramref name="bytesize"/> bytes of managed memory on the device and returns in
        /// *<paramref name="dptr"/> a pointer to the allocated memory. If the device doesn't support
        /// allocating managed memory, ::CUDA_ERROR_NOT_SUPPORTED is returned. Support
        /// for managed memory can be queried using the device attribute
        /// ::CU_DEVICE_ATTRIBUTE_MANAGED_MEMORY. The allocated memory is suitably
        /// aligned for any kind of variable. The memory is not cleared. If <paramref name="bytesize"/>
        /// is 0, ::cuMemAllocManaged returns ::CUDA_ERROR_INVALID_VALUE. The pointer
        /// is valid on the CPU and on all GPUs in the system that support managed memory.
        /// All accesses to this pointer must obey the Unified Memory programming model.
        ///
        /// <paramref name="flags"/> specifies the default stream association for this allocation.
        /// <paramref name="flags"/> must be one of ::CU_MEM_ATTACH_GLOBAL or ::CU_MEM_ATTACH_HOST. If
        /// ::CU_MEM_ATTACH_GLOBAL is specified, then this memory is accessible from
        /// any stream on any device. If ::CU_MEM_ATTACH_HOST is specified, then the
        /// allocation should not be accessed from devices that have a zero value for the
        /// device attribute ::CU_DEVICE_ATTRIBUTE_CONCURRENT_MANAGED_ACCESS; an explicit call to
        /// ::cuStreamAttachMemAsync will be required to enable access on such devices.
        ///
        /// If the association is later changed via ::cuStreamAttachMemAsync to
        /// a single stream, the default association as specifed during ::cuMemAllocManaged
        /// is restored when that stream is destroyed. For __managed__ variables, the
        /// default association is always ::CU_MEM_ATTACH_GLOBAL. Note that destroying a
        /// stream is an asynchronous operation, and as a result, the change to default
        /// association won't happen until all work in the stream has completed.
        ///
        /// Memory allocated with ::cuMemAllocManaged should be released with ::cuMemFree.
        ///
        /// Device memory oversubscription is possible for GPUs that have a non-zero value for the
        /// device attribute ::CU_DEVICE_ATTRIBUTE_CONCURRENT_MANAGED_ACCESS. Managed memory on
        /// such GPUs may be evicted from device memory to host memory at any time by the Unified
        /// Memory driver in order to make room for other allocations.
        ///
        /// In a multi-GPU system where all GPUs have a non-zero value for the device attribute
        /// ::CU_DEVICE_ATTRIBUTE_CONCURRENT_MANAGED_ACCESS, managed memory may not be populated when this
        /// API returns and instead may be populated on access. In such systems, managed memory can
        /// migrate to any processor's memory at any time. The Unified Memory driver will employ heuristics to
        /// maintain data locality and prevent excessive page faults to the extent possible. The application
        /// can also guide the driver about memory usage patterns via ::cuMemAdvise. The application
        /// can also explicitly migrate memory to a desired processor's memory via
        /// ::cuMemPrefetchAsync.
        ///
        /// In a multi-GPU system where all of the GPUs have a zero value for the device attribute
        /// ::CU_DEVICE_ATTRIBUTE_CONCURRENT_MANAGED_ACCESS and all the GPUs have peer-to-peer support
        /// with each other, the physical storage for managed memory is created on the GPU which is active
        /// at the time ::cuMemAllocManaged is called. All other GPUs will reference the data at reduced
        /// bandwidth via peer mappings over the PCIe bus. The Unified Memory driver does not migrate
        /// memory among such GPUs.
        ///
        /// In a multi-GPU system where not all GPUs have peer-to-peer support with each other and
        /// where the value of the device attribute ::CU_DEVICE_ATTRIBUTE_CONCURRENT_MANAGED_ACCESS
        /// is zero for at least one of those GPUs, the location chosen for physical storage of managed
        /// memory is system-dependent.
        /// - On Linux, the location chosen will be device memory as long as the current set of active
        /// contexts are on devices that either have peer-to-peer support with each other or have a
        /// non-zero value for the device attribute ::CU_DEVICE_ATTRIBUTE_CONCURRENT_MANAGED_ACCESS.
        /// If there is an active context on a GPU that does not have a non-zero value for that device
        /// attribute and it does not have peer-to-peer support with the other devices that have active
        /// contexts on them, then the location for physical storage will be 'zero-copy' or host memory.
        /// Note that this means that managed memory that is located in device memory is migrated to
        /// host memory if a new context is created on a GPU that doesn't have a non-zero value for
        /// the device attribute and does not support peer-to-peer with at least one of the other devices
        /// that has an active context. This in turn implies that context creation may fail if there is
        /// insufficient host memory to migrate all managed allocations.
        /// - On Windows, the physical storage is always created in 'zero-copy' or host memory.
        /// All GPUs will reference the data at reduced bandwidth over the PCIe bus. In these
        /// circumstances, use of the environment variable CUDA_VISIBLE_DEVICES is recommended to
        /// restrict CUDA to only use those GPUs that have peer-to-peer support.
        /// Alternatively, users can also set CUDA_MANAGED_FORCE_DEVICE_ALLOC to a
        /// non-zero value to force the driver to always use device memory for physical storage.
        /// When this environment variable is set to a non-zero value, all contexts created in
        /// that process on devices that support managed memory have to be peer-to-peer compatible
        /// with each other. Context creation will fail if a context is created on a device that
        /// supports managed memory and is not peer-to-peer compatible with any of the other
        /// managed memory supporting devices on which contexts were previously created, even if
        /// those contexts have been destroyed. These environment variables are described
        /// in the CUDA programming guide under the "CUDA environment variables" section.
        /// - On ARM, managed memory is not available on discrete gpu with Drive PX-2.</summary>
        ///
        /// <param name="dptr">Returned device pointer</param>
        /// <param name="bytesize">Requested allocation size in bytes</param>
        /// <param name="flags">Must be one of ::CU_MEM_ATTACH_GLOBAL or ::CU_MEM_ATTACH_HOST</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_NOT_SUPPORTED,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_OUT_OF_MEMORY
        /// </returns>
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
        /// ::cuDeviceGetAttribute, ::cuStreamAttachMemAsync,
        /// ::cudaMallocManaged
        /// CUresult CUDAAPI cuMemAllocManaged(CUdeviceptr *dptr, size_t bytesize, unsigned int flags);
        [DllImport(_dllpath, EntryPoint = "cuMemAllocManaged")]
        public static extern CuResult MemAllocManaged(out CuDevicePtr dptr, IntPtr bytesize, MemoryAttachFlags flags);

        /// <summary>Registers an existing host memory range for use by CUDA
        ///
        /// Page-locks the memory range specified by <paramref name="p"/> and <paramref name="bytesize"/> and maps it
        /// for the device(s) as specified by <paramref name="Flags"/>. This memory range also is added
        /// to the same tracking mechanism as ::cuMemHostAlloc to automatically accelerate
        /// calls to functions such as ::cuMemcpyHtoD(). Since the memory can be accessed
        /// directly by the device, it can be read or written with much higher bandwidth
        /// than pageable memory that has not been registered.  Page-locking excessive
        /// amounts of memory may degrade system performance, since it reduces the amount
        /// of memory available to the system for paging. As a result, this function is
        /// best used sparingly to register staging areas for data exchange between
        /// host and device.
        ///
        /// This function has limited support on Mac OS X. OS 10.7 or higher is required.
        ///
        /// The <paramref name="Flags"/> parameter enables different options to be specified that
        /// affect the allocation, as follows.
        ///
        /// - ::CU_MEMHOSTREGISTER_PORTABLE: The memory returned by this call will be
        ///   considered as pinned memory by all CUDA contexts, not just the one that
        ///   performed the allocation.
        ///
        /// - ::CU_MEMHOSTREGISTER_DEVICEMAP: Maps the allocation into the CUDA address
        ///   space. The device pointer to the memory may be obtained by calling
        ///   ::cuMemHostGetDevicePointer().
        ///
        /// - ::CU_MEMHOSTREGISTER_IOMEMORY: The pointer is treated as pointing to some
        ///   I/O memory space, e.g. the PCI Express resource of a 3rd party device.
        ///
        /// All of these flags are orthogonal to one another: a developer may page-lock
        /// memory that is portable or mapped with no restrictions.
        ///
        /// The CUDA context must have been created with the ::CU_CTX_MAP_HOST flag in
        /// order for the ::CU_MEMHOSTREGISTER_DEVICEMAP flag to have any effect.
        ///
        /// The ::CU_MEMHOSTREGISTER_DEVICEMAP flag may be specified on CUDA contexts for
        /// devices that do not support mapped pinned memory. The failure is deferred
        /// to ::cuMemHostGetDevicePointer() because the memory may be mapped into
        /// other CUDA contexts via the ::CU_MEMHOSTREGISTER_PORTABLE flag.
        ///
        /// For devices that have a non-zero value for the device attribute
        /// ::CU_DEVICE_ATTRIBUTE_CAN_USE_HOST_POINTER_FOR_REGISTERED_MEM, the memory
        /// can also be accessed from the device using the host pointer <paramref name="p"/>.
        /// The device pointer returned by ::cuMemHostGetDevicePointer() may or may not
        /// match the original host pointer <paramref name="p"/> and depends on the devices visible to the
        /// application. If all devices visible to the application have a non-zero value for the
        /// device attribute, the device pointer returned by ::cuMemHostGetDevicePointer()
        /// will match the original pointer <paramref name="p"/>. If any device visible to the application
        /// has a zero value for the device attribute, the device pointer returned by
        /// ::cuMemHostGetDevicePointer() will not match the original host pointer <paramref name="p"/>,
        /// but it will be suitable for use on all devices provided Unified Virtual Addressing
        /// is enabled. In such systems, it is valid to access the memory using either pointer
        /// on devices that have a non-zero value for the device attribute. Note however that
        /// such devices should access the memory using only of the two pointers and not both.
        ///
        /// The memory page-locked by this function must be unregistered with
        /// ::cuMemHostUnregister().</summary>
        ///
        /// <param name="p">Host pointer to memory to page-lock</param>
        /// <param name="bytesize">Size in bytes of the address range to page-lock</param>
        /// <param name="Flags">Flags for allocation request</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_OUT_OF_MEMORY,
        /// ::CUDA_ERROR_HOST_MEMORY_ALREADY_REGISTERED,
        /// ::CUDA_ERROR_NOT_PERMITTED,
        /// ::CUDA_ERROR_NOT_SUPPORTED
        /// </returns>
        /// \notefnerr
        ///
        /// \sa
        /// ::cuMemHostUnregister,
        /// ::cuMemHostGetFlags,
        /// ::cuMemHostGetDevicePointer,
        /// ::cudaHostRegister
        /// CUresult CUDAAPI cuMemHostRegister(void *p, size_t bytesize, unsigned int Flags);
        [DllImport(_dllpath, EntryPoint = "cuMemHostRegister")]
        public static extern CuResult MemHostRegister(CuHostMemory p, IntPtr bytesize, int Flags);

        /// <summary>Unregisters a memory range that was registered with cuMemHostRegister.
        ///
        /// Unmaps the memory range whose base address is specified by <paramref name="p"/>, and makes
        /// it pageable again.
        ///
        /// The base address must be the same one specified to ::cuMemHostRegister().</summary>
        ///
        /// <param name="p">Host pointer to memory to unregister</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_OUT_OF_MEMORY,
        /// ::CUDA_ERROR_HOST_MEMORY_NOT_REGISTERED,
        /// </returns>
        /// \notefnerr
        ///
        /// \sa
        /// ::cuMemHostRegister,
        /// ::cudaHostUnregister
        /// CUresult CUDAAPI cuMemHostUnregister(void *p);
        [DllImport(_dllpath, EntryPoint = "cuMemHostUnregister")]
        public static extern CuResult MemHostUnregister(void* p);

        #region Array
        /// <summary>Creates a 1D or 2D CUDA array
        ///
        /// Creates a CUDA array according to the ::CUDA_ARRAY_DESCRIPTOR structure
        /// <paramref name="pAllocateArray"/> and returns a handle to the new CUDA array in *<paramref name="pHandle"/>.
        /// where:
        ///
        /// - <c>Width</c>, and <c>Height</c> are the width, and height of the CUDA array (in
        /// elements); the CUDA array is one-dimensional if height is 0, two-dimensional
        /// otherwise;
        /// - ::Format specifies the format of the elements;
        /// - <c>NumChannels</c> specifies the number of packed components per CUDA array
        /// element; it may be 1, 2, or 4;
        ///
        /// Here are examples of CUDA array descriptions:
        ///
        /// Description for a CUDA array of 2048 floats:
        /// <code>
        /// CUDA_ARRAY_DESCRIPTOR desc;
        /// desc.Format = CU_AD_FORMAT_FLOAT;
        /// desc.NumChannels = 1;
        /// desc.Width = 2048;
        /// desc.Height = 1;
        /// </code>
        ///
        /// Description for a 64 x 64 CUDA array of floats:
        /// <code>
        /// CUDA_ARRAY_DESCRIPTOR desc;
        /// desc.Format = CU_AD_FORMAT_FLOAT;
        /// desc.NumChannels = 1;
        /// desc.Width = 64;
        /// desc.Height = 64;
        /// </code>
        ///
        /// Description for a <c>Width</c> x <c>Height</c> CUDA array of 64-bit, 4x16-bit
        /// float16's:
        /// <code>
        /// CUDA_ARRAY_DESCRIPTOR desc;
        /// desc.FormatFlags = CU_AD_FORMAT_HALF;
        /// desc.NumChannels = 4;
        /// desc.Width = width;
        /// desc.Height = height;
        /// </code>
        ///
        /// Description for a <c>Width</c> x <c>Height</c> CUDA array of 16-bit elements, each
        /// of which is two 8-bit unsigned chars:
        /// <code>
        /// CUDA_ARRAY_DESCRIPTOR arrayDesc;
        /// desc.FormatFlags = CU_AD_FORMAT_UNSIGNED_INT8;
        /// desc.NumChannels = 2;
        /// desc.Width = width;
        /// desc.Height = height;
        /// </code></summary>
        ///
        /// <param name="pHandle">Returned array</param>
        /// <param name="pAllocateArray">Array descriptor</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_OUT_OF_MEMORY,
        /// ::CUDA_ERROR_UNKNOWN
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D16,
        /// ::cuMemsetD2D32, ::cuMemsetD8, ::cuMemsetD16, ::cuMemsetD32,
        /// ::cudaMallocArray
        /// CUresult CUDAAPI cuArrayCreate(CUarray *pHandle, const CUDA_ARRAY_DESCRIPTOR *pAllocateArray);
        [DllImport(_dllpath, EntryPoint = "cuArrayCreate")]
        public static extern CuResult ArrayCreate(out CuArray pHandle, ref CuArrayDescription pAllocateArray);

        /// <summary>Get a 1D or 2D CUDA array descriptor
        ///
        /// Returns in *<paramref name="pArrayDescriptor"/> a descriptor containing information on the
        /// format and dimensions of the CUDA array <paramref name="hArray"/>. It is useful for
        /// subroutines that have been passed a CUDA array, but need to know the CUDA
        /// array parameters for validation or other purposes.</summary>
        ///
        /// <param name="pArrayDescriptor">Returned array descriptor</param>
        /// <param name="hArray">Array to get descriptor of</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_INVALID_HANDLE
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D16,
        /// ::cuMemsetD2D32, ::cuMemsetD8, ::cuMemsetD16, ::cuMemsetD32,
        /// ::cudaArrayGetInfo
        /// CUresult CUDAAPI cuArrayGetDescriptor(CUDA_ARRAY_DESCRIPTOR *pArrayDescriptor, CUarray hArray);
        [DllImport(_dllpath, EntryPoint = "cuArrayGetDescriptor")]
        public static extern CuResult ArrayGetDescriptor(out CuArrayDescription pArrayDescriptor, CuArray hArray);

        /// <summary>Destroys a CUDA array
        ///
        /// Destroys the CUDA array <paramref name="hArray"/>.</summary>
        ///
        /// <param name="hArray">Array to destroy</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_ARRAY_IS_MAPPED
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D16,
        /// ::cuMemsetD2D32, ::cuMemsetD8, ::cuMemsetD16, ::cuMemsetD32,
        /// ::cudaFreeArray
        /// CUresult CUDAAPI cuArrayDestroy(CUarray hArray);
        [DllImport(_dllpath, EntryPoint = "cuArrayDestroy")]
        public static extern CuResult ArrayDestroy(CuArray hArray);

        /// <summary>Creates a 3D CUDA array
        ///
        /// Creates a CUDA array according to the ::CUDA_ARRAY3D_DESCRIPTOR structure
        /// <paramref name="pAllocateArray"/> and returns a handle to the new CUDA array in *<paramref name="pHandle"/>.
        /// where:
        ///
        /// - <c>Width</c>, <c>Height</c>, and <c>Depth</c> are the width, height, and depth of the
        /// CUDA array (in elements); the following types of CUDA arrays can be allocated:
        ///     - A 1D array is allocated if <c>Height</c> and <c>Depth</c> extents are both zero.
        ///     - A 2D array is allocated if only <c>Depth</c> extent is zero.
        ///     - A 3D array is allocated if all three extents are non-zero.
        ///     - A 1D layered CUDA array is allocated if only <c>Height</c> is zero and the
        ///       ::CUDA_ARRAY3D_LAYERED flag is set. Each layer is a 1D array. The number
        ///       of layers is determined by the depth extent.
        ///     - A 2D layered CUDA array is allocated if all three extents are non-zero and
        ///       the ::CUDA_ARRAY3D_LAYERED flag is set. Each layer is a 2D array. The number
        ///       of layers is determined by the depth extent.
        ///     - A cubemap CUDA array is allocated if all three extents are non-zero and the
        ///       ::CUDA_ARRAY3D_CUBEMAP flag is set. <c>Width</c> must be equal to <c>Height</c>, and
        ///       <c>Depth</c> must be six. A cubemap is a special type of 2D layered CUDA array,
        ///       where the six layers represent the six faces of a cube. The order of the six
        ///       layers in memory is the same as that listed in ::CUarray_cubemap_face.
        ///     - A cubemap layered CUDA array is allocated if all three extents are non-zero,
        ///       and both, ::CUDA_ARRAY3D_CUBEMAP and ::CUDA_ARRAY3D_LAYERED flags are set.
        ///       <c>Width</c> must be equal to <c>Height</c>, and <c>Depth</c> must be a multiple of six.
        ///       A cubemap layered CUDA array is a special type of 2D layered CUDA array that
        ///       consists of a collection of cubemaps. The first six layers represent the first
        ///       cubemap, the next six layers form the second cubemap, and so on.
        ///
        /// - <c>NumChannels</c> specifies the number of packed components per CUDA array
        /// element; it may be 1, 2, or 4;
        ///
        /// - ::Flags may be set to
        ///   - ::CUDA_ARRAY3D_LAYERED to enable creation of layered CUDA arrays. If this flag is set,
        ///     <c>Depth</c> specifies the number of layers, not the depth of a 3D array.
        ///   - ::CUDA_ARRAY3D_SURFACE_LDST to enable surface references to be bound to the CUDA array.
        ///     If this flag is not set, ::cuSurfRefSetArray will fail when attempting to bind the CUDA array
        ///     to a surface reference.
        ///   - ::CUDA_ARRAY3D_CUBEMAP to enable creation of cubemaps. If this flag is set, <c>Width</c> must be
        ///     equal to <c>Height</c>, and <c>Depth</c> must be six. If the ::CUDA_ARRAY3D_LAYERED flag is also set,
        ///     then <c>Depth</c> must be a multiple of six.
        ///   - ::CUDA_ARRAY3D_TEXTURE_GATHER to indicate that the CUDA array will be used for texture gather.
        ///     Texture gather can only be performed on 2D CUDA arrays.
        ///
        /// <c>Width</c>, <c>Height</c> and <c>Depth</c> must meet certain size requirements as listed in the following table.
        /// All values are specified in elements. Note that for brevity's sake, the full name of the device attribute
        /// is not specified. For ex., TEXTURE1D_WIDTH refers to the device attribute
        /// ::CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE1D_WIDTH.
        ///
        /// Note that 2D CUDA arrays have different size requirements if the ::CUDA_ARRAY3D_TEXTURE_GATHER flag
        /// is set. <c>Width</c> and <c>Height</c> must not be greater than ::CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_GATHER_WIDTH
        /// and ::CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_GATHER_HEIGHT respectively, in that case.
        ///
        /// <table>
        /// <tr><td><b>CUDA array type</b></td>
        /// <td><b>Valid extents that must always be met {(width range in elements), (height range),
        /// (depth range)}</b></td>
        /// <td><b>Valid extents with CUDA_ARRAY3D_SURFACE_LDST set
        /// {(width range in elements), (height range), (depth range)}</b></td></tr>
        /// <tr><td>1D</td>
        /// <td><small>{ (1,TEXTURE1D_WIDTH), 0, 0 }</small></td>
        /// <td><small>{ (1,SURFACE1D_WIDTH), 0, 0 }</small></td></tr>
        /// <tr><td>2D</td>
        /// <td><small>{ (1,TEXTURE2D_WIDTH), (1,TEXTURE2D_HEIGHT), 0 }</small></td>
        /// <td><small>{ (1,SURFACE2D_WIDTH), (1,SURFACE2D_HEIGHT), 0 }</small></td></tr>
        /// <tr><td>3D</td>
        /// <td><small>{ (1,TEXTURE3D_WIDTH), (1,TEXTURE3D_HEIGHT), (1,TEXTURE3D_DEPTH) }
        ///  OR { (1,TEXTURE3D_WIDTH_ALTERNATE), (1,TEXTURE3D_HEIGHT_ALTERNATE),
        /// (1,TEXTURE3D_DEPTH_ALTERNATE) }</small></td>
        /// <td><small>{ (1,SURFACE3D_WIDTH), (1,SURFACE3D_HEIGHT),
        /// (1,SURFACE3D_DEPTH) }</small></td></tr>
        /// <tr><td>1D Layered</td>
        /// <td><small>{ (1,TEXTURE1D_LAYERED_WIDTH), 0,
        /// (1,TEXTURE1D_LAYERED_LAYERS) }</small></td>
        /// <td><small>{ (1,SURFACE1D_LAYERED_WIDTH), 0,
        /// (1,SURFACE1D_LAYERED_LAYERS) }</small></td></tr>
        /// <tr><td>2D Layered</td>
        /// <td><small>{ (1,TEXTURE2D_LAYERED_WIDTH), (1,TEXTURE2D_LAYERED_HEIGHT),
        /// (1,TEXTURE2D_LAYERED_LAYERS) }</small></td>
        /// <td><small>{ (1,SURFACE2D_LAYERED_WIDTH), (1,SURFACE2D_LAYERED_HEIGHT),
        /// (1,SURFACE2D_LAYERED_LAYERS) }</small></td></tr>
        /// <tr><td>Cubemap</td>
        /// <td><small>{ (1,TEXTURECUBEMAP_WIDTH), (1,TEXTURECUBEMAP_WIDTH), 6 }</small></td>
        /// <td><small>{ (1,SURFACECUBEMAP_WIDTH),
        /// (1,SURFACECUBEMAP_WIDTH), 6 }</small></td></tr>
        /// <tr><td>Cubemap Layered</td>
        /// <td><small>{ (1,TEXTURECUBEMAP_LAYERED_WIDTH), (1,TEXTURECUBEMAP_LAYERED_WIDTH),
        /// (1,TEXTURECUBEMAP_LAYERED_LAYERS) }</small></td>
        /// <td><small>{ (1,SURFACECUBEMAP_LAYERED_WIDTH), (1,SURFACECUBEMAP_LAYERED_WIDTH),
        /// (1,SURFACECUBEMAP_LAYERED_LAYERS) }</small></td></tr>
        /// </table>
        ///
        /// Here are examples of CUDA array descriptions:
        ///
        /// Description for a CUDA array of 2048 floats:
        /// <code>
        /// CUDA_ARRAY3D_DESCRIPTOR desc;
        /// desc.Format = CU_AD_FORMAT_FLOAT;
        /// desc.NumChannels = 1;
        /// desc.Width = 2048;
        /// desc.Height = 0;
        /// desc.Depth = 0;
        /// </code>
        ///
        /// Description for a 64 x 64 CUDA array of floats:
        /// <code>
        /// CUDA_ARRAY3D_DESCRIPTOR desc;
        /// desc.Format = CU_AD_FORMAT_FLOAT;
        /// desc.NumChannels = 1;
        /// desc.Width = 64;
        /// desc.Height = 64;
        /// desc.Depth = 0;
        /// </code>
        ///
        /// Description for a <c>Width</c> x <c>Height</c> x <c>Depth</c> CUDA array of 64-bit,
        /// 4x16-bit float16's:
        /// <code>
        /// CUDA_ARRAY3D_DESCRIPTOR desc;
        /// desc.FormatFlags = CU_AD_FORMAT_HALF;
        /// desc.NumChannels = 4;
        /// desc.Width = width;
        /// desc.Height = height;
        /// desc.Depth = depth;
        /// </code></summary>
        ///
        /// <param name="pHandle">Returned array</param>
        /// <param name="pAllocateArray">3D array descriptor</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_OUT_OF_MEMORY,
        /// ::CUDA_ERROR_UNKNOWN
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D16,
        /// ::cuMemsetD2D32, ::cuMemsetD8, ::cuMemsetD16, ::cuMemsetD32,
        /// ::cudaMalloc3DArray
        /// CUresult CUDAAPI cuArray3DCreate(CUarray *pHandle, const CUDA_ARRAY3D_DESCRIPTOR *pAllocateArray);
        [DllImport(_dllpath, EntryPoint = "cuArray3DCreate")]
        public static extern CuResult Array3DCreate(out CuArray pHandle, ref CuArray3DDescription pAllocateArray);

        /// <summary>Get a 3D CUDA array descriptor
        ///
        /// Returns in *<paramref name="pArrayDescriptor"/> a descriptor containing information on the
        /// format and dimensions of the CUDA array <paramref name="hArray"/>. It is useful for
        /// subroutines that have been passed a CUDA array, but need to know the CUDA
        /// array parameters for validation or other purposes.
        ///
        /// This function may be called on 1D and 2D arrays, in which case the  <c>Height</c>
        /// and/or <c>depth</c> members of the descriptor struct will be set to 0.</summary>
        ///
        /// <param name="pArrayDescriptor">Returned 3D array descriptor</param>
        /// <param name="hArray">3D array to get descriptor of</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_INVALID_HANDLE
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuArray3DCreate, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D16,
        /// ::cuMemsetD2D32, ::cuMemsetD8, ::cuMemsetD16, ::cuMemsetD32,
        /// ::cudaArrayGetInfo
        /// CUresult CUDAAPI cuArray3DGetDescriptor(CUDA_ARRAY3D_DESCRIPTOR *pArrayDescriptor, CUarray hArray);
        [DllImport(_dllpath, EntryPoint = "cuArray3DGetDescriptor")]
        public static extern CuResult Array3DGetDescriptor(out CuArray3DDescription pArrayDescriptor, CuArray hArray);

        /// <summary>Creates a CUDA mipmapped array
        ///
        /// Creates a CUDA mipmapped array according to the ::CUDA_ARRAY3D_DESCRIPTOR structure
        /// <paramref name="pMipmappedArrayDesc"/> and returns a handle to the new CUDA mipmapped array in *<paramref name="pHandle"/>.
        /// <paramref name="numMipmapLevels"/> specifies the number of mipmap levels to be allocated. This value is
        /// clamped to the range [1, 1 + floor(log2(max(width, height, depth)))].
        ///
        /// where:
        ///
        /// - <c>Width</c>, <c>Height</c>, and <c>Depth</c> are the width, height, and depth of the
        /// CUDA array (in elements); the following types of CUDA arrays can be allocated:
        ///     - A 1D mipmapped array is allocated if <c>Height</c> and <c>Depth</c> extents are both zero.
        ///     - A 2D mipmapped array is allocated if only <c>Depth</c> extent is zero.
        ///     - A 3D mipmapped array is allocated if all three extents are non-zero.
        ///     - A 1D layered CUDA mipmapped array is allocated if only <c>Height</c> is zero and the
        ///       ::CUDA_ARRAY3D_LAYERED flag is set. Each layer is a 1D array. The number
        ///       of layers is determined by the depth extent.
        ///     - A 2D layered CUDA mipmapped array is allocated if all three extents are non-zero and
        ///       the ::CUDA_ARRAY3D_LAYERED flag is set. Each layer is a 2D array. The number
        ///       of layers is determined by the depth extent.
        ///     - A cubemap CUDA mipmapped array is allocated if all three extents are non-zero and the
        ///       ::CUDA_ARRAY3D_CUBEMAP flag is set. <c>Width</c> must be equal to <c>Height</c>, and
        ///       <c>Depth</c> must be six. A cubemap is a special type of 2D layered CUDA array,
        ///       where the six layers represent the six faces of a cube. The order of the six
        ///       layers in memory is the same as that listed in ::CUarray_cubemap_face.
        ///     - A cubemap layered CUDA mipmapped array is allocated if all three extents are non-zero,
        ///       and both, ::CUDA_ARRAY3D_CUBEMAP and ::CUDA_ARRAY3D_LAYERED flags are set.
        ///       <c>Width</c> must be equal to <c>Height</c>, and <c>Depth</c> must be a multiple of six.
        ///       A cubemap layered CUDA array is a special type of 2D layered CUDA array that
        ///       consists of a collection of cubemaps. The first six layers represent the first
        ///       cubemap, the next six layers form the second cubemap, and so on.
        ///
        ///
        /// - <c>NumChannels</c> specifies the number of packed components per CUDA array
        /// element; it may be 1, 2, or 4;
        ///
        /// - ::Flags may be set to
        ///   - ::CUDA_ARRAY3D_LAYERED to enable creation of layered CUDA mipmapped arrays. If this flag is set,
        ///     <c>Depth</c> specifies the number of layers, not the depth of a 3D array.
        ///   - ::CUDA_ARRAY3D_SURFACE_LDST to enable surface references to be bound to individual mipmap levels of
        ///     the CUDA mipmapped array. If this flag is not set, ::cuSurfRefSetArray will fail when attempting to
        ///     bind a mipmap level of the CUDA mipmapped array to a surface reference.
        ///   - ::CUDA_ARRAY3D_CUBEMAP to enable creation of mipmapped cubemaps. If this flag is set, <c>Width</c> must be
        ///     equal to <c>Height</c>, and <c>Depth</c> must be six. If the ::CUDA_ARRAY3D_LAYERED flag is also set,
        ///     then <c>Depth</c> must be a multiple of six.
        ///   - ::CUDA_ARRAY3D_TEXTURE_GATHER to indicate that the CUDA mipmapped array will be used for texture gather.
        ///     Texture gather can only be performed on 2D CUDA mipmapped arrays.
        ///
        /// <c>Width</c>, <c>Height</c> and <c>Depth</c> must meet certain size requirements as listed in the following table.
        /// All values are specified in elements. Note that for brevity's sake, the full name of the device attribute
        /// is not specified. For ex., TEXTURE1D_MIPMAPPED_WIDTH refers to the device attribute
        /// ::CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE1D_MIPMAPPED_WIDTH.
        ///
        /// <table>
        /// <tr><td><b>CUDA array type</b></td>
        /// <td><b>Valid extents that must always be met {(width range in elements), (height range),
        /// (depth range)}</b></td>
        /// <td><b>Valid extents with CUDA_ARRAY3D_SURFACE_LDST set
        /// {(width range in elements), (height range), (depth range)}</b></td></tr>
        /// <tr><td>1D</td>
        /// <td><small>{ (1,TEXTURE1D_MIPMAPPED_WIDTH), 0, 0 }</small></td>
        /// <td><small>{ (1,SURFACE1D_WIDTH), 0, 0 }</small></td></tr>
        /// <tr><td>2D</td>
        /// <td><small>{ (1,TEXTURE2D_MIPMAPPED_WIDTH), (1,TEXTURE2D_MIPMAPPED_HEIGHT), 0 }</small></td>
        /// <td><small>{ (1,SURFACE2D_WIDTH), (1,SURFACE2D_HEIGHT), 0 }</small></td></tr>
        /// <tr><td>3D</td>
        /// <td><small>{ (1,TEXTURE3D_WIDTH), (1,TEXTURE3D_HEIGHT), (1,TEXTURE3D_DEPTH) }
        ///  OR { (1,TEXTURE3D_WIDTH_ALTERNATE), (1,TEXTURE3D_HEIGHT_ALTERNATE),
        /// (1,TEXTURE3D_DEPTH_ALTERNATE) }</small></td>
        /// <td><small>{ (1,SURFACE3D_WIDTH), (1,SURFACE3D_HEIGHT),
        /// (1,SURFACE3D_DEPTH) }</small></td></tr>
        /// <tr><td>1D Layered</td>
        /// <td><small>{ (1,TEXTURE1D_LAYERED_WIDTH), 0,
        /// (1,TEXTURE1D_LAYERED_LAYERS) }</small></td>
        /// <td><small>{ (1,SURFACE1D_LAYERED_WIDTH), 0,
        /// (1,SURFACE1D_LAYERED_LAYERS) }</small></td></tr>
        /// <tr><td>2D Layered</td>
        /// <td><small>{ (1,TEXTURE2D_LAYERED_WIDTH), (1,TEXTURE2D_LAYERED_HEIGHT),
        /// (1,TEXTURE2D_LAYERED_LAYERS) }</small></td>
        /// <td><small>{ (1,SURFACE2D_LAYERED_WIDTH), (1,SURFACE2D_LAYERED_HEIGHT),
        /// (1,SURFACE2D_LAYERED_LAYERS) }</small></td></tr>
        /// <tr><td>Cubemap</td>
        /// <td><small>{ (1,TEXTURECUBEMAP_WIDTH), (1,TEXTURECUBEMAP_WIDTH), 6 }</small></td>
        /// <td><small>{ (1,SURFACECUBEMAP_WIDTH),
        /// (1,SURFACECUBEMAP_WIDTH), 6 }</small></td></tr>
        /// <tr><td>Cubemap Layered</td>
        /// <td><small>{ (1,TEXTURECUBEMAP_LAYERED_WIDTH), (1,TEXTURECUBEMAP_LAYERED_WIDTH),
        /// (1,TEXTURECUBEMAP_LAYERED_LAYERS) }</small></td>
        /// <td><small>{ (1,SURFACECUBEMAP_LAYERED_WIDTH), (1,SURFACECUBEMAP_LAYERED_WIDTH),
        /// (1,SURFACECUBEMAP_LAYERED_LAYERS) }</small></td></tr>
        /// </table>
        ///</summary>
        ///
        /// <param name="pHandle">Returned mipmapped array</param>
        /// <param name="pMipmappedArrayDesc">mipmapped array descriptor</param>
        /// <param name="numMipmapLevels">Number of mipmap levels</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_OUT_OF_MEMORY,
        /// ::CUDA_ERROR_UNKNOWN
        /// </returns>
        /// \notefnerr
        ///
        /// \sa
        /// ::cuMipmappedArrayDestroy,
        /// ::cuMipmappedArrayGetLevel,
        /// ::cuArrayCreate,
        /// ::cudaMallocMipmappedArray
        /// CUresult CUDAAPI cuMipmappedArrayCreate(CUmipmappedArray *pHandle, const CUDA_ARRAY3D_DESCRIPTOR *pMipmappedArrayDesc, unsigned int numMipmapLevels);
        [DllImport(_dllpath, EntryPoint = "cuMipmappedArrayCreate")]
        public static extern CuResult MipmappedArrayCreate(out CuMipMappedArray pHandle, ref CuArray3DDescription pMipmappedArrayDesc, int numMipmapLevels);

        /// <summary>Gets a mipmap level of a CUDA mipmapped array
        ///
        /// Returns in *<paramref name="pLevelArray"/> a CUDA array that represents a single mipmap level
        /// of the CUDA mipmapped array <paramref name="hMipmappedArray."/>
        ///
        /// If <paramref name="level"/> is greater than the maximum number of levels in this mipmapped array,
        /// ::CUDA_ERROR_INVALID_VALUE is returned.</summary>
        ///
        /// <param name="pLevelArray">Returned mipmap level CUDA array</param>
        /// <param name="hMipmappedArray">CUDA mipmapped array</param>
        /// <param name="level">Mipmap level</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_INVALID_HANDLE
        /// </returns>
        /// \notefnerr
        ///
        /// \sa
        /// ::cuMipmappedArrayCreate,
        /// ::cuMipmappedArrayDestroy,
        /// ::cuArrayCreate,
        /// ::cudaGetMipmappedArrayLevel
        /// CUresult CUDAAPI cuMipmappedArrayGetLevel(CUarray *pLevelArray, CUmipmappedArray hMipmappedArray, unsigned int level);
        [DllImport(_dllpath, EntryPoint = "cuMipmappedArrayGetLevel")]
        public static extern CuResult MipmappedArrayGetLevel(out CuArray pLevelArray, CuMipMappedArray hMipmappedArray, int level);

        /// <summary>Destroys a CUDA mipmapped array
        ///
        /// Destroys the CUDA mipmapped array <paramref name="hMipmappedArray"/>.</summary>
        ///
        /// <param name="hMipmappedArray">Mipmapped array to destroy</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_ARRAY_IS_MAPPED
        /// </returns>
        /// \notefnerr
        ///
        /// \sa
        /// ::cuMipmappedArrayCreate,
        /// ::cuMipmappedArrayGetLevel,
        /// ::cuArrayCreate,
        /// ::cudaFreeMipmappedArray
        /// CUresult CUDAAPI cuMipmappedArrayDestroy(CUmipmappedArray hMipmappedArray);
        [DllImport(_dllpath, EntryPoint = "cuMipmappedArrayDestroy")]
        public static extern CuResult MipmappedArrayDestroy(CuMipMappedArray hMipmappedArray);
        #endregion
    }
}