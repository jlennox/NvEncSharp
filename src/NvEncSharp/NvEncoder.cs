using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using static Lennox.NvEncSharp.LibNvEnc;

// ReSharper disable UnusedMember.Global

namespace Lennox.NvEncSharp
{
    public partial struct NvEncoder
    {
        private static NvEncApiFunctionList Fn => FunctionList;

        /// <summary>Retrieves the number of supported encode Guids.
        ///
        /// The function returns the number of codec Guids supported by the NvEncodeAPI
        /// interface.</summary>
        ///
        /// <param name="encodeGuidCount">Number of supported encode Guids.</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_GENERIC</returns>
        public void GetEncodeGuidCount(out uint encodeGuidCount)
        {
            encodeGuidCount = 0;
            var status = Fn.GetEncodeGuidCount(this, ref encodeGuidCount);
            CheckResult(this, status);
        }

        public uint GetEncodeGuidCount()
        {
            uint encodeGuidCount = 0;
            var status = Fn.GetEncodeGuidCount(this, ref encodeGuidCount);
            CheckResult(this, status);
            return encodeGuidCount;
        }

        /// <summary>Retrieves an array of supported encoder codec Guids.
        ///
        /// The function returns an array of codec Guids supported by the NvEncodeAPI interface.
        /// The client must allocate an array where the NvEncodeAPI interface can
        /// fill the supported Guids and pass the pointer in \p///Guids parameter.
        /// The size of the array can be determined by using ::NvEncGetEncodeGuidCount() API.
        /// The Nvidia Encoding interface returns the number of codec Guids it has actually
        /// filled in the Guid array in the <paramref name="guidCount"/> parameter.</summary>
        ///
        /// <param name="guids">Array of supported Encode Guids.</param>
        /// <param name="guidCount">Number of supported Encode Guids.</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_GENERIC</returns>
        public unsafe void GetEncodeGuids(Span<Guid> guids, out uint guidCount)
        {
            guidCount = 0;
            fixed (Guid* ptr = guids)
            {
                var status = Fn.GetEncodeGuids(
                    this, ptr, (uint)guids.Length, ref guidCount);
                CheckResult(this, status);
            }
        }

        public IReadOnlyList<Guid> GetEncodeGuids()
        {
            var count = GetEncodeGuidCount();
            if (count == 0) return Array.Empty<Guid>();
            Span<Guid> guids = stackalloc Guid[(int)count];
            GetEncodeGuids(guids, out var actualCount);
            return guids.Slice(0, (int)actualCount).ToArray();
        }

        /// <summary>Retrieves the number of supported profile Guids.
        ///
        /// The function returns the number of profile Guids supported for a given codec.
        /// The client must first enumerate the codec Guids supported by the NvEncodeAPI
        /// interface. After determining the codec Guid, it can query the NvEncodeAPI
        /// interface to determine the number of profile Guids supported for a particular
        /// codec Guid.</summary>
        ///
        /// <param name="encodeGuid">The codec Guid for which the profile Guids are being enumerated.</param>
        /// <param name="encodeProfileGuidCount">Number of encode profiles supported for the given encodeGuid.</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_GENERIC</returns>
        public void GetEncodeProfileGuidCount(
            Guid encodeGuid, out uint encodeProfileGuidCount)
        {
            encodeProfileGuidCount = 0;
            var status = Fn.GetEncodeProfileGuidCount(
                this, encodeGuid, ref encodeProfileGuidCount);
            CheckResult(this, status);
        }

        public uint GetEncodeProfileGuidCount(Guid encodeGuid)
        {
            uint encodeProfileGuidCount = 0;
            var status = Fn.GetEncodeProfileGuidCount(
                this, encodeGuid, ref encodeProfileGuidCount);
            CheckResult(this, status);
            return encodeProfileGuidCount;
        }

        /// <summary>Retrieves an array of supported encode profile Guids.
        ///
        /// The function returns an array of supported profile Guids for a particular
        /// codec Guid. The client must allocate an array where the NvEncodeAPI interface
        /// can populate the profile Guids. The client can determine the array size using
        /// ::NvEncGetEncodeProfileGuidCount() API. The client must also validiate that the
        /// NvEncodeAPI interface supports the Guid the client wants to pass as <paramref name="encodeGuid"/>
        /// parameter.</summary>
        ///
        /// <param name="encodeGuid">The encode Guid whose profile Guids are being enumerated.</param>
        /// <param name="profileGuids">Array of supported Encode Profile Guids</param>
        /// <param name="guidCount">Number of valid encode profile Guids in <paramref name="profileGuids"/> array.</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_GENERIC</returns>
        public unsafe void GetEncodeProfileGuids(
            Guid encodeGuid, Span<Guid> profileGuids, out uint guidCount)
        {
            guidCount = 0;

            fixed (Guid* ptr = profileGuids)
            {
                var status = Fn.GetEncodeProfileGuids(
                    this, encodeGuid, ptr,
                    (uint)profileGuids.Length, ref guidCount);
                CheckResult(this, status);
            }
        }

        public IReadOnlyList<Guid> GetEncodeProfileGuids(Guid encodeGuid)
        {
            var count = GetEncodeProfileGuidCount(encodeGuid);
            if (count == 0) return Array.Empty<Guid>();
            Span<Guid> guids = stackalloc Guid[(int)count];
            GetEncodeProfileGuids(encodeGuid, guids, out var actualCount);
            return guids.Slice(0, (int)actualCount).ToArray();
        }

        /// <summary>Retrieve the number of supported Input formats.
        ///
        /// The function returns the number of supported input formats. The client must
        /// query the NvEncodeAPI interface to determine the supported input formats
        /// before creating the input surfaces.</summary>
        ///
        /// <param name="encodeGuid">Encode Guid, corresponding to which the number of supported input formats
        ///   is to be retrieved.</param>
        /// <param name="inputFmtCount">Number of input formats supported for specified Encode Guid.</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_GENERIC</returns>
        public void GetInputFormatCount(
            Guid encodeGuid, out uint inputFmtCount)
        {
            inputFmtCount = 0;
            var status = Fn.GetInputFormatCount(
                this, encodeGuid, ref inputFmtCount);
            CheckResult(this, status);
        }

