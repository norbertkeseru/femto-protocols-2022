// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


#include "UnityCG.cginc"

float _LineWidth;
fixed4 _LineColor;

#ifdef WIREFRAME_SOLID
fixed4 _FillColor;
#endif


struct VsInput
{
	float4 pos: POSITION;
};

struct GsInput
{
	float4 pos: POSITION;
};

struct PsInput
{
	float4 pos: SV_POSITION;
	noperspective float4 info1: TEXCOORD0;
	noperspective float4 info2: TEXCOORD1;
	uint variant: TEXCOORD2;
};


float2 projToWindow(in float4 pos)
{
	return float2(_ScreenParams.x * 0.5 * (1.0 + pos.x / pos.w),
				  _ScreenParams.y * 0.5 * (1.0 - pos.y / pos.w));
}


GsInput vert(VsInput input)
{
	GsInput output;
	output.pos = UnityObjectToClipPos(input.pos);
	return output;
}


[maxvertexcount(3)]
void geom(triangle GsInput input[3], inout TriangleStream<PsInput> stream)
{
	PsInput vertex;

	vertex.variant = (input[0].pos.z < 0) * 4 + (input[1].pos.z < 0) * 2 + (input[2].pos.z < 0); 

	if (vertex.variant == 7)
		return;

	float2 points[3];
	points[0] = projToWindow(input[0].pos);
	points[1] = projToWindow(input[1].pos);
	points[2] = projToWindow(input[2].pos);
	
	if (vertex.variant == 0) 
	{
		vertex.info1 = float4(0, 0, 0, 0);
		vertex.info2 = float4(0, 0, 0, 0);

		float2 p0p1 = points[1] - points[0];
		float2 p1p2 = points[2] - points[1];
		float2 p2p0 = points[0] - points[2];
		float p0p1Len = length(p0p1);
		float p1p2Len = length(p1p2);
		float p2p0Len = length(p2p0);

		float p0Cos = dot(-p2p0, p0p1) / (p2p0Len * p0p1Len);
		float p1Cos = dot(-p0p1, p1p2) / (p0p1Len * p1p2Len);
		float p2Cos = dot(-p1p2, p2p0) / (p1p2Len * p2p0Len);

		float p1Height = p0p1Len * sqrt(1 - p0Cos * p0Cos); // p0p1Len * sine
		float p2Height = p1p2Len * sqrt(1 - p1Cos * p1Cos); // p1p2Len * sine
		float p0Height = p2p0Len * sqrt(1 - p2Cos * p2Cos); // p2p0Len * sine

		vertex.pos = input[0].pos;
		vertex.info1.x = 0;
		vertex.info1.y = p0Height;
		vertex.info1.z = 0;
		stream.Append(vertex);

		vertex.pos = input[1].pos;
		vertex.info1.x = 0;
		vertex.info1.y = 0;
		vertex.info1.z = p1Height;
		stream.Append(vertex);

		vertex.pos = input[2].pos;
		vertex.info1.x = p2Height;
		vertex.info1.y = 0;
		vertex.info1.z = 0;
		stream.Append(vertex);

		stream.RestartStrip();
	}
	else
	{
		const uint a[7]  = { 0, 0, 0, 0, 1, 1, 2 }; // A points for each case
		const uint b[7]  = { 1, 1, 2, 0, 2, 1, 2 }; // B points for each case
		const uint ad[7] = { 2, 2, 1, 1, 0, 0, 0 }; // C point for AC segment
		const uint bd[7] = { 2, 2, 1, 2, 0, 2, 1 }; // C point for BC segment

		vertex.info1.xy = points[a[vertex.variant]]; // the A point, in screen coords
		vertex.info2.xy = points[b[vertex.variant]]; // the B point, in screen coords

		vertex.info1.zw = normalize(vertex.info1.xy - points[ad[vertex.variant]]); // (A - C) direction, in screen coords
		vertex.info2.zw = normalize(vertex.info2.xy - points[bd[vertex.variant]]); // (B - C) direction, in screen coords

		vertex.pos = input[0].pos;
		stream.Append(vertex);

		vertex.pos = input[1].pos;
		stream.Append(vertex);

		vertex.pos = input[2].pos;
		stream.Append(vertex);

		stream.RestartStrip();
	}
}


