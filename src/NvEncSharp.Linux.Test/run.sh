#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")/../.."
out="$PWD/artifacts/linux-test"
mkdir -p "$out/mock"
g++ -std=c++17 src/NvEncSharp.Test/NativeSizes.cpp -o "$out/nvenc-abi"
g++ -std=c++17 -Isrc/NvEncSharp.Test src/NvEncSharp.Linux.Test/DecoderAbi.cpp -o "$out/decoder-abi"
"$out/nvenc-abi" > "$out/abi.txt"
"$out/decoder-abi" >> "$out/abi.txt"
g++ -std=c++17 -shared -fPIC -Isrc/NvEncSharp.Test src/NvEncSharp.Linux.Test/MockDriver.cpp -o "$out/mock/libcuda.so.1"
for name in libnvcuvid.so.1 libnvidia-encode.so.1 libNvEncSharp.Cuda.Library.so; do
    ln -sf libcuda.so.1 "$out/mock/$name"
done
export LD_LIBRARY_PATH="$out/mock${LD_LIBRARY_PATH:+:$LD_LIBRARY_PATH}"
if [[ $# -gt 0 ]]; then
    # Allows a self-contained test executable built on another machine.
    "$1" "$out/abi.txt"
else
    dotnet run --project src/NvEncSharp.Linux.Test -c Release -p:BuildCudaLibrary=false -p:GeneratePackageOnBuild=false -- "$out/abi.txt"
fi
