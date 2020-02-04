using System;
using System.Runtime.InteropServices;

// ReSharper disable UnusedMember.Global

namespace Lennox.NvEncSharp
{
    public static partial class LibCuda
    {
        /// <summary>Initializes device memory
        ///
        /// Sets the memory range of <paramref name="n"/> 8-bit values to the specified value
        /// <paramref name="uc"/>.</summary>
        ///
        /// <param name="dstDevice">Destination device pointer</param>
        /// <param name="uc">Value to set</param>
        /// <param name="n">Number of elements</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \notefnerr
        /// \note_memset
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D8Async,
        /// ::cuMemsetD2D16, ::cuMemsetD2D16Async, ::cuMemsetD2D32, ::cuMemsetD2D32Async,
        /// ::cuMemsetD8Async, ::cuMemsetD16, ::cuMemsetD16Async,
        /// ::cuMemsetD32, ::cuMemsetD32Async,
        /// ::cudaMemset
        /// CUresult CUDAAPI cuMemsetD8(CUdeviceptr dstDevice, unsigned char uc, size_t N);
        [DllImport(_dllpath, EntryPoint = "cuMemsetD8")]
        public static extern CuResult MemsetD8(CuDevicePtr dstDevice, byte uc, IntPtr n);

        /// <summary>Initializes device memory
        ///
        /// Sets the memory range of <paramref name="n"/> 16-bit values to the specified value
        /// <paramref name="us"/>. The <paramref name="dstDevice"/> pointer must be two byte aligned.</summary>
        ///
        /// <param name="dstDevice">Destination device pointer</param>
        /// <param name="us">Value to set</param>
        /// <param name="n">Number of elements</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \notefnerr
        /// \note_memset
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D8Async,
        /// ::cuMemsetD2D16, ::cuMemsetD2D16Async, ::cuMemsetD2D32, ::cuMemsetD2D32Async,
        /// ::cuMemsetD8, ::cuMemsetD8Async, ::cuMemsetD16Async,
        /// ::cuMemsetD32, ::cuMemsetD32Async,
        /// ::cudaMemset
        /// CUresult CUDAAPI cuMemsetD16(CUdeviceptr dstDevice, unsigned short us, size_t N);
        [DllImport(_dllpath, EntryPoint = "cuMemsetD16")]
        public static extern CuResult MemsetD16(CuDevicePtr dstDevice, short us, IntPtr n);

        /// <summary>Initializes device memory
        ///
        /// Sets the memory range of <paramref name="n"/> 32-bit values to the specified value
        /// <paramref name="ui"/>. The <paramref name="dstDevice"/> pointer must be four byte aligned.</summary>
        ///
        /// <param name="dstDevice">Destination device pointer</param>
        /// <param name="ui">Value to set</param>
        /// <param name="n">Number of elements</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \notefnerr
        /// \note_memset
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D8Async,
        /// ::cuMemsetD2D16, ::cuMemsetD2D16Async, ::cuMemsetD2D32, ::cuMemsetD2D32Async,
        /// ::cuMemsetD8, ::cuMemsetD8Async, ::cuMemsetD16, ::cuMemsetD16Async,
        /// ::cuMemsetD32Async,
        /// ::cudaMemset
        /// CUresult CUDAAPI cuMemsetD32(CUdeviceptr dstDevice, unsigned int ui, size_t N);
        [DllImport(_dllpath, EntryPoint = "cuMemsetD32")]
        public static extern CuResult MemsetD32(CuDevicePtr dstDevice, int ui, IntPtr n);

        /// <summary>Initializes device memory
        ///
        /// Sets the 2D memory range of <paramref name="width"/> 8-bit values to the specified value
        /// <paramref name="uc"/>. <paramref name="height"/> specifies the number of rows to set, and <paramref name="dstPitch"/>
        /// specifies the number of bytes between each row. This function performs
        /// fastest when the pitch is one that has been passed back by
        /// ::cuMemAllocPitch().</summary>
        ///
        /// <param name="dstDevice">Destination device pointer</param>
        /// <param name="dstPitch">Pitch of destination device pointer</param>
        /// <param name="uc">Value to set</param>
        /// <param name="width">Width of row</param>
        /// <param name="height">Number of rows</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \notefnerr
        /// \note_memset
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8Async,
        /// ::cuMemsetD2D16, ::cuMemsetD2D16Async, ::cuMemsetD2D32, ::cuMemsetD2D32Async,
        /// ::cuMemsetD8, ::cuMemsetD8Async, ::cuMemsetD16, ::cuMemsetD16Async,
        /// ::cuMemsetD32, ::cuMemsetD32Async,
        /// ::cudaMemset2D
        /// CUresult CUDAAPI cuMemsetD2D8(CUdeviceptr dstDevice, size_t dstPitch, unsigned char uc, size_t Width, size_t Height);
        [DllImport(_dllpath, EntryPoint = "cuMemsetD2D8")]
        public static extern CuResult MemsetD2D8(CuDevicePtr dstDevice, IntPtr dstPitch, byte uc, IntPtr width, IntPtr height);

