using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


namespace UniVFX.Editor
{
    public class SurfaceOptionTransparent : UniVFXOption
    {
        public const string _ColorMultiplAlpha = "_ColorMultiplAlpha";
        public const string _SrcBlend = "_SrcBlend";
        public const string _SrcBlendAlpha = "_SrcBlendAlpha";
        public const string _DstBlend = "_DstBlend";
        public const string _DstBlendAlpha = "_DstBlendAlpha";
        public const string _ZTest = "_ZTest";
        public const string _Cull = "_Cull";

        public readonly static string[] _BlendMode = { "Zero", "One", "DstColor", "SrcColor", "OneMinusDstColor", "SrcAlpha", "OneMinusSrcColor", "DstAlpha", "OneMinusDstAlpha", "SrcAlphaSaturate", "OneMinusSrcAlpha" };
        public readonly static string[] _ZTestMode = { "Off", "Never", "Less", "Equal", "LEqual", "Greater", "NotEqual", "GEqual", "Always"};
        public readonly static string[] _CullMode = { "Off", "Front", "Back"};

        public SurfaceOptionTransparent(bool isCanvas, bool isBRP) : base(isCanvas, isBRP)
        {
        }


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
                                if(_mat.HasProperty(_SrcBlendAlpha) && _mat.HasProperty(_DstBlendAlpha))
                                {
                                    UniVFXGUILayout.OptionPopupField(ref _mat, _SrcBlendAlpha, "Src Blend Alpha", _BlendMode);
                                    UniVFXGUILayout.OptionPopupField(ref _mat, _DstBlendAlpha, "Dst Blend Alpha", _BlendMode);
                                }
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

        public override void CollectUVChannel(ref List<List<string>> useUVChannelList)
        {
        }

        public override void VaridateCustomData()
        {
        }

        public override List<string> GetPropertyCode()
        {
            var code = new List<string>();
            return code;
        }
        public override List<string> GetCBufferCode()
        {
            var code = new List<string>();
            return code;
        }
        public override List<string> GetTextureCode()
        {
            var code = new List<string>();
            return code;
        }
        public override List<string> GetUseV2fCode()
        {
            var code = new List<string>();
            return code;
        }
        public override List<string> GetVertexHeadCode()
        {
            var code = new List<string>();
            return code;
        }
        public override List<string> GetVertexCode()
        {
            var code = new List<string>();
            return code;
        }
        public override List<string> GetFragmentHeadCode()
        {
            var code = new List<string>();
            return code;
        }
        public override List<string> GetFragmentCode()
        {
            var code = new List<string>();
            return code;
        }
    }
}
