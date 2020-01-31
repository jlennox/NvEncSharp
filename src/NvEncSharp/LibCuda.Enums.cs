using System;

// ReSharper disable UnusedMember.Global

namespace Lennox.NvEncSharp
{
    [Flags]
    public enum CuContextFlags
    {
        /// <summary>Sets SchedAuto.</summary>
        Default = 0x00,
        /// <summary>Automatic scheduling</summary>
        SchedAuto = 0x00,
        /// <summary>Set spin as default scheduling</summary>
        SchedSpin = 0x01,
        /// <summary>Set yield as default scheduling</summary>
        SchedYield = 0x02,
        /// <summary>Set blocking synchronization as default scheduling</summary>
        SchedBlockingSync = 0x04,
        /// <summary>Set blocking synchronization as default scheduling
        /// \deprecated This flag was deprecated as of CUDA 4.0
        /// and was replaced with ::CU_CTX_SCHED_BLOCKING_SYNC.</summary>
        [Obsolete]
        BlockingSync = 0x04,
        SchedMask = 0x07,
        /// <summary>Support mapped pinned allocations</summary>
        MapHost = 0x08,
        /// <summary>Keep local memory allocation after launch</summary>
        LmemResizeToMax = 0x10,
        FlagsMask = 0x1f
    }

