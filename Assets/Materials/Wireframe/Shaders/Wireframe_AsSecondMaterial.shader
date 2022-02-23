Shader "Wireframe/As Second Material"
{
	Properties 
	{
		_LineWidth ("Line Width", Float) = 2
		_LineColor ("Line Color", Color) = (0, 1, 0, 1)
	}

	SubShader
	{
		Tags { "Queue" = "Transparent" }

		Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha DstAlpha
		ZWrite On

		Pass 
		{
			CGPROGRAM

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