// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/CheapMatCapLike"
{
	Properties
	{
		_Color("Tint Color",Color) = (1,1,1,1)
		_MatCap("MatCap (RGB)", 2D) = "white" {}
		[MaterialToggle] TextureTransforms("Transform Texture coords", Float) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			name "CHEAP_BASE"
			CGPROGRAM

			#pragma multi_compile _ TEXTURETRANSFORMS_ON

			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest

			#include "UnityCG.cginc"

			struct v2f
			{
				float2 n_uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 colorx2 : TEXCOORD1;
			};

			fixed4 _Color;
			sampler2D _MatCap;

			#if defined(TEXTURETRANSFORMS_ON)

				float4 _MatCap_ST;

			#endif
			
			v2f vert (appdata_base v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);

				float3 nuv = mul( (float3x3)UNITY_MATRIX_IT_MV, v.normal.xyz);
				o.n_uv = nuv.xy * 0.5 + 0.5;

				#if defined(TEXTURETRANSFORMS_ON)
					o.n_uv = TRANSFORM_TEX( o.n_uv, _MatCap);
				#endif

				o.colorx2 = _Color + _Color;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				return tex2D(_MatCap, i.n_uv) * i.colorx2;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
