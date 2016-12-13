//C:\CrossPlatform\OpenTkExample\OpenTkExample\vertex_shader.vs
#version 330	
precision highp float;
in vec3 vPosition;
in vec3 vNormal;		

out vec3 Normal;		
out vec3 FragPos;

in vec4 vColor;
out vec4 outColor;

uniform mat4 modelview;
uniform mat4 model;	

void main()			
{						 
	gl_Position = modelview * vec4(vPosition,1.0);	
	FragPos = vec3(model* vec4(vPosition,1.0f));	
	outColor = vColor;

	//Normal = mat3(transpose(inverse(model))) * vNormal; 
}	