        /// <summary>Initializes device memory
        ///
        /// Sets the 2D memory range of <paramref name="width"/> 16-bit values to the specified value
        /// <paramref name="us"/>. <paramref name="height"/> specifies the number of rows to set, and <paramref name="dstPitch"/>
        /// specifies the number of bytes between each row. The <paramref name="dstDevice"/> pointer
        /// and <paramref name="dstPitch"/> offset must be two byte aligned. This function performs
        /// fastest when the pitch is one that has been passed back by
        /// ::cuMemAllocPitch().</summary>
        ///
        /// <param name="dstDevice">Destination device pointer</param>
        /// <param name="dstPitch">Pitch of destination device pointer</param>
        /// <param name="us">Value to set</param>
        /// <param name="width">Width of row</param>
        /// <param name="height">Number of rows</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \notefnerr
        /// \note_memset
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D8Async,
        /// ::cuMemsetD2D16Async, ::cuMemsetD2D32, ::cuMemsetD2D32Async,
        /// ::cuMemsetD8, ::cuMemsetD8Async, ::cuMemsetD16, ::cuMemsetD16Async,
        /// ::cuMemsetD32, ::cuMemsetD32Async,
        /// ::cudaMemset2D
        /// CUresult CUDAAPI cuMemsetD2D16(CUdeviceptr dstDevice, size_t dstPitch, unsigned short us, size_t Width, size_t Height);
        [DllImport(_dllpath, EntryPoint = "cuMemsetD2D16")]
        public static extern CuResult MemsetD2D16(CuDevicePtr dstDevice, IntPtr dstPitch, short us, IntPtr width, IntPtr height);

        /// <summary>Initializes device memory
        ///
        /// Sets the 2D memory range of <paramref name="width"/> 32-bit values to the specified value
        /// <paramref name="ui"/>. <paramref name="height"/> specifies the number of rows to set, and <paramref name="dstPitch"/>
        /// specifies the number of bytes between each row. The <paramref name="dstDevice"/> pointer
        /// and <paramref name="dstPitch"/> offset must be four byte aligned. This function performs
        /// fastest when the pitch is one that has been passed back by
        /// ::cuMemAllocPitch().</summary>
        ///
        /// <param name="dstDevice">Destination device pointer</param>
        /// <param name="dstPitch">Pitch of destination device pointer</param>
        /// <param name="ui">Value to set</param>
        /// <param name="width">Width of row</param>
        /// <param name="height">Number of rows</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \notefnerr
        /// \note_memset
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D8Async,
        /// ::cuMemsetD2D16, ::cuMemsetD2D16Async, ::cuMemsetD2D32Async,
        /// ::cuMemsetD8, ::cuMemsetD8Async, ::cuMemsetD16, ::cuMemsetD16Async,
        /// ::cuMemsetD32, ::cuMemsetD32Async,
        /// ::cudaMemset2D
        /// CUresult CUDAAPI cuMemsetD2D32(CUdeviceptr dstDevice, size_t dstPitch, unsigned int ui, size_t Width, size_t Height);
        [DllImport(_dllpath, EntryPoint = "cuMemsetD2D32")]
        public static extern CuResult MemsetD2D32(CuDevicePtr dstDevice, IntPtr dstPitch, int ui, IntPtr width, IntPtr height);

        /// <summary>Sets device memory
        ///
        /// Sets the memory range of <paramref name="n"/> 8-bit values to the specified value
        /// <paramref name="uc"/>.</summary>
        ///
        /// <param name="dstDevice">Destination device pointer</param>
        /// <param name="uc">Value to set</param>
        /// <param name="n">Number of elements</param>
        /// <param name="hStream">Stream identifier</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \notefnerr
        /// \note_memset
        /// \note_null_stream
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D8Async,
        /// ::cuMemsetD2D16, ::cuMemsetD2D16Async, ::cuMemsetD2D32, ::cuMemsetD2D32Async,
        /// ::cuMemsetD8, ::cuMemsetD16, ::cuMemsetD16Async,
        /// ::cuMemsetD32, ::cuMemsetD32Async,
        /// ::cudaMemsetAsync
        /// CUresult CUDAAPI cuMemsetD8Async(CUdeviceptr dstDevice, unsigned char uc, size_t N, CUstream hStream);
        [DllImport(_dllpath, EntryPoint = "cuMemsetD8Async")]
        public static extern CuResult MemsetD8Async(CuDevicePtr dstDevice, byte uc, IntPtr n, CuStream hStream);

