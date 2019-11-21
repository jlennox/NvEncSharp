using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Lennox.NvEncSharp.Test.CodeGeneration;

namespace Lennox.NvEncSharp.Test
{
    [TestClass]
    public class GenerateStruct
    {
        private class StructEntry
        {
            public string Type { get; set; }
            public int ArraySize { get; set; }
            public bool IsPointer { get; set; }
            public bool IsPointerPointer { get; set; }
            public string Name { get; set; }
            public string Comment { get; set; }
            public int BitLength { get; set; }
            public string Attribute { get; set; }

            public string PointerDescription => IsPointer ? "*" : IsPointerPointer ? "**" : "";

            public void PrintComment(StringBuilder output)
            {
                output.Append($"        /// <summary>{Name}");

                if (!string.IsNullOrWhiteSpace(Comment))
                {
                    var comment = CleanupComment(Comment, "        ")
                        .TrimStart(' ', '/');
                    output.Append($": {comment}");
                }

                output.Append("</summary>\r\n");
            }
        }

        private enum StructPos
        {
            Unknown,
            Type,
            Name,
            Comment
        }

        private static readonly Regex _typeLength = new Regex(
            @"(?<name>.+?)\[(?<value>\d+)\]", RegexOptions.Compiled);

        private static IReadOnlyList<StructEntry> ParseStruct(string def)
        {
            var pos = StructPos.Unknown;
            var i = def.IndexOf('{');
            var sb = new StringBuilder();
            var entries = new List<StructEntry>();
            var currentEntry = new StructEntry();

            for (; i < def.Length; ++i)
            {
                var chr = def[i];
                var ended = false;
                var rest = def.Substring(i); // for debugging.

                switch (chr)
                {
                    case '{' when pos == StructPos.Unknown:
                        pos = StructPos.Type;
                        SkipWhitespace(def, ref i);
                        break;
                    case '}' when pos == StructPos.Type:
                        return entries;
                    case '/' when pos == StructPos.Type:
                        currentEntry.Comment = GetComment(def, ref i);
                        break;
                    case ' ' when pos == StructPos.Type:
                        var typeFull = sb.ToString().Trim();
                        var typeName = typeFull.TrimEnd('*');
                        var points = typeFull.Length - typeName.Length;
                        switch (points)
                        {
                            case 1: currentEntry.IsPointer = true; break;
                            case 2: currentEntry.IsPointerPointer = true; break;
                        }
                        currentEntry.Type = typeName;
                        sb.Clear();
                        SkipWhitespace(def, ref i);
                        pos = StructPos.Name;
                        break;
                    case ':' when pos == StructPos.Name:
                    case ';' when pos == StructPos.Name:
                    case '/' when pos == StructPos.Name:
                        currentEntry.Name = sb.ToString().Trim();
                        sb.Clear();
                        SkipWhitespace(def, ref i);
                        if (chr == ':')
                        {
                            ++i;
                            SkipWhitespace(def, ref i);
                            var numsb = new StringBuilder();
                            for (; i < def.Length; ++i)
                            {
                                chr = def[i];
                                if (char.IsDigit(chr))
                                {
                                    numsb.Append(chr);
                                    continue;
                                }

                                break;
                            }

                            currentEntry.BitLength = int.Parse(numsb.ToString());
                            SkipWhitespace(def, ref i);


                            for (; i < def.Length; ++i)
                            {
                                chr = def[i];
                                var end = false;
                                switch (chr)
                                {
                                    case ';': break;
                                    case ' ': continue;
                                    default: --i; end = true; break;
                                }

                                if (end) break;
                            }
                        }
                        pos = StructPos.Comment;
                        ended = true;
                        break;
                    default:
                        switch (pos)
                        {
                            case StructPos.Comment:
                            case StructPos.Name:
                            case StructPos.Type:
                                sb.Append(chr);
                                break;
                        }
                        break;
                }

                if (ended)
                {
                    if (currentEntry.Comment == null)
                    {
                        for (; i < def.Length; ++i)
                        {
                            var chr2 = def[i];
                            if (char.IsWhiteSpace(chr2)) continue;
                            if (chr2 == '/' && def[i + 1] == '*')
                            {
                                currentEntry.Comment = GetComment(def, ref i);
                                break;
                            }

                            --i;
                            break;
                        }
                    }

                    var match = _typeLength.Match(currentEntry.Name);

                    if (match.Success)
                    {
                        currentEntry.ArraySize = int.Parse(match.Groups["value"].Value);
                    }

                    entries.Add(currentEntry);
                    currentEntry = new StructEntry();
                    pos = StructPos.Type;
                    sb.Clear();
                    SkipWhitespace(def, ref i);
                }
            }

            return entries;
        }

