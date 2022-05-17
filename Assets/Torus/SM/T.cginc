// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

#include "UnityCG.cginc"


float _Radius, _Thickness, _Speed, _VisA, _VisB;
int _Reps;
            
sampler2D _MainTex;
sampler2D _Clouds;          
                   
                    
struct appdata
{
    float4 vertex : POSITION;
    float2 uv     : TEXCOORD0;
    float2 uv2    : TEXCOORD1;
    float4 color  : COLOR;
    UNITY_VERTEX_INPUT_INSTANCE_ID;
};

struct v2f
{
    float2 uv     : TEXCOORD0;
    float4 vertex : SV_POSITION;
    float3 pos    : TEXCOORD1;
    float4 color  : TEXCOORD2;
};

struct helper
{
    float s, u;
    float3 tex;
};

            
            
v2f vert (appdata v)
{
    v2f o;
    
    float4 p = float4( v.uv2.x, 0, v.uv2.y, 1);
           p = p * _Radius + (v.vertex - p) *  _Thickness;
           
    o.vertex = UnityObjectToClipPos(p);
    o.uv     = v.uv;
    o.pos    = mul(unity_ObjectToWorld, p);
    o.color  = v.color;
    return o;
}


helper GetHelper(v2f i, float speed)
{
    helper h;
    h.s   = speed * _Time.y * .01 * (1 + i.color.y);
    h.u   = i.uv.x + h.s;
    h.tex = tex2D(_MainTex, float2(i.uv.x + h.s * -13.4, 0)).xyz;
    return h;
}

helper GetHelper2(v2f i, float speed)
{
    helper h;
    h.s   = speed * _Time.y * .01 * (1 + i.color.y);
    h.u   = (1 - i.uv.x) + h.s + (sin(i.uv.y * 16 * 3.14159265358979323846264338327) * .005 + 1) * 0;
    h.tex = float3(0, 0, 0);
    return h;
}


fixed4 T (v2f i, float dist, float speed, int reps) : SV_Target
{
    helper h = GetHelper(i, speed);
    
    float v = 1 - abs(.5 - (saturate(h.u * reps % 1 / dist))) * 2;
          v = saturate(pow(1 - v, 10));
          
    float c = v * lerp( _VisB, _VisA, (1 - pow(1 - pow(i.color.x, 2.5), 2)));
    
    return float4(h.tex * saturate(c), 1);
}


fixed4 T2 (v2f i, float dist, float speed, int reps) : SV_Target
{
    helper h = GetHelper(i, speed);
    
    float v = saturate((1 - abs(.5 - (saturate(h.u * reps % 1))) * 2) / dist * 2);
          v = saturate(pow(1 - v, 100));
          
    float c = v * lerp( _VisB, _VisA, (1 - pow(1 - pow(i.color.x, 1.1), 2)));
    
    return float4(h.tex * saturate(c), 1);
}


fixed4 T3 (v2f i, float dist, float speed, int reps) : SV_Target
{
    helper h = GetHelper(i, speed);
    
    float m = (1 + i.color.y * .5);
    float v  = tex2D(_Clouds, float2(h.u * 4 * m, i.uv.y * 6) * .5).x;
          v += tex2D(_Clouds, float2((i.uv.x + h.s * 3.5) * 4 * m, i.uv.y * 6)  + 2.21231).x;
          v += tex2D(_Clouds, float2((i.uv.x + h.s * -2.5) * 2 * m, i.uv.y * 4) + 1.21231).x;
          v /= 3.0;
          v = saturate(pow(1 - v, 4));
          
    float c = v * lerp( _VisB, _VisA, (1 - pow(1 - pow(i.color.x, 1.2), 1)));
    
    return float4(h.tex * saturate(c * 1.5), 1);
}


fixed4 T4 (v2f i, float dist, float speed, int reps) : SV_Target
{
    helper h = GetHelper2(i, speed);
    
    float v = saturate((abs(.5 - (saturate(h.u * reps % 1))) * 2) / dist * 2);
          v = saturate(pow(1 - v, 20));
          
    float c = v * (1 - pow(1 - pow(i.color.x, 1.1), 2));
    
    return c;//float4(h.tex * saturate(c), 1);
}