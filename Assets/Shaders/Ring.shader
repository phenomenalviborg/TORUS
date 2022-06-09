Shader "Torus/Ring"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        CULL Off

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
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float3 uv : TEXCOORD0;
                //float3 normal : TEXCOORD1;
                //float3 wP : TEXCOORD2;
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            
            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float, _Anim)
                UNITY_DEFINE_INSTANCED_PROP(float, _Vis)
            UNITY_INSTANCING_BUFFER_END(Props)

            sampler2D _MainTex;
            float4 RingColor;
            
            float _LineS;
            float _LineG;
            
            v2f vert (appdata v)
            {
                v2f o;
                
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                
                float3 p = (v.vertex + v.normal * -1) * .1;
                
                float vis  = UNITY_ACCESS_INSTANCED_PROP(Props, _Vis);
                float dist = length(mul(unity_ObjectToWorld, float4(p, 1)).xyz - _WorldSpaceCameraPos);
                float size = length(mul(unity_ObjectToWorld, float4(1, 0, 0, 0)));
                
                float anim   = UNITY_ACCESS_INSTANCED_PROP(Props, _Anim);
                float growth = saturate(1 - dist * _LineG);
                
                p = p + v.normal * (dist * .0011 * _LineS * vis * growth) / size;
             
                o.vertex  = UnityObjectToClipPos(p);
                o.uv      = float3(v.uv.x, dist, vis);
                //o.normal  =  mul(unity_ObjectToWorld, v.normal);
                //o.wP = mul(unity_ObjectToWorld, float4(p, 1)).xyz;
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                float anim = UNITY_ACCESS_INSTANCED_PROP(Props, _Anim);
                clip(step(anim, i.uv.x) * -1 + .5);
                
                //return 1.0 - pow(1.0 - pow(1 - (dot(normalize(i.normal), normalize(i.wP - _WorldSpaceCameraPos)) * .5 + .5), 10), 4);
                
                float tint  =1.0 - pow(1.0 - pow(saturate(1.0 - i.uv.y * .03), 7), 4);
                float trail = saturate((anim - i.uv.x) * 20 * i.uv.z);
                return RingColor * tint * 1.5 * i.uv.z * saturate(_LineS); //stex2D(_MainTex, float2(trail, 0));// * tint * 1.5;
            }
            ENDCG
        }
    }
}
