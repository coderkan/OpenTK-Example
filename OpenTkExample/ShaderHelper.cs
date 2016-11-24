using OpenTK.Graphics.OpenGL;
using System;

namespace OpenTkExample
{
	class ShaderHelper : BaseHelper
	{
		public ShaderHelper(string vertexShader,string fragmentShader)
		{

			ProgramId = GL.CreateProgram();
			if(ProgramId == 0)
			{
				Console.WriteLine("Error Create ProgramId " + ProgramId);
				return;
			}

			VertexShaderId = LoadShader(vertexShader, ShaderType.VertexShader);
			FragmentShaderId = LoadShader(fragmentShader, ShaderType.FragmentShader);

			GL.LinkProgram(ProgramId);

			AttributeVPosition = GL.GetAttribLocation(ProgramId, "vPosition");
			AttributeVcolor = GL.GetAttribLocation(ProgramId, "vColor");
			UniformModelView = GL.GetUniformLocation(ProgramId, "modelview");

			GL.GenBuffers(1, out vboPosition);
			GL.GenBuffers(1, out vboColor);
			GL.GenBuffers(1, out vboModelView);
			GL.GenBuffers(1, out iboElements);

			GL.GenBuffers(1, out vboNormal);
			





		}

	}
}
