using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkExample
{
	class Shaders
	{
		// For Lighting
		public static string VertexShader =
			"#version 330 \n" +
			"in vec3 vPosition; \n" +
			"in vec3 vColor;	\n" +
			"out vec4 color;	\n" +
			"in vec3 vert;	\n" +
			"in vec3 vertNormal;	\n" +
			"out vec3 fragVert;	\n" +
			"out vec3 fragNormal;	\n" +
			"uniform mat4 modelview; \n" +
			"void main() \n" +
			"{ \n" +
			"	gl_Position = modelview * vec4(vPosition,1.0); \n" +
			"	color = vec4(vColor,1.0);	\n" +
			"   fragVert = vert;	\n" +
			"   fragNormal = vertNormal;	\n" +
			"}"
			;

		// For Lighting
		public static string FragmentShader =
			"#version 330	\n" +
			"in vec4 color;	\n" +
			"uniform mat4 modelview; \n" +
			"uniform struct Light {		\n" +
			"	vec3 position;		\n" +
			"	vec3 intensities;	\n" +
			"} light;		\n"+
			"in vec3 fragNormal;	\n" +
			"in vec3 fragVert;	\n" +
			"out vec4 outputColor;" + // final color
			"void main()	\n" +
			"{	\n" +
			"	mat3 normalMatrix = transpose(inverse(mat3(modelview)));	\n" +
			"	vec3 normal = normalize(normalMatrix * fragNormal);	\n" +
			"	vec3 fragPosition = vec3(model * vec4(fragVert,1));	\n" +
			"	vec3 surfaceToLight = light.position - fragPosition; \n" +
			"	float brightness = dot(normal,surfaceToLight) / (length(surfaceToLight) * length(normal));		\n" +
			"	brightness = clamp(brightness,0,1);		\n" +
			"	outputColor = vec4( brightness * light.intensities * color.rgb, color.a);   \n" +
			"	\n" +
			"	\n" +
			//"	outputColor = color;	\n" +
			"	\n" +
			"	\n" +
			"	\n" +
			"	\n" +
			"}	\n"
			;


		public static string VertexShaderX = 
			"#version 330 \n"+
			"in vec3 vPosition; \n"+
			"in vec3 vColor;	\n"+
			"out vec4 color;	\n"+ 
			"uniform mat4 modelview; \n"+ 
			"void main() \n" +
			"{ \n"+ 
			"	gl_Position = modelview * vec4(vPosition,1.0); \n"+
			"	color = vec4(vColor,1.0);	\n" +
			"}"
			;


		public static string FragmentShaderX = 
			"#version 330	\n"+
			"in vec4 color;	\n"+
			"out vec4 outputColor;"+
			"void main()	\n"+
			"{	\n"+
			"	outputColor = color;	\n"+
			"}	\n"
			;


		public static string VertexShaderXY =
			"#version 330 \n" +
			"in vec3 vPosition; \n" +
			"in vec3 vColor;	\n" +
			"out vec4 color;	\n" +
			"uniform mat4 modelview; \n" +
			"void main() \n" +
			"{ \n" +
			"	gl_Position = modelview * vec4(vPosition,1.0); \n" +
			"	color = vec4(vColor,1.0);	\n" +
			"}"
			;


	}
}
