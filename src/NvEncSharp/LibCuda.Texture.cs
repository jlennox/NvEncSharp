using System;
using System.Runtime.InteropServices;

// ReSharper disable UnusedMember.Global

namespace Lennox.NvEncSharp
{
    public unsafe partial class LibCuda
    {
        /// <summary>Creates a texture object
        ///
        /// Creates a texture object and returns it in <paramref name="pTexObject."/> <paramref name="pResDesc"/> describes
        /// the data to texture from. <paramref name="pTexDesc"/> describes how the data should be sampled.
        /// <paramref name="pResViewDesc"/> is an optional argument that specifies an alternate format for
        /// the data described by <paramref name="pResDesc,"/> and also describes the subresource region
        /// to restrict access to when texturing. <paramref name="pResViewDesc"/> can only be specified if
        /// the type of resource is a CUDA array or a CUDA mipmapped array.
        ///
        /// Texture objects are only supported on devices of compute capability 3.0 or higher.
        /// Additionally, a texture object is an opaque value, and, as such, should only be
        /// accessed through CUDA API calls.
        /// where:
        /// - ::CUDA_RESOURCE_DESC::resType specifies the type of resource to texture from.
        ///
        /// \par
        /// If ::CUDA_RESOURCE_DESC::resType is set to ::CU_RESOURCE_TYPE_ARRAY, ::CUDA_RESOURCE_DESC::res::array::hArray
        /// must be set to a valid CUDA array handle.
        ///
        /// \par
        /// If ::CUDA_RESOURCE_DESC::resType is set to ::CU_RESOURCE_TYPE_MIPMAPPED_ARRAY, ::CUDA_RESOURCE_DESC::res::mipmap::hMipmappedArray
        /// must be set to a valid CUDA mipmapped array handle.
        ///
        /// \par
        /// If ::CUDA_RESOURCE_DESC::resType is set to ::CU_RESOURCE_TYPE_LINEAR, ::CUDA_RESOURCE_DESC::res::linear::devPtr
        /// must be set to a valid device pointer, that is aligned to ::CU_DEVICE_ATTRIBUTE_TEXTURE_ALIGNMENT.
        /// ::CUDA_RESOURCE_DESC::res::linear::format and ::CUDA_RESOURCE_DESC::res::linear::numChannels
        /// describe the format of each component and the number of components per array element. ::CUDA_RESOURCE_DESC::res::linear::sizeInBytes
        /// specifies the size of the array in bytes. The total number of elements in the linear address range cannot exceed
        /// ::CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE1D_LINEAR_WIDTH. The number of elements is computed as (sizeInBytes / (sizeof(format) * numChannels)).
        ///
        /// \par
        /// If ::CUDA_RESOURCE_DESC::resType is set to ::CU_RESOURCE_TYPE_PITCH2D, ::CUDA_RESOURCE_DESC::res::pitch2D::devPtr
        /// must be set to a valid device pointer, that is aligned to ::CU_DEVICE_ATTRIBUTE_TEXTURE_ALIGNMENT.
        /// ::CUDA_RESOURCE_DESC::res::pitch2D::format and ::CUDA_RESOURCE_DESC::res::pitch2D::numChannels
        /// describe the format of each component and the number of components per array element. ::CUDA_RESOURCE_DESC::res::pitch2D::width
        /// and ::CUDA_RESOURCE_DESC::res::pitch2D::height specify the width and height of the array in elements, and cannot exceed
        /// ::CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_LINEAR_WIDTH and ::CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_LINEAR_HEIGHT respectively.
        /// ::CUDA_RESOURCE_DESC::res::pitch2D::pitchInBytes specifies the pitch between two rows in bytes and has to be aligned to
        /// ::CU_DEVICE_ATTRIBUTE_TEXTURE_PITCH_ALIGNMENT. Pitch cannot exceed ::CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_LINEAR_PITCH.
        ///
        /// - ::flags must be set to zero.
        ///
        ///   This is ignored if ::CUDA_RESOURCE_DESC::resType is ::CU_RESOURCE_TYPE_LINEAR.
        ///
        /// - ::CUDA_TEXTURE_DESC::flags can be any combination of the following:
        ///   - ::CU_TRSF_READ_AS_INTEGER, which suppresses the default behavior of having the texture promote integer data to floating point data in the
        ///     range [0, 1]. Note that texture with 32-bit integer format would not be promoted, regardless of whether or not this flag is specified.
        ///   - ::CU_TRSF_NORMALIZED_COORDINATES, which suppresses the default behavior of having the texture coordinates range from [0, Dim) where Dim is
        ///     the width or height of the CUDA array. Instead, the texture coordinates [0, 1.0) reference the entire breadth of the array dimension; Note
        ///     that for CUDA mipmapped arrays, this flag has to be set.
        ///
        /// - ::CUDA_TEXTURE_DESC::maxAnisotropy specifies the maximum anisotropy ratio to be used when doing anisotropic filtering. This value will be
        ///   clamped to the range [1,16].
        ///
        /// - ::CUDA_TEXTURE_DESC::mipmapFilterMode specifies the filter mode when the calculated mipmap level lies between two defined mipmap levels.
        ///
        /// - ::CUDA_TEXTURE_DESC::mipmapLevelBias specifies the offset to be applied to the calculated mipmap level.
        ///
        /// - ::CUDA_TEXTURE_DESC::minMipmapLevelClamp specifies the lower end of the mipmap level range to clamp access to.
        ///
        /// - ::CUDA_TEXTURE_DESC::maxMipmapLevelClamp specifies the upper end of the mipmap level range to clamp access to.
        ///
        /// where:
        /// - ::CUDA_RESOURCE_VIEW_DESC::format specifies how the data contained in the CUDA array or CUDA mipmapped array should
        ///   be interpreted. Note that this can incur a change in size of the texture data. If the resource view format is a block
        ///   compressed format, then the underlying CUDA array or CUDA mipmapped array has to have a base of format ::CU_AD_FORMAT_UNSIGNED_INT32.
        ///   with 2 or 4 channels, depending on the block compressed format. For ex., BC1 and BC4 require the underlying CUDA array to have
        ///   a format of ::CU_AD_FORMAT_UNSIGNED_INT32 with 2 channels. The other BC formats require the underlying resource to have the same base
        ///   format but with 4 channels.
        ///
        /// - ::CUDA_RESOURCE_VIEW_DESC::width specifies the new width of the texture data. If the resource view format is a block
        ///   compressed format, this value has to be 4 times the original width of the resource. For non block compressed formats,
        ///   this value has to be equal to that of the original resource.
        ///
        /// - ::CUDA_RESOURCE_VIEW_DESC::height specifies the new height of the texture data. If the resource view format is a block
        ///   compressed format, this value has to be 4 times the original height of the resource. For non block compressed formats,
        ///   this value has to be equal to that of the original resource.
        ///
        /// - ::CUDA_RESOURCE_VIEW_DESC::depth specifies the new depth of the texture data. This value has to be equal to that of the
        ///   original resource.
        ///
        /// - ::CUDA_RESOURCE_VIEW_DESC::firstMipmapLevel specifies the most detailed mipmap level. This will be the new mipmap level zero.
        ///   For non-mipmapped resources, this value has to be zero.::CUDA_TEXTURE_DESC::minMipmapLevelClamp and ::CUDA_TEXTURE_DESC::maxMipmapLevelClamp
        ///   will be relative to this value. For ex., if the firstMipmapLevel is set to 2, and a minMipmapLevelClamp of 1.2 is specified,
        ///   then the actual minimum mipmap level clamp will be 3.2.
        ///
        /// - ::CUDA_RESOURCE_VIEW_DESC::lastMipmapLevel specifies the least detailed mipmap level. For non-mipmapped resources, this value
        ///   has to be zero.
        ///
        /// - ::CUDA_RESOURCE_VIEW_DESC::firstLayer specifies the first layer index for layered textures. This will be the new layer zero.
        ///   For non-layered resources, this value has to be zero.
        ///
        /// - ::CUDA_RESOURCE_VIEW_DESC::lastLayer specifies the last layer index for layered textures. For non-layered resources,
        ///   this value has to be zero.
        ///</summary>
        ///
        /// <param name="pTexObject">Texture object to create</param>
        /// <param name="pResDesc">Resource descriptor</param>
        /// <param name="pTexDesc">Texture descriptor</param>
        /// <param name="pResViewDesc">Resource view descriptor </param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa
        /// ::cuTexObjectDestroy,
        /// ::cudaCreateTextureObject
        /// CUresult CUDAAPI cuTexObjectCreate(CUtexObject *pTexObject, const CUDA_RESOURCE_DESC *pResDesc, const CUDA_TEXTURE_DESC *pTexDesc, const CUDA_RESOURCE_VIEW_DESC *pResViewDesc);
        [DllImport(_dllpath, EntryPoint = "cuTexObjectCreate")]
        public static extern CuResult TexObjectCreate(out CuTexObject pTexObject, ref CuResourceDescription pResDesc, ref CuTextureDescription pTexDesc, ref CuResourceViewDescription pResViewDesc);

