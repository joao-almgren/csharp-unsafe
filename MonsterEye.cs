using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Unsafe
{
	class MonsterEye
	{
		Rectangle clientRect;

		double px = 20, vx = 1;
		double py = 20, vy = 1;
		int radius = 20;
		double pa = 0.0;
		double va = 0.0025;
		double p2x = 0, ptx = 6;
		double p2y = 0, pty = 6;

		public MonsterEye(Rectangle clientRect)
		{
			this.clientRect = clientRect;
		}

		public void Tick()
		{
			px += vx;
			if (px > clientRect.Width - radius || px < radius)
				vx = -vx;

			py += vy;
			if (py > clientRect.Height - radius || py < radius)
				vy = -vy;

			pa += va;
			if (pa > 0.2 || pa < -0.2)
				va = -va;

			ptx = vx * 6;
			pty = vy * 6;

			const double offs = 0.15;

			if (p2x - ptx < -offs)
				p2x += offs;
			else if (p2x - ptx > offs)
				p2x -= offs;

			if (p2y - pty < -offs)
				p2y += offs;
			else if (p2y - pty > offs)
				p2y -= offs;
		}

		public void Render(BitmapData bmpData)
		{
			DrawStar(bmpData, (int)px, (int)py, Color.Red.ToArgb());
			DrawCircle(bmpData, (int)px, (int)py, radius, Color.White.ToArgb());
			DrawCircle(bmpData, (int)(px + p2x), (int)(py + p2y), radius / 2, Color.CornflowerBlue.ToArgb());
		}

		unsafe void DrawCircle(BitmapData bmpData, int px, int py, int radius, Int32 col)
		{
			Int32* buf = (Int32*)bmpData.Scan0;
			Int32 stride = bmpData.Stride / 4; // single byte -> 32bit

			for (int y = 0; y < clientRect.Height; y++)
				for (int x = 0; x < clientRect.Width; x++)
				{
					int ox = px - x;
					int oy = py - y;

					if ((ox * ox + oy * oy) < (radius * radius))
						buf[x + (y * stride)] = col;
				}
		}

		unsafe void DrawStar(BitmapData bmpData, int px, int py, Int32 col)
		{
			Int32* buf = (Int32*)bmpData.Scan0;
			Int32 stride = bmpData.Stride / 4; // single byte -> 32bit

			for (int y = 0; y < clientRect.Height; y++)
				for (int x = 1; x < clientRect.Width; x++) // avoid division by zero
				{
					double lx = (px * px) - (x * x);
					double ly = (py * py) - (y * y);

					double angle = Math.Atan(ly / lx) * 180.0 / Math.PI;
					angle += pa * Math.Sqrt((px - x) * (px - x) + (py - y) * (py - y));

					angle = Math.Abs(angle);
					angle = angle % 90;

					if ((angle > 5 && angle < 10)
						|| (angle > 24 && angle < 29)
						|| (angle > 42 && angle < 47)
						|| (angle > 61 && angle < 66)
						|| (angle > 80 && angle < 85))
					{
						buf[x + (y * stride)] = col;
					}
				}
		}
	}
}
