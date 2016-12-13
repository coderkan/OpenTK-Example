﻿//C:\CrossPlatform\OpenTkExample\OpenTkExample\fragment_shader.fs

#version 330		
precision highp float;

out vec4 color;			     
in vec3 FragPos;			 
in vec3 Normal;		

in vec3 vObje;

uniform vec3 lightPos;		 
uniform vec3 viewPos;		 
uniform vec3 objectColor;	 
in vec3 lightColor;

in vec4 outColor;
out vec4 output_color;

void main()				     
{							 
/// Ambient
	float ambientStrength = 0.1f;	 
	output_color = ambientStrength * outColor;
	//vec3 ambient = ambientStrength * lightColor;	 
//// Diffuse                                            
//	vec3 norm = normalize(Normal);	           
//	vec3 lightDir = normalize(lightPos - FragPos);	 
//	float diff = max(dot(norm,lightDir),0.0);		 
//	vec3 diffuse = diff * lightColor;				 
//// Specular                                           
//	float specularStrength = 0.5f;					 
//	vec3 viewDir = normalize(viewPos - FragPos);	 
//	vec3 reflectDir = reflect(-lightDir,norm);		 
//	float spec = pow(max(dot(viewDir,reflectDir),0.0),32); 
//	vec3 specular = specularStrength * spec * lightColor;	 
//// Result
//	vec3 result = (ambient + diffuse + specular) * objectColor; 
	//gl_FragColor = vec4(0.0f, 0.0f, 1.0f,1.0);
	//gl_FragColor = vec4(lightColor,1.0);
	gl_FragColor = output_color;//vec4(1.0, 1.0, 0.0, 1.0);
  // gl_FragColor = vec4(0.0f,0.0f,1.0f,1.0);  
}						 