        /// <summary>Sets device memory
        ///
        /// Sets the memory range of <paramref name="n"/> 16-bit values to the specified value
        /// <paramref name="us"/>. The <paramref name="dstDevice"/> pointer must be two byte aligned.</summary>
        ///
        /// <param name="dstDevice">Destination device pointer</param>
        /// <param name="us">Value to set</param>
        /// <param name="n">Number of elements</param>
        /// <param name="hStream">Stream identifier</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \notefnerr
        /// \note_memset
        /// \note_null_stream
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D8Async,
        /// ::cuMemsetD2D16, ::cuMemsetD2D16Async, ::cuMemsetD2D32, ::cuMemsetD2D32Async,
        /// ::cuMemsetD8, ::cuMemsetD8Async, ::cuMemsetD16,
        /// ::cuMemsetD32, ::cuMemsetD32Async,
        /// ::cudaMemsetAsync
        /// CUresult CUDAAPI cuMemsetD16Async(CUdeviceptr dstDevice, unsigned short us, size_t N, CUstream hStream);
        [DllImport(_dllpath, EntryPoint = "cuMemsetD16Async")]
        public static extern CuResult MemsetD16Async(CuDevicePtr dstDevice, short us, IntPtr n, CuStream hStream);

        /// <summary>Sets device memory
        ///
        /// Sets the memory range of <paramref name="n"/> 32-bit values to the specified value
        /// <paramref name="ui"/>. The <paramref name="dstDevice"/> pointer must be four byte aligned.</summary>
        ///
        /// <param name="dstDevice">Destination device pointer</param>
        /// <param name="ui">Value to set</param>
        /// <param name="n">Number of elements</param>
        /// <param name="hStream">Stream identifier</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \notefnerr
        /// \note_memset
        /// \note_null_stream
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D8Async,
        /// ::cuMemsetD2D16, ::cuMemsetD2D16Async, ::cuMemsetD2D32, ::cuMemsetD2D32Async,
        /// ::cuMemsetD8, ::cuMemsetD8Async, ::cuMemsetD16, ::cuMemsetD16Async, ::cuMemsetD32,
        /// ::cudaMemsetAsync
        /// CUresult CUDAAPI cuMemsetD32Async(CUdeviceptr dstDevice, unsigned int ui, size_t N, CUstream hStream);
        [DllImport(_dllpath, EntryPoint = "cuMemsetD32Async")]
        public static extern CuResult MemsetD32Async(CuDevicePtr dstDevice, int ui, IntPtr n, CuStream hStream);

        /// <summary>Sets device memory
        ///
        /// Sets the 2D memory range of <paramref name="width"/> 8-bit values to the specified value
        /// <paramref name="uc"/>. <paramref name="height"/> specifies the number of rows to set, and <paramref name="dstPitch"/>
        /// specifies the number of bytes between each row. This function performs
        /// fastest when the pitch is one that has been passed back by
        /// ::cuMemAllocPitch().</summary>
        ///
        /// <param name="dstDevice">Destination device pointer</param>
        /// <param name="dstPitch">Pitch of destination device pointer</param>
        /// <param name="uc">Value to set</param>
        /// <param name="width">Width of row</param>
        /// <param name="height">Number of rows</param>
        /// <param name="hStream">Stream identifier</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \notefnerr
        /// \note_memset
        /// \note_null_stream
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8,
        /// ::cuMemsetD2D16, ::cuMemsetD2D16Async, ::cuMemsetD2D32, ::cuMemsetD2D32Async,
        /// ::cuMemsetD8, ::cuMemsetD8Async, ::cuMemsetD16, ::cuMemsetD16Async,
        /// ::cuMemsetD32, ::cuMemsetD32Async,
        /// ::cudaMemset2DAsync
        /// CUresult CUDAAPI cuMemsetD2D8Async(CUdeviceptr dstDevice, size_t dstPitch, unsigned char uc, size_t Width, size_t Height, CUstream hStream);
        [DllImport(_dllpath, EntryPoint = "cuMemsetD2D8Async")]
        public static extern CuResult MemsetD2D8Async(CuDevicePtr dstDevice, IntPtr dstPitch, byte uc, IntPtr width, IntPtr height, CuStream hStream);

