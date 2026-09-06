using System;
using System.IO;

#nullable enable

namespace Lennox.NvEncSharp.Sample.VideoDecode
{
    // Sorry, this is sample code, and it's not good without a buffering reader.
    // Use a FileStream, MemoryStream, or BufferedStream.
    internal static class Av1ObuReader
    {
        // Match the sample's existing maximum buffered NAL size.
        private const int MaxPayloadSize = 50 * 1024 * 1024;

        public static byte[]? ReadNext(Stream input)
        {
            var header = input.ReadByte();
            if (header < 0) return null;

            // Low-overhead OBUs must carry their own size. Neither IVF nor
            // AV1 Annex B framing is accepted by this reader.
            if ((header & 0x81) != 0 || (header & 2) == 0)
            {
                throw new InvalidDataException("Expected a size-delimited AV1 OBU (raw .obu input).");
            }

            // One header byte, one optional extension byte and up to eight size bytes.
            Span<byte> prefix = stackalloc byte[10];
            var prefixLength = 0;
            prefix[prefixLength++] = (byte)header;
            if ((header & 4) != 0)
            {
                var extension = ReadByte(input);
                if ((extension & 7) != 0)
                {
                    throw new InvalidDataException("Invalid AV1 OBU extension header.");
                }
                prefix[prefixLength++] = extension;
            }

            ulong size = 0;
            for (var i = 0; ; ++i)
            {
                var value = ReadByte(input);
                prefix[prefixLength++] = value;
                size |= (ulong)(value & 0x7f) << (i * 7);
                if ((value & 0x80) == 0) break;
                if (i == 7)
                {
                    throw new InvalidDataException("Invalid AV1 OBU size.");
                }
            }

            if (size > MaxPayloadSize)
            {
                throw new InvalidDataException("AV1 OBU exceeds the sample's 50 MiB limit.");
            }

            var packet = new byte[prefixLength + (int)size];
            prefix.Slice(0, prefixLength).CopyTo(packet);
            var offset = prefixLength;
            while (offset < packet.Length)
            {
                var count = input.Read(packet, offset, packet.Length - offset);
                if (count == 0) throw new EndOfStreamException("Truncated AV1 OBU payload.");
                offset += count;
            }

            return packet;
        }

        private static byte ReadByte(Stream input)
        {
            var value = input.ReadByte();
            if (value < 0) throw new EndOfStreamException("Truncated AV1 OBU header.");
            return (byte)value;
        }
    }
}
