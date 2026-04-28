using System;

// ReSharper disable UnusedMember.Global
namespace Lennox.NvEncSharp
{
    public static class NvEncCodecGuids
    {
        public static Guid H264 = Guid.Parse("6BC82762-4E63-4ca4-AA85-1E50F321F6BF");
        public static Guid Hevc = Guid.Parse("790CDC88-4522-4d7b-9425-BDA9975F7603");
        public static Guid Av1 = Guid.Parse("0A352289-0AA7-4759-862D-5D15CD16D254");
    }

    public static class NvEncProfileGuids
    {
        public static readonly Guid CodecAutoselect = Guid.Parse("BFD6F8E7-233C-4341-8B3E-4818523803F4");
        public static readonly Guid H264Baseline = Guid.Parse("0727BCAA-78C4-4c83-8C2F-EF3DFF267C6A");
        public static readonly Guid H264Main = Guid.Parse("60B5C1D4-67FE-4790-94D5-C4726D7B6E6D");
        public static readonly Guid H264High = Guid.Parse("E7CBC309-4F7A-4b89-AF2A-D537C92BE310");
        public static readonly Guid H264High444 = Guid.Parse("7AC663CB-A598-4960-B844-339B261A7D52");
        public static readonly Guid H264Stereo = Guid.Parse("40847BF5-33F7-4601-9084-E8FE3C1DB8B7");
        public static readonly Guid H264ProgressiveHigh = Guid.Parse("B405AFAC-F32B-417B-89C4-9ABEED3E5978");
        public static readonly Guid H264ConstrainedHigh = Guid.Parse("AEC1BD87-E85B-48f2-84C3-98BCA6285072");
        public static readonly Guid HevcMain = Guid.Parse("B514C39A-B55B-40fa-878F-F1253B4DFDEC");
        public static readonly Guid HevcMain10 = Guid.Parse("fa4d2b6c-3a5b-411a-8018-0a3f5e3c9be5");
        /// <summary>For HEVC Main 444 8 bit and HEVC Main 444 10 bit profiles only</summary>
        public static readonly Guid HevcFrext = Guid.Parse("51ec32b5-1b4c-453c-9cbd-b616bd621341");
        public static readonly Guid Av1Main = Guid.Parse("5f2a39f5-f14e-4f95-9a9e-b76d568fcf97");
    }

    public static class NvEncPresetGuids
    {
        public static readonly Guid P1 = Guid.Parse("FC0A8D3E-45F8-4CF8-80C7-298871590EBF");
        public static readonly Guid P2 = Guid.Parse("F581CFB8-88D6-4381-93F0-DF13F9C27DAB");
        public static readonly Guid P3 = Guid.Parse("36850110-3A07-441F-94D5-3670631F91F6");
        public static readonly Guid P4 = Guid.Parse("90A7B826-DF06-4862-B9D2-CD6D73A08681");
        public static readonly Guid P5 = Guid.Parse("21C6E6B4-297A-4CBA-998F-B6CBDE72ADE3");
        public static readonly Guid P6 = Guid.Parse("8E75C279-6299-4AB6-8302-0B215A335CF5");
        public static readonly Guid P7 = Guid.Parse("84848C12-6F71-4C13-931B-53E283F57974");
    }
}