        [TestMethod]
        public void GenerateStructs()
        {
            var header = LoadNvencHeader();

            var skippedStructs = new HashSet<string> { "NV_ENCODE_API_FUNCTION_LIST" };
            var validForFixed = new HashSet<string> { "bool", "byte", "short", "int", "long", "char", "sbyte", "ushort", "uint", "ulong", "float", "double", _sysintname };

            // The way comments are matched is not the best.
            var exp = new Regex(@"((?<=/\*)(?<comment>[^/]+)\*/\s+)?typedef (?<type>struct|union) _(?<name>[^\s{]+).+?\s*?\r\n\s*?}.+?;\s*?\r\n",
                RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline);

            var output = new StringBuilder();
            AppendHeader(output);
            var matches = exp.Matches(header).Cast<Match>().ToArray();
            var structs = matches.Where(t => t.Groups["type"].Value == "struct").ToArray();
            var unions = matches.Where(t => t.Groups["type"].Value == "union").ToArray();

            void PrintLine(string type, string line)
            {
                if (type != _sysintname)
                {
                    output.Append(line);
                    return;
                }

                output.Append("#if X64PLATFORM\r\n");
                output.Append(line.Replace(_sysintname, "ulong"));
                output.Append("#else\r\n");
                output.Append(line.Replace(_sysintname, "uint"));
                output.Append("#endif\r\n");
            }

            void PrintEntry(StructEntry entry)
            {
                var type = FixTypename(entry);
                if (entry.Attribute != null) output.Append($"    {entry.Attribute}\r\n");

                if (entry.ArraySize > 0 && !validForFixed.Contains(type))
                {
                    var name = FixStructMemeberName(entry.Name);
                    name = name.Substring(0, name.IndexOf('['));
                    for (var i2 = 0; i2 < entry.ArraySize; ++i2)
                    {
                        PrintLine(type, $"        public {type}{entry.PointerDescription} {name}{i2};\r\n");
                    }

                    return;
                }

                PrintLine(type, $"        public {(entry.ArraySize > 0 ? "fixed " : "")}{type}{entry.PointerDescription} {FixStructMemeberName(entry.Name)};\r\n");
            }

            void PrintDefinition(string name, Match match, LayoutKind layoutKind)
            {
                var comment = match.Groups["comment"];

                output.Append($"    /// <summary>{name}");

                if (comment.Success &&
                    !string.IsNullOrWhiteSpace(comment.Value))
                {
                    output.Append($"\r\n{CleanupComment(comment.Value, "    ")}");
                }

                output.Append($"</summary>\r\n");
                output.Append($"    [StructLayout(LayoutKind.{layoutKind.ToString()})]\r\n");
                output.Append($"    public unsafe struct {Fixname(name)}\r\n    {{\r\n");
            }

            // Thankfully all the unions are discrete types and only union
            // a single offset.
            foreach (var match in unions)
            {
                var def = match.Value;
                var name = match.Groups["name"].Value;

                if (skippedStructs.Contains(name)) continue;

                var entries = ParseStruct(def);
                PrintDefinition(name, match, LayoutKind.Explicit);

                foreach (var entry in entries)
                {
                    entry.PrintComment(output);

                    output.Append($"        [FieldOffset(0)]\r\n");
                    PrintEntry(entry);
                }

                output.Append("    }\r\n\r\n");
            }

            foreach (var match in structs)
            {
                var def = match.Value;
                var name = match.Groups["name"].Value;

                if (skippedStructs.Contains(name)) continue;

                var entries = ParseStruct(def);
                PrintDefinition(name, match, LayoutKind.Sequential);

                var bitId = 0;
                var bitFieldName = "Uninitialized";
                var bitoffset = 0;
                var bitsleft = 0;

                for (var i = 0; i < entries.Count; ++i)
                {
                    var entry = entries[i];

                    if (entry.BitLength > 0 && bitsleft == 0)
                    {
                        var bitcount = 0;
                        bitoffset = 0;
                        var foundByteBoundary = false;

                        for (var i2 = i; i2 < entries.Count && !foundByteBoundary; ++i2)
                        {
                            var subentry = entries[i2];
                            if (subentry.BitLength == 0)
                            {
                                break;
                            }

                            bitcount += subentry.BitLength;

                            switch (bitcount)
                            {
                                case 8:
                                case 16:
                                case 32:
                                case 64:
                                    foundByteBoundary = true;
                                    break;
                            }
                        }

                        if (!foundByteBoundary)
                        {
                            Debugger.Break();
                            //throw new System.Exception();
                        }

                        bitsleft = bitcount;
                        bitFieldName = "BitField" + ++bitId;
                        output.Append($"        internal fixed byte {bitFieldName}[{bitcount / 8}];\r\n");
                    }

                    entry.PrintComment(output);

                    if (entry.BitLength == 0)
                    {
                        PrintEntry(entry);
                        continue;
                    }

                    var type = TypeFromBitLength(entry);

                    var skipOutput = entry.Name.Contains("reserved");
                    var toResult = entry.BitLength == 1 ? " == 1" : "";
                    var fromValue = entry.BitLength == 1 ? " ? 1 : 0" : "";
                    var ptrType = entry.BitLength == 1 ? "byte" : type;

                    if (!skipOutput)
                    {
                        output.Append($"        public {type} {FixStructMemeberName(entry.Name)} {{\r\n");
                        if (entry.BitLength == 1)
                        {
                            output.Append($"            get => ({bitFieldName}[{bitoffset / 8}] & {1 << bitoffset}) != 0;\r\n");
                        }
                        else
                        {
                            output.Append($"            get {{ fixed (byte* ptr = &{bitFieldName}[{bitoffset / 8}]) {{ ");
                            output.Append($"return ((*({ptrType}*)ptr >> {bitoffset % 8}) & {entry.BitLength}){toResult}; }} }}\r\n");
                        }
                    }

                    var sets = new List<string>();
                    var thisbitsleft = entry.BitLength;
                    var thisoffset = bitoffset;
                    var thisvalueoffset = 0;

                    if (entry.BitLength == 1)
                    {
                        sets.Add($"{bitFieldName}[{thisoffset / 8}] = value ? (byte)({bitFieldName}[{thisoffset / 8}] | {1 << bitoffset}) : (byte)({bitFieldName}[{thisoffset / 8}] & {~(1 << bitoffset)});");
                    }
                    else
                    {
                        while (thisbitsleft > 0)
                        {
                            var bitindex = thisoffset / 8;
                            var currentbits = thisbitsleft > 8 ? (thisbitsleft % 8) : thisbitsleft;

                            currentbits = currentbits == 0 ? 1 : currentbits;

                            sets.Add($"{bitFieldName}[{bitindex}] = (byte)(({bitFieldName}[{bitindex}] & ~{currentbits << thisoffset}) | (((value{fromValue}) << {(thisoffset % 8)}) & {currentbits << (thisoffset % 8)}));");

                            thisvalueoffset += currentbits;
                            thisbitsleft -= currentbits;
                            thisoffset += currentbits;
                        }
                    }

                    if (!skipOutput)
                    {
                        if (sets.Count == 1)
                        {
                            output.Append($"            set => {sets[0]}\r\n");
                        }
                        else
                        {
                            output.Append($"            set {{\r\n");
                            foreach (var set in sets) output.Append($"                {set}\r\n");
                            output.Append($"            }}\r\n");
                        }

                        output.Append($"        }}\r\n");
                    }

                    bitoffset += entry.BitLength;
                    bitsleft -= entry.BitLength;
                }

                output.Append("    }\r\n\r\n");
            }

            output.Append("}");

            WriteNvencHeaderOutput(output, "Structs.cs");
        }

