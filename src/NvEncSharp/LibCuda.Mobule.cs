using System;
using System.Runtime.InteropServices;

namespace Lennox.NvEncSharp
{
    public unsafe partial class LibCuda
    {
        /// <summary>Loads a compute module
        ///
        /// Takes a filename <paramref name="fname"/> and loads the corresponding module <paramref name="module"/> into
        /// the current context. The CUDA driver API does not attempt to lazily
        /// allocate the resources needed by a module; if the memory for functions and
        /// data (constant and global) needed by the module cannot be allocated,
        /// ::cuModuleLoad() fails. The file should be a \e cubin file as output by
        /// \b nvcc, or a \e PTX file either as output by \b nvcc or handwritten, or
        /// a \e fatbin file as output by \b nvcc from toolchain 4.0 or later.</summary>
        ///
        /// <param name="module">Returned module</param>
        /// <param name="fname">Filename of module to load</param>
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_INVALID_PTX,
        /// ::CUDA_ERROR_NOT_FOUND,
        /// ::CUDA_ERROR_OUT_OF_MEMORY,
        /// ::CUDA_ERROR_FILE_NOT_FOUND,
        /// ::CUDA_ERROR_NO_BINARY_FOR_GPU,
        /// ::CUDA_ERROR_SHARED_OBJECT_SYMBOL_NOT_FOUND,
        /// ::CUDA_ERROR_SHARED_OBJECT_INIT_FAILED,
        /// ::CUDA_ERROR_JIT_COMPILER_NOT_FOUND
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuModuleGetFunction,
        /// ::cuModuleGetGlobal,
        /// ::cuModuleGetTexRef,
        /// ::cuModuleLoadData,
        /// ::cuModuleLoadDataEx,
        /// ::cuModuleLoadFatBinary,
        /// ::cuModuleUnload
        /// CUresult CUDAAPI cuModuleLoad(CUmodule *module, const char *fname);
        [DllImport(_dllpath, EntryPoint = "cuModuleLoad", CharSet = CharSet.Ansi)]
        public static extern CuResult ModuleLoad(out CuModule module, string fname);

        /// <summary>Load a module's data
        ///
        /// Takes a pointer <paramref name="image"/> and loads the corresponding module <paramref name="module"/> into
        /// the current context. The pointer may be obtained by mapping a \e cubin or
        /// \e PTX or \e fatbin file, passing a \e cubin or \e PTX or \e fatbin file
        /// as a NULL-terminated text string, or incorporating a \e cubin or \e fatbin
        /// object into the executable resources and using operating system calls such
        /// as Windows \c FindResource() to obtain the pointer.</summary>
        ///
        /// <param name="module">Returned module</param>
        /// <param name="image">Module data to load</param>
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_INVALID_PTX,
        /// ::CUDA_ERROR_OUT_OF_MEMORY,
        /// ::CUDA_ERROR_NO_BINARY_FOR_GPU,
        /// ::CUDA_ERROR_SHARED_OBJECT_SYMBOL_NOT_FOUND,
        /// ::CUDA_ERROR_SHARED_OBJECT_INIT_FAILED,
        /// ::CUDA_ERROR_JIT_COMPILER_NOT_FOUND
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuModuleGetFunction,
        /// ::cuModuleGetGlobal,
        /// ::cuModuleGetTexRef,
        /// ::cuModuleLoad,
        /// ::cuModuleLoadDataEx,
        /// ::cuModuleLoadFatBinary,
        /// ::cuModuleUnload
        /// CUresult CUDAAPI cuModuleLoadData(CUmodule *module, const void *image);
        [DllImport(_dllpath, EntryPoint = "cuModuleLoadData")]
        public static extern CuResult ModuleLoadData(out CuModule module, IntPtr image);

