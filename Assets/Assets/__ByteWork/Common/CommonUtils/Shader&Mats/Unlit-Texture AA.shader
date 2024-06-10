// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Unlit shader. Simplest possible textured shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Unlit/Texture AA" {
Properties {
	_MainTex ("mainTexture", 2D) = "white" {}
	_ULeft("U Left", float) = 0
	_URight("U Right", float) = 1
	_VBottom("V Bottom", float) = 0
	_VTop("V Top", float) = 1
	_UOffset("U Offset", float) = 0
	_VOffset("V Offset", float) = 0
	_Rotator("Rotator", int) = 0
}

SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 100
	
	Pass {  
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _ULeft;
			float _URight;
			float _VBottom;
			float _VTop;
			float _UOffset;
			float _VOffset;
			int _Rotator;
			
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				//o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				
				half edge = 0.001f;
				if(i.texcoord.x < _UOffset || i.texcoord.x > ( 1 - _UOffset) )
					return 0;

				if(i.texcoord.y < _VOffset || i.texcoord.y > (1 - _VOffset))
					return 0;

				i.texcoord.x = (i.texcoord.x   - _UOffset ) / ((1 -  _UOffset  * 2 ) / _MainTex_ST.x) + _MainTex_ST.z;
				i.texcoord.y = (i.texcoord.y   - _VOffset ) / ((1 -  _VOffset  * 2 ) / _MainTex_ST.y) + _MainTex_ST.w;

				half2 texcoord = i.texcoord;
				if(_Rotator == 90)
				{
					texcoord.x = i.texcoord.y;
					texcoord.y = i.texcoord.x;
				}
				else if(_Rotator == 180)
				{
					texcoord.y = 1 -  i.texcoord.y;
				}
				else if(_Rotator == 270)
				{
					texcoord.x = 1 - i.texcoord.y;
					texcoord.y = 1 - i.texcoord.x;
				}
				fixed4 col = tex2D(_MainTex, texcoord);

				half xMask = 1;
				half yMask = 1;
				if(i.texcoord.x >_URight-edge)
				{
					xMask=(_URight-i.texcoord.x)/edge;
				}
				if(i.texcoord.x <_ULeft+edge)
				{
					xMask=(i.texcoord.x-_ULeft)/edge;
				}
				if(i.texcoord.y>_VTop-edge)
				{
					yMask=(_VTop-i.texcoord.y)/edge;
				}
				if(i.texcoord.y<_VBottom+edge)
				{
					yMask=(i.texcoord.y-_VBottom)/edge;
				}

				return col*xMask*yMask;
			}
		ENDCG
	}
}

}
