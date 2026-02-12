Shader "UI/FrostedGlass"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,0.5)
        _BlurAmount ("Blur Amount", Range(0, 0.1)) = 0.005
    }
    
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
        }
        
        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        
        GrabPass { }
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };
            
            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 uvgrab : TEXCOORD1;
            };
            
            fixed4 _Color;
            sampler2D _GrabTexture;
            float4 _GrabTexture_TexelSize;
            float _BlurAmount;
            
            v2f vert(appdata_t v)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(v.vertex);
                OUT.texcoord = v.texcoord;
                OUT.color = v.color * _Color;
                
                #if UNITY_UV_STARTS_AT_TOP
                float scale = -1.0;
                #else
                float scale = 1.0;
                #endif
                
                OUT.uvgrab.xy = (float2(OUT.vertex.x, OUT.vertex.y*scale) + OUT.vertex.w) * 0.5;
                OUT.uvgrab.zw = OUT.vertex.zw;
                
                return OUT;
            }
            
            fixed4 frag(v2f IN) : SV_Target
            {
                // Simple 4-sample blur
                half4 sum = half4(0,0,0,0);
                
                sum += tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(IN.uvgrab.x - _BlurAmount, IN.uvgrab.y - _BlurAmount, IN.uvgrab.z, IN.uvgrab.w)));
                sum += tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(IN.uvgrab.x + _BlurAmount, IN.uvgrab.y - _BlurAmount, IN.uvgrab.z, IN.uvgrab.w)));
                sum += tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(IN.uvgrab.x - _BlurAmount, IN.uvgrab.y + _BlurAmount, IN.uvgrab.z, IN.uvgrab.w)));
                sum += tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(IN.uvgrab.x + _BlurAmount, IN.uvgrab.y + _BlurAmount, IN.uvgrab.z, IN.uvgrab.w)));
                
                sum = sum * 0.25;
                
                return sum * IN.color;
            }
            ENDCG
        }
    }
}