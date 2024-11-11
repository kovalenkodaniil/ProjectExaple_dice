Shader "Unlit/Glitch"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Value ("Value", Range(0, 1)) = 0
        _StripsDistance ("Strip Distance", Range(0,0.05)) = 0.05
        _StripsQuantity ("Strip Quantity", Integer) = 10
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

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
            float _Value;
            float _StripsDistance;
            float _StripsQuantity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 glitch(float2 uv)
            {
                const float offset = 0.06 * _Value;

                fixed4 colBase = tex2D(_MainTex, uv);

                float colR = tex2D(_MainTex, float2(uv.x - offset, uv.y - offset)).r;
                float colG = colBase.g;
                float colB = tex2D(_MainTex, float2(uv.x + offset, uv.y + offset)).b;

                return fixed4(colR, colG, colB, colBase.a);
            }

            float2 strips(float2 uv)
            {
                float height = 1.0 / _StripsQuantity;
                int index = (int)(uv.y / height);
                float offsetX = (index % 2 == 0 ? 1 : -1) * (_Value * _StripsDistance);
                return float2(uv.x + offsetX, uv.y);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uvStrips = strips(i.uv);
                fixed4 col = glitch(uvStrips);
                //fixed4 col = tex2D(_MainTex, uvStrips);
                return col;
            }
            ENDCG
        }
    }
}
