Shader "Unlit/TestOutline"
{
    Properties
    {
    	_MainTex ("MainTex", 2D) = "white" {}
		[HDR] _OutlineColor ("Outline Color", Color) = (1,1,1,1)
		_OutlineAlpha ("Outline Alpha", Range(0,1)) = 1
		_Thickness ("Thickness", Range(0,1)) = 1
		_GlowIntensity ("GlowIntensity", Range(0, 10)) = 1

        // default properties
        _Color ("Tint", Color) = (1,1,1,1)
        _StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255
		_ColorMask ("Color Mask", Float) = 15
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

        Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}

        Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                half2 texcoord  : TEXCOORD0;
                fixed4 gradientColor : COLOR1;
            };

            sampler2D _MainTex;

            fixed4 _Color;
            fixed4 _OutlineColor;
            float _OutlineAlpha;
            float _Thickness;
            float _GlowIntensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.texcoord = v.texcoord;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 baseCol = tex2D(_MainTex, i.uv);
            	fixed4 outlineColor = i.color;
            	
            	if(all(outlineColor.rgb < 0.1))
            	{
            		outlineColor = _OutlineColor * _GlowIntensity * lerp(0, 1, _Thickness);
            		outlineColor *= baseCol;
            		outlineColor.a = baseCol.a * _OutlineAlpha;
            	}
            	else
            	{
            		outlineColor = baseCol;	
            	}
            	
            	return outlineColor;
            }
            ENDCG
        }
    }
}
