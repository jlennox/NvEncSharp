using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using PInvoke;
using static Lennox.NvEncSharp.Sample.VideoDecode.NativeWindowNative;

namespace Lennox.NvEncSharp.Sample.VideoDecode
{
    public abstract class NativeWindow : IDisposable
    {
        private static int _id;

        public IntPtr Hwnd { get; }

        private readonly WndProcDelegate _wndProc;

        public NativeWindow(string title, int width, int height)
        {
            var processHandle = Process.GetCurrentProcess().Handle;

            _wndProc = WndProc;

            using var cursor = User32.LoadCursor(IntPtr.Zero, (IntPtr) 32512);

            var wndClass = new WNDCLASSEX
            {
                cbSize = Marshal.SizeOf(typeof(User32.WNDCLASSEX)),
                hbrBackground = IntPtr.Zero + 1,
                cbClsExtra = 0,
                cbWndExtra = 0,
                hInstance = processHandle,
                hIcon = IntPtr.Zero,
                hCursor = cursor.DangerousGetHandle(),
                lpszMenuName = null,
                lpszClassName = $"{nameof(NativeWindow)}.{Interlocked.Increment(ref _id)}",
                lpfnWndProc = Marshal.GetFunctionPointerForDelegate(_wndProc),
                hIconSm = IntPtr.Zero
            };
            var regResult = RegisterClassEx(ref wndClass);

            Hwnd = CreateWindowEx(0, regResult, "Hello Win32",
                (int)(User32.WindowStyles.WS_OVERLAPPEDWINDOW | User32.WindowStyles.WS_VISIBLE),
                0, 0, width, height, IntPtr.Zero, IntPtr.Zero,
                wndClass.hInstance, IntPtr.Zero);

            if (Hwnd == IntPtr.Zero)
            {
                var error = Marshal.GetLastWin32Error();
                throw new Exception(error.ToString());
            }

            User32.SetWindowText(Hwnd, title);

            ShowWindow(Hwnd, (int)User32.WindowShowStyle.SW_SHOW);
            UpdateWindow(Hwnd);
        }

        public virtual IntPtr WndProc(
            IntPtr hWnd, User32.WindowMessage msg,
            IntPtr wParam, IntPtr lParam)
        {
            return DefWindowProc(hWnd, msg, wParam, lParam);
        }

        public void InvalidateWindow()
        {
            InvalidateRect(Hwnd, IntPtr.Zero, false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            User32.DestroyWindow(Hwnd);
        }
    }

    public static class NativeWindowNative
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct WNDCLASSEX
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public User32.ClassStyles style;
            public IntPtr lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpszMenuName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpszClassName;
            public IntPtr hIconSm;
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr CreateWindowEx(
            int dwExStyle,
            ushort regResult,
            //[MarshalAs(UnmanagedType.LPStr)]
            //string lpClassName,
            [MarshalAs(UnmanagedType.LPStr)]
            string lpWindowName,
            int dwStyle,
            int x,
            int y,
            int nWidth,
            int nHeight,
            IntPtr hWndParent,
            IntPtr hMenu,
            IntPtr hInstance,
            IntPtr lpParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern ushort RegisterClassEx([In] ref WNDCLASSEX lpWndClass);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, User32.WindowMessage msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UpdateWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);

        public delegate IntPtr WndProcDelegate(IntPtr hWnd, User32.WindowMessage msg, IntPtr wParam, IntPtr lParam);
    }
}
