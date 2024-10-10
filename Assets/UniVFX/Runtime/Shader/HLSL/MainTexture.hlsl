#ifdef _SURFACEFADE
    #define MainTextureSample(uv, color) \
        TextureSampler(color, _Main, uv);\
        color = _MainTexSurfaceFade ? color * GetSurfaceFadeValue : color;\
        color = _MainTexMask ? color * GetMaskTextureValue : color;\
        half4 mainColor = GetVertexColorDataArray(_MainColor);\
        color.rgb = _MainColorMultiple ? color.rgb * mainColor.rgb : color.rgb;\
        color.a = _MainAlphaMultiple ? color.a * mainColor.a : color.a;
#else
    #define MainTextureSample(uv, color) \
        TextureSampler(color, _Main, uv);\
        color = _MainTexMask ? color * GetMaskTextureValue : color;\
        half4 mainColor = GetVertexColorDataArray(_MainColor);\
        color.rgb = _MainColorMultiple ? color.rgb * mainColor.rgb : color.rgb;\
        color.a = _MainAlphaMultiple ? color.a * mainColor.a : color.a;
#endif