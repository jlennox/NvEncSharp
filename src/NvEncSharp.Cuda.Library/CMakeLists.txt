cmake_minimum_required(VERSION 3.18)
project(NvEncSharpCudaLibrary LANGUAGES CXX CUDA)

if(NOT CMAKE_SYSTEM_NAME MATCHES "^(Linux|Windows)$" OR
   NOT CMAKE_SYSTEM_PROCESSOR MATCHES "^(x86_64|amd64|AMD64)$" OR
   NOT CMAKE_SIZEOF_VOID_P EQUAL 8)
    message(FATAL_ERROR "This build currently targets Windows x64 and Linux x64.")
endif()

find_package(CUDAToolkit REQUIRED)
add_library(NvEncSharp.Cuda.Library SHARED dllmain.cpp ColorSpace.cu Resize.cu)
target_compile_features(NvEncSharp.Cuda.Library PRIVATE cxx_std_17 cuda_std_17)
target_link_libraries(NvEncSharp.Cuda.Library PRIVATE CUDA::cudart_static)
if(WIN32)
    target_compile_definitions(NvEncSharp.Cuda.Library PRIVATE _CRT_SECURE_NO_WARNINGS)
endif()
set_target_properties(NvEncSharp.Cuda.Library PROPERTIES
    POSITION_INDEPENDENT_CODE ON
    CUDA_RUNTIME_LIBRARY Static)

# No driver or Video Codec SDK stub libraries are needed by these kernels.
# Install directly into the NuGet runtime asset tree before dotnet pack.
install(TARGETS NvEncSharp.Cuda.Library
    LIBRARY DESTINATION runtimes/linux-x64/native
    RUNTIME DESTINATION runtimes/win-x64/native)
