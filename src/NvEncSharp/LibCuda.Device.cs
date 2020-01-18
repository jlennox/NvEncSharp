using System;
using System.Runtime.InteropServices;

namespace Lennox.NvEncSharp
{
    public static unsafe partial class LibCuda
    {
        /// <summary>CUresult cuDeviceGet ( CUdevice* device, int  ordinal )
        /// Returns a handle to a compute device.</summary>
        [DllImport(_dllpath, EntryPoint = "cuDeviceGet")]
        public static extern CuResult DeviceGet(out CuDevice device, int ordinal);

        /// <summary>CUresult cuDeviceGetAttribute ( int* pi, CUdevice_attribute attrib, CUdevice dev )
        /// Returns information about the device.</summary>
        [DllImport(_dllpath, EntryPoint = "cuDeviceGetAttribute")]
        public static extern CuResult DeviceGetAttribute(out int pi, CuDeviceAttribute attrib, CuDevice device);

        /// <summary>CUresult cuDeviceGetCount ( int* count )
        /// Returns the number of compute-capable devices.</summary>
        [DllImport(_dllpath, EntryPoint = "cuDeviceGetCount")]
        public static extern CuResult DeviceGetCount(out int count);

        /// <summary>CUresult cuDeviceGetLuid ( char* luid, unsigned int* deviceNodeMask, CUdevice dev )
        /// Return an LUID and device node mask for the device.</summary>
        [DllImport(_dllpath, EntryPoint = "cuDeviceGetLuid")]
        public static extern CuResult DeviceGetLuid(out byte luid, out uint deviceNodeMask, CuDevice device);

        /// <summary>CUresult cuDeviceGetName ( char* name, int  len, CUdevice dev )
        /// Returns an identifer string for the device.</summary>
        [DllImport(_dllpath, EntryPoint = "cuDeviceGetName")]
        public static extern CuResult DeviceGetName(byte* name, int len, CuDevice device);

        /// <summary>CUresult cuDeviceGetNvSciSyncAttributes ( void* nvSciSyncAttrList, CUdevice dev, int  flags )
        /// Return NvSciSync attributes that this device can support.</summary>
        [DllImport(_dllpath, EntryPoint = "cuDeviceGetNvSciSyncAttributes")]
        public static extern CuResult DeviceGetNvSciSyncAttributes(IntPtr nvSciSyncAttrList, CuDevice device, int flags);

        /// <summary>CUresult cuDeviceGetUuid ( CUuuid* uuid, CUdevice dev )
        /// Return an UUID for the device.</summary>
        // TODO: Does CUuuid == GUID?
        [DllImport(_dllpath, EntryPoint = "cuDeviceGetUuid")]
        public static extern CuResult DeviceGetUuid(out Guid uuid, CuDevice device);

        /// <summary>CUresult cuDeviceTotalMem ( size_t* bytes, CUdevice dev )
        /// Returns the total amount of memory on the device.</summary>
        [DllImport(_dllpath, EntryPoint = "cuDeviceTotalMem" + _ver)]
        public static extern CuResult DeviceTotalMemory(out IntPtr bytes, CuDevice device);
    }
}