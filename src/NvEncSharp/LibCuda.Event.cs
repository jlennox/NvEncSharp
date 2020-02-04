using System.Runtime.InteropServices;

// ReSharper disable UnusedMember.Global

namespace Lennox.NvEncSharp
{
    public static unsafe partial class LibCuda
    {
        /// <summary>Creates an event
        ///
        /// Creates an event *phEvent with the flags specified via <paramref name="flags"/>. Valid flags
        /// include:
        /// - ::CU_EVENT_DEFAULT: Default event creation flag.
        /// - ::CU_EVENT_BLOCKING_SYNC: Specifies that the created event should use blocking
        ///   synchronization.  A CPU thread that uses ::cuEventSynchronize() to wait on
        ///   an event created with this flag will block until the event has actually
        ///   been recorded.
        /// - ::CU_EVENT_DISABLE_TIMING: Specifies that the created event does not need
        ///   to record timing data.  Events created with this flag specified and
        ///   the ::CU_EVENT_BLOCKING_SYNC flag not specified will provide the best
        ///   performance when used with ::cuStreamWaitEvent() and ::cuEventQuery().
        /// - ::CU_EVENT_INTERPROCESS: Specifies that the created event may be used as an
        ///   interprocess event by ::cuIpcGetEventHandle(). ::CU_EVENT_INTERPROCESS must
        ///   be specified along with ::CU_EVENT_DISABLE_TIMING.</summary>
        ///
        /// <param name="phEvent">Returns newly created event</param>
        /// <param name="flags">Event creation flags</param>
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
        /// \sa
        /// ::cuEventRecord,
        /// ::cuEventQuery,
        /// ::cuEventSynchronize,
        /// ::cuEventDestroy,
        /// ::cuEventElapsedTime,
        /// ::cudaEventCreate,
        /// ::cudaEventCreateWithFlags
        /// CUresult CUDAAPI cuEventCreate(CUevent *phEvent, unsigned int Flags);
        [DllImport(_dllpath, EntryPoint = "cuEventCreate")]
        public static extern CuResult EventCreate(out CuEvent phEvent, int flags);

        /// <summary>Records an event
        ///
        /// Records an event. See note on NULL stream behavior. Since operation is
        /// asynchronous, ::cuEventQuery or ::cuEventSynchronize() must be used
        /// to determine when the event has actually been recorded.
        ///
        /// If ::cuEventRecord() has previously been called on <paramref name="hEvent"/>, then this
        /// call will overwrite any existing state in <paramref name="hEvent"/>..  Any subsequent calls
        /// which examine the status of <paramref name="hEvent"/> will only examine the completion of
        /// this most recent call to ::cuEventRecord().
        ///
        /// It is necessary that <paramref name="hEvent"/> and <paramref name="hStream"/> be created on the same context.</summary>
        ///
        /// <param name="hEvent">Event to record</param>
        /// <param name="hStream">Stream to record event for</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// \note_null_stream
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuEventCreate,
        /// ::cuEventQuery,
        /// ::cuEventSynchronize,
        /// ::cuStreamWaitEvent,
        /// ::cuEventDestroy,
        /// ::cuEventElapsedTime,
        /// ::cudaEventRecord
        /// CUresult CUDAAPI cuEventRecord(CUevent hEvent, CUstream hStream);
        [DllImport(_dllpath, EntryPoint = "cuEventRecord")]
        public static extern CuResult EventRecord(CuEvent hEvent, CuStream hStream);

        /// <summary>Queries an event's status
        ///
        /// Query the status of all device work preceding the most recent
        /// call to ::cuEventRecord() (in the appropriate compute streams,
        /// as specified by the arguments to ::cuEventRecord()).
        ///
        /// If this work has successfully been completed by the device, or if
        /// ::cuEventRecord() has not been called on <paramref name="hEvent"/>, then ::CUDA_SUCCESS is
        /// returned. If this work has not yet been completed by the device then
        /// ::CUDA_ERROR_NOT_READY is returned.
        ///
        /// For the purposes of Unified Memory, a return value of ::CUDA_SUCCESS
        /// is equivalent to having called ::cuEventSynchronize().</summary>
        ///
        /// <param name="hEvent">Event to query</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_NOT_READY
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuEventCreate,
        /// ::cuEventRecord,
        /// ::cuEventSynchronize,
        /// ::cuEventDestroy,
        /// ::cuEventElapsedTime,
        /// ::cudaEventQuery
        /// CUresult CUDAAPI cuEventQuery(CUevent hEvent);
        [DllImport(_dllpath, EntryPoint = "cuEventQuery")]
        public static extern CuResult EventQuery(CuEvent hEvent);

