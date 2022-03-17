Shader "Unlit/Stripes_45"
{
	Properties
	{
		_Stripenumber("Stripenumber", Range(0,30)) = 0
		_Direction("Direction", Range(0, 90)) = 0
		_Color1("Color 1", Color) = (0.5, 0.5, 0.5, 1)
		_Color2("Color 2", Color) = (0, 0, 0, 1)
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			float _Stripenumber;
			float _Direction;
			float pos;
			float sn1;
			float sn2;
			float dir;
			fixed4 _Color1;
			fixed4 _Color2;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				dir = _Direction / 90;
				sn1 = _Stripenumber / cos(dir * 3.14159265358979 / 2);
				sn2 = _Stripenumber / sin(dir * 3.14159265358979 / 2);
				if (dir < 0.5)
				{
					pos = lerp(i.uv.x * sn1, i.uv.y * sn1 / 5, dir);
				}
				else
				{
					pos = lerp(i.uv.x * sn2, i.uv.y * sn2 / 5, dir);
				}
				fixed value = floor(frac(pos) + 0.5);
				return lerp(_Color1, _Color2, value);
			}
			ENDCG
		}
	}
}
