#ifdef _SURFACEFADE
    #define GradationColor(uv, baseCol)\
        float2 gradationMinus = uv < 0 ? -1 : 0;\
        float2 gradationRepeat = abs((uv + gradationMinus)) % 2 >= 1 ? 1 - frac(uv) : frac(uv);\
        half4 gradationColor00 = GetVertexColorDataArray(_GradationColor00);\
        half4 gradationColor01 = GetVertexColorDataArray(_GradationColor01);\
        half4 gradationColor10 = GetVertexColorDataArray(_GradationColor10);\
        half4 gradationColor11 = GetVertexColorDataArray(_GradationColor11);\
        half4 gradationColor0 = lerp(gradationColor00, gradationColor10, gradationRepeat.x);\
        half4 gradationColor1 = lerp(gradationColor01, gradationColor11, gradationRepeat.x);\
        half4 gradationColor = lerp(gradationColor0, gradationColor1, gradationRepeat.y);\
        gradationColor.a = _GradationSurfaceFade ? gradationColor.a * GetSurfaceFadeValue : gradationColor.a;\
        gradationColor.a = _GradationMask ? gradationColor.a * GetMaskTextureValue : gradationColor.a;\
        ColorMix(baseCol.rgb, baseCol.rgb, gradationColor.rgb, gradationColor.a, _GradationBlendMode);
#else
    #define GradationColor(uv, baseCol)\
        float2 gradationMinus = uv < 0 ? -1 : 0;\
        float2 gradationRepeat = abs((uv + gradationMinus)) % 2 >= 1 ? 1 - frac(uv) : frac(uv);\
        half4 gradationColor00 = GetVertexColorDataArray(_GradationColor00);\
        half4 gradationColor01 = GetVertexColorDataArray(_GradationColor01);\
        half4 gradationColor10 = GetVertexColorDataArray(_GradationColor10);\
        half4 gradationColor11 = GetVertexColorDataArray(_GradationColor11);\
        half4 gradationColor0 = lerp(gradationColor00, gradationColor10, uv.x);\
        half4 gradationColor1 = lerp(gradationColor01, gradationColor11, uv.x);\
        half4 gradationColor = lerp(gradationColor0, gradationColor1, uv.y);\
        gradationColor.a = _GradationMask ? gradationColor.a * GetMaskTextureValue : gradationColor.a;\
        ColorMix(baseCol.rgb, baseCol.rgb, gradationColor.rgb, gradationColor.a, _GradationBlendMode);
#endif
