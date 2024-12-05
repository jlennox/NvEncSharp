# About

NvEncSharp provides a dotnet core and .net framework interface to Nvidia's NvEnc
hardware video encoder (nvEncodeAPI), the CUDA video decoder (nvcuvid), and the
CUDA APIs (nvcuda).

The CUDA API implementation is version 10020. Most (but some) deprecated APIs are not included.

[Official SDK](https://developer.nvidia.com/nvidia-video-codec-sdk)

There's additional CUDA kernels for frame resizing and decoding inside `LibCudaLibrary`.

# How to use

- [Add the nuget package.](https://www.nuget.org/packages/Lennox.NvEncSharp)
- [Reference the encoder sample code: Capturing the screen as a video.](src/NvEncSharp.Sample.ScreenCapture/Program.cs)
- [Reference the decoder sample code: Decoding container-free h264 NAL packets to .bmp files and the screen.](src/NvEncSharp.Sample.VideoDecode/Program.cs)

# Welcome contributions

Testing support for linux and x86 would be great. This has only been used and tested on 64bit Windows.

Large sections of the CUDA API have not been tested. There's likely transcription errors.

Maintaining forward compatibility with CUDA APIs.

# Building
Sorry, this is way more a mess than it should be.

Note: The CUDA SDK is _only_ needed for the sample projects. And is only needed for GPU based color space conversions.

- Install CUDA SDK 12.6, see notes below. [CUDA SDK site](https://developer.nvidia.com/cuda-downloads).
- Select Visual Studio integration in the installer.

This originally targeted [10.2 CUDA SDK](https://developer.nvidia.com/cuda-10.2-download-archive), and the API definitions still match that, but has been updated to target 12.6. I have not found a direct download link for 12.6.

The [12.6 documentation](https://docs.nvidia.com/cuda/archive/12.6.0/cuda-installation-guide-microsoft-windows/index.html) suggests the archived versions are now offered using conda but I have not tried this.

Updating the SDK version may be easier than finding the depricated version.

# Updating/changing the CUDA SDK
- Modify `CUDA_VERSION` inside `NvEncSharp/src/NvEncSharp.Cuda.Library/NvEncSharp.Cuda.Library.props` to your target version.
- If it compiles it compiles. If not, it's possible that the CUDA SDK has introduced breaking changes that need to be addressed.