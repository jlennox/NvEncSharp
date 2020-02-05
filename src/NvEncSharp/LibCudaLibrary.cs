using System;
using System.Runtime.InteropServices;

// ReSharper disable UnusedMember.Global

namespace Lennox.NvEncSharp
{
    /// <summary>
    /// Please note that the byte ordering is reverse how most libraries
    /// consider them. BGRA32 returns the byte order \xAA \xRR \xGG \xBB.
    /// </summary>
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

        [DllImport(_dll, SetLastError = true)]
        public static extern void ResizeNv12(
            CuDevicePtr dstNv12, int dstPitch, int dstWidth, int dstHeight,
            CuDevicePtr srcNv12, int srcPitch, int srcWidth, int srcHeight,
            CuDevicePtr dstNv12UV);
    }
}