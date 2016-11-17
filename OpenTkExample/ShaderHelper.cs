using OpenTK.Graphics.OpenGL;
using System;

namespace OpenTkExample
{
	class ShaderHelper
	{
		public int ProgramId { private set; get; }

		public int VertexShaderId { private set; get; }

		public int AttributeVcolor { private set; get; }
		public int AttributeVPosition { private set; get; }

		public int UniformModelView { private set; get; }

		public int VBOPosition { private set; get; }
		public int VBOColor { private set; get; }

		public int VBOModelView { private set; get; }
		public int IBOElements { private set; get; }

		public ShaderHelper(string vertexShader,string fragmentShader)
		{

			ProgramId = GL.CreateProgram();
			if(ProgramId == 0)
			{
				Console.WriteLine("Error Create ProgramId " + ProgramId);
				return;
			}









		}

	}
}
