using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


namespace UniVFX.Editor
{

    public class UVRotate : UniVFXOption
    {
        static bool _viewGUI = false;
        protected const string _IsActive = "_ROTATEUVENABLE";
        protected const string _Rotate = "_UVRotate";

        public UVRotate(bool isCanvas, bool isBRP) : base(isCanvas, isBRP)
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
                    _viewGUI = EditorGUILayout.Foldout(_viewGUI, "UV Rotate");
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
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Rotate, "Rotate", 0, 360, _isCanvas);
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
            useCustomDataList[(int)_mat.GetFloat(_Rotate + "_Data")].Add("Rotate");
        }

        public override void CollectCustomColorData(ref List<List<string>> useCustomDataList)
        {

        }

        public override void CollectUVChannel(ref List<List<string>> useUVChannelList)
        {
        }

        public override void VaridateCustomData()
        {
            if (!IsActive())
                return;
            UniVFXGUILayout.VaridateCustomDataInt(ref _mat, _Rotate, _isCanvas);
        }

        public override List<string> GetPropertyCode(RefactOption refactOption)
        {
            var code = new List<string>();
            if (!IsActive())
                return code;

            var rotate = UniVFXGUILayout.VertexDataToFloatCode(_mat, _Rotate, _isCanvas, refactOption);
            var isInvalid = rotate == "0" && refactOption.HasFlag(RefactOption.PropertiesToFixedValue);
            var isFixedValue = refactOption.HasFlag(RefactOption.PropertiesToFixedValue);

            if(isInvalid)
            {
                code.Add("//Rotate Skipped, Rotate is 0");
                return code;
            }

            if(isFixedValue)
            {
                code.Add("//Rotate Property Skipped, FixedValue");
            }
            else
            {
                if(_mat.GetInt(_Rotate + "_Data") == 0)
                    code.Add(_Rotate + "(\"" + _Rotate.Replace("_", "") + "\", float) = 1");
            }

            return code;
        }
        public override List<string> GetCBufferCode(RefactOption refactOption)
        {
            var code = new List<string>();
            if (!IsActive())
                return code;

            var rotate = UniVFXGUILayout.VertexDataToFloatCode(_mat, _Rotate, _isCanvas, refactOption);
            var isInvalid = rotate == "0" && refactOption.HasFlag(RefactOption.PropertiesToFixedValue);
            var isFixedValue = refactOption.HasFlag(RefactOption.PropertiesToFixedValue);

            if(isInvalid)
            {
                code.Add("//Rotate Skipped, Rotate is 0");
                return code;
            }

            if(isFixedValue)
            {
                code.Add("//Rotate Property Skipped, FixedValue");
            }
            else
            {
                if(_mat.GetInt(_Rotate + "_Data") == 0)
                    code.Add("half " + _Rotate + ";");
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
            if (!IsActive())
                return code;

            var rotate = UniVFXGUILayout.VertexDataToFloatCode(_mat, _Rotate, _isCanvas, refactOption);
            var isInvalid = rotate == "0" && refactOption.HasFlag(RefactOption.PropertiesToFixedValue);
            
            var uv = "rotateUV";

            code.Add("//RotateUV");
            if(isInvalid)
            {
                code.Add("float2 " + uv + " = texCoord0.xy;");
                code.Add("//RotateUV Skipped, Rotate is 0");
                code.Add("");
                return code;
            }
            
            code.Add("float2 " + uv + " = 0;");
            code.Add("float rotateAngle = " + rotate + " * 3.14 / 180 ;");
            code.Add("float rotateCos = cos(rotateAngle);");
            code.Add("float rotateSin = sin(rotateAngle);");
            code.Add(uv + " = (mul(texCoord0.xy - float2(0.5, 0.5), float2x2(rotateCos, -rotateSin, rotateSin, rotateCos)) + float2(0.5, 0.5));");
            code.Add("");
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


    }
}
