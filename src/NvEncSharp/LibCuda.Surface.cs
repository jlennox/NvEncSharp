using System.Runtime.InteropServices;

// ReSharper disable UnusedMember.Global

namespace Lennox.NvEncSharp
{
    public unsafe partial class LibCuda
    {
        /// <summary>Sets the CUDA array for a surface reference.
        ///
        /// Sets the CUDA array <paramref name="hArray"/> to be read and written by the surface reference
        /// <paramref name="hSurfRef"/>.  Any previous CUDA array state associated with the surface
        /// reference is superseded by this function.  <paramref name="flags"/> must be set to 0.
        /// The ::CUDA_ARRAY3D_SURFACE_LDST flag must have been set for the CUDA array.
        /// Any CUDA array previously bound to <paramref name="hSurfRef"/> is unbound.
        /// </summary>
        ///
        /// <param name="hSurfRef">Surface reference handle</param>
        /// <param name="hArray">CUDA array handle</param>
        /// <param name="flags">set to 0</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa
        /// ::cuModuleGetSurfRef,
        /// ::cuSurfRefGetArray,
        /// ::cudaBindSurfaceToArray
        /// CUresult CUDAAPI cuSurfRefSetArray(CUsurfref hSurfRef, CUarray hArray, unsigned int Flags);
        [DllImport(_dllpath, EntryPoint = "cuSurfRefSetArray")]
        public static extern CuResult SurfRefSetArray(CuSurfRef hSurfRef, CuArray hArray, int flags = 0);

        /// <summary>Passes back the CUDA array bound to a surface reference.
        ///
        /// Returns in *<paramref name="phArray"/> the CUDA array bound to the surface reference
        /// <paramref name="hSurfRef"/>, or returns ::CUDA_ERROR_INVALID_VALUE if the surface reference
        /// is not bound to any CUDA array.</summary>
        ///
        /// <param name="phArray">Surface reference handle</param>
        /// <param name="hSurfRef">Surface reference handle</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa ::cuModuleGetSurfRef, ::cuSurfRefSetArray
        /// CUresult CUDAAPI cuSurfRefGetArray(CUarray *phArray, CUsurfref hSurfRef);
        [DllImport(_dllpath, EntryPoint = "cuSurfRefGetArray")]
        public static extern CuResult SurfRefGetArray(out CuArray phArray, CuSurfRef hSurfRef);

        /// <summary>Creates a surface object
        ///
        /// Creates a surface object and returns it in <paramref name="pSurfObject"/>. <paramref name="pResDesc"/> describes
        /// the data to perform surface load/stores on. ::CUDA_RESOURCE_DESC::resType must be
        /// ::CU_RESOURCE_TYPE_ARRAY and  ::CUDA_RESOURCE_DESC::res::array::hArray
        /// must be set to a valid CUDA array handle. ::CUDA_RESOURCE_DESC::flags must be set to zero.
        ///
        /// Surface objects are only supported on devices of compute capability 3.0 or higher.
        /// Additionally, a surface object is an opaque value, and, as such, should only be
        /// accessed through CUDA API calls.</summary>
        ///
        /// <param name="pSurfObject">Surface object to create</param>
        /// <param name="pResDesc">Resource descriptor</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa
        /// ::cuSurfObjectDestroy,
        /// ::cudaCreateSurfaceObject
        /// CUresult CUDAAPI cuSurfObjectCreate(CUsurfObject *pSurfObject, const CUDA_RESOURCE_DESC *pResDesc);
        [DllImport(_dllpath, EntryPoint = "cuSurfObjectCreate")]
        public static extern CuResult SurfObjectCreate(out CuSurfObject pSurfObject, ref CuResourceDescription pResDesc);

        /// <summary>Destroys a surface object
        ///
        /// Destroys the surface object specified by <paramref name="surfObject"/>.</summary>
        ///
        /// <param name="surfObject">Surface object to destroy</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa
        /// ::cuSurfObjectCreate,
        /// ::cudaDestroySurfaceObject
        /// CUresult CUDAAPI cuSurfObjectDestroy(CUsurfObject surfObject);
        [DllImport(_dllpath, EntryPoint = "cuSurfObjectDestroy")]
        public static extern CuResult SurfObjectDestroy(CuSurfObject surfObject);

        /// <summary>Returns a surface object's resource descriptor
        ///
        /// Returns the resource descriptor for the surface object specified by <paramref name="surfObject"/>.</summary>
        ///
        /// <param name="pResDesc">Resource descriptor</param>
        /// <param name="surfObject">Surface object</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa
        /// ::cuSurfObjectCreate,
        /// ::cudaGetSurfaceObjectResourceDesc
        /// CUresult CUDAAPI cuSurfObjectGetResourceDesc(CUDA_RESOURCE_DESC *pResDesc, CUsurfObject surfObject);
        [DllImport(_dllpath, EntryPoint = "cuSurfObjectGetResourceDesc")]
        public static extern CuResult SurfObjectGetResourceDesc(out CuResourceDescription pResDesc, CuSurfObject surfObject);
    }
}
