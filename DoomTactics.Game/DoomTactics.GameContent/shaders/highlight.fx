sampler TextureSampler : register(s0);

float4 PixelShaderFunction(float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 tex = tex2D(TextureSampler, texCoord);
	tex.r = 0.75 * tex.r;
	tex.g = 0.75 * tex.g;
    tex.b = 1.5 * tex.b;
    return tex;
}

technique Highlight
{
    pass Pass0
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
