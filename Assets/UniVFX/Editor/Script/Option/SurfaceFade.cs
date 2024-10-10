using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


namespace UniVFX.Editor
{
    public class SurfaceFade : UniVFXOption
    {
        const string _IsActive = "_SURFACEFADE";
        const string _Frenel = "_Frenel";
        const string _Pow = "_FrenelPow";
        const string _Reverce = "_FrenelReverce";
        const string _FadeIn = "_SurfaceFadeIn";
        const string _FadeOut = "_SurfaceFadeOut";
        const string _Type = "_SurfaceFadeType";
        readonly static string[] _TypeOption = { "Screen Z", "Height", "SoftParticle", "Off" };

        const string _TargetFinalColor = "_FinalColorSurfaceFade";
        const string _TargetFinalAlpha = "_FinalAlphaSurfaceFade";
        const string _TargetMainTex = "_MainTexSurfaceFade";
        const string _TargetBlendColor = "_BlendColorSurfaceFade";
        const string _TargetBlendTex = "_BlendTexSurfaceFade";
        const string _TargetHSVShift = "_HSVShiftSurfaceFade";
        const string _TargetDistortion = "_DistortionSurfaceFade";
        const string _TargetDissolve = "_DissolveSurfaceFade";

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
            return 30;
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
                    _viewGUI = EditorGUILayout.Foldout(_viewGUI, "Surface Fade");
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

                                    var frenel = UniVFXGUILayout.OptionBoolField(ref _mat, _Frenel, "Frenel");
                                    if(frenel)
                                    {
                                        UniVFXGUILayout.OptionSlider(ref _mat, _Pow, "Power", 0.001f, 50);
                                        UniVFXGUILayout.OptionBoolField(ref _mat, _Reverce, "Reverce");
                                    }

                                    var type = UniVFXGUILayout.OptionPopupField(ref _mat, _Type, "Position Fade Type", _TypeOption);
                                    if (type < 3)
                                    {
                                        UniVFXGUILayout.OptionSlider(ref _mat, _FadeIn, "FadeIn", -10, 10);
                                        UniVFXGUILayout.OptionSlider(ref _mat, _FadeOut, "FadeOut", -10, 10);
                                    }

                                    

                                    _viewTargetGUI = EditorGUILayout.Foldout(_viewTargetGUI, "Target");
                                    if (_viewTargetGUI)
                                    {
                                        GUI.color = new Color(0f, 0f, 0f, 0.8f);
                                        using (new EditorGUILayout.VerticalScope("Box"))
                                        {
                                            GUI.color = new Color(1f, 1f, 1f, 1.0f);
                                            UniVFXGUILayout.OptionBoolField(ref _mat, _TargetFinalColor, "Final Color");
                                            UniVFXGUILayout.OptionBoolField(ref _mat, _TargetFinalAlpha, "Final Alpha");
                                            UniVFXGUILayout.OptionBoolField(ref _mat, _TargetMainTex, "Main Texture");
                                            var blend = UniVFXGUILayout.OptionBoolField(ref _mat, _TargetBlendTex, "Blend Texture");
                                            var distortion = UniVFXGUILayout.OptionBoolField(ref _mat, _TargetDistortion, "Distortion");
                                            var dissolve = UniVFXGUILayout.OptionBoolField(ref _mat, _TargetDissolve, "Dissolve");
                                            var hsv = UniVFXGUILayout.OptionBoolField(ref _mat, _TargetHSVShift, "HSV Shift");

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
            useCustomDataList[_mat.GetInt(_Pow + "_Data")].Add("Frenel Power");
            useCustomDataList[_mat.GetInt(_FadeIn + "_Data")].Add("Surface FadeIn");
            useCustomDataList[_mat.GetInt(_FadeOut + "_Data")].Add("Surface FadeOut");
        }

        public override void CollectCustomColorData(ref List<List<string>> useCustomDataList)
        {

        }

    }

}
