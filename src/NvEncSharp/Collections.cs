using System;

namespace Lennox.NvEncSharp
{
    internal static class Collections
    {
        public static unsafe void ZeroMemory(byte* destination, int length)
        {
            new Span<byte>(destination, length).Clear();
        }
    }
}
