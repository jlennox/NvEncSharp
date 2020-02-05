// ReSharper disable UnusedMember.Global

using System;
using System.Runtime.InteropServices;

namespace Lennox.NvEncSharp
{
    /// \defgroup CUDA_UNIFIED Unified Addressing
    ///
    /// ___MANBRIEF___ unified addressing functions of the low-level CUDA driver
    /// API (___CURRENT_FILE___) ___ENDMANBRIEF___
    ///
    /// This section describes the unified addressing functions of the
    /// low-level CUDA driver application programming interface.
    ///
    /// @{
    ///
    /// \section CUDA_UNIFIED_overview Overview
    ///
    /// CUDA devices can share a unified address space with the host.
    /// For these devices there is no distinction between a device
    /// pointer and a host pointer -- the same pointer value may be
    /// used to access memory from the host program and from a kernel
    /// running on the device (with exceptions enumerated below).
    ///
    /// \section CUDA_UNIFIED_support Supported Platforms
    ///
    /// Whether or not a device supports unified addressing may be
    /// queried by calling ::cuDeviceGetAttribute() with the device
    /// attribute ::CU_DEVICE_ATTRIBUTE_UNIFIED_ADDRESSING.
    ///
    /// Unified addressing is automatically enabled in 64-bit processes
    ///
    /// \section CUDA_UNIFIED_lookup Looking Up Information from Pointer Values
    ///
    /// It is possible to look up information about the memory which backs a
    /// pointer value.  For instance, one may want to know if a pointer points
    /// to host or device memory.  As another example, in the case of device
    /// memory, one may want to know on which CUDA device the memory
    /// resides.  These properties may be queried using the function
    /// ::cuPointerGetAttribute()
    ///
    /// Since pointers are unique, it is not necessary to specify information
    /// about the pointers specified to the various copy functions in the
    /// CUDA API.  The function ::cuMemcpy() may be used to perform a copy
    /// between two pointers, ignoring whether they point to host or device
    /// memory (making ::cuMemcpyHtoD(), ::cuMemcpyDtoD(), and ::cuMemcpyDtoH()
    /// unnecessary for devices supporting unified addressing).  For
    /// multidimensional copies, the memory type ::CU_MEMORYTYPE_UNIFIED may be
    /// used to specify that the CUDA driver should infer the location of the
    /// pointer from its value.
    ///
    /// \section CUDA_UNIFIED_automaphost Automatic Mapping of Host Allocated Host Memory
    ///
    /// All host memory allocated in all contexts using ::cuMemAllocHost() and
    /// ::cuMemHostAlloc() is always directly accessible from all contexts on
    /// all devices that support unified addressing.  This is the case regardless
    /// of whether or not the flags ::CU_MEMHOSTALLOC_PORTABLE and
    /// ::CU_MEMHOSTALLOC_DEVICEMAP are specified.
    ///
    /// The pointer value through which allocated host memory may be accessed
    /// in kernels on all devices that support unified addressing is the same
    /// as the pointer value through which that memory is accessed on the host,
    /// so it is not necessary to call ::cuMemHostGetDevicePointer() to get the device
    /// pointer for these allocations.
    ///
    /// Note that this is not the case for memory allocated using the flag
    /// ::CU_MEMHOSTALLOC_WRITECOMBINED, as discussed below.
    ///
    /// \section CUDA_UNIFIED_autopeerregister Automatic Registration of Peer Memory
    ///
    /// Upon enabling direct access from a context that supports unified addressing
    /// to another peer context that supports unified addressing using
    /// ::cuCtxEnablePeerAccess() all memory allocated in the peer context using
    /// ::cuMemAlloc() and ::cuMemAllocPitch() will immediately be accessible
    /// by the current context.  The device pointer value through
    /// which any peer memory may be accessed in the current context
    /// is the same pointer value through which that memory may be
    /// accessed in the peer context.
    ///
    /// \section CUDA_UNIFIED_exceptions Exceptions, Disjoint Addressing
    ///
    /// Not all memory may be accessed on devices through the same pointer
    /// value through which they are accessed on the host.  These exceptions
    /// are host memory registered using ::cuMemHostRegister() and host memory
    /// allocated using the flag ::CU_MEMHOSTALLOC_WRITECOMBINED.  For these
    /// exceptions, there exists a distinct host and device address for the
    /// memory.  The device address is guaranteed to not overlap any valid host
    /// pointer range and is guaranteed to have the same value across all
    /// contexts that support unified addressing.
    ///
    /// This device address may be queried using ::cuMemHostGetDevicePointer()
    /// when a context using unified addressing is current.  Either the host
    /// or the unified device pointer value may be used to refer to this memory
    /// through ::cuMemcpy() and similar functions using the
    /// ::CU_MEMORYTYPE_UNIFIED memory type.
    public static partial class LibCuda
    {
        /// <summary>Returns information about a pointer
        ///
        /// The supported attributes are:
        ///
        /// - ::CU_POINTER_ATTRIBUTE_CONTEXT:
        ///
        ///      Returns in *<paramref name="data"/> the ::CUcontext in which <paramref name="ptr"/> was allocated or
        ///      registered.
        ///      The type of <paramref name="data"/> must be ::CUcontext *.
        ///
        ///      If <paramref name="ptr"/> was not allocated by, mapped by, or registered with
        ///      a ::CUcontext which uses unified virtual addressing then
        ///      ::CUDA_ERROR_INVALID_VALUE is returned.
        ///
        /// - ::CU_POINTER_ATTRIBUTE_MEMORY_TYPE:
        ///
        ///      Returns in *<paramref name="data"/> the physical memory type of the memory that
        ///      <paramref name="ptr"/> addresses as a ::CUmemorytype enumerated value.
        ///      The type of <paramref name="data"/> must be unsigned int.
        ///
        ///      If <paramref name="ptr"/> addresses device memory then *<paramref name="data"/> is set to
        ///      ::CU_MEMORYTYPE_DEVICE.  The particular ::CUdevice on which the
        ///      memory resides is the ::CUdevice of the ::CUcontext returned by the
        ///      ::CU_POINTER_ATTRIBUTE_CONTEXT attribute of <paramref name="ptr."/>
        ///
        ///      If <paramref name="ptr"/> addresses host memory then *<paramref name="data"/> is set to
        ///      ::CU_MEMORYTYPE_HOST.
        ///
        ///      If <paramref name="ptr"/> was not allocated by, mapped by, or registered with
        ///      a ::CUcontext which uses unified virtual addressing then
        ///      ::CUDA_ERROR_INVALID_VALUE is returned.
        ///
        ///      If the current ::CUcontext does not support unified virtual
        ///      addressing then ::CUDA_ERROR_INVALID_CONTEXT is returned.
        ///
        /// - ::CU_POINTER_ATTRIBUTE_DEVICE_POINTER:
        ///
        ///      Returns in *<paramref name="data"/> the device pointer value through which
        ///      <paramref name="ptr"/> may be accessed by kernels running in the current
        ///      ::CUcontext.
        ///      The type of <paramref name="data"/> must be CUdeviceptr *.
        ///
        ///      If there exists no device pointer value through which
        ///      kernels running in the current ::CUcontext may access
        ///      <paramref name="ptr"/> then ::CUDA_ERROR_INVALID_VALUE is returned.
        ///
        ///      If there is no current ::CUcontext then
        ///      ::CUDA_ERROR_INVALID_CONTEXT is returned.
        ///
        ///      Except in the exceptional disjoint addressing cases discussed
        ///      below, the value returned in *<paramref name="data"/> will equal the input
        ///      value <paramref name="ptr."/>
        ///
        /// - ::CU_POINTER_ATTRIBUTE_HOST_POINTER:
        ///
        ///      Returns in *<paramref name="data"/> the host pointer value through which
        ///      <paramref name="ptr"/> may be accessed by by the host program.
        ///      The type of <paramref name="data"/> must be void **.
        ///      If there exists no host pointer value through which
        ///      the host program may directly access <paramref name="ptr"/> then
        ///      ::CUDA_ERROR_INVALID_VALUE is returned.
        ///
        ///      Except in the exceptional disjoint addressing cases discussed
        ///      below, the value returned in *<paramref name="data"/> will equal the input
        ///      value <paramref name="ptr."/>
        ///
        /// - ::CU_POINTER_ATTRIBUTE_P2P_TOKENS:
        ///
        ///      Returns in *<paramref name="data"/> two tokens for use with the nv-p2p.h Linux
        ///      kernel interface. <paramref name="data"/> must be a struct of type
        ///      CUDA_POINTER_ATTRIBUTE_P2P_TOKENS.
        ///
        ///      <paramref name="ptr"/> must be a pointer to memory obtained from :cuMemAlloc().
        ///      Note that p2pToken and vaSpaceToken are only valid for the
        ///      lifetime of the source allocation. A subsequent allocation at
        ///      the same address may return completely different tokens.
        ///      Querying this attribute has a side effect of setting the attribute
        ///      ::CU_POINTER_ATTRIBUTE_SYNC_MEMOPS for the region of memory that
        ///      <paramref name="ptr"/> points to.
        ///
        /// - ::CU_POINTER_ATTRIBUTE_SYNC_MEMOPS:
        ///
        ///      A boolean attribute which when set, ensures that synchronous memory operations
        ///      initiated on the region of memory that <paramref name="ptr"/> points to will always synchronize.
        ///      See further documentation in the section titled "API synchronization behavior"
        ///      to learn more about cases when synchronous memory operations can
        ///      exhibit asynchronous behavior.
        ///
        /// - ::CU_POINTER_ATTRIBUTE_BUFFER_ID:
        ///
        ///      Returns in *<paramref name="data"/> a buffer ID which is guaranteed to be unique within the process.
        ///      <paramref name="data"/> must point to an unsigned long long.
        ///
        ///      <paramref name="ptr"/> must be a pointer to memory obtained from a CUDA memory allocation API.
        ///      Every memory allocation from any of the CUDA memory allocation APIs will
        ///      have a unique ID over a process lifetime. Subsequent allocations do not reuse IDs
        ///      from previous freed allocations. IDs are only unique within a single process.
        ///
        ///
        /// - ::CU_POINTER_ATTRIBUTE_IS_MANAGED:
        ///
        ///      Returns in *<paramref name="data"/> a boolean that indicates whether the pointer points to
        ///      managed memory or not.
        ///
        /// \par
        ///
        /// Note that for most allocations in the unified virtual address space
        /// the host and device pointer for accessing the allocation will be the
        /// same.  The exceptions to this are
        ///  - user memory registered using ::cuMemHostRegister
        ///  - host memory allocated using ::cuMemHostAlloc with the
        ///    ::CU_MEMHOSTALLOC_WRITECOMBINED flag
        /// For these types of allocation there will exist separate, disjoint host
        /// and device addresses for accessing the allocation.  In particular
        ///  - The host address will correspond to an invalid unmapped device address
        ///    (which will result in an exception if accessed from the device)
        ///  - The device address will correspond to an invalid unmapped host address
        ///    (which will result in an exception if accessed from the host).
        /// For these types of allocations, querying ::CU_POINTER_ATTRIBUTE_HOST_POINTER
        /// and ::CU_POINTER_ATTRIBUTE_DEVICE_POINTER may be used to retrieve the host
        /// and device addresses from either address.</summary>
        ///
        /// <param name="data">Returned pointer attribute value</param>
        /// <param name="attribute">Pointer attribute to query</param>
        /// <param name="ptr">Pointer</param>
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
        /// \sa
        /// ::cuPointerSetAttribute,
        /// ::cuMemAlloc,
        /// ::cuMemFree,
        /// ::cuMemAllocHost,
        /// ::cuMemFreeHost,
        /// ::cuMemHostAlloc,
        /// ::cuMemHostRegister,
        /// ::cuMemHostUnregister,
        /// ::cudaPointerGetAttributes
        /// CUresult CUDAAPI cuPointerGetAttribute(void *data, CUpointer_attribute attribute, CUdeviceptr ptr);
        [DllImport(_dllpath, EntryPoint = "cuPointerGetAttribute")]
        public static extern CuResult PointerGetAttribute(IntPtr data, PointerAttribute attribute, CuDevicePtr ptr);

