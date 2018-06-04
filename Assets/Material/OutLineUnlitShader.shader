Shader "Custom/OutLineUnlitShader" {
	Properties
	{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_DisolveTex("DisolveTex (RGB)", 2D) = "white" {}
		_Threshold("Threshold", Range(0,1)) = 0.0
		_Color("Color", Color) = (1,1,1,1)
		_OutlineWidth("Outline Width", float) = 0.1
		_OutlineColor("Outline Color", Color) = (1,1,1,1)
		
	}
	SubShader
	{
		Tags{ "RenderType" = "Opaque" "Queue" = "Geometry" }

	Pass
	{

		// 前面をカリング
		Cull Front

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

		struct appdata
		{
			half4 vertex : POSITION;
			half3 normal : NORMAL;

			float2 tex : TEXCOORD0;
		};

		struct v2f
		{
			half4 pos : SV_POSITION;

			float2 uv : TEXCOORD0;
		};

		half _OutlineWidth;
		half4 _OutlineColor;

		half _Threshold;
		sampler2D _DisolveTex;

		v2f vert(appdata v)
		{
			v2f o = (v2f)0;
			o.uv = v.tex;

			// アウトラインの分だけ法線方向に拡大する
			o.pos = UnityObjectToClipPos(v.vertex + v.normal * _OutlineWidth);
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			fixed4 m = tex2D(_DisolveTex, i.uv);
			half g = m.r * 0.2 + m.g * 0.7 + m.b * 0.1;
			if (g < _Threshold) {
				discard;
			}

			return _OutlineColor;
		}
			ENDCG
		}
		//sampler2D _MainTex;
		// 2パス目は好きなようにレンダリングする
			Pass
		{
			Tags{ "LightMode" = "ForwardBase" }

			CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi__compile_fwdbase

#include "UnityCG.cginc"
#include "AutoLight.cginc"

		struct appdata
		{
			half4 vertex : POSITION;
			half3 normal : NORMAL;
			float2 tex : TEXCOORD0;
		};

		struct v2f
		{
			half4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
			half3 normal: TEXCOORD1;

			float3 lightDir:TEXCOORD2; // ライトへのベクトル
			float3 viewDir:TEXCOORD3; // カメラへのベクトル
			LIGHTING_COORDS(4, 5) //Unityのライディング支援関数で利用する
		};

		half4 _Color;
		half4 _LightColor0;
		half _Threshold;
		sampler2D _MainTex;
		sampler2D _DisolveTex;

		v2f vert(appdata v)
		{
			v2f o = (v2f)0;

			o.pos = UnityObjectToClipPos(v.vertex);
			o.normal = UnityObjectToWorldNormal(v.normal);
			o.uv = v.tex;

			o.lightDir = normalize(ObjSpaceLightDir(v.vertex));
			o.viewDir = normalize(ObjSpaceViewDir(v.vertex));

			TRANSFER_VERTEX_TO_FRAGMENT(o);

			

			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			half3 diff = max(0, dot(i.normal, _WorldSpaceLightPos0.xyz)) * _LightColor0;

			fixed4 col;
			//col.rgb = _Color * diff;
			col = tex2D(_MainTex, i.uv);

			fixed atten = LIGHT_ATTENUATION(i);
			float diffuse = max(0, mul(i.lightDir, i.normal));
			float specular = max(0, mul(normalize(i.viewDir + i.lightDir), i.normal));
			specular = pow(specular, 30);
			float4 color = UNITY_LIGHTMODEL_AMBIENT + (/*_Diffuse **/ _LightColor0 * diffuse + _LightColor0 * half4(1.0, 1.0, 1.0, 1.0) * specular) * atten;

			fixed4 m = tex2D(_DisolveTex, i.uv);
			half g = m.r * 0.2 + m.g * 0.7 + m.b * 0.1;
			if (g < _Threshold) {
				discard;
			}

			return col * color;
		}
			ENDCG
		}
	}
}