        /// <summary>Destroys a texture object
        ///
        /// Destroys the texture object specified by <paramref name="texObject"/>.</summary>
        ///
        /// <param name="texObject">Texture object to destroy</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa
        /// ::cuTexObjectCreate,
        /// ::cudaDestroyTextureObject
        /// CUresult CUDAAPI cuTexObjectDestroy(CUtexObject texObject);
        [DllImport(_dllpath, EntryPoint = "cuTexObjectDestroy")]
        public static extern CuResult TexObjectDestroy(CuTexObject texObject);

        /// <summary>Returns a texture object's resource descriptor
        ///
        /// Returns the resource descriptor for the texture object specified by <paramref name="texObject"/>.</summary>
        ///
        /// <param name="pResDesc">Resource descriptor</param>
        /// <param name="texObject">Texture object</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa
        /// ::cuTexObjectCreate,
        /// ::cudaGetTextureObjectResourceDesc,
        /// CUresult CUDAAPI cuTexObjectGetResourceDesc(CUDA_RESOURCE_DESC *pResDesc, CUtexObject texObject);
        [DllImport(_dllpath, EntryPoint = "cuTexObjectGetResourceDesc")]
        public static extern CuResult TexObjectGetResourceDesc(out CuResourceDescription pResDesc, CuTexObject texObject);

        /// <summary>Returns a texture object's texture descriptor
        ///
        /// Returns the texture descriptor for the texture object specified by <paramref name="texObject"/>.</summary>
        ///
        /// <param name="pTexDesc">Texture descriptor</param>
        /// <param name="texObject">Texture object</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa
        /// ::cuTexObjectCreate,
        /// ::cudaGetTextureObjectTextureDesc
        /// CUresult CUDAAPI cuTexObjectGetTextureDesc(CUDA_TEXTURE_DESC *pTexDesc, CUtexObject texObject);
        [DllImport(_dllpath, EntryPoint = "cuTexObjectGetTextureDesc")]
        public static extern CuResult TexObjectGetTextureDesc(out CuTextureDescription pTexDesc, CuTexObject texObject);

        /// <summary>Returns a texture object's resource view descriptor
        ///
        /// Returns the resource view descriptor for the texture object specified by <paramref name="texObject"/>.
        /// If no resource view was set for <paramref name="texObject,"/> the ::CUDA_ERROR_INVALID_VALUE is returned.</summary>
        ///
        /// <param name="pResViewDesc">Resource view descriptor</param>
        /// <param name="texObject">Texture object</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa
        /// ::cuTexObjectCreate,
        /// ::cudaGetTextureObjectResourceViewDesc
        /// CUresult CUDAAPI cuTexObjectGetResourceViewDesc(CUDA_RESOURCE_VIEW_DESC *pResViewDesc, CUtexObject texObject);
        [DllImport(_dllpath, EntryPoint = "cuTexObjectGetResourceViewDesc")]
        public static extern CuResult TexObjectGetResourceViewDesc(out CuResourceViewDescription pResViewDesc, CuTexObject texObject);