#ifdef WIREFRAME_EXCLUDED_EDGE_TOPOLOGY
float calcMinDistanceToEdgesExt(in PsInput input, out float3 edgeSqDists, out uint3 nearestEdges)
{
	if (input.variant == 0)
	{
		edgeSqDists = input.info1.xyz * input.info1.xyz;
		nearestEdges = uint3(0, 1, 2);
		if (edgeSqDists[1] < edgeSqDists[0])
			nearestEdges.xy = nearestEdges.yx;
		if (edgeSqDists[2] < edgeSqDists[nearestEdges.y])
			nearestEdges.yz = nearestEdges.zy;
		if (edgeSqDists[2] < edgeSqDists[nearestEdges.x])
			nearestEdges.xy = nearestEdges.yx;
	}
	else
	{
		float2 af = input.pos.xy - input.info1.xy;
		float afSq = dot(af, af);
		float afCosA = dot(af, input.info1.zw); // dot(af, normalize(A - C))
		edgeSqDists[0] = abs(afSq - afCosA * afCosA);
		nearestEdges = uint3(0, 1, 2);

		float2 bf = input.pos.xy - input.info2.xy;
		float bfSq = dot(bf, bf);
		float bfCosB = dot(bf, input.info2.zw); // dot(bf, normalize(B - C))
		edgeSqDists[1] = abs(bfSq - bfCosB * bfCosB);

		if (edgeSqDists[1] < edgeSqDists[0])
			nearestEdges.xy = nearestEdges.yx;

		if (input.variant == 1 || input.variant == 2 || input.variant == 4)
		{
			float afCosA0 = dot(af, normalize(input.info2.xy - input.info1.xy));
			edgeSqDists[2] = abs(afSq - afCosA0 * afCosA0);

			if (edgeSqDists[2] < edgeSqDists[nearestEdges.y])
				nearestEdges.yz = nearestEdges.zy;

			if (edgeSqDists[2] < edgeSqDists[nearestEdges.x])
				nearestEdges.xy = nearestEdges.yx;
		}
		else
			edgeSqDists[2] = 0;
	}

	return sqrt(edgeSqDists[nearestEdges.x]);
}

#else

float calcMinDistanceToEdges(in PsInput input)
{
	float dist;
	if (input.variant == 0)
		dist = min(min(input.info1.x, input.info1.y), input.info1.z);
	else
	{
		float2 af = input.pos.xy - input.info1.xy;
		float afSq = dot(af, af);
		float afCosA = dot(af, input.info1.zw); // dot(af, normalize(A - C))
		dist = abs(afSq - afCosA * afCosA);

		float2 bf = input.pos.xy - input.info2.xy;
		float bfSq = dot(bf, bf);
		float bfCosB = dot(bf, input.info2.zw); // dot(bf, normalize(B - C))
		dist = min(dist, abs(bfSq - bfCosB * bfCosB));

		if (input.variant == 1 || input.variant == 2 || input.variant == 4)
		{
			float afCosA0 = dot(af, normalize(input.info2.xy - input.info1.xy));
			dist = min(dist, abs(afSq - afCosA0 * afCosA0));
		}

		dist = sqrt(dist);
	}
	return dist;
}
#endif


fixed4 frag(PsInput input) : SV_Target
{
#ifdef WIREFRAME_EXCLUDED_EDGE_TOPOLOGY
	float3 edgeSqDists;
	uint3 nearestEdges;
	float dist = calcMinDistanceToEdgesExt(input, edgeSqDists, nearestEdges);
#else
	float dist = calcMinDistanceToEdges(input);
#endif

	if (dist > 0.5 * _LineWidth + 1)
	{
#ifdef WIREFRAME_SOLID
		return _FillColor;
#else
		discard;
#endif
	}

#ifdef WIREFRAME_EXCLUDED_EDGE_TOPOLOGY
	const uint excludedEdge[7] = WIREFRAME_EXCLUDED_EDGE_TOPOLOGY;
	if (nearestEdges.x == excludedEdge[input.variant])
	{
		float widthRefSq = pow(0.5 * _LineWidth + 1, 2);
		if (edgeSqDists[nearestEdges.y] < widthRefSq)
			dist = sqrt(edgeSqDists[nearestEdges.y]);
		else if (edgeSqDists[nearestEdges.z] < widthRefSq)
			dist = sqrt(edgeSqDists[nearestEdges.z]);
		else
			discard;
	}
#endif
	
	dist = clamp(dist - (0.5 * _LineWidth - 1), 0, 2);
	float wireAlpha = exp2(-2 * dist * dist);

#ifdef WIREFRAME_SOLID
	float4 color = _LineColor * wireAlpha + _FillColor * (1 - wireAlpha);
#else
	fixed4 color = _LineColor;
	color.a *= wireAlpha;
#endif

	return color;
}
