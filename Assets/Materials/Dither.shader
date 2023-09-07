//※参考元
//https://ny-program.hatenablog.com/entry/2023/04/09/195811

Shader "Dither"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _DitherLevel("DitherLevel", Range(0, 16)) = 1
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
                    float4 positionOS : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 positionCS : SV_POSITION;
                    float4 positionSS : TEXCOORD1;
                };

                // しきい値マップ
                static const float4x4 pattern =
                {
                    0,8,2,10,
                    12,4,14,6,
                    3,11,1,9,
                    15,7,13,565
                };
                static const int PATTERN_ROW_SIZE = 4;

                sampler2D _MainTex;
                sampler2D _DitherTex;
                float _Alpha;
                half _DitherLevel;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.positionCS = UnityObjectToClipPos(v.positionOS);
                    o.positionSS = ComputeScreenPos(o.positionCS);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // ①
                    // スクリーン座標
                    float2 screenPos = i.positionSS.xy / i.positionSS.w;
                    // 画面サイズを乗算して、ピクセル単位に
                    float2 screenPosInPixel = screenPos.xy * _ScreenParams.xy;

                    // ②
                    // ディザリングテクスチャ用のUVを作成
                    int ditherUV_x = (int)fmod(screenPosInPixel.x, PATTERN_ROW_SIZE);
                    int ditherUV_y = (int)fmod(screenPosInPixel.y, PATTERN_ROW_SIZE);
                    float dither = pattern[ditherUV_x, ditherUV_y];

                    // ③
                    // 閾値が0以下なら描画しない
                   clip(dither - _DitherLevel);

                   //メインテクスチャからサンプリング`
                   float4 color = tex2D(_MainTex, i.uv);
                   return color;
               }
               ENDCG
           }
        }
}