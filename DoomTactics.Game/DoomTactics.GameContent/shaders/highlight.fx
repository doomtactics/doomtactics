sampler TextureSampler : register(s0);
float4 tint : COLOR0;

float4 Highlight(float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 tex = tex2D(TextureSampler, texCoord);
	tex.a = tint.a * tex.a;
	tex.r = tint.r * tex.r;
	tex.g = tint.g * tex.g;
    tex.b = tint.b * tex.b;
    return tex;
}

technique Blue
{
    pass Pass0
    {
        PixelShader = compile ps_2_0 Highlight();
    }
}
