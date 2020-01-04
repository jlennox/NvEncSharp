using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lennox.NvEncSharp.Test
{
    [TestClass]
    public unsafe class NalPacketTests
    {
        [TestMethod]
        public void IndexOfSignature()
        {
            {
                Span<byte> index0 = new byte[] { 0, 0, 1 };
                Assert.AreEqual(0, NalPacket.IndexOfSignature(index0, out var size));
                Assert.AreEqual(size, 3);
            }

            {
                Span<byte> index1 = new byte[] { 0, 0, 0, 1 };
                Assert.AreEqual(0, NalPacket.IndexOfSignature(index1, out var size));
                Assert.AreEqual(size, 4);
            }

            {
                Span<byte> indexNever = new byte[] { 0, 0, 0, 0 };
                Assert.AreEqual(-1, NalPacket.IndexOfSignature(indexNever, out var size));
            }
        }

        [TestMethod]
        public void ReadNextPacket()
        {
            Span<byte> stream = new byte[] {
                0, 0, 1, 0xfe, 0xff,
                0, 0, 0, 0, 1, 0xc4, 0xc5, 0xc5, 0xc5
            };

            {
                var packet = NalPacket.ReadNextPacket(ref stream);

                Assert.IsTrue(packet.Complete);
                CollectionAssert.AreEqual(
                    new byte[] { 0, 0, 1, 0xfe, 0xff },
                    packet.Packet.ToArray());
            }

            {
                var packet = NalPacket.ReadNextPacket(ref stream);

                Assert.IsFalse(packet.Complete);
                CollectionAssert.AreEqual(
                    new byte[] { 0, 0, 0, 0, 1, 0xc4, 0xc5, 0xc5, 0xc5 },
                    packet.Packet.ToArray());
            }
        }

        [TestMethod]
        public void ReadSegmentedWrites()
        {
            var stream = new MemoryStream(new byte[] {
                0x00, 0x00, 0x00, 0x01, 0x67, 0x64, 0x00, 0x34, 0xAC, 0x2B,
                0x40, 0x1E, 0x00, 0x21, 0xF6, 0x02, 0x20, 0x00, 0x00, 0x7D,
                0x00, 0x00, 0x20, 0x3A, 0x10, 0x80
            });

            var buffer = new byte[10];

            {
                var nread = stream.Read(buffer, 0, buffer.Length);
                var inputBuffer = new Span<byte>(buffer, 0, nread);
                var packet = NalPacket.ReadNextPacket(ref inputBuffer);
                Assert.AreEqual(10, packet.Packet.Length);
                Assert.AreEqual(0, packet.PacketPrefix.Length);
                Assert.AreEqual(false, packet.Complete);
            }

            {
                var nread = stream.Read(buffer, 0, buffer.Length);
                var inputBuffer = new Span<byte>(buffer, 0, nread);
                var packet = NalPacket.ReadNextPacket(ref inputBuffer);
                Assert.AreEqual(10, packet.Packet.Length);
                Assert.AreEqual(0, packet.PacketPrefix.Length);
                Assert.AreEqual(false, packet.Complete);
            }

            {
                var nread = stream.Read(buffer, 0, buffer.Length);
                var inputBuffer = new Span<byte>(buffer, 0, nread);
                var packet = NalPacket.ReadNextPacket(ref inputBuffer);
                Assert.AreEqual(6, packet.Packet.Length);
                Assert.AreEqual(0, packet.PacketPrefix.Length);
                Assert.AreEqual(false, packet.Complete);
            }
        }

        [TestMethod]
        public void ReadMultipleSegmentedWrites()
        {
            var stream = new MemoryStream(new byte[] {
                0x00, 0x00, 0x00, 0x01, 0x67, 0x64, 0x00, 0x34, 0xAC, 0x2B,
                0x40, 0x1E, 0x00, 0x21, 0xF6, 0x02, 0x20, 0x00, 0x00, 0x7D,
                0x00, 0x00, 0x20, 0x3A, 0x10, 0x80, 0x00, 0x00, 0x00, 0x01,
                0x68, 0xEE, 0x3C, 0xB0, 0x00, 0x00, 0x00, 0x01, 0x05, 0xB8
            });

            var buffer = new byte[10];

            {
                var nread = stream.Read(buffer, 0, buffer.Length);
                var inputBuffer = new Span<byte>(buffer, 0, nread);
                var packet = NalPacket.ReadNextPacket(ref inputBuffer);
                Assert.AreEqual(10, packet.Packet.Length);
                Assert.AreEqual(0, packet.PacketPrefix.Length);
                Assert.AreEqual(false, packet.Complete);
            }

            {
                var nread = stream.Read(buffer, 0, buffer.Length);
                var inputBuffer = new Span<byte>(buffer, 0, nread);
                var packet = NalPacket.ReadNextPacket(ref inputBuffer);
                Assert.AreEqual(10, packet.Packet.Length);
                Assert.AreEqual(0, packet.PacketPrefix.Length);
                Assert.AreEqual(false, packet.Complete);
            }

            {
                var nread = stream.Read(buffer, 0, buffer.Length);
                var inputBuffer = new Span<byte>(buffer, 0, nread);
                var packet = NalPacket.ReadNextPacket(ref inputBuffer);
                Assert.AreEqual(6, packet.Packet.Length);
                Assert.AreEqual(0, packet.PacketPrefix.Length);
                Assert.AreEqual(true, packet.Complete);

                var packet2 = NalPacket.ReadNextPacket(ref inputBuffer);
                Assert.AreEqual(4, packet2.Packet.Length);
                Assert.AreEqual(0, packet2.PacketPrefix.Length);
                Assert.AreEqual(false, packet2.Complete);
            }

            {
                var nread = stream.Read(buffer, 0, buffer.Length);
                var inputBuffer = new Span<byte>(buffer, 0, nread);
                var packet = NalPacket.ReadNextPacket(ref inputBuffer);
                Assert.AreEqual(6, packet.Packet.Length);
                Assert.AreEqual(0, packet.PacketPrefix.Length);
                Assert.AreEqual(true, packet.Complete);
            }
        }

        [TestMethod]
        public void GetNalPacketType()
        {
            Span<byte> stream = new byte[] {
                0x00, 0x00, 0x00, 0x01, 0x67, 0x64, 0x00, 0x34, 0xAC, 0x2B,
                0x40, 0x1E, 0x00, 0x21, 0xF6, 0x02, 0x20, 0x00, 0x00, 0x7D,
                0x00, 0x00, 0x20, 0x3A, 0x10, 0x80
            };

            var packet = NalPacket.ReadNextPacket(ref stream);
            Assert.AreEqual(
                NalPacketType.SequenceParameterSet,
                packet.PacketType);
        }
    }
}
