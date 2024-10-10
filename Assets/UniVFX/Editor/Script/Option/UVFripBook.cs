using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;


namespace UniVFX.Editor
{
    public class UVFripBook : UniVFXOption
    {
        const string _IsActive = "_FRIPBOOK";
        const string _Row = "_FripBookRow";
        const string _Column = "_FripBookColumn";
        const string _Index = "_FripBookIndex";
        const string _TargetMainTex = "_MainUVFripbook";
        const string _TargetBlendTex = "_BlendUVFripbook";
        const string _TargetDissolveTex = "_DissolveUVFripbook";
        const string _TargetDistortionTex = "_DistortionUVFripbook";
        const string _TargetVertexAnim = "_VertexAnimUVFripbook";
        const string _TargetMask = "_MaskUVFripbook";

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
                    _viewGUI = EditorGUILayout.Foldout(_viewGUI, "UV FripBook");
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
                                    var row = UniVFXGUILayout.IntSlider(ref _mat, _Row, "Row", 1, 8);
                                    var column = UniVFXGUILayout.IntSlider(ref _mat, _Column, "Column", 1, 8);
                                    UniVFXGUILayout.OptionIntSlider(ref _mat, _Index, "Index", 0, row * column - 1);
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
            useCustomDataList[(int)_mat.GetInt(_Index + "_Data")].Add("FripBook Row");
        }

        public override void CollectCustomColorData(ref List<List<string>> useCustomDataList)
        {

        }


    }

}