        /// <summary>Prefetches memory to the specified destination device
        ///
        /// Prefetches memory to the specified destination device.  <paramref name="devPtr"/> is the
        /// base device pointer of the memory to be prefetched and <paramref name="dstDevice"/> is the
        /// destination device. <paramref name="count"/> specifies the number of bytes to copy. <paramref name="hStream"/>
        /// is the stream in which the operation is enqueued. The memory range must refer
        /// to managed memory allocated via ::cuMemAllocManaged or declared via __managed__ variables.
        ///
        /// Passing in CU_DEVICE_CPU for <paramref name="dstDevice"/> will prefetch the data to host memory. If
        /// <paramref name="dstDevice"/> is a GPU, then the device attribute ::CU_DEVICE_ATTRIBUTE_CONCURRENT_MANAGED_ACCESS
        /// must be non-zero. Additionally, <paramref name="hStream"/> must be associated with a device that has a
        /// non-zero value for the device attribute ::CU_DEVICE_ATTRIBUTE_CONCURRENT_MANAGED_ACCESS.
        ///
        /// The start address and end address of the memory range will be rounded down and rounded up
        /// respectively to be aligned to CPU page size before the prefetch operation is enqueued
        /// in the stream.
        ///
        /// If no physical memory has been allocated for this region, then this memory region
        /// will be populated and mapped on the destination device. If there's insufficient
        /// memory to prefetch the desired region, the Unified Memory driver may evict pages from other
        /// ::cuMemAllocManaged allocations to host memory in order to make room. Device memory
        /// allocated using ::cuMemAlloc or ::cuArrayCreate will not be evicted.
        ///
        /// By default, any mappings to the previous location of the migrated pages are removed and
        /// mappings for the new location are only setup on <paramref name="dstDevice"/>. The exact behavior however
        /// also depends on the settings applied to this memory range via ::cuMemAdvise as described
        /// below:
        ///
        /// If ::CU_MEM_ADVISE_SET_READ_MOSTLY was set on any subset of this memory range,
        /// then that subset will create a read-only copy of the pages on <paramref name="dstDevice."/>
        ///
        /// If ::CU_MEM_ADVISE_SET_PREFERRED_LOCATION was called on any subset of this memory
        /// range, then the pages will be migrated to <paramref name="dstDevice"/> even if <paramref name="dstDevice"/> is not the
        /// preferred location of any pages in the memory range.
        ///
        /// If ::CU_MEM_ADVISE_SET_ACCESSED_BY was called on any subset of this memory range,
        /// then mappings to those pages from all the appropriate processors are updated to
        /// refer to the new location if establishing such a mapping is possible. Otherwise,
        /// those mappings are cleared.
        ///
        /// Note that this API is not required for functionality and only serves to improve performance
        /// by allowing the application to migrate data to a suitable location before it is accessed.
        /// Memory accesses to this range are always coherent and are allowed even when the data is
        /// actively being migrated.
        ///
        /// Note that this function is asynchronous with respect to the host and all work
        /// on other devices.</summary>
        ///
        /// <param name="devPtr">Pointer to be prefetched</param>
        /// <param name="count">Size in bytes</param>
        /// <param name="dstDevice">Destination device to prefetch to</param>
        /// <param name="hStream">Stream to enqueue prefetch operation</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_INVALID_DEVICE
        /// </returns>
        /// \notefnerr
        /// \note_async
        /// \note_null_stream
        ///
        /// \sa ::cuMemcpy, ::cuMemcpyPeer, ::cuMemcpyAsync,
        /// ::cuMemcpy3DPeerAsync, ::cuMemAdvise,
        /// ::cudaMemPrefetchAsync
        /// CUresult CUDAAPI cuMemPrefetchAsync(CUdeviceptr devPtr, size_t count, CUdevice dstDevice, CUstream hStream);
        [DllImport(_dllpath, EntryPoint = "cuMemPrefetchAsync")]
        public static extern CuResult MemPrefetchAsync(CuDevicePtr devPtr, IntPtr count, CuDevice dstDevice, CuStream hStream);