        public uint GetInputFormatCount(Guid encodeGuid)
        {
            uint inputFmtCount = 0;
            var status = Fn.GetInputFormatCount(
                this, encodeGuid, ref inputFmtCount);
            CheckResult(this, status);
            return inputFmtCount;
        }

        /// <summary>Retrieves an array of supported Input formats
        ///
        /// Returns an array of supported input formats  The client must use the input
        /// format to create input surface using ::NvEncCreateInputBuffer() API.</summary>
        ///
        /// <param name="encodeGuid">Encode Guid, corresponding to which the number of supported input formats
        ///   is to be retrieved.</param>
        /// <param name="inputFmts">Array of input formats supported for this Encode Guid.</param>
        /// <param name="inputFmtCount">The number of valid input format types returned by the NvEncodeAPI
        ///   interface in <paramref name="inputFmts"/> array.</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_GENERIC</returns>
        public unsafe void GetInputFormats(
            Guid encodeGuid, Span<NvEncBufferFormat> inputFmts,
            out uint inputFmtCount)
        {
            inputFmtCount = 0;
            fixed (NvEncBufferFormat* ptr = inputFmts)
            {
                var status = Fn.GetInputFormats(
                    this, encodeGuid, ptr,
                    (uint)inputFmts.Length, ref inputFmtCount);
                CheckResult(this, status);
            }
        }

        public IReadOnlyList<NvEncBufferFormat> GetInputFormats(Guid encodeGuid)
        {
            var count = GetInputFormatCount(encodeGuid);
            if (count == 0) return Array.Empty<NvEncBufferFormat>();
            Span<NvEncBufferFormat> formats = stackalloc NvEncBufferFormat[(int)count];
            GetInputFormats(encodeGuid, formats, out var actualCount);
            return formats.Slice(0, (int)actualCount).ToArray();
        }

        /// <summary>Retrieves the capability value for a specified encoder attribute.
        ///
        /// The function returns the capability value for a given encoder attribute. The
        /// client must validate the encodeGuid using ::NvEncGetEncodeGuids() API before
        /// calling this function. The encoder attribute being queried are enumerated in
        /// ::NV_ENC_CAPS_PARAM enum.</summary>
        ///
        /// <param name="encodeGuid">Encode Guid, corresponding to which the capability attribute is to be retrieved.</param>
        /// <param name="capsParam">Used to specify attribute being queried. Refer ::NV_ENC_CAPS_PARAM for  more details.</param>
        /// <param name="capsVal">The value corresponding to the capability attribute being queried.</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_GENERIC</returns>
        public void GetEncodeCaps(
            Guid encodeGuid, ref NvEncCapsParam capsParam, ref int capsVal)
        {
            var status = Fn.GetEncodeCaps(
                this, encodeGuid, ref capsParam, ref capsVal);
            CheckResult(this, status);
        }

        /// <summary>Retrieves the number of supported preset Guids.
        ///
        /// The function returns the number of preset Guids available for a given codec.
        /// The client must validate the codec Guid using ::NvEncGetEncodeGuids() API
        /// before calling this function.</summary>
        ///
        /// <param name="encodeGuid">Encode Guid, corresponding to which the number of supported presets is to
        ///   be retrieved.</param>
        /// <param name="encodePresetGuidCount">Receives the number of supported preset Guids.</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_GENERIC</returns>
        public void GetEncodePresetCount(
            Guid encodeGuid, out uint encodePresetGuidCount)
        {
            encodePresetGuidCount = 0;
            var status = Fn.GetEncodePresetCount(
                this, encodeGuid, ref encodePresetGuidCount);
            CheckResult(this, status);
        }

        public uint GetEncodePresetCount(Guid encodeGuid)
        {
            uint encodePresetGuidCount = 0;
            var status = Fn.GetEncodePresetCount(
                this, encodeGuid, ref encodePresetGuidCount);
            CheckResult(this, status);
            return encodePresetGuidCount;
        }

        /// <summary>Receives an array of supported encoder preset Guids.
        ///
        /// The function returns an array of encode preset Guids available for a given codec.
        /// The client can directly use one of the preset Guids based upon the use case
        /// or target device. The preset Guid chosen can be directly used in
        /// NV_ENC_INITIALIZE_PARAMS::presetGuid parameter to ::NvEncEncodePicture() API.
        /// Alternately client can  also use the preset Guid to retrieve the encoding config
        /// parameters being used by NvEncodeAPI interface for that given preset, using
        /// ::NvEncGetEncodePresetConfig() API. It can then modify preset config parameters
        /// as per its use case and send it to NvEncodeAPI interface as part of
        /// NV_ENC_INITIALIZE_PARAMS::encodeConfig parameter for NvEncInitializeEncoder()
        /// API.</summary>
        ///
        ///
        /// <param name="encodeGuid">Encode Guid, corresponding to which the list of supported presets is to be retrieved.</param>
        /// <param name="presetGuids">Array of supported Encode preset Guids from the NvEncodeAPI interface to client.</param>
        /// <param name="encodePresetGuidCount">Receives the number of preset Guids returned by the NvEncodeAPI interface.</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_GENERIC</returns>
        public unsafe void GetEncodePresetGuids(
            Guid encodeGuid, Span<Guid> presetGuids,
            out uint encodePresetGuidCount)
        {
            encodePresetGuidCount = 0;
            fixed (Guid* ptr = presetGuids)
            {
                var status = Fn.GetEncodePresetGuids(
                    this, encodeGuid, ptr, (uint)presetGuids.Length,
                    ref encodePresetGuidCount);
                CheckResult(this, status);
            }
        }

        public IReadOnlyList<Guid> GetEncodePresetGuids(Guid encodeGuid)
        {
            var count = GetEncodePresetCount(encodeGuid);
            if (count == 0) return Array.Empty<Guid>();
            Span<Guid> guids = stackalloc Guid[(int)count];
            GetEncodePresetGuids(encodeGuid, guids, out var actualCount);
            return guids.Slice(0, (int)actualCount).ToArray();
        }

