// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/FillShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_MaskTex ("Texture", 2D) = "white" {}
		_Threshold("Threshold",Range(0,1))=0.5
	}
	SubShader
	{
		Tags { "Queue" = "Transparent+2000" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 100
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				fixed4 vertex : POSITION;
				half2 uv : TEXCOORD0;
			};

			struct v2f
			{
				half2 uv : TEXCOORD0;
				fixed4 vertex : SV_POSITION;
			};

			uniform sampler2D _MainTex;
			uniform sampler2D _MaskTex;
			fixed4 _MainTex_ST;
			fixed _Threshold;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			half4 frag(v2f i) : COLOR
			{
				fixed4 c = tex2D(_MainTex, i.uv);
				fixed a = tex2D(_MaskTex, i.uv).a;
				c.a *= a >= _Threshold ? 0 : 1;
				return c;
			}
			ENDCG
		}
	}
}
