using UnityEngine;
using UnityEditor;


namespace UniVFX.Editor
{
    public class CanvasDissolve : Dissolve
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
                                    UniVFXGUILayout.CanvasOptionColorField(ref _mat, _Color, "Color");
                                    UniVFXGUILayout.CanvasOptionSlider(ref _mat, _Param, "Alpha", 0, 0, 1);
                                    UniVFXGUILayout.CanvasOptionSlider(ref _mat, _Param, "Smooth", 1, 0, 1);
                                    UniVFXGUILayout.CanvasOptionSlider(ref _mat, _Param, "Emissive Width", 2, 0, 1);
                                    UniVFXGUILayout.CanvasOptionSlider(ref _mat, _Param, "Emissive Smooth", 3, 0, 1);
                                    UniVFXGUILayout.CanvasUVGUILayout(ref _mat, ref _viewUVGUI, _UV);
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
