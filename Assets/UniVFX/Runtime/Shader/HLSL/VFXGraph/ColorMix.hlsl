
#define ColorMixAdd(baseColor, mixColor, mixAlpha) baseColor + mixColor * mixAlpha;

#define ColorMixSub(baseColor, mixColor, mixAlpha) baseColor - mixColor * mixAlpha;

#define ColorMixMultiple(baseColor, mixColor, mixAlpha) baseColor * (1 - (1 - mixColor) * (mixAlpha));

#define ColorMixAlpha(baseColor, mixColor, mixAlpha) lerp(baseColor, mixColor, mixAlpha);

#define ColorMixOverlay(baseColor, mixColor, mixAlpha) lerp(baseColor, baseColor < 0.5 ? 2.0 * mixColor * baseColor : 1.0 - (1.0 - mixColor) * (1.0 - baseColor), mixAlpha);

#define ColorMix(resultColor , baseColor, mixColor, mixAlpha, blendMode)\
{\
    if(blendMode == 0)\
    {\
        resultColor = ColorMixAlpha(baseColor, mixColor, mixAlpha);\
    }else if(blendMode == 1)\
    {\
        resultColor = ColorMixAdd(baseColor, mixColor, mixAlpha);\
    }else if(blendMode == 2)\
    {\
        resultColor = ColorMixMultiple(baseColor, mixColor, mixAlpha);\
    }else if(blendMode == 3)\
    {\
        resultColor = ColorMixSub(baseColor, mixColor, mixAlpha);\
    }\
    else if(blendMode == 4)\
    {\
        resultColor = ColorMixOverlay(baseColor, mixColor, mixAlpha);\
    }\
}