        /// <summary>Returns a preset config structure supported for given preset Guid.
        ///
        /// The function returns a preset config structure for a given preset Guid. Before
        /// using this function the client must enumerate the preset Guids available for
        /// a given codec. The preset config structure can be modified by the client depending
        /// upon its use case and can be then used to initialize the encoder using
        /// ::NvEncInitializeEncoder() API. The client can use this function only if it
        /// wants to modify the NvEncodeAPI preset configuration, otherwise it can
        /// directly use the preset Guid.</summary>
        ///
        /// <param name="encodeGuid">Encode Guid, corresponding to which the list of supported presets is to be retrieved.</param>
        /// <param name="presetGuid">Preset Guid, corresponding to which the Encoding configurations is to be retrieved.</param>
        /// <param name="presetConfig">The requested Preset Encoder Attribute set. Refer ::_NV_ENC_CONFIG for more details.</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_INVALID_VERSION
        /// ::NV_ENC_ERR_GENERIC</returns>
        ///
        /// <summary>NV_ENC_CONFIG</summary>
        public void GetEncodePresetConfig(
            Guid encodeGuid, Guid presetGuid,
            ref NvEncPresetConfig presetConfig)
        {
            var status = Fn.GetEncodePresetConfig(
                this, encodeGuid, presetGuid, ref presetConfig);
            CheckResult(this, status);
        }

        public NvEncPresetConfig GetEncodePresetConfig(
            Guid encodeGuid, Guid presetGuid)
        {
            var presetConfig = new NvEncPresetConfig
            {
                Version = NV_ENC_PRESET_CONFIG_VER,
                PresetCfg = new NvEncConfig
                {
                    Version = NV_ENC_CONFIG_VER
                }
            };

            GetEncodePresetConfig(encodeGuid, presetGuid, ref presetConfig);
            return presetConfig;
        }

        /// <summary>Initialize the
        ///
        /// This API must be used to initialize the  The initialization parameter
        /// is passed using \p///createEncodeParams  The client must send the following
        /// fields of the _NV_ENC_INITIALIZE_PARAMS structure with a valid value.
        /// - NV_ENC_INITIALIZE_PARAMS::encodeGuid
        /// - NV_ENC_INITIALIZE_PARAMS::encodeWidth
        /// - NV_ENC_INITIALIZE_PARAMS::encodeHeight
        ///
        /// The client can pass a preset Guid directly to the NvEncodeAPI interface using
        /// NV_ENC_INITIALIZE_PARAMS::presetGuid field. If the client doesn't pass
        /// NV_ENC_INITIALIZE_PARAMS::encodeConfig structure, the codec specific parameters
        /// will be selected based on the preset Guid. The preset Guid must have been
        /// validated by the client using ::NvEncGetEncodePresetGuids() API.
        /// If the client passes a custom ::_NV_ENC_CONFIG structure through
        /// NV_ENC_INITIALIZE_PARAMS::encodeConfig , it will override the codec specific parameters
        /// based on the preset Guid. It is recommended that even if the client passes a custom config,
        /// it should also send a preset Guid. In this case, the preset Guid passed by the client
        /// will not override any of the custom config parameters programmed by the client,
        /// it is only used as a hint by the NvEncodeAPI interface to determine certain encoder parameters
        /// which are not exposed to the client.
        ///
        /// There are two modes of operation for the encoder namely:
        /// - Asynchronous mode
        /// - Synchronous mode
        ///
        /// The client can select asynchronous or synchronous mode by setting the \p
        /// enableEncodeAsync field in ::_NV_ENC_INITIALIZE_PARAMS to 1 or 0 respectively.
        ///\par Asynchronous mode of operation:
        /// The Asynchronous mode can be enabled by setting NV_ENC_INITIALIZE_PARAMS::enableEncodeAsync to 1.
        /// The client operating in asynchronous mode must allocate completion event object
        /// for each output buffer and pass the completion event object in the
        /// ::NvEncEncodePicture() API. The client can create another thread and wait on
        /// the event object to be signalled by NvEncodeAPI interface on completion of the
        /// encoding process for the output frame. This should unblock the main thread from
        /// submitting work to the  When the event is signalled the client can call
        /// NvEncodeAPI interfaces to copy the bitstream data using ::NvEncLockBitstream()
        /// API. This is the preferred mode of operation.
        ///
        /// NOTE: Asynchronous mode is not supported on Linux.
        ///
        ///\par Synchronous mode of operation:
        /// The client can select synchronous mode by setting NV_ENC_INITIALIZE_PARAMS::enableEncodeAsync to 0.
        /// The client working in synchronous mode can work in a single threaded or multi
        /// threaded mode. The client need not allocate any event objects. The client can
        /// only lock the bitstream data after NvEncodeAPI interface has returned
        /// ::NV_ENC_SUCCESS from encode picture. The NvEncodeAPI interface can return
        /// ::NV_ENC_ERR_NEED_MORE_INPUT error code from ::NvEncEncodePicture() API. The
        /// client must not lock the output buffer in such case but should send the next
        /// frame for encoding. The client must keep on calling ::NvEncEncodePicture() API
        /// until it returns ::NV_ENC_SUCCESS.
        /// The client must always lock the bitstream data in order in which it has submitted.
        /// This is true for both asynchronous and synchronous mode.
        ///
        ///\par Picture type decision:
        /// If the client is taking the picture type decision and it must disable the picture
        /// type decision module in NvEncodeAPI by setting NV_ENC_INITIALIZE_PARAMS::enablePTD
        /// to 0. In this case the client is  required to send the picture in encoding
        /// order to NvEncodeAPI by doing the re-ordering for B frames.
        /// If the client doesn't want to take the picture type decision it can enable
        /// picture type decision module in the NvEncodeAPI interface by setting
        /// NV_ENC_INITIALIZE_PARAMS::enablePTD to 1 and send the input pictures in display
        /// order.</summary>
        ///
        /// <param name="createEncodeParams">Refer ::_NV_ENC_INITIALIZE_PARAMS for details.</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_INVALID_VERSION
        /// ::NV_ENC_ERR_GENERIC</returns>
        /// <seealso cref="NvEncInitializeParams"/>
        public void InitializeEncoder(
            ref NvEncInitializeParams createEncodeParams)
        {
            var status = Fn.InitializeEncoder(this, ref createEncodeParams);
            CheckResult(this, status);
        }

