
#version 330
precision highp float;

in vec4 color;
out vec4 outputColor;

void main() {
	vec4 ncolor = color;
	//ncolor = 0.5;
	outputColor = ncolor;
}