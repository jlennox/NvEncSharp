// Test-only driver substitute. Never package or install this as an NVIDIA driver.
#include "nvEncodeAPI.h"
#include "cuviddec.h"
#include "../NvEncSharp.Cuda.Library/VideoSDK/include/nvcuvid.h"
#include <cstdint>

extern "C" {
CUresult cuInit(unsigned int flags) { return flags == 0 ? CUDA_SUCCESS : CUDA_ERROR_INVALID_VALUE; }

NVENCSTATUS NvEncodeAPIGetMaxSupportedVersion(uint32_t* version) {
    *version = (12 << 4) | 2;
    return NV_ENC_SUCCESS;
}
NVENCSTATUS NvEncodeAPICreateInstance(NV_ENCODE_API_FUNCTION_LIST* functions) {
    return functions->version == NV_ENCODE_API_FUNCTION_LIST_VER ? NV_ENC_SUCCESS : NV_ENC_ERR_INVALID_VERSION;
}

CUresult cuvidCreateDecoder(CUvideodecoder* decoder, CUVIDDECODECREATEINFO* p) {
    if (p->ulWidth != 1920 || p->ulHeight != 1080 || p->ulNumDecodeSurfaces != 8 ||
        p->CodecType != cudaVideoCodec_H264 || p->ChromaFormat != cudaVideoChromaFormat_420 ||
        p->ulCreationFlags != 1 || p->bitDepthMinus8 != 2 || p->ulIntraDecodeOnly != 1 ||
        p->ulMaxWidth != 3840 || p->ulMaxHeight != 2160 || p->Reserved1 != 0 ||
        p->ulTargetWidth != 1280 || p->ulTargetHeight != 720 || p->ulNumOutputSurfaces != 3 ||
        reinterpret_cast<uintptr_t>(p->vidLock) != 0x1234)
        return CUDA_ERROR_INVALID_VALUE;
    for (auto value : p->Reserved2) if (value != 0) return CUDA_ERROR_INVALID_VALUE;
    *decoder = reinterpret_cast<CUvideodecoder>(0x5678);
    return CUDA_SUCCESS;
}
CUresult cuvidMapVideoFrame64(CUvideodecoder decoder, int index, unsigned long long* ptr,
                            unsigned int* pitch, CUVIDPROCPARAMS*) {
    if (reinterpret_cast<uintptr_t>(decoder) != 0x5678 || index != 7) return CUDA_ERROR_INVALID_VALUE;
    *ptr = 0x123456789abcdef0ULL;
    *pitch = 2048;
    return CUDA_SUCCESS;
}
CUresult cuvidUnmapVideoFrame64(CUvideodecoder, unsigned long long ptr) {
    return ptr == 0x123456789abcdef0ULL ? CUDA_SUCCESS : CUDA_ERROR_INVALID_VALUE;
}
CUresult cuvidParseVideoData(CUvideoparser, CUVIDSOURCEDATAPACKET* p) {
    return p->flags == 2 && p->payload_size == 1 && p->payload[0] == 42 && p->timestamp == -1234567890123LL
        ? CUDA_SUCCESS : CUDA_ERROR_INVALID_VALUE;
}
void Nv12ToBGRA32(void*, int, uint32_t* dest, int, int, int, int) { *dest = 0x12345678; }
}
