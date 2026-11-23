using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UniVFX.Editor
{
    internal static class UniVFXUniqueShaderRevertUtility
    {
        internal static bool TryRevertToBaseShader(Material material, string baseShaderName)
        {
            if (material == null)
            {
                return false;
            }

            if (material.isVariant)
            {
                Debug.LogWarning(
                    "[UniVFX] Material Variant のシェーダーは直接変更できません。親マテリアルから汎用シェーダーに戻してください。",
                    material);
                return false;
            }

            var baseShader = Shader.Find(baseShaderName);
            if (baseShader == null)
            {
                Debug.LogWarning(
                    $"[UniVFX] 汎用シェーダー '{baseShaderName}' が見つからないため、シェーダーを戻せませんでした。",
                    material);
                return false;
            }

            var isConfirmed = EditorUtility.DisplayDialog(
                title: "汎用シェーダーに戻す",
                message: "このマテリアルを汎用シェーダーに戻します。\n他のマテリアルから参照されていない場合、現在の専用シェーダーアセットは削除されます。\n削除された専用シェーダーは Undo（元に戻す）で復元できません。\n続行しますか？",
                ok: "戻す",
                cancel: "キャンセル");
            if (!isConfirmed)
            {
                return false;
            }

            var currentShader = material.shader;
            var shaderPath = currentShader != null ? AssetDatabase.GetAssetPath(currentShader) : string.Empty;
            var canDeleteShaderAsset = CanDeleteShaderAsset(currentShader, baseShader, shaderPath);
            var hasOtherReferences =
                canDeleteShaderAsset &&
                HasOtherMaterialReferences(currentShader, material);
            var shouldDeleteShaderAsset = canDeleteShaderAsset && !hasOtherReferences;

            if (!shouldDeleteShaderAsset)
            {
                Undo.RecordObject(material, "UniVFX Revert Shader");
            }

            material.shader = baseShader;
            EditorUtility.SetDirty(material);

            if (!canDeleteShaderAsset)
            {
                return true;
            }

            if (hasOtherReferences)
            {
                Debug.LogWarning(
                    $"[UniVFX] 専用シェーダー '{shaderPath}' は他のマテリアルから参照されているため削除しませんでした。",
                    currentShader);
                return true;
            }

            if (!AssetDatabase.DeleteAsset(shaderPath))
            {
                Debug.LogWarning(
                    $"[UniVFX] 専用シェーダー '{shaderPath}' の削除に失敗しました。",
                    currentShader);
            }

            return true;
        }

        internal static bool HasOtherMaterialReferences(Shader targetShader, Material excludeMaterial)
        {
            if (targetShader == null)
            {
                return false;
            }

            var materialGuids = AssetDatabase.FindAssets("t:Material");
            var checkedPaths = new HashSet<string>();

            foreach (var guid in materialGuids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (string.IsNullOrEmpty(assetPath) || !checkedPaths.Add(assetPath))
                {
                    continue;
                }

                var assets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
                if (assets == null)
                {
                    continue;
                }

                foreach (var asset in assets)
                {
                    if (asset is Material otherMaterial &&
                        IsOtherShaderOwner(otherMaterial, targetShader, excludeMaterial))
                    {
                        return true;
                    }
                }
            }

            var loadedMaterials = Resources.FindObjectsOfTypeAll<Material>();
            foreach (var otherMaterial in loadedMaterials)
            {
                if (otherMaterial != null &&
                    (otherMaterial.hideFlags & HideFlags.DontSave) == 0 &&
                    IsOtherShaderOwner(otherMaterial, targetShader, excludeMaterial))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsOtherShaderOwner(Material otherMaterial, Shader targetShader, Material excludeMaterial)
        {
            if (otherMaterial == excludeMaterial)
            {
                return false;
            }

            // 親を持つ Material Variant は親マテリアルのシェーダーへ追従するため、親側の判定に委ねる
            if (otherMaterial.isVariant && otherMaterial.parent != null)
            {
                return false;
            }

            return otherMaterial.shader == targetShader;
        }

        private static bool CanDeleteShaderAsset(Shader currentShader, Shader baseShader, string shaderPath)
        {
            if (currentShader == null || currentShader == baseShader)
            {
                return false;
            }

            if (string.IsNullOrEmpty(shaderPath))
            {
                return false;
            }

            if (!shaderPath.StartsWith("Assets/", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!shaderPath.EndsWith(".shader", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return AssetDatabase.IsMainAsset(currentShader);
        }
    }
}
