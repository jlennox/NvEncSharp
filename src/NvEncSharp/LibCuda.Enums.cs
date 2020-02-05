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

    public enum PointerAttribute
    {
        /// <summary>The ::CUcontext on which a pointer was allocated or registered</summary>
        Context = 1,
        /// <summary>The ::CUmemorytype describing the physical location of a pointer</summary>
        MemoryType = 2,
        /// <summary>The address at which a pointer's memory may be accessed on the device</summary>
        DevicePointer = 3,
        /// <summary>The address at which a pointer's memory may be accessed on the host</summary>
        HostPointer = 4,
        /// <summary>A pair of tokens for use with the nv-p2p.h Linux kernel interface</summary>
        P2PTokens = 5,
        /// <summary>Synchronize every synchronous memory operation initiated on this region</summary>
        SyncMemops = 6,
        /// <summary>A process-wide unique ID for an allocated memory region*/</summary>
        BufferId = 7,
        /// <summary>Indicates if the pointer points to managed memory</summary>
        IsManaged = 8
    }

    public enum FunctionAttribute
    {
        /// <summary>The maximum number of threads per block, beyond which a launch of the
        /// function would fail. This number depends on both the function and the
        /// device on which the function is currently loaded.</summary>
        MaxThreadsPerBlock = 0,

        /// <summary>The size in bytes of statically-allocated shared memory required by
        /// this function. This does not include dynamically-allocated shared
        /// memory requested by the user at runtime.</summary>
        SharedSizeBytes = 1,

        /// <summary>The size in bytes of user-allocated constant memory required by this
        /// function.</summary>
        ConstSizeBytes = 2,

        /// <summary>The size in bytes of local memory used by each thread of this function.</summary>
        LocalSizeBytes = 3,

        /// <summary>The number of registers used by each thread of this function.</summary>
        NumRegs = 4,

        /// <summary>The PTX virtual architecture version for which the function was
        /// compiled. This value is the major PTX version * 10 + the minor PTX
        /// version, so a PTX version 1.3 function would return the value 13.
        /// Note that this may return the undefined value of 0 for cubins
        /// compiled prior to CUDA 3.0.</summary>
        PtxVersion = 5,

        /// <summary>The binary architecture version for which the function was compiled.
        /// This value is the major binary version * 10 + the minor binary version,
        /// so a binary version 1.3 function would return the value 13. Note that
        /// this will return a value of 10 for legacy cubins that do not have a
        /// properly-encoded binary architecture version.</summary>
        BinaryVersion = 6,

        /// <summary>The attribute to indicate whether the function has been compiled with
        /// user specified option "-Xptxas --dlcm=ca" set .</summary>
        CacheModeCa = 7,

        /// <summary>The maximum size in bytes of dynamically-allocated shared memory that can be used by
        /// this function. If the user-specified dynamic shared memory size is larger than this
        /// value, the launch will fail.</summary>
        MaxDynamicSharedSizeBytes = 8,

        /// <summary>On devices where the L1 cache and shared memory use the same hardware resources,
        /// this sets the shared memory carveout preference, in percent of the total resources.
        /// This is only a hint, and the driver can choose a different ratio if required to execute the function.</summary>
        PreferredSharedMemoryCarveout = 9,
        Max
    }

    public enum JitOption
    {
        /// <summary>Max number of registers that a thread may use.
        /// Option type: unsigned int
        /// Applies to: compiler only</summary>
        MaxRegisters = 0,

        /// <summary>IN: Specifies minimum number of threads per block to target compilation
        /// for
        /// OUT: Returns the number of threads the compiler actually targeted.
        /// This restricts the resource utilization fo the compiler (e.g. max
        /// registers) such that a block with the given number of threads should be
        /// able to launch based on register limitations. Note, this option does not
        /// currently take into account any other resource limitations, such as
        /// shared memory utilization.
        /// Cannot be combined with ::CU_JIT_TARGET.
        /// Option type: unsigned int
        /// Applies to: compiler only</summary>
        ThreadsPerBlock,

        /// <summary>Overwrites the option value with the total wall clock time, in
        /// milliseconds, spent in the compiler and linker
        /// Option type: float
        /// Applies to: compiler and linker</summary>
        WallTime,

        /// <summary>Pointer to a buffer in which to print any log messages
        /// that are informational in nature (the buffer size is specified via
        /// option ::CU_JIT_INFO_LOG_BUFFER_SIZE_BYTES)
        /// Option type: char *
        /// Applies to: compiler and linker</summary>
        InfoLogBuffer,

        /// <summary>IN: Log buffer size in bytes.  Log messages will be capped at this size
        /// (including null terminator)
        /// OUT: Amount of log buffer filled with messages
        /// Option type: unsigned int
        /// Applies to: compiler and linker</summary>
        InfoLogBufferSizeBytes,

        /// <summary>Pointer to a buffer in which to print any log messages that
        /// reflect errors (the buffer size is specified via option
        /// ::CU_JIT_ERROR_LOG_BUFFER_SIZE_BYTES)
        /// Option type: char *
        /// Applies to: compiler and linker</summary>
        ErrorLogBuffer,

        /// <summary>IN: Log buffer size in bytes.  Log messages will be capped at this size
        /// (including null terminator)
        /// OUT: Amount of log buffer filled with messages
        /// Option type: unsigned int
        /// Applies to: compiler and linker</summary>
        ErrorLogBufferSizeBytes,

        /// <summary>Level of optimizations to apply to generated code (0 - 4), with 4
        /// being the default and highest level of optimizations.
        /// Option type: unsigned int
        /// Applies to: compiler only</summary>
        OptimizationLevel,

        /// <summary>No option value required. Determines the target based on the current
        /// attached context (default)
        /// Option type: No option value needed
        /// Applies to: compiler and linker</summary>
        TargetFromCucontext,

        /// <summary>Target is chosen based on supplied ::CUjit_target.  Cannot be
        /// combined with ::CU_JIT_THREADS_PER_BLOCK.
        /// Option type: unsigned int for enumerated type ::CUjit_target
        /// Applies to: compiler and linker</summary>
        Target,

        /// <summary>Specifies choice of fallback strategy if matching cubin is not found.
        /// Choice is based on supplied ::CUjit_fallback.  This option cannot be
        /// used with cuLink* APIs as the linker requires exact matches.
        /// Option type: unsigned int for enumerated type ::CUjit_fallback
        /// Applies to: compiler only</summary>
        FallbackStrategy,

        /// <summary>Specifies whether to create debug information in output (-g)
        /// (0: false, default)
        /// Option type: int
        /// Applies to: compiler and linker</summary>
        GenerateDebugInfo,

        /// <summary>Generate verbose log messages (0: false, default)
        /// Option type: int
        /// Applies to: compiler and linker</summary>
        LogVerbose,

        /// <summary>Generate line number information (-lineinfo) (0: false, default)
        /// Option type: int
        /// Applies to: compiler only</summary>
        GenerateLineInfo,

        /// <summary>Specifies whether to enable caching explicitly (-dlcm)
        /// Choice is based on supplied ::CUjit_cacheMode_enum.
        /// Option type: unsigned int for enumerated type ::CUjit_cacheMode_enum
        /// Applies to: compiler only</summary>
        CacheMode,

        /// <summary>The below jit options are used for internal purposes only, in this version of CUDA</summary>
        NewSm3XOpt,
        FastCompile,
        NumOptions
    }

    public enum JitInputType
    {
        /// <summary>Compiled device-class-specific device code\n
        /// Applicable options: none</summary>
        Cubin = 0,

        /// <summary>PTX source code\n
        /// Applicable options: PTX compiler options</summary>
        Ptx,

        /// <summary>Bundle of multiple cubins and/or PTX of some device code\n
        /// Applicable options: PTX compiler options, ::CU_JIT_FALLBACK_STRATEGY</summary>
        Fatbinary,

        /// <summary>Host object with embedded device code\n
        /// Applicable options: PTX compiler options, ::CU_JIT_FALLBACK_STRATEGY</summary>
        Object,

        /// <summary>Archive of host objects with embedded device code\n
        /// Applicable options: PTX compiler options, ::CU_JIT_FALLBACK_STRATEGY</summary>
        Library,
        CuJitNumInputTypes
    }

    /// <summary>Online compilation targets</summary>
    public enum JitTarget
    {
        /// <summary>Compute device class 2.0</summary>
        TargetCompute20 = 20,
        /// <summary>Compute device class 2.1</summary>
        TargetCompute21 = 21,
        /// <summary>Compute device class 3.0</summary>
        TargetCompute30 = 30,
        /// <summary>Compute device class 3.2</summary>
        TargetCompute32 = 32,
        /// <summary>Compute device class 3.5</summary>
        TargetCompute35 = 35,
        /// <summary>Compute device class 3.7</summary>
        TargetCompute37 = 37,
        /// <summary>Compute device class 5.0</summary>
        TargetCompute50 = 50,
        /// <summary>Compute device class 5.2</summary>
        TargetCompute52 = 52,
        /// <summary>Compute device class 5.3</summary>
        TargetCompute53 = 53,
        /// <summary>Compute device class 6.0.*/</summary>
        TargetCompute60 = 60,
        /// <summary>Compute device class 6.1.*/</summary>
        TargetCompute61 = 61,
        /// <summary>Compute device class 6.2.*/</summary>
        TargetCompute62 = 62,
        /// <summary>Compute device class 7.0.*/</summary>
        TargetCompute70 = 70
    }

    /// <summary>Cubin matching fallback strategies</summary>
    public enum JitFallback
    {
        /// <summary>Prefer to compile ptx if exact binary match not found</summary>
        Ptx = 0,

        /// <summary>Prefer to fall back to compatible binary code if exact match not found</summary>
        Binary
    }

    /// <summary>Caching modes for dlcm</summary>
    public enum CitCacheMode
    {
        /// <summary>Compile with no -dlcm flag specified</summary>
        None = 0,
        /// <summary>Compile with L1 cache disabled</summary>
        Cg,
        /// <summary>Compile with L1 cache enabled</summary>
        Ca
    }

    public enum DeviceP2PAttribute
    {
        /// <summary>A relative value indicating the performance of the link between two devices</summary>
        PerformanceRank = 0x01,
        /// <summary>P2P Access is enable</summary>
        AccessSupported = 0x02,
        /// <summary>Atomic operation over the link supported</summary>
        NativeAtomicSupported = 0x03
    }

    [Flags]
    public enum MemHostAllocFlags
    {
        /// <summary>If set, host memory is portable between CUDA contexts.
        /// Flag for ::cuMemHostAlloc()</summary>
        Portable = 0x01,

        /// <summary>If set, host memory is mapped into CUDA address space and
        /// ::cuMemHostGetDevicePointer() may be called on the host pointer.
        /// Flag for ::cuMemHostAlloc()</summary>
        Devicemap = 0x02,

        /// <summary>If set, host memory is allocated as write-combined - fast to write,
        /// faster to DMA, slow to read except via SSE4 streaming load instruction
        /// (MOVNTDQA).
        /// Flag for ::cuMemHostAlloc()</summary>
        WriteCombined = 0x04
    }

    [Flags]
    public enum MemHostRegisterFlags
    {
        /// <summary>If set, host memory is portable between CUDA contexts.
        /// Flag for ::cuMemHostRegister()</summary>
        Portable = 0x01,

        /// <summary>If set, host memory is mapped into CUDA address space and
        /// ::cuMemHostGetDevicePointer() may be called on the host pointer.
        /// Flag for ::cuMemHostRegister()</summary>
        Devicemap = 0x02,

        /// <summary>If set, the passed memory pointer is treated as pointing to some
        /// memory-mapped I/O space, e.g. belonging to a third-party PCIe device.
        /// On Windows the flag is a no-op.
        /// On Linux that memory is marked as non cache-coherent for the GPU and
        /// is expected to be physically contiguous. It may return
        /// CUDA_ERROR_NOT_PERMITTED if run as an unprivileged user,
        /// CUDA_ERROR_NOT_SUPPORTED on older Linux kernel versions.
        /// On all other platforms, it is not supported and CUDA_ERROR_NOT_SUPPORTED
        /// is returned.
        /// Flag for ::cuMemHostRegister()</summary>
        IOMemory = 0x04
    }

    /// <summary>CUDA devices corresponding to a D3D11 device</summary>
    public enum CuD3D11DeviceList
    {
        /// <summary>The CUDA devices for all GPUs used by a D3D11 device</summary>
        All = 0x01,
        /// <summary>The CUDA devices for the GPUs used by a D3D11 device in its currently rendering frame</summary>
        CurrentFrame = 0x02,
        /// <summary>The CUDA devices for the GPUs to be used by a D3D11 device in the next frame</summary>
        NextFrame = 0x03,
    }

    /// <summary>Flags for ::cuStreamWaitValue32 and ::cuStreamWaitValue64</summary>
    public enum CuStreamWaitValue
    {
        /// <summary>Wait until (int32_t)(*addr - value) >= 0 (or int64_t for 64 bit
        /// values). Note this is a cyclic comparison which ignores wraparound.
        /// (Default behavior.)</summary>
        ValueGeq = 0x0,
        /// <summary>Wait until *addr == value.</summary>
        ValueEq = 0x1,
        /// <summary>Wait until (*addr & value) != 0.</summary>
        ValueAnd = 0x2,
        /// <summary>Wait until ~(*addr | value) != 0. Support for this operation can be
        /// queried with ::cuDeviceGetAttribute() and
        /// ::CU_DEVICE_ATTRIBUTE_CAN_USE_STREAM_WAIT_VALUE_NOR. Generally, this
        /// requires compute capability 7.0 or greater.</summary>
        ValueNor = 0x3,
        /// <summary>Follow the wait operation with a flush of outstanding remote writes. This
        /// means that, if a remote write operation is guaranteed to have reached the
        /// device before the wait can be satisfied, that write is guaranteed to be
        /// visible to downstream device work. The device is permitted to reorder
        /// remote writes internally. For example, this flag would be required if
        /// two remote writes arrive in a defined order, the wait is satisfied by the
        /// second write, and downstream work needs to observe the first write.</summary>
        ValueFlush = 1 << 30
    }

    /// <summary>Flags for ::cuStreamWriteValue32</summary>
    public enum CuStreamWriteValue
    {
        /// <summary>Default behavior</summary>
        Default = 0x0,
        /// <summary>Permits the write to be reordered with writes which were issued
        /// before it, as a performance optimization. Normally,
        /// ::cuStreamWriteValue32 will provide a memory fence before the
        /// write, which has similar semantics to
        /// __threadfence_system() but is scoped to the stream
        /// rather than a CUDA thread.</summary>
        NoMemoryBarrier = 0x1
    }

    /// <summary>Operations for ::cuStreamBatchMemOp</summary>
    public enum CuStreamBatchMemOpType
    {
        /// <summary>Represents a ::cuStreamWaitValue32 operation</summary>
        WaitValue32 = 1,
        /// <summary>Represents a ::cuStreamWriteValue32 operation</summary>
        WriteValue32 = 2,
        /// <summary>Represents a ::cuStreamWaitValue64 operation</summary>
        WaitValue64 = 4,
        /// <summary>Represents a ::cuStreamWriteValue64 operation</summary>
        WriteValue64 = 5,
        /// <summary>This has the same effect as ::CU_STREAM_WAIT_VALUE_FLUSH, but as a standalone operation.</summary>
        FlushRemoteWrites = 3
    }

    /// <summary>Resource view format</summary>
    public enum CuResourceViewFormat
    {
        /// <summary>No resource view format (use underlying resource format)</summary>
        None = 0x00,
        /// <summary>1 channel unsigned 8-bit integers</summary>
        Uint1X8 = 0x01,
        /// <summary>2 channel unsigned 8-bit integers</summary>
        Uint2X8 = 0x02,
        /// <summary>4 channel unsigned 8-bit integers</summary>
        Uint4X8 = 0x03,
        /// <summary>1 channel signed 8-bit integers</summary>
        Sint1X8 = 0x04,
        /// <summary>2 channel signed 8-bit integers</summary>
        Sint2X8 = 0x05,
        /// <summary>4 channel signed 8-bit integers</summary>
        Sint4X8 = 0x06,
        /// <summary>1 channel unsigned 16-bit integers</summary>
        Uint1X16 = 0x07,
        /// <summary>2 channel unsigned 16-bit integers</summary>
        Uint2X16 = 0x08,
        /// <summary>4 channel unsigned 16-bit integers</summary>
        Uint4X16 = 0x09,
        /// <summary>1 channel signed 16-bit integers</summary>
        Sint1X16 = 0x0a,
        /// <summary>2 channel signed 16-bit integers</summary>
        Sint2X16 = 0x0b,
        /// <summary>4 channel signed 16-bit integers</summary>
        Sint4X16 = 0x0c,
        /// <summary>1 channel unsigned 32-bit integers</summary>
        Uint1X32 = 0x0d,
        /// <summary>2 channel unsigned 32-bit integers</summary>
        Uint2X32 = 0x0e,
        /// <summary>4 channel unsigned 32-bit integers</summary>
        Uint4X32 = 0x0f,
        /// <summary>1 channel signed 32-bit integers</summary>
        Sint1X32 = 0x10,
        /// <summary>2 channel signed 32-bit integers</summary>
        Sint2X32 = 0x11,
        /// <summary>4 channel signed 32-bit integers</summary>
        Sint4X32 = 0x12,
        /// <summary>1 channel 16-bit floating point</summary>
        Float1X16 = 0x13,
        /// <summary>2 channel 16-bit floating point</summary>
        Float2X16 = 0x14,
        /// <summary>4 channel 16-bit floating point</summary>
        Float4X16 = 0x15,
        /// <summary>1 channel 32-bit floating point</summary>
        Float1X32 = 0x16,
        /// <summary>2 channel 32-bit floating point</summary>
        Float2X32 = 0x17,
        /// <summary>4 channel 32-bit floating point</summary>
        Float4X32 = 0x18,
        /// <summary>Block compressed 1</summary>
        UnsignedBc1 = 0x19,
        /// <summary>Block compressed 2</summary>
        UnsignedBc2 = 0x1a,
        /// <summary>Block compressed 3</summary>
        UnsignedBc3 = 0x1b,
        /// <summary>Block compressed 4 unsigned</summary>
        UnsignedBc4 = 0x1c,
        /// <summary>Block compressed 4 signed</summary>
        SignedBc4 = 0x1d,
        /// <summary>Block compressed 5 unsigned</summary>
        UnsignedBc5 = 0x1e,
        /// <summary>Block compressed 5 signed</summary>
        SignedBc5 = 0x1f,
        /// <summary>Block compressed 6 unsigned half-float</summary>
        UnsignedBc6H = 0x20,
        /// <summary>Block compressed 6 signed half-float</summary>
        SignedBc6H = 0x21,
        /// <summary>Block compressed 7</summary>
        UnsignedBc7 = 0x22
    }

    [Flags]
    public enum CuTrsfFlags
    {
        /// <summary>Read the texture as integers rather than promoting the values to floats
        /// in the range [0,1].</summary>
        ReadAsInteger = 0x01,

        /// <summary>Use normalized texture coordinates in the range [0,1) instead of [0,dim).</summary>
        NormalizedCoordinates = 0x02,

        /// <summary>Perform sRGB->linear conversion during texture read.</summary>
        Srgb = 0x10
    }

    public enum CuCooperativeLaunchMultiDevice
    {
        /// <summary>If set, each kernel launched as part of ::cuLaunchCooperativeKernelMultiDevice only
        /// waits for prior work in the stream corresponding to that GPU to complete before the
        /// kernel begins execution.</summary>
        NoPreLaunchSync = 0x01,

        /// <summary>If set, any subsequent work pushed in a stream that participated in a call to
        /// ::cuLaunchCooperativeKernelMultiDevice will only wait for the kernel launched on
        /// the GPU corresponding to that stream to complete before it begins execution.</summary>
        NoPostLaunchSync = 0x02
    }

    [Flags]
    public enum CuArray3DFlags
    {
        /// <summary>CUDA_ARRAY3D_LAYERED
        /// If set, the CUDA array is a collection of layers, where each layer is either a 1D
        /// or a 2D array and the Depth member of DESCRIPTOR specifies the number
        /// of layers, not the depth of a 3D array.</summary>
        Layered = 0x01,

        /// <summary>CUDA_ARRAY3D_2DARRAY
        /// Deprecated, use LAYERED</summary>
        Array2D = 0x01,

        /// <summary>CUDA_ARRAY3D_SURFACE_LDST
        /// This flag must be set in order to bind a surface reference
        /// to the CUDA array</summary>
        SurfaceLdst = 0x02,

        /// <summary>CUDA_ARRAY3D_CUBEMAP
        /// If set, the CUDA array is a collection of six 2D arrays, representing faces of a cube. The
        /// width of such a CUDA array must be equal to its height, and Depth must be six.
        /// If ::LAYERED flag is also set, then the CUDA array is a collection of cubemaps
        /// and Depth must be a multiple of six.</summary>
        Cubemap = 0x04,

        /// <summary>CUDA_ARRAY3D_TEXTURE_GATHER
        /// This flag must be set in order to perform texture gather operations
        /// on a CUDA array.</summary>
        TextureGather = 0x08,

        /// <summary>CUDA_ARRAY3D_DEPTH_TEXTURE
        /// This flag if set indicates that the CUDA
        /// array is a DEPTH_TEXTURE.</summary>
        DepthTexture = 0x10
    }

    public enum CuLaunchParam
    {
        /// <summary>CU_LAUNCH_PARAM_END
        /// End of array terminator for the <c>extra</c> parameter to
        /// ::cuLaunchKernel</summary>
        ParamEnd = 0x00,

        /// <summary>CU_LAUNCH_PARAM_BUFFER_POINTER
        /// Indicator that the next value in the <c>extra</c> parameter to
        /// ::cuLaunchKernel will be a pointer to a buffer containing all kernel
        /// parameters used for launching kernel <c>f</c>.  This buffer needs to
        /// honor all alignment/padding requirements of the individual parameters.
        /// If ::CU_LAUNCH_PARAM_BUFFER_SIZE is not also specified in the
        /// <c>extra</c> array, then ::CU_LAUNCH_PARAM_BUFFER_POINTER will have no
        /// effect.</summary>
        BufferPointer = 0x01,

        /// <summary>CU_LAUNCH_PARAM_BUFFER_SIZE
        /// Indicator that the next value in the <c>extra</c> parameter to
        /// ::cuLaunchKernel will be a pointer to a size_t which contains the
        /// size of the buffer specified with ::CU_LAUNCH_PARAM_BUFFER_POINTER.
        /// It is required that ::CU_LAUNCH_PARAM_BUFFER_POINTER also be specified
        /// in the <c>extra</c> array if the value associated with
        /// ::CU_LAUNCH_PARAM_BUFFER_SIZE is not zero.</summary>
        BufferSize = 0x02
    }
}