        /// <summary>Load a module's data with options
        ///
        /// Takes a pointer <paramref name="image"/> and loads the corresponding module <paramref name="module"/> into
        /// the current context. The pointer may be obtained by mapping a \e cubin or
        /// \e PTX or \e fatbin file, passing a \e cubin or \e PTX or \e fatbin file
        /// as a NULL-terminated text string, or incorporating a \e cubin or \e fatbin
        /// object into the executable resources and using operating system calls such
        /// as Windows \c FindResource() to obtain the pointer. Options are passed as
        /// an array via <paramref name="options"/> and any corresponding parameters are passed in
        /// <paramref name="optionValues."/> The number of total options is supplied via <paramref name="numOptions"/>.
        /// Any outputs will be returned via <paramref name="optionValues."/></summary>
        ///
        /// <param name="module">Returned module</param>
        /// <param name="image">Module data to load</param>
        /// <param name="numOptions">Number of options</param>
        /// <param name="options">Options for JIT</param>
        /// <param name="optionValues">Option values for JIT</param>
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_INVALID_PTX,
        /// ::CUDA_ERROR_OUT_OF_MEMORY,
        /// ::CUDA_ERROR_NO_BINARY_FOR_GPU,
        /// ::CUDA_ERROR_SHARED_OBJECT_SYMBOL_NOT_FOUND,
        /// ::CUDA_ERROR_SHARED_OBJECT_INIT_FAILED,
        /// ::CUDA_ERROR_JIT_COMPILER_NOT_FOUND
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuModuleGetFunction,
        /// ::cuModuleGetGlobal,
        /// ::cuModuleGetTexRef,
        /// ::cuModuleLoad,
        /// ::cuModuleLoadData,
        /// ::cuModuleLoadFatBinary,
        /// ::cuModuleUnload
        /// CUresult CUDAAPI cuModuleLoadDataEx(CUmodule *module, const void *image, unsigned int numOptions, CUjit_option *options, void **optionValues);
        [DllImport(_dllpath, EntryPoint = "cuModuleLoadDataEx")]
        public static extern CuResult ModuleLoadDataEx(out CuModule module, IntPtr image, int numOptions, JitOption *options, void** optionValues);

        /// <summary>Load a module's data
        ///
        /// Takes a pointer <paramref name="fatCubin"/> and loads the corresponding module <paramref name="module"/>
        /// into the current context. The pointer represents a <i>fat binary</i> object,
        /// which is a collection of different \e cubin and/or \e PTX files, all
        /// representing the same device code, but compiled and optimized for different
        /// architectures.
        ///
        /// Prior to CUDA 4.0, there was no documented API for constructing and using
        /// fat binary objects by programmers.  Starting with CUDA 4.0, fat binary
        /// objects can be constructed by providing the <i>-fatbin option</i> to \b nvcc.
        /// More information can be found in the \b nvcc document.</summary>
        ///
        /// <param name="module">Returned module</param>
        /// <param name="fatCubin">Fat binary to load</param>
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_INVALID_PTX,
        /// ::CUDA_ERROR_NOT_FOUND,
        /// ::CUDA_ERROR_OUT_OF_MEMORY,
        /// ::CUDA_ERROR_NO_BINARY_FOR_GPU,
        /// ::CUDA_ERROR_SHARED_OBJECT_SYMBOL_NOT_FOUND,
        /// ::CUDA_ERROR_SHARED_OBJECT_INIT_FAILED,
        /// ::CUDA_ERROR_JIT_COMPILER_NOT_FOUND
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuModuleGetFunction,
        /// ::cuModuleGetGlobal,
        /// ::cuModuleGetTexRef,
        /// ::cuModuleLoad,
        /// ::cuModuleLoadData,
        /// ::cuModuleLoadDataEx,
        /// ::cuModuleUnload
        /// CUresult CUDAAPI cuModuleLoadFatBinary(CUmodule *module, const void *fatCubin);
        [DllImport(_dllpath, EntryPoint = "cuModuleLoadFatBinary")]
        public static extern CuResult ModuleLoadFatBinary(out CuModule module, IntPtr fatCubin);

        /// <summary>Unloads a module
        ///
        /// Unloads a module <paramref name="hmod"/> from the current context.</summary>
        ///
        /// <param name="hmod">Module to unload</param>
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuModuleGetFunction,
        /// ::cuModuleGetGlobal,
        /// ::cuModuleGetTexRef,
        /// ::cuModuleLoad,
        /// ::cuModuleLoadData,
        /// ::cuModuleLoadDataEx,
        /// ::cuModuleLoadFatBinary
        /// CUresult CUDAAPI cuModuleUnload(CUmodule hmod);
        [DllImport(_dllpath, EntryPoint = "cuModuleUnload")]
        public static extern CuResult ModuleUnload(CuModule hmod);

