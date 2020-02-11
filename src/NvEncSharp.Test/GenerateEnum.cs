using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Lennox.NvEncSharp.Test.CodeGeneration;

namespace Lennox.NvEncSharp.Test
{
    [TestClass]
    public class GenerateEnum
    {
        private class EnumEntry
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public string Comment { get; set; }
        }

        private enum EnumPos
        {
            Unknown,
            Name,
            Equals,
            Value,
            Comment
        }

        private static IReadOnlyList<EnumEntry> ParseEnum(string def)
        {
            var pos = EnumPos.Unknown;
            var i = def.IndexOf('{');
            var sb = new StringBuilder();
            var entries = new List<EnumEntry>();
            var currentEntry = new EnumEntry();

            for (; i < def.Length; ++i)
            {
                var chr = def[i];
                var ended = false;
                var rest = def.Substring(i); // for debugging.

                switch (chr)
                {
                    case '{' when pos == EnumPos.Unknown:
                        pos = EnumPos.Name;
                        SkipWhitespace(def, ref i);
                        break;
                    case '/' when pos == EnumPos.Name:
                        currentEntry.Comment = GetComment(def, ref i);
                        break;
                    case ',' when pos == EnumPos.Name:
                        currentEntry.Name = sb.ToString();
                        sb.Clear();
                        SkipWhitespace(def, ref i);
                        ended = true;
                        break;
                    case '=' when pos == EnumPos.Name:
                    case ' ' when pos == EnumPos.Name:
                    case '/' when pos == EnumPos.Name:
                        currentEntry.Name = sb.ToString();
                        sb.Clear();
                        SkipWhitespace(def, ref i);
                        pos = EnumPos.Equals;
                        break;
                    case ',' when pos == EnumPos.Equals:
                        SkipWhitespace(def, ref i);
                        pos = EnumPos.Comment;
                        break;
                    case '=' when pos == EnumPos.Equals:
                        SkipWhitespace(def, ref i);
                        pos = EnumPos.Value;
                        break;
                    case ',' when pos == EnumPos.Value || pos == EnumPos.Equals:
                    case '/' when pos == EnumPos.Value || pos == EnumPos.Equals:
                    case '}' when pos == EnumPos.Value || pos == EnumPos.Equals:
                    case ' ' when pos == EnumPos.Value:
                        currentEntry.Value = sb.ToString().Trim();
                        sb.Clear();
                        SkipWhitespace(def, ref i);
                        pos = EnumPos.Comment;
                        ended = true;
                        break;
                    default:
                        switch (pos)
                        {
                            case EnumPos.Comment:
                            case EnumPos.Name:
                            case EnumPos.Value:
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

                    entries.Add(currentEntry);
                    currentEntry = new EnumEntry();
                    pos = EnumPos.Name;
                    sb.Clear();
                    SkipWhitespace(def, ref i);
                }
            }

            return entries;
        }

        [TestMethod]
        public void GenerateEnums()
        {
            var header = LoadNvencHeader();

            var exp = new Regex(@"typedef enum _(?<name>[^\s{]+).+?\s*?\r\n}.+?;\s*?\r\n",
                RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline);

            var output = new StringBuilder();
            AppendHeader(output);

            foreach (Match match in exp.Matches(header))
            {
                var def = match.Value;
                var name = match.Groups["name"].Value;

                var entries = ParseEnum(def);
                output.Append($"    /// <summary>{name}</summary>\r\n");
                output.Append($"    public enum {Fixname(name)}\r\n    {{\r\n");

                foreach (var entry in entries)
                {
                    output.Append($"        /// <summary>{entry.Name}");

                    if (!string.IsNullOrWhiteSpace(entry.Comment))
                    {
                        output.Append($": {entry.Comment.Replace("\r\n", "\r\n        /// ")}");
                    }

                    output.Append("</summary>\r\n");
                    output.Append($"        {Fixname(entry.Name, name)}");
                    if (entry.Value != null) output.Append($" = {entry.Value}");
                    output.Append(",\r\n");
                }

                output.Append("    }\r\n\r\n");
            }

            output.Append("}");

            WriteNvencHeaderOutput(output, "Enums.cs");
        }

        [TestMethod]
        public void ParseEnumTests()
        {
            // TODO: Write actual tests.

            const string x = @"typedef enum _NV_ENC_BUFFER_FORMAT
{
    NV_ENC_BUFFER_FORMAT_UNDEFINED                       = 0x00000000,  /**< Undefined buffer format */

    NV_ENC_BUFFER_FORMAT_NV12                            = 0x00000001,  /**< Semi-Planar YUV [Y plane followed by interleaved UV plane] */
    NV_ENC_BUFFER_FORMAT_YV12                            = 0x00000010,  /**< Planar YUV [Y plane followed by V and U planes] */
    NV_ENC_BUFFER_FORMAT_IYUV                            = 0x00000100,  /**< Planar YUV [Y plane followed by U and V planes] */
    NV_ENC_BUFFER_FORMAT_YUV444                          = 0x00001000,  /**< Planar YUV [Y plane followed by U and V planes] */
    NV_ENC_BUFFER_FORMAT_YUV420_10BIT                    = 0x00010000,  /**< 10 bit Semi-Planar YUV [Y plane followed by interleaved UV plane]. Each pixel of size 2 bytes. Most Significant 10 bits contain pixel data. */
    NV_ENC_BUFFER_FORMAT_YUV444_10BIT                    = 0x00100000,  /**< 10 bit Planar YUV444 [Y plane followed by U and V planes]. Each pixel of size 2 bytes. Most Significant 10 bits contain pixel data.  */
    NV_ENC_BUFFER_FORMAT_ARGB                            = 0x01000000,  /**< 8 bit Packed A8R8G8B8. This is a word-ordered format
                                                                             where a pixel is represented by a 32-bit word with B
                                                                             in the lowest 8 bits, G in the next 8 bits, R in the
                                                                             8 bits after that and A in the highest 8 bits. */
    NV_ENC_BUFFER_FORMAT_ARGB10                          = 0x02000000,  /**< 10 bit Packed A2R10G10B10. This is a word-ordered format
                                                                             where a pixel is represented by a 32-bit word with B
                                                                             in the lowest 10 bits, G in the next 10 bits, R in the
                                                                             10 bits after that and A in the highest 2 bits. */
    NV_ENC_BUFFER_FORMAT_AYUV                            = 0x04000000,  /**< 8 bit Packed A8Y8U8V8. This is a word-ordered format
                                                                             where a pixel is represented by a 32-bit word with V
                                                                             in the lowest 8 bits, U in the next 8 bits, Y in the
                                                                             8 bits after that and A in the highest 8 bits. */
    NV_ENC_BUFFER_FORMAT_ABGR                            = 0x10000000,  /**< 8 bit Packed A8B8G8R8. This is a word-ordered format
                                                                             where a pixel is represented by a 32-bit word with R
                                                                             in the lowest 8 bits, G in the next 8 bits, B in the
                                                                             8 bits after that and A in the highest 8 bits. */
    NV_ENC_BUFFER_FORMAT_ABGR10                          = 0x20000000,  /**< 10 bit Packed A2B10G10R10. This is a word-ordered format
                                                                             where a pixel is represented by a 32-bit word with R
                                                                             in the lowest 10 bits, G in the next 10 bits, B in the
                                                                             10 bits after that and A in the highest 2 bits. */
    NV_ENC_BUFFER_FORMAT_U8                              = 0x40000000,  /**< Buffer format representing one-dimensional buffer.
                                                                             This format should be used only when registering the
                                                                             resource as output buffer, which will be used to write
                                                                             the encoded bit stream or H.264 ME only mode output. */
} NV_ENC_BUFFER_FORMAT;";


            //var parsed = ParseEnum(x);

            const string x2 = @"typedef enum _NVENCSTATUS
{
    /**
     * This indicates that API call returned with no errors.
     */
    NV_ENC_SUCCESS,

    /**
     * This indicates that no encode capable devices were detected.
     */
    NV_ENC_ERR_NO_ENCODE_DEVICE,

    /**
     * This indicates that devices pass by the client is not supported.
     */
    NV_ENC_ERR_UNSUPPORTED_DEVICE,

    /**
     * This indicates that the encoder device supplied by the client is not
     * valid.
     */
    NV_ENC_ERR_INVALID_ENCODERDEVICE,

    /**
     * This indicates that device passed to the API call is invalid.
     */
    NV_ENC_ERR_INVALID_DEVICE,

    /**
     * This indicates that device passed to the API call is no longer available and
     * needs to be reinitialized. The clients need to destroy the current encoder
     * session by freeing the allocated input output buffers and destroying the device
     * and create a new encoding session.
     */
    NV_ENC_ERR_DEVICE_NOT_EXIST,

    /**
     * This indicates that one or more of the pointers passed to the API call
     * is invalid.
     */
    NV_ENC_ERR_INVALID_PTR,

    /**
     * This indicates that completion event passed in ::NvEncEncodePicture() call
     * is invalid.
     */
    NV_ENC_ERR_INVALID_EVENT,

    /**
     * This indicates that one or more of the parameter passed to the API call
     * is invalid.
     */
    NV_ENC_ERR_INVALID_PARAM,

    /**
     * This indicates that an API call was made in wrong sequence/order.
     */
    NV_ENC_ERR_INVALID_CALL,

    /**
     * This indicates that the API call failed because it was unable to allocate
     * enough memory to perform the requested operation.
     */
    NV_ENC_ERR_OUT_OF_MEMORY,

    /**
     * This indicates that the encoder has not been initialized with
     * ::NvEncInitializeEncoder() or that initialization has failed.
     * The client cannot allocate input or output buffers or do any encoding
     * related operation before successfully initializing the encoder.
     */
    NV_ENC_ERR_ENCODER_NOT_INITIALIZED,

    /**
     * This indicates that an unsupported parameter was passed by the client.
     */
    NV_ENC_ERR_UNSUPPORTED_PARAM,

    /**
     * This indicates that the ::NvEncLockBitstream() failed to lock the output
     * buffer. This happens when the client makes a non blocking lock call to
     * access the output bitstream by passing NV_ENC_LOCK_BITSTREAM::doNotWait flag.
     * This is not a fatal error and client should retry the same operation after
     * few milliseconds.
     */
    NV_ENC_ERR_LOCK_BUSY,

    /**
     * This indicates that the size of the user buffer passed by the client is
     * insufficient for the requested operation.
     */
    NV_ENC_ERR_NOT_ENOUGH_BUFFER,

    /**
     * This indicates that an invalid struct version was used by the client.
     */
    NV_ENC_ERR_INVALID_VERSION,

    /**
     * This indicates that ::NvEncMapInputResource() API failed to map the client
     * provided input resource.
     */
    NV_ENC_ERR_MAP_FAILED,

    /**
     * This indicates encode driver requires more input buffers to produce an output
     * bitstream. If this error is returned from ::NvEncEncodePicture() API, this
     * is not a fatal error. If the client is encoding with B frames then,
     * ::NvEncEncodePicture() API might be buffering the input frame for re-ordering.
     *
     * A client operating in synchronous mode cannot call ::NvEncLockBitstream()
     * API on the output bitstream buffer if ::NvEncEncodePicture() returned the
     * ::NV_ENC_ERR_NEED_MORE_INPUT error code.
     * The client must continue providing input frames until encode driver returns
     * ::NV_ENC_SUCCESS. After receiving ::NV_ENC_SUCCESS status the client can call
     * ::NvEncLockBitstream() API on the output buffers in the same order in which
     * it has called ::NvEncEncodePicture().
     */
    NV_ENC_ERR_NEED_MORE_INPUT,

    /**
     * This indicates that the HW encoder is busy encoding and is unable to encode
     * the input. The client should call ::NvEncEncodePicture() again after few
     * milliseconds.
     */
    NV_ENC_ERR_ENCODER_BUSY,

    /**
     * This indicates that the completion event passed in ::NvEncEncodePicture()
     * API has not been registered with encoder driver using ::NvEncRegisterAsyncEvent().
     */
    NV_ENC_ERR_EVENT_NOT_REGISTERD,

    /**
     * This indicates that an unknown internal error has occurred.
     */
    NV_ENC_ERR_GENERIC,

    /**
     * This indicates that the client is attempting to use a feature
     * that is not available for the license type for the current system.
     */
    NV_ENC_ERR_INCOMPATIBLE_CLIENT_KEY,

    /**
     * This indicates that the client is attempting to use a feature
     * that is not implemented for the current version.
     */
    NV_ENC_ERR_UNIMPLEMENTED,

    /**
     * This indicates that the ::NvEncRegisterResource API failed to register the resource.
     */
    NV_ENC_ERR_RESOURCE_REGISTER_FAILED,

    /**
     * This indicates that the client is attempting to unregister a resource
     * that has not been successfully registered.
     */
    NV_ENC_ERR_RESOURCE_NOT_REGISTERED,

    /**
     * This indicates that the client is attempting to unmap a resource
     * that has not been successfully mapped.
     */
    NV_ENC_ERR_RESOURCE_NOT_MAPPED,

} NVENCSTATUS;";

            //var parsed2 = ParseEnum(x2);

            const string x3 = @"typedef enum _NV_ENC_LEVEL
{
    NV_ENC_LEVEL_AUTOSELECT         = 0,

    NV_ENC_LEVEL_H264_1             = 10,
    NV_ENC_LEVEL_H264_1b            = 9,
    NV_ENC_LEVEL_H264_11            = 11,
    NV_ENC_LEVEL_H264_12            = 12,
    NV_ENC_LEVEL_H264_13            = 13,
    NV_ENC_LEVEL_H264_2             = 20,
    NV_ENC_LEVEL_H264_21            = 21,
    NV_ENC_LEVEL_H264_22            = 22,
    NV_ENC_LEVEL_H264_3             = 30,
    NV_ENC_LEVEL_H264_31            = 31,
    NV_ENC_LEVEL_H264_32            = 32,
    NV_ENC_LEVEL_H264_4             = 40,
    NV_ENC_LEVEL_H264_41            = 41,
    NV_ENC_LEVEL_H264_42            = 42,
    NV_ENC_LEVEL_H264_5             = 50,
    NV_ENC_LEVEL_H264_51            = 51,
    NV_ENC_LEVEL_H264_52            = 52,


    NV_ENC_LEVEL_HEVC_1             = 30,
    NV_ENC_LEVEL_HEVC_2             = 60,
    NV_ENC_LEVEL_HEVC_21            = 63,
    NV_ENC_LEVEL_HEVC_3             = 90,
    NV_ENC_LEVEL_HEVC_31            = 93,
    NV_ENC_LEVEL_HEVC_4             = 120,
    NV_ENC_LEVEL_HEVC_41            = 123,
    NV_ENC_LEVEL_HEVC_5             = 150,
    NV_ENC_LEVEL_HEVC_51            = 153,
    NV_ENC_LEVEL_HEVC_52            = 156,
    NV_ENC_LEVEL_HEVC_6             = 180,
    NV_ENC_LEVEL_HEVC_61            = 183,
    NV_ENC_LEVEL_HEVC_62            = 186,

    NV_ENC_TIER_HEVC_MAIN           = 0,
    NV_ENC_TIER_HEVC_HIGH           = 1
} NV_ENC_LEVEL;";

            var parsed3 = ParseEnum(x3);
        }
    }
}