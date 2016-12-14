
#version 330
precision highp float;

in vec4 color;
out vec4 outputColor;

in vec3 vNormal;

 

//vec3 ambient = vec3(0.1, 0.1, 0.1);
//vec3 lightVecNormalized = normalize(0.5, 0.5, 2.0);
//vec3 lightColor = vec3(0.9, 0.9, 0.7);

void main() {
	vec4 ncolor = color;
	vec3 ambient = vec3(0.1, 0.1, 0.1);
	vec3 lightVector = vec3(0.5, 0.5, 2.0);
	vec3 lightColor = vec3(0.9, 0.9, 0.7);

	vec3 lightVecNormalized = normalize(lightVector);
	float dotproduct = dot(lightVecNormalized, normalize(vNormal));
	float diffuse = clamp(dotproduct, 0.0, 1.0);
	//float diffuse = clamp(dot(lightVecNormalized, normalize(vNormal)), 0.0, 1.0);

	//ncolor = 0.5;
	//float diffuse = clamp(dot(lightVecNormalized, normalize(vNormal)), 0.0, 1.0);
	outputColor = vec4(ambient + diffuse * lightColor, 1.0);;
}