//���Q�l��
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

                // �������l�}�b�v
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
                    // �@
                    // �X�N���[�����W
                    float2 screenPos = i.positionSS.xy / i.positionSS.w;
                    // ��ʃT�C�Y����Z���āA�s�N�Z���P�ʂ�
                    float2 screenPosInPixel = screenPos.xy * _ScreenParams.xy;

                    // �A
                    // �f�B�U�����O�e�N�X�`���p��UV���쐬
                    int ditherUV_x = (int)fmod(screenPosInPixel.x, PATTERN_ROW_SIZE);
                    int ditherUV_y = (int)fmod(screenPosInPixel.y, PATTERN_ROW_SIZE);
                    float dither = pattern[ditherUV_x, ditherUV_y];

                    // �B
                    // 臒l��0�ȉ��Ȃ�`�悵�Ȃ�
                   clip(dither - _DitherLevel);

                   //���C���e�N�X�`������T���v�����O`
                   float4 color = tex2D(_MainTex, i.uv);
                   return color;
               }
               ENDCG
           }
        }
}