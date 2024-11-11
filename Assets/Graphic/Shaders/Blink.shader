Shader "Unlit/Blink"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR] _BlinkColor ("Blink Color", Color) = (1,1,1,1)
        _BlinkIntensity ("BlinkIntensity", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags 
        {
            "Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
        }

        Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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
            fixed4 _Color;
            fixed4 _BlinkColor;
            float _BlinkIntensity;

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
                float4 baseCol = tex2D(_MainTex, i.uv);
                fixed4 blinkColor = i.color;

                if (baseCol.a != 0)
                {
                    blinkColor = lerp(baseCol, _BlinkColor, _BlinkIntensity);
                    blinkColor.a = baseCol.a;
                }
                else
                {
                    blinkColor = baseCol;
                }
                
                return blinkColor;
            }
            ENDCG
        }
    }
}