        /// <summary>Returns a function handle
        ///
        /// Returns in <paramref name="*hfunc"/> the handle of the function of name <paramref name="name"/> located in
        /// module <paramref name="hmod."/> If no function of that name exists, ::cuModuleGetFunction()
        /// returns ::CUDA_ERROR_NOT_FOUND.</summary>
        ///
        /// <param name="hfunc">Returned function handle</param>
        /// <param name="hmod">Module to retrieve function from</param>
        /// <param name="name">Name of function to retrieve</param>
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_NOT_FOUND
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuModuleGetGlobal,
        /// ::cuModuleGetTexRef,
        /// ::cuModuleLoad,
        /// ::cuModuleLoadData,
        /// ::cuModuleLoadDataEx,
        /// ::cuModuleLoadFatBinary,
        /// ::cuModuleUnload
        /// CUresult CUDAAPI cuModuleGetFunction(CUfunction *hfunc, CUmodule hmod, const char *name);
        [DllImport(_dllpath, EntryPoint = "cuModuleGetFunction", CharSet = CharSet.Ansi)]
        public static extern CuResult ModuleGetFunction(out CuFunction hfunc, CuModule hmod, string name);

        /// <summary>Returns a global pointer from a module
        ///
        /// Returns in <paramref name="*dptr"/> and <paramref name="*bytes"/> the base pointer and size of the
        /// global of name <paramref name="name"/> located in module <paramref name="hmod."/> If no variable of that name
        /// exists, ::cuModuleGetGlobal() returns ::CUDA_ERROR_NOT_FOUND. Both
        /// parameters <paramref name="dptr"/> and <paramref name="bytes"/> are optional. If one of them is
        /// NULL, it is ignored.</summary>
        ///
        /// <param name="dptr">Returned global device pointer</param>
        /// <param name="bytes">Returned global size in bytes</param>
        /// <param name="hmod">Module to retrieve global from</param>
        /// <param name="name">Name of global to retrieve</param>
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_NOT_FOUND
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuModuleGetFunction,
        /// ::cuModuleGetTexRef,
        /// ::cuModuleLoad,
        /// ::cuModuleLoadData,
        /// ::cuModuleLoadDataEx,
        /// ::cuModuleLoadFatBinary,
        /// ::cuModuleUnload,
        /// ::cudaGetSymbolAddress,
        /// ::cudaGetSymbolSize
        /// CUresult CUDAAPI cuModuleGetGlobal(CUdeviceptr *dptr, size_t *bytes, CUmodule hmod, const char *name);
        [DllImport(_dllpath, EntryPoint = "cuModuleGetGlobal", CharSet = CharSet.Ansi)]
        public static extern CuResult ModuleGetGlobal(out CuDevicePtr dptr, out IntPtr bytes, CuModule hmod, string name);

        /// <summary>Returns a handle to a texture reference
        ///
        /// Returns in <paramref name="*pTexRef"/> the handle of the texture reference of name <paramref name="name
        ///"/> in the module <paramref name="hmod."/> If no texture reference of that name exists,
        /// ::cuModuleGetTexRef() returns ::CUDA_ERROR_NOT_FOUND. This texture reference
        /// handle should not be destroyed, since it will be destroyed when the module
        /// is unloaded.</summary>
        ///
        /// <param name="pTexRef">Returned texture reference</param>
        /// <param name="hmod">Module to retrieve texture reference from</param>
        /// <param name="name">Name of texture reference to retrieve</param>
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_NOT_FOUND
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuModuleGetFunction,
        /// ::cuModuleGetGlobal,
        /// ::cuModuleGetSurfRef,
        /// ::cuModuleLoad,
        /// ::cuModuleLoadData,
        /// ::cuModuleLoadDataEx,
        /// ::cuModuleLoadFatBinary,
        /// ::cuModuleUnload,
        /// ::cudaGetTextureReference
        /// CUresult CUDAAPI cuModuleGetTexRef(CUtexref *pTexRef, CUmodule hmod, const char *name);
        [DllImport(_dllpath, EntryPoint = "cuModuleGetTexRef", CharSet = CharSet.Ansi)]
        public static extern CuResult ModuleGetTexRef(out CuTextRef pTexRef, CuModule hmod, string name);

