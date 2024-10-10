

#define GetVertexDataArray1(prop) prop##_Data.x > 0 ? vertexData[prop##_Data.x] : prop.x

#define GetVertexDataArray2(prop) prop##_Data.xy > 0 ? float2(vertexData[prop##_Data.x], vertexData[prop##_Data.y]) : prop.xy

#define GetVertexDataArray3(prop) prop##_Data.xyz > 0 ? float3(vertexData[prop##_Data.x], vertexData[prop##_Data.y], vertexData[prop##_Data.z]) : prop.xyz

#define GetVertexDataArray4(prop) prop##_Data.xyzw > 0 ? float4(vertexData[prop##_Data.x], vertexData[prop##_Data.y], vertexData[prop##_Data.z], vertexData[prop##_Data.w]) : prop.xyzw

#define GetVertexColorDataArray(prop) prop##_Data.x > 0 ? vertexColorData[prop##_Data.x] : prop.xyzw

#define GetUVDataArraty(index) uvData[index]

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

