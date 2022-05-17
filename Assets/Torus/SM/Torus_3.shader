Shader "Torus/3"
{
    Properties
    {
        _Radius	   ("_Radius", float) = 0
        _Thickness ("_Thickness", float) = 0
        _Speed	   ("_Speed", float) = 0
        _Reps	   ("_Reps", int) = 1
        _MainTex   ("_MainTex", 2D) = "white" {}
        _Clouds    ("_Clouds", 2D) = "white" {}
        _VisA	   ("_VisA", float) = 1
        _VisB	   ("_VisB", float) = 0
    }
    
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha One
        Cull Off 
        Lighting Off 
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "T.cginc"
            
            fixed4 frag (v2f i) : SV_Target
            {
                return T3(i, length(i.pos - _WorldSpaceCameraPos), _Speed, _Reps);
            }
            ENDCG
        }
    }
}
