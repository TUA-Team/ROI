struct VertexInp
{
	float4 Position   	: POSITION;
	float4 Color		: COLOR0;
};

struct PixelInp
{
	float4 Color : COLOR0;
};

float4x4 xView;
float4x4 xProjection;
float4x4 xWorld;

VertexInp VertexShad(float4 inPos : POSITION, float4 inColor : COLOR)
{
	VertexInp Output = (VertexInp)0;
	float4x4 viewProj = mul(xView, xProjection);
	float4x4 wvpMatrix = mul(xWorld, viewProj);

	Output.Position = mul(inPos, wvpMatrix);
	Output.Color = inColor;

	return Output;
}

PixelInp PixelShad(VertexInp PSIn)
{
	PixelInp Output = (PixelInp)0;

	Output.Color = PSIn.Color;

	return Output;
}

technique CurvingShader
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 VertexShad();
		PixelShader = compile ps_2_0 PixelShad();
	}
}