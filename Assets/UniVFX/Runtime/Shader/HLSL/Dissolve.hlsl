

#ifdef _SURFACEFADE
    #define Dissolve(uv, baseColor) \
        half4 dissolveParam = GetVertexDataArray4(_DissolveParam);\
        half4 dissolveColor = GetVertexColorDataArray(_DissolveColor);\
        half4 dissolveTex = half4(1, 1, 1, 1);\
        TextureSampler(dissolveTex, _Dissolve, uv);\
        dissolveParam.x = _DissolveSurfaceFade ? dissolveParam.x * GetSurfaceFadeValue : dissolveParam.x;\
        dissolveTex.x = _DissolveMask ? 1 - (1 - dissolveTex.x) * GetMaskTextureValue : dissolveTex.x;\
        half dissolveEmissive = dissolveParam.z + dissolveParam.w;\
        half dissolveSmooth = max(0.0001, dissolveParam.y);\
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
#else
    #define Dissolve(uv, baseColor) \
        half4 dissolveParam = GetVertexDataArray4(_DissolveParam);\
        half4 dissolveColor = GetVertexColorDataArray(_DissolveColor);\
        half4 dissolveTex = half4(1, 1, 1, 1);\
        TextureSampler(dissolveTex, _Dissolve, uv);\
        dissolveTex.x = _DissolveMask ? 1 - (1 - dissolveTex.x) * GetMaskTextureValue : dissolveTex.x;\
        half dissolveEmissive = dissolveParam.z + dissolveParam.w;\
        half dissolveSmooth = max(0.0001, dissolveParam.y);\
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
#endif