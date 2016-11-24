using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkExample
{
	public class BaseHelper
	{
		public int ProgramId { protected set; get; }

		public int VertexShaderId { protected set; get; }

		public int FragmentShaderId { protected set; get; }

		public int AttributeVcolor { protected set; get; }
		public int AttributeVPosition { protected set; get; }

		public int UniformModelView { protected set; get; }

		protected int vboPosition; // { protected set; get; }
		protected int vboColor;

		protected int vboModelView;
		protected int iboElements;
		protected int iboIndices;
		protected int vboNormal;

		public int Address { protected set; get; }

		public int LoadShader(string file,ShaderType type)
		{
			int address = GL.CreateShader(type);
			GL.ShaderSource(address, file);
			GL.CompileShader(address);
			GL.AttachShader(ProgramId, address);
			Console.WriteLine(GL.GetShaderInfoLog(address));
			return address;
		}

		public int VBOColor
		{
			get { return this.vboColor; }
		}

		public int VBOPosition
		{
			get { return this.vboPosition; }
		}

		public int VBOModelView
		{
			get { return this.vboModelView; }
		}
		public int IBOElements
		{
			get { return this.iboElements; }
		}
		public int IBOIndices
		{
			get { return this.iboIndices; }
		}
		public void SetUniform(string uniform,Vector3 val)
		{
			GL.Uniform3(GetUniform(uniform), val);
		}
		public int GetUniform(string s)
		{
			int uniform = GL.GetUniformLocation(ProgramId, s);
			return uniform;
		}
		public int GetAttrib(string att)
		{
			int attrib = GL.GetAttribLocation(ProgramId, att);
			return attrib;
		}
		public int VBONormal
		{
			get { return this.vboNormal; }
		}
 
	}
}
