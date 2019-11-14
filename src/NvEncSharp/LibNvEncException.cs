using System;

namespace Lennox.NvEncSharp
{
    public class LibNvEncException : Exception
    {
        public LibNvEncException(string callerName, string description, NvEncStatus status) : base($"{callerName} returned invalid status: {status}, {description}") { }
    }
}