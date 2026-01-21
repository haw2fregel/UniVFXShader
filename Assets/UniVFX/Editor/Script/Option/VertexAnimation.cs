using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;


namespace UniVFX.Editor
{
    public class VertexAnimation : UniVFXOption
    {
        static bool _viewGUI = false;
        static bool _viewUVGUI = false;
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

        public override List<string> GetPropertyCode(RefactOption refactOption)
        {
            var code = new List<string>();
            if (!IsActive())
                return code;

            var paramX = UniVFXGUILayout.VertexDataToVectorCode(_mat, _Param, 0, _isCanvas, refactOption);
            var paramY = UniVFXGUILayout.VertexDataToVectorCode(_mat, _Param, 1, _isCanvas, refactOption);
            var paramZ = UniVFXGUILayout.VertexDataToVectorCode(_mat, _Param, 2, _isCanvas, refactOption);
            var paramW = UniVFXGUILayout.VertexDataToVectorCode(_mat, _Param, 3, _isCanvas, refactOption);
            var isIntensityLess = (paramX == "0" && paramY == "0" && paramZ == "0") || paramW == "0";
            var isInvalid = isIntensityLess && refactOption.HasFlag(RefactOption.PropertiesToFixedValue);
            var isFixedValue = refactOption.HasFlag(RefactOption.PropertiesToFixedValue);
            var isTextureNone = _mat.GetTexture(_Tex) == null && refactOption.HasFlag(RefactOption.NoneTextureToFixedValue);

            if(isInvalid)
            {
                code.Add("//VertexAnimation Skipped, Intensity is 0");
                return code;
            }

            if (isTextureNone)
            {
                code.Add("//VertexAnimation Skipped, Texture is null");
            }else
            {
                code.Add("[NoScaleOffset]" + _Tex + "(\"" + _Tex.Replace("_", "") + "\", 2D) = \"grey\" {}");
            }

            if(isFixedValue)
            {
                code.Add("//VertexAnimation Property Skipped, FixedValue");
            }
            else
            {
                var paramData = _mat.GetVector(_Param + "_Data");
                if(paramData.x == 0 || paramData.y == 0 || paramData.z == 0 || paramData.w == 0)
                    code.Add(_Param + "(\"" + _Param.Replace("_", "") + "\", Vector) = (1,0,0,0)");
                
                if (isTextureNone)
                {
                    code.Add("//VertexAnimation Transform Skipped, Texture is null");
                }
                else
                {
                    var transformData = _mat.GetVector(_UV + "Transform_Data");
                    if(transformData.x == 0 || transformData.y == 0 || transformData.z == 0 || transformData.w == 0)
                        code.Add(_UV + "Transform(\"" + _UV.Replace("_", "") + "Transform\", Vector) = (0,0,1,1)");
                }
            }

            return code;
        }
        public override List<string> GetCBufferCode(RefactOption refactOption)
        {
            var code = new List<string>();
            if (!IsActive())
                return code;

            var paramX = UniVFXGUILayout.VertexDataToVectorCode(_mat, _Param, 0, _isCanvas, refactOption);
            var paramY = UniVFXGUILayout.VertexDataToVectorCode(_mat, _Param, 1, _isCanvas, refactOption);
            var paramZ = UniVFXGUILayout.VertexDataToVectorCode(_mat, _Param, 2, _isCanvas, refactOption);
            var paramW = UniVFXGUILayout.VertexDataToVectorCode(_mat, _Param, 3, _isCanvas, refactOption);
            var isIntensityLess = (paramX == "0" && paramY == "0" && paramZ == "0") || paramW == "0";
            var isInvalid = isIntensityLess && refactOption.HasFlag(RefactOption.PropertiesToFixedValue);
            var isFixedValue = refactOption.HasFlag(RefactOption.PropertiesToFixedValue);
            var isTextureNone = _mat.GetTexture(_Tex) == null && refactOption.HasFlag(RefactOption.NoneTextureToFixedValue);

            if(isInvalid)
            {
                code.Add("//VertexAnimation Skipped, Intensity is 0");
                return code;
            }

            if(isFixedValue)
            {
                code.Add("//VertexAnimation Property Skipped, FixedValue");
            }
            else
            {
                var paramData = _mat.GetVector(_Param + "_Data");
                if(paramData.x == 0 || paramData.y == 0 || paramData.z == 0 || paramData.w == 0)
                    code.Add("half4 " + _Param + ";");
                
                if (isTextureNone)
                {
                    code.Add("//VertexAnimation Transform Skipped, Texture is null");
                }
                else
                {
                    var transformData = _mat.GetVector(_UV + "Transform_Data");
                    if(transformData.x == 0 || transformData.y == 0 || transformData.z == 0 || transformData.w == 0)
                        code.Add("float4 " + _UV + "Transform;");
                }
            }

            return code;
        }
        public override List<string> GetTextureCode(RefactOption refactOption)
        {
            var code = new List<string>();
            if (!IsActive())
                return code;

            var paramX = UniVFXGUILayout.VertexDataToVectorCode(_mat, _Param, 0, _isCanvas, refactOption);
            var paramY = UniVFXGUILayout.VertexDataToVectorCode(_mat, _Param, 1, _isCanvas, refactOption);
            var paramZ = UniVFXGUILayout.VertexDataToVectorCode(_mat, _Param, 2, _isCanvas, refactOption);
            var paramW = UniVFXGUILayout.VertexDataToVectorCode(_mat, _Param, 3, _isCanvas, refactOption);
            var isIntensityLess = (paramX == "0" && paramY == "0" && paramZ == "0") || paramW == "0";
            var isInvalid = isIntensityLess && refactOption.HasFlag(RefactOption.PropertiesToFixedValue);
            var isTextureNone = _mat.GetTexture(_Tex) == null && refactOption.HasFlag(RefactOption.NoneTextureToFixedValue);

            if(isInvalid)
            {
                code.Add("//VertexAnimation Skipped, Intensity is 0");
                return code;
            }

            if (isTextureNone)
            {
                code.Add("//VertexAnimationTex Skipped, Texture is null");
            }
            else
            {
                code.Add("TEXTURE2D(" + _Tex + ");");
            }
            
            
            return code;
        }
        public override List<string> GetUseV2fCode(RefactOption refactOption)
        {
            var code = new List<string>();
            return code;
        }
        public override List<string> GetVertexHeadCode(RefactOption refactOption)
        {
            var code = new List<string>();
            return code;
        }
        public override List<string> GetVertexCode(RefactOption refactOption)
        {
            var code = new List<string>();
            if (!IsActive())
                return code;

            var paramX = UniVFXGUILayout.VertexDataToVectorCode(_mat, _Param, 0, _isCanvas, refactOption);
            var paramY = UniVFXGUILayout.VertexDataToVectorCode(_mat, _Param, 1, _isCanvas, refactOption);
            var paramZ = UniVFXGUILayout.VertexDataToVectorCode(_mat, _Param, 2, _isCanvas, refactOption);
            var paramW = UniVFXGUILayout.VertexDataToVectorCode(_mat, _Param, 3, _isCanvas, refactOption);
            var isIntensityLess = (paramX == "0" && paramY == "0" && paramZ == "0") || paramW == "0";
            var isInvalid = isIntensityLess && refactOption.HasFlag(RefactOption.PropertiesToFixedValue);
            var isTextureNone = _mat.GetTexture(_Tex) == null && refactOption.HasFlag(RefactOption.NoneTextureToFixedValue);
            
            var sampler = UniVFXGUILayout.GetSamplerName(_mat.GetInt(_UV + "Transform_Sampler"));

            var uvName = UniVFXGUILayout.GetVertUVName(_mat.GetInt(_UV + "Transform_Index"), _mat);
            var transform = "st_" + _Tex.Replace("_", "");
            var transformX = UniVFXGUILayout.VertexDataToVectorCode(_mat, _UV + "Transform", 0, _isCanvas, refactOption);
            var transformY = UniVFXGUILayout.VertexDataToVectorCode(_mat, _UV + "Transform", 1, _isCanvas, refactOption);
            var transformZ = UniVFXGUILayout.VertexDataToVectorCode(_mat, _UV + "Transform", 2, _isCanvas, refactOption);
            var transformW = UniVFXGUILayout.VertexDataToVectorCode(_mat, _UV + "Transform", 3, _isCanvas, refactOption);

            var param = "param_" + _Tex.Replace("_", "");
            var uv = "uv_" + _Tex.Replace("_", "");
            var tex = "tex_" + _Tex.Replace("_", "");

            code.Add("//VertexAnimation");
            if(isInvalid)
            {
                code.Add("//VertexAnimation Skipped, Intensity is 0");
                return code;
            }

            if (isTextureNone)
            {
                code.Add("float4 " + tex + " = float4(0.5,0.5,0.5,0.5);");
                code.Add("//VertexAnimationTex Skipped, Texture is null");
            }
            else
            {
                code.Add("float2 " + uv + " = " + uvName + ";");
                if(transformX != "1" || transformY != "1" || transformZ != "0" || transformW != "0")
                {
                    code.Add("float4 " + transform + " = float4(" + transformX + ", " + transformY + ", " + transformZ + ", " + transformW + ");");
                    code.Add(uv + " = (" + uv + " - float2(0.5, 0.5)) * " + transform + ".xy + float2(0.5, 0.5) + " + transform + ".zw;");
                }
                code.Add("half4 " + tex + " = SAMPLE_TEXTURE2D_LOD(" + _Tex + ", " + sampler + ", " + uv + ", 0);");
            }
            code.Add("float3 biNormal = cross(normalize(v.normal), normalize(v.tangent.xyz));");
            code.Add("float3 normalTangent = " + tex + ".xyz * 2.0 - 1.0;");
            code.Add("float4 " + param + " = float4(" + paramX + ", " + paramY + ", " + paramZ + ", " + paramW + ");");
            code.Add("normalTangent *= " + param + ".xyz * " + param + ".w;");
            code.Add("v.vertex.xyz += v.tangent.xyz * normalTangent.x + biNormal.xyz * normalTangent.y + v.normal.xyz * normalTangent.z;");
            code.Add("");

            return code;
        }
        
        public override List<string> GetFragmentHeadCode(RefactOption refactOption)
        {
            var code = new List<string>();
            return code;
        }
        public override List<string> GetFragmentCode(RefactOption refactOption)
        {
            var code = new List<string>();
            return code;
        }


    }

}
