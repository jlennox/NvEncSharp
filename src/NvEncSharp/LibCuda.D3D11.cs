using System;
using System.Runtime.InteropServices;

// ReSharper disable UnusedMember.Global

namespace Lennox.NvEncSharp
{
    public unsafe partial class LibCuda
    {
        /// <summary>Gets the CUDA device corresponding to a display adapter.</summary>
        ///
        /// Returns in *<paramref name="cudaDevice"/> the CUDA-compatible device corresponding to the
        /// adapter <paramref name="pAdapter"/> obtained from ::IDXGIFactory::EnumAdapters.
        ///
        /// If no device on <paramref name="pAdapter"/> is CUDA-compatible the call will return
        /// ::CUDA_ERROR_NO_DEVICE.
        ///
        /// <param name="cudaDevice">Returned CUDA device corresponding to <paramref name="pAdapter"/></param>
        /// <param name="pAdapter">Adapter to query for CUDA device</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_NO_DEVICE,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_NOT_FOUND,
        /// ::CUDA_ERROR_UNKNOWN
        /// </returns>
        /// \notefnerr
        ///
        /// \sa
        /// ::cuD3D11GetDevices,
        /// ::cudaD3D11GetDevice
        ///
        /// CUresult CUDAAPI cuD3D11GetDevice(CUdevice* pCudaDevice, IDXGIAdapter* pAdapter);
        [DllImport(_dllpath, EntryPoint = "cuD3D11GetDevice")]
        public static extern CuResult D3D11GetDevice(out CuDevice cudaDevice, IntPtr pAdapter);

        /// <summary>Gets the CUDA devices corresponding to a Direct3D 11 device</summary>
        ///
        /// Returns in *<paramref name="cudaDeviceCount"/> the number of CUDA-compatible device corresponding
        /// to the Direct3D 11 device <paramref name="pD3D11Device"/>.
        /// Also returns in *<paramref name="cudaDevices"/> at most <paramref name="cudaDeviceCount"/> of the CUDA-compatible devices
        /// corresponding to the Direct3D 11 device <paramref name="pD3D11Device"/>.
        ///
        /// If any of the GPUs being used to render <c>pDevice</c> are not CUDA capable then the
        /// call will return ::CUDA_ERROR_NO_DEVICE.
        ///
        /// <param name="returnedCudaDeviceCount">Returned number of CUDA devices corresponding to <paramref name="pD3D11Device"/></param>
        /// <param name="cudaDevices">Returned CUDA devices corresponding to <paramref name="pD3D11Device"/></param>
        /// <param name="cudaDeviceCount">The size of the output device array <paramref name="cudaDevices"/></param>
        /// <param name="pD3D11Device">Direct3D 11 device to query for CUDA devices</param>
        /// <param name="deviceList">The set of devices to return.  This set may be
        /// ::CU_D3D11_DEVICE_LIST_ALL for all devices,
        /// ::CU_D3D11_DEVICE_LIST_CURRENT_FRAME for the devices used to
        /// render the current frame (in SLI), or
        /// ::CU_D3D11_DEVICE_LIST_NEXT_FRAME for the devices used to
        /// render the next frame (in SLI).</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_NO_DEVICE,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_NOT_FOUND,
        /// ::CUDA_ERROR_UNKNOWN
        /// </returns>
        /// \notefnerr
        ///
        /// \sa
        /// ::cuD3D11GetDevice,
        /// ::cudaD3D11GetDevices
        ///
        /// CUresult CUDAAPI cuD3D11GetDevices(unsigned int *pCudaDeviceCount, CUdevice *pCudaDevices, int cudaDeviceCount, ref ID3D11Device pD3D11Device, CUd3d11DeviceList deviceList);
        [DllImport(_dllpath, EntryPoint = "cuD3D11GetDevices")]
        public static extern CuResult D3D11GetDevices(out int returnedCudaDeviceCount, CuDevice* cudaDevices, int cudaDeviceCount, IntPtr pD3D11Device, CuD3D11DeviceList deviceList);

