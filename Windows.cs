using System;
using System.Runtime.InteropServices;

namespace Unsafe
{
	class Windows
	{
		[StructLayout(LayoutKind.Sequential)]
		public struct Message
		{
			public IntPtr hWnd;
			public Int32 msg;
			public IntPtr wParam;
			public IntPtr lParam;
			public uint time;
			public System.Drawing.Point p;
		}

		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public static extern bool PeekMessage(
			out Message msg,
			IntPtr hWnd,
			uint messageFilterMin,
			uint messageFilterMax,
			uint flags);

		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32", CharSet = CharSet.Auto)]
		public static extern bool QueryPerformanceCounter(ref long PerformanceCount);

		[System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32", CharSet = CharSet.Auto)]
		public static extern bool QueryPerformanceFrequency(ref long PerformanceFrequency);
	}
}