    /// <summary>\enum CUdevice_attribute</summary>
    public enum CuDeviceAttribute
    {
        /// <summary>CU_DEVICE_ATTRIBUTE_MAX_THREADS_PER_BLOCK = 1,</summary>
        MaxThreadsPerBlock = 1,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAX_BLOCK_DIM_X = 2,</summary>
        MaxBlockDimX = 2,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAX_BLOCK_DIM_Y = 3,</summary>
        MaxBlockDimY = 3,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAX_BLOCK_DIM_Z = 4,</summary>
        MaxBlockDimZ = 4,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAX_GRID_DIM_X = 5,</summary>
        MaxGridDimX = 5,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAX_GRID_DIM_Y = 6,</summary>
        MaxGridDimY = 6,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAX_GRID_DIM_Z = 7,</summary>
        MaxGridDimZ = 7,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAX_SHARED_MEMORY_PER_BLOCK = 8,</summary>
        MaxSharedMemoryPerBlock = 8,
        /// <summary>CU_DEVICE_ATTRIBUTE_SHARED_MEMORY_PER_BLOCK = 8,</summary>
        SharedMemoryPerBlock = 8,
        /// <summary>CU_DEVICE_ATTRIBUTE_TOTAL_CONSTANT_MEMORY = 9,</summary>
        TotalConstantMemory = 9,
        /// <summary>CU_DEVICE_ATTRIBUTE_WARP_SIZE = 10,</summary>
        WarpSize = 10,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAX_PITCH = 11,</summary>
        MaxPitch = 11,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAX_REGISTERS_PER_BLOCK = 12,</summary>
        MaxRegistersPerBlock = 12,
        /// <summary>CU_DEVICE_ATTRIBUTE_REGISTERS_PER_BLOCK = 12,</summary>
        RegistersPerBlock = 12,
        /// <summary>CU_DEVICE_ATTRIBUTE_CLOCK_RATE = 13,</summary>
        ClockRate = 13,
        /// <summary>CU_DEVICE_ATTRIBUTE_TEXTURE_ALIGNMENT = 14,</summary>
        TextureAlignment = 14,
        /// <summary>CU_DEVICE_ATTRIBUTE_GPU_OVERLAP = 15,</summary>
        GpuOverlap = 15,
        /// <summary>CU_DEVICE_ATTRIBUTE_MULTIPROCESSOR_COUNT = 16,</summary>
        MultiprocessorCount = 16,
        /// <summary>CU_DEVICE_ATTRIBUTE_KERNEL_EXEC_TIMEOUT = 17,</summary>
        KernelExecTimeout = 17,
        /// <summary>CU_DEVICE_ATTRIBUTE_INTEGRATED = 18,</summary>
        Integrated = 18,
        /// <summary>CU_DEVICE_ATTRIBUTE_CAN_MAP_HOST_MEMORY = 19,</summary>
        CanMapHostMemory = 19,
        /// <summary>CU_DEVICE_ATTRIBUTE_COMPUTE_MODE = 20,</summary>
        ComputeMode = 20,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE1D_WIDTH = 21,</summary>
        MaximumTexture1DWidth = 21,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_WIDTH = 22,</summary>
        MaximumTexture2DWidth = 22,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_HEIGHT = 23,</summary>
        MaximumTexture2DHeight = 23,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE3D_WIDTH = 24,</summary>
        MaximumTexture3DWidth = 24,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE3D_HEIGHT = 25,</summary>
        MaximumTexture3DHeight = 25,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE3D_DEPTH = 26,</summary>
        MaximumTexture3DDepth = 26,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_LAYERED_WIDTH = 27,</summary>
        MaximumTexture2DLayeredWidth = 27,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_LAYERED_HEIGHT = 28,</summary>
        MaximumTexture2DLayeredHeight = 28,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_LAYERED_LAYERS = 29,</summary>
        MaximumTexture2DLayeredLayers = 29,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_ARRAY_WIDTH = 27,</summary>
        MaximumTexture2DArrayWidth = 27,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_ARRAY_HEIGHT = 28,</summary>
        MaximumTexture2DArrayHeight = 28,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_ARRAY_NUMSLICES = 29,</summary>
        MaximumTexture2DArrayNumslices = 29,
        /// <summary>CU_DEVICE_ATTRIBUTE_SURFACE_ALIGNMENT = 30,</summary>
        SurfaceAlignment = 30,
        /// <summary>CU_DEVICE_ATTRIBUTE_CONCURRENT_KERNELS = 31,</summary>
        ConcurrentKernels = 31,
        /// <summary>CU_DEVICE_ATTRIBUTE_ECC_ENABLED = 32,</summary>
        EccEnabled = 32,
        /// <summary>CU_DEVICE_ATTRIBUTE_PCI_BUS_ID = 33,</summary>
        PciBusId = 33,
        /// <summary>CU_DEVICE_ATTRIBUTE_PCI_DEVICE_ID = 34,</summary>
        PciDeviceId = 34,
        /// <summary>CU_DEVICE_ATTRIBUTE_TCC_DRIVER = 35,</summary>
        TccDriver = 35,
        /// <summary>CU_DEVICE_ATTRIBUTE_MEMORY_CLOCK_RATE = 36,</summary>
        MemoryClockRate = 36,
        /// <summary>CU_DEVICE_ATTRIBUTE_GLOBAL_MEMORY_BUS_WIDTH = 37,</summary>
        GlobalMemoryBusWidth = 37,
        /// <summary>CU_DEVICE_ATTRIBUTE_L2_CACHE_SIZE = 38,</summary>
        L2CacheSize = 38,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAX_THREADS_PER_MULTIPROCESSOR = 39,</summary>
        MaxThreadsPerMultiprocessor = 39,
        /// <summary>CU_DEVICE_ATTRIBUTE_ASYNC_ENGINE_COUNT = 40,</summary>
        AsyncEngineCount = 40,
        /// <summary>CU_DEVICE_ATTRIBUTE_UNIFIED_ADDRESSING = 41,</summary>
        UnifiedAddressing = 41,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE1D_LAYERED_WIDTH = 42,</summary>
        MaximumTexture1DLayeredWidth = 42,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE1D_LAYERED_LAYERS = 43,</summary>
        MaximumTexture1DLayeredLayers = 43,
        /// <summary>CU_DEVICE_ATTRIBUTE_CAN_TEX2D_GATHER = 44,</summary>
        CanTex2DGather = 44,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_GATHER_WIDTH = 45,</summary>
        MaximumTexture2DGatherWidth = 45,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_GATHER_HEIGHT = 46,</summary>
        MaximumTexture2DGatherHeight = 46,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE3D_WIDTH_ALTERNATE = 47,</summary>
        MaximumTexture3DWidthAlternate = 47,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE3D_HEIGHT_ALTERNATE = 48,</summary>
        MaximumTexture3DHeightAlternate = 48,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE3D_DEPTH_ALTERNATE = 49,</summary>
        MaximumTexture3DDepthAlternate = 49,
        /// <summary>CU_DEVICE_ATTRIBUTE_PCI_DOMAIN_ID = 50,</summary>
        PciDomainId = 50,
        /// <summary>CU_DEVICE_ATTRIBUTE_TEXTURE_PITCH_ALIGNMENT = 51,</summary>
        TexturePitchAlignment = 51,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURECUBEMAP_WIDTH = 52,</summary>
        MaximumTexturecubemapWidth = 52,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURECUBEMAP_LAYERED_WIDTH = 53,</summary>
        MaximumTexturecubemapLayeredWidth = 53,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURECUBEMAP_LAYERED_LAYERS = 54,</summary>
        MaximumTexturecubemapLayeredLayers = 54,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_SURFACE1D_WIDTH = 55,</summary>
        MaximumSurface1DWidth = 55,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_SURFACE2D_WIDTH = 56,</summary>
        MaximumSurface2DWidth = 56,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_SURFACE2D_HEIGHT = 57,</summary>
        MaximumSurface2DHeight = 57,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_SURFACE3D_WIDTH = 58,</summary>
        MaximumSurface3DWidth = 58,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_SURFACE3D_HEIGHT = 59,</summary>
        MaximumSurface3DHeight = 59,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_SURFACE3D_DEPTH = 60,</summary>
        MaximumSurface3DDepth = 60,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_SURFACE1D_LAYERED_WIDTH = 61,</summary>
        MaximumSurface1DLayeredWidth = 61,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_SURFACE1D_LAYERED_LAYERS = 62,</summary>
        MaximumSurface1DLayeredLayers = 62,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_SURFACE2D_LAYERED_WIDTH = 63,</summary>
        MaximumSurface2DLayeredWidth = 63,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_SURFACE2D_LAYERED_HEIGHT = 64,</summary>
        MaximumSurface2DLayeredHeight = 64,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_SURFACE2D_LAYERED_LAYERS = 65,</summary>
        MaximumSurface2DLayeredLayers = 65,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_SURFACECUBEMAP_WIDTH = 66,</summary>
        MaximumSurfacecubemapWidth = 66,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_SURFACECUBEMAP_LAYERED_WIDTH = 67,</summary>
        MaximumSurfacecubemapLayeredWidth = 67,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_SURFACECUBEMAP_LAYERED_LAYERS = 68,</summary>
        MaximumSurfacecubemapLayeredLayers = 68,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE1D_LINEAR_WIDTH = 69,</summary>
        MaximumTexture1DLinearWidth = 69,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_LINEAR_WIDTH = 70,</summary>
        MaximumTexture2DLinearWidth = 70,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_LINEAR_HEIGHT = 71,</summary>
        MaximumTexture2DLinearHeight = 71,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_LINEAR_PITCH = 72,</summary>
        MaximumTexture2DLinearPitch = 72,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_MIPMAPPED_WIDTH = 73,</summary>
        MaximumTexture2DMipmappedWidth = 73,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_MIPMAPPED_HEIGHT = 74,</summary>
        MaximumTexture2DMipmappedHeight = 74,
        /// <summary>CU_DEVICE_ATTRIBUTE_COMPUTE_CAPABILITY_MAJOR = 75,</summary>
        ComputeCapabilityMajor = 75,
        /// <summary>CU_DEVICE_ATTRIBUTE_COMPUTE_CAPABILITY_MINOR = 76,</summary>
        ComputeCapabilityMinor = 76,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE1D_MIPMAPPED_WIDTH = 77,</summary>
        MaximumTexture1DMipmappedWidth = 77,
        /// <summary>CU_DEVICE_ATTRIBUTE_STREAM_PRIORITIES_SUPPORTED = 78,</summary>
        StreamPrioritiesSupported = 78,
        /// <summary>CU_DEVICE_ATTRIBUTE_GLOBAL_L1_CACHE_SUPPORTED = 79,</summary>
        GlobalL1CacheSupported = 79,
        /// <summary>CU_DEVICE_ATTRIBUTE_LOCAL_L1_CACHE_SUPPORTED = 80,</summary>
        LocalL1CacheSupported = 80,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAX_SHARED_MEMORY_PER_MULTIPROCESSOR = 81,</summary>
        MaxSharedMemoryPerMultiprocessor = 81,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAX_REGISTERS_PER_MULTIPROCESSOR = 82,</summary>
        MaxRegistersPerMultiprocessor = 82,
        /// <summary>CU_DEVICE_ATTRIBUTE_MANAGED_MEMORY = 83,</summary>
        ManagedMemory = 83,
        /// <summary>CU_DEVICE_ATTRIBUTE_MULTI_GPU_BOARD = 84,</summary>
        MultiGpuBoard = 84,
        /// <summary>CU_DEVICE_ATTRIBUTE_MULTI_GPU_BOARD_GROUP_ID = 85,</summary>
        MultiGpuBoardGroupId = 85,
        /// <summary>CU_DEVICE_ATTRIBUTE_HOST_NATIVE_ATOMIC_SUPPORTED = 86,</summary>
        HostNativeAtomicSupported = 86,
        /// <summary>CU_DEVICE_ATTRIBUTE_SINGLE_TO_DOUBLE_PRECISION_PERF_RATIO = 87,</summary>
        SingleToDoublePrecisionPerfRatio = 87,
        /// <summary>CU_DEVICE_ATTRIBUTE_PAGEABLE_MEMORY_ACCESS = 88,</summary>
        PageableMemoryAccess = 88,
        /// <summary>CU_DEVICE_ATTRIBUTE_CONCURRENT_MANAGED_ACCESS = 89,</summary>
        ConcurrentManagedAccess = 89,
        /// <summary>CU_DEVICE_ATTRIBUTE_COMPUTE_PREEMPTION_SUPPORTED = 90,</summary>
        ComputePreemptionSupported = 90,
        /// <summary>CU_DEVICE_ATTRIBUTE_CAN_USE_HOST_POINTER_FOR_REGISTERED_MEM = 91,</summary>
        CanUseHostPointerForRegisteredMem = 91,
        /// <summary>CU_DEVICE_ATTRIBUTE_CAN_USE_STREAM_MEM_OPS = 92,</summary>
        CanUseStreamMemOps = 92,
        /// <summary>CU_DEVICE_ATTRIBUTE_CAN_USE_64_BIT_STREAM_MEM_OPS = 93,</summary>
        CanUse64BitStreamMemOps = 93,
        /// <summary>CU_DEVICE_ATTRIBUTE_CAN_USE_STREAM_WAIT_VALUE_NOR = 94,</summary>
        CanUseStreamWaitValueNor = 94,
        /// <summary>CU_DEVICE_ATTRIBUTE_COOPERATIVE_LAUNCH = 95,</summary>
        CooperativeLaunch = 95,
        /// <summary>CU_DEVICE_ATTRIBUTE_COOPERATIVE_MULTI_DEVICE_LAUNCH = 96,</summary>
        CooperativeMultiDeviceLaunch = 96,
        /// <summary>CU_DEVICE_ATTRIBUTE_MAX_SHARED_MEMORY_PER_BLOCK_OPTIN = 97</summary>
        MaxSharedMemoryPerBlockOptin = 97
    }

