using System.Runtime.InteropServices;

namespace Lennox.NvEncSharp
{
    public static unsafe partial class LibCuda
    {
        /// <summary>Gets an interprocess handle for a previously allocated event
        ///
        /// Takes as input a previously allocated event. This event must have been
        /// created with the ::CU_EVENT_INTERPROCESS and ::CU_EVENT_DISABLE_TIMING
        /// flags set. This opaque handle may be copied into other processes and
        /// opened with ::cuIpcOpenEventHandle to allow efficient hardware
        /// synchronization between GPU work in different processes.
        ///
        /// After the event has been opened in the importing process,
        /// ::cuEventRecord, ::cuEventSynchronize, ::cuStreamWaitEvent and
        /// ::cuEventQuery may be used in either process. Performing operations
        /// on the imported event after the exported event has been freed
        /// with ::cuEventDestroy will result in undefined behavior.
        ///
        /// IPC functionality is restricted to devices with support for unified
        /// addressing on Linux operating systems.</summary>
        ///
        /// <param name="pHandle">Pointer to a user allocated CUipcEventHandle
        ///                    in which to return the opaque event handle</param>
        /// <param name="event">Event allocated with ::CU_EVENT_INTERPROCESS and
        ///                    ::CU_EVENT_DISABLE_TIMING flags.</param>
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_OUT_OF_MEMORY,
        /// ::CUDA_ERROR_MAP_FAILED
        /// </returns>
        ///
        /// \sa
        /// ::cuEventCreate,
        /// ::cuEventDestroy,
        /// ::cuEventSynchronize,
        /// ::cuEventQuery,
        /// ::cuStreamWaitEvent,
        /// ::cuIpcOpenEventHandle,
        /// ::cuIpcGetMemHandle,
        /// ::cuIpcOpenMemHandle,
        /// ::cuIpcCloseMemHandle,
        /// ::cudaIpcGetEventHandle
        /// CUresult CUDAAPI cuIpcGetEventHandle(CUipcEventHandle *pHandle, CUevent event);
        [DllImport(_dllpath, EntryPoint = "cuIpcGetEventHandle")]
        public static extern CuResult IpcGetEventHandle(out CuIpcEventHandle pHandle, CuEvent @event);

        /// <summary>Opens an interprocess event handle for use in the current process
        ///
        /// Opens an interprocess event handle exported from another process with
        /// ::cuIpcGetEventHandle. This function returns a ::CUevent that behaves like
        /// a locally created event with the ::CU_EVENT_DISABLE_TIMING flag specified.
        /// This event must be freed with ::cuEventDestroy.
        ///
        /// Performing operations on the imported event after the exported event has
        /// been freed with ::cuEventDestroy will result in undefined behavior.
        ///
        /// IPC functionality is restricted to devices with support for unified
        /// addressing on Linux operating systems.</summary>
        ///
        /// <param name="phEvent">Returns the imported event</param>
        /// <param name="handle">Interprocess handle to open</param>
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_MAP_FAILED,
        /// ::CUDA_ERROR_PEER_ACCESS_UNSUPPORTED,
        /// ::CUDA_ERROR_INVALID_HANDLE
        /// </returns>
        ///
        /// \sa
        /// ::cuEventCreate,
        /// ::cuEventDestroy,
        /// ::cuEventSynchronize,
        /// ::cuEventQuery,
        /// ::cuStreamWaitEvent,
        /// ::cuIpcGetEventHandle,
        /// ::cuIpcGetMemHandle,
        /// ::cuIpcOpenMemHandle,
        /// ::cuIpcCloseMemHandle,
        /// ::cudaIpcOpenEventHandle
        /// CUresult CUDAAPI cuIpcOpenEventHandle(CUevent *phEvent, CUipcEventHandle handle);
        [DllImport(_dllpath, EntryPoint = "cuIpcOpenEventHandle")]
        public static extern CuResult IpcOpenEventHandle(out CuEvent phEvent, CuIpcEventHandle handle);

        /// <summary>Gets an interprocess memory handle for an existing device memory
        /// allocation
        ///
        /// Takes a pointer to the base of an existing device memory allocation created
        /// with ::cuMemAlloc and exports it for use in another process. This is a
        /// lightweight operation and may be called multiple times on an allocation
        /// without adverse effects.
        ///
        /// If a region of memory is freed with ::cuMemFree and a subsequent call
        /// to ::cuMemAlloc returns memory with the same device address,
        /// ::cuIpcGetMemHandle will return a unique handle for the
        /// new memory.
        ///
        /// IPC functionality is restricted to devices with support for unified
        /// addressing on Linux operating systems.</summary>
        ///
        /// <param name="pHandle">Pointer to user allocated ::CUipcMemHandle to return
        ///                    the handle in.</param>
        /// <param name="dptr">Base pointer to previously allocated device memory </param>
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_OUT_OF_MEMORY,
        /// ::CUDA_ERROR_MAP_FAILED,
        /// </returns>
        ///
        /// \sa
        /// ::cuMemAlloc,
        /// ::cuMemFree,
        /// ::cuIpcGetEventHandle,
        /// ::cuIpcOpenEventHandle,
        /// ::cuIpcOpenMemHandle,
        /// ::cuIpcCloseMemHandle,
        /// ::cudaIpcGetMemHandle
        /// CUresult CUDAAPI cuIpcGetMemHandle(CUipcMemHandle *pHandle, CUdeviceptr dptr);
        [DllImport(_dllpath, EntryPoint = "cuIpcGetMemHandle")]