        /// <summary>Advise about the usage of a given memory range
        ///
        /// Advise the Unified Memory subsystem about the usage pattern for the memory range
        /// starting at <paramref name="devPtr"/> with a size of <paramref name="count"/> bytes. The start address and end address of the memory
        /// range will be rounded down and rounded up respectively to be aligned to CPU page size before the
        /// advice is applied. The memory range must refer to managed memory allocated via ::cuMemAllocManaged
        /// or declared via __managed__ variables.
        ///
        /// The <paramref name="advice"/> parameter can take the following values:
        /// - ::CU_MEM_ADVISE_SET_READ_MOSTLY: This implies that the data is mostly going to be read
        /// from and only occasionally written to. Any read accesses from any processor to this region will create a
        /// read-only copy of at least the accessed pages in that processor's memory. Additionally, if ::cuMemPrefetchAsync
        /// is called on this region, it will create a read-only copy of the data on the destination processor.
        /// If any processor writes to this region, all copies of the corresponding page will be invalidated
        /// except for the one where the write occurred. The <paramref name="device"/> argument is ignored for this advice.
        /// Note that for a page to be read-duplicated, the accessing processor must either be the CPU or a GPU
        /// that has a non-zero value for the device attribute ::CU_DEVICE_ATTRIBUTE_CONCURRENT_MANAGED_ACCESS.
        /// Also, if a context is created on a device that does not have the device attribute
        /// ::CU_DEVICE_ATTRIBUTE_CONCURRENT_MANAGED_ACCESS set, then read-duplication will not occur until
        /// all such contexts are destroyed.
        /// - ::CU_MEM_ADVISE_UNSET_READ_MOSTLY:  Undoes the effect of ::CU_MEM_ADVISE_SET_READ_MOSTLY and also prevents the
        /// Unified Memory driver from attempting heuristic read-duplication on the memory range. Any read-duplicated
        /// copies of the data will be collapsed into a single copy. The location for the collapsed
        /// copy will be the preferred location if the page has a preferred location and one of the read-duplicated
        /// copies was resident at that location. Otherwise, the location chosen is arbitrary.
        /// - ::CU_MEM_ADVISE_SET_PREFERRED_LOCATION: This advice sets the preferred location for the
        /// data to be the memory belonging to <paramref name="device"/>. Passing in CU_DEVICE_CPU for <paramref name="device"/> sets the
        /// preferred location as host memory. If <paramref name="device"/> is a GPU, then it must have a non-zero value for the
        /// device attribute ::CU_DEVICE_ATTRIBUTE_CONCURRENT_MANAGED_ACCESS. Setting the preferred location
        /// does not cause data to migrate to that location immediately. Instead, it guides the migration policy
        /// when a fault occurs on that memory region. If the data is already in its preferred location and the
        /// faulting processor can establish a mapping without requiring the data to be migrated, then
        /// data migration will be avoided. On the other hand, if the data is not in its preferred location
        /// or if a direct mapping cannot be established, then it will be migrated to the processor accessing
        /// it. It is important to note that setting the preferred location does not prevent data prefetching
        /// done using ::cuMemPrefetchAsync.
        /// Having a preferred location can override the page thrash detection and resolution logic in the Unified
        /// Memory driver. Normally, if a page is detected to be constantly thrashing between for example host and device
        /// memory, the page may eventually be pinned to host memory by the Unified Memory driver. But
        /// if the preferred location is set as device memory, then the page will continue to thrash indefinitely.
        /// If ::CU_MEM_ADVISE_SET_READ_MOSTLY is also set on this memory region or any subset of it, then the
        /// policies associated with that advice will override the policies of this advice.
        /// - ::CU_MEM_ADVISE_UNSET_PREFERRED_LOCATION: Undoes the effect of ::CU_MEM_ADVISE_SET_PREFERRED_LOCATION
        /// and changes the preferred location to none.
        /// - ::CU_MEM_ADVISE_SET_ACCESSED_BY: This advice implies that the data will be accessed by <paramref name="device"/>.
        /// Passing in ::CU_DEVICE_CPU for <paramref name="device"/> will set the advice for the CPU. If <paramref name="device"/> is a GPU, then
        /// the device attribute ::CU_DEVICE_ATTRIBUTE_CONCURRENT_MANAGED_ACCESS must be non-zero.
        /// This advice does not cause data migration and has no impact on the location of the data per se. Instead,
        /// it causes the data to always be mapped in the specified processor's page tables, as long as the
        /// location of the data permits a mapping to be established. If the data gets migrated for any reason,
        /// the mappings are updated accordingly.
        /// This advice is recommended in scenarios where data locality is not important, but avoiding faults is.
        /// Consider for example a system containing multiple GPUs with peer-to-peer access enabled, where the
        /// data located on one GPU is occasionally accessed by peer GPUs. In such scenarios, migrating data
        /// over to the other GPUs is not as important because the accesses are infrequent and the overhead of
        /// migration may be too high. But preventing faults can still help improve performance, and so having
        /// a mapping set up in advance is useful. Note that on CPU access of this data, the data may be migrated
        /// to host memory because the CPU typically cannot access device memory directly. Any GPU that had the
        /// ::CU_MEM_ADVISE_SET_ACCESSED_BY flag set for this data will now have its mapping updated to point to the
        /// page in host memory.
        /// If ::CU_MEM_ADVISE_SET_READ_MOSTLY is also set on this memory region or any subset of it, then the
        /// policies associated with that advice will override the policies of this advice. Additionally, if the
        /// preferred location of this memory region or any subset of it is also <paramref name="device"/>, then the policies
        /// associated with ::CU_MEM_ADVISE_SET_PREFERRED_LOCATION will override the policies of this advice.
        /// - ::CU_MEM_ADVISE_UNSET_ACCESSED_BY: Undoes the effect of ::CU_MEM_ADVISE_SET_ACCESSED_BY. Any mappings to
        /// the data from <paramref name="device"/> may be removed at any time causing accesses to result in non-fatal page faults.</summary>
        ///
        /// <param name="devPtr">Pointer to memory to set the advice for</param>
        /// <param name="count">Size in bytes of the memory range</param>
        /// <param name="advice">Advice to be applied for the specified memory range</param>
        /// <param name="device">Device to apply the advice for</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_INVALID_DEVICE
        /// </returns>
        /// \notefnerr
        /// \note_async
        /// \note_null_stream
        ///
        /// \sa ::cuMemcpy, ::cuMemcpyPeer, ::cuMemcpyAsync,
        /// ::cuMemcpy3DPeerAsync, ::cuMemPrefetchAsync,
        /// ::cudaMemAdvise
        /// CUresult CUDAAPI cuMemAdvise(CUdeviceptr devPtr, size_t count, CUmem_advise advice, CUdevice device);
        [DllImport(_dllpath, EntryPoint = "cuMemAdvise")]
        public static extern CuResult MemAdvise(CuDevicePtr devPtr, IntPtr count, CuMemAdvice advice, CuDevice device);