    [Flags]
    public enum CuGraphicsRegisters
    {
        None = 0x00,
        ReadOnly = 0x01,
        WriteDiscard = 0x02,
        SurfaceLdst = 0x04,
        TextureGather = 0x08
    }

    /// <summary>Flags for mapping and unmapping interop resources</summary>
    [Flags]
    public enum CuGraphicsMapResources
    {
        None = 0x00,
        ReadOnly = 0x01,
        WriteDiscard = 0x02
    }

    /// <summary>Array indices for cube faces</summary>
    public enum CuArrayCubemapFace
    {
        /// <summary>Positive X face of cubemap</summary>
        PositiveX = 0x00,
        /// <summary>Negative X face of cubemap</summary>
        NegativeX = 0x01,
        /// <summary>Positive Y face of cubemap</summary>
        PositiveY = 0x02,
        /// <summary>Negative Y face of cubemap</summary>
        NegativeY = 0x03,
        /// <summary>Positive Z face of cubemap</summary>
        PositiveZ = 0x04,
        /// <summary>Negative Z face of cubemap</summary>
        NegativeZ = 0x05
    }

    /// <summary>Limits</summary>
    public enum CuLimit
    {
        /// <summary>GPU thread stack size</summary>
        StackSize = 0x00,
        /// <summary>GPU printf FIFO size</summary>
        PrintfFifoSize = 0x01,
        /// <summary>GPU malloc heap size</summary>
        MallocHeapSize = 0x02,
        /// <summary>GPU device runtime launch synchronize depth</summary>
        DevRuntimeSyncDepth = 0x03,
        /// <summary>GPU device runtime pending launch count</summary>
        DevRuntimePendingLaunchCount = 0x04,
        Max
    }

