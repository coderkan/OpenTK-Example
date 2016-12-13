// C:\\CrossPlatform\\OpenTkExample\\OpenTkExample\\light_vertex.c
#version 330												 
layout (location = 0) in vec3 position;						 
uniform mat4 model;											 
uniform mat4 view;											 
uniform mat4 projection;									 
uniform mat4 modelview;										 
void main()													 
{															 
	gl_Position = modelview * vec4(position,1.0);
	gl_PointSize = 5.0;
}	

