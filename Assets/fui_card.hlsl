float3 hash3(float2 p) {
	float3 q = float3(dot(p, float2(127.1, 311.7)),
		dot(p, float2(269.5, 183.3)),
		dot(p, float2(419.2, 371.9)));
	return frac(sin(q) * 43758.5453);
}

float iqnoise(in float2 x, float u, float v) {
	float2 p = floor(x);
	float2 f = frac(x);

	float k = 1.0 + 63.0 * pow(1.0 - v, 4.0);

	float va = 0.0;
	float wt = 0.0;
	[unroll(100)]
	for (int j = -2; j <= 2; j++)
		[unroll(100)]
	for (int i = -2; i <= 2; i++)
	{
		float2 g = float2(float(i), float(j));
		float3 o = hash3(p + g) * float3(u, u, 1.0);
		float2 r = g - f + o.xy;
		float d = dot(r, r);
		float ww = pow(1.0 - smoothstep(0.0, 1.414, sqrt(d)), k);
		va += o.z * ww;
		wt += ww;
	}

	return va / wt;
}

float sdBox(in float2 p, in float2 b)
{
	float2 d = abs(p) - b;
	return length(max(d, float2(0, 0))) + min(max(d.x, d.y), 0.0);
}

float sdLine(in float2 p, in float2 a, in float2 b)
{
	float2 pa = p - a, ba = b - a;
	float h = clamp(dot(pa, ba) / dot(ba, ba), 0.0, 1.0);
	return length(pa - ba * h);
}

float3 not(float3 a) {
	return float3(!a.x, !a.y, !a.z);
}

float sdPoly( float2 v[10], float2 p)
{
	const int num = 10;
	float d = dot(p - v[0], p - v[0]);
	float s = 1.0;
	[unroll(100)]
	for (int i = 0, j = num - 1; i < num; j = i, i++)
	{
		// distance
		float2 e = v[j] - v[i];
		float2 w = p - v[i];
		float2 b = w - e * clamp(dot(w, e) / dot(e, e), 0.0, 1.0);
		d = min(d, dot(b, b));

		// winding number from http://geomalgorithms.com/a03-_inclusion.html
		float3 cond = float3(p.y >= v[i].y, p.y<v[j].y, e.x * w.y>e.y * w.x);
		if (all(cond) || all(not(cond))) s *= -1.0;
	}

	return s * sqrt(d);
}

float nrand(float2 st) {
	return frac(sin(dot(st.xy,
		float2(12.9898, 78.233))) *
		43758.5453123);
}

//card height is 1.618, when card width is 1.0
//card_c: card color
//line_c: line color
float4 fui_card(in float2 p, in float2 scale, in float4 card_c, in float4 line_c)
{
	float4 col = float4(0.0, 0.0, 0.0, 0.0);
	float god_r = 1.618;
	float m = 0.06;
	float tab_w = 0.55;

	float2 lt = float2(-scale.x, scale.y);
	float2 rt = float2(scale.x, scale.y);
	float2 lb = float2(-scale.x, -scale.y);
	float2 rb = float2(scale.x, -scale.y);

	float2 v0 = lt + float2(0.0, -m);
	float2 v1 = lt + float2(scale.x * m * 2.0, +m);
	float2 v2 = v1 + float2(scale.x * tab_w, 0.0);
	float2 v3 = v2 + float2(scale.x * m, -m);
	float2 v4 = rt;

	float2 v5 = rb + float2(0.0, +m);
	float2 v6 = rb + float2(-scale.x * m * 2.0, -m);
	float2 v7 = v6 + float2(-scale.x * tab_w, 0.0);
	float2 v8 = v7 + float2(-scale.x * m, +m);
	float2 v9 = lb;

	float d;

	float2 lw = float2(0.0, 0.02); //line width
	float2 lw2 = float2(0.0, 0.024); //line width

	//paper polygon
	float2 p_poly[10] = { v0, v1, v2, v3, v4, v5, v6, v7, v8, v9 };
	d = sdPoly(p_poly, p);
	if (d < 0.0) col = card_c;
	//nrand(float2(1.0, p.y));
	//(sin(fmod(p.y * 90.0, 2.0 * PI)) > 0.0 ? 1.0 : 0.0);


	//top line polygon
	float2 tl_poly[10] = { v0, v1, v2, v3, v4, v4 + lw, v3 + lw, v2 + lw2, v1 + lw2, v0 + lw2 };
	d = sdPoly(tl_poly, p);
	if (d < 0.0) col = line_c;

	//bottom line polygon
	float2 bl_poly[10] = { v5, v6, v7, v8, v9, v9 - lw, v8 - lw, v7 - lw2, v6 - lw2, v5 - lw2 };
	d = sdPoly(bl_poly, p);
	if (d < 0.0) col = line_c;

	return col;
}


void FUICard_float(
	float2 uv, float3 inPos,  float time, float4 card_c, float4 line_c,
	out float4 col, out float alpha)
{

	// Normalized pixel coordinates (from 0 to 1)
	float2 p = (uv * 2.0 - 1) / 1;

	float2 scale = float2(0.95, 0.9);
	float c_anim_speed = 15.0;
	//scale.y = lerp(0.0, 0.9, clamp(_Time.y * c_anim_speed, 0.0, 1.0));
	col = fui_card(p, scale, card_c, line_c);
	alpha = col.w;

	if (col.x == 0.0 && col.y == 0.0 && col.z == 0.0) discard;
	//if (col == float3(0.0, 0.0, 0.0), 0.0), 0.0)) discard;
}
