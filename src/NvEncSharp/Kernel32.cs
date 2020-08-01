using System.Runtime.InteropServices;

namespace Lennox.NvEncSharp
{
    internal static class Kernel32
    {
        [DllImport("kernel32.dll", EntryPoint = "RtlZeroMemory")]
        public unsafe static extern bool ZeroMemory(byte* destination, int length);
    }
}
