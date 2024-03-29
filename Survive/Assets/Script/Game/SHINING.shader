Shader "Unlit/SHINING"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
			float4 _MainTex_TexelSize;
            float4 _MainTex_ST;

            float4 inColor;

			float  shiningWidth;
			float4 shaderColor;
			float  shiningTime;// 0~3s

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
#define mix(x, y, a)  ((x) * (1-(a)) + (y) * (a))
			fixed4 frag(v2f i) : SV_Target
			{
                fixed4 col = tex2D(_MainTex, i.uv) * inColor;

				if (col.a > 0)
				{
					float2 fragCoord = i.uv * _MainTex_TexelSize.zw;
					//float x = _MainTex_TexelSize.z * shiningTime;// +sin(radians(i.uv.y * 180)) * 100;
					float x = _MainTex_TexelSize.z * shiningTime * 0.3 - i.uv.y * 500;
                    //float x = _MainTex_TexelSize.z * shiningTime * 0.3;
					
					float a = clamp(abs(fragCoord.x - x)/ shiningWidth, 0, 1);
					a = 1 - a;
                    a = a * a * a;

					col = mix(col, shaderColor, a);                        
				}
				
                return col;
            }
            ENDCG
        }
    }
}
