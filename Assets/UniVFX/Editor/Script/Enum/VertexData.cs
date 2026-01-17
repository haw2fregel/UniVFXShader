namespace UniVFX.Editor
{
    public enum VertexData
    {
        Input,
        TEXCOORD1X,
        TEXCOORD1Y,
        TEXCOORD1Z,
        TEXCOORD1W,
        TEXCOORD2X,
        TEXCOORD2Y,
        TEXCOORD2Z,
        TEXCOORD2W,
        VertexColorR,
        VertexColorG,
        VertexColorB,
        VertexColorA,
        TimeX,
        TimeY,
        TimeZ,
        TimeW,
        MinusTimeX,
        MinusTimeY,
        MinusTimeZ,
        MinusTimeW,
        TimeMapX,
        TimeMapY,
        TimeMapZ,
        TimeMapW
    }

    public class VertexDataConvert
    {
        public static string VertexDataToCode(int vertexData, string prop)
        {
            switch (vertexData)
            {
                case (int)VertexData.Input:
                    return prop;
                case (int)VertexData.TEXCOORD1X:
                    return "texCoord1.x";
                case (int)VertexData.TEXCOORD1Y:
                    return "texCoord1.y";
                case (int)VertexData.TEXCOORD1Z:
                    return "texCoord1.z";
                case (int)VertexData.TEXCOORD1W:
                    return "texCoord1.w";
                case (int)VertexData.TEXCOORD2X:
                    return "texCoord2.x";
                case (int)VertexData.TEXCOORD2Y:
                    return "texCoord2.y";
                case (int)VertexData.TEXCOORD2Z:
                    return "texCoord2.z";
                case (int)VertexData.TEXCOORD2W:
                    return "texCoord2.w";
                case (int)VertexData.VertexColorR:
                    return "vertexColor.r";
                case (int)VertexData.VertexColorG:
                    return "vertexColor.g";
                case (int)VertexData.VertexColorB:
                    return "vertexColor.b";
                case (int)VertexData.VertexColorA:
                    return "vertexColor.a";
                case (int)VertexData.TimeX:
                    return "time.x";
                case (int)VertexData.TimeY:
                    return "time.y";
                case (int)VertexData.TimeZ:
                    return "time.z";
                case (int)VertexData.TimeW:
                    return "time.w";
                case (int)VertexData.MinusTimeX:
                    return "-time.x";
                case (int)VertexData.MinusTimeY:
                    return "-time.y";
                case (int)VertexData.MinusTimeZ:
                    return "-time.z";
                case (int)VertexData.MinusTimeW:
                    return "-time.w";
                case (int)VertexData.TimeMapX:
                    return "timeMap.x";
                case (int)VertexData.TimeMapY:
                    return "timeMap.y";
                case (int)VertexData.TimeMapZ:
                    return "timeMap.z";
                case (int)VertexData.TimeMapW:
                    return "timeMap.w";
                default:
                    return prop;
            }
        }
    }
    

}
