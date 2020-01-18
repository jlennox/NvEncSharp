using System;
using System.Runtime.InteropServices;

namespace Lennox.NvEncSharp
{
    public static partial class LibCuda
    {
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
        [DllImport(_dllpath, EntryPoint = "cuMemcpyHtoD" + _ver)]
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
        [DllImport(_dllpath, EntryPoint = "cuMemcpyDtoH" + _ver)]
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
        [DllImport(_dllpath, EntryPoint = "cuMemcpyDtoD" + _ver)]
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
        [DllImport(_dllpath, EntryPoint = "cuMemcpyDtoA" + _ver)]
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
        [DllImport(_dllpath, EntryPoint = "cuMemcpyAtoD" + _ver)]
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
        [DllImport(_dllpath, EntryPoint = "cuMemcpyHtoA" + _ver)]
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
        [DllImport(_dllpath, EntryPoint = "cuMemcpyAtoH" + _ver)]
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
        [DllImport(_dllpath, EntryPoint = "cuMemcpyAtoA" + _ver)]
        public static extern CuResult MemcpyAtoA(CuArray dstArray, IntPtr dstOffset, CuArray srcArray, IntPtr srcOffset, IntPtr byteCount);

        /// <summary>Copies memory for 2D arrays
        ///
        /// Perform a 2D memory copy according to the parameters specified in <paramref name="pCopy"/>.
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
        [DllImport(_dllpath, EntryPoint = "cuMemcpy2D" + _ver)]
        public static extern CuResult Memcpy2D(ref CudaMemcopy2D pCopy);

        /// <summary>Copies memory for 2D arrays
        ///
        /// Perform a 2D memory copy according to the parameters specified in <paramref name="pCopy"/>.
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
        [DllImport(_dllpath, EntryPoint = "cuMemcpy2DUnaligned" + _ver)]
        public static extern CuResult Memcpy2DUnaligned(ref CudaMemcopy2D pCopy);

        /// <summary>Copies memory for 3D arrays
        ///
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
        ///
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
        [DllImport(_dllpath, EntryPoint = "cuMemcpyHtoDAsync" + _ver)]
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
        [DllImport(_dllpath, EntryPoint = "cuMemcpyDtoHAsync" + _ver)]
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
        [DllImport(_dllpath, EntryPoint = "cuMemcpyDtoDAsync" + _ver)]
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
        [DllImport(_dllpath, EntryPoint = "cuMemcpyHtoAAsync" + _ver)]
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
        [DllImport(_dllpath, EntryPoint = "cuMemcpyAtoHAsync" + _ver)]
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
        [DllImport(_dllpath, EntryPoint = "cuMemcpy2DAsync" + _ver)]
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
        [DllImport(_dllpath, EntryPoint = "cuMemcpy3DAsync" + _ver)]
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
    }
}