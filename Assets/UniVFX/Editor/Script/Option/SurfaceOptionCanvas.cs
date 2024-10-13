using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


namespace UniVFX.Editor
{
    public class SurfaceOptionCanvas : UniVFXOption
    {
        const string _SrcBlend = "_SrcBlend";
        const string _DstBlend = "_DstBlend";


        readonly static string[] _BlendMode = { "Zero", "One", "DstColor", "SrcColor", "OneMinusDstColor", "SrcAlpha", "OneMinusSrcColor", "DstAlpha", "OneMinusDstAlpha", "SrcAlphaSaturate", "OneMinusSrcAlpha" };


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
                                UniVFXGUILayout.OptionPopupField(ref _mat, _SrcBlend, "Src Blend", _BlendMode);
                                UniVFXGUILayout.OptionPopupField(ref _mat, _DstBlend, "Dst Blend", _BlendMode);
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