    /// <summary>Resource types</summary>
    public enum CuResourceType
    {
        /// <summary>Array resoure</summary>
        Array = 0x00,
        /// <summary>Mipmapped array resource</summary>
        MipmappedArray = 0x01,
        /// <summary>Linear resource</summary>
        Linear = 0x02,
        /// <summary>Pitch 2D resource</summary>
        Pitch2D = 0x03
    }

    /// <summary>Memory types</summary>
    public enum CuMemoryType
    {
        /// <summary>Host memory</summary>
        Host = 0x01,
        /// <summary>Device memory</summary>
        Device = 0x02,
        /// <summary>Array memory</summary>
        Array = 0x03,
        /// <summary>Unified device or host memory</summary>
        Unified = 0x04
    }

    /// <summary>Function cache configurations</summary>
    public enum CuFunctionCache
    {
        /// <summary>no preference for shared memory or L1 (default)</summary>
        PreferNone = 0x00,
        /// <summary>prefer larger shared memory and smaller L1 cache</summary>
        PreferShared = 0x01,
        /// <summary>prefer larger L1 cache and smaller shared memory</summary>
        PreferL1 = 0x02,
        /// <summary>prefer equal sized L1 cache and shared memory</summary>
        PreferEqual = 0x03
    }

    /// <summary>Shared memory configurations</summary>
    public enum CuSharedConfig
    {
        /// <summary>set default shared memory bank size</summary>
        DefaultBankSize = 0x00,
        /// <summary>set shared memory bank width to four bytes</summary>
        FourByteBankSize = 0x01,
        /// <summary>set shared memory bank width to eight bytes</summary>
        EightByteBankSize = 0x02
    }

