
#define UVTransform(uv, prop, name) \
    float4 name##Transform = (prop##Transform); \
    uv = (GetUVDataArraty(prop##Transform_Index) - float2(0.5, 0.5)) * name##Transform.xy + float2(0.5, 0.5) + name##Transform.zw;

#define TextureSampler(result, prop, uv)\
    if (prop##UVTransform_Sampler == 0)\
    {\
        result = SAMPLE_TEXTURE2D(prop##Tex, SamplerState_Linear_Clamp, uv);\
    }\
    else if (prop##UVTransform_Sampler == 1)\
    {\
        result = SAMPLE_TEXTURE2D(prop##Tex, SamplerState_Linear_Repeat, uv);\
    }\
    else if (prop##UVTransform_Sampler == 2)\
    {\
        result = SAMPLE_TEXTURE2D(prop##Tex, SamplerState_Linear_Mirror, uv);\
    }\
    else if (prop##UVTransform_Sampler == 3)\
    {\
        result = SAMPLE_TEXTURE2D(prop##Tex, SamplerState_Linear_MirrorOnce, uv);\
    }

#define TextureSampler_LOD(result, prop, uv)\
    if (prop##UVTransform_Sampler == 0)\
    {\
        result = SAMPLE_TEXTURE2D_LOD(prop##Tex, SamplerState_Linear_Clamp, uv, 0);\
    }\
    else if (prop##UVTransform_Sampler == 1)\
    {\
        result = SAMPLE_TEXTURE2D_LOD(prop##Tex, SamplerState_Linear_Repeat, uv, 0);\
    }\
    else if (prop##UVTransform_Sampler == 2)\
    {\
        result = SAMPLE_TEXTURE2D_LOD(prop##Tex, SamplerState_Linear_Mirror, uv, 0);\
    }\
    else if (prop##UVTransform_Sampler == 3)\
    {\
        result = SAMPLE_TEXTURE2D_LOD(prop##Tex, SamplerState_Linear_MirrorOnce, uv, 0);\
    }