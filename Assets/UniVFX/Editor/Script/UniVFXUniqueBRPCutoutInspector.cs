using UnityEditor;
using UnityEngine;

namespace UniVFX.Editor
{
    public class UniVFXUniqueBRPCutoutInspector : ShaderGUI
    {
        private const string BaseShaderName = "Shader Graphs/UniVFXBRPCutout";

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
