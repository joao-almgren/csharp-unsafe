using System;
using System.Windows.Forms;

namespace Unsafe
{
	class RenderLoop
	{
		public delegate void LoopHandler(double elapsedTime);
		public event LoopHandler Loop;

		static long TicksPerSecond = 0;
		double _lastTime = 0;

		public RenderLoop(LoopHandler loopHandler)
		{
			Windows.QueryPerformanceFrequency(ref TicksPerSecond);

			Loop += loopHandler;
			Application.Idle += new EventHandler(OnApplicationIdle);
		}

		void OnApplicationIdle(object sender, EventArgs e)
		{
			while (AppStillIdle)
			{
				long time = 0;
				Windows.QueryPerformanceCounter(ref time);

				double elapsedTime = (double)(time - _lastTime) / (double)TicksPerSecond;
				_lastTime = time;

				Loop(elapsedTime);
			}
		}

		bool AppStillIdle
		{
			get
			{
				Windows.Message msg;
				return (!Windows.PeekMessage(out msg, IntPtr.Zero, 0, 0, 0));
			}
		}
	}
}
