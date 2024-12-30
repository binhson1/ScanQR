using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class MonitorName : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

    [DllImport("user32.dll")]
    private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfo lpmi);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct MonitorInfo
    {
        public int cbSize;
        public Rect rcMonitor;
        public Rect rcWork;
        public uint dwFlags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szDevice;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    void Start()
    {
        IntPtr hMonitor = MonitorFromWindow(IntPtr.Zero, 0);
        MonitorInfo mi = new MonitorInfo();
        mi.cbSize = Marshal.SizeOf(mi);
        if (GetMonitorInfo(hMonitor, ref mi))
        {
            Debug.Log("Monitor Name: " + mi.szDevice);
        }
        else
        {
            Debug.Log("Failed to get monitor name.");
        }
    }
}
