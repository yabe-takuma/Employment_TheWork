Shader "Unlit/CullingShader"
{
   
    Properties
    {
       
        _Dissolve("Dissolve",Range(-0.01,1.01))=0.0
        _Color ("Color", Color) = (1,1,1,1)
        [NoScaleOffset] _MainTex ("Albedo (RGB)", 2D) = "white" {}
        [Toggle] _UseMetallicMap ("Use Metallic Map", Float) = 0.0
        [NoScaleOffset] _MetallicGlossMap("Metallic", 2D) = "black" {}
        [Gamma] _Metallic ("Metallic", Range(0,1)) = 0.0
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _BumpScale("Scale", Float) = 1.0
        [NoScaleOffset] _BumpMap("Normal Map", 2D) = "bump" {}
        _Cutoff("Alpha Cutoff", Range(0.01,1)) = 0.5
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
        }

        Blend SrcAlpha OneMinusSrcAlpha

        CGINCLUDE
            #pragma vertex vert
            #pragma fragment frag
            

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MaskTex;
            float4 _MaskTex_ST;
            float _Dissolve;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            ENDCG

        Pass
        {
            Cull front
            CGPROGRAM
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 mask = tex2D(_MaskTex,i.uv);
                clip(mask.r-_Dissolve);
                return fixed4(0,1,1,1);
            }
            ENDCG
        }//一つ目のPass終了

        Pass //二つ目のPass
        {
            CGPROGRAM
            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 mask = tex2D(_MaskTex,i.uv);
                clip(mask.r-_Dissolve);
                return mask;
            }
            ENDCG  
        }
    }
}