        /// <summary>Waits for an event to complete
        ///
        /// Wait until the completion of all device work preceding the most recent
        /// call to ::cuEventRecord() (in the appropriate compute streams, as specified
        /// by the arguments to ::cuEventRecord()).
        ///
        /// If ::cuEventRecord() has not been called on <paramref name="hEvent"/>, ::CUDA_SUCCESS is
        /// returned immediately.
        ///
        /// Waiting for an event that was created with the ::CU_EVENT_BLOCKING_SYNC
        /// flag will cause the calling CPU thread to block until the event has
        /// been completed by the device.  If the ::CU_EVENT_BLOCKING_SYNC flag has
        /// not been set, then the CPU thread will busy-wait until the event has
        /// been completed by the device.</summary>
        ///
        /// <param name="hEvent">Event to wait for</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_HANDLE
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuEventCreate,
        /// ::cuEventRecord,
        /// ::cuEventQuery,
        /// ::cuEventDestroy,
        /// ::cuEventElapsedTime,
        /// ::cudaEventSynchronize
        /// CUresult CUDAAPI cuEventSynchronize(CUevent hEvent);
        [DllImport(_dllpath, EntryPoint = "cuEventSynchronize")]
        public static extern CuResult EventSynchronize(CuEvent hEvent);

        /// <summary>Destroys an event
        ///
        /// Destroys the event specified by <paramref name="hEvent"/>.
        ///
        /// In case <paramref name="hEvent"/> has been recorded but has not yet been completed
        /// when ::cuEventDestroy() is called, the function will return immediately and
        /// the resources associated with <paramref name="hEvent"/> will be released automatically once
        /// the device has completed <paramref name="hEvent"/>.</summary>
        ///
        /// <param name="hEvent">Event to destroy</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_HANDLE
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuEventCreate,
        /// ::cuEventRecord,
        /// ::cuEventQuery,
        /// ::cuEventSynchronize,
        /// ::cuEventElapsedTime,
        /// ::cudaEventDestroy
        /// CUresult CUDAAPI cuEventDestroy(CUevent hEvent);
        [DllImport(_dllpath, EntryPoint = "cuEventDestroy")]
        public static extern CuResult EventDestroy(CuEvent hEvent);

        /// <summary>Computes the elapsed time between two events
        ///
        /// Computes the elapsed time between two events (in milliseconds with a
        /// resolution of around 0.5 microseconds).
        ///
        /// If either event was last recorded in a non-NULL stream, the resulting time
        /// may be greater than expected (even if both used the same stream handle). This
        /// happens because the ::cuEventRecord() operation takes place asynchronously
        /// and there is no guarantee that the measured latency is actually just between
        /// the two events. Any number of other different stream operations could execute
        /// in between the two measured events, thus altering the timing in a significant
        /// way.
        ///
        /// If ::cuEventRecord() has not been called on either event then
        /// ::CUDA_ERROR_INVALID_HANDLE is returned. If ::cuEventRecord() has been called
        /// on both events but one or both of them has not yet been completed (that is,
        /// ::cuEventQuery() would return ::CUDA_ERROR_NOT_READY on at least one of the
        /// events), ::CUDA_ERROR_NOT_READY is returned. If either event was created with
        /// the ::CU_EVENT_DISABLE_TIMING flag, then this function will return
        /// ::CUDA_ERROR_INVALID_HANDLE.</summary>
        ///
        /// <param name="pMilliseconds">Time between <paramref name="hStart"/> and <paramref name="hEnd"/> in ms</param>
        /// <param name="hStart">Starting event</param>
        /// <param name="hEnd">Ending event</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_NOT_READY
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuEventCreate,
        /// ::cuEventRecord,
        /// ::cuEventQuery,
        /// ::cuEventSynchronize,
        /// ::cuEventDestroy,
        /// ::cudaEventElapsedTime
        /// CUresult CUDAAPI cuEventElapsedTime(float *pMilliseconds, CUevent hStart, CUevent hEnd);
        [DllImport(_dllpath, EntryPoint = "cuEventElapsedTime")]
        public static extern CuResult EventElapsedTime(out float pMilliseconds, CuEvent hStart, CuEvent hEnd);