        /// <summary>Binds an array as a texture reference
        ///
        /// Binds the CUDA array <paramref name="hArray"/> to the texture reference <paramref name="hTexRef"/>. Any
        /// previous address or CUDA array state associated with the texture reference
        /// is superseded by this function. <paramref name="flags"/> must be set to
        /// ::CU_TRSA_OVERRIDE_FORMAT. Any CUDA array previously bound to <paramref name="hTexRef"/> is
        /// unbound.</summary>
        ///
        /// <param name="hTexRef">Texture reference to bind</param>
        /// <param name="hArray">Array to bind</param>
        /// <param name="flags">Options (must be ::CU_TRSA_OVERRIDE_FORMAT)</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa ::cuTexRefSetAddress,
        /// ::cuTexRefSetAddress2D, ::cuTexRefSetAddressMode,
        /// ::cuTexRefSetFilterMode, ::cuTexRefSetFlags, ::cuTexRefSetFormat,
        /// ::cuTexRefGetAddress, ::cuTexRefGetAddressMode, ::cuTexRefGetArray,
        /// ::cuTexRefGetFilterMode, ::cuTexRefGetFlags, ::cuTexRefGetFormat,
        /// ::cudaBindTextureToArray
        /// CUresult CUDAAPI cuTexRefSetArray(CUtexref hTexRef, CUarray hArray, unsigned int Flags);
        [DllImport(_dllpath, EntryPoint = "cuTexRefSetArray")]
        public static extern CuResult TexRefSetArray(CuTextRef hTexRef, CuArray hArray, int flags = 1);

        /// <summary>Binds a mipmapped array to a texture reference
        ///
        /// Binds the CUDA mipmapped array <paramref name="hMipmappedArray"/> to the texture reference <paramref name="hTexRef"/>.
        /// Any previous address or CUDA array state associated with the texture reference
        /// is superseded by this function. <paramref name="flags"/> must be set to ::CU_TRSA_OVERRIDE_FORMAT.
        /// Any CUDA array previously bound to <paramref name="hTexRef"/> is unbound.</summary>
        ///
        /// <param name="hTexRef">Texture reference to bind</param>
        /// <param name="hMipmappedArray">Mipmapped array to bind</param>
        /// <param name="flags">Options (must be ::CU_TRSA_OVERRIDE_FORMAT)</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa ::cuTexRefSetAddress,
        /// ::cuTexRefSetAddress2D, ::cuTexRefSetAddressMode,
        /// ::cuTexRefSetFilterMode, ::cuTexRefSetFlags, ::cuTexRefSetFormat,
        /// ::cuTexRefGetAddress, ::cuTexRefGetAddressMode, ::cuTexRefGetArray,
        /// ::cuTexRefGetFilterMode, ::cuTexRefGetFlags, ::cuTexRefGetFormat,
        /// ::cudaBindTextureToMipmappedArray
        /// CUresult CUDAAPI cuTexRefSetMipmappedArray(CUtexref hTexRef, CUmipmappedArray hMipmappedArray, unsigned int Flags);
        [DllImport(_dllpath, EntryPoint = "cuTexRefSetMipmappedArray")]
        public static extern CuResult TexRefSetMipmappedArray(CuTextRef hTexRef, CuMipMappedArray hMipmappedArray, int flags = 1);

        /// <summary>Binds an address as a texture reference
        ///
        /// Binds a linear address range to the texture reference <paramref name="hTexRef"/>. Any
        /// previous address or CUDA array state associated with the texture reference
        /// is superseded by this function. Any memory previously bound to <paramref name="hTexRef"/>
        /// is unbound.
        ///
        /// Since the hardware enforces an alignment requirement on texture base
        /// addresses, ::cuTexRefSetAddress() passes back a byte offset in
        /// *<paramref name="byteOffset"/> that must be applied to texture fetches in order to read from
        /// the desired memory. This offset must be divided by the texel size and
        /// passed to kernels that read from the texture so they can be applied to the
        /// ::tex1Dfetch() function.
        ///
        /// If the device memory pointer was returned from ::cuMemAlloc(), the offset
        /// is guaranteed to be 0 and NULL may be passed as the <paramref name="byteOffset"/> parameter.
        ///
        /// The total number of elements (or texels) in the linear address range
        /// cannot exceed ::CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE1D_LINEAR_WIDTH.
        /// The number of elements is computed as (<paramref name="bytes"/> / bytesPerElement),
        /// where bytesPerElement is determined from the data format and number of
        /// components set using ::cuTexRefSetFormat().</summary>
        ///
        /// <param name="byteOffset">Returned byte offset</param>
        /// <param name="hTexRef">Texture reference to bind</param>
        /// <param name="dptr">Device pointer to bind</param>
        /// <param name="bytes">Size of memory to bind in bytes</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa ::cuTexRefSetAddress2D, ::cuTexRefSetAddressMode, ::cuTexRefSetArray,
        /// ::cuTexRefSetFilterMode, ::cuTexRefSetFlags, ::cuTexRefSetFormat,
        /// ::cuTexRefGetAddress, ::cuTexRefGetAddressMode, ::cuTexRefGetArray,
        /// ::cuTexRefGetFilterMode, ::cuTexRefGetFlags, ::cuTexRefGetFormat,
        /// ::cudaBindTexture
        /// CUresult CUDAAPI cuTexRefSetAddress(size_t *ByteOffset, CUtexref hTexRef, CUdeviceptr dptr, size_t bytes);
        [DllImport(_dllpath, EntryPoint = "cuTexRefSetAddress")]
        public static extern CuResult TexRefSetAddress(out IntPtr byteOffset, CuTextRef hTexRef, CuDevicePtr dptr, IntPtr bytes);

