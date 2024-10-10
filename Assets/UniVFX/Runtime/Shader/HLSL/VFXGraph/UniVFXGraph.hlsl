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

void UniVFXGraphVert_float(float3 vertex, float3 normal, float3 tangent, float4 texcoord0, float4 texcoord1, float4 texcoord2, float4 color, float3 viewNormal, float2 screenPosition,
float4 _MainUVTransform, float _MainUVTransform_Index,
bool _MASKTEXTURE, float4 _MaskUVTransform, float _MaskUVTransform_Index,
bool _BLENDTEXTURE, float4 _BlendUVTransform, float _BlendUVTransform_Index,
bool _GRADATIONCOLOR, float4 _GradationUVTransform, float _GradationUVTransform_Index,
bool _DISTORTIONNABLE, float4 _DistortionUVTransform, float _DistortionUVTransform_Index,
bool _VERTEXANIMATION, UnityTexture2D _VertexAnimTex, float4 _VertexAnimUVTransform, float _VertexAnimUVTransform_Index, float _VertexAnimUVTransform_Sampler, float4 _VertexAnimParam,
bool _DISSOLVE, float4 _DissolveUVTransform, float _DissolveUVTransform_Index,
bool _FRIPBOOK, float _FripBookRow, float _FripBookColumn, float _FripBookIndex,
bool _UVROTATEENABLE, float _UVRotate,
bool _UVBEND,
UnityTexture2D _TimeMap, float _TimeSpeed,
out float3 outPos, out float4 uv1, out float4 uv2, out float4 uv3, out float4 uv4)
{
    float2 uv = texcoord0.xy;
    uv1 = uv.xyxy;
    uv2 = uv.xyxy;
    uv3 = uv.xyxy;
    uv4 = uv.xyxy;
    float2 uv5 = uv.xy;

    outPos = vertex;

    RotateUV(uv, uv4.xy);
    FripBookUV(uv, uv4.zw);
    
    UVArrayInitVert(uv, vertex, viewNormal, screenPosition)

    if (true)
    {
        UVTransform(uv1.xy, _MainUV, mainUV)
        if (_BLENDTEXTURE)
        {
            UVTransform(uv1.zw, _BlendUV, blendUV)
        }

        if (_GRADATIONCOLOR)
        {
            UVTransform(uv2.xy, _GradationUV, gradationUV)
        }

        if (_DISSOLVE)
        {
            UVTransform(uv2.zw, _DissolveUV, dissolveUV)
        }

        if (_MASKTEXTURE)
        {
            UVTransform(uv3.xy, _MaskUV, maskUV)
        }

        if (_DISTORTIONNABLE)
        {
            UVTransform(uv3.zw, _DistortionUV, distortionUV)
        }
    }

    if (_VERTEXANIMATION)
    {
        UVTransform(uv5.xy, _VertexAnimUV, vertexAnimUV); \
        VertexAnimation(uv5.xy, outPos, normal, tangent)
    }
}