        /// <summary>Wait on a memory location
        ///
        /// Enqueues a synchronization of the stream on the given memory location. Work
        /// ordered after the operation will block until the given condition on the
        /// memory is satisfied. By default, the condition is to wait for
        /// (int32_t)(*addr - value) >= 0, a cyclic greater-or-equal.
        /// Other condition types can be specified via <paramref name="flags"/>.
        ///
        /// If the memory was registered via ::cuMemHostRegister(), the device pointer
        /// should be obtained with ::cuMemHostGetDevicePointer(). This function cannot
        /// be used with managed memory (::cuMemAllocManaged).
        ///
        /// Support for this can be queried with ::cuDeviceGetAttribute() and
        /// ::CU_DEVICE_ATTRIBUTE_CAN_USE_STREAM_MEM_OPS. The only requirement for basic
        /// support is that on Windows, a device must be in TCC mode.</summary>
        ///
        /// <param name="stream">The stream to synchronize on the memory location.</param>
        /// <param name="addr">The memory location to wait on.</param>
        /// <param name="value">The value to compare with the memory location.</param>
        /// <param name="flags">See ::CUstreamWaitValue_flags.</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_NOT_SUPPORTED
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuStreamWaitValue64,
        /// ::cuStreamWriteValue32,
        /// ::cuStreamWriteValue64
        /// ::cuStreamBatchMemOp,
        /// ::cuMemHostRegister,
        /// ::cuStreamWaitEvent
        /// CUresult CUDAAPI cuStreamWaitValue32(CUstream stream, CUdeviceptr addr, cuuint32_t value, unsigned int flags);
        [DllImport(_dllpath, EntryPoint = "cuStreamWaitValue32")]
        public static extern CuResult StreamWaitValue32(CuStream stream, CuDevicePtr addr, int value, CuStreamWaitValue flags);

        /// <summary>Wait on a memory location
        ///
        /// Enqueues a synchronization of the stream on the given memory location. Work
        /// ordered after the operation will block until the given condition on the
        /// memory is satisfied. By default, the condition is to wait for
        /// (int64_t)(*addr - value) >= 0, a cyclic greater-or-equal.
        /// Other condition types can be specified via <paramref name="flags"/>.
        ///
        /// If the memory was registered via ::cuMemHostRegister(), the device pointer
        /// should be obtained with ::cuMemHostGetDevicePointer().
        ///
        /// Support for this can be queried with ::cuDeviceGetAttribute() and
        /// ::CU_DEVICE_ATTRIBUTE_CAN_USE_64_BIT_STREAM_MEM_OPS. The requirements are
        /// compute capability 7.0 or greater, and on Windows, that the device be in
        /// TCC mode.</summary>
        ///
        /// <param name="stream">The stream to synchronize on the memory location.</param>
        /// <param name="addr">The memory location to wait on.</param>
        /// <param name="value">The value to compare with the memory location.</param>
        /// <param name="flags">See ::CUstreamWaitValue_flags.</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_NOT_SUPPORTED
        /// </returns>
        ///
        /// \notefnerr
        ///
        /// \sa ::cuStreamWaitValue32,
        /// ::cuStreamWriteValue32,
        /// ::cuStreamWriteValue64,
        /// ::cuStreamBatchMemOp,
        /// ::cuMemHostRegister,
        /// ::cuStreamWaitEvent
        /// CUresult CUDAAPI cuStreamWaitValue64(CUstream stream, CUdeviceptr addr, cuuint64_t value, unsigned int flags);
        [DllImport(_dllpath, EntryPoint = "cuStreamWaitValue64")]
        public static extern CuResult StreamWaitValue64(CuStream stream, CuDevicePtr addr, long value, CuStreamWriteValue flags);