        public static extern CuResult IpcGetMemHandle(CuIpcMemHandle* pHandle, CuDevicePtr dptr);

        /// <summary>Opens an interprocess memory handle exported from another process
        /// and returns a device pointer usable in the local process.
        ///
        /// Maps memory exported from another process with ::cuIpcGetMemHandle into
        /// the current device address space. For contexts on different devices
        /// ::cuIpcOpenMemHandle can attempt to enable peer access between the
        /// devices as if the user called ::cuCtxEnablePeerAccess. This behavior is
        /// controlled by the ::CU_IPC_MEM_LAZY_ENABLE_PEER_ACCESS flag.
        /// ::cuDeviceCanAccessPeer can determine if a mapping is possible.
        ///
        /// Contexts that may open ::CUipcMemHandles are restricted in the following way.
        /// ::CUipcMemHandles from each ::CUdevice in a given process may only be opened
        /// by one ::CUcontext per ::CUdevice per other process.
        ///
        /// Memory returned from ::cuIpcOpenMemHandle must be freed with
        /// ::cuIpcCloseMemHandle.
        ///
        /// Calling ::cuMemFree on an exported memory region before calling
        /// ::cuIpcCloseMemHandle in the importing context will result in undefined
        /// behavior.
        ///
        /// IPC functionality is restricted to devices with support for unified
        /// addressing on Linux operating systems.</summary>
        ///
        /// <param name="pdptr">Returned device pointer</param>
        /// <param name="handle">::CUipcMemHandle to open</param>
        /// <param name="flags">Flags for this operation. Must be specified as ::CU_IPC_MEM_LAZY_ENABLE_PEER_ACCESS</param>
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_MAP_FAILED,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_TOO_MANY_PEERS
        /// </returns>
        ///
        /// \note No guarantees are made about the address returned in <paramref name="*pdptr"/>.
        /// In particular, multiple processes may not receive the same address for the same <paramref name="handle.
        ///
        ///"/> \sa
        /// ::cuMemAlloc,
        /// ::cuMemFree,
        /// ::cuIpcGetEventHandle,
        /// ::cuIpcOpenEventHandle,
        /// ::cuIpcGetMemHandle,
        /// ::cuIpcCloseMemHandle,
        /// ::cuCtxEnablePeerAccess,
        /// ::cuDeviceCanAccessPeer,
        /// ::cudaIpcOpenMemHandle
        /// CUresult CUDAAPI cuIpcOpenMemHandle(CUdeviceptr *pdptr, CUipcMemHandle handle, unsigned int Flags);
        [DllImport(_dllpath, EntryPoint = "cuIpcOpenMemHandle")]
        public static extern CuResult IpcOpenMemHandle(out CuDevicePtr pdptr, CuIpcMemHandle handle, CuIpcMemFlags flags);

        /// <summary>Close memory mapped with ::cuIpcOpenMemHandle
        ///
        /// Unmaps memory returnd by ::cuIpcOpenMemHandle. The original allocation
        /// in the exporting process as well as imported mappings in other processes
        /// will be unaffected.
        ///
        /// Any resources used to enable peer access will be freed if this is the
        /// last mapping using them.
        ///
        /// IPC functionality is restricted to devices with support for unified
        /// addressing on Linux operating systems.</summary>
        ///
        /// <param name="dptr">Device pointer returned by ::cuIpcOpenMemHandle
        /// </param>
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_MAP_FAILED,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// </returns>
        ///
        /// \sa
        /// ::cuMemAlloc,
        /// ::cuMemFree,
        /// ::cuIpcGetEventHandle,
        /// ::cuIpcOpenEventHandle,
        /// ::cuIpcGetMemHandle,
        /// ::cuIpcOpenMemHandle,
        /// ::cudaIpcCloseMemHandle
        /// CUresult CUDAAPI cuIpcCloseMemHandle(CUdeviceptr dptr);
        [DllImport(_dllpath, EntryPoint = "cuIpcCloseMemHandle")]
        public static extern CuResult IpcCloseMemHandle(CuDevicePtr dptr);
    }
}
