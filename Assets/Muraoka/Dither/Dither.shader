Shader "DitheringFade"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DitherLevel("DitherLevel", Range(0, 16)) = 1 //ディザリングの具合を調整
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            int _DitherLevel;

            static const int pattern[16] = { //staticがないとアクセスできないので注意！
                 0,  8,  2, 10,
                12,  4, 14,  6,
                 3, 11,  1,  9,
                15,  7, 13,  5
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); //頂点をMVP行列変換
                o.uv = TRANSFORM_TEX(v.uv, _MainTex); //テクスチャスケールとタイリングを加味
                o.screenPos = ComputeScreenPos(o.vertex); //クリップ座標からスクリーン座標を計算
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 viewPortPos = i.screenPos.xy / i.screenPos.w; //wで除算し、スクリーンでの投影位置を取得
                float2 screenPosInPixel = viewPortPos.xy * _ScreenParams.xy; //0～1の線形からピクセルに変換

                // ディザリングテクスチャ用のUVを作成
                int ditherUV_x = (int)fmod(screenPosInPixel.x, 4.0f); //パターンの大きさで割ったときの余りを求める
                int ditherUV_y = (int)fmod(screenPosInPixel.y, 4.0f); //今回のパターンサイズは4x4なので4で除算
                float dither = pattern[ditherUV_x + ditherUV_y * 4]; //求めた余りからパターン値を取得

                clip(dither - _DitherLevel); //閾値が0以下なら描画しない

                float4 color = tex2D(_MainTex, i.uv); //メインテクスチャからサンプリング
                return color;
            }
            ENDCG
        }
    }
}