    /// <summary>Shared memory carveout configurations</summary>
    public enum CuSharedCarveout
    {
        /// <summary>no preference for shared memory or L1 (default)</summary>
        Default = -1,
        /// <summary>prefer maximum available shared memory, minimum L1 cache</summary>
        MaxShared = 100,
        /// <summary>prefer maximum available L1 cache, minimum shared memory</summary>
        MaxL1 = 0
    }

    /// <summary>Compute Modes</summary>
    public enum CuComputeMode
    {
        /// <summary>Default compute mode (Multiple contexts allowed per device)</summary>
        Default = 0,
        /// <summary>Compute-prohibited mode (No contexts can be created on this device at this time)</summary>
        Prohibited = 2,
        /// <summary>Compute-exclusive-process mode (Only one context used by a single process can be present on this device at a time)</summary>
        ExclusiveProcess = 3
    }

    /// <summary>Memory advise values</summary>
    public enum CuMemAdvice
    {
        /// <summary>Data will mostly be read and only occassionally be written to</summary>
        SetReadMostly = 1,
        /// <summary>Undo the effect of ::CU_MEM_ADVISE_SET_READ_MOSTLY</summary>
        UnsetReadMostly = 2,
        /// <summary>Set the preferred location for the data as the specified device</summary>
        SetPreferredLocation = 3,
        /// <summary>Clear the preferred location for the data</summary>
        UnsetPreferredLocation = 4,
        /// <summary>Data will be accessed by the specified device, so prevent page faults as much as possible</summary>
        SetAccessedBy = 5,
        /// <summary>Let the Unified Memory subsystem decide on the page faulting policy for the specified device</summary>
        UnsetAccessedBy = 6
    }