        /// <summary>Allocates Input buffer.
        ///
        /// This function is used to allocate an input buffer. The client must enumerate
        /// the input buffer format before allocating the input buffer resources. The
        /// NV_ENC_INPUT_PTR returned by the NvEncodeAPI interface in the
        /// NV_ENC_CREATE_INPUT_BUFFER::inputBuffer field can be directly used in
        /// ::NvEncEncodePicture() API. The number of input buffers to be allocated by the
        /// client must be at least 4 more than the number of B frames being used for encoding.</summary>
        ///
        /// <param name="createInputBufferParams">Pointer to the ::NV_ENC_CREATE_INPUT_BUFFER structure.</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_INVALID_VERSION
        /// ::NV_ENC_ERR_GENERIC</returns>
        public void CreateInputBuffer(
            ref NvEncCreateInputBuffer createInputBufferParams)
        {
            var status = Fn.CreateInputBuffer(this, ref createInputBufferParams);
            CheckResult(this, status);
        }

        public NvEncCreateInputBuffer CreateInputBuffer(
            int width, int height, NvEncBufferFormat bufferFormat)
        {
            var createInputBufferParams = new NvEncCreateInputBuffer
            {
                Version = NV_ENC_CREATE_INPUT_BUFFER_VER,
                Width = (uint)width,
                Height = (uint)height,
                BufferFmt = bufferFormat
            };

            var status = Fn.CreateInputBuffer(this, ref createInputBufferParams);
            CheckResult(this, status);

            return createInputBufferParams;
        }

        /// <summary>Release an input buffers.
        ///
        /// This function is used to free an input buffer. If the client has allocated
        /// any input buffer using ::NvEncCreateInputBuffer() API, it must free those
        /// input buffers by calling this function. The client must release the input
        /// buffers before destroying the encoder using ::NvEncDestroyEncoder() API.</summary>
        ///
        /// <param name="inputBuffer">Pointer to the input buffer to be released.</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_INVALID_VERSION
        /// ::NV_ENC_ERR_GENERIC</returns>
        public void DestroyInputBuffer(NvEncInputPtr inputBuffer)
        {
            CheckResult(this, Fn.DestroyInputBuffer(this, inputBuffer));
        }

        /// <summary>Set input and output CUDA stream for specified encoder attribute.
        ///
        /// Encoding may involve CUDA pre-processing on the input and post-processing on encoded output.
        /// This function is used to set input and output CUDA streams to pipeline the CUDA pre-processing
        /// and post-processing tasks. Clients should call this function before the call to
        /// NvEncUnlockInputBuffer(). If this function is not called, the default CUDA stream is used for
        /// input and output processing. After a successful call to this function, the streams specified
        /// in that call will replace the previously-used streams.
        /// This API is supported for NVCUVID interface only.</summary>
        ///
        /// <param name="inputStream">Pointer to CUstream which is used to process ::NV_ENC_PIC_PARAMS::inputFrame for encode.
        ///   In case of ME-only mode, inputStream is used to process ::NV_ENC_MEONLY_PARAMS::inputBuffer and
        ///   ::NV_ENC_MEONLY_PARAMS::referenceFrame</param>
        /// <param name="outputStream">Pointer to CUstream which is used to process ::NV_ENC_PIC_PARAMS::outputBuffer for encode.
        ///  In case of ME-only mode, outputStream is used to process ::NV_ENC_MEONLY_PARAMS::mvBuffer</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_INVALID_VERSION
        /// ::NV_ENC_ERR_GENERIC</returns>
        public void SetIOCudaStreams(
            NvEncCustreamPtr inputStream, NvEncCustreamPtr outputStream)
        {
            var status = Fn.SetIOCudaStreams(this, inputStream, outputStream);
            CheckResult(this, status);
        }

        /// <summary>Allocates an output bitstream buffer
        ///
        /// This function is used to allocate an output bitstream buffer and returns a
        /// NV_ENC_OUTPUT_PTR to bitstream  buffer to the client in the
        /// NV_ENC_CREATE_BITSTREAM_BUFFER::bitstreamBuffer field.
        /// The client can only call this function after the encoder session has been
        /// initialized using ::NvEncInitializeEncoder() API. The minimum number of output
        /// buffers allocated by the client must be at least 4 more than the number of B
        /// B frames being used for encoding. The client can only access the output
        /// bitsteam data by locking the <c>bitstreamBuffer</c> using the ::NvEncLockBitstream()
        /// function.</summary>
        ///
        /// <param name="createBitstreamBufferParams">Pointer ::NV_ENC_CREATE_BITSTREAM_BUFFER for details.</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_INVALID_VERSION
        /// ::NV_ENC_ERR_ENCODER_NOT_INITIALIZED
        /// ::NV_ENC_ERR_GENERIC</returns>
        public void CreateBitstreamBuffer(
            ref NvEncCreateBitstreamBuffer createBitstreamBufferParams)
        {
            var status = Fn.CreateBitstreamBuffer(
                this, ref createBitstreamBufferParams);
            CheckResult(this, status);
        }

        public NvEncCreateBitstreamBuffer CreateBitstreamBuffer()
        {
            var createBitstreamBufferParams = new NvEncCreateBitstreamBuffer
            {
                Version = NV_ENC_CREATE_BITSTREAM_BUFFER_VER
            };

            var status = Fn.CreateBitstreamBuffer(
                this, ref createBitstreamBufferParams);
            CheckResult(this, status);

            return createBitstreamBufferParams;
        }

        /// <summary>Release a bitstream buffer.
        ///
        /// This function is used to release the output bitstream buffer allocated using
        /// the ::NvEncCreateBitstreamBuffer() function. The client must release the output
        /// bitstreamBuffer using this function before destroying the encoder session.</summary>
        ///
        /// <param name="bitstreamBuffer">Pointer to the bitstream buffer being released.</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_INVALID_VERSION
        /// ::NV_ENC_ERR_ENCODER_NOT_INITIALIZED
        /// ::NV_ENC_ERR_GENERIC</returns>
        public void DestroyBitstreamBuffer(NvEncOutputPtr bitstreamBuffer)
        {
            var status = Fn.DestroyBitstreamBuffer(this, bitstreamBuffer);
            CheckResult(this, status);
        }

