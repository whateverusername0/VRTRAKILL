Shader "psx/unlit/emissive" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_EmissiveTex ("Emissive Mask Texture (G)", 2D) = "black" {}
		[Toggle] _UseAlbedoAsEmissive ("Use Base Texture as Emissve Color", Float) = 0
		[Toggle] _EmissiveReplaces ("Emissive Replaces Instead of Adding to Underlying Color", Float) = 0
		_EmissiveColor ("Emissive Color (RGB)", Vector) = (0,0,0,0)
		_EmissiveIntensity ("Emissive Strength", Float) = 1
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