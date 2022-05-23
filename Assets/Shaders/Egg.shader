Shader "Unlit/Egg"
{
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        Blend SrcAlpha One              //1st change here
        LOD 100
        ZWRITE ON

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
            };

            struct v2f
            {
                float3 normal : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = v.normal;
                o.viewDir = ObjSpaceViewDir(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 n = mul(unity_ObjectToWorld, i.normal).xyz;
                float d = dot(normalize(i.normal), normalize(i.viewDir));
                return saturate(pow(d, 6)) * .3;
            }
            ENDCG
        }
    }
}
