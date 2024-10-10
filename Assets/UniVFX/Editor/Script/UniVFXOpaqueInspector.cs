using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System;


namespace UniVFX.Editor
{
    public class UniVFXOpaqueInspector : ShaderGUI
    {
        bool _viewVertexData = false;
        List<UniVFXOption> _options;
        Gradient _heatGradation;
        // MARK: HeatGradation
        // 描画負荷ゲージで使うグラデーションの生成
        Gradient HeatGradation()
        {
            if (_heatGradation != null)
                return _heatGradation;

            _heatGradation = new Gradient();

            var colorKey = new GradientColorKey[4];
            colorKey[0].color = new Color(0, 0.608357f, 1);
            colorKey[0].time = 0.4f;
            colorKey[1].color = new Color(0.7f, 0.7f, 0.0f);
            colorKey[1].time = 0.42f;
            colorKey[2].color = new Color(1f, 0.5f, 0.0f);
            colorKey[2].time = 0.55f;
            colorKey[3].color = new Color(1f, 0f, 0);
            colorKey[3].time = 0.7f;

            var alphaKey = new GradientAlphaKey[1];
            alphaKey[0].alpha = 0.7f;
            alphaKey[0].time = 0.0f;

            _heatGradation.SetKeys(colorKey, alphaKey);

            return _heatGradation;
        }

        // MARK: AssignNewShaderToMaterial
        public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
        {
            material.shader = newShader;
            if(material.IsKeywordEnabled("_SURFACE_TYPE_TRANSPARENT"))
                material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
        }

        // MARK: OnGUI
        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            Material material = materialEditor.target as Material;
            Undo.RecordObject(material, "UniVFX Change");

            // MARK: Setup
            if (_options == null)
            {
                _options = new List<UniVFXOption>();
                _options.Add(new MainTexture());
                _options.Add(new MaskTexture());
                _options.Add(new BlendTexture());
                _options.Add(new GradationColor());
                _options.Add(new Dissolve());
                _options.Add(new UVDistortion());
                _options.Add(new UVBend());
                _options.Add(new UVParallax());
                _options.Add(new UVRotate());
                _options.Add(new UVFripBook());
                _options.Add(new HSVShift());
                _options.Add(new SurfaceFade());
                _options.Add(new FakeLight());
                _options.Add(new VertexAnimation());
                _options.Add(new Time());
            }
            foreach (var option in _options)
                option.SetMaterial(material);
            var useVertexDataList = new List<List<string>>();
            for (int i = 0; i < Enum.GetValues(typeof(VertexData)).Length; i++)
            {
                useVertexDataList.Add(new List<string>());
                useVertexDataList[i].Add(((VertexData)Enum.ToObject(typeof(VertexData), i)).ToString());
            }
            var useVertexColorDataList = new List<List<string>>();
            for (int i = 0; i < Enum.GetValues(typeof(VertexColorData)).Length; i++)
            {
                useVertexColorDataList.Add(new List<string>());
                useVertexColorDataList[i].Add(((VertexColorData)Enum.ToObject(typeof(VertexColorData), i)).ToString());
            }


            // MARK: ViewOptions
            foreach (var option in _options)
            {
                //CustomDataの使用状況を収集
                option.CollectCustomData(ref useVertexDataList);
                option.CollectCustomColorData(ref useVertexColorDataList);
                //Inspector表示
                option.OptionGUI();
                EditorGUILayout.Space(0.5f);
            }

            EditorGUILayout.Space(2);
            GUI.color = new Color(5f, 5f, 5f, 1.0f);
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(4));
            using (new EditorGUI.IndentLevelScope())
            {
                // MARK: HeatGage
                // Optionの使用状況から描画負荷を判別。ゲージで表示
                var rect = EditorGUILayout.GetControlRect();
                EditorGUI.DrawRect(rect, new Color(0, 0, 0, 0.5f));
                int heat = 0;
                int heatMax = 0;
                foreach (var option in _options)
                    option.GetHeatValue(ref heat, ref heatMax);
                var heatRate = (float)heat / (float)heatMax;
                var heatRect = rect;
                heatRect.width *= heatRate;
                var heatColor = HeatGradation().Evaluate(heatRate);
                EditorGUI.DrawRect(heatRect, heatColor);
                EditorGUI.LabelField(rect, "Heat", EditorStyles.boldLabel);

                // MARK: ViewCustomData
                // CustomDataの使用状況を表示
                GUI.color = new Color(0.6f, 0.6f, 0.6f, 1.0f);
                _viewVertexData = EditorGUILayout.Foldout(_viewVertexData, "Use VertexData List");
                if (_viewVertexData)
                {
                    using (new EditorGUI.IndentLevelScope())
                    {
                        using (new EditorGUILayout.VerticalScope("Box"))
                        {
                            for (int i = 1; i < Enum.GetValues(typeof(VertexData)).Length; i++)
                            {
                                if (useVertexDataList[i].Count > 1)
                                {
                                    EditorGUILayout.LabelField("Float Data", EditorStyles.wordWrappedLabel);
                                    break;
                                }
                            }

                            using (new EditorGUI.IndentLevelScope(2))
                            {
                                for (int i = 1; i < Enum.GetValues(typeof(VertexData)).Length; i++)
                                {
                                    if (useVertexDataList[i].Count > 1)
                                    {
                                        useVertexDataList[i].RemoveAt(0);
                                        EditorGUILayout.LabelField(((VertexData)Enum.ToObject(typeof(VertexData), i)).ToString() + ":", String.Join(" / ", useVertexDataList[i]), EditorStyles.wordWrappedLabel);
                                    }
                                }
                            }

                            for (int i = 1; i < Enum.GetValues(typeof(VertexColorData)).Length; i++)
                            {
                                if (useVertexColorDataList[i].Count > 1)
                                {
                                    EditorGUILayout.LabelField("Color Data", EditorStyles.wordWrappedLabel);
                                    break;
                                }
                            }

                            using (new EditorGUI.IndentLevelScope(2))
                            {
                                for (int i = 1; i < Enum.GetValues(typeof(VertexColorData)).Length; i++)
                                {
                                    if (useVertexColorDataList[i].Count > 1)
                                    {
                                        useVertexColorDataList[i].RemoveAt(0);
                                        EditorGUILayout.LabelField(((VertexColorData)Enum.ToObject(typeof(VertexColorData), i)).ToString() + ":", String.Join(" / ", useVertexColorDataList[i]), EditorStyles.wordWrappedLabel);
                                    }
                                }
                            }
                        }
                    }
                }
                GUI.color = new Color(1f, 1f, 1f, 1.0f);
            }
            EditorUtility.SetDirty(material);
        }
    }
}