        private static string TypeFromBitLength(StructEntry entry)
        {
            if (entry.BitLength == 1) return "bool";

            return FixTypename(entry);
        }

        private static readonly Dictionary<string, string> _fixedTypes = new Dictionary<string, string>
        {
            { "uint8_t", "byte" },
            { "uint16_t", "ushort" },
            { "uint32_t", "uint" },
            { "uint64_t", "ulong" },
            { "int8_t", "byte" },
            { "int16_t", "short" },
            { "int32_t", "int" },
            { "int64_t", "long" },
            { "GUID", "Guid" },
            { "void", "IntPtr" }
        };

        private static readonly Dictionary<string, string> _aliasedTypes = new Dictionary<string, string>
        {
            { "NV_ENC_CONFIG_HEVC_VUI_PARAMETERS", "NV_ENC_CONFIG_H264_VUI_PARAMETERS" }
        };

        private const string _sysintname = "__sysint__";

        private static string FixTypename(StructEntry entry)
        {
            if (_aliasedTypes.TryGetValue(entry.Type, out var aliased)) entry.Type = aliased;

            if (entry.Type == "void")
            {
                entry.IsPointer = false;
                if (entry.IsPointerPointer)
                {
                    entry.IsPointer = true;
                    entry.IsPointerPointer = false;
                }

                if (entry.ArraySize > 0)
                {
                    //entry.Attribute = "[MarshalAs(UnmanagedType.SysInt)]";
                    return _sysintname;
                }
            }

            if (_fixedTypes.TryGetValue(entry.Type, out var fixedType))
            {
                return fixedType;
            }

            return Fixname(entry.Type).Trim();
        }

