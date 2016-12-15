
#version 330
precision highp float;

in vec4 color;
out vec4 outputColor;

in vec3 vNormal;

in vec3 eye_direction;
in vec3 view_vec;
in vec3 v_pos;


uniform mat4 view_matrix;

//vec3 ambient = vec3(0.1, 0.1, 0.1);
//vec3 lightVecNormalized = normalize(0.5, 0.5, 2.0);
//vec3 lightColor = vec3(0.9, 0.9, 0.7);

void main() {

	vec4 ncolor = color;
	vec3 ambient = vec3(0.1, 0.1, 0.1);
	//vec3 lightVector = vec3(0.5, 0.5, 2.0);
	vec3 lightVector = vec3(1.0, 0.0, 1.0);
	vec3 lightColor = vec3(0.9, 0.9, 0.7);

	vec3 lightVecNormalized = normalize(lightVector);
	vec3 inverselightVecNormalized = normalize(-lightVector);

	float dotproduct = dot(lightVecNormalized, normalize(vNormal));
	float diffuse = clamp(dotproduct, 0.0, 1.0);

	vec4 diffuse_color = vec4(ambient + diffuse * lightColor, 1.0);

	outputColor = ncolor * diffuse_color;

	// specular 
	vec3 reflectionvec = normalize(reflect(-lightVector, vNormal));
	vec3 viewvec = normalize(vec3(inverse(view_matrix) * vec4(0, 0, 0, 1)) - v_pos);
	float material_specularreflection = max(dot(vNormal, lightVector), 0.0) * pow(max(dot(reflectionvec, viewvec), 0.0), 2.0);

	vec3 material_specular = vec3(0.3, 0.3, 0.3);

	outputColor = outputColor + vec4(material_specular*lightColor, 0.0) * material_specularreflection;










	//vec4 ncolor = color;
	//
	////float lightPower = 50.f;

	//vec3 ambient = vec3(0.1, 0.1, 0.1);
	////vec3 lightVector = vec3(0.5, 0.5, 2.0);
	//vec3 lightVector = vec3(0.0, 0.0, 0.0); // light pos
	//vec3 lightColor = vec3(1.0, 1.0, 1.0);



	//vec3 lightVecNormalized = normalize(lightVector);

	//float dotproduct = dot(lightVecNormalized, normalize(vNormal));

	//float diffuse = clamp(dotproduct, 0.0, 1.0);

	//vec4 diffuse_color = vec4(ambient + (diffuse*lightColor), 1.0);

	//outputColor = diffuse_color;

	//outputColor = vec4(ambient + diffuse * lightColor, 1.0);

	////float diffuse = clamp(dot(lightVecNormalized, normalize(vNormal)), 0.0, 1.0);

	//vec3 diffuse_color = ncolor.xyz;
	//vec3 ambientColor = ambient*diffuse_color;
	//vec3 specularColor = vec3(0.3, 0.3, 0.3);



	//vec4 v = vec4(lightVector, 0.0);
	//float distance = length(v - gl_FragCoord);

	//vec3 n = normalize(vNormal);
	//vec3 l = normalize(lightVector);

	//float cosTheta = clamp(dot(n, l), 0, 1);

	//vec3 E = normalize(eye_direction);
	//vec3 R = reflect(-l, n);
	//float cosAlpha = clamp(dot(E, R), 0, 1);

	//vec3 _color =
	//	ambientColor +
	//	diffuse_color * lightColor * lightPower * cosTheta / (distance * distance) +
	//	specularColor * lightColor * lightPower * pow(cosAlpha, 5) / (distance * distance);

	////outputColor = color*vec4(ambient + diffuse * lightColor, 1.0);;
	//outputColor = vec4(_color,1.0);

	 

	//ncolor = 0.5;
	//float diffuse = clamp(dot(lightVecNormalized, normalize(vNormal)), 0.0, 1.0);
	//outputColor = color*vec4(ambient + diffuse * lightColor, 1.0);
}


//void main()
//{
//	/// Ambient
//	float ambientStrength = 0.1f;
//	output_color = ambientStrength * outColor;
//	//vec3 ambient = ambientStrength * lightColor;	 
//	//// Diffuse                                            
//	//	vec3 norm = normalize(Normal);	           
//	//	vec3 lightDir = normalize(lightPos - FragPos);	 
//	//	float diff = max(dot(norm,lightDir),0.0);		 
//	//	vec3 diffuse = diff * lightColor;				 
//	//// Specular                                           
//	//	float specularStrength = 0.5f;					 
//	//	vec3 viewDir = normalize(viewPos - FragPos);	 
//	//	vec3 reflectDir = reflect(-lightDir,norm);		 
//	//	float spec = pow(max(dot(viewDir,reflectDir),0.0),32); 
//	//	vec3 specular = specularStrength * spec * lightColor;	 
//	//// Result
//	//	vec3 result = (ambient + diffuse + specular) * objectColor; 
//	//gl_FragColor = vec4(0.0f, 0.0f, 1.0f,1.0);
//	//gl_FragColor = vec4(lightColor,1.0);
//	gl_FragColor = output_color;//vec4(1.0, 1.0, 0.0, 1.0);
//								// gl_FragColor = vec4(0.0f,0.0f,1.0f,1.0);  
//}