        /// <summary>Binds an address as a 2D texture reference
        ///
        /// Binds a linear address range to the texture reference <paramref name="hTexRef"/>. Any
        /// previous address or CUDA array state associated with the texture reference
        /// is superseded by this function. Any memory previously bound to <paramref name="hTexRef"/>
        /// is unbound.
        ///
        /// Using a ::tex2D() function inside a kernel requires a call to either
        /// ::cuTexRefSetArray() to bind the corresponding texture reference to an
        /// array, or ::cuTexRefSetAddress2D() to bind the texture reference to linear
        /// memory.
        ///
        /// Function calls to ::cuTexRefSetFormat() cannot follow calls to
        /// ::cuTexRefSetAddress2D() for the same texture reference.
        ///
        /// It is required that <paramref name="dptr"/> be aligned to the appropriate hardware-specific
        /// texture alignment. You can query this value using the device attribute
        /// ::CU_DEVICE_ATTRIBUTE_TEXTURE_ALIGNMENT. If an unaligned <paramref name="dptr"/> is
        /// supplied, ::CUDA_ERROR_INVALID_VALUE is returned.
        ///
        /// <paramref name="pitch"/> has to be aligned to the hardware-specific texture pitch alignment.
        /// This value can be queried using the device attribute
        /// ::CU_DEVICE_ATTRIBUTE_TEXTURE_PITCH_ALIGNMENT. If an unaligned <paramref name="pitch"/> is
        /// supplied, ::CUDA_ERROR_INVALID_VALUE is returned.
        ///
        /// Width and Height, which are specified in elements (or texels), cannot exceed
        /// ::CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_LINEAR_WIDTH and
        /// ::CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_LINEAR_HEIGHT respectively.
        /// <paramref name="pitch"/>, which is specified in bytes, cannot exceed
        /// ::CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_LINEAR_PITCH.</summary>
        ///
        /// <param name="hTexRef">Texture reference to bind</param>
        /// <param name="desc">Descriptor of CUDA array</param>
        /// <param name="dptr">Device pointer to bind</param>
        /// <param name="pitch">Line pitch in bytes</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa ::cuTexRefSetAddress,
        /// ::cuTexRefSetAddressMode, ::cuTexRefSetArray,
        /// ::cuTexRefSetFilterMode, ::cuTexRefSetFlags, ::cuTexRefSetFormat,
        /// ::cuTexRefGetAddress, ::cuTexRefGetAddressMode, ::cuTexRefGetArray,
        /// ::cuTexRefGetFilterMode, ::cuTexRefGetFlags, ::cuTexRefGetFormat,
        /// ::cudaBindTexture2D
        /// CUresult CUDAAPI cuTexRefSetAddress2D(CUtexref hTexRef, const CUDA_ARRAY_DESCRIPTOR *desc, CUdeviceptr dptr, size_t Pitch);
        [DllImport(_dllpath, EntryPoint = "cuTexRefSetAddress2D")]
        public static extern CuResult TexRefSetAddress2D(CuTextRef hTexRef, ref CuArrayDescription desc, CuDevicePtr dptr, IntPtr pitch);

        /// <summary>Sets the format for a texture reference
        ///
        /// Specifies the format of the data to be read by the texture reference
        /// <paramref name="hTexRef"/>. <paramref name="fmt"/> and <paramref name="numPackedComponents"/> are exactly analogous to the
        /// ::Format and ::NumChannels members of the ::CUDA_ARRAY_DESCRIPTOR structure:
        /// They specify the format of each component and the number of components per
        /// array element.</summary>
        ///
        /// <param name="hTexRef">Texture reference</param>
        /// <param name="fmt">Format to set</param>
        /// <param name="numPackedComponents">Number of components per array element</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa ::cuTexRefSetAddress,
        /// ::cuTexRefSetAddress2D, ::cuTexRefSetAddressMode, ::cuTexRefSetArray,
        /// ::cuTexRefSetFilterMode, ::cuTexRefSetFlags,
        /// ::cuTexRefGetAddress, ::cuTexRefGetAddressMode, ::cuTexRefGetArray,
        /// ::cuTexRefGetFilterMode, ::cuTexRefGetFlags, ::cuTexRefGetFormat,
        /// ::cudaCreateChannelDesc,
        /// ::cudaBindTexture,
        /// ::cudaBindTexture2D,
        /// ::cudaBindTextureToArray,
        /// ::cudaBindTextureToMipmappedArray
        /// CUresult CUDAAPI cuTexRefSetFormat(CUtexref hTexRef, CUarray_format fmt, int NumPackedComponents);
        [DllImport(_dllpath, EntryPoint = "cuTexRefSetFormat")]
        public static extern CuResult TexRefSetFormat(CuTextRef hTexRef, CuArrayFormat fmt, int numPackedComponents);

        /// <summary>Sets the addressing mode for a texture reference
        ///
        /// Specifies the addressing mode <paramref name="am"/> for the given dimension <paramref name="dim"/> of the
        /// texture reference <paramref name="hTexRef"/>. If <paramref name="dim"/> is zero, the addressing mode is
        /// applied to the first parameter of the functions used to fetch from the
        /// texture; if <paramref name="dim"/> is 1, the second, and so on.
        ///
        /// Note that this call has no effect if <paramref name="hTexRef"/> is bound to linear memory.
        /// Also, if the flag, ::CU_TRSF_NORMALIZED_COORDINATES, is not set, the only
        /// supported address mode is ::CU_TR_ADDRESS_MODE_CLAMP.</summary>
        ///
        /// <param name="hTexRef">Texture reference</param>
        /// <param name="dim">Dimension</param>
        /// <param name="am">Addressing mode to set</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa ::cuTexRefSetAddress,
        /// ::cuTexRefSetAddress2D, ::cuTexRefSetArray,
        /// ::cuTexRefSetFilterMode, ::cuTexRefSetFlags, ::cuTexRefSetFormat,
        /// ::cuTexRefGetAddress, ::cuTexRefGetAddressMode, ::cuTexRefGetArray,
        /// ::cuTexRefGetFilterMode, ::cuTexRefGetFlags, ::cuTexRefGetFormat,
        /// ::cudaBindTexture,
        /// ::cudaBindTexture2D,
        /// ::cudaBindTextureToArray,
        /// ::cudaBindTextureToMipmappedArray
        /// CUresult CUDAAPI cuTexRefSetAddressMode(CUtexref hTexRef, int dim, CUaddress_mode am);
        [DllImport(_dllpath, EntryPoint = "cuTexRefSetAddressMode")]
        public static extern CuResult TexRefSetAddressMode(CuTextRef hTexRef, int dim, AddressMode am);


