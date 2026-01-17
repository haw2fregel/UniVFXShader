namespace UniVFX.Editor
{
    public enum VertexColorData
    {
        Input,
        TEXCOORD1,
        TEXCOORD2,
        VertexColor
    }
    public class VertexColorDataConvert
    {
        public static string VertexColorDataToCode(int vertexColorData, string prop)
        {
            switch (vertexColorData)
            {
                case (int)VertexColorData.Input:
                    return prop;
                case (int)VertexColorData.TEXCOORD1:
                    return "texCoord1";
                case (int)VertexColorData.TEXCOORD2:
                    return "texCoord2";
                case (int)VertexColorData.VertexColor:
                    return "vertexColor";
                default:
                    return prop;
            }
        }
    }
}
