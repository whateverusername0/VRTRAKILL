Shader "psx/unlit/unlit-customcolors" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		_CustomColor1 ("Custom Color 1", Vector) = (1,1,1,1)
		_CustomColor2 ("Custom Color 2", Vector) = (1,1,1,1)
		_CustomColor3 ("Custom Color 3", Vector) = (1,1,1,1)
		_HighlightStrength ("CC Highlight Strength", Range(0, 1)) = 0.5
		_HighlightFalloff ("CC Highlight Falloff", Range(0.5, 10)) = 2
		_IDTex ("ID Texture", 2D) = "white" {}
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_VertexWarpScale ("Vertex Warping Scalar", Range(0, 10)) = 1
		[Toggle] _Outline ("Assist Outline", Float) = 0
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
}