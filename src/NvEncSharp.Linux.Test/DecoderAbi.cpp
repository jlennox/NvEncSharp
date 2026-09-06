#include "cuviddec.h"
#include "../NvEncSharp.Cuda.Library/VideoSDK/include/nvcuvid.h"
#include <cstddef>
#include <iostream>

#define FIELD(managed, native) std::cout << "LinuxDecodeCreateInfo." #managed " " << offsetof(CUVIDDECODECREATEINFO, native) << '\n'
int main() {
    std::cout << "LinuxDecodeCreateInfo.size " << sizeof(CUVIDDECODECREATEINFO) << '\n';
    FIELD(Width, ulWidth); FIELD(Height, ulHeight); FIELD(NumDecodeSurfaces, ulNumDecodeSurfaces);
    FIELD(CodecType, CodecType); FIELD(ChromaFormat, ChromaFormat); FIELD(CreationFlags, ulCreationFlags);
    FIELD(BitDepthMinus8, bitDepthMinus8); FIELD(IntraDecodeOnly, ulIntraDecodeOnly);
    FIELD(MaxWidth, ulMaxWidth); FIELD(MaxHeight, ulMaxHeight); FIELD(_reserved1, Reserved1);
    FIELD(DisplayArea, display_area); FIELD(OutputFormat, OutputFormat); FIELD(DeinterlaceMode, DeinterlaceMode);
    FIELD(TargetWidth, ulTargetWidth); FIELD(TargetHeight, ulTargetHeight);
    FIELD(NumOutputSurfaces, ulNumOutputSurfaces); FIELD(VideoLock, vidLock);
    FIELD(TargetRect, target_rect); FIELD(_reserved2, Reserved2);
    std::cout << "CuVideoDecodeCaps " << sizeof(CUVIDDECODECAPS) << '\n';
    std::cout << "CuVideoProcParams " << sizeof(CUVIDPROCPARAMS) << '\n';
    std::cout << "CuVideoPicParams " << sizeof(CUVIDPICPARAMS) << '\n';
    std::cout << "CuVideoReconfigureDecoderInfo " << sizeof(CUVIDRECONFIGUREDECODERINFO) << '\n';
    std::cout << "CuVideoGetDecodeStatus " << sizeof(CUVIDGETDECODESTATUS) << '\n';
    std::cout << "CuVideoParserParams " << sizeof(CUVIDPARSERPARAMS) << '\n';
    std::cout << "CuVideoParseDisplayInfo " << sizeof(CUVIDPARSERDISPINFO) << '\n';
    std::cout << "CuVideoFormat " << sizeof(CUVIDEOFORMAT) << '\n';
    std::cout << "CuVideoFormatEx " << sizeof(CUVIDEOFORMATEX) << '\n';
    std::cout << "LinuxSourceDataPacket.size " << sizeof(CUVIDSOURCEDATAPACKET) << '\n';
    std::cout << "LinuxSourceDataPacket.Flags " << offsetof(CUVIDSOURCEDATAPACKET, flags) << '\n';
    std::cout << "LinuxSourceDataPacket.PayloadSize " << offsetof(CUVIDSOURCEDATAPACKET, payload_size) << '\n';
    std::cout << "LinuxSourceDataPacket.Payload " << offsetof(CUVIDSOURCEDATAPACKET, payload) << '\n';
    std::cout << "LinuxSourceDataPacket.Timestamp " << offsetof(CUVIDSOURCEDATAPACKET, timestamp) << '\n';
}
