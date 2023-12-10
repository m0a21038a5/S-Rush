Shader "Unlit/AfterImageShaderTest"
{
   SubShader  
    {
        Pass  
        {
            CGPROGRAM  
            #pragma vertex vert_img  
            #pragma fragment frag  

            #include "UnityCG.cginc"  

            sampler2D _Tex0;  
            sampler2D _Tex1;  
            sampler2D _Tex2;  

            fixed4 frag (v2f_img i) : SV_Target  
            {
                fixed4 col0 = tex2D(_Tex0, i.uv) / 3;  
                fixed4 col1 = tex2D(_Tex1, i.uv) / 3;  
                fixed4 col2 = tex2D(_Tex2, i.uv) / 3;  
                return col0 + col1 + col2;  
            }
            ENDCG  
        }
    }
}
