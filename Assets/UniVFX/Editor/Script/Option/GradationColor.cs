using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


namespace UniVFX.Editor
{
    public class GradationColor : UniVFXOption
    {
        protected const string _IsActive = "_GRADATIONCOLOR";
        protected const string _Color00 = "_GradationColor00";
        protected const string _Color01 = "_GradationColor01";
        protected const string _Color10 = "_GradationColor10";
        protected const string _Color11 = "_GradationColor11";
        protected const string _UV = "_GradationUV";
        protected const string _BlendMode = "_GradationBlendMode";
        protected readonly static string[] _BlendModeOption = { "Overwrite", "Add", "Multiply", "Subtract", "Overlay" };

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
            return 25;
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
                    _viewGUI = EditorGUILayout.Foldout(_viewGUI, "Gradation");
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
                                    UniVFXGUILayout.OptionColorField(ref _mat, _Color00, "Color00");
                                    UniVFXGUILayout.OptionColorField(ref _mat, _Color01, "Color01");
                                    UniVFXGUILayout.OptionColorField(ref _mat, _Color10, "Color10");
                                    UniVFXGUILayout.OptionColorField(ref _mat, _Color11, "Color11");
                                    UniVFXGUILayout.OptionPopupField(ref _mat, _BlendMode, "Blend Mode", _BlendModeOption);
                                    UniVFXGUILayout.UVGUILayoutClamp(ref _mat, ref _viewUVGUI, _UV);
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
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").x].Add("GradationUV Offset X");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").y].Add("GradationUV Offset Y");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").z].Add("GradationUV Tile X");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").w].Add("GradationUV Tile Y");
        }

        public override void CollectCustomColorData(ref List<List<string>> useCustomDataList)
        {
            useCustomDataList[_mat.GetInt(_Color00 + "_Data")].Add("Gradation Color00");
            useCustomDataList[_mat.GetInt(_Color01 + "_Data")].Add("Gradation Color01");
            useCustomDataList[_mat.GetInt(_Color10 + "_Data")].Add("Gradation Color10");
            useCustomDataList[_mat.GetInt(_Color11 + "_Data")].Add("Gradation Color11");
        }

    }

}
