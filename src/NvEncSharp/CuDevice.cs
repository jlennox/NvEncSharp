using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

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
            var result = LibCuda.DeviceGet(out var device, ordinal);
            LibNvVideo.CheckResult(result);

            return device;
        }

        public static int GetCount()
        {
            var result = LibCuda.DeviceGetCount(out var count);
            LibNvVideo.CheckResult(result);

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
            var result = LibCuda.DeviceGetName(name, inputLength, this);
            LibNvVideo.CheckResult(result);
            return Marshal.PtrToStringAnsi((IntPtr)name, inputLength);
        }

        public long GetTotalMemory()
        {
            var result = LibCuda.DeviceTotalMemory(out var memorySize, this);
            LibNvVideo.CheckResult(result);

            return memorySize.ToInt64();
        }

        public CuDeviceDescription GetDescription()
        {
            return new CuDeviceDescription(this);
        }

        public int GetAttribute(CuDeviceAttribute attribute)
        {
            var result = LibCuda.DeviceGetAttribute(
                out var output, attribute, this);
            LibNvVideo.CheckResult(result);

            return output;
        }

        public CuContext CreateContext(
            CuContextFlags flags = CuContextFlags.SchedAuto)
        {
            return LibCuda.CtxCreate(this, flags);
        }
    }
}