using UnityEngine;
using UnityEditor;

namespace UniVFX.Editor
{
    public class UniVFXUniqueBRPTransparentInspector : ShaderGUI
    {

        // MARK: OnGUI
        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            Material material = materialEditor.target as Material;
            Undo.RecordObject(material, "UniVFX Change");

            base.OnGUI(materialEditor, properties);

            if (GUILayout.Button("汎用シェーダーに戻す"))
            {
                var currentShader = material.shader;
                material.shader = Shader.Find("Shader Graphs/UniVFXBRPTransparent");
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(currentShader));

            }
            
            EditorUtility.SetDirty(material);
        }
    }
}