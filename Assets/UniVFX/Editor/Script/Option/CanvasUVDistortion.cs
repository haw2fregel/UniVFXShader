using UnityEngine;
using UnityEditor;


namespace UniVFX.Editor
{
    public class CanvasUVDistortion : UVDistortion
    {
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
                                    UniVFXGUILayout.CanvasOptionSlider(ref _mat, _Intensity, "Intensity", -1, 1);
                                    UniVFXGUILayout.CanvasUVGUILayout(ref _mat, ref _viewUVGUI, _UV);
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
    }
}