        /// <summary>Sets the filtering mode for a texture reference
        ///
        /// Specifies the filtering mode <paramref name="fm"/> to be used when reading memory through
        /// the texture reference <paramref name="hTexRef"/>.
        ///
        /// Note that this call has no effect if <paramref name="hTexRef"/> is bound to linear memory.</summary>
        ///
        /// <param name="hTexRef">Texture reference</param>
        /// <param name="fm">Filtering mode to set</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa ::cuTexRefSetAddress,
        /// ::cuTexRefSetAddress2D, ::cuTexRefSetAddressMode, ::cuTexRefSetArray,
        /// ::cuTexRefSetFlags, ::cuTexRefSetFormat,
        /// ::cuTexRefGetAddress, ::cuTexRefGetAddressMode, ::cuTexRefGetArray,
        /// ::cuTexRefGetFilterMode, ::cuTexRefGetFlags, ::cuTexRefGetFormat,
        /// ::cudaBindTextureToArray
        /// CUresult CUDAAPI cuTexRefSetFilterMode(CUtexref hTexRef, CUfilter_mode fm);
        [DllImport(_dllpath, EntryPoint = "cuTexRefSetFilterMode")]
        public static extern CuResult TexRefSetFilterMode(CuTextRef hTexRef, FilterMode fm);

        /// <summary>Sets the mipmap filtering mode for a texture reference
        ///
        /// Specifies the mipmap filtering mode <paramref name="fm"/> to be used when reading memory through
        /// the texture reference <paramref name="hTexRef"/>.
        ///
        /// Note that this call has no effect if <paramref name="hTexRef"/> is not bound to a mipmapped array.</summary>
        ///
        /// <param name="hTexRef">Texture reference</param>
        /// <param name="fm">Filtering mode to set</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa ::cuTexRefSetAddress,
        /// ::cuTexRefSetAddress2D, ::cuTexRefSetAddressMode, ::cuTexRefSetArray,
        /// ::cuTexRefSetFlags, ::cuTexRefSetFormat,
        /// ::cuTexRefGetAddress, ::cuTexRefGetAddressMode, ::cuTexRefGetArray,
        /// ::cuTexRefGetFilterMode, ::cuTexRefGetFlags, ::cuTexRefGetFormat,
        /// ::cudaBindTextureToMipmappedArray
        /// CUresult CUDAAPI cuTexRefSetMipmapFilterMode(CUtexref hTexRef, CUfilter_mode fm);
        [DllImport(_dllpath, EntryPoint = "cuTexRefSetMipmapFilterMode")]
        public static extern CuResult TexRefSetMipmapFilterMode(CuTextRef hTexRef, FilterMode fm);

        /// <summary>Sets the mipmap level bias for a texture reference
        ///
        /// Specifies the mipmap level bias <paramref name="bias"/> to be added to the specified mipmap level when
        /// reading memory through the texture reference <paramref name="hTexRef."/>
        ///
        /// Note that this call has no effect if <paramref name="hTexRef"/> is not bound to a mipmapped array.</summary>
        ///
        /// <param name="hTexRef">Texture reference</param>
        /// <param name="bias">Mipmap level bias</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa ::cuTexRefSetAddress,
        /// ::cuTexRefSetAddress2D, ::cuTexRefSetAddressMode, ::cuTexRefSetArray,
        /// ::cuTexRefSetFlags, ::cuTexRefSetFormat,
        /// ::cuTexRefGetAddress, ::cuTexRefGetAddressMode, ::cuTexRefGetArray,
        /// ::cuTexRefGetFilterMode, ::cuTexRefGetFlags, ::cuTexRefGetFormat,
        /// ::cudaBindTextureToMipmappedArray
        /// CUresult CUDAAPI cuTexRefSetMipmapLevelBias(CUtexref hTexRef, float bias);
        [DllImport(_dllpath, EntryPoint = "cuTexRefSetMipmapLevelBias")]
        public static extern CuResult TexRefSetMipmapLevelBias(CuTextRef hTexRef, float bias);

        /// <summary>Sets the mipmap min/max mipmap level clamps for a texture reference
        ///
        /// Specifies the min/max mipmap level clamps, <paramref name="minMipmapLevelClamp"/> and <paramref name="maxMipmapLevelClamp"/>
        /// respectively, to be used when reading memory through the texture reference
        /// <paramref name="hTexRef."/>
        ///
        /// Note that this call has no effect if <paramref name="hTexRef"/> is not bound to a mipmapped array.</summary>
        ///
        /// <param name="hTexRef">Texture reference</param>
        /// <param name="minMipmapLevelClamp">Mipmap min level clamp</param>
        /// <param name="maxMipmapLevelClamp">Mipmap max level clamp</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa ::cuTexRefSetAddress,
        /// ::cuTexRefSetAddress2D, ::cuTexRefSetAddressMode, ::cuTexRefSetArray,
        /// ::cuTexRefSetFlags, ::cuTexRefSetFormat,
        /// ::cuTexRefGetAddress, ::cuTexRefGetAddressMode, ::cuTexRefGetArray,
        /// ::cuTexRefGetFilterMode, ::cuTexRefGetFlags, ::cuTexRefGetFormat,
        /// ::cudaBindTextureToMipmappedArray
        /// CUresult CUDAAPI cuTexRefSetMipmapLevelClamp(CUtexref hTexRef, float minMipmapLevelClamp, float maxMipmapLevelClamp);
        [DllImport(_dllpath, EntryPoint = "cuTexRefSetMipmapLevelClamp")]
        public static extern CuResult TexRefSetMipmapLevelClamp(CuTextRef hTexRef, float minMipmapLevelClamp, float maxMipmapLevelClamp);

        /// <summary>Sets the maximum anisotropy for a texture reference
        ///
        /// Specifies the maximum anisotropy <paramref name="maxAniso"/> to be used when reading memory through
        /// the texture reference <paramref name="hTexRef."/>
        ///
        /// Note that this call has no effect if <paramref name="hTexRef"/> is bound to linear memory.</summary>
        ///
        /// <param name="hTexRef">Texture reference</param>
        /// <param name="maxAniso">Maximum anisotropy</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa ::cuTexRefSetAddress,
        /// ::cuTexRefSetAddress2D, ::cuTexRefSetAddressMode, ::cuTexRefSetArray,
        /// ::cuTexRefSetFlags, ::cuTexRefSetFormat,
        /// ::cuTexRefGetAddress, ::cuTexRefGetAddressMode, ::cuTexRefGetArray,
        /// ::cuTexRefGetFilterMode, ::cuTexRefGetFlags, ::cuTexRefGetFormat,
        /// ::cudaBindTextureToArray,
        /// ::cudaBindTextureToMipmappedArray
        /// CUresult CUDAAPI cuTexRefSetMaxAnisotropy(CUtexref hTexRef, unsigned int maxAniso);
        [DllImport(_dllpath, EntryPoint = "cuTexRefSetMaxAnisotropy")]
        public static extern CuResult TexRefSetMaxAnisotropy(CuTextRef hTexRef, int maxAniso);

