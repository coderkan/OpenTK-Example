#version 330
precision highp float;
in vec3 lPosition;

uniform mat4 lmodelview;
void main() {
	gl_Position = lmodelview * vec4(lPosition, 1.0);
	gl_PointSize = 15.0;
}