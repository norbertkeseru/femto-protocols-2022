Shader "Wireframe/Solid" 
{
	Properties 
	{
		_LineWidth ("Line Width", Float) = 2
		_LineColor ("Line Color", Color) = (0, 1, 0, 1)
		_FillColor ("Fill Color", Color) = (0.5, 0.5, 0.5, 1)
	}

	SubShader
	{
		Pass 
		{
			CGPROGRAM

			#define WIREFRAME_SOLID

			#include "Wireframe.cginc"

			#pragma only_renderers d3d11

			#pragma vertex vert
			#pragma geometry geom
			#pragma fragment frag

			ENDCG
		}
	}

	FallBack "Diffuse"
}