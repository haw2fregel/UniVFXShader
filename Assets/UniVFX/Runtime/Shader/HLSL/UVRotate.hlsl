
    #define GetRotateUV rotateUV

    #define RotateUV(uv, uv1) \
        float2 rotateUV = uv;\
        if(_ROTATEUVENABLE)\
        {\
            float rotateValue = GetVertexDataArray1(_UVRotate); \
            float rotateAngle = rotateValue * 3.14 / 180 ; \
            float rotateCos = cos(rotateAngle); \
            float rotateSin = sin(rotateAngle); \
            rotateUV = (mul(uv - float2(0.5, 0.5), float2x2(rotateCos, -rotateSin, rotateSin, rotateCos)) + float2(0.5, 0.5));\
            uv1 = rotateUV;\
        }