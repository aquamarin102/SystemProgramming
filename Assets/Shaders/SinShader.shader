Shader "MyShaders/SinShader"
{
	Properties
	{
		_Tex1("Texture1", 2D) = "white" {}
		_Color("Main Color", COLOR) = (1,1,1,1)
		_Height("Height", Range(0,20)) = 0.5
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 100
		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _Tex1;
			float4 _Tex1_ST;
			float4 _Color;
			float _Height;

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata_full v)
			{
				v2f result;
				float pi = 3.14159265358979323846264338327;
	
				v.vertex.xyz += v.normal * (_Height * sin(v.texcoord.x * pi) - _Height);
			
				result.vertex = UnityObjectToClipPos(v.vertex);
				result.uv = TRANSFORM_TEX(v.texcoord, _Tex1);
				return result;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 color;
				color = tex2D(_Tex1, i.uv);
				color = color * _Color;
				return color;
			}
			ENDCG
		}
	}
}
