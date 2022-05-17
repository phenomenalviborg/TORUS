Shader "Torus/Tile"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Black ("Black", Color) = (0, 0, 0, 1)
        _White ("White", Color) = (1,1,1,1)
        _MatCap("MatCap", 2D) = "white" {}
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
            sampler _MatCap;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv   = v.uv;
                o.tint = -mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).y;
                o.wP   = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }
            
            fixed2 GetMatCapUV(float3 normal)
            {
                fixed2 matCapNormal = mul(UNITY_MATRIX_V, float4(normal, 0)).rg;
                            
                return fixed2((matCapNormal.r *  .485) + .5, 
                               matCapNormal.g *  .485 + .5);
            }
                        
                        
            float MatCapX(float3 normal)
            {
                return tex2D(_MatCap, GetMatCapUV(normal)).x;
            }  

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 col = tex2D(_MainTex, i.uv);
                fixed tint = col.z;
                float dist = length(float2(i.wP.x, i.wP.z) - float2(HeadPos.x, HeadPos.z));
                float d = pow(1 - pow(saturate(dist * .5), 6), 6);
                float u  = tint * (1.0 - saturate(i.tint * 50));
                
                fixed2 n2d = fixed2(-(col.x * 2 - 1), col.y * 2 - 1);
                fixed l = length(n2d);
                       n2d = n2d / l * min(l, 1);
                       
                fixed tint2 = pow(tint, 2);
                fixed3 n = fixed3(n2d.x, 1 - n2d.x - n2d.y, n2d.y);
                fixed matcap = MatCapX(n) * tint2;
                      
                fixed t = (d * .995 + .005) * (1.0 - pow(saturate(dist * .3), 3));
                
                float dt = saturate(dot(-normalize(i.wP - _WorldSpaceCameraPos), n)) * tint2;
                //return dt;
                return lerp(_Black, _White, u) * t * (dt * .5 + .5) + matcap * t * .35;
            }
            ENDCG
        }
    }
}
