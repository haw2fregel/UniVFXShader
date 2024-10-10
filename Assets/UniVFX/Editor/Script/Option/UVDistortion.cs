using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


namespace UniVFX.Editor
{
    public class UVDistortion : UniVFXOption
    {
        const string _IsActive = "_DISTORTIONENABLE";
        const string _Tex = "_DistortionTex";
        const string _UV = "_DistortionUV";
        const string _Intensity = "_DistortionIntensity";
        const string _TargetMainTex = "_MainUVDistortion";
        const string _TargetBlendTex = "_BlendUVDistortion";
        const string _TargetDissolveTex = "_DissolveUVDistortion";
        const string _TargetGradation = "_GradationUVDistortion";

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
                    _viewGUI = EditorGUILayout.Foldout(_viewGUI, "UV Distortion");
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
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Intensity, "Intensity", -1, 1);
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
                                            var dissolve = UniVFXGUILayout.OptionBoolField(ref _mat, _TargetDissolveTex, "Dissolve Texture");
                                            var gradation = UniVFXGUILayout.OptionBoolField(ref _mat, _TargetGradation, "Gradation");

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
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").x].Add("DistortionUV Tile X");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").y].Add("DistortionUV Tile Y");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").z].Add("DistortionUV Offset X");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").w].Add("DistortionUV Offset Y");
            useCustomDataList[(int)_mat.GetFloat(_Intensity + "_Data")].Add("Distortion Intensity");
        }

        public override void CollectCustomColorData(ref List<List<string>> useCustomDataList)
        {

        }


    }
}