void UniVFXGraphFrag_half(float4 uv1, float4 uv2, float4 uv3, float4 uv4,
float4 texcoord0, float4 texcoord1, float4 texcoord2, float4 color,
float3 worldPos, float3 viewPos, float3 normal, float3 viewNormal, float3 viewDir, float3 boundsCenter, float sceneDepth, float3 tangentSpaceViewDir, bool isFront,
UnityTexture2D _MainTex, float4 _MainUVTransform, float _MainUVTransform_Index, float _MainUVTransform_Sampler, float4 _MainColor, bool _MainColorMultiple, bool _MainAlphaMultiple,
bool _MASKTEXTURE, UnityTexture2D _MaskTex, float4 _MaskUVTransform, float _MaskUVTransform_Index, float _MaskUVTransform_Sampler, float _MaskOffset, bool _MaskRepeat,
bool _MainTexMask, bool _BlendTexMask, bool _SurfaceFadeMask, bool _ParallaxMask, bool _DistortionMask, bool _DissolveMask, bool _HSVShiftMask, bool _FakeLightMask, bool _GradationMask,
bool _BLENDTEXTURE, UnityTexture2D _BlendTex, float4 _BlendUVTransform, float _BlendUVTransform_Index, float _BlendUVTransform_Sampler, float4 _BlendTexColor, float _BlendTexBlendMode, float _BlendIntensity,
bool _GRADATIONCOLOR, float4 _GradationColor00, float4 _GradationColor01, float4 _GradationColor10, float4 _GradationColor11, float4 _GradationUVTransform, float _GradationUVTransform_Index, float _GradationBlendMode,
bool _DISTORTIONENABLE, UnityTexture2D _DistortionTex, float4 _DistortionUVTransform, float _DistortionUVTransform_Index, float _DistortionUVTransform_Sampler, float _DistortionIntensity,
bool _MainUVDistortion, bool _BlendUVDistortion, bool _DissolveUVDistortion, bool _GradationUVDistortion,
bool _PARALLAXMAPPING, UnityTexture2D _ParallaxTex, float _ParallaxAmplitude, bool _MainUVParallax, bool _BlendUVParallax, bool _DissolveUVParallax, bool _MaskUVParallax,
bool _HSVSHIFT, float4 _HSVShiftParametors, 
bool _SURFACEFADE, float _SurfaceFadeType, float _SurfaceFadeIn, float _SurfaceFadeOut, bool _Frenel, float _FrenelPow, bool _FrenelReverce,
bool _FinalColorSurfaceFade, bool _FinalAlphaSurfaceFade, bool _MainTexSurfaceFade, bool _BlendTexSurfaceFade, bool _HSVShiftSurfaceFade, bool _FakeLightSurfaceFade, bool _DistortionSurfaceFade, bool _DissolveSurfaceFade, bool _GradationSurfaceFade,
bool _FAKELIGHT, float _FakeLightType, UnityTexture2D _FakeLightMap, float _FakeLightIntensity, float4 _FakeLightColor, float4 _FakeShadowColor, float4 _FakeLightParam,
bool _DISSOLVE, UnityTexture2D _DissolveTex, float4 _DissolveColor, float4 _DissolveParam, float4 _DissolveUVTransform, float _DissolveUVTransform_Index, float _DissolveUVTransform_Sampler,
bool _UVBEND, bool _UVPolar, float4 _UVBendParam,
bool _FACECOLOR, float4 _BackFaceColor, float4 _FrontFaceColor,
UnityTexture2D _TimeMap, float _TimeSpeed, bool _ColorMultiplAlpha,
out float4 outColor)
{
    outColor = half4(0, 0, 0, 0);

    //UVBendがOnの場合はTileOffsetもFragmentで行う
    if (_UVBEND)
    {
        BendUV(texcoord0.xy)
        UVArrayInitFrag(texcoord0.xy, worldPos, viewNormal, viewPos, uv4.xy, uv4.zw)

        UVTransform(uv1.xy, _MainUV, mainUV)
        if (_BLENDTEXTURE)
        {
            UVTransform(uv1.zw, _BlendUV, blendUV)
        }
        
        if (_GRADATIONCOLOR)
        {
            UVTransform(uv2.xy, _GradationUV, gradationUV)
        }

        if (_DISSOLVE)
        {
            UVTransform(uv2.zw, _DissolveUV, dissolveUV)
        }

        if (_MASKTEXTURE)
        {
            UVTransform(uv3.xy, _MaskUV, maskUV)
        }

        if (_DISTORTIONENABLE)
        {
            UVTransform(uv3.zw, _DistortionUV, distortionUV)
        }
    }

    //Mask用テクスチャのサンプリング
    //GetMaskTextureValueで値を取得する
    MaskTextureSample(uv3.xy)

    //Surface情報を利用したフェード機能
    //GetSurfaceFadeValueで値を取得する
    SurfaceFadeCalc(worldPos, viewPos, viewDir, normal, sceneDepth)

    //uv1, uv2　にディストーションを適用
    if (_DISTORTIONENABLE)
    {
        DistortionUV(uv3.zw, uv1, uv2)
    }


    //uv1, uv2.zw　にパララックスを適用
    if (_PARALLAXMAPPING)
    {
        ParallaxMappingFrag(texcoord0.xy, tangentSpaceViewDir, uv1, uv2.zw)
    }

    MainTextureSample(uv1.xy, outColor)

    
    
    if (_BLENDTEXTURE)
    {
        BlendTexture(uv1.zw, outColor)
    }
    
    if (_GRADATIONCOLOR)
    {
        GradationColor(uv2.xy, outColor)
    }

    if (_FAKELIGHT)
    {
        FakeLight(worldPos, normal, boundsCenter, outColor)
    }

    

    if (_DISSOLVE)
    {
        Dissolve(uv2.zw, outColor)
    }

    if (_HSVSHIFT)
    {
        HSVColorBlend(outColor)
    }

    if (_SURFACEFADE)
    {
        SurfaceFade(outColor)
    }

    if (_FACECOLOR)
    {
        outColor *= isFront ? (_FrontFaceColor) : (_BackFaceColor);
    }

    

    outColor.rgb = _ColorMultiplAlpha > 0 ? outColor.rgb * outColor.a : outColor.rgb;
    outColor.a = saturate(outColor.a);
}

#endif
