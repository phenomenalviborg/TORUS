Shader "Unlit/Egg"
{
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        Blend SrcAlpha One              //1st change here
        LOD 100
        ZWRITE OFF

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
                float3 color : TEXCOORD2;
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
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 n = mul(unity_ObjectToWorld, i.normal).xyz;
                float d = dot(normalize(i.normal), normalize(i.viewDir));
                float t = (1.0 - pow(1.0 - i.color.x, 8)) * i.color.x;
                float dist = pow(saturate(1 - i.dist * (.01 * (1 + (1.0 - i.color.y) * 5))), 6);
                float u = (sin(_Time.y * 2 - i.color.x * 16) + 1) * .065 + .925;
                return saturate(pow(d, 6)) * .7 * t * dist * u * i.color.z;
            }
            ENDCG
        }
    }
}
