Shader "Unlit/Dot"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float dist : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            
            float _LineS;

            v2f vert (appdata v)
            {
                v2f o;
                
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                
                float3 p = mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz;
                
                float dist = length(mul(unity_ObjectToWorld, float4(p, 1)).xyz - _WorldSpaceCameraPos);
                float size = length(mul(unity_ObjectToWorld, float4(1, 0, 0, 0)));
                
                o.vertex = UnityObjectToClipPos(v.vertex * (dist * .0075 * _LineS) / size);
                o.dist = dist;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float tint = .3 + .7 * (1.0 - pow(1.0 - pow(saturate(1.0 - i.dist * .03), 7), 4));
                return fixed4(.85 * tint, .125 * tint, .15 * tint, 1);
            }
            ENDCG
        }
    }
}
