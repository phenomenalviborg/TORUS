Shader "Torus/Black"
{
    Properties
    {
        _Radius	("_Radius", float) = 0
        _Thickness	("_Thickness", float) = 0
    }
    
    SubShader
    {
        Tags { "RenderType"="Transparent-100" }
        LOD 100
        Cull Front  
        Lighting Off 
        ZWrite On

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "T.cginc"
            
            fixed4 frag (v2f i) : SV_Target
            {
                return fixed4(0, 0, 0, 1);
            }
            ENDCG
        }
    }
}
