Shader "Torus/Math"
{
    Properties
    {
        _MainTex  ("Texture",      2D) = "white" {}
        _Anim	  ("Anim",      float) = 0
        _Radius   ("Radius",    float) = 1
        _Thickness("Thickness", float) = 1
        _Spin("Spin", float) = 1
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

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _Anim;
            float _Thickness;
            float _Radius;
            float _Spin;
            
            ////////////////// BEGIN QUATERNION FUNCTIONS //////////////////

            float PI = 3.1415926535897932384626433832795;
            
            float4 setAxisAngle (float3 axis, float rad) {
              rad = rad * 0.5;
              float s = sin(rad);
              return float4(s * axis[0], s * axis[1], s * axis[2], cos(rad));
            }
            
            float3 xUnitVec3 = float3(1.0, 0.0, 0.0);
            float3 yUnitVec3 = float3(0.0, 1.0, 0.0);
            
            float4 rotationTo (float3 a, float3 b) {
              float vecDot = dot(a, b);
              float3 tmpvec3 = float3(0, 0, 0);
              if (vecDot < -0.999999) {
                tmpvec3 = cross(xUnitVec3, a);
                if (length(tmpvec3) < 0.000001) {
                  tmpvec3 = cross(yUnitVec3, a);
                }
                tmpvec3 = normalize(tmpvec3);
                return setAxisAngle(tmpvec3, PI);
              } else if (vecDot > 0.999999) {
                return float4(0,0,0,1);
              } else {
                tmpvec3 = cross(a, b);
                float4 _out = float4(tmpvec3[0], tmpvec3[1], tmpvec3[2], 1.0 + vecDot);
                return normalize(_out);
              }
            }
            
            float4 multQuat(float4 q1, float4 q2) {
              return float4(
                q1.w * q2.x + q1.x * q2.w + q1.z * q2.y - q1.y * q2.z,
                q1.w * q2.y + q1.y * q2.w + q1.x * q2.z - q1.z * q2.x,
                q1.w * q2.z + q1.z * q2.w + q1.y * q2.x - q1.x * q2.y,
                q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z
              );
            }
            
            float3 rotateVector( float4 quat, float3 vec ) {
              // https://twistedpairdevelopment.wordpress.com/2013/02/11/rotating-a-vector-by-a-quaternion-in-glsl/
              float4 qv = multQuat( quat, float4(vec, 0.0) );
              return multQuat( qv, float4(-quat.x, -quat.y, -quat.z, quat.w) ).xyz;
            }
            
            ////////////////// END QUATERNION FUNCTIONS //////////////////
            

            v2f vert (appdata v)
            {
                v2f o;
                
                float3 p = v.vertex * .1 + v.normal * -.01;
                
                float3 pr   = float3(p.x, 0, p.z);
                float3 rDir = normalize(pr);
                float3 sDir = float3(rDir.z, 0, -rDir.x);
                float4 rot = setAxisAngle (sDir, _Spin);
                
                
                float3 r = rDir * 2;
                       p = r + rotateVector(rot, p - r) * _Thickness;
                       p += rDir * (-1 - (1 - _Thickness) + _Radius);
                
                
                float dist = length(mul(unity_ObjectToWorld, float4(p, 1)).xyz - _WorldSpaceCameraPos);
                float size = length(mul(unity_ObjectToWorld, float4(1, 0, 0, 0)));
                float u = (_Anim - v.uv.x) * 50;
                float trail = 1.0 - pow(1.0 - saturate(u), 4);
                float s = 1; //(sin(u * 6) * .5 + .5) * .6 + .4;
                
                o.vertex = UnityObjectToClipPos(p + rotateVector(rot, v.normal * dist * .0011 * trail * s) / size);
                o.uv     = float2(v.uv.x, dist);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                clip(step(_Anim, i.uv.x) * -1 + .5);
                
                float tint = 1.0 - pow(1.0 - pow(saturate(1.0 - i.uv.y * .06), 7), 4);
                float trail = saturate((_Anim - i.uv.x) * 20);
                return tex2D(_MainTex, float2(trail, 0)) * tint * 1.5;
            }
            ENDCG
        }
    }
}
