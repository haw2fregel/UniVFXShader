#ifndef UNIVFX_INCLUDED
#define UNIVFX_INCLUDED

#include "CustomDataArray.hlsl"
#include "TextureTrasform.hlsl"
#include "ColorMix.hlsl"
#include "MaskTexture.hlsl"
#include "MainTexture.hlsl"
#include "BlendTexture.hlsl"
#include "Dissolve.hlsl"
#include "SurfaceFade.hlsl"
#include "FakeLight.hlsl"
#include "HSVShift.hlsl"
#include "UVDistortion.hlsl"
#include "UVParallaxMapping.hlsl"
#include "UVBend.hlsl"
#include "GradationColor.hlsl"
#include "UVFripBook.hlsl"
#include "UVRotate.hlsl"
#include "VertexAnimation.hlsl"

void UniVFXVert_float(float3 vertex, float3 normal, float3 tangent, float4 texcoord0, float4 texcoord1, float4 texcoord2, float4 texcoord3, float4 color, float3 viewNormal, float2 screenPosition, out float3 outPos, out float4 uv1, out float4 uv2, out float4 uv3, out float4 uv4)
{
    float2 uv = texcoord0.xy;
    uv1 = uv.xyxy;
    uv2 = uv.xyxy;
    uv3 = uv.xyxy;
    uv4 = uv.xyxy;
    float2 uv5 = uv.xy;

    outPos = vertex;

    CanvasVertexDataArrayInitVert(texcoord2, texcoord3, color);

    RotateUV(uv, uv4.xy);
    FripBookUV(uv, uv4.zw);
    
    CanvasUVArrayInitVert(uv, texcoord1.xy, screenPosition)

    #ifndef _UVBEND
        UVTransform(uv1.xy, _MainUV, mainUV)
        if (_BLENDTEXTURE)
        {
            UVTransform(uv1.zw, _BlendUV, blendUV)
        }

        #ifdef _GRADATIONCOLOR
            UVTransform(uv2.xy, _GradationUV, gradationUV)
        #endif

        #ifdef _DISSOLVE
            UVTransform(uv2.zw, _DissolveUV, dissolveUV)
        #endif

        if (_MASKTEXTURE)
        {
            UVTransform(uv3.xy, _MaskUV, maskUV)
        }

        if (_DISTORTIONENABLE)
        {
            UVTransform(uv3.zw, _DistortionUV, distortionUV)
        }
        
    #endif
}

void UniVFXFrag_half(float4 uv1, float4 uv2, float4 uv3, float4 uv4, float4 texcoord0, float4 texcoord1, float4 texcoord2, float4 texcoord3, float4 color, float3 worldPos, float3 viewPos, float3 normal, float3 viewNormal, float3 viewDir, float3 boundsCenter, float sceneDepth, float3 tangentSpaceViewDir, bool isFront, out float4 outColor)
{
    outColor = half4(0, 0, 0, 0);

    //CustomDataを配列に詰め込む
    //ここで用意した配列を各パラメータで参照する
    CanvasVertexDataArrayInitFrag(texcoord2, texcoord3, color);
    CanvasVertexColorDataArrayInit(texcoord2, texcoord3, color);

    RotateUV(uv2.xy, uv4.xy);
    FripBookUV(uv2.xy, uv4.zw);
    
    #ifdef _UVBEND
        BendUV(texcoord0.xy)
        
        CanvasUVArrayInitFrag(texcoord0.xy, texcoord2.xy, viewPos, uv4.xy, uv4.zw)

        UVTransform(uv1.xy, _MainUV, mainUV)
        if (_BLENDTEXTURE)
        {
            UVTransform(uv1.zw, _BlendUV, blendUV)
        }
        
        #ifdef _GRADATIONCOLOR
            UVTransform(uv2.xy, _GradationUV, gradationUV)
        #endif

        #ifdef _DISSOLVE
            UVTransform(uv2.zw, _DissolveUV, dissolveUV)
        #endif

        if (_MASKTEXTURE)
        {
            UVTransform(uv3.xy, _MaskUV, maskUV)
        }

        if (_DISTORTIONENABLE)
        {
            UVTransform(uv3.zw, _DistortionUV, distortionUV)
        }
    #endif
    

    //Mask用テクスチャのサンプリング
    //GetMaskTextureValueで値を取得する
    MaskTextureSample(uv3.xy)

    //uv1, uv2　にディストーションを適用
    if (_DISTORTIONENABLE)
    {
        DistortionUV(uv3.zw, uv1, uv2)
    }

    MainTextureSample(uv1.xy, outColor)

    
    if (_BLENDTEXTURE)
    {
        BlendTexture(uv1.zw, outColor)
    }
    
    #ifdef _GRADATIONCOLOR
        GradationColor(uv2.xy, outColor)
    #endif

    #ifdef _DISSOLVE
        Dissolve(uv2.zw, outColor)
    #endif

    #ifdef _HSVSHIFT
        HSVColorBlend(outColor)
    #endif

    
}

#endif
