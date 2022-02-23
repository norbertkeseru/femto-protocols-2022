Shader "Wireframe/Two Sided" 
{
	Properties 
	{
		_LineWidth ("Line Width", Float) = 2
		_LineColor ("Line Color", Color) = (0, 1, 0, 1)
	}

	SubShader
	{
		Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha DstAlpha
		Cull Off

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