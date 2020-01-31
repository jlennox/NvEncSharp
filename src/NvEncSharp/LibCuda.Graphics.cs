using System;
using System.Runtime.InteropServices;

namespace Lennox.NvEncSharp
{
    public unsafe partial class LibCuda
    {
        /// <summary>Unregisters a graphics resource for access by CUDA
        ///
        /// Unregisters the graphics resource \p resource so it is not accessible by
        /// CUDA unless registered again.
        ///
        /// If \p resource is invalid then ::CUDA_ERROR_INVALID_HANDLE is
        /// returned.</summary>
        ///
        /// <param name="resource">Resource to unregister</param>
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_UNKNOWN
        /// </returns>
        /// \notefnerr
        ///
        /// \sa
        /// ::cuGraphicsD3D9RegisterResource,
        /// ::cuGraphicsD3D10RegisterResource,
        /// ::cuGraphicsD3D11RegisterResource,
        /// ::cuGraphicsGLRegisterBuffer,
        /// ::cuGraphicsGLRegisterImage,
        /// ::cudaGraphicsUnregisterResource
        /// CUresult CUDAAPI cuGraphicsUnregisterResource(CUgraphicsResource resource);
        [DllImport(_dllpath, EntryPoint = "cuGraphicsUnregisterResource")]
        public static extern CuResult GraphicsUnregisterResource(CuGraphicsResource resource);

        /// <summary>Get an array through which to access a subresource of a mapped graphics resource.
        ///
        /// Returns in \p *pArray an array through which the subresource of the mapped
        /// graphics resource \p resource which corresponds to array index \p arrayIndex
        /// and mipmap level \p mipLevel may be accessed.  The value set in \p *pArray may
        /// change every time that \p resource is mapped.
        ///
        /// If \p resource is not a texture then it cannot be accessed via an array and
        /// ::CUDA_ERROR_NOT_MAPPED_AS_ARRAY is returned.
        /// If \p arrayIndex is not a valid array index for \p resource then
        /// ::CUDA_ERROR_INVALID_VALUE is returned.
        /// If \p mipLevel is not a valid mipmap level for \p resource then
        /// ::CUDA_ERROR_INVALID_VALUE is returned.
        /// If \p resource is not mapped then ::CUDA_ERROR_NOT_MAPPED is returned.</summary>
        ///
        /// <param name="pArray">Returned array through which a subresource of \p resource may be accessed</param>
        /// <param name="resource">Mapped resource to access</param>
        /// <param name="arrayIndex">Array index for array textures or cubemap face
        ///                      index as defined by ::CUarray_cubemap_face for
        ///                      cubemap textures for the subresource to access</param>
        /// <param name="mipLevel">Mipmap level for the subresource to access</param>
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_NOT_MAPPED,
        /// ::CUDA_ERROR_NOT_MAPPED_AS_ARRAY
        /// </returns>
        /// \notefnerr
        ///
        /// \sa
        /// ::cuGraphicsResourceGetMappedPointer,
        /// ::cudaGraphicsSubResourceGetMappedArray
        /// CUresult CUDAAPI cuGraphicsSubResourceGetMappedArray(CUarray *pArray, CUgraphicsResource resource, unsigned int arrayIndex, unsigned int mipLevel);
        [DllImport(_dllpath, EntryPoint = "cuGraphicsSubResourceGetMappedArray")]
        public static extern CuResult GraphicsSubResourceGetMappedArray(out CuArray pArray, CuGraphicsResource resource, int arrayIndex, int mipLevel);