        /// <summary>Sets the border color for a texture reference
        ///
        /// Specifies the value of the RGBA color via the <paramref name="pBorderColor"/> to the texture reference
        /// <paramref name="hTexRef"/>. The color value supports only float type and holds color components in
        /// the following sequence:
        /// pBorderColor[0] holds 'R' component
        /// pBorderColor[1] holds 'G' component
        /// pBorderColor[2] holds 'B' component
        /// pBorderColor[3] holds 'A' component
        ///
        /// Note that the color values can be set only when the Address mode is set to
        /// CU_TR_ADDRESS_MODE_BORDER using ::cuTexRefSetAddressMode.
        /// Applications using integer border color values have to "reinterpret_cast" their values to float.</summary>
        ///
        /// <param name="hTexRef">Texture reference</param>
        /// <param name="pBorderColor">RGBA color</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa ::cuTexRefSetAddressMode,
        /// ::cuTexRefGetAddressMode, ::cuTexRefGetBorderColor,
        /// ::cudaBindTexture,
        /// ::cudaBindTexture2D,
        /// ::cudaBindTextureToArray,
        /// ::cudaBindTextureToMipmappedArray
        /// CUresult CUDAAPI cuTexRefSetBorderColor(CUtexref hTexRef, float *pBorderColor);
        [DllImport(_dllpath, EntryPoint = "cuTexRefSetBorderColor")]
        public static extern CuResult TexRefSetBorderColor(CuTextRef hTexRef, [MarshalAs(UnmanagedType.LPArray, SizeConst = 4)] float[] pBorderColor);

        /// <summary>Sets the flags for a texture reference
        ///
        /// Specifies optional flags via <paramref name="flags"/> to specify the behavior of data
        /// returned through the texture reference <paramref name="hTexRef"/>. The valid flags are:
        ///
        /// - ::CU_TRSF_READ_AS_INTEGER, which suppresses the default behavior of
        ///   having the texture promote integer data to floating point data in the
        ///   range [0, 1]. Note that texture with 32-bit integer format
        ///   would not be promoted, regardless of whether or not this
        ///   flag is specified;
        /// - ::CU_TRSF_NORMALIZED_COORDINATES, which suppresses the
        ///   default behavior of having the texture coordinates range
        ///   from [0, Dim) where Dim is the width or height of the CUDA
        ///   array. Instead, the texture coordinates [0, 1.0) reference
        ///   the entire breadth of the array dimension;</summary>
        ///
        /// <param name="hTexRef">Texture reference</param>
        /// <param name="flags">Optional flags to set</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa ::cuTexRefSetAddress,
        /// ::cuTexRefSetAddress2D, ::cuTexRefSetAddressMode, ::cuTexRefSetArray,
        /// ::cuTexRefSetFilterMode, ::cuTexRefSetFormat,
        /// ::cuTexRefGetAddress, ::cuTexRefGetAddressMode, ::cuTexRefGetArray,
        /// ::cuTexRefGetFilterMode, ::cuTexRefGetFlags, ::cuTexRefGetFormat,
        /// ::cudaBindTexture,
        /// ::cudaBindTexture2D,
        /// ::cudaBindTextureToArray,
        /// ::cudaBindTextureToMipmappedArray
        /// CUresult CUDAAPI cuTexRefSetFlags(CUtexref hTexRef, unsigned int Flags);
        [DllImport(_dllpath, EntryPoint = "cuTexRefSetFlags")]
        public static extern CuResult TexRefSetFlags(CuTextRef hTexRef, TrsfFlags flags);

        /// <summary>Gets the address associated with a texture reference
        ///
        /// Returns in *<paramref name="pdptr"/> the base address bound to the texture reference
        /// <paramref name="hTexRef"/>, or returns ::CUDA_ERROR_INVALID_VALUE if the texture reference
        /// is not bound to any device memory range.</summary>
        ///
        /// <param name="pdptr">Returned device address</param>
        /// <param name="hTexRef">Texture reference</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa ::cuTexRefSetAddress,
        /// ::cuTexRefSetAddress2D, ::cuTexRefSetAddressMode, ::cuTexRefSetArray,
        /// ::cuTexRefSetFilterMode, ::cuTexRefSetFlags, ::cuTexRefSetFormat,
        /// ::cuTexRefGetAddressMode, ::cuTexRefGetArray,
        /// ::cuTexRefGetFilterMode, ::cuTexRefGetFlags, ::cuTexRefGetFormat
        /// CUresult CUDAAPI cuTexRefGetAddress(CUdeviceptr *pdptr, CUtexref hTexRef);
        [DllImport(_dllpath, EntryPoint = "cuTexRefGetAddress")]
        public static extern CuResult TexRefGetAddress(out CuDevicePtr pdptr, CuTextRef hTexRef);

        /// <summary>Gets the array bound to a texture reference
        ///
        /// Returns in *<paramref name="phArray"/> the CUDA array bound to the texture reference
        /// <paramref name="hTexRef"/>, or returns ::CUDA_ERROR_INVALID_VALUE if the texture reference
        /// is not bound to any CUDA array.</summary>
        ///
        /// <param name="phArray">Returned array</param>
        /// <param name="hTexRef">Texture reference</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa ::cuTexRefSetAddress,
        /// ::cuTexRefSetAddress2D, ::cuTexRefSetAddressMode, ::cuTexRefSetArray,
        /// ::cuTexRefSetFilterMode, ::cuTexRefSetFlags, ::cuTexRefSetFormat,
        /// ::cuTexRefGetAddress, ::cuTexRefGetAddressMode,
        /// ::cuTexRefGetFilterMode, ::cuTexRefGetFlags, ::cuTexRefGetFormat
        /// CUresult CUDAAPI cuTexRefGetArray(CUarray *phArray, CUtexref hTexRef);
        [DllImport(_dllpath, EntryPoint = "cuTexRefGetArray")]
        public static extern CuResult TexRefGetArray(out CuArray phArray, CuTextRef hTexRef);