        /// <summary>Returns a handle to a surface reference
        ///
        /// Returns in <paramref name="*pSurfRef"/> the handle of the surface reference of name <paramref name="name
        ///"/> in the module <paramref name="hmod."/> If no surface reference of that name exists,
        /// ::cuModuleGetSurfRef() returns ::CUDA_ERROR_NOT_FOUND.</summary>
        ///
        /// <param name="pSurfRef">Returned surface reference</param>
        /// <param name="hmod">Module to retrieve surface reference from</param>
        /// <param name="name">Name of surface reference to retrieve</param>
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_NOT_FOUND
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuModuleGetFunction,
        /// ::cuModuleGetGlobal,
        /// ::cuModuleGetTexRef,
        /// ::cuModuleLoad,
        /// ::cuModuleLoadData,
        /// ::cuModuleLoadDataEx,
        /// ::cuModuleLoadFatBinary,
        /// ::cuModuleUnload,
        /// ::cudaGetSurfaceReference
        /// CUresult CUDAAPI cuModuleGetSurfRef(CUsurfref *pSurfRef, CUmodule hmod, const char *name);
        [DllImport(_dllpath, EntryPoint = "cuModuleGetSurfRef", CharSet = CharSet.Ansi)]
        public static extern CuResult ModuleGetSurfRef(out CuSurfRef pSurfRef, CuModule hmod, string name);

        /// <summary>Creates a pending JIT linker invocation.
        ///
        /// If the call is successful, the caller owns the returned CUlinkState, which
        /// should eventually be destroyed with ::cuLinkDestroy.  The
        /// device code machine size (32 or 64 bit) will match the calling application.
        ///
        /// Both linker and compiler options may be specified.  Compiler options will
        /// be applied to inputs to this linker action which must be compiled from PTX.
        /// The options ::CU_JIT_WALL_TIME,
        /// ::CU_JIT_INFO_LOG_BUFFER_SIZE_BYTES, and ::CU_JIT_ERROR_LOG_BUFFER_SIZE_BYTES
        /// will accumulate data until the CUlinkState is destroyed.
        ///
        /// <paramref name="optionValues"/> must remain valid for the life of the CUlinkState if output
        /// options are used.  No other references to inputs are maintained after this
        /// call returns.</summary>
        ///
        /// \param numOptions   Size of options arrays
        /// \param options      Array of linker and compiler options
        /// \param optionValues Array of option values, each cast to void *
        /// \param stateOut     On success, this will contain a CUlinkState to specify
        ///                     and complete this action
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_DEINITIALIZED,
        /// ::CUDA_ERROR_NOT_INITIALIZED,
        /// ::CUDA_ERROR_INVALID_CONTEXT,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_OUT_OF_MEMORY,
        /// ::CUDA_ERROR_JIT_COMPILER_NOT_FOUND
        /// </returns>
        /// \notefnerr
        ///
        /// \sa ::cuLinkAddData,
        /// ::cuLinkAddFile,
        /// ::cuLinkComplete,
        /// ::cuLinkDestroy
        /// CUresult CUDAAPI cuLinkCreate(unsigned int numOptions, CUjit_option *options, void **optionValues, CUlinkState *stateOut);
        [DllImport(_dllpath, EntryPoint = "cuLinkCreate")]
        public static extern CuResult LinkCreate(int numOptions, JitOption* options, void** optionValues, CuLinkState* stateOut);

        /// <summary>Add an input to a pending linker invocation
        ///
        /// Ownership of <paramref name="data"/> is retained by the caller.  No reference is retained to any
        /// inputs after this call returns.
        ///
        /// This method accepts only compiler options, which are used if the data must
        /// be compiled from PTX, and does not accept any of
        /// ::CU_JIT_WALL_TIME, ::CU_JIT_INFO_LOG_BUFFER, ::CU_JIT_ERROR_LOG_BUFFER,
        /// ::CU_JIT_TARGET_FROM_CUCONTEXT, or ::CU_JIT_TARGET.</summary>
        ///
        /// \param state        A pending linker action.
        /// \param type         The type of the input data.
        /// \param data         The input data.  PTX must be NULL-terminated.
        /// \param size         The length of the input data.
        /// \param name         An optional name for this input in log messages.
        /// \param numOptions   Size of options.
        /// \param options      Options to be applied only for this input (overrides options from ::cuLinkCreate).
        /// \param optionValues Array of option values, each cast to void *.
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_INVALID_IMAGE,
        /// ::CUDA_ERROR_INVALID_PTX,
        /// ::CUDA_ERROR_OUT_OF_MEMORY,
        /// ::CUDA_ERROR_NO_BINARY_FOR_GPU
        /// </returns>
        ///
        /// \sa ::cuLinkCreate,
        /// ::cuLinkAddFile,
        /// ::cuLinkComplete,
        /// ::cuLinkDestroy
        /// CUresult CUDAAPI cuLinkAddData(CUlinkState state, CUjitInputType type, void *data, size_t size, const char *name, unsigned int numOptions, CUjit_option *options, void **optionValues);
        [DllImport(_dllpath, EntryPoint = "cuLinkAddData", CharSet = CharSet.Ansi)]
        public static extern CuResult LinkAddData(CuLinkState state, JitInputType type, void* data, IntPtr size, string name, int numOptions, JitOption *options, void** optionValues);

