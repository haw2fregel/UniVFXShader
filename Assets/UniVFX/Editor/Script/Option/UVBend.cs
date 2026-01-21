using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


namespace UniVFX.Editor
{
    public class UVBend : UniVFXOption
    {
        static bool _viewGUI = false;
        public const string _IsActive = "_UVBEND";
        public const string _Polar = "_UVPolar";
        public const string _Param = "_UVBendParam";

        public UVBend(bool isCanvas, bool isBRP) : base(isCanvas, isBRP)
        {
        }


        public override bool IsActive()
        {
            return _mat.IsKeywordEnabled(_IsActive);
        }

        public static bool IsActive(Material mat)
        {
            return mat.IsKeywordEnabled(_IsActive);
        }

        public override void SetActive(bool active)
        {
            if (active)
            {
                if(!_mat.IsKeywordEnabled(_IsActive)) 
                    _mat.EnableKeyword(_IsActive);
            }else{
                if(_mat.IsKeywordEnabled(_IsActive)) 
                    _mat.DisableKeyword(_IsActive);
            }
        }

        public static void SetActive(Material mat, bool active)
        {
            if (active)
            {
                if(!mat.IsKeywordEnabled(_IsActive)) 
                    mat.EnableKeyword(_IsActive);
            }else{
                if(mat.IsKeywordEnabled(_IsActive)) 
                    mat.DisableKeyword(_IsActive);
            }
        }

        public override int HeatValue()
        {
            return 50;
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
                    _viewGUI = EditorGUILayout.Foldout(_viewGUI, "UV Bend");
                    if (_viewGUI)
                    {
                        var active = UniVFXGUILayout.OptionKeywordField(ref _mat, _IsActive, "Active");
                        if (active)
                        {
                            GUI.color = new Color(0f, 0f, 0f, 0.6f);
                            using (new EditorGUILayout.VerticalScope("Box"))
                            {
                                using (new EditorGUI.IndentLevelScope())
                                {
                                    GUI.color = new Color(1f, 1f, 1f, 1f);
                                    UniVFXGUILayout.OptionBoolField(ref _mat, _Polar, "Polar");
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Param, "Center X", 0, -1, 1, _isCanvas);
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Param, "Center Y", 1, -1, 1, _isCanvas);
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Param, "Bend X", 2, -1, 1, _isCanvas);
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Param, "Bend Y", 3, -1, 1, _isCanvas);
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
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").x].Add("Bend Center X");
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").y].Add("Bend Center Y");
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").z].Add("Bend X");
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").w].Add("Bend Y");
        }

        public override void CollectCustomColorData(ref List<List<string>> useCustomDataList)
        {

        }

        public override void CollectUVChannel(ref List<List<string>> useUVChannelList)
        {
        }

        public override void VaridateCustomData()
        {
            UniVFXGUILayout.VaridateCustomDataVector(ref _mat, _Param, _isCanvas);
        }

        public override List<string> GetPropertyCode(RefactOption refactOption)
        {
            var code = new List<string>();
            if (!IsActive())
                return code;

            var paramZ = UniVFXGUILayout.VertexDataToVectorCode(_mat, _Param, 2, _isCanvas, refactOption);
            var paramW = UniVFXGUILayout.VertexDataToVectorCode(_mat, _Param, 3, _isCanvas, refactOption);
            var isInvalid = paramZ == "0" && paramW == "0" && refactOption.HasFlag(RefactOption.PropertiesToFixedValue);
            var isFixedValue = refactOption.HasFlag(RefactOption.PropertiesToFixedValue);

            if(isInvalid)
            {
                code.Add("//UVBend Skipped, Intensity is 0");
                return code;
            }

            if(isFixedValue)
            {
                code.Add("//UVBend Property Skipped, FixedValue");
            }
            else
            {
                var paramData = _mat.GetVector(_Param + "_Data");
                if(paramData.x == 0 || paramData.y == 0 || paramData.z == 0 || paramData.w == 0)
                    code.Add(_Param + "(\"" + _Param.Replace("_", "") + "\", Vector) = (1,0,0,0)");
            }
            
            return code;
        }
        public override List<string> GetCBufferCode(RefactOption refactOption)
        {
            var code = new List<string>();
            if (!IsActive())
                return code;

            var paramZ = UniVFXGUILayout.VertexDataToVectorCode(_mat, _Param, 2, _isCanvas, refactOption);
            var paramW = UniVFXGUILayout.VertexDataToVectorCode(_mat, _Param, 3, _isCanvas, refactOption);
            var isInvalid = paramZ == "0" && paramW == "0" && refactOption.HasFlag(RefactOption.PropertiesToFixedValue);
            var isFixedValue = refactOption.HasFlag(RefactOption.PropertiesToFixedValue);
            
            if(isInvalid)
            {
                code.Add("//UVBend Skipped, Intensity is 0");
                return code;
            }

            if(isFixedValue)
            {
                code.Add("//UVBend Property Skipped, FixedValue");
            }
            else
            {
                var paramData = _mat.GetVector(_Param + "_Data");
                if(paramData.x == 0 || paramData.y == 0 || paramData.z == 0 || paramData.w == 0)
                    code.Add("half4 " + _Param + ";");
            }

            return code;
        }
        public override List<string> GetTextureCode(RefactOption refactOption)
        {
            var code = new List<string>();
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

        public static void ApplyBendCode(ref List<string> code, Material mat, string uvName, bool isCanvas, RefactOption refactOption)
        {

            var paramX = UniVFXGUILayout.VertexDataToVectorCode(mat, _Param, 0, isCanvas, refactOption);
            var paramY = UniVFXGUILayout.VertexDataToVectorCode(mat, _Param, 1, isCanvas, refactOption);
            var paramZ = UniVFXGUILayout.VertexDataToVectorCode(mat, _Param, 2, isCanvas, refactOption);
            var paramW = UniVFXGUILayout.VertexDataToVectorCode(mat, _Param, 3, isCanvas, refactOption);
            var isInvalid = paramZ == "0" && paramW == "0" && refactOption.HasFlag(RefactOption.PropertiesToFixedValue);

            if (isInvalid)
                return ;

            code.Add(uvName + " += float2(abs(" + uvName + ".y - " + paramX + ") * " + paramZ + ", abs(" + uvName + ".x - " + paramY + ") * " + paramW + ");");
            code.Add("");
        }

        public static void ApplyBendPolarCode(ref List<string> code, string uvName)
        {
            code.Add("float2 " + uvName + "_polarDelta = " + uvName + " - float2(0.5, 0.5);");
            code.Add("float " + uvName + "_polarRadius = length(" + uvName + "_polarDelta) * 2;");
            code.Add("float " + uvName + "_polarAngle = (atan2(" + uvName + "_polarDelta.y, " + uvName + "_polarDelta.x) + 3.14) / 6.28;");
            code.Add(uvName + " = float2(" + uvName + "_polarRadius, " + uvName + "_polarAngle);");
            code.Add("");
        }


    }
}
