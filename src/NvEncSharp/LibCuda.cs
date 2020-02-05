using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// ReSharper disable UnusedMember.Global

namespace Lennox.NvEncSharp
{
    public unsafe partial class LibCuda
    {
        public const int ApiVerison = 10020;

        private const string _dllpath = "nvcuda.dll";
        private const string _ver = "_v2";

        /// <summary>CUDA stream callback</summary>
        /// <param name="hStream">The stream the callback was added to, as passed to ::cuStreamAddCallback.  May be NULL.</param>
        /// <param name="status">::CUDA_SUCCESS or any persistent error on the stream.</param>
        /// <param name="userData">User parameter provided at registration.</param>
        public delegate void CuStreamCallback(CuStream hStream, CuResult status, IntPtr userData);

        /// <summary>Block size to per-block dynamic shared memory mapping for a certain kernel.</summary>
        /// <param name="blockSize">blockSize Block size of the kernel</param>
        /// <returns>The dynamic shared memory needed by a block.</returns>
        public delegate IntPtr CuOccupancyB2DSize(int blockSize);

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
        /// Returns in *<paramref name="driverVersion"/> the version number of the installed CUDA
        /// driver. This function automatically returns ::CUDA_ERROR_INVALID_VALUE if
        /// the <paramref name="driverVersion"/> argument is NULL.
        /// </remarks>
        ///
        /// <param name="driverVersion">Returns the CUDA driver version</param>
        ///
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
        /// Sets *<paramref name="str"/> to the address of a NULL-terminated string description
        /// of the error code <paramref name="error"/>.
        /// If the error code is not recognized, ::CUDA_ERROR_INVALID_VALUE
        /// will be returned and *<paramref name="str"/> will be set to the NULL address.
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
        /// Sets *<paramref name="str"/> to the address of a NULL-terminated string representation
        /// of the name of the enum error code <c>error</c>.
        /// If the error code is not recognized, ::CUDA_ERROR_INVALID_VALUE
        /// will be returned and *<paramref name="str"/> will be set to the NULL address.
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

        /// CUresult CUDAAPI cuGetExportTable(const void **ppExportTable, const CUuuid *pExportTableId);
        [DllImport(_dllpath, EntryPoint = "cuGetExportTable")]
        public static extern CuResult GetExportTable(IntPtr* ppExportTable, Guid* pExportTableId);

        /// <summary>
        /// Exception if <paramref name="result"/> is not <c>CuResult.Success</c>.
        /// </summary>
        /// <param name="result">The CuResult from a LibCuda API call to check.</param>
        /// <param name="callerName"></param>
        /// <exception cref="LibNvEncException">Thrown if <paramref name="result"/> is not <c>CuResult.Success</c>.</exception>
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
    }
}