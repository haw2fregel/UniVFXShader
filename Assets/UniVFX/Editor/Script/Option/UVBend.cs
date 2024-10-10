using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


namespace UniVFX.Editor
{
    public class UVBend : UniVFXOption
    {
        const string _IsActive = "_UVBEND";
        const string _Polar = "_UVPolar";
        const string _Param = "_UVBendParam";

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
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Param, "Center X", 0, -1, 1);
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Param, "Center Y", 1, -1, 1);
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Param, "Bend X", 2, -1, 1);
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Param, "Bend Y", 3, -1, 1);
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
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").x].Add("Bend Center X");
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").y].Add("Bend Center Y");
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").z].Add("Bend X");
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").w].Add("Bend Y");
        }

        public override void CollectCustomColorData(ref List<List<string>> useCustomDataList)
        {

        }


    }
}
