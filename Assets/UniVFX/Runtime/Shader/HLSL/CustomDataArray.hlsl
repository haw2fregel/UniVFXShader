
float vertexData[25];
half4 vertexColorDataArray[3];

#define GetVertexDataArray1(prop) prop##_Data.x > 0 ? vertexData[prop##_Data.x] : prop.x

#define GetVertexDataArray2(prop) prop##_Data.xy > 0 ? float2(vertexData[prop##_Data.x], vertexData[prop##_Data.y]) : prop.xy

#define GetVertexDataArray3(prop) prop##_Data.xyz > 0 ? float3(vertexData[prop##_Data.x], vertexData[prop##_Data.y], vertexData[prop##_Data.z]) : prop.xyz

#define GetVertexDataArray4(prop) prop##_Data.xyzw > 0 ? float4(vertexData[prop##_Data.x], vertexData[prop##_Data.y], vertexData[prop##_Data.z], vertexData[prop##_Data.w]) : prop.xyzw

#define GetVertexColorDataArray(prop) prop##_Data.x > 0 ? prop##_Data.x > 3 ? vertexColorData : vertexColorDataArray[prop##_Data.x] : prop.xyzw

#define GetUVDataArraty(index) uvData[index]

#define VertexDataArrayInitVert(texcoord1, texcoord2, vertexColor)\
    float4 times = _Time * _TimeSpeed;\
    float4 timeMaps = SAMPLE_TEXTURE2D_LOD(_TimeMap, sampler_TimeMap, frac(times.yy), 0);\
    vertexData[0] = 0.0;\
    vertexData[1] = texcoord1.x;\
    vertexData[2] = texcoord1.y;\
    vertexData[3] = texcoord1.z;\
    vertexData[4] = texcoord1.w;\
    vertexData[5] = texcoord2.x;\
    vertexData[6] = texcoord2.y;\
    vertexData[7] = texcoord2.z;\
    vertexData[8] = texcoord2.w;\
    vertexData[9] = vertexColor.x;\
    vertexData[10] = vertexColor.y;\
    vertexData[11] = vertexColor.z;\
    vertexData[12] = vertexColor.w;\
    vertexData[13] = times.x;\
    vertexData[14] = times.y;\
    vertexData[15] = times.z;\
    vertexData[16] = times.w;\
    vertexData[17] = -times.x;\
    vertexData[18] = -times.y;\
    vertexData[19] = -times.z;\
    vertexData[20] = -times.w;\
    vertexData[21] = timeMaps.x;\
    vertexData[22] = timeMaps.y;\
    vertexData[23] = timeMaps.z;\
    vertexData[24] = timeMaps.w;

#define VertexDataArrayInitFrag(texcoord1, texcoord2, vertexColor)\
    float4 times = _Time * _TimeSpeed;\
    float4 timeMaps = SAMPLE_TEXTURE2D(_TimeMap, sampler_TimeMap, frac(times.yy));\
    vertexData[0] = 0.0;\
    vertexData[1] = texcoord1.x;\
    vertexData[2] = texcoord1.y;\
    vertexData[3] = texcoord1.z;\
    vertexData[4] = texcoord1.w;\
    vertexData[5] = texcoord2.x;\
    vertexData[6] = texcoord2.y;\
    vertexData[7] = texcoord2.z;\
    vertexData[8] = texcoord2.w;\
    vertexData[9] = vertexColor.x;\
    vertexData[10] = vertexColor.y;\
    vertexData[11] = vertexColor.z;\
    vertexData[12] = vertexColor.w;\
    vertexData[13] = times.x;\
    vertexData[14] = times.y;\
    vertexData[15] = times.z;\
    vertexData[16] = times.w;\
    vertexData[17] = -times.x;\
    vertexData[18] = -times.y;\
    vertexData[19] = -times.z;\
    vertexData[20] = -times.w;\
    vertexData[21] = timeMaps.x;\
    vertexData[22] = timeMaps.y;\
    vertexData[23] = timeMaps.z;\
    vertexData[24] = timeMaps.w;

