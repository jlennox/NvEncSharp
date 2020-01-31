# About

NvEncSharp provides a dotnet core and .net framework interface to Nvidia's NvEnc
hardware video encoder, the CUDA video decoder, and many CUDA APIs.
[Official SDK](https://developer.nvidia.com/nvidia-video-codec-sdk)

There's additional CUDA kernels for frame resizing and decoding.

# How to use

* [Add the nuget package.](https://www.nuget.org/packages/Lennox.NvEncSharp)
* [Reference the encoder sample code: Capturing the screen as a video.](src/NvEncSharp.Sample.ScreenCapture/Program.cs)
* [Reference the decoder sample code: Capturing the screen as a video.](src/NvEncSharp.Sample.VideoDecode/Program.cs)

# Welcome contributions

Testing support for linux and x86 would be great. This has only been used and
tested on 64bit Windows.