        /// <summary>Sets device memory
        ///
        /// Sets the 2D memory range of <paramref name="width"/> 16-bit values to the specified value
        /// <paramref name="us"/>. <paramref name="height"/> specifies the number of rows to set, and <paramref name="dstPitch"/>
        /// specifies the number of bytes between each row. The <paramref name="dstDevice"/> pointer
        /// and <paramref name="dstPitch"/> offset must be two byte aligned. This function performs
        /// fastest when the pitch is one that has been passed back by
        /// ::cuMemAllocPitch().</summary>
        ///
        /// <param name="dstDevice">Destination device pointer</param>
        /// <param name="dstPitch">Pitch of destination device pointer</param>
        /// <param name="us">Value to set</param>
        /// <param name="width">Width of row</param>
        /// <param name="height">Number of rows</param>
        /// <param name="hStream">Stream identifier</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \notefnerr
        /// \note_memset
        /// \note_null_stream
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D8Async,
        /// ::cuMemsetD2D16, ::cuMemsetD2D32, ::cuMemsetD2D32Async,
        /// ::cuMemsetD8, ::cuMemsetD8Async, ::cuMemsetD16, ::cuMemsetD16Async,
        /// ::cuMemsetD32, ::cuMemsetD32Async,
        /// ::cudaMemset2DAsync
        /// CUresult CUDAAPI cuMemsetD2D16Async(CUdeviceptr dstDevice, size_t dstPitch, unsigned short us, size_t Width, size_t Height, CUstream hStream);
        [DllImport(_dllpath, EntryPoint = "cuMemsetD2D16Async")]
        public static extern CuResult MemsetD2D16Async(CuDevicePtr dstDevice, IntPtr dstPitch, short us, IntPtr width, IntPtr height, CuStream hStream);

        /// <summary>Sets device memory
        ///
        /// Sets the 2D memory range of <paramref name="width"/> 32-bit values to the specified value
        /// <paramref name="ui"/>. <paramref name="height"/> specifies the number of rows to set, and <paramref name="dstPitch"/>
        /// specifies the number of bytes between each row. The <paramref name="dstDevice"/> pointer
        /// and <paramref name="dstPitch"/> offset must be four byte aligned. This function performs
        /// fastest when the pitch is one that has been passed back by
        /// ::cuMemAllocPitch().</summary>
        ///
        /// <param name="dstDevice">Destination device pointer</param>
        /// <param name="dstPitch">Pitch of destination device pointer</param>
        /// <param name="ui">Value to set</param>
        /// <param name="width">Width of row</param>
        /// <param name="height">Number of rows</param>
        /// <param name="hStream">Stream identifier</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \notefnerr
        /// \note_memset
        /// \note_null_stream
        ///
        /// \sa ::cuArray3DCreate, ::cuArray3DGetDescriptor, ::cuArrayCreate,
        /// ::cuArrayDestroy, ::cuArrayGetDescriptor, ::cuMemAlloc, ::cuMemAllocHost,
        /// ::cuMemAllocPitch, ::cuMemcpy2D, ::cuMemcpy2DAsync, ::cuMemcpy2DUnaligned,
        /// ::cuMemcpy3D, ::cuMemcpy3DAsync, ::cuMemcpyAtoA, ::cuMemcpyAtoD,
        /// ::cuMemcpyAtoH, ::cuMemcpyAtoHAsync, ::cuMemcpyDtoA, ::cuMemcpyDtoD, ::cuMemcpyDtoDAsync,
        /// ::cuMemcpyDtoH, ::cuMemcpyDtoHAsync, ::cuMemcpyHtoA, ::cuMemcpyHtoAAsync,
        /// ::cuMemcpyHtoD, ::cuMemcpyHtoDAsync, ::cuMemFree, ::cuMemFreeHost,
        /// ::cuMemGetAddressRange, ::cuMemGetInfo, ::cuMemHostAlloc,
        /// ::cuMemHostGetDevicePointer, ::cuMemsetD2D8, ::cuMemsetD2D8Async,
        /// ::cuMemsetD2D16, ::cuMemsetD2D16Async, ::cuMemsetD2D32,
        /// ::cuMemsetD8, ::cuMemsetD8Async, ::cuMemsetD16, ::cuMemsetD16Async,
        /// ::cuMemsetD32, ::cuMemsetD32Async,
        /// ::cudaMemset2DAsync
        /// CUresult CUDAAPI cuMemsetD2D32Async(CUdeviceptr dstDevice, size_t dstPitch, unsigned int ui, size_t Width, size_t Height, CUstream hStream);
        [DllImport(_dllpath, EntryPoint = "cuMemsetD2D32Async")]
        public static extern CuResult MemsetD2D32Async(CuDevicePtr dstDevice, IntPtr dstPitch, int ui, IntPtr width, IntPtr height, CuStream hStream);
    }
}
