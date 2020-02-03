using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using static Lennox.NvEncSharp.LibCuda;
using static Lennox.NvEncSharp.LibCuVideo;

// ReSharper disable PureAttributeOnVoidMethod
// ReSharper disable UnusedMember.Global

namespace Lennox.NvEncSharp
{
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{" + nameof(Handle) + "}")]
    public struct CuContext : IDisposable
    {
        public static readonly CuContext Empty = new CuContext { Handle = IntPtr.Zero };
        public IntPtr Handle;
        public bool IsEmpty => Handle == IntPtr.Zero;

        public struct CuContextPush : IDisposable
        {
            private CuContext _context;
            private int _disposed;

            internal CuContextPush(CuContext context)
            {
                _context = context;
                _disposed = 0;
            }

            /// <inheritdoc cref="CtxPopCurrent(out CuContext)"/>
            public void Dispose()
            {
                var disposed = Interlocked.Exchange(ref _disposed, 1);
                if (disposed != 0) return;

                CtxPopCurrent(out _);
            }
        }

        /// <inheritdoc cref="CtxLockCreate(out CuVideoContextLock, CuContext)"/>
        public CuVideoContextLock CreateLock()
        {
            var result = CtxLockCreate(out var lok, this);
            CheckResult(result);

            return lok;
        }

        /// <inheritdoc cref="CtxPushCurrent(CuContext)"/>
        public CuContextPush Push()
        {
            var result = CtxPushCurrent(this);
            CheckResult(result);

            return new CuContextPush(this);
        }

        /// <inheritdoc cref="CtxSetCurrent(CuContext)"/>
        public void SetCurrent()
        {
            var result = CtxSetCurrent(this);
            CheckResult(result);
        }

        /// <inheritdoc cref="CtxGetApiVersion(CuContext, out uint)"/>
        public uint GetApiVersion()
        {
            var result = CtxGetApiVersion(this, out var version);
            CheckResult(result);

            return version;
        }

        /// <inheritdoc cref="CtxGetDevice(out CuDevice)"/>
        public CuDevice GetDevice()
        {
            using var _ = Push();
            var result = CtxGetDevice(out var device);
            CheckResult(result);

            return device;
        }

        /// <inheritdoc cref="CtxGetCurrent(out CuContext)"/>
        public static CuContext GetCurrent()
        {
            var result = CtxGetCurrent(out var ctx);
            CheckResult(result);

            return ctx;
        }

        /// <inheritdoc cref="CtxGetSharedMemConfig(out CuSharedConfig)"/>
        public static CuSharedConfig GetSharedMemConfig()
        {
            var result = CtxGetSharedMemConfig(out var config);
            CheckResult(result);

            return config;
        }

        /// <inheritdoc cref="CtxSetSharedMemConfig(CuSharedConfig)"/>
        public static void SetSharedMemConfig(CuSharedConfig config)
        {
            var result = CtxSetSharedMemConfig(config);
            CheckResult(result);
        }

        /// <inheritdoc cref="CtxGetCacheConfig(out CuFunctionCache)"/>
        public static CuFunctionCache GetCacheConfig()
        {
            var result = CtxGetCacheConfig(out var config);
            CheckResult(result);

            return config;
        }

        /// <inheritdoc cref="CtxSetCacheConfig(CuFunctionCache)"/>
        public static void SetCacheConfig(CuFunctionCache config)
        {
            var result = CtxSetCacheConfig(config);
            CheckResult(result);
        }

        /// <inheritdoc cref="CtxGetDevice(out CuDevice)"/>
        public static CuDevice GetCurrentDevice()
        {
            var result = CtxGetDevice(out var device);
            CheckResult(result);

            return device;
        }

