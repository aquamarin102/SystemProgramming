Shader "MyShaders/AtmosphereShader"
{
    Properties
	{
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
		_Height("Height", Range(0,1)) = 0.5
    }
			
	SubShader
	{
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			sampler2D _MainTex;
			half4 _MainTex_ST;
			float4 _Color;
			float _Height;

			struct vertIn
			{
				float3 pos : POSITION;
				half2 tex : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				half2 tex : TEXCOORD0;
			};

			v2f vert(vertIn v)
			{
				v2f o;
				v.pos += v.normal * _Height;
				o.pos = UnityObjectToClipPos(v.pos);
				o.tex = v.tex * _MainTex_ST.xy + _MainTex_ST.zw;
				o.tex += _Time.x / 2;
				return o;
			}

			float4 frag(v2f f) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, f.tex);
				col *= _Color;
				return col;
			}
			ENDCG
		}
	}
}