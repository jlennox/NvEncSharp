#if NET8_0_OR_GREATER
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Lennox.NvEncSharp
{
    internal static class NativeLibraries
    {
        // Register before any P/Invoke, including calls to the many partial CUDA classes.
#pragma warning disable CA2255
        [ModuleInitializer]
#pragma warning restore CA2255
        internal static void Initialize()
        {
            // Keep the existing Windows imports and let the runtime resolve RID assets.
            if (OperatingSystem.IsLinux())
                NativeLibrary.SetDllImportResolver(typeof(NativeLibraries).Assembly, Resolve);
        }

        private static IntPtr Resolve(string name, Assembly assembly, DllImportSearchPath? searchPath)
        {
            if (!OperatingSystem.IsLinux()) return IntPtr.Zero;
            if (RuntimeInformation.ProcessArchitecture != Architecture.X64)
                throw new PlatformNotSupportedException("Linux support currently requires an x64 process.");

            var linuxName = name switch
            {
                "nvcuda.dll" => "libcuda.so.1",
                "nvcuvid.dll" => "libnvcuvid.so.1",
                "nvEncodeAPI64.dll" => "libnvidia-encode.so.1",
                "NvEncSharp.Cuda.Library.dll" => "libNvEncSharp.Cuda.Library.so",
                _ => null
            };
            return linuxName == null ? IntPtr.Zero : NativeLibrary.Load(linuxName, assembly, searchPath);
        }
    }
}
#endif
