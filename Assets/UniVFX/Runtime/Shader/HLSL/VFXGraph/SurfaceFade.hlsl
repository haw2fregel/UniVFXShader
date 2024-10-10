
#define GetSurfaceFadeValue surfaceFade

#define GetSurfaceFadeOneMinusValue (1 - surfaceFade)

#define SurfaceFade(outColor)\
    outColor.rgb = _FinalColorSurfaceFade ? outColor.rgb * surfaceFade : outColor.rgb; \
    outColor.a = _FinalAlphaSurfaceFade ? outColor.a * surfaceFade : outColor.a; \

#define SurfaceFadeCalc(worldPos, viewPos, viewDir, normal, sceneDepth)\
    float surfaceFade = 1; \
    if (_SURFACEFADE)\
    {\
        float surfaceFadeOut = _SurfaceFadeOut; \
        float surfaceFadeIn = _SurfaceFadeIn; \
        if (_Frenel)\
        {\
            float rimFade = saturate(dot(normal, viewDir)); \
            float frenelPow = _FrenelPow; \
            rimFade = pow(rimFade, frenelPow); \
            rimFade = _FrenelReverce ? 1 - rimFade : rimFade; \
            surfaceFade *= rimFade; \
        }\
        if (_SurfaceFadeType == 0)\
        {\
            surfaceFade *= smoothstep(surfaceFadeOut, surfaceFadeIn, -viewPos.z); \
        }\
        else if (_SurfaceFadeType == 1)\
        {\
            surfaceFade *= smoothstep(surfaceFadeOut, surfaceFadeIn, worldPos.y); \
        }\
        else if (_SurfaceFadeType == 2)\
        {\
            float depthDiff = sceneDepth + viewPos.z; \
            surfaceFade *= smoothstep(surfaceFadeOut, surfaceFadeIn, depthDiff); \
        }\
        surfaceFade = _SurfaceFadeMask ? 1 - (1 - surfaceFade) * GetMaskTextureValue : surfaceFade; \
    }


