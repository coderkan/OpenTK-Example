using OpenTK;
using System;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Input;

namespace OpenTkExample
{
	class Game :GameWindow
	{
		ShaderHelper helper;
		Cube cube;
		private static System.Diagnostics.Stopwatch watch;
		float time = 0.0f;
		float etime = 0.0f;
		bool autoRotate = false;
		private static float xangle, yangle;
		bool right = false, left = false, up = false, down = false;
		OpenTK.Input.KeyboardState lastKeystate;
		float deltaTime = 0.0f;
		static float FISTDISTANCE = -5.0f;
		float xdist = 0.0f;
		float ydist = 0.0f;
		float zdist = FISTDISTANCE;

		float horizontalAngle = 3.14f;
		float verticalAngle = 0.0f;

		Vector3 direction = new Vector3();
		Vector3 rightDirection = new Vector3();



		public Game():
			base(512,512,new OpenTK.Graphics.GraphicsMode(32, 24, 0, 4))
		{
			Mouse.Move += Mouse_Move;
			Mouse.WheelChanged += Mouse_WheelChanged;
		}
		

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			helper = new ShaderHelper(Shaders.VertexShader, Shaders.FragmentShader);
			cube = new Cube();
			cube.Position = new Vector3(0f, 0.0f/*-0.5f-0.5f + (float)Math.Sin(time)*/, 0f);
			cube.Rotation = new Vector3(0f, 0f, 0f);
			cube.Scale = new Vector3(1f, 1f, 1f);
			cube.CalculateModelMatrix();
			GL.ClearColor(Color.Transparent);
			watch = System.Diagnostics.Stopwatch.StartNew();
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{


			base.OnRenderFrame(e);

			GL.Viewport(0, 0, Width, Height);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);

			GL.EnableVertexAttribArray(helper.AttributeVPosition);
			GL.EnableVertexAttribArray(helper.AttributeVcolor);

			GL.UniformMatrix4(helper.UniformModelView, false, ref cube.ModelViewProjectionMatrix);
			GL.DrawElements(BeginMode.Triangles, cube.IndicesCount, DrawElementsType.UnsignedInt, 0);

			GL.DisableVertexAttribArray(helper.AttributeVPosition);
			GL.DisableVertexAttribArray(helper.AttributeVcolor);

			GL.Flush();
			SwapBuffers();
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			watch.Stop();
			deltaTime = (float)watch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
			watch.Restart();

			base.OnUpdateFrame(e);
			List<Vector3> verts = new List<Vector3>();
			List<int> inds = new List<int>();
			List<Vector3> colors = new List<Vector3>();

			int vertCount = 0;
			verts.AddRange(cube.GetVertex().ToList());
			inds.AddRange(cube.GetIndices(vertCount).ToList());
			colors.AddRange(cube.GetColorData().ToList());



			Vector3[] vertdata = verts.ToArray();
			int[] indicedata = inds.ToArray();
			Vector3[] coldata = colors.ToArray();


