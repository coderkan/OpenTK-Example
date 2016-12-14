
#version 330

in vec3 vPosition;
in vec3 vColor;
out vec4 color;

in vec3 inNormal;
out vec3 vNormal;

uniform mat4 modelview; // mvp // projection
uniform mat4 view_matrix;



void main() {
	gl_Position = modelview * vec4(vPosition, 1.0);
	vec4 vRes = view_matrix * vec4(inNormal, 0.0);
	vNormal = vRes.xyz;

	color = vec4(vColor, 1.0);
}