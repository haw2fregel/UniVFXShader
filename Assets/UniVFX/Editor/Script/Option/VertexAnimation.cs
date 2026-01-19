using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;


namespace UniVFX.Editor
{
    public class VertexAnimation : UniVFXOption
    {
        const string _IsActive = "_VERTEXANIMATION";
        const string _Tex = "_VertexAnimTex";
        const string _UV = "_VertexAnimUV";
        const string _Param = "_VertexAnimParam";

        public VertexAnimation(bool isCanvas, bool isBRP) : base(isCanvas, isBRP)
        {
        }

        public override bool IsActive()
        {
            return _mat.GetInt(_IsActive) == 1;
        }

        public static bool IsActive(Material mat)
        {
            return mat.GetInt(_IsActive) == 1;
        }

        public override void SetActive(bool active)
        {
            _mat.SetInt(_IsActive, active ? 1 : 0);
        }

        public static void SetActive(Material mat, bool active)
        {
            mat.SetInt(_IsActive, active ? 1 : 0);
        }
        public override int HeatValue()
        {
            return 0;
        }

        public override void GetHeatValue(ref int value, ref int max)
        {
            max += HeatValue();
            if(IsActive())
                value += HeatValue();
        }

        public override void OptionGUI()
        {
            GUI.color = new Color(3f, 3f, 3f, 1.0f);
            if (IsActive())
                GUI.color = new Color(5f, 5f, 5f, 1.0f);
            using (new EditorGUILayout.VerticalScope("Box"))
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    GUI.color = new Color(1f, 1f, 1f, 1.0f);
                    _viewGUI = EditorGUILayout.Foldout(_viewGUI, "Vertex Animation");
                    if (_viewGUI)
                    {
                        var active = UniVFXGUILayout.OptionBoolField(ref _mat, _IsActive, "Active");
                        if (active)
                        {
                            GUI.color = new Color(0f, 0f, 0f, 0.6f);
                            using (new EditorGUILayout.VerticalScope("Box"))
                            {
                                using (new EditorGUI.IndentLevelScope())
                                {
                                    GUI.color = new Color(1f, 1f, 1f, 1f);
                                    UniVFXGUILayout.OptionTextureField(ref _mat, _Tex, "Texture");
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Param, "X", 0, -1, 1);
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Param, "Y", 1, -1, 1);
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Param, "Z", 2, -1, 1);
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Param, "Intensity", 3, -1, 1);
                                    UniVFXGUILayout.UVGUILayoutVert(ref _mat, ref _viewUVGUI, _UV, _isCanvas);
                                }
                            }
                        }
                    }
                }
            }
            GUI.color = new Color(1f, 1f, 1f, 1f);
        }

        public override void CollectCustomData(ref List<List<string>> useCustomDataList)
        {
            if (!IsActive())
                return;
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").x].Add("VertexAnim Tile X");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").y].Add("VertexAnim Tile Y");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").z].Add("VertexAnim Offset X");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").w].Add("VertexAnim Offset Y");
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").x].Add("VertexAnim X");
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").y].Add("VertexAnim Y");
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").z].Add("VertexAnim Z");
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").w].Add("VertexAnim Intensity");

        }

        public override void CollectCustomColorData(ref List<List<string>> useCustomDataList)
        {

        }

        public override void CollectUVChannel(ref List<List<string>> useUVChannelList)
        {
            if (!IsActive())
                return;
            useUVChannelList[_mat.GetInt(_UV + "Transform_Index")].Add(_Tex);
        }


        public override void VaridateCustomData()
        {
            UniVFXGUILayout.VaridateCustomDataVector(ref _mat, _UV + "Transform", _isCanvas);
            UniVFXGUILayout.VaridateArrayIndex(ref _mat, _UV + "Transform_Index", UniVFXGUILayout._UVChannelOptionVert);
            UniVFXGUILayout.VaridateCustomDataVector(ref _mat, _Param, _isCanvas);
        }

        public override List<string> GetPropertyCode()
        {
            var code = new List<string>();
            if (!IsActive())
                return code;
            
            code.Add("[NoScaleOffset]" + _Tex + "(\"" + _Tex.Replace("_", "") + "\", 2D) = \"white\" {}");
            return code;
        }
        public override List<string> GetCBufferCode()
        {
            var code = new List<string>();
            if (!IsActive())
                return code;
            return code;
        }
        public override List<string> GetTextureCode()
        {
            var code = new List<string>();
            if (!IsActive())
                return code;
            
            code.Add("TEXTURE2D(" + _Tex + ");");
            return code;
        }
        public override List<string> GetUseV2fCode()
        {
            var code = new List<string>();
            return code;
        }
        public override List<string> GetVertexHeadCode()
        {
            var code = new List<string>();
            return code;
        }
        public override List<string> GetVertexCode()
        {
            var code = new List<string>();
            if (!IsActive())
                return code;
            
            var sampler = UniVFXGUILayout.GetSamplerName(_mat.GetInt(_UV + "Transform_Sampler"));

            var uvName = UniVFXGUILayout.GetVertUVName(_mat.GetInt(_UV + "Transform_Index"), _mat);
            var transform = "st_" + _Tex.Replace("_", "");
            var transformX = UniVFXGUILayout.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").x, _mat.GetVector(_UV + "Transform").x.ToString(), _isCanvas);
            var transformY = UniVFXGUILayout.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").y, _mat.GetVector(_UV + "Transform").y.ToString(), _isCanvas);
            var transformZ = UniVFXGUILayout.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").z, _mat.GetVector(_UV + "Transform").z.ToString(), _isCanvas);
            var transformW = UniVFXGUILayout.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").w, _mat.GetVector(_UV + "Transform").w.ToString(), _isCanvas);

            var param = "param_" + _Tex.Replace("_", "");
            var paramX = UniVFXGUILayout.VertexDataToCode((int)_mat.GetVector(_Param + "_Data").x, _mat.GetVector(_Param).x.ToString(), _isCanvas);
            var paramY = UniVFXGUILayout.VertexDataToCode((int)_mat.GetVector(_Param + "_Data").y, _mat.GetVector(_Param).y.ToString(), _isCanvas);
            var paramZ = UniVFXGUILayout.VertexDataToCode((int)_mat.GetVector(_Param + "_Data").z, _mat.GetVector(_Param).z.ToString(), _isCanvas);
            var paramW = UniVFXGUILayout.VertexDataToCode((int)_mat.GetVector(_Param + "_Data").w, _mat.GetVector(_Param).w.ToString(), _isCanvas);

            var uv = "uv_" + _Tex.Replace("_", "");
            var tex = "tex_" + _Tex.Replace("_", "");

            code.Add("//VertexAnimation");
            code.Add("float4 " + param + " = float4(" + paramX + ", " + paramY + ", " + paramZ + ", " + paramW + ");");
            code.Add("float4 " + transform + " = float4(" + transformX + ", " + transformY + ", " + transformZ + ", " + transformW + ");");
            code.Add("float2 " + uv + " = (" + uvName + " - float2(0.5, 0.5)) * " + transform + ".xy + float2(0.5, 0.5) + " + transform + ".zw;");
            code.Add("half4 " + tex + " = SAMPLE_TEXTURE2D_LOD(" + _Tex + ", " + sampler + ", " + uv + ", 0);");
            code.Add("float3 biNormal = cross(normalize(v.normal), normalize(v.tangent.xyz));");
            code.Add("float3 normalTangent = " + tex + ".xyz * 2.0 - 1.0;");
            code.Add("normalTangent *= " + param + ".xyz * " + param + ".w;");
            code.Add("v.vertex.xyz += v.tangent * normalTangent.x + biNormal * normalTangent.y + v.normal * normalTangent.z;");
            code.Add("");

            return code;
        }
        
        public override List<string> GetFragmentHeadCode()
        {
            var code = new List<string>();
            return code;
        }
        public override List<string> GetFragmentCode()
        {
            var code = new List<string>();
            return code;
        }


    }

}
