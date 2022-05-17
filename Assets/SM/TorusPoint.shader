Shader "Torus/Point"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Reps	   ("_Reps", int) = 1
        _Radius	   ("_Radius", float) = 0
        _Thickness ("_Thickness", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Blend SrcAlpha One
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float a : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            int _Reps;
            float _Radius, _Thickness;
            
            
            float4 Billboard(float4 vrt, float4 p)
            {
                // get the camera basis vectors
                       float3 forward = -normalize(UNITY_MATRIX_V._m20_m21_m22);
                       float3 up      =  normalize(UNITY_MATRIX_V._m10_m11_m12);
                       float3 right   =  normalize(UNITY_MATRIX_V._m00_m01_m02);
            
                       // rotate to face camera
                       float4x4 rotationMatrix = float4x4(right, 0,
                           up, 0,
                           forward, 0,
                           0, 0, 0, 1);
                       vrt = mul(vrt, rotationMatrix);
            
                       // undo object to world transform surface shader will apply
                       vrt.xyz = mul(unity_WorldToObject, float4(vrt.xyz + p, 1));
            
                return vrt;
            }


            v2f vert (appdata v)
            {
                v2f o;
                
                float t  = -_Time.y * .1 * (10 + v.color.x * 30) + v.color.y * 2000 + v.color.x * 500;
                float t2 = -_Time.y * 1.21 * .01 * 360 * 2;
                
                float row = round(v.color.x * 44);
               
                float angle   = (360 / 44.0 * row + t + t2);
                float radians = angle * .01745329252;
                
                float ca = cos(radians);
                float sa = sin(radians);
                
                float3 p = float3(ca, sa, 0) ;
                
                o.a = ((1 - pow(p.x * .5 + .5, 6)) * .75 + .25) * ((1 - pow(1 - (p.y * .5 + .5), 6)) * .6 + .4);
                
                p = float3(p.x * _Thickness * .25 + _Radius * .5, p.y * _Thickness * .25, 0) * 10;
                
                radians = (t * (-1 + step(.5, v.color.z) * 2)) * .01745329252;
                
                ca = cos(radians);
                sa = sin(radians);
                
                p = float3(ca * p.x, p.y, sa * p.x);
                
                float s = (.75 + v.color.z * .5) * (sin(_Time.y * 12 + v.color.y * 1000) * .3 + .8);
                o.vertex = UnityObjectToClipPos(Billboard(v.vertex * .0004 * s, mul(unity_ObjectToWorld, float4(p.xyz, 0))));
                o.uv = v.uv;
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float v = 1 - pow(1 - pow(1 - saturate(length(i.uv) * 2), 7), 5);
                return i.a * v;
            }
            ENDCG
        }
    }
}
