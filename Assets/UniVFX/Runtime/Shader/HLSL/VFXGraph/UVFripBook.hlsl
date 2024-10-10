#define GetFripBookUV fripbookUV

#define FripBookUV(uv, uv1) \
    float2 fripbookUV = uv;\
    if(_FRIPBOOK)\
    {\
        int fripbookIndex = _FripBookIndex; \
        float invRow = 1.0 / (float)_FripBookRow; \
        float invColumn = 1.0 / (float)_FripBookColumn; \
        int indexColumn = fripbookIndex / _FripBookRow; \
        int indexRow = fripbookIndex % _FripBookRow; \
        float2 fripbook_tiling = float2(invRow, invColumn); \
        float2 fripbook_offset = float2(indexRow * invRow, indexColumn * invColumn); \
        fripbookUV = (uv * fripbook_tiling) + fripbook_offset; \
        uv1 = fripbookUV;\
    }
