using System;
using System.Runtime.InteropServices;

namespace Lennox.NvEncSharp
{
    public static class LibCudaLibrary
    {
        private const string _dll = "NvEncSharp.Cuda.Library.dll";

        [DllImport(_dll, SetLastError = true)]
        public static extern void Nv12ToBGRA32(
            IntPtr nv12, int nv12Pitch,
            IntPtr dest, int destPitch, int width, int height, int matrix = 0);

        [DllImport(_dll, SetLastError = true)]
        public static extern void Nv12ToRGBA32(
            IntPtr nv12, int nv12Pitch,
            IntPtr dest, int destPitch, int width, int height, int matrix = 0);

        [DllImport(_dll, SetLastError = true)]
        public static extern void Nv12ToARGB32(
            IntPtr nv12, int nv12Pitch,
            IntPtr dest, int destPitch, int width, int height, int matrix = 0);

        [DllImport(_dll, SetLastError = true)]
        public static extern void Nv12ToRGB24(
            IntPtr nv12, int nv12Pitch,
            IntPtr dest, int destPitch, int width, int height, int matrix = 0);
    }
}