using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


namespace UniVFX.Editor
{
    public class Dissolve : UniVFXOption
    {
        protected const string _IsActive = "_DISSOLVE";
        protected const string _Tex = "_DissolveTex";
        protected const string _UV = "_DissolveUV";
        protected const string _Color = "_DissolveColor";
        protected const string _Param = "_DissolveParam";

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
            return 31;
        }

        public override void GetHeatValue(ref int value, ref int max)
        {
            max += HeatValue();
            if (IsActive())
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
                    _viewGUI = EditorGUILayout.Foldout(_viewGUI, "Dissolve");
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
                                    UniVFXGUILayout.OptionTextureField(ref _mat, _Tex, "Texture");
                                    UniVFXGUILayout.OptionColorField(ref _mat, _Color, "Color");
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Param, "Alpha", 0, 0, 1);
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Param, "Smooth", 1, 0, 1);
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Param, "Emissive Width", 2, 0, 1);
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Param, "Emissive Smooth", 3, 0, 1);
                                    UniVFXGUILayout.UVGUILayout(ref _mat, ref _viewUVGUI, _UV);
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
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").x].Add("DissolveUV Tile X");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").y].Add("DissolveUV Tile Y");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").z].Add("DissolveUV Offset X");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").w].Add("DissolveUV Offset Y");
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").x].Add("Disslve Alpha");
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").y].Add("Disslve Smooth");
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").z].Add("Disslve Emissive Width");
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").w].Add("Disslve Emissive Smooth");
        }

        public override void CollectCustomColorData(ref List<List<string>> useCustomDataList)
        {
            useCustomDataList[_mat.GetInt(_Color + "_Data")].Add("Dissolve Color");
        }

        public override void VaridateCustomData()
        {
            UniVFXGUILayout.VaridateCustomDataVector(ref _mat, _UV + "Transform");
            UniVFXGUILayout.VaridateCustomDataVector(ref _mat, _Param);
            UniVFXGUILayout.VaridateArrayIndex(ref _mat, _UV + "Transform_Index", UniVFXGUILayout._UVChannelOption);
            UniVFXGUILayout.VaridateCustomColorDataInt(ref _mat, _Color);
        }


    }
}
