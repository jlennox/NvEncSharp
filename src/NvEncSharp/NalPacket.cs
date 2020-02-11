using System;

// ReSharper disable UnusedMember.Global

namespace Lennox.NvEncSharp
{
    public enum NalPacketType
    {
        Unknown = 0,
        /// <summary>non-IDR non-data partition</summary>
        Slice = 1,
        /// <summary>data partition A</summary>
        DataPartitionA = 2,
        /// <summary>data partition B</summary>
        DataPartitionB = 3,
        /// <summary>data partition C</summary>
        DataPartitionC = 4,
        /// <summary>IDR NAL</summary>
        Idr = 5,
        /// <summary>supplemental enhancement info</summary>
        SupplentalEnhancementInfo = 6,
        /// <summary>sequence parameter set</summary>
        SequenceParameterSet = 7,
        /// <summary>picture parameter set</summary>
        PictureParameterSet = 8,
        /// <summary>access unit delimiter</summary>
        AccessUnitDelimiter = 9,
        /// <summary>end of sequence</summary>
        EndOfSequence = 10,
        /// <summary>end of stream</summary>
        EndOfStream = 11,
        /// <summary>filler data</summary>
        Fill = 12
    }

    /// <summary>
    /// Performs basic operations on binary representations of NAL packets.
    /// </summary>
    public ref struct NalPacket
    {
        public Span<byte> Packet;
        public Span<byte> PacketPrefix;
        public NalPacketType PacketType;
        public bool Complete;

        private static readonly byte[] _nalSignature = { 0, 0, 1 };

        /// <summary>
        /// Finds the index of the first NAL signature in <paramref name="input"/>.
        /// </summary>
        /// <param name="input">The bytes to search.</param>
        /// <param name="length">The length of the signature. \x00\x00\x01 for
        /// example sets this value to 3.</param>
        ///
        /// <returns>The index that includes the NAL signature.
        /// -1 is returned if no signature is found.</returns>
        public static int IndexOfSignature(Span<byte> input, out int length)
        {
            length = 3;
            var index = input.IndexOf(_nalSignature);
            if (index == -1) return -1;

            for (var i = index - 1; i >= 0; --i)
            {
                if (input[i] != 0) return i + 1;
                ++length;
            }

            return 0;
        }

        /// <summary>
        /// Get the bytes of the next NAL packet in <paramref name="input"/>.
        /// </summary>
        /// <param name="input">The bytes to search. The stream is updated to
        /// be the memory after the end of the packet.</param>
        public static NalPacket ReadNextPacket(ref Span<byte> input)
        {
            var startIndex = IndexOfSignature(input, out var startLength);

            if (startIndex == -1)
            {
                var oldInput = input;
                input = Span<byte>.Empty;

                return new NalPacket
                {
                    Packet = oldInput,
                    PacketPrefix = Span<byte>.Empty,
                    PacketType = NalPacketType.Unknown,
                    Complete = false
                };
            }

            var endSlice = input.Slice(startIndex + startLength);
            var endIndex = IndexOfSignature(endSlice, out _);

            var complete = endIndex != -1;
            endIndex = complete
                ? endIndex + startIndex + startLength
                : input.Length;

            var packet = input.Slice(startIndex, endIndex - startIndex);
            input = input.Slice(endIndex - startIndex);

            return new NalPacket
            {
                Packet = packet,
                PacketPrefix = startIndex > 0
                    ? input.Slice(0, startIndex)
                    : Span<byte>.Empty,
                PacketType = startLength < packet.Length
                    ? GetNalPacketType(packet[startLength])
                    : NalPacketType.Unknown,
                Complete = complete
            };
        }

        private static NalPacketType GetNalPacketType(byte firstByte)
        {
            return (NalPacketType)(firstByte & 0x1F);
        }
    }
}