        /// <summary>Submit an input picture for encoding.
        ///
        /// This function is used to submit an input picture buffer for encoding. The
        /// encoding parameters are passed using \p///encodePicParams which is a pointer
        /// to the ::_NV_ENC_PIC_PARAMS structure.
        ///
        /// If the client has set NV_ENC_INITIALIZE_PARAMS::enablePTD to 0, then it must
        /// send a valid value for the following fields.
        /// - NV_ENC_PIC_PARAMS::pictureType
        /// - NV_ENC_PIC_PARAMS_H264::displayPOCSyntax(H264 only)
        /// - NV_ENC_PIC_PARAMS_H264::frameNumSyntax(H264 only)
        /// - NV_ENC_PIC_PARAMS_H264::refPicFlag(H264 only)
        ///
        ///\par MVC Encoding:
        /// For MVC encoding the client must call encode picture api for each view separately
        /// and must pass valid view id in NV_ENC_PIC_PARAMS_MVC::viewID field. Currently
        /// NvEncodeAPI only support stereo MVC so client must send viewID as 0 for base
        /// view and view ID as 1 for dependent view.
        ///
        ///\par Asynchronous Encoding
        /// If the client has enabled asynchronous mode of encoding by setting
        /// NV_ENC_INITIALIZE_PARAMS::enableEncodeAsync to 1 in the ::NvEncInitializeEncoder()
        /// API ,then the client must send a valid NV_ENC_PIC_PARAMS::completionEvent.
        /// Incase of asynchronous mode of operation, client can queue the ::NvEncEncodePicture()
        /// API commands from the main thread and then queue output buffers to be processed
        /// to a secondary worker thread. Before the locking the output buffers in the
        /// secondary thread , the client must wait on NV_ENC_PIC_PARAMS::completionEvent
        /// it has queued in ::NvEncEncodePicture() API call. The client must always process
        /// completion event and the output buffer in the same order in which they have been
        /// submitted for encoding. The NvEncodeAPI interface is responsible for any
        /// re-ordering required for B frames and will always ensure that encoded bitstream
        /// data is written in the same order in which output buffer is submitted.
        ///\par Synchronous Encoding
        /// The client can enable synchronous mode of encoding by setting
        /// NV_ENC_INITIALIZE_PARAMS::enableEncodeAsync to 0 in ::NvEncInitializeEncoder() API.
        /// The NvEncodeAPI interface may return ::NV_ENC_ERR_NEED_MORE_INPUT error code for
        /// some ::NvEncEncodePicture() API calls when NV_ENC_INITIALIZE_PARAMS::enablePTD
        /// is set to 1, but the client must not treat it as a fatal error. The NvEncodeAPI
        /// interface might not be able to submit an input picture buffer for encoding
        /// immediately due to re-ordering for B frames. The NvEncodeAPI interface cannot
        /// submit the input picture which is decided to be encoded as B frame as it waits
        /// for backward reference from  temporally subsequent frames. This input picture
        /// is buffered internally and waits for more input picture to arrive. The client
        /// must not call ::NvEncLockBitstream() API on the output buffers whose
        /// ::NvEncEncodePicture() API returns ::NV_ENC_ERR_NEED_MORE_INPUT. The client must
        /// wait for the NvEncodeAPI interface to return ::NV_ENC_SUCCESS before locking the
        /// output bitstreams to read the encoded bitstream data. The following example
        /// explains the scenario with synchronous encoding with 2 B frames.</summary>
        ///
        /// <param name="encodePicParams">Pointer to the ::_NV_ENC_PIC_PARAMS structure.</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_INVALID_VERSION
        /// ::NV_ENC_ERR_ENCODER_BUSY
        /// ::NV_ENC_ERR_NEED_MORE_INPUT
        /// ::NV_ENC_ERR_ENCODER_NOT_INITIALIZED
        /// ::NV_ENC_ERR_GENERIC</returns>
        public void EncodePicture(ref NvEncPicParams encodePicParams)
        {
            CheckResult(this, Fn.EncodePicture(this, ref encodePicParams));
        }

        /// <summary>Lock output bitstream buffer
        ///
        /// This function is used to lock the bitstream buffer to read the encoded data.
        /// The client can only access the encoded data by calling this function.
        /// The pointer to client accessible encoded data is returned in the
        /// NV_ENC_LOCK_BITSTREAM::bitstreamBufferPtr field. The size of the encoded data
        /// in the output buffer is returned in the NV_ENC_LOCK_BITSTREAM::bitstreamSizeInBytes
        /// The NvEncodeAPI interface also returns the output picture type and picture structure
        /// of the encoded frame in NV_ENC_LOCK_BITSTREAM::pictureType and
        /// NV_ENC_LOCK_BITSTREAM::pictureStruct fields respectively. If the client has
        /// set NV_ENC_LOCK_BITSTREAM::doNotWait to 1, the function might return
        /// ::NV_ENC_ERR_LOCK_BUSY if client is operating in synchronous mode. This is not
        /// a fatal failure if NV_ENC_LOCK_BITSTREAM::doNotWait is set to 1. In the above case the client can
        /// retry the function after few milliseconds.</summary>
        ///
        /// <param name="lockBitstreamBufferParams">Pointer to the ::_NV_ENC_LOCK_BITSTREAM structure.</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_INVALID_VERSION
        /// ::NV_ENC_ERR_LOCK_BUSY
        /// ::NV_ENC_ERR_ENCODER_NOT_INITIALIZED
        /// ::NV_ENC_ERR_GENERIC</returns>
        public void LockBitstream(
            ref NvEncLockBitstream lockBitstreamBufferParams)
        {
            var status = Fn.LockBitstream(this, ref lockBitstreamBufferParams);
            CheckResult(this, status);
        }

        public NvEncLockBitstream LockBitstream(
            ref NvEncCreateBitstreamBuffer buffer,
            bool doNotWait = false)
        {
            var lockBitstreamBufferParams = new NvEncLockBitstream
            {
                Version = NV_ENC_LOCK_BITSTREAM_VER,
                OutputBitstream = buffer.BitstreamBuffer.Handle,
                DoNotWait = doNotWait
            };

            var status = Fn.LockBitstream(this, ref lockBitstreamBufferParams);
            CheckResult(this, status);

            return lockBitstreamBufferParams;
        }

        public NvEncLockedBitstream LockBitstreamAndCreateStream(
            ref NvEncCreateBitstreamBuffer buffer,
            bool doNotWait = false)
        {
            var locked = LockBitstream(ref buffer, doNotWait);

            return new NvEncLockedBitstream(this, buffer, locked);
        }

        public unsafe class NvEncLockedBitstream : UnmanagedMemoryStream
        {
            private readonly NvEncoder _encoder;
            private readonly NvEncCreateBitstreamBuffer _buffer;
            private int _disposed;