        /// <summary>Gets the mipmapped array bound to a texture reference
        ///
        /// Returns in *<paramref name="phMipmappedArray"/> the CUDA mipmapped array bound to the texture
        /// reference <paramref name="hTexRef"/>, or returns ::CUDA_ERROR_INVALID_VALUE if the texture reference
        /// is not bound to any CUDA mipmapped array.</summary>
        ///
        /// <param name="phMipmappedArray">Returned mipmapped array</param>
        /// <param name="hTexRef">Texture reference</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa ::cuTexRefSetAddress,
        /// ::cuTexRefSetAddress2D, ::cuTexRefSetAddressMode, ::cuTexRefSetArray,
        /// ::cuTexRefSetFilterMode, ::cuTexRefSetFlags, ::cuTexRefSetFormat,
        /// ::cuTexRefGetAddress, ::cuTexRefGetAddressMode,
        /// ::cuTexRefGetFilterMode, ::cuTexRefGetFlags, ::cuTexRefGetFormat
        /// CUresult CUDAAPI cuTexRefGetMipmappedArray(CUmipmappedArray *phMipmappedArray, CUtexref hTexRef);
        [DllImport(_dllpath, EntryPoint = "cuTexRefGetMipmappedArray")]
        public static extern CuResult TexRefGetMipmappedArray(out CuMipMappedArray phMipmappedArray, CuTextRef hTexRef);

        /// <summary>Gets the addressing mode used by a texture reference
        ///
        /// Returns in *<paramref name="pam"/> the addressing mode corresponding to the
        /// dimension <paramref name="dim"/> of the texture reference <paramref name="hTexRef"/>. Currently, the only
        /// valid value for <paramref name="dim"/> are 0 and 1.</summary>
        ///
        /// <param name="pam">Returned addressing mode</param>
        /// <param name="hTexRef">Texture reference</param>
        /// <param name="dim">Dimension</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa ::cuTexRefSetAddress,
        /// ::cuTexRefSetAddress2D, ::cuTexRefSetAddressMode, ::cuTexRefSetArray,
        /// ::cuTexRefSetFilterMode, ::cuTexRefSetFlags, ::cuTexRefSetFormat,
        /// ::cuTexRefGetAddress, ::cuTexRefGetArray,
        /// ::cuTexRefGetFilterMode, ::cuTexRefGetFlags, ::cuTexRefGetFormat
        /// CUresult CUDAAPI cuTexRefGetAddressMode(CUaddress_mode *pam, CUtexref hTexRef, int dim);
        [DllImport(_dllpath, EntryPoint = "cuTexRefGetAddressMode")]
        public static extern CuResult TexRefGetAddressMode(out AddressMode pam, CuTextRef hTexRef, int dim);

        /// <summary>Gets the filter-mode used by a texture reference
        ///
        /// Returns in *<paramref name="pfm"/> the filtering mode of the texture reference
        /// <paramref name="hTexRef"/>.</summary>
        ///
        /// <param name="pfm">Returned filtering mode</param>
        /// <param name="hTexRef">Texture reference</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa ::cuTexRefSetAddress,
        /// ::cuTexRefSetAddress2D, ::cuTexRefSetAddressMode, ::cuTexRefSetArray,
        /// ::cuTexRefSetFilterMode, ::cuTexRefSetFlags, ::cuTexRefSetFormat,
        /// ::cuTexRefGetAddress, ::cuTexRefGetAddressMode, ::cuTexRefGetArray,
        /// ::cuTexRefGetFlags, ::cuTexRefGetFormat
        /// CUresult CUDAAPI cuTexRefGetFilterMode(CUfilter_mode *pfm, CUtexref hTexRef);
        [DllImport(_dllpath, EntryPoint = "cuTexRefGetFilterMode")]
        public static extern CuResult TexRefGetFilterMode(out FilterMode pfm, CuTextRef hTexRef);

        /// <summary>Gets the format used by a texture reference
        ///
        /// Returns in *<paramref name="pFormat"/> and *<paramref name="pNumChannels"/> the format and number
        /// of components of the CUDA array bound to the texture reference <paramref name="hTexRef"/>.
        /// If <paramref name="pFormat"/> or <paramref name="pNumChannels"/> is NULL, it will be ignored.</summary>
        ///
        /// <param name="pFormat">Returned format</param>
        /// <param name="pNumChannels">Returned number of components</param>
        /// <param name="hTexRef">Texture reference</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa ::cuTexRefSetAddress,
        /// ::cuTexRefSetAddress2D, ::cuTexRefSetAddressMode, ::cuTexRefSetArray,
        /// ::cuTexRefSetFilterMode, ::cuTexRefSetFlags, ::cuTexRefSetFormat,
        /// ::cuTexRefGetAddress, ::cuTexRefGetAddressMode, ::cuTexRefGetArray,
        /// ::cuTexRefGetFilterMode, ::cuTexRefGetFlags
        /// CUresult CUDAAPI cuTexRefGetFormat(CUarray_format *pFormat, int *pNumChannels, CUtexref hTexRef);
        [DllImport(_dllpath, EntryPoint = "cuTexRefGetFormat")]
        public static extern CuResult TexRefGetFormat(out CuArrayFormat pFormat, out int pNumChannels, CuTextRef hTexRef);

        /// <summary>Gets the mipmap filtering mode for a texture reference
        ///
        /// Returns the mipmap filtering mode in <paramref name="pfm"/> that's used when reading memory through
        /// the texture reference <paramref name="hTexRef"/>.</summary>
        ///
        /// <param name="pfm">Returned mipmap filtering mode</param>
        /// <param name="hTexRef">Texture reference</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa ::cuTexRefSetAddress,
        /// ::cuTexRefSetAddress2D, ::cuTexRefSetAddressMode, ::cuTexRefSetArray,
        /// ::cuTexRefSetFlags, ::cuTexRefSetFormat,
        /// ::cuTexRefGetAddress, ::cuTexRefGetAddressMode, ::cuTexRefGetArray,
        /// ::cuTexRefGetFilterMode, ::cuTexRefGetFlags, ::cuTexRefGetFormat
        /// CUresult CUDAAPI cuTexRefGetMipmapFilterMode(CUfilter_mode *pfm, CUtexref hTexRef);
        [DllImport(_dllpath, EntryPoint = "cuTexRefGetMipmapFilterMode")]
        public static extern CuResult TexRefGetMipmapFilterMode(out FilterMode pfm, CuTextRef hTexRef);

