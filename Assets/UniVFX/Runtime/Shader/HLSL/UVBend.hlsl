
    #define GetBendUV uvBend

    #define BendUV(uv)\
        float4 uvBendParam = GetVertexDataArray4(_UVBendParam); \
        float2 uvBend = uv;\
        if(_UVPolar)\
        {\
            float2 polarDelta = uv - float2(0.5, 0.5);\
            float polarRadius = length(polarDelta) * 2;\
            float polarAngle = (atan2(polarDelta.y, polarDelta.x) + 3.14) / 6.28;\
            float2 polarUV = float2(polarRadius, polarAngle);\
            uvBend = polarUV;\
        }\
        uvBend += float2(abs(uvBend.y - uvBendParam.x) * uvBendParam.z, abs(uvBend.x - uvBendParam.y) * uvBendParam.w);
