using UnityEditor;
using UnityEngine;

namespace UniVFX.Editor
{
    public class UniVFXUniqueBRPOpaqueInspector : ShaderGUI
    {
        private const string BaseShaderName = "Shader Graphs/UniVFXBRPOpaque";

        // MARK: OnGUI
        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            var material = materialEditor.target as Material;
            if (material == null)
            {
                return;
            }

            EditorGUI.BeginChangeCheck();
            base.OnGUI(materialEditor, properties);
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(material);
            }

            if (GUILayout.Button("汎用シェーダーに戻す"))
            {
                UniVFXUniqueShaderRevertUtility.TryRevertToBaseShader(material, BaseShaderName);
            }
        }
    }
}
