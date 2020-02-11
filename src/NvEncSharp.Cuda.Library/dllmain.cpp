// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include "ColorSpace.h"

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

template <class COLOR24>
void Nv12ToColor24(uint8_t* dpNv12, int nNv12Pitch, uint8_t* dpBgra, int nBgraPitch, int nWidth, int nHeight, int iMatrix = 0);
template <class COLOR32>
void Nv12ToColor32(uint8_t* dpNv12, int nNv12Pitch, uint8_t* dpBgra, int nBgraPitch, int nWidth, int nHeight, int iMatrix = 0);
template <class COLOR64>
void Nv12ToColor64(uint8_t* dpNv12, int nNv12Pitch, uint8_t* dpBgra, int nBgraPitch, int nWidth, int nHeight, int iMatrix = 0);

template <class COLOR32>
void P016ToColor32(uint8_t* dpP016, int nP016Pitch, uint8_t* dpBgra, int nBgraPitch, int nWidth, int nHeight, int iMatrix = 4);
template <class COLOR64>
void P016ToColor64(uint8_t* dpP016, int nP016Pitch, uint8_t* dpBgra, int nBgraPitch, int nWidth, int nHeight, int iMatrix = 4);

template <class COLOR32>
void YUV444ToColor32(uint8_t* dpYUV444, int nPitch, uint8_t* dpBgra, int nBgraPitch, int nWidth, int nHeight, int iMatrix = 0);
template <class COLOR64>
void YUV444ToColor64(uint8_t* dpYUV444, int nPitch, uint8_t* dpBgra, int nBgraPitch, int nWidth, int nHeight, int iMatrix = 0);

template <class COLOR32>
void YUV444P16ToColor32(uint8_t* dpYUV444, int nPitch, uint8_t* dpBgra, int nBgraPitch, int nWidth, int nHeight, int iMatrix = 4);
template <class COLOR64>
void YUV444P16ToColor64(uint8_t* dpYUV444, int nPitch, uint8_t* dpBgra, int nBgraPitch, int nWidth, int nHeight, int iMatrix = 4);

template <class COLOR32>
void Nv12ToColorPlanar(uint8_t* dpNv12, int nNv12Pitch, uint8_t* dpBgrp, int nBgrpPitch, int nWidth, int nHeight, int iMatrix = 0);
template <class COLOR32>
void P016ToColorPlanar(uint8_t* dpP016, int nP016Pitch, uint8_t* dpBgrp, int nBgrpPitch, int nWidth, int nHeight, int iMatrix = 4);

template <class COLOR32>
void YUV444ToColorPlanar(uint8_t* dpYUV444, int nPitch, uint8_t* dpBgrp, int nBgrpPitch, int nWidth, int nHeight, int iMatrix = 0);
template <class COLOR32>
void YUV444P16ToColorPlanar(uint8_t* dpYUV444, int nPitch, uint8_t* dpBgrp, int nBgrpPitch, int nWidth, int nHeight, int iMatrix = 4);

void Bgra64ToP016(uint8_t* dpBgra, int nBgraPitch, uint8_t* dpP016, int nP016Pitch, int nWidth, int nHeight, int iMatrix = 4);

extern "C" void __declspec(dllexport) Nv12ToBGRA32(uint8_t * dpNv12, int nNv12Pitch, uint8_t * dpBgra, int nBgraPitch, int nWidth, int nHeight, int iMatrix)
{
    Nv12ToColor32<BGRA32>(dpNv12, nNv12Pitch, dpBgra, nBgraPitch, nWidth, nHeight, iMatrix);
}

extern "C" void __declspec(dllexport) Nv12ToRGBA32(uint8_t * dpNv12, int nNv12Pitch, uint8_t * dpRgba, int nRgbaPitch, int nWidth, int nHeight, int iMatrix)
{
    Nv12ToColor32<RGBA32>(dpNv12, nNv12Pitch, dpRgba, nRgbaPitch, nWidth, nHeight, iMatrix);
}

extern "C" void __declspec(dllexport) Nv12ToARGB32(uint8_t * dpNv12, int nNv12Pitch, uint8_t * dpRgba, int nRgbaPitch, int nWidth, int nHeight, int iMatrix)
{
    Nv12ToColor32<ARGB32>(dpNv12, nNv12Pitch, dpRgba, nRgbaPitch, nWidth, nHeight, iMatrix);
}

extern "C" void __declspec(dllexport) Nv12ToRGB24(uint8_t * dpNv12, int nNv12Pitch, uint8_t * dpRgb, int nRgbPitch, int nWidth, int nHeight, int iMatrix)
{
    Nv12ToColor24<RGB24>(dpNv12, nNv12Pitch, dpRgb, nRgbPitch, nWidth, nHeight, iMatrix);
}

// ----

void ResizeNv12Fn(unsigned char* dpDstNv12, int nDstPitch, int nDstWidth, int nDstHeight, unsigned char* dpSrcNv12, int nSrcPitch, int nSrcWidth, int nSrcHeight, unsigned char* dpDstNv12UV = nullptr);
void ResizeP016(unsigned char* dpDstP016, int nDstPitch, int nDstWidth, int nDstHeight, unsigned char* dpSrcP016, int nSrcPitch, int nSrcWidth, int nSrcHeight, unsigned char* dpDstP016UV = nullptr);

void ScaleYUV420(unsigned char* dpDstY, unsigned char* dpDstU, unsigned char* dpDstV, int nDstPitch, int nDstChromaPitch, int nDstWidth, int nDstHeight,
    unsigned char* dpSrcY, unsigned char* dpSrcU, unsigned char* dpSrcV, int nSrcPitch, int nSrcChromaPitch, int nSrcWidth, int nSrcHeight, bool bSemiplanar);

extern "C" void __declspec(dllexport) ResizeNv12(unsigned char* dpDstNv12, int nDstPitch, int nDstWidth, int nDstHeight, unsigned char* dpSrcNv12, int nSrcPitch, int nSrcWidth, int nSrcHeight, unsigned char* dpDstNv12UV)
{
    ResizeNv12Fn(dpDstNv12, nDstPitch, nDstWidth, nDstHeight, dpSrcNv12, nSrcPitch, nSrcWidth, nSrcHeight, dpDstNv12UV);
}

// ----

void ComputeCRC(uint8_t* pBuffer, uint32_t* crcValue, CUstream_st* outputCUStream);

// ----


