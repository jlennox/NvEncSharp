using System;
using Lennox.NvEncSharp.Sample.Library;
using PInvoke;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D11.Device;

namespace Lennox.NvEncSharp.Sample.VideoDecode
{
    internal class DisplayWindow : NativeWindow
    {
        private readonly CuContext _context;
        private SwapChain _swap;
        private Device _device;
        private readonly Adapter _adapter;
        private CuGraphicsResource _resource;
        private bool _hasSwap;
        private bool _disposed;

        private readonly object _createdSwapSync = new object();

        public DisplayWindow(
            CuContext context,
            string title, int width, int height)
            : base(title, width, height)
        {
            using var _ = context.Push();

            _context = context;
            _adapter = GetAdapterByContext(context);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _disposed = true;
                _resource.Dispose();
                _swap?.Dispose();
                _adapter?.Dispose();
                _device?.Dispose();
            }

            base.Dispose(disposing);
        }

        public override IntPtr WndProc(
            IntPtr hWnd, User32.WindowMessage msg,
            IntPtr wParam, IntPtr lParam)
        {
            switch (msg)
            {
                case User32.WindowMessage.WM_PAINT:
                    _swap?.Present(1, PresentFlags.None);
                    break;
                case User32.WindowMessage.WM_CLOSE:
                    Environment.Exit(0);
                    return IntPtr.Zero;
            }

            return base.WndProc(hWnd, msg, wParam, lParam);
        }

        private static Adapter GetAdapterByContext(CuContext context)
        {
            var contextDevice = context.GetDevice();

            using var factory = new Factory1();

            foreach (var adapter in factory.Adapters)
            {
                var device = CuDevice.GetD3D11Device(adapter.NativePointer);

                if (device.Handle == contextDevice.Handle)
                {
                    return adapter;
                }

                adapter.Dispose();
            }

            throw new Exception("Unable to locate display adaptor for context.");
        }

        private void CreateSwap(FrameInformation frame)
        {
            var sc = new SwapChainDescription
            {
                BufferCount = 1,
                ModeDescription = new ModeDescription(
                    frame.Info.Width, frame.Info.Height,
                    new Rational(30, 1), Format.B8G8R8A8_UNorm),
                IsWindowed = true,
                OutputHandle = Hwnd,
                SampleDescription = new SampleDescription(1, 0),
                Usage = Usage.RenderTargetOutput,
                Flags = SwapChainFlags.None
            };

            Device.CreateWithSwapChain(
                _adapter, DeviceCreationFlags.Debug,
                sc, out _device, out _swap);

            var buffer = _swap.GetBackBuffer<Texture2D>(0);
            _resource = CuGraphicsResource.Register(buffer.NativePointer);
            _resource.SetMapFlags(CuGraphicsMapResources.WriteDiscard);
        }

        public unsafe void FrameArrivedHost(FrameInformation frame)
        {
            var rgbSize = frame.GetRgba32Size();
            var width = frame.Info.Width;
            var rgba32Pitch = width * 4;
            var buffer = _swap.GetBackBuffer<Texture2D>(0);

            // TODO: Pool the buffer.
            var destHost = new byte[rgbSize];
            fixed (byte* destHostPtr = destHost)
            {
                _device.ImmediateContext.UpdateSubresource(
                    buffer, 0, null, (IntPtr)destHostPtr, rgba32Pitch, 0);
            }

            _swap?.Present(1, PresentFlags.None);
        }

        public void FrameArrivedDevice(FrameInformation frame)
        {
            using var _ = _resource.Map();
            var resourceArray = _resource.GetMappedArray();
            var rgbSize = frame.GetRgba32Size();
            var width = frame.Info.Width;
            var rgba32Pitch = width * 4;
            var rgba32PitchPtr = (IntPtr)rgba32Pitch;

            using (var destPtr = CuDeviceMemory.Allocate(rgbSize))
            {
                frame.DecodeToDeviceRgba32(destPtr);

                var memcopy = new CuMemcopy2D
                {
                    SrcMemoryType = CuMemoryType.Device,
                    SrcDevice = destPtr,
                    SrcPitch = rgba32PitchPtr,
                    DstMemoryType = CuMemoryType.Array,
                    DstArray = resourceArray,
                    WidthInBytes = rgba32PitchPtr,
                    Height = (IntPtr)frame.Info.Height
                };

                memcopy.Memcpy2D();
            }

            _swap?.Present(1, PresentFlags.None);
        }

        public void FrameArrived(FrameInformation frame)
        {
            if (_disposed) return;

            using var _ = _context.Push();

            // TODO: This does need to detect configuration changes.
            // https://en.wikipedia.org/wiki/Double-checked_locking
            if (!_hasSwap)
            {
                lock (_createdSwapSync)
                {
                    if (!_hasSwap)
                    {
                        CreateSwap(frame);
                        _hasSwap = true;
                    }
                }
            }

            switch (frame.Buffer.MemoryType)
            {
                case CuMemoryType.Host:
                    FrameArrivedHost(frame);
                    break;
                case CuMemoryType.Device:
                    FrameArrivedDevice(frame);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(frame.Buffer.MemoryType),
                        frame.Buffer.MemoryType,
                        "Unsupported memory type.");
            }
        }
    }
}