        /// <summary>Get a mipmapped array through which to access a mapped graphics resource.
        ///
        /// Returns in \p *pMipmappedArray a mipmapped array through which the mapped graphics
        /// resource \p resource. The value set in \p *pMipmappedArray may change every time
        /// that \p resource is mapped.
        ///
        /// If \p resource is not a texture then it cannot be accessed via a mipmapped array and
        /// ::CUDA_ERROR_NOT_MAPPED_AS_ARRAY is returned.
        /// If \p resource is not mapped then ::CUDA_ERROR_NOT_MAPPED is returned.</summary>
        ///
        /// <param name="pMipmappedArray">Returned mipmapped array through which \p resource may be accessed</param>
        /// <param name="resource">Mapped resource to access</param>
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_NOT_MAPPED,
        /// ::CUDA_ERROR_NOT_MAPPED_AS_ARRAY
        /// </returns>
        /// \notefnerr
        ///
        /// \sa
        /// ::cuGraphicsResourceGetMappedPointer,
        /// ::cudaGraphicsResourceGetMappedMipmappedArray
        /// CUresult CUDAAPI cuGraphicsResourceGetMappedMipmappedArray(CUmipmappedArray *pMipmappedArray, CUgraphicsResource resource);
        [DllImport(_dllpath, EntryPoint = "cuGraphicsResourceGetMappedMipmappedArray")]
        public static extern CuResult GraphicsResourceGetMappedMipmappedArray(out CuMipMappedArray pMipmappedArray, CuGraphicsResource resource);

        /// <summary>Get a device pointer through which to access a mapped graphics resource.
        ///
        /// Returns in \p *pDevPtr a pointer through which the mapped graphics resource
        /// \p resource may be accessed.
        /// Returns in \p pSize the size of the memory in bytes which may be accessed from that pointer.
        /// The value set in \p pPointer may change every time that \p resource is mapped.
        ///
        /// If \p resource is not a buffer then it cannot be accessed via a pointer and
        /// ::CUDA_ERROR_NOT_MAPPED_AS_POINTER is returned.
        /// If \p resource is not mapped then ::CUDA_ERROR_NOT_MAPPED is returned.
        /// *
        /// </summary>
        /// <param name="pDevPtr">Returned pointer through which \p resource may be accessed</param>
        /// <param name="pSize">Returned size of the buffer accessible starting at \p *pPointer</param>
        /// <param name="resource">Mapped resource to access</param>
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_NOT_MAPPED,
        /// ::CUDA_ERROR_NOT_MAPPED_AS_POINTER
        /// </returns>
        /// \notefnerr
        ///
        /// \sa
        /// ::cuGraphicsMapResources,
        /// ::cuGraphicsSubResourceGetMappedArray,
        /// ::cudaGraphicsResourceGetMappedPointer
        /// CUresult CUDAAPI cuGraphicsResourceGetMappedPointer(CUdeviceptr *pDevPtr, size_t *pSize, CUgraphicsResource resource);
        [DllImport(_dllpath, EntryPoint = "cuGraphicsResourceGetMappedPointer")]
        public static extern CuResult GraphicsResourceGetMappedPointer(out CuDevicePtr pDevPtr, out IntPtr pSize, CuGraphicsResource resource);

        /// <summary>Set usage flags for mapping a graphics resource</summary>
        ///
        /// <remarks>
        /// Set <paramref name="flags"/> for mapping the graphics resource <paramref name="resource"/>.
        ///
        /// Changes to <paramref name="flags"/> will take effect the next time <paramref name="resource"/> is mapped.
        /// The <paramref name="flags"/> argument may be any of the following:
        /// - ::CU_GRAPHICS_MAP_RESOURCE_FLAGS_NONE: Specifies no hints about how this
        ///   resource will be used. It is therefore assumed that this resource will be
        ///   read from and written to by CUDA kernels.  This is the default value.
        /// - ::CU_GRAPHICS_MAP_RESOURCE_FLAGS_READONLY: Specifies that CUDA kernels which
        ///   access this resource will not write to this resource.
        /// - ::CU_GRAPHICS_MAP_RESOURCE_FLAGS_WRITEDISCARD: Specifies that CUDA kernels
        ///   which access this resource will not read from this resource and will
        ///   write over the entire contents of the resource, so none of the data
        ///   previously stored in the resource will be preserved.
        ///
        /// If <paramref name="resource"/> is presently mapped for access by CUDA then
        /// ::CUDA_ERROR_ALREADY_MAPPED is returned.
        /// If <paramref name="flags"/> is not one of the above values then ::CUDA_ERROR_INVALID_VALUE is returned.
        /// </remarks>
        ///
        /// <param name="resource">Registered resource to set flags for</param>
        /// <param name="flags">Parameters for resource mapping</param>
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_ALREADY_MAPPED
        /// </returns>
        /// \notefnerr
        ///
        /// \sa
        /// ::cuGraphicsMapResources,
        /// ::cudaGraphicsResourceSetMapFlags
        /// CUresult CUDAAPI cuGraphicsResourceSetMapFlags(CUgraphicsResource resource, unsigned int flags);
        [DllImport(_dllpath, EntryPoint = "cuGraphicsResourceSetMapFlags" + _ver)]
        public static extern CuResult GraphicsResourceSetMapFlags(CuGraphicsResource resource, CuGraphicsMapResources flags);

