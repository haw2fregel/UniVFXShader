
#define VertexAnimation(uv, vertexPos, normal, tangent) \
    float4 vertexAnimationParam = _VertexAnimParam; \
    float3 biNormal = cross(normalize(normal), normalize(tangent.xyz)); \
    float4 vertexAnimTex = float4(1, 1, 1, 1);\
    TextureSampler_LOD(vertexAnimTex, _VertexAnim, uv);\
    float3 normalTangent = vertexAnimTex.xyz * 2.0 - 1.0; \
    normalTangent *= vertexAnimationParam.xyz * vertexAnimationParam.w; \
    vertexPos.xyz += tangent * normalTangent.x + biNormal * normalTangent.y + normal * normalTangent.z;
