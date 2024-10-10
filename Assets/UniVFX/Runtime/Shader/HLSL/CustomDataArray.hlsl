

#define GetVertexDataArray1(prop) prop##_Data.x > 0 ? vertexData[prop##_Data.x] : prop.x

#define GetVertexDataArray2(prop) prop##_Data.xy > 0 ? float2(vertexData[prop##_Data.x], vertexData[prop##_Data.y]) : prop.xy

#define GetVertexDataArray3(prop) prop##_Data.xyz > 0 ? float3(vertexData[prop##_Data.x], vertexData[prop##_Data.y], vertexData[prop##_Data.z]) : prop.xyz

#define GetVertexDataArray4(prop) prop##_Data.xyzw > 0 ? float4(vertexData[prop##_Data.x], vertexData[prop##_Data.y], vertexData[prop##_Data.z], vertexData[prop##_Data.w]) : prop.xyzw

#define GetVertexColorDataArray(prop) prop##_Data.x > 0 ? vertexColorData[prop##_Data.x] : prop.xyzw

#define GetUVDataArraty(index) uvData[index]

#define VertexDataArrayInitVert(texcoord1, texcoord2, vertexColor)\
    float4 times = _Time * _TimeSpeed;\
    float4 timeMaps = SAMPLE_TEXTURE2D_LOD(_TimeMap, sampler_TimeMap, frac(times.yy), 0);\
    float vertexData[25] = \
    {\
        0.0, \
        texcoord1.x, texcoord1.y, texcoord1.z, texcoord1.w, \
        texcoord2.x, texcoord2.y, texcoord2.z, texcoord2.w, \
        vertexColor.r, vertexColor.g, vertexColor.b, vertexColor.a, \
        times.x, times.y, times.z, times.w, \
        -times.x, -times.y, -times.z, -times.w, \
        timeMaps.x, timeMaps.y, timeMaps.z, timeMaps.w\
    };

#define VertexDataArrayInitFrag(texcoord1, texcoord2, vertexColor)\
    float4 times = _Time * _TimeSpeed;\
    float4 timeMaps = SAMPLE_TEXTURE2D(_TimeMap, sampler_TimeMap, frac(times.yy));\
    float vertexData[25] = \
    {\
        0.0, \
        texcoord1.x, texcoord1.y, texcoord1.z, texcoord1.w, \
        texcoord2.x, texcoord2.y, texcoord2.z, texcoord2.w, \
        vertexColor.r, vertexColor.g, vertexColor.b, vertexColor.a, \
        times.x, times.y, times.z, times.w, \
        -times.x, -times.y, -times.z, -times.w, \
        timeMaps.x, timeMaps.y, timeMaps.z, timeMaps.w\
    };

#define VertexColorDataArrayInit(texcoord1, texcoord2, vertexColor)\
    half4 vertexColorData[4] = \
    {\
        half4(0.0,0.0,0.0,0.0),\
        texcoord1,\
        texcoord2,\
        vertexColor\
    };

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

