
#version 330

in vec3 vPosition;
in vec3 vColor;
out vec4 color;

in vec3 inNormal;
out vec3 vNormal;

uniform mat4 modelview; // mvp // projection
uniform mat4 view_matrix;
uniform mat4 model;

out vec3 eye_direction;

out vec3 view_vec;
out vec3 v_pos;


in vec3 vert_normal;

out vec3 frag_vert;
out vec3 frag_normal;




out vec3 fragNormal;

void main() {

	frag_vert = vPosition;
	frag_normal = inNormal;

	gl_Position = modelview * vec4(vPosition, 1.0);

	//vec4 vRes = view_matrix * vec4(inNormal, 0.0);
	//vNormal = vRes.xyz;
	//fragNormal = inNormal;
	//v_pos = vPosition;
	color = vec4(vColor, 1.0);
}