using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static Lennox.NvEncSharp.LibCuda;

namespace Lennox.NvEncSharp
{
    public readonly struct CuDeviceDescription
    {
        public readonly CuDevice Device;
        public readonly string Name;
        public readonly long TotalMemory;

        public int Handle => Device.Handle;

        public CuDeviceDescription(CuDevice device)
        {
            Device = device;
            Name = device.GetName();
            TotalMemory = device.GetTotalMemory();
        }

        /// <inheritdoc cref="LibCuda.DeviceGetAttribute(out int, CuDeviceAttribute, CuDevice)"/>
        public int GetAttribute(CuDeviceAttribute attribute) => Device.GetAttribute(attribute);
    }

    public unsafe partial struct CuDevice
    {
        /// <inheritdoc cref="LibCuda.DeviceGet(out CuDevice, int)"/>
        public static CuDevice GetDevice(int ordinal)
        {
            var result = DeviceGet(out var device, ordinal);
            CheckResult(result);

            return device;
        }

        /// <inheritdoc cref="LibCuda.DeviceGetCount(out int)"/>
        public static int GetCount()
        {
            var result = DeviceGetCount(out var count);
            CheckResult(result);

            return count;
        }

        public static IEnumerable<CuDeviceDescription> GetDescriptions()
        {
            var count = GetCount();

            for (var i = 0; i < count; ++i)
            {
                yield return new CuDeviceDescription(new CuDevice(i));
            }
        }

        /// <inheritdoc cref="LibCuda.D3D11GetDevice(out CuDevice, IntPtr)"/>
        public static CuDevice GetD3D11Device(IntPtr adapter)
        {
            var result = D3D11GetDevice(out var device, adapter);
            CheckResult(result);

            return device;
        }

        /// <inheritdoc cref="LibCuda.DeviceGetName(byte*, int, CuDevice)"/>
        public string GetName()
        {
            const int inputLength = 256;
            var name = stackalloc byte[inputLength];
            var result = DeviceGetName(name, inputLength, this);
            CheckResult(result);
            return Marshal.PtrToStringAnsi((IntPtr)name, inputLength);
        }

        /// <inheritdoc cref="LibCuda.DeviceTotalMemory(out IntPtr, CuDevice)"/>
        public long GetTotalMemory()
        {
            var result = DeviceTotalMemory(out var memorySize, this);
            CheckResult(result);

            return memorySize.ToInt64();
        }

        public CuDeviceDescription GetDescription()
        {
            return new CuDeviceDescription(this);
        }

        /// <inheritdoc cref="LibCuda.DeviceGetAttribute(out int, CuDeviceAttribute, CuDevice)"/>
        public int GetAttribute(CuDeviceAttribute attribute)
        {
            var result = DeviceGetAttribute(
                out var output, attribute, this);
            CheckResult(result);

            return output;
        }

        /// <inheritdoc cref="LibCuda.CtxCreate(out CuContext, CuContextFlags, CuDevice)"/>
        public CuContext CreateContext(
            CuContextFlags flags = CuContextFlags.SchedAuto)
        {
            var result = CtxCreate(out var ctx, flags, this);
            CheckResult(result);

            return ctx;
        }
    }
}