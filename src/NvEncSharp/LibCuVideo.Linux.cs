#if NET8_0_OR_GREATER
using System.Runtime.InteropServices;

namespace Lennox.NvEncSharp
{
    public unsafe partial class LibCuVideo
    {
        [DllImport(_dllpath, EntryPoint = "cuvidParseVideoData")]
        private static extern CuResult ParseVideoDataLinux(CuVideoParser parser, ref LinuxSourceDataPacket packet);

        [StructLayout(LayoutKind.Sequential)]
        internal struct LinuxSourceDataPacket
        {
            public ulong Flags, PayloadSize;
            public byte* Payload;
            public long Timestamp;

            internal LinuxSourceDataPacket(CuVideoSourceDataPacket packet)
            {
                Flags = unchecked((uint)packet.Flags);
                PayloadSize = packet.PayloadSize;
                Payload = packet.Payload;
                Timestamp = packet.Timestamp;
            }
        }

        [DllImport(_dllpath, EntryPoint = "cuvidCreateDecoder")]
        private static extern CuResult CreateDecoderLinux(out CuVideoDecoder decoder, ref LinuxDecodeCreateInfo info);

        // CUVIDDECODECREATEINFO uses unsigned long: Linux x64 is LP64, Windows is LLP64.
        // Translate at the call boundary to preserve the public Windows-compatible structure.
        [StructLayout(LayoutKind.Sequential)]
        internal struct LinuxDecodeCreateInfo
        {
            public ulong Width, Height, NumDecodeSurfaces;
            public CuVideoCodec CodecType;
            public CuVideoChromaFormat ChromaFormat;
            public ulong CreationFlags, BitDepthMinus8, IntraDecodeOnly, MaxWidth, MaxHeight;
            private ulong _reserved1;
            public CuRectangleShort DisplayArea;
            public CuVideoSurfaceFormat OutputFormat;
            public CuVideoDeinterlaceMode DeinterlaceMode;
            public ulong TargetWidth, TargetHeight, NumOutputSurfaces;
            public CuVideoContextLock VideoLock;
            public CuRectangleShort TargetRect;
            private fixed ulong _reserved2[5];

            internal LinuxDecodeCreateInfo(CuVideoDecodeCreateInfo info)
            {
                this = default;
                Width = unchecked((uint)info.Width);
                Height = unchecked((uint)info.Height);
                NumDecodeSurfaces = unchecked((uint)info.NumDecodeSurfaces);
                CodecType = info.CodecType;
                ChromaFormat = info.ChromaFormat;
                CreationFlags = unchecked((uint)info.CreationFlags);
                BitDepthMinus8 = unchecked((uint)info.BitDepthMinus8);
                IntraDecodeOnly = unchecked((uint)info.IntraDecodeOnly);
                MaxWidth = unchecked((uint)info.MaxWidth);
                MaxHeight = unchecked((uint)info.MaxHeight);
                DisplayArea = info.DisplayArea;
                OutputFormat = info.OutputFormat;
                DeinterlaceMode = info.DeinterlaceMode;
                TargetWidth = unchecked((uint)info.TargetWidth);
                TargetHeight = unchecked((uint)info.TargetHeight);
                NumOutputSurfaces = unchecked((uint)info.NumOutputSurfaces);
                VideoLock = info.VideoLock;
                TargetRect = info.TargetRect;
            }
        }
    }
}
#endif
