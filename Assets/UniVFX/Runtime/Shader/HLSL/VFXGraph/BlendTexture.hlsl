    #define BlendTexture(uv, baseCol) \
        half4 blendTex = half4(1, 1, 1, 1);\
        TextureSampler(blendTex, _Blend, uv);\
        blendTex *= _BlendTexColor;\
        blendTex.a *= _BlendIntensity;\
        if(_SURFACEFADE)\
        {\
            blendTex.a = _BlendTexSurfaceFade ? blendTex.a * GetSurfaceFadeValue : blendTex.a;\
        }\
        blendTex.a = _BlendTexMask ? blendTex.a * GetMaskTextureValue : blendTex.a;\
        ColorMix(baseCol.rgb, baseCol.rgb, blendTex.rgb, blendTex.a, _BlendTexBlendMode);