        /// <inheritdoc cref="CtxDestroy(CuContext)"/>
        public void Dispose()
        {
            var handle = Interlocked.Exchange(ref Handle, IntPtr.Zero);
            if (handle == IntPtr.Zero) return;
            var obj = new CuContext { Handle = handle };

            CtxDestroy(obj);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{" + nameof(Handle) + "}")]
    public struct CuGraphicsResource : IDisposable
    {
        public static readonly CuGraphicsResource Empty = new CuGraphicsResource { Handle = IntPtr.Zero };
        public IntPtr Handle;
        public bool IsEmpty => Handle == IntPtr.Zero;

        /// <inheritdoc cref="GraphicsD3D11RegisterResource(out CuGraphicsResource, IntPtr, CuGraphicsRegisters)"/>
        public static CuGraphicsResource Register(
            IntPtr resourcePtr,
            CuGraphicsRegisters flags = CuGraphicsRegisters.None)
        {
            var result = GraphicsD3D11RegisterResource(
                out var resource, resourcePtr, flags);
            CheckResult(result);

            return resource;
        }

        /// <inheritdoc cref="GraphicsResourceSetMapFlags(CuGraphicsResource, CuGraphicsMapResources)"/>
        public void SetMapFlags(CuGraphicsMapResources flags)
        {
            var result = GraphicsResourceSetMapFlags(this, flags);
            CheckResult(result);
        }

        /// <inheritdoc cref="GraphicsMapResources(int, CuGraphicsResource*, CuStream)"/>
        public CuGraphicsMappedResource Map()
        {
            return Map(CuStream.Empty);
        }

        /// <inheritdoc cref="GraphicsMapResources(int, CuGraphicsResource*, CuStream)"/>
        public unsafe CuGraphicsMappedResource Map(CuStream stream)
        {
            var copy = this;
            var result = GraphicsMapResources(1, &copy, stream);
            CheckResult(result);

            return new CuGraphicsMappedResource(this, stream);
        }

        public unsafe struct CuGraphicsMappedResource : IDisposable
        {
            private readonly CuGraphicsResource _resource;
            private readonly CuStream _stream;

            public CuGraphicsMappedResource(
                CuGraphicsResource resource,
                CuStream stream)
            {
                _resource = resource;
                _stream = stream;
            }

            public void Dispose()
            {
                var copy = _resource;
                GraphicsUnmapResources(1, &copy, _stream);
            }
        }

        /// <inheritdoc cref="GraphicsSubResourceGetMappedArray(out CuArray, CuGraphicsResource, int, int)"/>
        public CuArray GetMappedArray(
            int arrayIndex = 0,
            int mipLevel = 0)
        {
            var result = GraphicsSubResourceGetMappedArray(
                out var array,
                this, arrayIndex, mipLevel);
            CheckResult(result);

            return array;
        }

        /// <inheritdoc cref="GraphicsUnregisterResource(CuGraphicsResource)"/>
        public void Dispose()
        {
            var handle = Interlocked.Exchange(ref Handle, IntPtr.Zero);
            if (handle == IntPtr.Zero) return;
            var obj = new CuGraphicsResource { Handle = handle };

            GraphicsUnregisterResource(obj);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{" + nameof(Handle) + "}")]
    public struct CuMipMappedArray
    {
        public static readonly CuMipMappedArray Empty = new CuMipMappedArray { Handle = IntPtr.Zero };
        public IntPtr Handle;
        public bool IsEmpty => Handle == IntPtr.Zero;
    }

    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{" + nameof(Handle) + "}")]
    public struct CuModule
    {
        public static readonly CuModule Empty = new CuModule { Handle = IntPtr.Zero };
        public IntPtr Handle;
        public bool IsEmpty => Handle == IntPtr.Zero;
    }

    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{" + nameof(Handle) + "}")]
    public struct CuLinkState
    {
        public static readonly CuLinkState Empty = new CuLinkState { Handle = IntPtr.Zero };
        public IntPtr Handle;
        public bool IsEmpty => Handle == IntPtr.Zero;
    }

    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{" + nameof(Handle) + "}")]
    public struct CuSurfRef
    {
        public static readonly CuSurfRef Empty = new CuSurfRef { Handle = IntPtr.Zero };
        public IntPtr Handle;
        public bool IsEmpty => Handle == IntPtr.Zero;
    }

    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{" + nameof(Handle) + "}")]
    public struct CuFunction
    {
        public static readonly CuFunction Empty = new CuFunction { Handle = IntPtr.Zero };
        public IntPtr Handle;
        public bool IsEmpty => Handle == IntPtr.Zero;
    }

    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{" + nameof(Handle) + "}")]
    public struct CuTextRef
    {
        public static readonly CuTextRef Empty = new CuTextRef { Handle = IntPtr.Zero };
        public IntPtr Handle;
        public bool IsEmpty => Handle == IntPtr.Zero;
    }

    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{" + nameof(Handle) + "}")]
    public struct CuIpcEventHandle
    {
        public static readonly CuIpcEventHandle Empty = new CuIpcEventHandle { Handle = IntPtr.Zero };
        public IntPtr Handle;
        public bool IsEmpty => Handle == IntPtr.Zero;
    }

    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{" + nameof(Handle) + "}")]
    public struct CuIpcMemHandle
    {
        public static readonly CuIpcMemHandle Empty = new CuIpcMemHandle { Handle = IntPtr.Zero };
        public IntPtr Handle;
        public bool IsEmpty => Handle == IntPtr.Zero;
    }

    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{" + nameof(Handle) + "}")]
    public struct CuHostMemory : IDisposable
    {
        public static readonly CuHostMemory Empty = new CuHostMemory { Handle = IntPtr.Zero };
        public IntPtr Handle;
        public bool IsEmpty => Handle == IntPtr.Zero;

        /// <inheritdoc cref="MemAllocHost(out CuHostMemory, IntPtr)"/>
        public static CuHostMemory Allocate(long bytesize)
        {
            CheckResult(MemAllocHost(out var mem, (IntPtr)bytesize));
            return mem;
        }

        // TODO: Move?
        /// <inheritdoc cref="MemAllocManaged(out CuDevicePtr, IntPtr, CuMemAttachFlags)"/>
        public static CuDevicePtr AllocateManaged(
            long bytesize, CuMemAttachFlags flags)
        {
            CheckResult(MemAllocManaged(out var mem, (IntPtr)bytesize, flags));
            return mem;
        }

        /// <inheritdoc cref="MemFreeHost(CuHostMemory)"/>
        public void Dispose()
        {
            var handle = Interlocked.Exchange(ref Handle, IntPtr.Zero);
            if (handle == IntPtr.Zero) return;
            var obj = new CuHostMemory { Handle = handle };

            MemFreeHost(obj);
        }
    }
}
