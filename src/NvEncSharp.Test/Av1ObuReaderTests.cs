using System;
using System.IO;
using Lennox.NvEncSharp.Sample.VideoDecode;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lennox.NvEncSharp.Test
{
    [TestClass]
    public class Av1ObuReaderTests
    {
        [TestMethod]
        public void ReadsSeparateObusAndEndOfStream()
        {
            using var input = new MemoryStream(new byte[] { 0x12, 0, 0x32, 3, 0, 0, 1 });
            CollectionAssert.AreEqual(new byte[] { 0x12, 0 }, Av1ObuReader.ReadNext(input));
            // NAL start-code bytes inside an OBU must remain payload.
            CollectionAssert.AreEqual(new byte[] { 0x32, 3, 0, 0, 1 }, Av1ObuReader.ReadNext(input));
            Assert.IsNull(Av1ObuReader.ReadNext(input));
        }

        [TestMethod]
        public void ReadsExtensionAndMultibyteSizeAcrossShortReads()
        {
            var packet = new byte[132];
            packet[0] = 0x36;
            packet[1] = 0x20;
            packet[2] = 0x80;
            packet[3] = 1; // 128 payload bytes.
            using var input = new ShortReadStream(packet);
            CollectionAssert.AreEqual(packet, Av1ObuReader.ReadNext(input));
        }

        [TestMethod]
        public void RejectsTruncatedHeadersAndPayloads()
        {
            foreach (var bytes in new[] {
                new byte[] { 0x32 }, new byte[] { 0x36 },
                new byte[] { 0x32, 0x80 }, new byte[] { 0x32, 2, 0 }
            })
            {
                using var input = new MemoryStream(bytes);
                Assert.ThrowsException<EndOfStreamException>(() => Av1ObuReader.ReadNext(input));
            }
        }

        [TestMethod]
        public void RejectsInvalidHeadersAndExcessiveSizes()
        {
            foreach (var bytes in new[] {
                new byte[] { 0x30 }, new byte[] { 0xb2 }, new byte[] { 0x33 },
                new byte[] { 0x36, 1 },
                new byte[] { 0x32, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80 },
                new byte[] { 0x32, 0x80, 0x80, 0x80, 0x20 } // 64 MiB.
            })
            {
                using var input = new MemoryStream(bytes);
                Assert.ThrowsException<InvalidDataException>(() => Av1ObuReader.ReadNext(input));
            }
        }

        private sealed class ShortReadStream : MemoryStream
        {
            public ShortReadStream(byte[] buffer) : base(buffer) { }

            public override int Read(byte[] buffer, int offset, int count)
                => base.Read(buffer, offset, Math.Min(count, 1));
        }
    }
}
