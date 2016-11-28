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
		float zdist = 2.0f;// FISTDISTANCE + 2;

		int attribute_vcol;
		int attribute_vpos;
		int uniform_mview;
		int vbo_position;
		int vbo_color;
		int vbo_mview;
		int ibo_elements;

 
		Vector3[] vertdata;
		Vector3[] coldata;
		int[] indsdata;
		Matrix4[] mviewdata;
		//int vboPosition, vboColor, vboModelView, iboElements;
		Triangle triangle;

		public GameSquare() :
			base(512, 512, new OpenTK.Graphics.GraphicsMode(32, 24, 0, 4))
		{
			Mouse.Move += Mouse_Move;
			Mouse.WheelChanged += Mouse_WheelChanged;
		}


		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
 
			init();

			vertdata = new Vector3[] { new Vector3(-0.8f, -0.8f, 0f),
				new Vector3( 0.8f, -0.8f, 0f),
				new Vector3( 0f,  0.8f, 0f)};


			coldata = new Vector3[] {
				new Vector3( 0f, 0f, 1f),
				new Vector3( 0f, 0f, 1f),
				new Vector3( 0f, 0f, 1f)
			};

			indsdata = new int[]
			{
				0,1,2
			};

			mviewdata = new Matrix4[]{
				Matrix4.Identity
			};

			triangle = new Triangle(vertdata, coldata);
			triangle.Position = new Vector3(0f, 0f, -2.0f);
			triangle.Rotation = new Vector3(0f, 0f, 0f);
			triangle.Scale = new Vector3(1f, 1f, 1f);
			triangle.CalculateModelMatrix();
		 





			GL.ClearColor(Color.Red);
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


			attribute_vpos = GetAttribLocation("vPosition");
			attribute_vcol = GetAttribLocation("vColor");
			uniform_mview = GetUniformLocation("modelview");


			GL.GenBuffers(1, out vbo_position);
			GL.GenBuffers(1, out vbo_color);
			GL.GenBuffers(1, out vbo_mview);
			GL.GenBuffers(1, out ibo_elements);

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
			//GL.Enable(EnableCap.CullFace); //  

			GL.EnableVertexAttribArray(GetAttribLocation("vPosition"));
			GL.EnableVertexAttribArray(GetAttribLocation("vColor"));

			GL.UniformMatrix4(GetUniformLocation("modelview"), false, ref triangle.ModelViewProjectionMatrix);
			GL.DrawArrays(BeginMode.Triangles, 0, 3);

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

			// 
			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_position);
			GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertdata.Length * Vector3.SizeInBytes),
				vertdata,
				BufferUsageHint.StaticDraw
				);
			GL.VertexAttribPointer(GetAttribLocation("vPosition"), 3, VertexAttribPointerType.Float, false, 0, 0);

			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_color);
			GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(coldata.Length * Vector3.SizeInBytes),
				coldata,
				BufferUsageHint.StaticDraw
				);
			GL.VertexAttribPointer(GetAttribLocation("vColor"), 3, VertexAttribPointerType.Float, false, 0, 0);

			time += (float)e.Time;
			etime = (float)e.Time;

			Inputs();
			Sides();
		
			triangle.ViewProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(1.3f, ClientSize.Width / (float)ClientSize.Height, 1.0f, 100.0f);
			triangle.ViewMatrix = Matrix4.LookAt(new Vector3(0, 0, zdist), Vector3.Zero, Vector3.UnitY);
			triangle.ModelViewProjectionMatrix = triangle.ModelMatrix * triangle.ViewMatrix * triangle.ViewProjectionMatrix;

			GL.UseProgram(programId);

		}



		private void Sides()
		{
			// perform rotation of the cube depending on the keyboard state
			if (autoRotate)
			{
				xangle = deltaTime / 2;
				yangle = deltaTime;
				triangle.SetValueX(Matrix4.CreateRotationX(xangle));
				triangle.SetValueY(Matrix4.CreateRotationY(yangle));
			}
			if (right)
			{
				yangle = deltaTime;

				triangle.SetValueY(Matrix4.CreateRotationY(yangle));
			}
			if (left)
			{
				yangle = -deltaTime;
				triangle.SetValueY(Matrix4.CreateRotationY(yangle));
				Console.WriteLine("Left");
			}
			if (up)
			{
				xangle = deltaTime;
				triangle.SetValueX(Matrix4.CreateRotationX(xangle));
			}
			if (down)
			{
				xangle = -deltaTime;
				triangle.SetValueX(Matrix4.CreateRotationX(xangle));
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
				xdist = 0.05f;
				triangle.SetTranslation(xdist, 'X');
			}
			if (state.IsKeyDown(Key.Left))
			{
				xdist = -0.05f;
				triangle.SetTranslation(xdist, 'X');
			}
			if (state.IsKeyDown(Key.Up))
			{
				ydist = 0.05f;
				triangle.SetTranslation(ydist, 'Y');
			}
			if (state.IsKeyDown(Key.Down))
			{
				ydist = -0.05f;
				triangle.SetTranslation(ydist, 'Y');
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
					triangle.SetValueX(Matrix4.CreateRotationX(xangle));
				}
				// Move Down
				if (e.YDelta > 0)
				{
					xangle = -deltaTime * 2;
					triangle.SetValueX(Matrix4.CreateRotationX(xangle));
				}
				// Move Left
				if (e.XDelta < 0)
				{
					yangle = -deltaTime * 2;
					triangle.SetValueY(Matrix4.CreateRotationY(yangle));
				}

				// Move Right
				if (e.XDelta > 0)
				{
					yangle = deltaTime * 2;
					triangle.SetValueY(Matrix4.CreateRotationY(yangle));
				}
			}
		}
	}
}
