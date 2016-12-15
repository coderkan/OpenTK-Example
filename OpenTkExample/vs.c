
#version 330

in vec3 vPosition;
in vec3 vColor;
out vec4 color;

in vec3 inNormal;
out vec3 vNormal;

uniform mat4 modelview; // mvp // projection
uniform mat4 view_matrix;

out vec3 eye_direction;

out vec3 view_vec;
out vec3 v_pos;


void main() {
	gl_Position = modelview * vec4(vPosition, 1.0);
	vec4 vRes = view_matrix * vec4(inNormal, 0.0);
	vNormal = vRes.xyz;
	//view_vec = view_matrix.xyz;
	v_pos = vPosition;
	// can be put view_matrix
	//vec3 vert_pos = gl_Position.xyz;
	//eye_direction = vec3(0.0, 0.0, 0.0) - vert_pos;

	color = vec4(vColor, 1.0);
}