        /// <summary>Register a Direct3D 11 resource for access by CUDA</summary>
        ///
        /// Registers the Direct3D 11 resource <paramref name="pD3DResource"/> for access by CUDA and
        /// returns a CUDA handle to <paramref name="pD3DResource"/> in <paramref name="cudaResource"/>.
        /// The handle returned in <paramref name="cudaResource"/> may be used to map and unmap this
        /// resource until it is unregistered.
        /// On success this call will increase the internal reference count on
        /// <paramref name="pD3DResource"/>. This reference count will be decremented when this
        /// resource is unregistered through ::cuGraphicsUnregisterResource().
        ///
        /// This call is potentially high-overhead and should not be called every frame
        /// in interactive applications.
        ///
        /// The type of <paramref name="pD3DResource"/> must be one of the following.
        /// - ::ID3D11Buffer: may be accessed through a device pointer.
        /// - ::ID3D11Texture1D: individual subresources of the texture may be accessed via arrays
        /// - ::ID3D11Texture2D: individual subresources of the texture may be accessed via arrays
        /// - ::ID3D11Texture3D: individual subresources of the texture may be accessed via arrays
        ///
        /// The <paramref name="flags"/> argument may be used to specify additional parameters at register
        /// time.  The valid values for this parameter are
        /// - ::CU_GRAPHICS_REGISTER_FLAGS_NONE: Specifies no hints about how this
        ///   resource will be used.
        /// - ::CU_GRAPHICS_REGISTER_FLAGS_SURFACE_LDST: Specifies that CUDA will
        ///   bind this resource to a surface reference.
        /// - ::CU_GRAPHICS_REGISTER_FLAGS_TEXTURE_GATHER: Specifies that CUDA will perform
        ///   texture gather operations on this resource.
        ///
        /// Not all Direct3D resources of the above types may be used for
        /// interoperability with CUDA.  The following are some limitations.
        /// - The primary rendertarget may not be registered with CUDA.
        /// - Textures which are not of a format which is 1, 2, or 4 channels of 8, 16,
        ///   or 32-bit integer or floating-point data cannot be shared.
        /// - Surfaces of depth or stencil formats cannot be shared.
        ///
        /// A complete list of supported DXGI formats is as follows. For compactness the
        /// notation A_{B,C,D} represents A_B, A_C, and A_D.
        /// - DXGI_FORMAT_A8_UNORM
        /// - DXGI_FORMAT_B8G8R8A8_UNORM
        /// - DXGI_FORMAT_B8G8R8X8_UNORM
        /// - DXGI_FORMAT_R16_FLOAT
        /// - DXGI_FORMAT_R16G16B16A16_{FLOAT,SINT,SNORM,UINT,UNORM}
        /// - DXGI_FORMAT_R16G16_{FLOAT,SINT,SNORM,UINT,UNORM}
        /// - DXGI_FORMAT_R16_{SINT,SNORM,UINT,UNORM}
        /// - DXGI_FORMAT_R32_FLOAT
        /// - DXGI_FORMAT_R32G32B32A32_{FLOAT,SINT,UINT}
        /// - DXGI_FORMAT_R32G32_{FLOAT,SINT,UINT}
        /// - DXGI_FORMAT_R32_{SINT,UINT}
        /// - DXGI_FORMAT_R8G8B8A8_{SINT,SNORM,UINT,UNORM,UNORM_SRGB}
        /// - DXGI_FORMAT_R8G8_{SINT,SNORM,UINT,UNORM}
        /// - DXGI_FORMAT_R8_{SINT,SNORM,UINT,UNORM}
        ///
        /// If <paramref name="pD3DResource"/> is of incorrect type or is already registered then
        /// ::CUDA_ERROR_INVALID_HANDLE is returned.
        /// If <paramref name="pD3DResource"/> cannot be registered then ::CUDA_ERROR_UNKNOWN is returned.
        /// If <paramref name="flags"/> is not one of the above specified value then ::CUDA_ERROR_INVALID_VALUE
        /// is returned.
        ///
        /// <param name="cudaResource">Returned graphics resource handle</param>
        /// <param name="pD3DResource">Direct3D resource to register</param>
        /// <param name="flags">Parameters for resource registration</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_OUT_OF_MEMORY,
        /// ::CUDA_ERROR_UNKNOWN
        /// </returns>
        /// \notefnerr
        ///
        /// \sa
        /// ::cuGraphicsUnregisterResource,
        /// ::cuGraphicsMapResources,
        /// ::cuGraphicsSubResourceGetMappedArray,
        /// ::cuGraphicsResourceGetMappedPointer,
        /// ::cudaGraphicsD3D11RegisterResource
        /// CUresult CUDAAPI cuGraphicsD3D11RegisterResource(CUgraphicsResource *pCudaResource, ID3D11Resource *pD3DResource, unsigned int Flags);
        [DllImport(_dllpath, EntryPoint = "cuGraphicsD3D11RegisterResource")]
        public static extern CuResult GraphicsD3D11RegisterResource(out CuGraphicsResource cudaResource, IntPtr pD3DResource, CuGraphicsRegisters flags);
    }
}
