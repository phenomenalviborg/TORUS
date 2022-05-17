Shader "Torus/Mix"
{
    Properties
    {
        _Radius	("_Radius", float) = 0
        _Thickness	("_Thickness", float) = 0
        _MainTex		("_MainTex", 2D) = "white" {}
        _Clouds   ("_Clouds", 2D) = "white" {}
        _VisA	   ("_VisA", float) = 1
        _VisB	   ("_VisB", float) = 0
    }
    
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        //Tags { "RenderType"="Opaque" }
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
                float d = length(i.pos - _WorldSpaceCameraPos);
                float m = 1;
                return (T(i, d, 1.85, 16.3) + T2(i, d, 1.21 * m, 44) + T3(i, d, 0.82, 15.54)) / 1.75;
            }
            ENDCG
        }
    }
}
