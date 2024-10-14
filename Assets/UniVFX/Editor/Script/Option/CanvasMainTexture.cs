using UnityEngine;
using UnityEditor;


namespace UniVFX.Editor
{
    public class CanvasMainTexture : MainTexture
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
                    _viewGUI = EditorGUILayout.Foldout(_viewGUI, "Main Texture");
                    if (_viewGUI)
                    {
                        GUI.color = new Color(0f, 0f, 0f, 0.6f);
                        using (new EditorGUILayout.VerticalScope("Box"))
                        {
                            using (new EditorGUI.IndentLevelScope())
                            {
                                GUI.color = new Color(1f, 1f, 1f, 1f);
                                UniVFXGUILayout.CanvasOptionColorField(ref _mat, _Color, "Color");
                                UniVFXGUILayout.OptionBoolField(ref _mat, _ColorMultiple, "Color Multiple");
                                UniVFXGUILayout.OptionBoolField(ref _mat, _AlphaMultiple, "Alpha Multiple");
                                UniVFXGUILayout.CanvasUVGUILayout(ref _mat, ref _viewUVGUI, _UV);
                            }
                        }
                    }
                }
            }
            GUI.color = new Color(1f, 1f, 1f, 1f);
        }

        public override void VaridateCustomData()
        {
            UniVFXGUILayout.VaridateCanvasCustomDataVector(ref _mat, _UV + "Transform");
            UniVFXGUILayout.VaridateArrayIndex(ref _mat, _UV + "Transform_Index", UniVFXGUILayout._CanvasUVChannelOption);
            UniVFXGUILayout.VaridateCanvasCustomColorDataInt(ref _mat, _Color);
        }

    }

}
