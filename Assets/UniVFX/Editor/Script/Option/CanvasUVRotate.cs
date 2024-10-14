using UnityEngine;
using UnityEditor;


namespace UniVFX.Editor
{

    public class CanvasUVRotate : UVRotate
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
                    _viewGUI = EditorGUILayout.Foldout(_viewGUI, "UV Rotate");
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
                                    UniVFXGUILayout.CanvasOptionSlider(ref _mat, _Rotate, "Rotate", 0, 360);
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
            UniVFXGUILayout.VaridateCanvasCustomDataInt(ref _mat, _Rotate);
        }

    }
}