        /// <summary>Query an attribute of a given memory range
        ///
        /// Query an attribute about the memory range starting at <paramref name="devPtr"/> with a size of <paramref name="count"/> bytes. The
        /// memory range must refer to managed memory allocated via ::cuMemAllocManaged or declared via
        /// __managed__ variables.
        ///
        /// The <paramref name="attribute"/> parameter can take the following values:
        /// - ::CU_MEM_RANGE_ATTRIBUTE_READ_MOSTLY: If this attribute is specified, <paramref name="data"/> will be interpreted
        /// as a 32-bit integer, and <paramref name="dataSize"/> must be 4. The result returned will be 1 if all pages in the given
        /// memory range have read-duplication enabled, or 0 otherwise.
        /// - ::CU_MEM_RANGE_ATTRIBUTE_PREFERRED_LOCATION: If this attribute is specified, <paramref name="data"/> will be
        /// interpreted as a 32-bit integer, and <paramref name="dataSize"/> must be 4. The result returned will be a GPU device
        /// id if all pages in the memory range have that GPU as their preferred location, or it will be CU_DEVICE_CPU
        /// if all pages in the memory range have the CPU as their preferred location, or it will be CU_DEVICE_INVALID
        /// if either all the pages don't have the same preferred location or some of the pages don't have a
        /// preferred location at all. Note that the actual location of the pages in the memory range at the time of
        /// the query may be different from the preferred location.
        /// - ::CU_MEM_RANGE_ATTRIBUTE_ACCESSED_BY: If this attribute is specified, <paramref name="data"/> will be interpreted
        /// as an array of 32-bit integers, and <paramref name="dataSize"/> must be a non-zero multiple of 4. The result returned
        /// will be a list of device ids that had ::CU_MEM_ADVISE_SET_ACCESSED_BY set for that entire memory range.
        /// If any device does not have that advice set for the entire memory range, that device will not be included.
        /// If <paramref name="data"/> is larger than the number of devices that have that advice set for that memory range,
        /// CU_DEVICE_INVALID will be returned in all the extra space provided. For ex., if <paramref name="dataSize"/> is 12
        /// (i.e. <paramref name="data"/> has 3 elements) and only device 0 has the advice set, then the result returned will be
        /// { 0, CU_DEVICE_INVALID, CU_DEVICE_INVALID }. If <paramref name="data"/> is smaller than the number of devices that have
        /// that advice set, then only as many devices will be returned as can fit in the array. There is no
        /// guarantee on which specific devices will be returned, however.
        /// - ::CU_MEM_RANGE_ATTRIBUTE_LAST_PREFETCH_LOCATION: If this attribute is specified, <paramref name="data"/> will be
        /// interpreted as a 32-bit integer, and <paramref name="dataSize"/> must be 4. The result returned will be the last location
        /// to which all pages in the memory range were prefetched explicitly via ::cuMemPrefetchAsync. This will either be
        /// a GPU id or CU_DEVICE_CPU depending on whether the last location for prefetch was a GPU or the CPU
        /// respectively. If any page in the memory range was never explicitly prefetched or if all pages were not
        /// prefetched to the same location, CU_DEVICE_INVALID will be returned. Note that this simply returns the
        /// last location that the applicaton requested to prefetch the memory range to. It gives no indication as to
        /// whether the prefetch operation to that location has completed or even begun.</summary>
        ///
        /// <param name="data">A pointers to a memory location where the result
        ///                    of each attribute query will be written to.</param>
        /// <param name="dataSize">Array containing the size of data</param>
        /// <param name="attribute">The attribute to query</param>
        /// <param name="devPtr">Start of the range to query</param>
        /// <param name="count">Size of the range to query</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_INVALID_DEVICE
        /// </returns>
        /// \notefnerr
        /// \note_async
        /// \note_null_stream
        ///
        /// \sa ::cuMemRangeGetAttributes, ::cuMemPrefetchAsync,
        /// ::cuMemAdvise,
        /// ::cudaMemRangeGetAttribute
        /// CUresult CUDAAPI cuMemRangeGetAttribute(void *data, size_t dataSize, CUmem_range_attribute attribute, CUdeviceptr devPtr, size_t count);
        [DllImport(_dllpath, EntryPoint = "cuMemRangeGetAttribute")]
        public static extern CuResult MemRangeGetAttribute(IntPtr data, IntPtr dataSize, CuMemRangeAttribute attribute, CuDevicePtr devPtr, IntPtr count);

