using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// ReSharper disable UnusedMember.Global

namespace Lennox.NvEncSharp
{
    public partial class LibCuda
    {
        private const string _dllpath = "nvcuda.dll";
        private const string _ver = "_v2";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CheckResult(
            CuResult result,
            [CallerMemberName] string callerName = "")
        {
            if (result != CuResult.Success)
            {
                throw new LibNvEncException(
                    callerName, result,
                    GetErrorName(result),
                    GetErrorString(result));
            }
        }

        /// <summary>CUresult cuInit(unsigned int Flags)
        /// Initialize the CUDA driver API.</summary>
        [DllImport(_dllpath, EntryPoint = "cuInit")]
        public static extern CuResult Initialize(uint flags);

        /// <inheritdoc cref="LibCuda.Initialize(uint)"/>
        public static void Initialize()
        {
            CheckResult(Initialize(0));
        }

        /// <summary>Returns the CUDA driver version</summary>
        ///
        /// <remarks>
        /// Returns in \p *driverVersion the version number of the installed CUDA
        /// driver. This function automatically returns ::CUDA_ERROR_INVALID_VALUE if
        /// the <c>driverVersion</c> argument is NULL.
        /// </remarks>
        ///
        /// <param name="driverVersion">Returns the CUDA driver version</param>
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \notefnerr
        ///
        /// \sa
        /// ::cudaDriverGetVersion,
        /// ::cudaRuntimeGetVersion
        /// CUresult CUDAAPI cuDriverGetVersion(int *driverVersion);
        [DllImport(_dllpath, EntryPoint = "cuDriverGetVersion")]
        public static extern CuResult DriverGetVersion(out int driverVersion);

        /// <inheritdoc cref="LibCuda.DriverGetVersion(out int)"/>
        public static int DriverGetVersion()
        {
            CheckResult(DriverGetVersion(out var version));
            return version;
        }

        /// <summary>Gets the string description of an error code</summary>
        ///
        /// <remarks>
        /// Sets \p *pStr to the address of a NULL-terminated string description
        /// of the error code <c>error</c>.
        /// If the error code is not recognized, ::CUDA_ERROR_INVALID_VALUE
        /// will be returned and \p *pStr will be set to the NULL address.
        /// </remarks>
        ///
        /// <param name="error">Error code to convert to string</param>
        /// <param name="str">Address of the string pointer.</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa
        /// ::CUresult,
        /// ::cudaGetErrorString
        /// CUresult CUDAAPI cuGetErrorString(CUresult error, const char **pStr);
        [DllImport(_dllpath, EntryPoint = "cuGetErrorString")]
        public static extern CuResult GetErrorString(CuResult error, out IntPtr str);

        /// <inheritdoc cref="LibCuda.GetErrorString(CuResult, out IntPtr)"/>
        public static string GetErrorString(CuResult error)
        {
            CheckResult(GetErrorString(error, out var str));
            return str == IntPtr.Zero
                ? "Unknown error"
                : Marshal.PtrToStringAnsi(str);
        }

        /// <summary>Gets the string representation of an error code enum name</summary>
        ///
        /// <remarks>
        /// Sets \p *pStr to the address of a NULL-terminated string representation
        /// of the name of the enum error code <c>error</c>.
        /// If the error code is not recognized, ::CUDA_ERROR_INVALID_VALUE
        /// will be returned and \p *pStr will be set to the NULL address.
        /// </remarks>
        ///
        /// <param name="error">Error code to convert to string</param>
        /// <param name="str">Address of the string pointer.</param>
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \sa
        /// ::CUresult,
        /// ::cudaGetErrorName
        /// CUresult CUDAAPI cuGetErrorName(CUresult error, const char **pStr);
        [DllImport(_dllpath, EntryPoint = "cuGetErrorName")]
        public static extern CuResult GetErrorName(CuResult error, out IntPtr str);

        /// <inheritdoc cref="LibCuda.GetErrorName(CuResult, out IntPtr)"/>
        public static string GetErrorName(CuResult error)
        {
            CheckResult(GetErrorName(error, out var str));
            return str == IntPtr.Zero
                ? "Unknown error"
                : Marshal.PtrToStringAnsi(str);
        }
    }
}