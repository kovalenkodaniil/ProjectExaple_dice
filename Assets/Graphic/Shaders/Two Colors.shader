Shader "Unlit/TwoCollors"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR] _FirstColor ("First Color", Color) = (1,1,1,1)
        [HDR] _SecondColor ("Second Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { 
            "Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True" 
            }

        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            fixed4 _FirstColor;
            fixed4 _SecondColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 baseCol = tex2D(_MainTex, i.uv);
                fixed4 newColor = _FirstColor;

                if (baseCol.a > 0)
                {
                    if(i.uv.x > 0.5)
                        newColor = _FirstColor;
                    else
                        newColor = _SecondColor;

                    /*if(i.uv.x > 0.5)
                        newColor = lerp(baseCol, _FirstColor, 0.5);
                    else
                        newColor = lerp(baseCol, _SecondColor, 0.5);*/
                    
                    newColor.a = baseCol.a;
                }
                else
                {
                    newColor.a = 0;
                }
                
                return newColor;
            }
            ENDCG
        }
    }
}
