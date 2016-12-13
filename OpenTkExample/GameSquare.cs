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
		int programIdLight;

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

		// Lights
		int lattribute_vcol;
		int lattribute_vpos;
		int luniform_mview;
		int lvbo_position;
		int lvbo_color;
		int lvbo_mview;
		int libo_elements;
		Vector3[] lvertdata;
		Vector3[] lcoldata;
		Vector3[] lncoldata;
		int[] lindsdata;
		Matrix4[] lmviewdata;


		Vector3[] vertdata;
		Vector3[] coldata;
		Vector3[] ncoldata;
		Vector3[] normaldata;
		int[] indsdata;
		Matrix4[] mviewdata;
		//int vboPosition, vboColor, vboModelView, iboElements;
		Triangle triangle;
		Triangle lamp;
		Vector3[] lightColor;

		public GameSquare() :
			base(512, 512, new OpenTK.Graphics.GraphicsMode(32, 24, 0, 4))
		{
			Mouse.Move += Mouse_Move;
			Mouse.WheelChanged += Mouse_WheelChanged;
		}

		public Vector3[] CalculateNormals(Vector3[] verts)
		{
			Vector3[] normals = new Vector3[verts.Length];
			int[] inds = new int[] { 0, 1, 2 };
			for (int i = 0; i < inds.Length; i += 3)
			{
				Vector3 v1 = verts[inds[i]];
				Vector3 v2 = verts[inds[i + 1]];
				Vector3 v3 = verts[inds[i + 2]];

				normals[inds[i]] += Vector3.Cross(v2 - v1, v3 - v1);
				normals[inds[i + 1]] += Vector3.Cross(v2 - v1, v3 - v1);
				normals[inds[i + 2]] += Vector3.Cross(v2 - v1, v3 - v1);
			}
			for (int i = 0; i < normals.Length; i++)
			{
				normals[i] = normals[i].Normalized();
			}
			for(int j = 0; j < normals.Length; j++)
			{
				Console.WriteLine("X : " + normals[j].X + " Y : "+ normals[j].Y + " Z : " + normals[j].Z );
			}
			//Normals = normals;
			return normals;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
 
			init();
			initLight();
			vertdata = new Vector3[] { new Vector3(-0.8f, -0.8f, 0f),
				new Vector3( 0.8f, -0.8f, 0f),
				new Vector3( 0f,  0.8f, 0f)};


			coldata = new Vector3[] {
				new Vector3( 0f, 0f, 1f),
				new Vector3( 0f, 0f, 1f),
				new Vector3( 0f, 0f, 1f)
			};


			normaldata = CalculateNormals(vertdata);



			ncoldata = new Vector3[] {
				new Vector3( 1f, 0f, 1f),
				new Vector3( 1f, 0f, 1f),
				new Vector3( 1f, 0f, 1f)
			};

			lightColor = new Vector3[] {
				new Vector3( 1f, 1f, 1f),
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


			lamp = new Triangle(vertdata, coldata);
			lamp.Position = new Vector3(0f, 0f, 0.0f);
			lamp.Rotation = new Vector3(0f, 0f, 0f);
			lamp.Scale = new Vector3(0.05f, 0.05f, 0.05f);
			lamp.CalculateModelMatrix();






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
			//int ff = GetAttribLocation("vColorLight");
			GL.GenBuffers(1, out vbo_position);
			GL.GenBuffers(1, out vbo_color);
			GL.GenBuffers(1, out vbo_mview);
			GL.GenBuffers(1, out ibo_elements);

		}
		private void initLight()
		{
			programIdLight = GL.CreateProgram();
			if(programIdLight == 0)
			{
				Console.WriteLine("Error Creating ProgramId For Light " + programIdLight);
				return;
			}
			int vertexShaderId = LoadShader(programIdLight, Shaders.VertexLight, ShaderType.VertexShader);
			int fragmentShaderId = LoadShader(programIdLight, Shaders.FragmentLight, ShaderType.FragmentShader);

			GL.LinkProgram(programIdLight);

			lattribute_vpos = GL.GetAttribLocation(programIdLight, "position");
			luniform_mview = GL.GetUniformLocation(programIdLight, "modelview");

			GL.GenBuffers(programIdLight, out lvbo_position);
			GL.GenBuffers(programIdLight, out lvbo_mview);

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

			watch.Stop();
			deltaTime = (float)watch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
			watch.Restart();



			base.OnRenderFrame(e);

			GL.Viewport(0, 0, Width, Height);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.Enable(EnableCap.DepthTest);
            //GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            //glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

            GL.UseProgram(programId);

			GL.EnableVertexAttribArray(GetAttribLocation("vPosition"));
			GL.UniformMatrix4(GetUniformLocation("modelview"), false, ref triangle.ModelViewProjectionMatrix);
			GL.DrawArrays(BeginMode.Triangles, 0, 3);
			GL.DisableVertexAttribArray(GetAttribLocation("vPosition"));
			// 
			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_position);
			GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertdata.Length * Vector3.SizeInBytes),
				vertdata,
				BufferUsageHint.StaticDraw
				);
			GL.VertexAttribPointer(GetAttribLocation("vPosition"), 3, VertexAttribPointerType.Float, false, 0, 0);

			int nrmls;
			GL.GenBuffers(1, out nrmls);
			GL.BindBuffer(BufferTarget.ArrayBuffer, nrmls);
			GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(normaldata.Length * Vector3.SizeInBytes),			normaldata,
				BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(GetAttribLocation("vNormal"), 3, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexAttribArray(GL.GetAttribLocation(programId, "vNormal"));
			GL.BindVertexArray(0);



			int objectColorLoc = GL.GetUniformLocation(programId, "objectColor");
			int lightColorLoc = GL.GetUniformLocation(programId, "lightColor");
            int modelLoc = GL.GetUniformLocation(programId, "model");

            GL.Uniform4(modelLoc, 0.0f, 1.0f, 0.0f,1.0f);
            GL.Uniform3(objectColorLoc, 0.0f, 1.0f, 1.0f);
			GL.Uniform3(lightColorLoc, 1.0f, 1.0f, 1.0f);

 


			time += (float)e.Time;
			etime = (float)e.Time;

			Inputs();
			Sides();

			triangle.ViewProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(1.3f, ClientSize.Width / (float)ClientSize.Height, 1.0f, 100.0f);
			triangle.ViewMatrix = Matrix4.LookAt(new Vector3(0, 0, zdist), Vector3.Zero, Vector3.UnitY);
			triangle.ModelViewProjectionMatrix = triangle.ModelMatrix * triangle.ViewMatrix * triangle.ViewProjectionMatrix;


			GL.UseProgram(programIdLight);


			int s = GL.GetAttribLocation(programIdLight, "position"); //GetAttribLocation("position");
			if (s == -1)
				Console.WriteLine("Attrib is not created.");
			GL.EnableVertexAttribArray(s);
			int uniform = GL.GetUniformLocation(programIdLight,"modelview");

			if (uniform == -1)
				Console.WriteLine("Attrib is not created.");

			GL.UniformMatrix4(uniform, false, ref lamp.ModelViewProjectionMatrix);
			GL.DrawArrays(BeginMode.Triangles, 0, 3);

			GL.DisableVertexAttribArray(s);


			GL.BindBuffer(BufferTarget.ArrayBuffer, lvbo_position);
			GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(lamp.GetVertex().Length * Vector3.SizeInBytes), lamp.GetVertex(), BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(GL.GetAttribLocation(programIdLight,"position"), 3, VertexAttribPointerType.Float, false, 0, 0);
			// 


			lamp.ViewProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(1.3f, ClientSize.Width / (float)ClientSize.Height, 1.0f, 100.0f);
			lamp.ViewMatrix = Matrix4.LookAt(new Vector3(0, 0, zdist), Vector3.Zero, Vector3.UnitY);
			lamp.ModelViewProjectionMatrix = lamp.ModelMatrix * lamp.ViewMatrix * lamp.ViewProjectionMatrix;

			GL.Flush();
			SwapBuffers();
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

			if (state.IsKeyDown(Key.F1))
			{
				triangle.SetColors(ncoldata);
			}
			if(state.IsKeyDown(Key.F2))
			{
				triangle.SetColors(coldata);
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