#define CanvasVertexDataArrayInitVert(texcoord2, texcoord3, vertexColor)\
    float4 times = _Time * _TimeSpeed;\
    float4 timeMaps = SAMPLE_TEXTURE2D_LOD(_TimeMap, sampler_TimeMap, frac(times.yy), 0);\
    vertexData[0] = 0.0;\
    vertexData[1] = texcoord2.x;\
    vertexData[2] = texcoord2.y;\
    vertexData[3] = texcoord2.z;\
    vertexData[4] = texcoord2.w;\
    vertexData[5] = vertexColor.x;\
    vertexData[6] = vertexColor.y;\
    vertexData[7] = vertexColor.z;\
    vertexData[8] = vertexColor.w;\
    vertexData[9] = times.x;\
    vertexData[10] = times.y;\
    vertexData[11] = times.z;\
    vertexData[12] = times.w;\
    vertexData[13] = -times.x;\
    vertexData[14] = -times.y;\
    vertexData[15] = -times.z;\
    vertexData[16] = -times.w;\
    vertexData[17] = timeMaps.x;\
    vertexData[18] = timeMaps.y;\
    vertexData[19] = timeMaps.z;\
    vertexData[20] = timeMaps.w;

#define CanvasVertexDataArrayInitFrag(texcoord2, texcoord3, vertexColor)\
    float4 times = _Time * _TimeSpeed;\
    float4 timeMaps = SAMPLE_TEXTURE2D(_TimeMap, sampler_TimeMap, frac(times.yy));\
    vertexData[0] = 0.0;\
    vertexData[1] = texcoord2.x;\
    vertexData[2] = texcoord2.y;\
    vertexData[3] = texcoord2.z;\
    vertexData[4] = texcoord2.w;\
    vertexData[5] = vertexColor.x;\
    vertexData[6] = vertexColor.y;\
    vertexData[7] = vertexColor.z;\
    vertexData[8] = vertexColor.w;\
    vertexData[9] = times.x;\
    vertexData[10] = times.y;\
    vertexData[11] = times.z;\
    vertexData[12] = times.w;\
    vertexData[13] = -times.x;\
    vertexData[14] = -times.y;\
    vertexData[15] = -times.z;\
    vertexData[16] = -times.w;\
    vertexData[17] = timeMaps.x;\
    vertexData[18] = timeMaps.y;\
    vertexData[19] = timeMaps.z;\
    vertexData[20] = timeMaps.w;

#define VertexColorDataArrayInit(texcoord1, texcoord2, vertexColor)\
    vertexColorDataArray[0] = half4(0.0,0.0,0.0,0.0);\
    vertexColorDataArray[1] = texcoord1;\
    vertexColorDataArray[2] = texcoord2;\
    half4 vertexColorData = vertexColor;\


#define CanvasVertexColorDataArrayInit(texcoord2, texcoord3, vertexColor)\
    vertexColorDataArray[0] = half4(0.0,0.0,0.0,0.0);\
    vertexColorDataArray[1] = texcoord2.xyxy;\
    vertexColorDataArray[2] = texcoord3.xyxy;\
    half4 vertexColorData = vertexColor;\

#define UVArrayInitFrag(uv, position, viewNormal, screenPosition, rotateUV, fripBookUV) \
    float2 uvData[9] = \
    {\
        uv,\
        position.xy,\
        position.xz,\
        position.yz,\
        screenPosition.xy,\
        viewNormal.xy + 0.5,\
        rotateUV,\
        fripBookUV,\
        GetBendUV\
    };

#define CanvasUVArrayInitFrag(uv, uv2, screenPosition, rotateUV, fripBookUV) \
    float2 uvData[6] = \
    {\
        uv,\
        uv2,\
        screenPosition.xy,\
        rotateUV,\
        fripBookUV,\
        GetBendUV\
    };

#define UVArrayInitVert(uv, position, viewNormal, screenPosition) \
    float2 uvData[9] = \
    {\
        uv,\
        position.xy,\
        position.xz,\
        position.yz,\
        screenPosition.xy,\
        viewNormal.xy + 0.5,\
        GetRotateUV,\
        GetFripBookUV,\
        uv\
    };

#define CanvasUVArrayInitVert(uv, uv2, screenPosition) \
    float2 uvData[6] = \
    {\
        uv,\
        uv2,\
        screenPosition.xy,\
        GetRotateUV,\
        GetFripBookUV,\
        uv\
    };

