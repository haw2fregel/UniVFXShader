using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


namespace UniVFX.Editor
{
    public class SurfaceOptionBRPTransparent  : UniVFXOption
    {
        const string _ColorMultiplAlpha = "_ColorMultiplAlpha";
        const string _SrcBlend = "_BUILTIN_SrcBlend";
        const string _DstBlend = "_BUILTIN_DstBlend";
        const string _ZTest = "_BUILTIN_ZTest";
        const string _Cull = "_BUILTIN_CullMode";

        readonly static string[] _BlendMode = { "Zero", "One", "DstColor", "SrcColor", "OneMinusDstColor", "SrcAlpha", "OneMinusSrcColor", "DstAlpha", "OneMinusDstAlpha", "SrcAlphaSaturate", "OneMinusSrcAlpha" };
        readonly static string[] _ZTestMode = { "Disabled", "Never", "Less", "Equal", "LessEqual", "Greater", "NotEqual", "GreaterEqual", "Always"};
        readonly static string[] _CullMode = { "Off", "Front", "Back"};


        public override bool IsActive()
        {
            return true;
        }

        public override void SetActive(bool active)
        {
        }
        public override int HeatValue()
        {
            return 0;
        }

        public override void GetHeatValue(ref int value, ref int max)
        {
            max += HeatValue();
            if (IsActive())
                value += HeatValue();
        }

        public override void OptionGUI()
        {
            GUI.color = new Color(5f, 5f, 5f, 1.0f);
            using (new EditorGUILayout.VerticalScope("Box"))
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    GUI.color = new Color(1f, 1f, 1f, 1.0f);
                    _viewGUI = EditorGUILayout.Foldout(_viewGUI, "Surface Option");
                    if (_viewGUI)
                    {
                        GUI.color = new Color(0f, 0f, 0f, 0.6f);
                        using (new EditorGUILayout.VerticalScope("Box"))
                        {
                            using (new EditorGUI.IndentLevelScope())
                            {
                                GUI.color = new Color(1f, 1f, 1f, 1f);
                                UniVFXGUILayout.OptionBoolField(ref _mat, _ColorMultiplAlpha, "Color Multiple Alpha");
                                UniVFXGUILayout.OptionPopupField(ref _mat, _SrcBlend, "Src Blend", _BlendMode);
                                UniVFXGUILayout.OptionPopupField(ref _mat, _DstBlend, "Dst Blend", _BlendMode);
                                UniVFXGUILayout.OptionPopupField(ref _mat, _ZTest, "ZTest", _ZTestMode);
                                UniVFXGUILayout.OptionPopupField(ref _mat, _Cull, "Cull", _CullMode);
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
        }
    }
}