        [TestMethod]
        public void TestNvEncConfigH264Meonly()
        {
            var config = new NvEncConfigH264Meonly();

            Assert.AreEqual(false, config.DisablePartition16x16);
            Assert.AreEqual(false, config.DisablePartition8x16);
            Assert.AreEqual(false, config.DisablePartition16x8);
            Assert.AreEqual(false, config.DisablePartition8x8);
            Assert.AreEqual(false, config.DisableIntraSearch);
            Assert.AreEqual(false, config.BStereoEnable);

            config.DisablePartition16x8 = true;

            Assert.AreEqual(false, config.DisablePartition16x16);
            Assert.AreEqual(false, config.DisablePartition8x16);
            Assert.AreEqual(true, config.DisablePartition16x8);
            Assert.AreEqual(false, config.DisablePartition8x8);
            Assert.AreEqual(false, config.DisableIntraSearch);
            Assert.AreEqual(false, config.BStereoEnable);

            config.DisablePartition16x8 = false;

            Assert.AreEqual(false, config.DisablePartition16x16);
            Assert.AreEqual(false, config.DisablePartition8x16);
            Assert.AreEqual(false, config.DisablePartition16x8);
            Assert.AreEqual(false, config.DisablePartition8x8);
            Assert.AreEqual(false, config.DisableIntraSearch);
            Assert.AreEqual(false, config.BStereoEnable);
        }

