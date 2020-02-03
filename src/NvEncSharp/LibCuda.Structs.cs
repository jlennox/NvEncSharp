using System;
using System.Runtime.InteropServices;
using static Lennox.NvEncSharp.LibCuda;

// ReSharper disable UnusedMember.Global

namespace Lennox.NvEncSharp
{
    /// <summary>2D memory copy parameters</summary>
    public struct CudaMemcopy2D
    {
        /// <summary>Source X in bytes</summary>
        public IntPtr SrcXInBytes;
        /// <summary>Source Y</summary>
        public IntPtr SrcY;

        /// <summary>Source memory type (host, device, array)</summary>
        public CuMemoryType SrcMemoryType;
        /// <summary>Source host pointer</summary>
        public IntPtr SrcHost;
        /// <summary>Source device pointer</summary>
        public CuDevicePtr SrcDevice;
        /// <summary>Source array reference</summary>
        public CuArray SrcArray;
        /// <summary>Source pitch (ignored when src is array)</summary>
        public IntPtr SrcPitch;

        /// <summary>Destination X in bytes</summary>
        public IntPtr DstXInBytes;
        /// <summary>Destination Y</summary>
        public IntPtr DstY;

        /// <summary>Destination memory type (host, device, array)</summary>
        public CuMemoryType DstMemoryType;
        /// <summary>Destination host pointer</summary>
        public IntPtr DstHost;
        /// <summary>Destination device pointer</summary>
        public CuDevicePtr DstDevice;
        /// <summary>Destination array reference</summary>
        public CuArray DstArray;
        /// <summary>Destination pitch (ignored when dst is array)</summary>
        public IntPtr DstPitch;

        /// <summary>Width of 2D memory copy in bytes</summary>
        public IntPtr WidthInBytes;
        /// <summary>Height of 2D memory copy</summary>
        public IntPtr Height;

        /// <inheritdoc cref="LibCuda.Memcpy2D(ref CudaMemcopy2D)"/>
        public void Memcpy2D()
        {
            var result = LibCuda.Memcpy2D(ref this);
            CheckResult(result);
        }
    }

    public struct CudaMemcpy3D
    {
        /// <summary>Source X in bytes</summary>
        public uint SrcXInBytes;
        /// <summary>Source Y</summary>
        public uint SrcY;
        /// <summary>Source Z</summary>
        public uint SrcZ;
        /// <summary>Source LOD</summary>
        public uint SrcLod;
        /// <summary>Source memory type (host, device, array)</summary>
        public CuMemoryType SrcMemoryType;
        /// <summary>Source host pointer</summary>
        public IntPtr SrcHost;
        /// <summary>Source device pointer</summary>
        public CuDevicePtr SrcDevice;
        /// <summary>Source array reference</summary>
        public CuArray SrcArray;
        /// <summary>Must be NULL</summary>
        private IntPtr _reserved0;
        /// <summary>Source pitch (ignored when src is array)</summary>
        public uint SrcPitch;
        /// <summary>Source height (ignored when src is array; may be 0 if Depth==1)</summary>
        public uint SrcHeight;

        /// <summary>Destination X in bytes</summary>
        public uint DstXInBytes;
        /// <summary>Destination Y</summary>
        public uint DstY;
        /// <summary>Destination Z</summary>
        public uint DstZ;
        /// <summary>Destination LOD</summary>
        public uint DstLod;
        /// <summary>Destination memory type (host, device, array)</summary>
        public CuMemoryType DstMemoryType;
        /// <summary>Destination host pointer</summary>
        public IntPtr DstHost;
        /// <summary>Destination device pointer</summary>
        public CuDevicePtr DstDevice;
        /// <summary>Destination array reference</summary>
        public CuArray DstArray;
        /// <summary>Must be NULL</summary>
        public IntPtr Reserved1;
        /// <summary>Destination pitch (ignored when dst is array)</summary>
        public uint DstPitch;
        /// <summary>Destination height (ignored when dst is array; may be 0 if Depth==1)</summary>
        public uint DstHeight;

        /// <summary>Width of 3D memory copy in bytes</summary>
        public uint WidthInBytes;
        /// <summary>Height of 3D memory copy</summary>
        public uint Height;
        /// <summary>Depth of 3D memory copy</summary>
        public uint Depth;
    }