            public NvEncLockedBitstream(
                NvEncoder encoder,
                NvEncCreateBitstreamBuffer buffer,
                NvEncLockBitstream bitstream)
            : base((byte*)bitstream.BitstreamBufferPtr,
                bitstream.BitstreamSizeInBytes)
            {
                _encoder = encoder;
                _buffer = buffer;
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing &&
                    Interlocked.Exchange(ref _disposed, 1) == 0)
                {
                    _encoder.UnlockBitstream(_buffer.BitstreamBuffer);
                }

                base.Dispose(disposing);
            }
        }

        /// <summary>Unlock the output bitstream buffer
        ///
        /// This function is used to unlock the output bitstream buffer after the client
        /// has read the encoded data from output buffer. The client must call this function
        /// to unlock the output buffer which it has previously locked using ::NvEncLockBitstream()
        /// function. Using a locked bitstream buffer in ::NvEncEncodePicture() API will cause
        /// the function to fail.</summary>
        ///
        /// <param name="bitstreamBuffer">bitstream buffer pointer being unlocked</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_ENCODER_NOT_INITIALIZED
        /// ::NV_ENC_ERR_GENERIC</returns>
        public void UnlockBitstream(NvEncOutputPtr bitstreamBuffer)
        {
            CheckResult(this, Fn.UnlockBitstream(this, bitstreamBuffer));
        }

        /// <summary>Locks an input buffer
        ///
        /// This function is used to lock the input buffer to load the uncompressed YUV
        /// pixel data into input buffer memory. The client must pass the NV_ENC_INPUT_PTR
        /// it had previously allocated using ::NvEncCreateInputBuffer()in the
        /// NV_ENC_LOCK_INPUT_BUFFER::inputBuffer field.
        /// The NvEncodeAPI interface returns pointer to client accessible input buffer
        /// memory in NV_ENC_LOCK_INPUT_BUFFER::bufferDataPtr field.</summary>
        ///
        /// <param name="lockInputBufferParams">Pointer to the ::_NV_ENC_LOCK_INPUT_BUFFER structure</param>
        ///
        /// <returns>\return
        /// ::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_INVALID_VERSION
        /// ::NV_ENC_ERR_LOCK_BUSY
        /// ::NV_ENC_ERR_ENCODER_NOT_INITIALIZED
        /// ::NV_ENC_ERR_GENERIC</returns>
        public void LockInputBuffer(
            ref NvEncLockInputBuffer lockInputBufferParams)
        {
            var status = Fn.LockInputBuffer(this, ref lockInputBufferParams);
            CheckResult(this, status);
        }

        /// <summary>Unlocks the input buffer
        ///
        /// This function is used to unlock the input buffer memory previously locked for
        /// uploading YUV pixel data. The input buffer must be unlocked before being used
        /// again for encoding, otherwise NvEncodeAPI will fail the ::NvEncEncodePicture()</summary>
        ///
        /// <param name="inputBuffer">Pointer to the input buffer that is being unlocked.</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_VERSION
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_ENCODER_NOT_INITIALIZED
        /// ::NV_ENC_ERR_GENERIC</returns>
        public void UnlockInputBuffer(NvEncInputPtr inputBuffer)
        {
            CheckResult(this, Fn.UnlockInputBuffer(this, inputBuffer));
        }

        /// <summary>Get encoding statistics.
        ///
        /// This function is used to retrieve the encoding statistics.
        /// This API is not supported when encode device type is CUDA.</summary>
        ///
        /// <param name="encodeStats">Pointer to the ::_NV_ENC_STAT structure.</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_ENCODER_NOT_INITIALIZED
        /// ::NV_ENC_ERR_GENERIC</returns>
        public void GetEncodeStats(ref NvEncStat encodeStats)
        {
            CheckResult(this, Fn.GetEncodeStats(this, ref encodeStats));
        }

        /// <summary>Get encoded sequence and picture header.
        ///
        /// This function can be used to retrieve the sequence and picture header out of
        /// band. The client must call this function only after the encoder has been
        /// initialized using ::NvEncInitializeEncoder() function. The client must
        /// allocate the memory where the NvEncodeAPI interface can copy the bitstream
        /// header and pass the pointer to the memory in NV_ENC_SEQUENCE_PARAM_PAYLOAD::spsppsBuffer.
        /// The size of buffer is passed in the field  NV_ENC_SEQUENCE_PARAM_PAYLOAD::inBufferSize.
        /// The NvEncodeAPI interface will copy the bitstream header payload and returns
        /// the actual size of the bitstream header in the field
        /// NV_ENC_SEQUENCE_PARAM_PAYLOAD::outSPSPPSPayloadSize.
        /// The client must call  ::NvEncGetSequenceParams() function from the same thread which is
        /// being used to call ::NvEncEncodePicture() function.</summary>
        ///
        /// <param name="sequenceParamPayload">Pointer to the ::_NV_ENC_SEQUENCE_PARAM_PAYLOAD structure.</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_VERSION
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_ENCODER_NOT_INITIALIZED
        /// ::NV_ENC_ERR_GENERIC</returns>
        public void GetSequenceParams(
            ref NvEncSequenceParamPayload sequenceParamPayload)
        {
            var status = Fn.GetSequenceParams(this, ref sequenceParamPayload);
            CheckResult(this, status);
        }

        /// <summary>Register event for notification to encoding completion.
        ///
        /// This function is used to register the completion event with NvEncodeAPI
        /// interface. The event is required when the client has configured the encoder to
        /// work in asynchronous mode. In this mode the client needs to send a completion
        /// event with every output buffer. The NvEncodeAPI interface will signal the
        /// completion of the encoding process using this event. Only after the event is
        /// signalled the client can get the encoded data using ::NvEncLockBitstream() function.</summary>
        ///
        /// <param name="eventParams">Pointer to the ::_NV_ENC_EVENT_PARAMS structure.</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_VERSION
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_ENCODER_NOT_INITIALIZED
        /// ::NV_ENC_ERR_GENERIC</returns>
        public void RegisterAsyncEvent(ref NvEncEventParams eventParams)
        {
            CheckResult(this, Fn.RegisterAsyncEvent(this, ref eventParams));
        }

