using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace UniVFX.Editor
{
    internal static class UniVFXUniqueShaderPathUtility
    {
        private const string DefaultGeneratedRoot = "Assets/UniVFXGenerated";
        private const string OutputDirectoryConfigKey = "UniVFX.UniqueShaderOutputDirectory";
        private const string GeneratedMarker = "CustomEditor \"UniVFX.Editor.UniVFXUnique";

        internal static string GetSavedOutputRoot()
        {
            var saved = EditorUserSettings.GetConfigValue(OutputDirectoryConfigKey);
            return NormalizeAssetsDirectory(saved);
        }

        internal static void SetSavedOutputRoot(string assetsDirectory)
        {
            var normalized = NormalizeAssetsDirectory(assetsDirectory);
            EditorUserSettings.SetConfigValue(OutputDirectoryConfigKey, normalized);
        }

        internal static void DrawOutputDirectoryField(Material material)
        {
            var savedRoot = GetSavedOutputRoot();
            var defaultRoot = ResolveDefaultRoot(material);
            var displayRoot = string.IsNullOrEmpty(savedRoot) ? defaultRoot : savedRoot;

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUI.BeginChangeCheck();
                var edited = EditorGUILayout.TextField("出力先フォルダ", displayRoot);
                if (EditorGUI.EndChangeCheck())
                {
                    if (string.IsNullOrWhiteSpace(edited))
                    {
                        SetSavedOutputRoot(string.Empty);
                    }
                    else
                    {
                        var normalized = NormalizeAssetsDirectory(edited);
                        if (!string.IsNullOrEmpty(normalized))
                        {
                            SetSavedOutputRoot(normalized);
                        }
                        else
                        {
                            Debug.LogWarning("[UniVFX] 出力先フォルダはプロジェクトの Assets 配下を指定してください。");
                        }
                    }
                }

                if (GUILayout.Button("...", GUILayout.Width(28f)))
                {
                    var projectRoot = Path.GetDirectoryName(Application.dataPath)?.Replace('\\', '/') ?? string.Empty;
                    var initialFullPath = !string.IsNullOrEmpty(projectRoot)
                        ? Path.Combine(projectRoot, displayRoot).Replace('\\', '/')
                        : Application.dataPath;
                    if (!Directory.Exists(initialFullPath))
                    {
                        initialFullPath = Application.dataPath;
                    }

                    var selected = EditorUtility.OpenFolderPanel("専用シェーダーの出力先フォルダを選択", initialFullPath, string.Empty);
                    if (!string.IsNullOrEmpty(selected))
                    {
                        var relative = ToAssetsRelativePath(selected);
                        if (!string.IsNullOrEmpty(relative))
                        {
                            SetSavedOutputRoot(relative);
                            GUI.FocusControl(null);
                        }
                        else
                        {
                            Debug.LogWarning("[UniVFX] 出力先フォルダはプロジェクトの Assets 配下を選択してください。");
                        }
                    }
                }

                using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(savedRoot)))
                {
                    if (GUILayout.Button("初期値", GUILayout.Width(48f)))
                    {
                        SetSavedOutputRoot(string.Empty);
                        GUI.FocusControl(null);
                    }
                }
            }
        }

        internal static string ResolveOutputPath(Material material, string categoryFolder)
        {
            var shaderPath = material != null && material.shader != null
                ? AssetDatabase.GetAssetPath(material.shader)
                : string.Empty;
            var savedRoot = GetSavedOutputRoot();
            var baseDir = !string.IsNullOrEmpty(savedRoot)
                ? CombineCategory(savedRoot, categoryFolder)
                : ResolveBaseDirectory(shaderPath, categoryFolder);
            var safeName = SanitizeFileName(material != null ? material.name : string.Empty);
            var candidatePath = baseDir + "/" + safeName + ".shader";

            if (ShouldAvoidOverwrite(candidatePath, material))
            {
                candidatePath = AssetDatabase.GenerateUniqueAssetPath(candidatePath);
            }

            return candidatePath;
        }

        internal static string SanitizeShaderName(string rawName)
        {
            var sanitized = SanitizeFileName(rawName)
                .Replace("\"", "_")
                .Replace("\r", string.Empty)
                .Replace("\n", string.Empty)
                .Trim();
            return string.IsNullOrEmpty(sanitized) ? "UniVFXMaterial" : sanitized;
        }

        internal static bool TryWriteAndAssignGeneratedShader(Material material, string path, string shaderCode)
        {
            if (material == null || string.IsNullOrEmpty(path))
            {
                return false;
            }

            var normalizedDir = NormalizeAssetsDirectory(Path.GetDirectoryName(path));
            if (string.IsNullOrEmpty(normalizedDir))
            {
                Debug.LogError($"[UniVFX] 出力先パスが Assets 配下ではないため変換を中止しました: '{path}'", material);
                return false;
            }

            var existedBefore = File.Exists(path);
            if (!Directory.Exists(normalizedDir))
            {
                Directory.CreateDirectory(normalizedDir);
            }

            File.WriteAllText(path, shaderCode);
            AssetDatabase.ImportAsset(path);

            var asset = AssetDatabase.LoadAssetAtPath<Shader>(path);
            if (asset == null || ShaderUtil.ShaderHasError(asset))
            {
                Debug.LogError($"[UniVFX] 専用シェーダーのコンパイルに失敗したため、マテリアルの切り替えを中止しました: '{path}'", asset != null ? (UnityEngine.Object)asset : material);
                if (!existedBefore)
                {
                    AssetDatabase.DeleteAsset(path);
                }
                return false;
            }

            Undo.RecordObject(material, "UniVFX Convert Unique Shader");
            material.parent = null;
            material.shader = asset;
            EditorUtility.SetDirty(material);
            return true;
        }

        internal static string ResolveBaseDirectory(string shaderAssetPath, string categoryFolder)
        {
            var root = ResolveDefaultRootFromShaderPath(shaderAssetPath);
            return CombineCategory(root, categoryFolder);
        }

        internal static string SanitizeFileName(string rawName)
        {
            if (string.IsNullOrWhiteSpace(rawName))
            {
                return "UniVFXMaterial";
            }

            var invalidChars = Path.GetInvalidFileNameChars();
            var chars = rawName.Trim().ToCharArray();
            for (var i = 0; i < chars.Length; i++)
            {
                if (Array.IndexOf(invalidChars, chars[i]) >= 0)
                {
                    chars[i] = '_';
                }
            }

            var sanitized = new string(chars).Trim().TrimEnd('.');
            return string.IsNullOrEmpty(sanitized) ? "UniVFXMaterial" : sanitized;
        }

        private static bool ShouldAvoidOverwrite(string candidatePath, Material ownerMaterial)
        {
            var existingShader = AssetDatabase.LoadAssetAtPath<Shader>(candidatePath);
            if (existingShader != null &&
                UniVFXUniqueShaderRevertUtility.HasOtherMaterialReferences(existingShader, ownerMaterial))
            {
                return true;
            }

            if (File.Exists(candidatePath))
            {
                // 手書きシェーダーやロード不能ファイルの上書きを防ぐため、UniVFX生成物マーカーが無い場合は別名回避する
                try
                {
                    var existingText = File.ReadAllText(candidatePath);
                    if (existingText.IndexOf(GeneratedMarker, StringComparison.Ordinal) < 0)
                    {
                        return true;
                    }
                }
                catch
                {
                    return true;
                }
            }

            return false;
        }

        private static string ResolveDefaultRoot(Material material)
        {
            var shaderPath = material != null && material.shader != null
                ? AssetDatabase.GetAssetPath(material.shader)
                : string.Empty;
            return ResolveDefaultRootFromShaderPath(shaderPath);
        }

        private static string ResolveDefaultRootFromShaderPath(string shaderAssetPath)
        {
            if (!string.IsNullOrEmpty(shaderAssetPath))
            {
                var normalizedShaderPath = shaderAssetPath.Replace('\\', '/');
                if (normalizedShaderPath.StartsWith("Assets/", StringComparison.OrdinalIgnoreCase))
                {
                    var shaderDir = Path.GetDirectoryName(normalizedShaderPath)?.Replace('\\', '/');
                    if (!string.IsNullOrEmpty(shaderDir))
                    {
                        return shaderDir.TrimEnd('/');
                    }
                }
            }

            return DefaultGeneratedRoot;
        }

        private static string CombineCategory(string root, string categoryFolder)
        {
            var normalizedRoot = root.Replace('\\', '/').TrimEnd('/');
            var normalizedCategory = string.IsNullOrWhiteSpace(categoryFolder)
                ? "Generated"
                : categoryFolder.Trim('/');
            return normalizedRoot + "/" + normalizedCategory;
        }

        private static string NormalizeAssetsDirectory(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }

            var trimmed = path.Trim().Replace('\\', '/').TrimEnd('/');
            if (!string.Equals(trimmed, "Assets", StringComparison.OrdinalIgnoreCase) &&
                !trimmed.StartsWith("Assets/", StringComparison.OrdinalIgnoreCase))
            {
                return string.Empty;
            }

            var projectRoot = Path.GetDirectoryName(Application.dataPath);
            if (string.IsNullOrEmpty(projectRoot))
            {
                return string.Empty;
            }

            try
            {
                var combinedFull = Path.GetFullPath(Path.Combine(projectRoot, trimmed));
                return ToAssetsRelativePath(combinedFull);
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string ToAssetsRelativePath(string fullPath)
        {
            if (string.IsNullOrWhiteSpace(fullPath))
            {
                return string.Empty;
            }

            var normalizedFull = Path.GetFullPath(fullPath).Replace('\\', '/').TrimEnd('/');
            var normalizedDataPath = Path.GetFullPath(Application.dataPath).Replace('\\', '/').TrimEnd('/');
            if (string.Equals(normalizedFull, normalizedDataPath, StringComparison.OrdinalIgnoreCase))
            {
                return "Assets";
            }

            if (normalizedFull.StartsWith(normalizedDataPath + "/", StringComparison.OrdinalIgnoreCase))
            {
                return "Assets" + normalizedFull.Substring(normalizedDataPath.Length);
            }

            return string.Empty;
        }
    }
}
