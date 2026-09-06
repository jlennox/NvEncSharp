using System;

namespace Lennox.NvEncSharp
{
    public static class CuVideoCodecExtensions
    {
        extension(CuVideoCodec)
        {
            public static CuVideoCodec FromName(string name)
            {
                if (name == null) throw new ArgumentNullException(nameof(name));

                return name.ToLowerInvariant() switch {
                    "h264" or "264" => CuVideoCodec.H264,
                    "hevc" or "h265" or "265" => CuVideoCodec.HEVC,
                    "av1" => CuVideoCodec.AV1,
                    _ => throw new ArgumentException("Supported codecs: h264, hevc, av1.", nameof(name))
                };
            }
        }
    }
}