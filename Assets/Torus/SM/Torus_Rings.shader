Shader "Torus/Rings"
{
    Properties
    {
        _Radius	("_Radius", float) = 0
        _Thickness	("_Thickness", float) = 0
        _Speed	("_Speed", float) = 0
        _Reps	("_Reps", int) = 1
        _MainTex		("_MainTex", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "T.cginc"


            fixed4 frag (v2f i) : SV_Target
            {
                return T4(i, length(i.pos - _WorldSpaceCameraPos), _Speed, _Reps);
            }
            ENDCG
        }
    }
}
