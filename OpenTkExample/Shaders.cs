using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkExample
{
	class Shaders
	{

		public static string VertexShader = 
			"#version 330 \n"+
			"in vec3 vPosition; \n"+
			"in vec3 vColor;	\n"+
			"out vec4 color;	\n"+
			"uniform mat4 modelview; \n"+
			"void main() \n"+
			"{ \n"+
			"	gl_Position = modelview * vec4(vPosition,1.0); \n"+
			"	color = vec4(vColor,1.0);	\n"+
			"}"
			;


		public static string FragmentShader = 
			"#version 330	\n"+
			"in vec4 color;	\n"+
			"out vec4 outputColor;"+
			"void main()	\n"+
			"{	\n"+
			"	outputColor = color;	\n"+
			"}	\n"
			;




	}
}
