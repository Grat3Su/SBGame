Shader "Unlit/OUTLINE"
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
			float4 _MainTex_TexelSize;// x, y, z, w = (1/width, 1/height, width, height)
			float4 _MainTex_ST;

            float4 inColor;
			float outlineWidth;// pixel
			float4 shaderColor;

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
				float2 fragCoord = i.uv * _MainTex_TexelSize.zw;

				fixed4 col = tex2D(_MainTex, i.uv) * inColor;

				float2 size = outlineWidth * _MainTex_TexelSize.xy;
				float outline = 
					tex2D(_MainTex, i.uv + float2(-size.x, -size.y)).a +
					tex2D(_MainTex, i.uv + float2(      0, -size.y)).a +
					tex2D(_MainTex, i.uv + float2(+size.x, -size.y)).a +
					tex2D(_MainTex, i.uv + float2(-size.x, 0)).a +
					tex2D(_MainTex, i.uv + float2(+size.x, 0)).a +
					tex2D(_MainTex, i.uv + float2(-size.x, +size.y)).a +
					tex2D(_MainTex, i.uv + float2(      0, +size.y)).a +
					tex2D(_MainTex, i.uv + float2(+size.x, +size.y)).a;
				outline = min(outline, 1);

				col = mix(col, shaderColor, outline- col.a);// mix(x, y, a) = x * (1-a) + y * a
                
                return col;
            }
            ENDCG
        }
    }
}