			// 
			GL.BindBuffer(BufferTarget.ArrayBuffer, helper.VBOPosition);
			GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertdata.Length * Vector3.SizeInBytes),
				vertdata,
				BufferUsageHint.StaticDraw
				);
			GL.VertexAttribPointer(helper.AttributeVPosition, 3, VertexAttribPointerType.Float, false, 0, 0);

			GL.BindBuffer(BufferTarget.ArrayBuffer, helper.VBOColor);
			GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(coldata.Length * Vector3.SizeInBytes), 
				coldata, 
				BufferUsageHint.StaticDraw
				);
			GL.VertexAttribPointer(helper.AttributeVcolor,3,VertexAttribPointerType.Float,false,0,0);


			time += (float)e.Time;
			etime = (float)e.Time;

			Inputs();
			Sides();
			 
			
			cube.ViewProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(1.3f, ClientSize.Width / (float)ClientSize.Height, 1.0f, 100.0f);
			cube.ViewMatrix = Matrix4.LookAt(new Vector3(0, 0, zdist), Vector3.Zero, Vector3.UnitY);
			cube.ModelViewProjectionMatrix = cube.ModelMatrix * cube.ViewMatrix * cube.ViewProjectionMatrix;

			GL.UseProgram(helper.ProgramId);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, helper.IBOElements);
			GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indicedata.Length * sizeof(int)), indicedata, BufferUsageHint.StaticDraw);

		}

	 

		private void Sides()
		{
			// perform rotation of the cube depending on the keyboard state
			if (autoRotate)
			{
				xangle = deltaTime / 2;
				yangle = deltaTime;
				cube.SetValueX(Matrix4.CreateRotationX(xangle));
				cube.SetValueY(Matrix4.CreateRotationY(yangle));
			}
			if (right)
			{
				yangle = deltaTime;
				
				cube.SetValueY(Matrix4.CreateRotationY(yangle));
			}
			if (left)
			{
				yangle = -deltaTime;
				cube.SetValueY(Matrix4.CreateRotationY(yangle));
			}
			if (up)
			{
				xangle = deltaTime;
				cube.SetValueX(Matrix4.CreateRotationX(xangle));
			}
			if (down)
			{
				xangle = -deltaTime;
				cube.SetValueX(Matrix4.CreateRotationX(xangle));
			}

		}

		private void Inputs()
		{

			OpenTK.Input.KeyboardState state = OpenTK.Input.Keyboard.GetState();

			if (state.IsKeyDown(Key.Space) && lastKeystate.IsKeyUp(Key.Space))
			{
				autoRotate = !autoRotate;
			}
			this.lastKeystate = state;
			if (state.IsKeyDown(Key.A))
			{
				left = true;
			}

			if (state.IsKeyDown(Key.S))
			{
				down = true;
			}
			if (state.IsKeyDown(Key.D))
			{
				right = true;
			}
			if (state.IsKeyDown(Key.W))
			{
				up = true;
			}

			if (state.IsKeyUp(Key.A))
			{
				left = false;
			}

			if (state.IsKeyUp(Key.S))
			{
				down = false;
			}
			if (state.IsKeyUp(Key.D))
			{
				right = false;
			}
			if (state.IsKeyUp(Key.W))
			{
				up = false;
			}
			if(state.IsKeyDown(Key.Up))
			{

			}
			if (state.IsKeyDown(Key.Down))
			{

			}
			if (state.IsKeyDown(Key.Right))
			{
				xdist = 0.05f;
				cube.SetTranslation(xdist,'x');
			}
			if (state.IsKeyDown(Key.Left))
			{
				//xdist -= 0.05f;

				//cube.SetTranslation(-xdist, 'X');
			}




			if (state.IsKeyDown(Key.Escape))
			{
				this.Exit();
			}



		}

		private void Mouse_WheelChanged(object sender, MouseWheelEventArgs e)
		{

			Console.WriteLine(
				" Whell... Value "+e.Value.ToString()+
				" Precision " + e.ValuePrecise.ToString()+
				" Delta " + e.Delta.ToString()+
				" DeltaPrecision " + e.DeltaPrecise.ToString()
				);

			// zoom in
			if(e.DeltaPrecise > 0)
			{
				zdist += 0.05f;
			}
			// zoom out
			if(e.DeltaPrecise < 0 )
			{
				zdist -= 0.05f;
			}



			return;
		}

		private void Mouse_Move(object sender, MouseMoveEventArgs e)
		{
			if (OpenTK.Input.Mouse.GetState()[OpenTK.Input.MouseButton.Left])
			{

				down = up = left = right = false;

				// Move Up
				if (e.YDelta < 0)
				{
					xangle = deltaTime * 2;
					cube.SetValueX(Matrix4.CreateRotationX(xangle));
				}
				// Move Down
				if (e.YDelta > 0)
				{
					xangle = -deltaTime * 2;
					cube.SetValueX(Matrix4.CreateRotationX(xangle));
				}
				// Move Left
				if (e.XDelta < 0 )
				{
					yangle = -deltaTime * 2;
					cube.SetValueY(Matrix4.CreateRotationY(yangle));
				}

				// Move Right
				if (e.XDelta > 0)
				{
					yangle = deltaTime * 2;
					cube.SetValueY(Matrix4.CreateRotationY(yangle));
				}
			}
		}
	}
}