        [TestMethod]
        public void ParseStructTests()
        {
            const string x = @"typedef struct _NV_ENC_CONFIG_H264
{
    uint32_t reserved                  :1;                          /**< [in]: Reserved and must be set to 0 */
    uint32_t enableStereoMVC           :1;                          /**< [in]: Set to 1 to enable stereo MVC*/
    uint32_t hierarchicalPFrames       :1;                          /**< [in]: Set to 1 to enable hierarchical PFrames */
    uint32_t hierarchicalBFrames       :1;                          /**< [in]: Set to 1 to enable hierarchical BFrames */
    uint32_t outputBufferingPeriodSEI  :1;                          /**< [in]: Set to 1 to write SEI buffering period syntax in the bitstream */
    uint32_t outputPictureTimingSEI    :1;                          /**< [in]: Set to 1 to write SEI picture timing syntax in the bitstream.  When set for following rateControlMode : NV_ENC_PARAMS_RC_CBR, NV_ENC_PARAMS_RC_CBR_LOWDELAY_HQ,
                                                                               NV_ENC_PARAMS_RC_CBR_HQ, filler data is inserted if needed to achieve hrd bitrate */
    uint32_t outputAUD                 :1;                          /**< [in]: Set to 1 to write access unit delimiter syntax in bitstream */
    uint32_t disableSPSPPS             :1;                          /**< [in]: Set to 1 to disable writing of Sequence and Picture parameter info in bitstream */
    uint32_t outputFramePackingSEI     :1;                          /**< [in]: Set to 1 to enable writing of frame packing arrangement SEI messages to bitstream */
    uint32_t outputRecoveryPointSEI    :1;                          /**< [in]: Set to 1 to enable writing of recovery point SEI message */
    uint32_t enableIntraRefresh        :1;                          /**< [in]: Set to 1 to enable gradual decoder refresh or intra refresh. If the GOP structure uses B frames this will be ignored */
    uint32_t enableConstrainedEncoding :1;                          /**< [in]: Set this to 1 to enable constrainedFrame encoding where each slice in the constarined picture is independent of other slices
                                                                               Check support for constrained encoding using ::NV_ENC_CAPS_SUPPORT_CONSTRAINED_ENCODING caps. */
    uint32_t repeatSPSPPS              :1;                          /**< [in]: Set to 1 to enable writing of Sequence and Picture parameter for every IDR frame */
    uint32_t enableVFR                 :1;                          /**< [in]: Set to 1 to enable variable frame rate. */
    uint32_t enableLTR                 :1;                          /**< [in]: Set to 1 to enable LTR (Long Term Reference) frame support. LTR can be used in two modes: ""LTR Trust"" mode and ""LTR Per Picture"" mode.
                                                                               LTR Trust mode: In this mode, ltrNumFrames pictures after IDR are automatically marked as LTR. This mode is enabled by setting ltrTrustMode = 1.
                                                                                               Use of LTR Trust mode is strongly discouraged as this mode may be deprecated in future.
                                                                               LTR Per Picture mode: In this mode, client can control whether the current picture should be marked as LTR. Enable this mode by setting
                                                                                                     ltrTrustMode = 0 and ltrMarkFrame = 1 for the picture to be marked as LTR. This is the preferred mode
                                                                                                     for using LTR.
                                                                               Note that LTRs are not supported if encoding session is configured with B-frames */
    uint32_t qpPrimeYZeroTransformBypassFlag :1;                    /**< [in]: To enable lossless encode set this to 1, set QP to 0 and RC_mode to NV_ENC_PARAMS_RC_CONSTQP and profile to HIGH_444_PREDICTIVE_PROFILE.
                                                                               Check support for lossless encoding using ::NV_ENC_CAPS_SUPPORT_LOSSLESS_ENCODE caps.  */
    uint32_t useConstrainedIntraPred   :1;                          /**< [in]: Set 1 to enable constrained intra prediction. */
    uint32_t enableFillerDataInsertion :1;                          /**< [in]: Set to 1 to enable insertion of filler data in the bitstream.
                                                                               This flag will take effect only when one of the CBR rate
                                                                               control modes (NV_ENC_PARAMS_RC_CBR, NV_ENC_PARAMS_RC_CBR_HQ,
                                                                               NV_ENC_PARAMS_RC_CBR_LOWDELAY_HQ) is in use and both
                                                                               NV_ENC_INITIALIZE_PARAMS::frameRateNum and
                                                                               NV_ENC_INITIALIZE_PARAMS::frameRateDen are set to non-zero
                                                                               values. Setting this field when
                                                                               NV_ENC_INITIALIZE_PARAMS::enableOutputInVidmem is also set
                                                                               is currently not supported and will make ::NvEncInitializeEncoder()
                                                                               return an error. */
    uint32_t reservedBitFields         :14;                         /**< [in]: Reserved bitfields and must be set to 0 */
    uint32_t level;                                                 /**< [in]: Specifies the encoding level. Client is recommended to set this to NV_ENC_LEVEL_AUTOSELECT in order to enable the NvEncodeAPI interface to select the correct level. */
    uint32_t idrPeriod;                                             /**< [in]: Specifies the IDR interval. If not set, this is made equal to gopLength in NV_ENC_CONFIG.Low latency application client can set IDR interval to NVENC_INFINITE_GOPLENGTH so that IDR frames are not inserted automatically. */
    uint32_t separateColourPlaneFlag;                               /**< [in]: Set to 1 to enable 4:4:4 separate colour planes */
    uint32_t disableDeblockingFilterIDC;                            /**< [in]: Specifies the deblocking filter mode. Permissible value range: [0,2] */
    uint32_t numTemporalLayers;                                     /**< [in]: Specifies max temporal layers to be used for hierarchical coding. Valid value range is [1,::NV_ENC_CAPS_NUM_MAX_TEMPORAL_LAYERS] */
    uint32_t spsId;                                                 /**< [in]: Specifies the SPS id of the sequence header */
    uint32_t ppsId;                                                 /**< [in]: Specifies the PPS id of the picture header */
    NV_ENC_H264_ADAPTIVE_TRANSFORM_MODE adaptiveTransformMode;      /**< [in]: Specifies the AdaptiveTransform Mode. Check support for AdaptiveTransform mode using ::NV_ENC_CAPS_SUPPORT_ADAPTIVE_TRANSFORM caps. */
    NV_ENC_H264_FMO_MODE                fmoMode;                    /**< [in]: Specified the FMO Mode. Check support for FMO using ::NV_ENC_CAPS_SUPPORT_FMO caps. */
    NV_ENC_H264_BDIRECT_MODE            bdirectMode;                /**< [in]: Specifies the BDirect mode. Check support for BDirect mode using ::NV_ENC_CAPS_SUPPORT_BDIRECT_MODE caps.*/
    NV_ENC_H264_ENTROPY_CODING_MODE     entropyCodingMode;          /**< [in]: Specifies the entropy coding mode. Check support for CABAC mode using ::NV_ENC_CAPS_SUPPORT_CABAC caps. */
    NV_ENC_STEREO_PACKING_MODE          stereoMode;                 /**< [in]: Specifies the stereo frame packing mode which is to be signalled in frame packing arrangement SEI */
    uint32_t                            intraRefreshPeriod;         /**< [in]: Specifies the interval between successive intra refresh if enableIntrarefresh is set. Requires enableIntraRefresh to be set.
                                                                               Will be disabled if NV_ENC_CONFIG::gopLength is not set to NVENC_INFINITE_GOPLENGTH. */
    uint32_t                            intraRefreshCnt;            /**< [in]: Specifies the length of intra refresh in number of frames for periodic intra refresh. This value should be smaller than intraRefreshPeriod */
    uint32_t                            maxNumRefFrames;            /**< [in]: Specifies the DPB size used for encoding. Setting it to 0 will let driver use the default dpb size.
                                                                               The low latency application which wants to invalidate reference frame as an error resilience tool
                                                                               is recommended to use a large DPB size so that the encoder can keep old reference frames which can be used if recent
                                                                               frames are invalidated. */
    uint32_t                            sliceMode;                  /**< [in]: This parameter in conjunction with sliceModeData specifies the way in which the picture is divided into slices
                                                                               sliceMode = 0 MB based slices, sliceMode = 1 Byte based slices, sliceMode = 2 MB row based slices, sliceMode = 3 numSlices in Picture.
                                                                               When forceIntraRefreshWithFrameCnt is set it will have priority over sliceMode setting
                                                                               When sliceMode == 0 and sliceModeData == 0 whole picture will be coded with one slice */
    uint32_t                            sliceModeData;              /**< [in]: Specifies the parameter needed for sliceMode. For:
                                                                               sliceMode = 0, sliceModeData specifies # of MBs in each slice (except last slice)
                                                                               sliceMode = 1, sliceModeData specifies maximum # of bytes in each slice (except last slice)
                                                                               sliceMode = 2, sliceModeData specifies # of MB rows in each slice (except last slice)
                                                                               sliceMode = 3, sliceModeData specifies number of slices in the picture. Driver will divide picture into slices optimally */
    NV_ENC_CONFIG_H264_VUI_PARAMETERS   h264VUIParameters;          /**< [in]: Specifies the H264 video usability info pamameters */
    uint32_t                            ltrNumFrames;               /**< [in]: Specifies the number of LTR frames. This parameter has different meaning in two LTR modes.
                                                                               In ""LTR Trust"" mode (ltrTrustMode = 1), encoder will mark the first ltrNumFrames base layer reference frames within each IDR interval as LTR.
                                                                               In ""LTR Per Picture"" mode (ltrTrustMode = 0 and ltrMarkFrame = 1), ltrNumFrames specifies maximum number of LTR frames in DPB. */
    uint32_t                            ltrTrustMode;               /**< [in]: Specifies the LTR operating mode. See comments near NV_ENC_CONFIG_H264::enableLTR for description of the two modes.
                                                                               Set to 1 to use ""LTR Trust"" mode of LTR operation. Clients are discouraged to use ""LTR Trust"" mode as this mode may
                                                                               be deprecated in future releases.
                                                                               Set to 0 when using ""LTR Per Picture"" mode of LTR operation. */
    uint32_t                            chromaFormatIDC;            /**< [in]: Specifies the chroma format. Should be set to 1 for yuv420 input, 3 for yuv444 input.
                                                                               Check support for YUV444 encoding using ::NV_ENC_CAPS_SUPPORT_YUV444_ENCODE caps.*/
    uint32_t                            maxTemporalLayers;          /**< [in]: Specifies the max temporal layer used for hierarchical coding. */
    NV_ENC_BFRAME_REF_MODE              useBFramesAsRef;            /**< [in]: Specifies the B-Frame as reference mode. Check support for useBFramesAsRef mode using ::NV_ENC_CAPS_SUPPORT_BFRAME_REF_MODE caps.*/
    NV_ENC_NUM_REF_FRAMES               numRefL0;                   /**< [in]: Specifies max number of reference frames in reference picture list L0, that can be used by hardware for prediction of a frame.
                                                                               Check support for numRefL0 using ::NV_ENC_CAPS_SUPPORT_MULTIPLE_REF_FRAMES caps. */
    NV_ENC_NUM_REF_FRAMES               numRefL1;                   /**< [in]: Specifies max number of reference frames in reference picture list L1, that can be used by hardware for prediction of a frame.
                                                                               Check support for numRefL1 using ::NV_ENC_CAPS_SUPPORT_MULTIPLE_REF_FRAMES caps. */
    uint32_t                            reserved1[267];             /**< [in]: Reserved and must be set to 0 */
    void*                               reserved2[64];              /**< [in]: Reserved and must be set to NULL */
} NV_ENC_CONFIG_H264;";

            var parsed = ParseStruct(x);
        }
    }
}