        /// <summary>Unregister completion event.
        ///
        /// This function is used to unregister completion event which has been previously
        /// registered using ::NvEncRegisterAsyncEvent() function. The client must unregister
        /// all events before destroying the encoder using ::NvEncDestroyEncoder() function.</summary>
        ///
        /// <param name="eventParams">Pointer to the ::_NV_ENC_EVENT_PARAMS structure.</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_VERSION
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_ENCODER_NOT_INITIALIZED
        /// ::NV_ENC_ERR_GENERIC</returns>
        public void UnregisterAsyncEvent(ref NvEncEventParams eventParams)
        {
            CheckResult(this, Fn.UnregisterAsyncEvent(this, ref eventParams));
        }

        /// <summary>Map an externally created input resource pointer for encoding.
        ///
        /// Maps an externally allocated input resource [using and returns a NV_ENC_INPUT_PTR
        /// which can be used for encoding in the ::NvEncEncodePicture() function. The
        /// mapped resource is returned in the field NV_ENC_MAP_INPUT_RESOURCE::outputResourcePtr.
        /// The NvEncodeAPI interface also returns the buffer format of the mapped resource
        /// in the field NV_ENC_MAP_INPUT_RESOURCE::outbufferFmt.
        /// This function provides synchronization guarantee that any graphics work submitted
        /// on the input buffer is completed before the buffer is used for encoding. This is
        /// also true for compute(i.e. CUDA) work, provided that the previous workload using
        /// the input resource was submitted to the default stream.
        /// The client should not access any input buffer while they are mapped by the </summary>
        ///
        /// <param name="mapInputResParams">Pointer to the ::_NV_ENC_MAP_INPUT_RESOURCE structure.</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_VERSION
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_ENCODER_NOT_INITIALIZED
        /// ::NV_ENC_ERR_RESOURCE_NOT_REGISTERED
        /// ::NV_ENC_ERR_MAP_FAILED
        /// ::NV_ENC_ERR_GENERIC</returns>
        public void MapInputResource(ref NvEncMapInputResource mapInputResParams)
        {
            CheckResult(this, Fn.MapInputResource(this, ref mapInputResParams));
        }

        /// <summary>UnMaps a NV_ENC_INPUT_PTR  which was mapped for encoding
        ///
        ///
        /// UnMaps an input buffer which was previously mapped using ::NvEncMapInputResource()
        /// API. The mapping created using ::NvEncMapInputResource() should be invalidated
        /// using this API before the external resource is destroyed by the client. The client
        /// must unmap the buffer after ::NvEncLockBitstream() API returns succuessfully for encode
        /// work submitted using the mapped input buffer.</summary>
        ///
        ///
        /// <param name="mappedInputBuffer">Pointer to the NV_ENC_INPUT_PTR</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_VERSION
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_ENCODER_NOT_INITIALIZED
        /// ::NV_ENC_ERR_RESOURCE_NOT_REGISTERED
        /// ::NV_ENC_ERR_RESOURCE_NOT_MAPPED
        /// ::NV_ENC_ERR_GENERIC</returns>
        public void UnmapInputResource(NvEncInputPtr mappedInputBuffer)
        {
            CheckResult(this, Fn.UnmapInputResource(this, mappedInputBuffer));
        }

        /// <summary>Destroy Encoding Session
        ///
        /// Destroys the encoder session previously created using ::NvEncOpenEncodeSession()
        /// function. The client must flush the encoder before freeing any resources. In order
        /// to flush the encoder the client must pass a NULL encode picture packet and either
        /// wait for the ::NvEncEncodePicture() function to return in synchronous mode or wait
        /// for the flush event to be signaled by the encoder in asynchronous mode.
        /// The client must free all the input and output resources created using the
        /// NvEncodeAPI interface before destroying the  If the client is operating
        /// in asynchronous mode, it must also unregister the completion events previously
        /// registered.</summary>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_GENERIC</returns>
        public void DestroyEncoder()
        {
            CheckResult(this, Fn.DestroyEncoder(this));
        }

        /// <summary>Invalidate reference frames
        ///
        /// Invalidates reference frame based on the time stamp provided by the client.
        /// The encoder marks any reference frames or any frames which have been reconstructed
        /// using the corrupt frame as invalid for motion estimation and uses older reference
        /// frames for motion estimation. The encoded forces the current frame to be encoded
        /// as an intra frame if no reference frames are left after invalidation process.
        /// This is useful for low latency application for error resiliency. The client
        /// is recommended to set NV_ENC_CONFIG_H264::maxNumRefFrames to a large value so
        /// that encoder can keep a backup of older reference frames in the DPB and can use them
        /// for motion estimation when the newer reference frames have been invalidated.
        /// This API can be called multiple times.</summary>
        ///
        /// <param name="invalidRefFrameTimeStamp">Timestamp of the invalid reference frames which needs to be invalidated.</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_GENERIC</returns>
        public void InvalidateRefFrames(ulong invalidRefFrameTimeStamp)
        {
            var status = Fn.InvalidateRefFrames(this, invalidRefFrameTimeStamp);
            CheckResult(this, status);
        }

        /// <summary>Registers a resource with the Nvidia Video Encoder Interface.
        ///
        /// Registers a resource with the Nvidia Video Encoder Interface for book keeping.
        /// The client is expected to pass the registered resource handle as well, while calling ::NvEncMapInputResource API.</summary>
        ///
        /// <param name="registerResParams">Pointer to a ::_NV_ENC_REGISTER_RESOURCE structure</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_VERSION
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_ENCODER_NOT_INITIALIZED
        /// ::NV_ENC_ERR_RESOURCE_REGISTER_FAILED
        /// ::NV_ENC_ERR_GENERIC
        /// ::NV_ENC_ERR_UNIMPLEMENTED</returns>
        public NvEncRegisteredResource RegisterResource(
            ref NvEncRegisterResource registerResParams)
        {
            var status = Fn.RegisterResource(this, ref registerResParams);
            CheckResult(this, status);

            return new NvEncRegisteredResource(
                this, registerResParams.RegisteredResource);
        }

        public class NvEncRegisteredResource : IDisposable
        {
            private readonly NvEncoder _encoder;
            private readonly NvEncRegisteredPtr _resource;
            private int _disposed;

            public NvEncRegisteredResource(
                 NvEncoder encoder,
                NvEncRegisteredPtr resource)
            {
                _encoder = encoder;
                _resource = resource;
            }

