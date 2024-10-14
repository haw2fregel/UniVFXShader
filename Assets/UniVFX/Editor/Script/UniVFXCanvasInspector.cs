using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System;


namespace UniVFX.Editor
{
    public class UniVFXCanvasInspector : ShaderGUI
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
            if(!material.IsKeywordEnabled("_SURFACE_TYPE_TRANSPARENT"))
                material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        }

        // MARK: OnGUI
        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            Material material = materialEditor.target as Material;
            Undo.RecordObject(material, "UniVFX Change");

            var tex = FindProperty("_MainTex", properties).textureValue;
            material.SetTexture("_MainTex", tex);

            // MARK: Setup
            if (_options == null)
            {
                _options = new List<UniVFXOption>();
                _options.Add(new CanvasMainTexture());
                _options.Add(new CanvasMaskTexture());
                _options.Add(new CanvasBlendTexture());
                _options.Add(new CanvasGradationColor());
                _options.Add(new CanvasDissolve());
                _options.Add(new CanvasUVDistortion());
                _options.Add(new CanvasUVBend());
                _options.Add(new CanvasUVRotate());
                _options.Add(new CanvasUVFripBook());
                _options.Add(new CanvasHSVShift());
                _options.Add(new Time());
                _options.Add(new SurfaceOptionCanvas());
            }
            foreach (var option in _options)
                option.SetMaterial(material);
            var useVertexDataList = new List<List<string>>();
            for (int i = 0; i < Enum.GetValues(typeof(CanvasVertexData)).Length; i++)
            {
                useVertexDataList.Add(new List<string>());
                useVertexDataList[i].Add(((CanvasVertexData)Enum.ToObject(typeof(CanvasVertexData), i)).ToString());
            }
            var useVertexColorDataList = new List<List<string>>();
            for (int i = 0; i < Enum.GetValues(typeof(CanvasVertexColorData)).Length; i++)
            {
                useVertexColorDataList.Add(new List<string>());
                useVertexColorDataList[i].Add(((CanvasVertexColorData)Enum.ToObject(typeof(CanvasVertexColorData), i)).ToString());
            }


            // MARK: ViewOptions
            foreach (var option in _options)
            {
                //Shader変更時の不正データを修正
                option.VaridateCustomData();
                //Inspector表示
                option.OptionGUI();
                //CustomDataの使用状況を収集
                option.CollectCustomData(ref useVertexDataList);
                option.CollectCustomColorData(ref useVertexColorDataList);
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
                            for (int i = 1; i < Enum.GetValues(typeof(CanvasVertexData)).Length; i++)
                            {
                                if (useVertexDataList[i].Count > 1)
                                {
                                    EditorGUILayout.LabelField("Float Data", EditorStyles.wordWrappedLabel);
                                    break;
                                }
                            }

                            using (new EditorGUI.IndentLevelScope(2))
                            {
                                for (int i = 1; i < Enum.GetValues(typeof(CanvasVertexData)).Length; i++)
                                {
                                    if (useVertexDataList[i].Count > 1)
                                    {
                                        useVertexDataList[i].RemoveAt(0);
                                        EditorGUILayout.LabelField(((CanvasVertexData)Enum.ToObject(typeof(CanvasVertexData), i)).ToString() + ":", String.Join(" / ", useVertexDataList[i]), EditorStyles.wordWrappedLabel);
                                    }
                                }
                            }

                            for (int i = 1; i < Enum.GetValues(typeof(CanvasVertexColorData)).Length; i++)
                            {
                                if (useVertexColorDataList[i].Count > 1)
                                {
                                    EditorGUILayout.LabelField("Color Data", EditorStyles.wordWrappedLabel);
                                    break;
                                }
                            }

                            using (new EditorGUI.IndentLevelScope(2))
                            {
                                for (int i = 1; i < Enum.GetValues(typeof(CanvasVertexColorData)).Length; i++)
                                {
                                    if (useVertexColorDataList[i].Count > 1)
                                    {
                                        useVertexColorDataList[i].RemoveAt(0);
                                        EditorGUILayout.LabelField(((CanvasVertexColorData)Enum.ToObject(typeof(CanvasVertexColorData), i)).ToString() + ":", String.Join(" / ", useVertexColorDataList[i]), EditorStyles.wordWrappedLabel);
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