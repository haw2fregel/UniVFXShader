using UnityEngine;
using UnityEditor;


namespace UniVFX.Editor
{
    public class CanvasUVBend : UVBend
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
                    _viewGUI = EditorGUILayout.Foldout(_viewGUI, "UV Bend");
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
                                    UniVFXGUILayout.OptionBoolField(ref _mat, _Polar, "Polar");
                                    UniVFXGUILayout.CanvasOptionSlider(ref _mat, _Param, "Center X", 0, -1, 1);
                                    UniVFXGUILayout.CanvasOptionSlider(ref _mat, _Param, "Center Y", 1, -1, 1);
                                    UniVFXGUILayout.CanvasOptionSlider(ref _mat, _Param, "Bend X", 2, -1, 1);
                                    UniVFXGUILayout.CanvasOptionSlider(ref _mat, _Param, "Bend Y", 3, -1, 1);
                                }
                            }
                        }
                    }
                }
            }
            GUI.color = new Color(1f, 1f, 1f, 1f);
        }

        public override void VaridateCustomData()
        {
            UniVFXGUILayout.VaridateCanvasCustomDataVector(ref _mat, _Param);
        }
    }
}
