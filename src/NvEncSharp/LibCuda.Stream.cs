using System;
using System.Runtime.InteropServices;

namespace Lennox.NvEncSharp
{
    public static partial class LibCuda
    {
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
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_OUT_OF_MEMORY
        /// </returns>
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
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_OUT_OF_MEMORY
        /// </returns>
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
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// \note_null_stream
        /// </returns>
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
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_NOT_SUPPORTED
        /// \note_null_stream
        /// </returns>
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
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_NOT_SUPPORTED
        /// \note_null_stream
        /// </returns>
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
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_NOT_READY
        /// \note_null_stream
        /// </returns>
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
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_HANDLE
        /// \note_null_stream
        /// </returns>
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
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuStreamCreate,
        /// ::cuStreamWaitEvent,
        /// ::cuStreamQuery,
        /// ::cuStreamSynchronize,
        /// ::cuStreamAddCallback,
        /// ::cudaStreamDestroy
        /// CUresult CUDAAPI cuStreamDestroy(CUstream hStream);
        [DllImport(_dllpath, EntryPoint = "cuStreamDestroy" + _ver)]
        public static extern CuResult StreamDestroy(CuStream hStream);
    }
}