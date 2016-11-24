using OpenTK;
using System;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Input;
using OpenTkExample.SquareType;

namespace OpenTkExample
{
	class GameSquare : GameWindow
	{
		ShaderHelper helper;
		ShaderHelper squareHelper;
		Cube cube;
		Square square;

		int programId;

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

		Light light;

		// // // 
		int vboPosition, vboColor, vboModelView, iboElements;


		public GameSquare() :
			base(512, 512, new OpenTK.Graphics.GraphicsMode(32, 24, 0, 4))
		{
			Mouse.Move += Mouse_Move;
			Mouse.WheelChanged += Mouse_WheelChanged;
		}


		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			helper = new ShaderHelper(Shaders.VertexShaderX, Shaders.FragmentShaderX);
			squareHelper = new ShaderHelper(Shaders.VertexShaderX, Shaders.FragmentShaderX);
			init();
			cube = new Cube();
			cube.Position = new Vector3(2f, 2.0f/*-0.5f-0.5f + (float)Math.Sin(time)*/, 0f);
			cube.Rotation = new Vector3(0f, 0f, 0f);
			cube.Scale = new Vector3(1f, 1f, 1f);
			cube.CalculateModelMatrix();
			cube.CalculateNormals();

			light.Position = new Vector3(0, 0, -5);
			light.Intensities = new Vector3(1, 1, 1);

			square = new Square();
			square.Position = new Vector3(2f, 2.0f, 0f);
			square.Rotation = new Vector3(0f, 0f, 0f);
			square.Scale = new Vector3(1f, 1f, 1f);
			square.CalculateModelMatrix();



			GL.ClearColor(Color.Transparent);
			watch = System.Diagnostics.Stopwatch.StartNew();
		}

		private void init()
		{
			programId = GL.CreateProgram();
			if(programId == 0)
			{
				Console.WriteLine("Error Creating ProgramId "+programId);
				return;
			}
			int vertexShaderId = LoadShader(programId, Shaders.VertexShaderX, ShaderType.VertexShader);
			int fragmentShaderId = LoadShader(programId, Shaders.FragmentShaderX, ShaderType.FragmentShader);

			// Link Program
			GL.LinkProgram(programId);


			GL.GenBuffers(1, out vboPosition);
			GL.GenBuffers(1, out vboColor);
			GL.GenBuffers(1, out vboModelView);
			GL.GenBuffers(1, out iboElements);


		}


		public int GetAttribLocation(string attr)
		{
			int attribLoc = GL.GetAttribLocation(programId, attr);
			return attribLoc;
		}
		public int GetUniformLocation(string uniform)
		{
			int unfrm = GL.GetUniformLocation(programId, uniform);
			return unfrm;
		}

		public int LoadShader(int programId,string file, ShaderType type)
		{
			int address = GL.CreateShader(type);
			GL.ShaderSource(address, file);
			GL.CompileShader(address);
			GL.AttachShader(programId, address);
			Console.WriteLine(GL.GetShaderInfoLog(address));
			return address;
		}


		protected override void OnRenderFrame(FrameEventArgs e)
		{


			base.OnRenderFrame(e);

			GL.Viewport(0, 0, Width, Height);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);

			GL.EnableVertexAttribArray(GetAttribLocation("vPosition"));
			GL.EnableVertexAttribArray(GetAttribLocation("vColor"));


			GL.UniformMatrix4(GetUniformLocation("modelview"), false, ref square.ModelViewProjectionMatrix);
			GL.DrawElements(BeginMode.Triangles, square.IndicesCount, DrawElementsType.UnsignedInt, 0);
 

			GL.DisableVertexAttribArray(GetAttribLocation("vPosition"));
			GL.DisableVertexAttribArray(GetAttribLocation("vColor"));




			GL.Flush();
			SwapBuffers();
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			watch.Stop();
			deltaTime = (float)watch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
			watch.Restart();

			base.OnUpdateFrame(e);



			List<Vector3> squareverts = new List<Vector3>();
			List<int> squareinds = new List<int>();
			List<Vector3> squarecolors = new List<Vector3>();

			int vertCount = 0;

			squareverts.AddRange(square.GetVertex().ToList());
			squareinds.AddRange(square.GetIndices(vertCount).ToList());
			squarecolors.AddRange(square.GetColorData().ToList());

			// Square
			Vector3[] squarevertdata = squareverts.ToArray();
			int[] squareindicedata = squareinds.ToArray();
			Vector3[] squarecoldata = squarecolors.ToArray();

			// 
			GL.BindBuffer(BufferTarget.ArrayBuffer, vboPosition);
			GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(squarevertdata.Length * Vector3.SizeInBytes),
				squarevertdata,
				BufferUsageHint.StaticDraw
				);
			GL.VertexAttribPointer(GetAttribLocation("vPosition"), 3, VertexAttribPointerType.Float, false, 0, 0);

			GL.BindBuffer(BufferTarget.ArrayBuffer, vboColor);
			GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(squarecoldata.Length * Vector3.SizeInBytes),
				squarecoldata,
				BufferUsageHint.StaticDraw
				);
			GL.VertexAttribPointer(GetAttribLocation("vColor"), 3, VertexAttribPointerType.Float, false, 0, 0);

			time += (float)e.Time;
			etime = (float)e.Time;

			Inputs();
			Sides();


			square.ViewProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(1.3f, ClientSize.Width / (float)ClientSize.Height, 1.0f, 100.0f);
			square.ViewMatrix = Matrix4.LookAt(new Vector3(0, 0, zdist), Vector3.Zero, Vector3.UnitY);
			square.ModelViewProjectionMatrix = square.ModelMatrix * square.ViewMatrix * square.ViewProjectionMatrix;


			GL.UseProgram(programId);

			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, iboElements);
			GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(squareindicedata.Length * sizeof(int)), squareindicedata, BufferUsageHint.StaticDraw);


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
			if (state.IsKeyDown(Key.Right))
			{
				xdist = -0.05f;
				cube.SetTranslation(xdist, 'x');
			}
			if (state.IsKeyDown(Key.Left))
			{
				xdist = 0.05f;
				cube.SetTranslation(xdist, 'X');
			}
			if (state.IsKeyDown(Key.Up))
			{
				ydist = 0.05f;
				cube.SetTranslation(ydist, 'Y');
			}
			if (state.IsKeyDown(Key.Down))
			{
				ydist = -0.05f;
				cube.SetTranslation(ydist, 'Y');
			}
			if (state.IsKeyDown(Key.Escape))
			{
				this.Exit();
			}



		}

		private void Mouse_WheelChanged(object sender, MouseWheelEventArgs e)
		{
			// zoom in
			if (e.DeltaPrecise > 0)
			{
				zdist += 0.05f;
			}
			// zoom out
			if (e.DeltaPrecise < 0)
			{
				zdist -= 0.05f;
			}
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
				if (e.XDelta < 0)
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
