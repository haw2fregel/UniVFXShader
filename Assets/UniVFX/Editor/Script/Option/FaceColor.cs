using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


namespace UniVFX.Editor
{
    public class FaceColor : UniVFXOption
    {
        static bool _viewGUI = false;
        const string _IsActive = "_FACECOLOR";
        const string _FrontColor = "_FrontFaceColor";
        const string _BackColor = "_BackFaceColor";

        public FaceColor(bool isCanvas, bool isBRP) : base(isCanvas, isBRP)
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
            return 11;
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
                    _viewGUI = EditorGUILayout.Foldout(_viewGUI, "Face Color");
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
                                    UniVFXGUILayout.OptionColorField(ref _mat, _FrontColor, "FrontFace Color", _isCanvas);
                                    UniVFXGUILayout.OptionColorField(ref _mat, _BackColor, "BackFace Color", _isCanvas);
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

        }

        public override void CollectCustomColorData(ref List<List<string>> useCustomDataList)
        {
            if (!IsActive())
                return;
            useCustomDataList[_mat.GetInt(_FrontColor + "_Data")].Add("FrontFace Color");
            useCustomDataList[_mat.GetInt(_BackColor + "_Data")].Add("BackFace Color");
        }

        public override void CollectUVChannel(ref List<List<string>> useUVChannelList)
        {
        }

        public override void VaridateCustomData()
        {
            UniVFXGUILayout.VaridateCustomColorDataInt(ref _mat, _FrontColor, _isCanvas);
            UniVFXGUILayout.VaridateCustomColorDataInt(ref _mat, _BackColor, _isCanvas);
        }

        public override List<string> GetPropertyCode(RefactOption refactOption)
        {
            var code = new List<string>();
            if (!IsActive())
                return code;
            var isFixedValue = refactOption.HasFlag(RefactOption.PropertiesToFixedValue);

            if(isFixedValue)
            {
                code.Add("//FaceColor Property Skipped, FixedValue");
            }
            else
            {
                if (_mat.GetInt(_FrontColor + "_Data") == 0)
                    code.Add(_FrontColor + "(\"" + _FrontColor.Replace("_", "") + "\", Color) = (1,1,1,1)");
                if (_mat.GetInt(_BackColor + "_Data") == 0)
                    code.Add(_BackColor + "(\"" + _BackColor.Replace("_", "") + "\", Color) = (1,1,1,1)");
            }
            return code;
        }
        public override List<string> GetCBufferCode(RefactOption refactOption)
        {
            var code = new List<string>();
            if (!IsActive())
                return code;

            var isFixedValue = refactOption.HasFlag(RefactOption.PropertiesToFixedValue);

            if(isFixedValue)
            {
                code.Add("//FaceColor Property Skipped, FixedValue");
            }
            else
            {
                if (_mat.GetInt(_FrontColor + "_Data") == 0)
                    code.Add("half4 " + _FrontColor + ";");
                if (_mat.GetInt(_BackColor + "_Data") == 0)
                    code.Add("half4 " + _BackColor + ";");
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
            if (!IsActive())
                return code;

            var frontColor = UniVFXGUILayout.VertexDataToColorCode(_mat, _FrontColor, true, _isCanvas, refactOption);
            var backColor = UniVFXGUILayout.VertexDataToColorCode(_mat, _BackColor, true, _isCanvas, refactOption);

            code.Add("col *= max(0, face) ? " + frontColor + " : " + backColor + ";");
            code.Add("");
            return code;          
        }

    }

}
