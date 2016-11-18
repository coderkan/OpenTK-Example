using OpenTK;
using System;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace OpenTkExample
{
	class Game :GameWindow
	{
		ShaderHelper helper;
		Cube cube;
		float time = 0.0f;
		public Game():
			base(512,512,new OpenTK.Graphics.GraphicsMode(32, 24, 0, 4))
		{

		}
		

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			helper = new ShaderHelper(Shaders.VertexShader, Shaders.FragmentShader);
			cube = new Cube();
			GL.ClearColor(Color.Blue);
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);

			GL.Viewport(0, 0, Width, Height);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.Enable(EnableCap.DepthTest);


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

			cube.Position = new Vector3(0.3f, -0.5f + (float)Math.Sin(time), -3.0f);
			cube.Rotation = new Vector3(0.55f * time, 0.25f * time, 0);
			cube.Scale = new Vector3(0.5f, 0.5f, 0.5f);

			cube.CalculateModelMatrix();
			cube.ViewProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(1.3f, ClientSize.Width / (float)ClientSize.Height, 1.0f, 40.0f);
			cube.ModelViewProjectionMatrix = cube.ModelMatrix * cube.ViewProjectionMatrix;

			GL.UseProgram(helper.ProgramId);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, helper.IBOElements);
			GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indicedata.Length * sizeof(int)), indicedata, BufferUsageHint.StaticDraw);


		}






	}
}