    public struct CudaMemcpy3DPeer
    {
        /// <summary>Source X in bytes</summary>
        public IntPtr SrcXInBytes;
        /// <summary>Source Y</summary>
        public IntPtr SrcY;
        /// <summary>Source Z</summary>
        public IntPtr SrcZ;
        /// <summary>Source LOD</summary>
        public IntPtr SrcLod;
        /// <summary>Source memory type (host, device, array)</summary>
        public CuMemoryType SrcMemoryType;
        /// <summary>Source host pointer</summary>
        public IntPtr SrcHost;
        /// <summary>Source device pointer</summary>
        public CuDevicePtr SrcDevice;
        /// <summary>Source array reference</summary>
        public CuArray SrcArray;
        /// <summary>Source context (ignored with srcMemoryType is ::CU_MEMORYTYPE_ARRAY)</summary>
        public CuContext SrcContext;
        /// <summary>Source pitch (ignored when src is array)</summary>
        public IntPtr SrcPitch;
        /// <summary>Source height (ignored when src is array; may be 0 if Depth==1)</summary>
        public IntPtr SrcHeight;

        /// <summary>Destination X in bytes</summary>
        public IntPtr DstXInBytes;
        /// <summary>Destination Y</summary>
        public IntPtr DstY;
        /// <summary>Destination Z</summary>
        public IntPtr DstZ;
        /// <summary>Destination LOD</summary>
        public IntPtr DstLod;
        /// <summary>Destination memory type (host, device, array)</summary>
        public CuMemoryType DstMemoryType;
        /// <summary>Destination host pointer</summary>
        public IntPtr DstHost;
        /// <summary>Destination device pointer</summary>
        public CuDevicePtr DstDevice;
        /// <summary>Destination array reference</summary>
        public CuArray DstArray;
        /// <summary>Destination context (ignored with dstMemoryType is ::CU_MEMORYTYPE_ARRAY)</summary>
        public CuContext DstContext;
        /// <summary>Destination pitch (ignored when dst is array)</summary>
        public IntPtr DstPitch;
        /// <summary>Destination height (ignored when dst is array; may be 0 if Depth==1)</summary>
        public IntPtr DstHeight;

        /// <summary>Width of 3D memory copy in bytes</summary>
        public IntPtr WidthInBytes;
        /// <summary>Height of 3D memory copy</summary>
        public IntPtr Height;
        /// <summary>Depth of 3D memory copy</summary>
        public IntPtr Depth;
    }

    public struct CudaArrayDescription
    {
        /// <summary>Width of array</summary>
        public uint Width;
        /// <summary>Height of array</summary>
        public uint Height;

        /// <summary>Array format</summary>
        public CuArrayFormat Format;
        /// <summary>Channels per array element</summary>
        public uint NumChannels;
    }

    public struct CudaArray3DDescription
    {
        /// <summary>Width of 3D array</summary>
        public uint Width;
        /// <summary>Height of 3D array</summary>
        public uint Height;
        /// <summary>Depth of 3D array</summary>
        public uint Depth;

        /// <summary>Array format</summary>
        public CuArrayFormat Format;
        /// <summary>Channels per array element</summary>
        public uint NumChannels;
        /// <summary>Flags</summary>
        public uint Flags;
    }

    public struct CuStreamMemOpWaitValueParams
    {
        public CuStreamBatchMemOpType Operation;
        public CuDevicePtr Address;
        public long Value64;
        public int Flags;
        /// <summary>For driver internal use. Initial value is unimportant.</summary>
        public CuDevicePtr Alias;
    }

    public struct CuStreamMemOpWriteValueParams
    {
        public CuStreamBatchMemOpType Operation;
        public CuDevicePtr Address;
        public long Value64;
        public int Flags;
        /// <summary>For driver internal use. Initial value is unimportant.</summary>
        public CuDevicePtr Alias;
    }

    public struct CuStreamMemOpFlushRemoteWritesParams
    {
        public CuStreamBatchMemOpType Operation;
        public int Flags;
    }

    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct CuSreamBatchMemOpParams
    {
        [FieldOffset(0)]
        public CuStreamBatchMemOpType Operation;
        [FieldOffset(0)]
        public CuStreamMemOpWaitValueParams WaitValue;
        [FieldOffset(0)]
        public CuStreamMemOpWriteValueParams WriteValue;
        [FieldOffset(0)]
        public CuStreamMemOpFlushRemoteWritesParams FlushRemoteWrites;
        [FieldOffset(0)]
        private fixed long _pad[6];
    }
}
