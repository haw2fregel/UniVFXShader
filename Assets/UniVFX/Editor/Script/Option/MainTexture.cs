using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


namespace UniVFX.Editor
{
    public class MainTexture : UniVFXOption
    {
        const string _Tex = "_MainTex";
        const string _UV = "_MainUV";
        const string _Color = "_MainColor";
        const string _ColorMultiple = "_MainColorMultiple";
        const string _AlphaMultiple = "_MainAlphaMultiple";

        public override bool IsActive()
        {
            return true;
        }

        public override void SetActive(bool active)
        {
        }

        public override int HeatValue()
        {
            return 6;
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
                    _viewGUI = EditorGUILayout.Foldout(_viewGUI, "Main Texture");
                    if (_viewGUI)
                    {
                        GUI.color = new Color(0f, 0f, 0f, 0.6f);
                        using (new EditorGUILayout.VerticalScope("Box"))
                        {
                            using (new EditorGUI.IndentLevelScope())
                            {
                                GUI.color = new Color(1f, 1f, 1f, 1f);
                                UniVFXGUILayout.OptionTextureField(ref _mat, _Tex, "Texture");
                                UniVFXGUILayout.OptionColorField(ref _mat, _Color, "Color");
                                UniVFXGUILayout.OptionBoolField(ref _mat, _ColorMultiple, "Color Multiple");
                                UniVFXGUILayout.OptionBoolField(ref _mat, _AlphaMultiple, "Alpha Multiple");
                                UniVFXGUILayout.UVGUILayout(ref _mat, ref _viewUVGUI, _UV);
                            }
                        }
                    }
                }
            }
            GUI.color = new Color(1f, 1f, 1f, 1f);
        }

        public override void CollectCustomData(ref List<List<string>> useCustomDataList)
        {
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").x].Add("MainUV Offset X");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").y].Add("MainUV Offset Y");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").z].Add("MainUV Tile X");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").w].Add("MainUV Tile Y");
        }

        public override void CollectCustomColorData(ref List<List<string>> useCustomDataList)
        {
            useCustomDataList[_mat.GetInt(_Color + "_Data")].Add("Main Color");
        }

    }

}
