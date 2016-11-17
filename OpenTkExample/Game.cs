using OpenTK;
using System;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;


namespace OpenTkExample
{
	class Game :GameWindow
	{
		public Game():
			base(512,512,new OpenTK.Graphics.GraphicsMode(32, 24, 0, 4))
		{

		}
		

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			GL.ClearColor(Color.Blue);
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);

			GL.Viewport(0, 0, Width, Height);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.Enable(EnableCap.DepthTest);


			GL.Flush();
			SwapBuffers();
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			base.OnUpdateFrame(e);
		}






	}
}
