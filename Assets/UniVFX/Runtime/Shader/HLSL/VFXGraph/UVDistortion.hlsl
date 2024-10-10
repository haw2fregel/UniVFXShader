
    #define DistortionUV(uv, uv1, uv2)\
        float distortionIntensity = _DistortionIntensity; \
        float4 distortionTex = float4(1, 1, 1, 1);\
        TextureSampler(distortionTex, _Distortion, uv);\
        float2 distortionMap = distortionTex.xy - float2(0.5, 0.5); \
        float2 distortion = distortionMap * distortionIntensity; \
        if(_SURFACEFADE)\
        {\
            distortion = _DistortionSurfaceFade != 0  ? distortion * (1 - GetSurfaceFadeValue) : distortion; \
        }\
        distortion = _DistortionMask ? distortion * GetMaskTextureValue : distortion;\
        uv1.xy = _MainUVDistortion ? uv1.xy + distortion : uv1.xy; \
        uv1.zw = _BlendUVDistortion ? uv1.zw + distortion : uv1.zw; \
        uv2.xy = _GradationUVDistortion ? uv2.xy + distortion : uv2.xy;\
        uv2.zw = _DissolveUVDistortion ? uv2.zw + distortion : uv2.zw;
