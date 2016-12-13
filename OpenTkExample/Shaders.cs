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
			"#version 330			\n" +
			"in vec3 vPosition;		\n" +
			"in vec3 vNormal;		\n" +
	
			"out vec3 Normal;		\n" +
			"out vec3 FragPos;		\n" +
			"uniform mat4 modelview;\n" +
			"uniform mat4 model;	\n" + 
			"void main()			\n" +
			"{						\n" +
			"	gl_Position = modelview * vec4(vPosition,1.0);	\n" +
			"   FragPos = vec3(model* vec4(vPosition,1.0f));		\n" +
			"	Normal = mat3(transpose(inverse(model))) * vNormal; \n" +
			"}						\n"
			;


		public static string FragmentShaderX =
			"#version 330				\n" +
			"out vec4 color;			\n" +
			"in vec3 FragPos;			\n" +
			"in vec3 Normal;			\n" +	
			"uniform vec3 lightPos;		\n" +
			"uniform vec3 viewPos;		\n" +
			"uniform vec3 lightColor;	\n" +
			"uniform vec3 objectColor;	\n" +
			"void main()				\n" +
			"{							\n" +
			// Ambient
			"	float ambientStrength = 0.1f;	\n" +
			"	vec3 ambient = ambientStrength * lightColor;	\n" +
			// Diffuse
			"	vec3 norm = normalize(Normal);	\n" +
			"	vec3 lightDir = normalize(lightPos - FragPos);	\n" +
			"	float diff = max(dot(norm,lightDir),0.0);		\n" +
			"	vec3 diffuse = diff * lightColor;				\n" +
			// Specular
			"	float specularStrength = 0.5f;					\n" +
			"	vec3 viewDir = normalize(viewPos - FragPos);	\n" +
			"	vec3 reflectDir = reflect(-lightDir,norm);		\n" +
			"	float spec = pow(max(dot(viewDir,reflectDir),0.0),32);	\n" +
			"	vec3 specular = specularStrength * spec * lightColor;	\n"+
			// Result
			"	vec3 result = (ambient + diffuse + specular) * objectColor; \n" +
            //"	color = vec4(0.0f,1.0f,0.0f,0.0);									\n" +
            "   gl_FragColor = vec4(0.0f,0.0f,1.0f,1.0);  \n" +
            //"   gl_FragColor.a = 0.5f; \n"+
            //"	color = vec4(result,1.0f);									\n" +
			"}							\n"
			;
		// basic light
		//public static string VertexShaderX = 
		//	"#version 330			\n"+
		//	"in vec3 vPosition;		\n"+
		//	"in vec3 vColor;		\n"+
		//	"in vec3 vColorLight;	\n"+
		//	"out vec4 color;		\n"+ 
		//	"out vec4 lightcolor;	\n"+
		//	"uniform mat4 modelview;\n"+ 
		//	"void main()			\n"+
		//	"{						\n"+ 
		//	"	gl_Position = modelview * vec4(vPosition,1.0); \n"+
		//	"   float _b = vColor[2];	\n" + 
		//	"	float _g = vColor[1];	\n"+
		//	//"	vColor.x = vColor.x/2;	\n" +
		//	//"	vColor.y = vColor.y/2;	\n" +
		//	//"	vColor.z = vColor.z/2;	\n" +
		//	//"	color = vec4(vColor,1.0);	\n" +
		//	//"	lightcolor = vec4(1.0,0.5,1.0,1.0);	\n" +
		//	"	lightcolor = vec4(vColorLight,0.5);	\n" +
		//	"}"
		//	;


		//public static string FragmentShaderX =
		//	"#version 330				\n" +
		//	"in vec4 color;				\n" +
		//	"in vec4 lightcolor;		\n" +
		//	"uniform vec3 color1;		\n"+
		//	"uniform vec3 color2;		\n"+
		//	"out vec4 outputColor;		\n" +
		//	"void main()				\n" +
		//	"{							\n " +
		//	//"	color.x = color.x/2;	\n"+
		//	//"	color.y = color.y/2;	\n" +
		//	//"	color.z = color.z/2;	\n" +
		//	"							\n" +
		//	"	outputColor = vec4(color1*color2,1.0);	\n" +
		//	//"	outputColor = color*lightcolor;	\n" +
		//	"}							\n"
		//	;

		//*lightcolor

		//public static string VertexShaderX =
		//	"#version 330 \n" +
		//	"in vec3 vPosition; \n" +
		//	"in vec3 vColor;	\n" +
		//	"out vec4 color;	\n" +
		//	"uniform mat4 modelview; \n" +
		//	"void main() \n" +
		//	"{ \n" +
		//	"	gl_Position = modelview * vec4(vPosition,1.0); \n" +
		//	"   float _b = vColor[2];	\n" +
		//	"	float _g = vColor[1];	\n" +
		//	//"	vColor.x = vColor.x/2;	\n" +
		//	//"	vColor.y = vColor.y/2;	\n" +
		//	//"	vColor.z = vColor.z/2;	\n" +
		//	"	color = vec4(vColor,1.0);	\n" +
		//	//"	color[1] = _b*0.8;			\n" +
		//	//"	color[2] = _g;				\n" +
		//	"}"
		//	;


		//public static string FragmentShaderX = 
		//	"#version 330				\n"+
		//	"in vec4 color;				\n"+
		//	"out vec4 outputColor;		\n"+
		//	"void main()				\n"+
		//	"{							\n "+
		//	//"	color.x = color.x/2;	\n"+
		//	//"	color.y = color.y/2;	\n" +
		//	//"	color.z = color.z/2;	\n" +
		//	"							\n" +
		//	"	outputColor = color;	\n"+
		//	"}							\n"
		//	;


		//layout (location = 0) 

		public static string VertexLight =
			"#version 330	\n" +
			"layout (location = 0) in vec3 position;	\n"+
			"uniform mat4 model;	\n" +
			"uniform mat4 view;	\n" +
			"uniform mat4 projection;	\n" +
			"uniform mat4 modelview;	\n"+
			"void main()		\n" +
			"{	\n" +
			"	gl_Position = modelview * vec4(position,1.0); \n" +
			//"	gl_Position = lprojection * lview * lmodel * vec4(lposition,1.0f);	\n" +
			"}	\n" 
			;
		public static string FragmentLight =
			"#version 330	\n" +
			"out vec4 color;	\n" +
			"void main()	\n" +
			"{	\n" +
			"	color = vec4(1.0f);	\n" +
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
