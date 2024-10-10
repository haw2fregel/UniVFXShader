using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


namespace UniVFX.Editor
{
    public class MaskTexture : UniVFXOption
    {
        const string _IsActive = "_MASKTEXTURE";
        const string _Tex = "_MaskTex";
        const string _UV = "_MaskUV";
        const string _Offset = "_MaskOffset";
        const string _Repeat = "_MaskRepeat";
        const string _TargetMainTex = "_MainTexMask";
        const string _TargetBlendTex = "_BlendTexMask";
        const string _TargetGradation = "_GradationMask";
        const string _TargetDistortionTex = "_DistortionMask";
        const string _TargetDissolveTex = "_DissolveMask";
        const string _TargetSurfaceFade = "_SurfaceFadeMask";
        const string _TargetHSVShift = "_HSVShiftMask";
        const string _TargetFakeLight = "_FakeLightMask";
        const string _TargetParallax = "_ParallaxMask";

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
                    _viewGUI = EditorGUILayout.Foldout(_viewGUI, "Mask Texture");
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
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Offset, "Value Offset", -2, 2);
                                    UniVFXGUILayout.OptionBoolField(ref _mat, _Repeat, "Repeat");
                                    UniVFXGUILayout.UVGUILayout(ref _mat, ref _viewUVGUI, _UV);

                                    _viewTargetGUI = EditorGUILayout.Foldout(_viewTargetGUI, "Target");
                                    if (_viewTargetGUI)
                                    {
                                        GUI.color = new Color(0f, 0f, 0f, 0.8f);
                                        using (new EditorGUILayout.VerticalScope("Box"))
                                        {
                                            GUI.color = new Color(1f, 1f, 1f, 1.0f);
                                            UniVFXGUILayout.OptionBoolField(ref _mat, _TargetMainTex, "Main Texture");
                                            var blend = UniVFXGUILayout.OptionBoolField(ref _mat, _TargetBlendTex, "Blend Texture");
                                            var gradation = UniVFXGUILayout.OptionBoolField(ref _mat, _TargetGradation, "Gradation");
                                            var distortion = UniVFXGUILayout.OptionBoolField(ref _mat, _TargetDistortionTex, "Distortion");
                                            var dissolve = UniVFXGUILayout.OptionBoolField(ref _mat, _TargetDissolveTex, "Dissolve");
                                            var surfafeFade = UniVFXGUILayout.OptionBoolField(ref _mat, _TargetSurfaceFade, "Surface Fade");
                                            var hsv = UniVFXGUILayout.OptionBoolField(ref _mat, _TargetHSVShift, "HSV Shift");
                                            var fakeLight = UniVFXGUILayout.OptionBoolField(ref _mat, _TargetFakeLight, "Fake Light");
                                            var parallax = UniVFXGUILayout.OptionBoolField(ref _mat, _TargetParallax, "Parallax");

                                            if (blend && !BlendTexture.IsActive(_mat))
                                            {
                                                var rect = EditorGUILayout.GetControlRect();
                                                rect.height = 40;
                                                rect.xMin += 30;
                                                EditorGUI.HelpBox(rect, "Please BlendTexture Active", MessageType.Warning);
                                                rect.xMin = rect.xMax - 70;
                                                if (GUI.Button(rect, "Fix now"))
                                                    BlendTexture.SetActive(_mat, true);
                                                EditorGUILayout.GetControlRect();
                                            }

                                            if (gradation && !GradationColor.IsActive(_mat))
                                            {
                                                var rect = EditorGUILayout.GetControlRect();
                                                rect.height = 40;
                                                rect.xMin += 30;
                                                EditorGUI.HelpBox(rect, "Please GradationColor Active", MessageType.Warning);
                                                rect.xMin = rect.xMax - 70;
                                                if (GUI.Button(rect, "Fix now"))
                                                    GradationColor.SetActive(_mat, true);
                                                EditorGUILayout.GetControlRect();
                                            }

                                            if (distortion && !UVDistortion.IsActive(_mat))
                                            {
                                                var rect = EditorGUILayout.GetControlRect();
                                                rect.height = 40;
                                                rect.xMin += 30;
                                                EditorGUI.HelpBox(rect, "Please Distortion Active", MessageType.Warning);
                                                rect.xMin = rect.xMax - 70;
                                                if (GUI.Button(rect, "Fix now"))
                                                    UVDistortion.SetActive(_mat, true);
                                                EditorGUILayout.GetControlRect();
                                            }

                                            if (dissolve && !Dissolve.IsActive(_mat))
                                            {
                                                var rect = EditorGUILayout.GetControlRect();
                                                rect.height = 40;
                                                rect.xMin += 30;
                                                EditorGUI.HelpBox(rect, "Please Dissolve Active", MessageType.Warning);
                                                rect.xMin = rect.xMax - 70;
                                                if (GUI.Button(rect, "Fix now"))
                                                    Dissolve.SetActive(_mat, true);
                                                EditorGUILayout.GetControlRect();
                                            }

                                            if (surfafeFade && !SurfaceFade.IsActive(_mat))
                                            {
                                                var rect = EditorGUILayout.GetControlRect();
                                                rect.height = 40;
                                                rect.xMin += 30;
                                                EditorGUI.HelpBox(rect, "Please SurfaceFade Active", MessageType.Warning);
                                                rect.xMin = rect.xMax - 70;
                                                if (GUI.Button(rect, "Fix now"))
                                                    SurfaceFade.SetActive(_mat, true);
                                                EditorGUILayout.GetControlRect();
                                            }

                                            if (hsv && !HSVShift.IsActive(_mat))
                                            {
                                                var rect = EditorGUILayout.GetControlRect();
                                                rect.height = 40;
                                                rect.xMin += 30;
                                                EditorGUI.HelpBox(rect, "Please HSVShift Active", MessageType.Warning);
                                                rect.xMin = rect.xMax - 70;
                                                if (GUI.Button(rect, "Fix now"))
                                                    HSVShift.SetActive(_mat, true);
                                                EditorGUILayout.GetControlRect();
                                            }

                                            if (fakeLight && !FakeLight.IsActive(_mat))
                                            {
                                                var rect = EditorGUILayout.GetControlRect();
                                                rect.height = 40;
                                                rect.xMin += 30;
                                                EditorGUI.HelpBox(rect, "Please FakeLight Active", MessageType.Warning);
                                                rect.xMin = rect.xMax - 70;
                                                if (GUI.Button(rect, "Fix now"))
                                                    FakeLight.SetActive(_mat, true);
                                                EditorGUILayout.GetControlRect();
                                            }

                                            if (parallax && !UVParallax.IsActive(_mat))
                                            {
                                                var rect = EditorGUILayout.GetControlRect();
                                                rect.height = 40;
                                                rect.xMin += 30;
                                                EditorGUI.HelpBox(rect, "Please UVParallax Active", MessageType.Warning);
                                                rect.xMin = rect.xMax - 70;
                                                if (GUI.Button(rect, "Fix now"))
                                                    UVParallax.SetActive(_mat, true);
                                                EditorGUILayout.GetControlRect();
                                            }

                                        }
                                    }
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
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").x].Add("MaskUV Tile X");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").y].Add("MaskUV Tile Y");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").z].Add("MaskUV Offset X");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").w].Add("MaskUV Offset Y");
            useCustomDataList[(int)_mat.GetFloat(_Offset + "_Data")].Add("Mask Offset");
        }

        public override void CollectCustomColorData(ref List<List<string>> useCustomDataList)
        {

        }


    }
}
