#ifdef _SURFACEFADE
    #define FakeLight(worldPos, normal, boundsCenter, baseColor) \
        float3 fakeLightDir; \
        float attan; \
        float4 fakeLightParam = GetVertexDataArray4(_FakeLightParam); \
        float fakeLightIntensity = GetVertexDataArray1(_FakeLightIntensity); \
        half4 lightColor = GetVertexColorDataArray(_FakeLightColor); \
        half4 shadowColor = GetVertexColorDataArray(_FakeShadowColor); \
        if (_FakeLightType == 0)\
        {\
            fakeLightDir = worldPos - (boundsCenter + fakeLightParam.xyz); \
            attan = 1; \
        }else if (_FakeLightType == 1)\
        {\
            fakeLightDir = worldPos - (boundsCenter + fakeLightParam.xyz); \
            attan = min(length(fakeLightDir), fakeLightParam.w); \
            attan = 1 - (attan / fakeLightParam.w); \
            attan *= attan; \
        }\
        fakeLightIntensity = saturate(fakeLightIntensity * attan); \
        fakeLightIntensity = _FakeLightSurfaceFade ? fakeLightIntensity * GetSurfaceFadeValue : fakeLightIntensity; \
        fakeLightIntensity = _FakeLightMask ? fakeLightIntensity * GetMaskTextureValue : fakeLightIntensity; \
        float nDotL = -dot(normal, normalize(fakeLightDir)) * 0.5 + 0.5; \
        half fakeLightMap = SAMPLE_TEXTURE2D(_FakeLightMap, sampler_FakeLightMap, nDotL.xx).x; \
        baseColor.rgb = lerp(baseColor.rgb, baseColor.rgb * shadowColor.rgb, (1 - saturate(fakeLightMap * 2)) * fakeLightIntensity); \
        baseColor.rgb = lerp(baseColor.rgb, baseColor.rgb + lightColor.rgb, saturate((fakeLightMap - 0.5) * 2) * fakeLightIntensity);
#else
    #define FakeLight(worldPos, normal, boundsCenter, baseColor) \
        float3 fakeLightDir; \
        float attan; \
        float4 fakeLightParam = GetVertexDataArray4(_FakeLightParam); \
        float fakeLightIntensity = GetVertexDataArray1(_FakeLightIntensity); \
        half4 lightColor = GetVertexColorDataArray(_FakeLightColor); \
        half4 shadowColor = GetVertexColorDataArray(_FakeShadowColor); \
        if (_FakeLightType == 0)\
        {\
            fakeLightDir = worldPos - (boundsCenter + fakeLightParam.xyz); \
            attan = 1; \
        }else if (_FakeLightType == 1)\
        {\
            fakeLightDir = worldPos - (boundsCenter + fakeLightParam.xyz); \
            attan = min(length(fakeLightDir), fakeLightParam.w); \
            attan = 1 - (attan / fakeLightParam.w); \
            attan *= attan; \
        }\
        fakeLightIntensity = saturate(fakeLightIntensity * attan); \
        fakeLightIntensity = _FakeLightMask ? fakeLightIntensity * GetMaskTextureValue : fakeLightIntensity; \
        float nDotL = -dot(normal, normalize(fakeLightDir)) * 0.5 + 0.5; \
        half fakeLightMap = SAMPLE_TEXTURE2D(_FakeLightMap, SamplerState_Linear_Clamp, nDotL.xx).x; \
        baseColor.rgb = lerp(baseColor.rgb, baseColor.rgb * shadowColor.rgb, (1 - saturate(fakeLightMap * 2)) * fakeLightIntensity); \
        baseColor.rgb = lerp(baseColor.rgb, baseColor.rgb + lightColor.rgb, saturate((fakeLightMap - 0.5) * 2) * fakeLightIntensity);
#endif
