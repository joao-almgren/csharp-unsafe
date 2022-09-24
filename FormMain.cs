using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace Unsafe
{
	public partial class FormMain : Form
	{
		Bitmap bmp = null;
		RenderLoop renderLoop = null;

		MonsterEye monsterEye = null;

		public FormMain()
		{
			InitializeComponent();

			this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			this.ClientSize = new Size(800, 600);
			this.Text = "Unsafe";

			this.Load += new EventHandler(FormMain_Load);
			this.KeyUp += new KeyEventHandler(FormMain_KeyUp);
			this.Paint += new PaintEventHandler(FormMain_Paint);
		}

		void FormMain_Load(object sender, EventArgs e)
		{
			monsterEye = new MonsterEye(this.ClientRectangle);

			bmp = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height, PixelFormat.Format32bppRgb);
			renderLoop = new RenderLoop(new RenderLoop.LoopHandler(OnLoop));
		}

		void FormMain_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				this.Close();
			}
		}

		private void OnLoop(double elapsedTime)
		{
			if (this.WindowState == FormWindowState.Normal)
			{
				Tick();
				Render();
			}
		}

		void Tick()
		{
			monsterEye.Tick();
		}

		void Render()
		{
			Graphics.FromImage(bmp).Clear(Color.Black);
			BitmapData bmpData = bmp.LockBits(this.ClientRectangle, ImageLockMode.WriteOnly, bmp.PixelFormat);

			monsterEye.Render(bmpData);

			bmp.UnlockBits(bmpData);
			this.Refresh();
		}

		void FormMain_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawImage(bmp, 0, 0);
		}
	}
}
