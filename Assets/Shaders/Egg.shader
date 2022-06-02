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
                float4 color : COLOR;
            };

            struct v2f
            {
                float3 normal : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
                float t : TEXCOORD2;
                float dist : TEXCOORD3;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = v.normal;
                o.viewDir = ObjSpaceViewDir(v.vertex);
                o.dist = length(mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz - _WorldSpaceCameraPos);
                o.t = v.color.x;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 n = mul(unity_ObjectToWorld, i.normal).xyz;
                float d = dot(normalize(i.normal), normalize(i.viewDir));
                float t = (1.0 - pow(1.0 - i.t, 8)) * i.t;
                float dist = pow(saturate(1 - i.dist * .01), 6);
                return saturate(pow(d, 6)) * .6 * t * dist;
            }
            ENDCG
        }
    }
}
