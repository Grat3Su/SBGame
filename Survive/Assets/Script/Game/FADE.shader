Shader "Unlit/FADE"
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
            float4 _MainTex_ST;

            float4 inColor;
            float rate;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col;// = inColor;// tex2D(_MainTex, i.uv)* inColor;

                vec2 uv = i.uv;
                vec2 c = _MainTex_Texelsize.zw / 2.;

                float len = rate * _MainTex_Texelsize.w;
                float d = length(fragCoord.xy - c);
                if (d > len)
                    col = inColor * float4(0, 0, 0, 0);
                else
                    col = inColor * float(0, 0, 0, 1);
                
                //if (nGrey % 5 == 0)
                //{
                //    float grey = col.r * 0.25 + col.g * 0.6 + col.b * 0.15;
                //    col.rgb = grey;//회색으로 그림
                //}
                
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