        /// <summary>Map graphics resources for access by CUDA
        ///
        /// Maps the \p count graphics resources in \p resources for access by CUDA.
        ///
        /// The resources in \p resources may be accessed by CUDA until they
        /// are unmapped. The graphics API from which \p resources were registered
        /// should not access any resources while they are mapped by CUDA. If an
        /// application does so, the results are undefined.
        ///
        /// This function provides the synchronization guarantee that any graphics calls
        /// issued before ::cuGraphicsMapResources() will complete before any subsequent CUDA
        /// work issued in \p stream begins.
        ///
        /// If \p resources includes any duplicate entries then ::CUDA_ERROR_INVALID_HANDLE is returned.
        /// If any of \p resources are presently mapped for access by CUDA then ::CUDA_ERROR_ALREADY_MAPPED is returned.</summary>
        ///
        /// <param name="count">Number of resources to map</param>
        /// <param name="resources">Resources to map for CUDA usage</param>
        /// <param name="hStream">Stream with which to synchronize</param>
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_ALREADY_MAPPED,
        /// ::CUDA_ERROR_UNKNOWN
        /// \note_null_stream
        /// </returns>
        /// \notefnerr
        ///
        /// \sa
        /// ::cuGraphicsResourceGetMappedPointer,
        /// ::cuGraphicsSubResourceGetMappedArray,
        /// ::cuGraphicsUnmapResources,
        /// ::cudaGraphicsMapResources
        /// CUresult CUDAAPI cuGraphicsMapResources(unsigned int count, CUgraphicsResource *resources, CUstream hStream);
        [DllImport(_dllpath, EntryPoint = "cuGraphicsMapResources")]
        public static extern CuResult GraphicsMapResources(int count, CuGraphicsResource* resources, CuStream hStream);

        /// <summary>Unmap graphics resources.
        ///
        /// Unmaps the \p count graphics resources in \p resources.
        ///
        /// Once unmapped, the resources in \p resources may not be accessed by CUDA
        /// until they are mapped again.
        ///
        /// This function provides the synchronization guarantee that any CUDA work issued
        /// in \p stream before ::cuGraphicsUnmapResources() will complete before any
        /// subsequently issued graphics work begins.
        ///
        ///
        /// If \p resources includes any duplicate entries then ::CUDA_ERROR_INVALID_HANDLE is returned.
        /// If any of \p resources are not presently mapped for access by CUDA then ::CUDA_ERROR_NOT_MAPPED is returned.</summary>
        ///
        /// <param name="count">Number of resources to unmap</param>
        /// <param name="resources">Resources to unmap</param>
        /// <param name="hStream">Stream with which to synchronize</param>
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_NOT_MAPPED,
        /// ::CUDA_ERROR_UNKNOWN
        /// \note_null_stream
        /// </returns>
        /// \notefnerr
        ///
        /// \sa
        /// ::cuGraphicsMapResources,
        /// ::cudaGraphicsUnmapResources
        /// CUresult CUDAAPI cuGraphicsUnmapResources(unsigned int count, CUgraphicsResource *resources, CUstream hStream);
        [DllImport(_dllpath, EntryPoint = "cuGraphicsUnmapResources")]
        public static extern CuResult GraphicsUnmapResources(int count, CuGraphicsResource* resources, CuStream hStream);
    }
}
