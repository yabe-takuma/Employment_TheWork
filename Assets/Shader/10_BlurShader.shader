Shader "Unlit/10_BlurShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _AngleDeg;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed Gaussian(float2 drawUV,float2 pickUV,float sigma)
            {
                float d=distance(drawUV,pickUV);
                return exp(-(d*d)/(2*sigma*sigma));
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed totalWeight = 0;
                float4 color = fixed4(0,0,0,0);
                float2 pickUV = float2(0,0);  //色を取得する座標
                float pickRange = 0.06;  //ガウス関数式でいうα
                float angleRad = _AngleDeg*3.14159/100;  //ぼかす角度(ラジアン)

                [loop]
                for(float j = -pickRange;j<=pickRange;j+=0.005)  //直線なのでfor文は一つ
                {
                    float x = cos(angleRad) * j;  //角度から座標を指定
                    float y = sin(angleRad) * j;
                    pickUV = i.uv + float2(x,y);

                    fixed weight = Gaussian(i.uv,pickUV,pickRange);  //自作のガウス関数で計算
                    color += tex2D(_MainTex,pickUV) * weight;  //取得する色にweightを掛ける
                    totalWeight += weight;  //掛けるweightの合計値を控えておく
                }
                color = color / totalWeight;  //足し合わせた色をweightの合計値で割る
                color.a=1;
                return color;
            }
            ENDCG
        }
    }
}
