# About

NvEncSharp provides a dotnet core and .net framework interface to Nvidia's NvEnc
hardware video encoder (nvEncodeAPI), the CUDA video decoder (nvcuvid), and the
CUDA APIs (nvcuda).

The CUDA API implementation is version 10020. Most (but some) deprecated APIs
are not included.

[Official SDK](https://developer.nvidia.com/nvidia-video-codec-sdk)

There's additional CUDA kernels for frame resizing and decoding inside
`LibCudaLibrary`.

# How to use

* [Add the nuget package.](https://www.nuget.org/packages/Lennox.NvEncSharp)
* [Reference the encoder sample code: Capturing the screen as a video.](src/NvEncSharp.Sample.ScreenCapture/Program.cs)
* [Reference the decoder sample code: Decoding container-free h264 NAL packets to .bmp files and the screen.](src/NvEncSharp.Sample.VideoDecode/Program.cs)

# Welcome contributions

Testing support for linux and x86 would be great. This has only been used and
tested on 64bit Windows.

Large sections of the CUDA API have not been tested. There's likely
transcription errors.

Maintaining forward compatibility with CUDA APIs.