        /// <summary>Add a file input to a pending linker invocation
        ///
        /// No reference is retained to any inputs after this call returns.
        ///
        /// This method accepts only compiler options, which are used if the input
        /// must be compiled from PTX, and does not accept any of
        /// ::CU_JIT_WALL_TIME, ::CU_JIT_INFO_LOG_BUFFER, ::CU_JIT_ERROR_LOG_BUFFER,
        /// ::CU_JIT_TARGET_FROM_CUCONTEXT, or ::CU_JIT_TARGET.
        ///
        /// This method is equivalent to invoking ::cuLinkAddData on the contents
        /// of the file.</summary>
        ///
        /// \param state        A pending linker action
        /// \param type         The type of the input data
        /// \param path         Path to the input file
        /// \param numOptions   Size of options
        /// \param options      Options to be applied only for this input (overrides options from ::cuLinkCreate)
        /// \param optionValues Array of option values, each cast to void *
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_FILE_NOT_FOUND
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_INVALID_VALUE,
        /// ::CUDA_ERROR_INVALID_IMAGE,
        /// ::CUDA_ERROR_INVALID_PTX,
        /// ::CUDA_ERROR_OUT_OF_MEMORY,
        /// ::CUDA_ERROR_NO_BINARY_FOR_GPU
        /// </returns>
        ///
        /// \sa ::cuLinkCreate,
        /// ::cuLinkAddData,
        /// ::cuLinkComplete,
        /// ::cuLinkDestroy
        /// CUresult CUDAAPI cuLinkAddFile(CUlinkState state, CUjitInputType type, const char *path, unsigned int numOptions, CUjit_option *options, void **optionValues);
        [DllImport(_dllpath, EntryPoint = "cuLinkAddFile", CharSet = CharSet.Ansi)]
        public static extern CuResult LinkAddFile(CuLinkState state, JitInputType type, string path, int numOptions, JitOption *options, void** optionValues);

        /// <summary>Complete a pending linker invocation
        ///
        /// Completes the pending linker action and returns the cubin image for the linked
        /// device code, which can be used with ::cuModuleLoadData.  The cubin is owned by
        /// <paramref name="state,"/> so it should be loaded before <paramref name="state"/> is destroyed via ::cuLinkDestroy.
        /// This call does not destroy <paramref name="state."/></summary>
        ///
        /// \param state    A pending linker invocation
        /// \param cubinOut On success, this will point to the output image
        /// \param sizeOut  Optional parameter to receive the size of the generated image
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_INVALID_HANDLE,
        /// ::CUDA_ERROR_OUT_OF_MEMORY
        /// </returns>
        ///
        /// \sa ::cuLinkCreate,
        /// ::cuLinkAddData,
        /// ::cuLinkAddFile,
        /// ::cuLinkDestroy,
        /// ::cuModuleLoadData
        /// CUresult CUDAAPI cuLinkComplete(CUlinkState state, void **cubinOut, size_t *sizeOut);
        [DllImport(_dllpath, EntryPoint = "cuLinkComplete")]
        public static extern CuResult LinkComplete(CuLinkState state, void** cubinOut, IntPtr* sizeOut);

        /// <summary>Destroys state for a JIT linker invocation.</summary>
        ///
        /// \param state State object for the linker invocation
        ///
        /// <returns>
        /// ::CUDA_SUCCESS,
        /// ::CUDA_ERROR_INVALID_HANDLE
        /// </returns>
        ///
        /// \sa ::cuLinkCreate
        /// CUresult CUDAAPI cuLinkDestroy(CUlinkState state);
        [DllImport(_dllpath, EntryPoint = "cuLinkDestroy")]
        public static extern CuResult LinkDestroy(CuLinkState state);
    }
}