        /// <summary>Write a value to memory
        ///
        /// Write a value to memory. Unless the ::CU_STREAM_WRITE_VALUE_NO_MEMORY_BARRIER
        /// flag is passed, the write is preceded by a system-wide memory fence,
        /// equivalent to a __threadfence_system() but scoped to the stream
        /// rather than a CUDA thread.
        ///
        /// If the memory was registered via ::cuMemHostRegister(), the device pointer
        /// should be obtained with ::cuMemHostGetDevicePointer(). This function cannot
        /// be used with managed memory (::cuMemAllocManaged).
        ///
        /// Support for this can be queried with ::cuDeviceGetAttribute() and
        /// ::CU_DEVICE_ATTRIBUTE_CAN_USE_STREAM_MEM_OPS. The only requirement for basic
        /// support is that on Windows, a device must be in TCC mode.</summary>
        ///
        /// <param name="stream">The stream to do the write in.</param>
        /// <param name="addr">The device address to write to.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="flags">See ::CUstreamWriteValue_flags.</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_NOT_SUPPORTED
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuStreamWriteValue64,
        /// ::cuStreamWaitValue32,
        /// ::cuStreamWaitValue64,
        /// ::cuStreamBatchMemOp,
        /// ::cuMemHostRegister,
        /// ::cuEventRecord
        /// CUresult CUDAAPI cuStreamWriteValue32(CUstream stream, CUdeviceptr addr, cuuint32_t value, unsigned int flags);
        [DllImport(_dllpath, EntryPoint = "cuStreamWriteValue32")]
        public static extern CuResult StreamWriteValue32(CuStream stream, CuDevicePtr addr, int value, CuStreamWriteValue flags);

        /// <summary>Write a value to memory
        ///
        /// Write a value to memory. Unless the ::CU_STREAM_WRITE_VALUE_NO_MEMORY_BARRIER
        /// flag is passed, the write is preceded by a system-wide memory fence,
        /// equivalent to a __threadfence_system() but scoped to the stream
        /// rather than a CUDA thread.
        ///
        /// If the memory was registered via ::cuMemHostRegister(), the device pointer
        /// should be obtained with ::cuMemHostGetDevicePointer().
        ///
        /// Support for this can be queried with ::cuDeviceGetAttribute() and
        /// ::CU_DEVICE_ATTRIBUTE_CAN_USE_64_BIT_STREAM_MEM_OPS. The requirements are
        /// compute capability 7.0 or greater, and on Windows, that the device be in
        /// TCC mode.</summary>
        ///
        /// <param name="stream">The stream to do the write in.</param>
        /// <param name="addr">The device address to write to.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="flags">See ::CUstreamWriteValue_flags.</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_NOT_SUPPORTED
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuStreamWriteValue32,
        /// ::cuStreamWaitValue32,
        /// ::cuStreamWaitValue64,
        /// ::cuStreamBatchMemOp,
        /// ::cuMemHostRegister,
        /// ::cuEventRecord
        /// CUresult CUDAAPI cuStreamWriteValue64(CUstream stream, CUdeviceptr addr, cuuint64_t value, unsigned int flags);
        [DllImport(_dllpath, EntryPoint = "cuStreamWriteValue64")]
        public static extern CuResult StreamWriteValue64(CuStream stream, CuDevicePtr addr, long value, int flags);

        /// <summary>Batch operations to synchronize the stream via memory operations
        ///
        /// This is a batch version of ::cuStreamWaitValue32() and ::cuStreamWriteValue32().
        /// Batching operations may avoid some performance overhead in both the API call
        /// and the device execution versus adding them to the stream in separate API
        /// calls. The operations are enqueued in the order they appear in the array.
        ///
        /// See ::CUstreamBatchMemOpType for the full set of supported operations, and
        /// ::cuStreamWaitValue32(), ::cuStreamWaitValue64(), ::cuStreamWriteValue32(),
        /// and ::cuStreamWriteValue64() for details of specific operations.
        ///
        /// Basic support for this can be queried with ::cuDeviceGetAttribute() and
        /// ::CU_DEVICE_ATTRIBUTE_CAN_USE_STREAM_MEM_OPS. See related APIs for details
        /// on querying support for specific operations.</summary>
        ///
        /// <param name="stream">The stream to enqueue the operations in.</param>
        /// <param name="count">The number of operations in the array. Must be less than 256.</param>
        /// <param name="paramArray">The types and parameters of the individual operations.</param>
        /// <param name="flags">Reserved for future expansion; must be 0.</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_NOT_SUPPORTED
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuStreamWaitValue32,
        /// ::cuStreamWaitValue64,
        /// ::cuStreamWriteValue32,
        /// ::cuStreamWriteValue64,
        /// ::cuMemHostRegister
        /// CUresult CUDAAPI cuStreamBatchMemOp(CUstream stream, unsigned int count, CUstreamBatchMemOpParams *paramArray, unsigned int flags);
        [DllImport(_dllpath, EntryPoint = "cuStreamBatchMemOp")]
        public static extern CuResult StreamBatchMemOp(CuStream stream, int count, CuSreamBatchMemOpParams* paramArray, int flags);
    }
}