        /// <summary>Gets the mipmap level bias for a texture reference
        ///
        /// Returns the mipmap level bias in <paramref name="pbias"/> that's added to the specified mipmap
        /// level when reading memory through the texture reference <paramref name="hTexRef"/>.</summary>
        ///
        /// <param name="pbias">Returned mipmap level bias</param>
        /// <param name="hTexRef">Texture reference</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa ::cuTexRefSetAddress,
        /// ::cuTexRefSetAddress2D, ::cuTexRefSetAddressMode, ::cuTexRefSetArray,
        /// ::cuTexRefSetFlags, ::cuTexRefSetFormat,
        /// ::cuTexRefGetAddress, ::cuTexRefGetAddressMode, ::cuTexRefGetArray,
        /// ::cuTexRefGetFilterMode, ::cuTexRefGetFlags, ::cuTexRefGetFormat
        /// CUresult CUDAAPI cuTexRefGetMipmapLevelBias(float *pbias, CUtexref hTexRef);
        [DllImport(_dllpath, EntryPoint = "cuTexRefGetMipmapLevelBias")]
        public static extern CuResult TexRefGetMipmapLevelBias(out float pbias, CuTextRef hTexRef);

        /// <summary>Gets the min/max mipmap level clamps for a texture reference
        ///
        /// Returns the min/max mipmap level clamps in <paramref name="pminMipmapLevelClamp"/> and <paramref name="pmaxMipmapLevelClamp"/>
        /// that's used when reading memory through the texture reference <paramref name="hTexRef"/>.</summary>
        ///
        /// <param name="pminMipmapLevelClamp">Returned mipmap min level clamp</param>
        /// <param name="pmaxMipmapLevelClamp">Returned mipmap max level clamp</param>
        /// <param name="hTexRef">Texture reference</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa ::cuTexRefSetAddress,
        /// ::cuTexRefSetAddress2D, ::cuTexRefSetAddressMode, ::cuTexRefSetArray,
        /// ::cuTexRefSetFlags, ::cuTexRefSetFormat,
        /// ::cuTexRefGetAddress, ::cuTexRefGetAddressMode, ::cuTexRefGetArray,
        /// ::cuTexRefGetFilterMode, ::cuTexRefGetFlags, ::cuTexRefGetFormat
        /// CUresult CUDAAPI cuTexRefGetMipmapLevelClamp(float *pminMipmapLevelClamp, float *pmaxMipmapLevelClamp, CUtexref hTexRef);
        [DllImport(_dllpath, EntryPoint = "cuTexRefGetMipmapLevelClamp")]
        public static extern CuResult TexRefGetMipmapLevelClamp(out float pminMipmapLevelClamp, out float pmaxMipmapLevelClamp, CuTextRef hTexRef);

        /// <summary>Gets the maximum anisotropy for a texture reference
        ///
        /// Returns the maximum anisotropy in <paramref name="pmaxAniso"/> that's used when reading memory through
        /// the texture reference <paramref name="hTexRef"/>.</summary>
        ///
        /// <param name="pmaxAniso">Returned maximum anisotropy</param>
        /// <param name="hTexRef">Texture reference</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa ::cuTexRefSetAddress,
        /// ::cuTexRefSetAddress2D, ::cuTexRefSetAddressMode, ::cuTexRefSetArray,
        /// ::cuTexRefSetFlags, ::cuTexRefSetFormat,
        /// ::cuTexRefGetAddress, ::cuTexRefGetAddressMode, ::cuTexRefGetArray,
        /// ::cuTexRefGetFilterMode, ::cuTexRefGetFlags, ::cuTexRefGetFormat
        /// CUresult CUDAAPI cuTexRefGetMaxAnisotropy(int *pmaxAniso, CUtexref hTexRef);
        [DllImport(_dllpath, EntryPoint = "cuTexRefGetMaxAnisotropy")]
        public static extern CuResult TexRefGetMaxAnisotropy(out int pmaxAniso, CuTextRef hTexRef);

        /// <summary>Gets the border color used by a texture reference
        ///
        /// Returns in <paramref name="pBorderColor"/>, values of the RGBA color used by
        /// the texture reference <paramref name="hTexRef"/>.
        /// The color value is of type float and holds color components in
        /// the following sequence:
        /// pBorderColor[0] holds 'R' component
        /// pBorderColor[1] holds 'G' component
        /// pBorderColor[2] holds 'B' component
        /// pBorderColor[3] holds 'A' component</summary>
        ///
        /// <param name="hTexRef">Texture reference</param>
        /// <param name="pBorderColor">Returned Type and Value of RGBA color</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa ::cuTexRefSetAddressMode,
        /// ::cuTexRefSetAddressMode, ::cuTexRefSetBorderColor
        /// CUresult CUDAAPI cuTexRefGetBorderColor(float *pBorderColor, CUtexref hTexRef);
        [DllImport(_dllpath, EntryPoint = "cuTexRefGetBorderColor")]
        public static extern CuResult TexRefGetBorderColor(out float pBorderColor, CuTextRef hTexRef);

        /// <summary>Gets the flags used by a texture reference
        ///
        /// Returns in *<paramref name="pFlags"/> the flags of the texture reference <paramref name="hTexRef"/>.</summary>
        ///
        /// <param name="pFlags">Returned flags</param>
        /// <param name="hTexRef">Texture reference</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa ::cuTexRefSetAddress,
        /// ::cuTexRefSetAddress2D, ::cuTexRefSetAddressMode, ::cuTexRefSetArray,
        /// ::cuTexRefSetFilterMode, ::cuTexRefSetFlags, ::cuTexRefSetFormat,
        /// ::cuTexRefGetAddress, ::cuTexRefGetAddressMode, ::cuTexRefGetArray,
        /// ::cuTexRefGetFilterMode, ::cuTexRefGetFormat
        /// CUresult CUDAAPI cuTexRefGetFlags(unsigned int *pFlags, CUtexref hTexRef);
        [DllImport(_dllpath, EntryPoint = "cuTexRefGetFlags")]
        public static extern CuResult TexRefGetFlags(out TrsfFlags pFlags, CuTextRef hTexRef);
    }
}
