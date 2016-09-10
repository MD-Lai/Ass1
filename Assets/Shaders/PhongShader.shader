// Phong Shader as adapted from Gouraud shader in the workshop

Shader "Unlit/PhongShader"
{
	Properties
	{
		_PointLightColor("Point Light Color", Color) = (0, 0, 0)
		_PointLightPosition("Point Light Position", Vector) = (0.0, 0.0, 0.0)
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform float3 _PointLightColor;    // variables that are set to be the same every render call
			uniform float3 _PointLightPosition; // both these set externally

			struct vertIn						// gets passed into vertex shader
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;         // pretty important, normal gives direction of normals
				float4 color : COLOR;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float4 normal : NORMAL;
				float4 color : COLOR;
				float4 worldvert : TEXCOORD0;
				float3 worldnorm : TEXCOORD1;
			};

			// Implementation of the vertex shader
			vertOut vert(vertIn v)
			{
				vertOut o;

				// Convert Vertex position and corresponding normal into world coords
				// Note that we have to multiply the normal by the transposed inverse of the world 
				// transformation matrix (for cases where we have non-uniform scaling; we also don't
				// care about the "fourth" dimension, because translations don't affect the normal) 
				// have to make sure vertex and normals are in the same space as light source
				float4 worldVertex = mul(_Object2World, v.vertex);
				float3 worldNormal = normalize(mul(transpose((float3x3)_World2Object), v.normal.xyz));

				// Transform vertex in world coordinates to camera coordinates
				// mandatory to transform vertices to screen space
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.worldnorm = worldNormal;
				o.worldvert = worldVertex;
				o.color = v.color;
				o.normal = v.normal;

				return o;
			}

			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : SV_Target
			{
				/* shading component */
				// Calculate ambient RGB intensities
				float Ka = 1;
				float3 amb = v.color.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * Ka;

				// Calculate diffuse RBG reflections, we save the results of L.N because we will use it again
				// (when calculating the reflected ray in our specular component)
				float fAtt = 1;
				float Kd = 1;
				float3 L = normalize(_PointLightPosition - v.worldvert.xyz);
				float LdotN = dot(L, v.worldnorm.xyz);
				float3 dif = fAtt * _PointLightColor.rgb * Kd * v.color.rgb * saturate(LdotN);

				// Calculate specular reflections
				float Ks = 0.15f;
				float specN = 90; // Values>>1 give tighter highlights
				float3 V = normalize(_WorldSpaceCameraPos - v.worldvert.xyz);
				float3 R = 2 * LdotN * v.worldnorm.xyz - L;
				float3 spe = fAtt * _PointLightColor.rgb * Ks * pow(saturate(dot(V, R)), specN);

				// Combine Phong illumination model components
				v.color.rgb = amb.rgb + dif.rgb + spe.rgb;
				v.color.a = v.color.a;
				/* end shading component */

				return v.color;
			}
			ENDCG
		}
	}
}
