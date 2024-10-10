
    #define GetMaskTextureValue maskValue

    #define MaskTextureSample(uv) \
        half maskValue = 1;\
        if(_MASKTEXTURE)\
        {\
            half4 maskTex = half4(1, 1, 1, 1);\
            TextureSampler(maskTex, _Mask, uv);\
            maskValue = maskTex.x + (_MaskOffset);\
            half maskMinus = maskValue < 0 ? -1 : 0;\
            half maskRepeat = abs((maskValue + maskMinus)) % 2 >= 1 ? 1 - frac(maskValue) : frac(maskValue);\
            maskValue = _MaskRepeat ? maskRepeat : saturate(maskValue);\
        }
