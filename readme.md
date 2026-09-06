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

Windows x64 and Linux x64 are supported build targets. Linux requires .NET 8 or
later; the older framework targets retain their Windows behavior. Linux ABI and
library loading checks run without a GPU. Hardware encoding, decoding, and CUDA
kernel execution on Linux still need validation on an NVIDIA system.

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

# Linux x64

The encoder bindings still target Video Codec SDK **12.2**. Install an NVIDIA
driver that supports that API and exposes `libcuda.so.1`, `libnvcuvid.so.1`, and
`libnvidia-encode.so.1`. These are driver libraries, not files supplied by this
package. Containers must expose both the compute and video driver capabilities.

Use a .NET 8 or later application. The package selects the Linux native helper
from `runtimes/linux-x64/native`; the same managed API resolves NVIDIA's Linux
libraries automatically. ARM64 and macOS are not supported by this first Linux
implementation.

Linux encoding uses a CUDA device/context and synchronous output:
`EnableEncodeAsync = 0`, no completion event, and `DoNotWait = 0` when locking
the bitstream. Direct3D interop, the legacy `CuVideoSource` file-source APIs,
and the existing desktop sample applications
remain Windows-specific. See the [NVIDIA encoding guide](https://docs.nvidia.com/video-technologies/video-codec-sdk/12.2/pdf/NVENC_VideoEncoder_API_ProgGuide.pdf).

## Build the native helper

Install CUDA Toolkit 12.6, a compatible GCC/G++ compiler, and CMake 3.18 or later.
CUDA is needed to compile the color conversion/resizing helper, not to build the
managed bindings. The helper statically links the CUDA runtime; users still need
the NVIDIA driver. No GPU is needed to compile it.

From the repository root:

```sh
cmake -S src/NvEncSharp.Cuda.Library -B artifacts/linux-cuda \
  -DCMAKE_BUILD_TYPE=Release \
  '-DCMAKE_CUDA_ARCHITECTURES=50;61;75;86;89;90'
cmake --build artifacts/linux-cuda --parallel 2
cmake --install artifacts/linux-cuda --prefix "$PWD/src/NvEncSharp/lib"
dotnet pack src/NvEncSharp/NvEncSharp.csproj -c Release -o artifacts \
  -p:BuildCudaLibrary=false -p:GeneratePackageOnBuild=false \
  -p:RequireLinuxNativeLibrary=true
```

Set `CMAKE_CUDA_COMPILER` if `nvcc` is not on PATH. Adjust
`CMAKE_CUDA_ARCHITECTURES` if using a different CUDA toolkit or targeting specific
GPUs. CI builds both native helpers from source with CUDA 12.6: Windows x64 on
Windows Server 2022 and Linux x64 on Ubuntu 22.04. The packaging job downloads
both build artifacts, so it does not use the checked-in Windows DLL. Building
from source without installing the
Linux helper produces a package with driver bindings only on Linux; methods in
`LibCudaLibrary` require that helper.

To build only the managed library:

```sh
dotnet build src/NvEncSharp/NvEncSharp.csproj -c Release -f net8.0 \
  -p:BuildCudaLibrary=false -p:GeneratePackageOnBuild=false
```

## Check Linux interop without a GPU

With .NET 8 SDK and G++ installed:

```sh
bash src/NvEncSharp.Linux.Test/run.sh
```

This compiles native probes against the checked-in headers, compares NVENC
structure and delegate sizes and NVDEC layouts, and tests library resolution,
decoder parameter translation, and 64-bit frame pointers using mock libraries.
The mocks stay under `artifacts/linux-test/mock` and must never be distributed
as driver libraries. These checks do not test GPU operation.
