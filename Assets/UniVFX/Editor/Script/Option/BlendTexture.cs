using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


namespace UniVFX.Editor
{
    public class BlendTexture : UniVFXOption
    {
        protected const string _IsActive = "_BLENDTEXTURE";
        protected const string _Tex = "_BlendTex";
        protected const string _Color = "_BlendTexColor";
        protected const string _Intensity = "_BlendIntensity";
        protected const string _UV = "_BlendUV";
        protected const string _BlendMode = "_BlendTexBlendMode";
        protected readonly static string[] _BlendModeOption = { "Overwrite", "Add", "Multiply", "Subtract", "Overlay" };

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
                    _viewGUI = EditorGUILayout.Foldout(_viewGUI, "Blend Texture");
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
                                    UniVFXGUILayout.OptionColorField(ref _mat, _Color, "Color");
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Intensity, "Intensity", 0, 1);
                                    UniVFXGUILayout.OptionPopupField(ref _mat, _BlendMode, "Blend Mode", _BlendModeOption);
                                    UniVFXGUILayout.UVGUILayout(ref _mat, ref _viewUVGUI, _UV);
                                }
                            }
                        }
                    }
                }
                GUI.color = new Color(1f, 1f, 1f, 1f);
            }
        }

        public override void CollectCustomData(ref List<List<string>> useCustomDataList)
        {
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").x].Add("BlendUV Offset X");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").y].Add("BlendUV Offset Y");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").z].Add("BlendUV Tile X");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").w].Add("BlendUV Tile Y");
        }

        public override void CollectCustomColorData(ref List<List<string>> useCustomDataList)
        {
            useCustomDataList[_mat.GetInt(_Color + "_Data")].Add("BlendTex Color");
        }

    }
}
