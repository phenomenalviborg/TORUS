Shader "Torus/Tile"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Black ("Black", Color) = (0, 0, 0, 1)
        _White ("White", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
                float4 color  : COLOR;
            };

            struct v2f
            {
                float2 uv     : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float  tint   : TEXCOORD1;
                float3 wP     : TEXCOORD2;
            };

            sampler2D _MainTex;
            float3 HeadPos;
            float4 _White, _Black;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv   = v.uv;
                o.tint = -mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).y;
                o.wP   = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed col = tex2D(_MainTex, i.uv).x;
                float dist = length(float2(i.wP.x, i.wP.z) - float2(HeadPos.x, HeadPos.z));
                float d = pow(1 - pow(saturate(dist * .5), 6), 6);
                float u  = col  * (1.0 - saturate(i.tint * 50));
                      
                return lerp(_Black, _White, u) * (d * .995 + .005) * (1.0 - pow(saturate(dist * .3), 3));// * .3;//(.995 * d + .005);
            }
            ENDCG
        }
    }
}
