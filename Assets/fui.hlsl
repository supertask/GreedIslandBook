float dot2(in float3 v) { return dot(v, v); }
float udTriangle(float3 p, float3 a, float3 b, float3 c)
{
	float3 ba = b - a; float3 pa = p - a;
	float3 cb = c - b; float3 pb = p - b;
	float3 ac = a - c; float3 pc = p - c;
	float3 nor = cross(ba, ac);

	return sqrt(
		(sign(dot(cross(ba, nor), pa)) +
			sign(dot(cross(cb, nor), pb)) +
			sign(dot(cross(ac, nor), pc)) < 2.0)
		?
		min(min(
			dot2(ba * clamp(dot(ba, pa) / dot2(ba), 0.0, 1.0) - pa),
			dot2(cb * clamp(dot(cb, pb) / dot2(cb), 0.0, 1.0) - pb)),
			dot2(ac * clamp(dot(ac, pc) / dot2(ac), 0.0, 1.0) - pc))
		:
		dot(nor, pa) * dot(nor, pa) / dot2(nor));
}

void FUI_float(float2 uv, float3 inPos,  float time, out float3 outPos, out float3 col)
{
 	//float t = time * 0.2;
 	float2 p = 2.0 * uv.xy - 1.0;

 	//col = lerp(float3(1,0,0), float3(0,0,0), distance(p, float2(0.0, 0.0)));
	float3 tri = udTriangle(float3(p, inPos.y), float3(-0.5, -0.5, 0.0), float3(0.5,-0.5,0.0), float3(0.0, 0.5, 0.0));
	col = float3(1,0,0) * step(tri.x, 0.00001);
	//col = tri;
	outPos = inPos;
}
