

    #define Dissolve(uv, baseColor) \
        half4 dissolveParam = _DissolveParam;\
        half4 dissolveColor = _DissolveColor;\
        half4 dissolveTex = half4(1, 1, 1, 1);\
        TextureSampler(dissolveTex, _Dissolve, uv);\
        half dissolveEmissive = dissolveParam.z + dissolveParam.w;\
        half dissolveSmooth = max(0.0001, dissolveParam.y);\
        if(_SURFACEFADE)\
        {\
            dissolveParam.x = _DissolveSurfaceFade ? dissolveParam.x * GetSurfaceFadeValue : dissolveParam.x;\
        }\
        dissolveParam.x = _DissolveMask ? 1 - (1 - dissolveParam.x) * GetMaskTextureValue : dissolveParam.x;\
        half dissolveAlpha = 1 - dissolveParam.x;\
        half dissolveAlphaMin = dissolveAlpha - (dissolveSmooth + dissolveEmissive);\
        dissolveAlpha = dissolveAlphaMin + dissolveAlpha * (1 - dissolveAlphaMin);\
        dissolveSmooth += dissolveAlpha;\
        half dissolveResult = smoothstep(dissolveAlpha, dissolveSmooth, dissolveTex.x);\
        half dissolveEmissiveWidth = dissolveSmooth + dissolveParam.z;\
        half dissolveEmissiveSmooth = dissolveEmissiveWidth + max(0.0001, dissolveParam.w);\
        half3 dissolveEmissiveResult = smoothstep(dissolveEmissiveSmooth, dissolveEmissiveWidth, dissolveTex.x) * step(0.001, dissolveEmissive) * dissolveColor.xyz;\
        baseColor.a *= dissolveResult;\
        baseColor.rgb += dissolveEmissiveResult;
