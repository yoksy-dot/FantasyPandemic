Shader "Custom/DisloveCloud" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	_DisolveTex("DisolveTex (RGB)", 2D) = "white" {}
	_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_Strength("Strength", Range(0,1)) = 0.0
		//_Alpha("Alpha", Range(0,1)) = 0.0
	}
		SubShader{
		Tags{ "Queue" = "Transparent" }
		LOD 200

		CGPROGRAM
#pragma surface surf Standard alpha:fade 
#pragma target 3.0

		sampler2D _MainTex;
	sampler2D _DisolveTex;

	struct Input {
		float2 uv_MainTex;
	};

	half _Glossiness;
	half _Metallic;
	half _Strength;
	half _Alpha;
	fixed4 _Color;

	UNITY_INSTANCING_CBUFFER_START(Props)
		UNITY_INSTANCING_CBUFFER_END

		void surf(Input IN, inout SurfaceOutputStandard o) {
		// Albedo comes from a texture tinted by color
		fixed4 m = tex2D(_DisolveTex, IN.uv_MainTex);
		half g = m.r * 0.2 + m.g * 0.7 + m.b * 0.1;
		if (0.25 <= g) {
			_Alpha = 0.25 + _Strength;
		}
		else if (g > 0.25 && 0.5 <= g) {
			_Alpha = 0.5 + _Strength;
		}
		else if (g > 0.5 && 0.75 <= g) {
			_Alpha = 0.75 + _Strength;
		}
		else {
			_Alpha = 1;
		}

		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		// Metallic and smoothness come from slider variables
		o.Metallic = _Metallic;
		o.Smoothness = _Glossiness;
		o.Alpha = c.a * (1 - _Alpha);
	}
	ENDCG
	}
		FallBack "Diffuse"
}