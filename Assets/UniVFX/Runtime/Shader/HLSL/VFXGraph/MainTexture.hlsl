
    #define MainTextureSample(uv, color) \
        TextureSampler(color, _Main, uv);\
        if(_SURFACEFADE)\
        {\
            color = _MainTexSurfaceFade ? color * GetSurfaceFadeValue : color;\
        }\
        color = _MainTexMask ? color * GetMaskTextureValue : color;\
        half4 mainColor = _MainColor;\
        color.rgb = _MainColorMultiple ? color.rgb * mainColor.rgb : color.rgb;\
        color.a = _MainAlphaMultiple ? color.a * mainColor.a : color.a;
