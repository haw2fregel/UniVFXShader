
    #define HSVShift(rbg, hsvShift)\
    {\
        half4 hsv_k = half4(0.0, -0.3333, 0.6666, -1.0); \
        half4 hsv_p = lerp(half4(rbg.b, rbg.g, hsv_k.w, hsv_k.z), half4(rbg.y, rbg.z, hsv_k.x, hsv_k.y), step(rbg.b, rbg.g)); \
        half4 hsv_q = lerp(half4(hsv_p.x, hsv_p.y, hsv_p.w, rbg.r), half4(rbg.r, hsv_p.y, hsv_p.z, hsv_p.x), step(hsv_p.x, rbg.r)); \
        half hsv_d = hsv_q.x - min(hsv_q.w, hsv_q.y); \
        half3 hsv = half3(abs(hsv_q.z + (hsv_q.w - hsv_q.y) / (6.0 * hsv_d + 0.001)), hsv_d / (hsv_q.x + 0.001), hsv_q.x); \
        rbg = lerp(half3(1, 1, 1), saturate(3.0 * abs(1.0 - 2.0 * frac((hsvShift.x + hsv.r) + half3(0.0, -0.3333, 0.3333))) - 1), (hsv.g + hsvShift.y)) * (hsv.b + hsvShift.z); \
    }

    #ifdef _SURFACEFADE
        #define HSVColorBlend(baseCol)\
            half3 hsvParam = GetVertexDataArray3(_HSVShiftParametors);\
            hsvParam = _HSVShiftSurfaceFade ? hsvParam * (1 - GetSurfaceFadeValue) : hsvParam;\
            hsvParam = _HSVShiftMask ? hsvParam * GetMaskTextureValue : hsvParam;\
            HSVShift(baseCol.rgb, hsvParam);
    #else
        #define HSVColorBlend(baseCol)\
            half3 hsvParam = GetVertexDataArray3(_HSVShiftParametors);\
            hsvParam = _HSVShiftMask ? hsvParam * GetMaskTextureValue : hsvParam;\
            HSVShift(baseCol.rgb, hsvParam);
    #endif