    public enum CuMemRangeAttribute
    {
        /// <summary>Whether the range will mostly be read and only occassionally be written to</summary>
        ReadMostly = 1,
        /// <summary>The preferred location of the range</summary>
        PreferredLocation = 2,
        /// <summary>Memory range has ::CU_MEM_ADVISE_SET_ACCESSED_BY set for specified device</summary>
        AccessedBy = 3,
        /// <summary>The last location to which the range was prefetched</summary>
        LastPrefetchLocation = 4
    }

    /// <summary>Occupancy calculator flag</summary>
    [Flags]
    public enum CuOccupancyFlags
    {
        /// <summary>Default behavior</summary>
        Default = 0x0,
        /// <summary>Assume global caching is enabled and cannot be automatically turned off</summary>
        DisableCachingOverride = 0x1
    }

    /// <summary>Array formats</summary>
    public enum CuArrayFormat
    {
        /// <summary>Unsigned 8-bit integers</summary>
        UnsignedInt8 = 0x01,
        /// <summary>Unsigned 16-bit integers</summary>
        UnsignedInt16 = 0x02,
        /// <summary>Unsigned 32-bit integers</summary>
        UnsignedInt32 = 0x03,
        /// <summary>Signed 8-bit integers</summary>
        SignedInt8 = 0x08,
        /// <summary>Signed 16-bit integers</summary>
        SignedInt16 = 0x09,
        /// <summary>Signed 32-bit integers</summary>
        SignedInt32 = 0x0a,
        /// <summary>16-bit floating point</summary>
        Half = 0x10,
        /// <summary>32-bit floating point</summary>
        Float = 0x20
    }

    /// <summary>Texture reference addressing modes</summary>
    public enum CuAddressMode
    {
        /// <summary>Wrapping address mode</summary>
        Wrap = 0,
        /// <summary>Clamp to edge address mode</summary>
        Clamp = 1,
        /// <summary>Mirror address mode</summary>
        Mirror = 2,
        /// <summary>Border address mode</summary>
        Border = 3
    }

    /// <summary>Texture reference filtering modes</summary>
    public enum CuFilterMode
    {
        /// <summary>Point filter mode</summary>
        Point = 0,
        /// <summary>Linear filter mode</summary>
        Linear = 1
    }

    /// <summary>Stream creation flags</summary>
    [Flags]
    public enum CuStreamFlags
    {
        /// <summary>Default stream flag</summary>
        Default = 0x0,
        /// <summary>Stream does not synchronize with stream 0 (the NULL stream)</summary>
        NonBlocking = 0x1
    }

    /// <summary>Event creation flags</summary>
    [Flags]
    public enum CuEventFlags
    {
        /// <summary>Default event flag</summary>
        Default = 0x0,
        /// <summary>Event uses blocking synchronization</summary>
        BlockingSync = 0x1,
        /// <summary>Event will not record timing data</summary>
        DisableTiming = 0x2,
        /// <summary>Event is suitable for interprocess use. CU_EVENT_DISABLE_TIMING must be set</summary>
        Interprocess = 0x4
    }

    /// <summary>CUDA Ipc Mem Flags</summary>
    [Flags]
    public enum CuIpcMemFlags
    {
        /// <summary>Automatically enable peer access between remote devices as needed</summary>
        LazyEnablePeerAccess = 0x1
    }

    /// <summary>CUDA Mem Attach Flags</summary>
    [Flags]
    public enum CuMemAttachFlags
    {
        /// <summary>Memory can be accessed by any stream on any device</summary>
        Global = 0x1,
        /// <summary>Memory cannot be accessed by any stream on any device</summary>
        Host = 0x2,
        /// <summary>Memory can only be accessed by a single stream on the associated device</summary>
        Single = 0x4
    }
}
