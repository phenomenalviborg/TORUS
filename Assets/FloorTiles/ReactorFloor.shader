Shader "Torus/ReactorFloor"
{
    Properties
    {
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
                float4 color  : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2  tint   : TEXCOORD0;
                float3 wP     : TEXCOORD1;
                float4 color : TEXCOORD2;
            };

            float3 HeadPos;
            float4 _White, _Black;
            sampler _MatCap;
            float _DistMulti;
            float4 _Tint;
            float _TintAmount;

            v2f vert (appdata v)
            {
                v2f o;
                
                o.wP     = mul(unity_ObjectToWorld, v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.tint   = float2(-mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).y, v.color.x);
                o.color  = v.color;//saturate(pow(v.color, 1.0 / 1.5));
                
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
                fixed4 col = i.color;//tex2D(_MainTex, i.uv);
                fixed tint = col.z * .85 + .15;
                float dist = length(float2(i.wP.x, i.wP.z) - float2(HeadPos.x, HeadPos.z)) * _DistMulti;
                float d = pow(1 - pow(saturate(dist * .1155), 6), 6);
                float k = 1.0 - saturate(i.tint.x * 30);
                float u  = tint * k;
                fixed t = ((d * .895 + .105) * ((1.0 - pow(saturate(dist * .021), 3))));
                
                return col * (tint * .75 + .25) * _Tint * t;
                
                fixed2 n2d = fixed2(-(col.x * 2 - 1), (col.y * 2 - 1));
                       n2d *= tint * .75 + .25;
                 
                //return fixed4(n2d, 1, 1);      
                fixed tint2 = pow(tint, 2);
                fixed3 n = mul(unity_ObjectToWorld, normalize(fixed3(n2d.x, max(0, 1.0 - n2d.x - n2d.y), n2d.y)));
                       n = normalize(fixed3(n2d.x, max(0, 1.0 - n2d.x - n2d.y), n2d.y));
                //return fixed4(n, 1);  
                fixed matcap = MatCapX(n) * tint2;
                     
                //return fixed4(n, 1);     
                
                
                //float whateverthisis = 
                float dt = (1.0 - pow(1.0 - saturate(dot(-normalize(i.wP - _WorldSpaceCameraPos), n)), 2)) * tint2 * .85 + .15;
              
                     dt = lerp(1, dt, _TintAmount);
                     t  = lerp(1, t, _TintAmount);
                     
                return ((lerp(_Black, _White, u) * t * dt  + matcap * t * .2 * k) * (i.tint.y * .1 + .9)) * _Tint;
            }
            ENDCG
        }
    }
}
