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

        public int GetAttribute(CuDeviceAttribute attribute) => Device.GetAttribute(attribute);
    }

    public unsafe partial struct CuDevice
    {
        public static CuDevice GetDevice(int ordinal)
        {
            var result = DeviceGet(out var device, ordinal);
            CheckResult(result);

            return device;
        }

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

        public string GetName()
        {
            const int inputLength = 256;
            var name = stackalloc byte[inputLength];
            var result = DeviceGetName(name, inputLength, this);
            CheckResult(result);
            return Marshal.PtrToStringAnsi((IntPtr)name, inputLength);
        }

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

        public int GetAttribute(CuDeviceAttribute attribute)
        {
            var result = DeviceGetAttribute(
                out var output, attribute, this);
            CheckResult(result);

            return output;
        }

        public CuContext CreateContext(
            CuContextFlags flags = CuContextFlags.SchedAuto)
        {
            var result = CtxCreate(out var ctx, flags, this);
            CheckResult(result);

            return ctx;
        }
    }
}