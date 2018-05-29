// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/disolve" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	_DisolveTex("DisolveTex (RGB)", 2D) = "white" {}
	_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_Threshold("Threshold", Range(0,1)) = 0.0
		_ToonShade("ToonShader Cubemap(RGB)", CUBE) = "" { }
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
#pragma surface surf Standard fullforwardshadows
#pragma target 3.0
#pragma multi_compile_fog

#include "UnityCG.cginc"

		sampler2D _MainTex;
	sampler2D _DisolveTex;
	samplerCUBE _ToonShade;

	struct Input {
		float2 uv_MainTex;
	};

	struct appdata {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
		float3 normal : NORMAL;
	};

	struct v2f {
		float4 pos : SV_POSITION;
		float2 texcoord : TEXCOORD0;
		float3 cubenormal : TEXCOORD1;
		UNITY_FOG_COORDS(2)
	};

	half _Glossiness;
	half _Metallic;
	half _Threshold;
	fixed4 _Color;

	UNITY_INSTANCING_CBUFFER_START(Props)
		UNITY_INSTANCING_CBUFFER_END


		v2f vert(appdata v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
		o.cubenormal = mul(UNITY_MATRIX_MV, float4(v.normal, 0));
		UNITY_TRANSFER_FOG(o, o.pos);
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		fixed4 col = _Color * tex2D(_MainTex, i.texcoord);
	fixed4 cube = texCUBE(_ToonShade, i.cubenormal);
	fixed4 c = fixed4(2.0f * cube.rgb * col.rgb, col.a);
	UNITY_APPLY_FOG(i.fogCoord, c);
	return c;
	}

		void surf(Input IN, inout SurfaceOutputStandard o) {
		// Albedo comes from a texture tinted by color
		fixed4 m = tex2D(_DisolveTex, IN.uv_MainTex);
		half g = m.r * 0.2 + m.g * 0.7 + m.b * 0.1;
		if (g < _Threshold) {
			discard;
		}

		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		// Metallic and smoothness come from slider variables
		o.Metallic = _Metallic;
		o.Smoothness = _Glossiness;
		o.Alpha = c.a;
	}

	
	ENDCG
	}
		FallBack "Diffuse"
}