            public void Dispose()
            {
                if (Interlocked.Exchange(ref _disposed, 1) != 0) return;

                _encoder.UnregisterResource(_resource);
            }
        }

        /// <summary>Unregisters a resource previously registered with the Nvidia Video Encoder Interface.
        ///
        /// Unregisters a resource previously registered with the Nvidia Video Encoder Interface.
        /// The client is expected to unregister any resource that it has registered with the
        /// Nvidia Video Encoder Interface before destroying the resource.</summary>
        ///
        /// <param name="registeredResource">The registered resource pointer that was returned in ::NvEncRegisterResource.</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_VERSION
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_ENCODER_NOT_INITIALIZED
        /// ::NV_ENC_ERR_RESOURCE_NOT_REGISTERED
        /// ::NV_ENC_ERR_GENERIC
        /// ::NV_ENC_ERR_UNIMPLEMENTED</returns>
        public void UnregisterResource(NvEncRegisteredPtr registeredResource)
        {
            CheckResult(this, Fn.UnregisterResource(this, registeredResource));
        }

        /// <summary>Reconfigure an existing encoding session.
        ///
        /// Reconfigure an existing encoding session.
        /// The client should call this API to change/reconfigure the parameter passed during
        /// NvEncInitializeEncoder API call.
        /// Currently Reconfiguration of following are not supported.
        /// Change in GOP structure.
        /// Change in sync-Async mode.
        /// Change in MaxWidth & MaxHeight.
        /// Change in PTDmode.
        ///
        /// Resolution change is possible only if maxEncodeWidth & maxEncodeHeight of NV_ENC_INITIALIZE_PARAMS
        /// is set while creating encoder session.</summary>
        ///
        /// <param name="reInitEncodeParams"> Pointer to a ::NV_ENC_RECONFIGURE_PARAMS structure.</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_NO_ENCODE_DEVICE
        /// ::NV_ENC_ERR_UNSUPPORTED_DEVICE
        /// ::NV_ENC_ERR_INVALID_DEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_GENERIC</returns>
        public void ReconfigureEncoder(
            ref NvEncReconfigureParams reInitEncodeParams)
        {
            var status = Fn.ReconfigureEncoder(this, ref reInitEncodeParams);
            CheckResult(this, status);
        }

        /// <summary>Allocates output MV buffer for ME only mode.
        ///
        /// This function is used to allocate an output MV buffer. The size of the mvBuffer is
        /// dependent on the frame height and width of the last ::NvEncCreateInputBuffer() call.
        /// The NV_ENC_OUTPUT_PTR returned by the NvEncodeAPI interface in the
        /// ::NV_ENC_CREATE_MV_BUFFER::mvBuffer field should be used in
        /// ::NvEncRunMotionEstimationOnly() API.
        /// Client must lock ::NV_ENC_CREATE_MV_BUFFER::mvBuffer using ::NvEncLockBitstream() API to get the motion vector data.</summary>
        ///
        /// <param name="createMvBufferParams">Pointer to the ::NV_ENC_CREATE_MV_BUFFER structure.</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_INVALID_VERSION
        /// ::NV_ENC_ERR_GENERIC</returns>
        public void CreateMvBuffer(
            ref NvEncCreateMvBuffer createMvBufferParams)
        {
            var status = Fn.CreateMvBuffer(this, ref createMvBufferParams);
            CheckResult(this, status);
        }

        /// <summary>Release an output MV buffer for ME only mode.
        ///
        /// This function is used to release the output MV buffer allocated using
        /// the ::NvEncCreateMVBuffer() function. The client must release the output
        /// mvBuffer using this function before destroying the encoder session.</summary>
        ///
        /// <param name="mvBuffer">Pointer to the mvBuffer being released.</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_INVALID_VERSION
        /// ::NV_ENC_ERR_ENCODER_NOT_INITIALIZED
        /// ::NV_ENC_ERR_GENERIC</returns>
        public void DestroyMvBuffer(NvEncOutputPtr mvBuffer)
        {
            CheckResult(this, Fn.DestroyMvBuffer(this, mvBuffer));
        }

        /// <summary>Submit an input picture and reference frame for motion estimation in ME only mode.
        ///
        /// This function is used to submit the input frame and reference frame for motion
        /// estimation. The ME parameters are passed using///meOnlyParams which is a pointer
        /// to ::_NV_ENC_MEONLY_PARAMS structure.
        /// Client must lock ::NV_ENC_CREATE_MV_BUFFER::mvBuffer using ::NvEncLockBitstream() API to get the motion vector data.
        /// to get motion vector data.</summary>
        ///
        /// <param name="meOnlyParams">Pointer to the ::_NV_ENC_MEONLY_PARAMS structure.</param>
        ///
        /// <returns>::NV_ENC_SUCCESS
        /// ::NV_ENC_ERR_INVALID_PTR
        /// ::NV_ENC_ERR_INVALID_ENCODERDEVICE
        /// ::NV_ENC_ERR_DEVICE_NOT_EXIST
        /// ::NV_ENC_ERR_UNSUPPORTED_PARAM
        /// ::NV_ENC_ERR_OUT_OF_MEMORY
        /// ::NV_ENC_ERR_INVALID_PARAM
        /// ::NV_ENC_ERR_INVALID_VERSION
        /// ::NV_ENC_ERR_NEED_MORE_INPUT
        /// ::NV_ENC_ERR_ENCODER_NOT_INITIALIZED
        /// ::NV_ENC_ERR_GENERIC</returns>
        public void RunMotionEstimationOnly(ref NvEncMeonlyParams meOnlyParams)
        {
            var status = Fn.RunMotionEstimationOnly(this, ref meOnlyParams);
            CheckResult(this, status);
        }

        /// <summary>Get the description of the last error reported by the API.
        ///
        /// This function returns a null-terminated string that can be used by clients to better understand the reason
        /// for failure of a previous API call.</summary>
        ///
        /// <returns>Pointer to buffer containing the details of the last error encountered by the API.</returns>
        public string GetLastError()
        {
            if (Handle == IntPtr.Zero) return "No NvEncoder.";

            var ptr = Fn.GetLastError(this);
            return ptr == IntPtr.Zero ? null : Marshal.PtrToStringAnsi(ptr);
        }
    }
}
