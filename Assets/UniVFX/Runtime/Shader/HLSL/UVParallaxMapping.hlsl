#ifdef _PARALLAXMAPPING

#define ParallaxMappingFrag(uv, tangentSpaceViewDir, uv1, uv2) \
{\
    float parallaxAmplitude = GetVertexDataArray1(_ParallaxAmplitude);\
    float parallaxMap = SAMPLE_TEXTURE2D(_ParallaxTex, SamplerState_Linear_Clamp, uv).x - 0.5;\
    float2 parallax = tangentSpaceViewDir.xy * parallaxMap * parallaxAmplitude / tangentSpaceViewDir.z;\
    parallax = _ParallaxMask ? parallax * GetMaskTextureValue : parallax;\
    uv1.xy = _MainUVParallax ? uv1.xy + parallax : uv1.xy; \
    uv1.zw = _BlendUVParallax ? uv1.zw + parallax : uv1.zw;\
    uv2 = _DissolveUVParallax ? uv2 + parallax : uv2;\
}

#endif