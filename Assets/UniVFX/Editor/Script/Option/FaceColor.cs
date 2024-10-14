using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


namespace UniVFX.Editor
{
    public class FaceColor : UniVFXOption
    {
        const string _IsActive = "_FACECOLOR";
        const string _FrontColor = "_FrontFaceColor";
        const string _BackColor = "_BackFaceColor";

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
                                    UniVFXGUILayout.OptionColorField(ref _mat, _FrontColor, "FrontFace Color");
                                    UniVFXGUILayout.OptionColorField(ref _mat, _BackColor, "BackFace Color");
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
            useCustomDataList[_mat.GetInt(_FrontColor + "_Data")].Add("FrontFace Color");
            useCustomDataList[_mat.GetInt(_BackColor + "_Data")].Add("BackFace Color");
        }

        public override void VaridateCustomData()
        {
            UniVFXGUILayout.VaridateCustomColorDataInt(ref _mat, _FrontColor);
            UniVFXGUILayout.VaridateCustomColorDataInt(ref _mat, _BackColor);
        }

    }

}