        /// <summary>Query attributes of a given memory range.
        ///
        /// Query attributes of the memory range starting at <paramref name="devPtr"/> with a size of <paramref name="count"/> bytes. The
        /// memory range must refer to managed memory allocated via ::cuMemAllocManaged or declared via
        /// __managed__ variables. The <paramref name="attributes"/> array will be interpreted to have <paramref name="numAttributes"/>
        /// entries. The <paramref name="dataSizes"/> array will also be interpreted to have <paramref name="numAttributes"/> entries.
        /// The results of the query will be stored in <paramref name="data."/>
        ///
        /// The list of supported attributes are given below. Please refer to ::cuMemRangeGetAttribute for
        /// attribute descriptions and restrictions.
        ///
        /// - ::CU_MEM_RANGE_ATTRIBUTE_READ_MOSTLY
        /// - ::CU_MEM_RANGE_ATTRIBUTE_PREFERRED_LOCATION
        /// - ::CU_MEM_RANGE_ATTRIBUTE_ACCESSED_BY
        /// - ::CU_MEM_RANGE_ATTRIBUTE_LAST_PREFETCH_LOCATION</summary>
        ///
        /// <param name="data">A two-dimensional array containing pointers to memory
        ///                        locations where the result of each attribute query will be written to.</param>
        /// <param name="dataSizes">Array containing the sizes of each result</param>
        /// <param name="attributes">An array of attributes to query
        ///                        (numAttributes and the number of attributes in this array should match)</param>
        /// <param name="numAttributes">Number of attributes to query</param>
        /// <param name="devPtr">Start of the range to query</param>
        /// <param name="count">Size of the range to query</param>
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
        /// \sa ::cuMemRangeGetAttribute, ::cuMemAdvise
        /// ::cuMemPrefetchAsync,
        /// ::cudaMemRangeGetAttributes
        /// CUresult CUDAAPI cuMemRangeGetAttributes(void **data, size_t *dataSizes, CUmem_range_attribute *attributes, size_t numAttributes, CUdeviceptr devPtr, size_t count);
        [DllImport(_dllpath, EntryPoint = "cuMemRangeGetAttributes")]
        public static extern CuResult MemRangeGetAttributes(IntPtr data, IntPtr dataSizes, [MarshalAs(UnmanagedType.LPArray)] CuMemRangeAttribute[] attributes, IntPtr numAttributes, CuDevicePtr devPtr, IntPtr count);
    }
}
