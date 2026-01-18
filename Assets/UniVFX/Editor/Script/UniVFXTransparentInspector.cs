using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System;
using System.IO;


namespace UniVFX.Editor
{
    public class UniVFXTransparentInspector : ShaderGUI
    {
        ParticleSystem _ps;
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
                _options.Add(new FaceColor());
                _options.Add(new Time());
                _options.Add(new SurfaceOptionTransparent());
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
                if (_ps != null)
                {
                    if (GUILayout.Button("Set CustomData Label"))
                    {
                        SetCustomDataLabel(useVertexDataList, useVertexColorDataList);
                    }
                }

                GUILayout.Space(5);
                EditorGUILayout.LabelField("最適化", EditorStyles.boldLabel);
                if (GUILayout.Button("専用シェーダーに変換"))
                {
                    ConvertUniqueShader(materialEditor);
                }

            }
            EditorUtility.SetDirty(material);
        }

        /// <summary>
        /// Materialの設定状況に応じて、ParticleSystemのCustomDataを設定する
        /// </summary>
        /// <param name="useVertexDataList"></param>
        /// <param name="useVertexColorDataList"></param>
        void SetCustomDataLabel(List<List<string>> useVertexDataList, List<List<string>> useVertexColorDataList)
        {
            var particleSystemSo = new SerializedObject(_ps);

            foreach (var item in useVertexDataList)
                item.RemoveAt(0);
            foreach (var item in useVertexColorDataList)
                item.RemoveAt(0);

            var isUseCustomVertex0 = useVertexDataList[(int)VertexData.TEXCOORD1X].Count + useVertexDataList[(int)VertexData.TEXCOORD1Y].Count + useVertexDataList[(int)VertexData.TEXCOORD1Z].Count + useVertexDataList[(int)VertexData.TEXCOORD1W].Count > 0;
            var isUseCustomColor0 = useVertexColorDataList[(int)VertexColorData.TEXCOORD1].Count > 0;
            var isUseCustomVertex1 = useVertexDataList[(int)VertexData.TEXCOORD2X].Count + useVertexDataList[(int)VertexData.TEXCOORD2Y].Count + useVertexDataList[(int)VertexData.TEXCOORD2Z].Count + useVertexDataList[(int)VertexData.TEXCOORD2W].Count > 0;
            var isUseCustomColor1 = useVertexColorDataList[(int)VertexColorData.TEXCOORD2].Count > 0;

            if (isUseCustomVertex0 || isUseCustomVertex1 || isUseCustomColor0 || isUseCustomColor1)
            {
                particleSystemSo.FindProperty("CustomDataModule.enabled").boolValue = true;
            }
            else
            {
                particleSystemSo.FindProperty("CustomDataModule.enabled").boolValue = false;
            }

            if (isUseCustomVertex0)
            {
                particleSystemSo.FindProperty("CustomDataModule.mode0").intValue = 1;

                if (useVertexDataList[(int)VertexData.TEXCOORD1X].Count > 0)
                    particleSystemSo.FindProperty("CustomDataModule.vectorComponentCount0").intValue = 1;
                if (useVertexDataList[(int)VertexData.TEXCOORD1Y].Count > 0)
                    particleSystemSo.FindProperty("CustomDataModule.vectorComponentCount0").intValue = 2;
                if (useVertexDataList[(int)VertexData.TEXCOORD1Z].Count > 0)
                    particleSystemSo.FindProperty("CustomDataModule.vectorComponentCount0").intValue = 3;
                if (useVertexDataList[(int)VertexData.TEXCOORD1W].Count > 0)
                    particleSystemSo.FindProperty("CustomDataModule.vectorComponentCount0").intValue = 4;
            }
            else
            {
                particleSystemSo.FindProperty("CustomDataModule.mode0").intValue = 0;
            }
            if (isUseCustomColor0)
                particleSystemSo.FindProperty("CustomDataModule.mode0").intValue = 2;

            if (isUseCustomVertex1)
            {
                particleSystemSo.FindProperty("CustomDataModule.mode1").intValue = 1;

                if (useVertexDataList[(int)VertexData.TEXCOORD2X].Count > 0)
                    particleSystemSo.FindProperty("CustomDataModule.vectorComponentCount1").intValue = 1;
                if (useVertexDataList[(int)VertexData.TEXCOORD2Y].Count > 0)
                    particleSystemSo.FindProperty("CustomDataModule.vectorComponentCount1").intValue = 2;
                if (useVertexDataList[(int)VertexData.TEXCOORD2Z].Count > 0)
                    particleSystemSo.FindProperty("CustomDataModule.vectorComponentCount1").intValue = 3;
                if (useVertexDataList[(int)VertexData.TEXCOORD2W].Count > 0)
                    particleSystemSo.FindProperty("CustomDataModule.vectorComponentCount1").intValue = 4;
            }
            else
            {
                particleSystemSo.FindProperty("CustomDataModule.mode1").intValue = 0;
            }
            if (isUseCustomColor1)
                particleSystemSo.FindProperty("CustomDataModule.mode1").intValue = 2;

            particleSystemSo.FindProperty("CustomDataModule.vectorLabel0_0").stringValue = string.Join("/", useVertexDataList[(int)VertexData.TEXCOORD1X]);
            particleSystemSo.FindProperty("CustomDataModule.vectorLabel0_1").stringValue = string.Join("/", useVertexDataList[(int)VertexData.TEXCOORD1Y]);
            particleSystemSo.FindProperty("CustomDataModule.vectorLabel0_2").stringValue = string.Join("/", useVertexDataList[(int)VertexData.TEXCOORD1Z]);
            particleSystemSo.FindProperty("CustomDataModule.vectorLabel0_3").stringValue = string.Join("/", useVertexDataList[(int)VertexData.TEXCOORD1W]);
            particleSystemSo.FindProperty("CustomDataModule.vectorLabel1_0").stringValue = string.Join("/", useVertexDataList[(int)VertexData.TEXCOORD2X]);
            particleSystemSo.FindProperty("CustomDataModule.vectorLabel1_1").stringValue = string.Join("/", useVertexDataList[(int)VertexData.TEXCOORD2Y]);
            particleSystemSo.FindProperty("CustomDataModule.vectorLabel1_2").stringValue = string.Join("/", useVertexDataList[(int)VertexData.TEXCOORD2Z]);
            particleSystemSo.FindProperty("CustomDataModule.vectorLabel1_3").stringValue = string.Join("/", useVertexDataList[(int)VertexData.TEXCOORD2W]);

            particleSystemSo.FindProperty("CustomDataModule.colorLabel0").stringValue = string.Join("/", useVertexColorDataList[(int)VertexColorData.TEXCOORD1]);
            particleSystemSo.FindProperty("CustomDataModule.colorLabel1").stringValue = string.Join("/", useVertexColorDataList[(int)VertexColorData.TEXCOORD2]);

            particleSystemSo.ApplyModifiedProperties();

        }

        override public void OnMaterialPreviewGUI(MaterialEditor materialEditor, Rect r, GUIStyle background)
        {
            if (Selection.activeGameObject != null)
                _ps = Selection.activeGameObject.GetComponent<ParticleSystem>();
             materialEditor.DefaultPreviewGUI(r, background);
        }

        override public void OnMaterialInteractivePreviewGUI(MaterialEditor materialEditor, Rect r, GUIStyle background)
        {
            if (Selection.activeGameObject != null)
                _ps = Selection.activeGameObject.GetComponent<ParticleSystem>();
            materialEditor.DefaultPreviewGUI(r, background);
        }


        public void ConvertUniqueShader(MaterialEditor materialEditor)
        {
            Material material = materialEditor.target as Material;
            var dir = AssetDatabase.GetAssetPath(material.shader);
            dir = System.IO.Path.GetDirectoryName(dir) + "/Transparent";
            var path = dir + "/" + material.name + ".shader";

            var useVertexDataList = new List<List<string>>();
            for (int i = 0; i < Enum.GetValues(typeof(VertexData)).Length; i++)
            {
                useVertexDataList.Add(new List<string>());
            }
            foreach (var option in _options)
            {
                option.CollectCustomData(ref useVertexDataList);
            }
            
            var useUVChannelList = new List<List<string>>();
            for (int i = 0; i < UniVFXGUILayout._UVChannelOption.Length; i++)
            {
                useUVChannelList.Add(new List<string>());
            }
            foreach (var option in _options)
            {
                option.CollectUVChannel(ref useUVChannelList);
            }

            

            var shaderCode = "";
            shaderCode += "Shader \"UniVFX/" + material.name + " \" \n";
            shaderCode += "{\n";
            shaderCode += "    Properties\n";
            shaderCode += "    {\n";

            // Properties定義をここに追加
            foreach (var option in _options)
            {
                foreach (var code in option.GetPropertyCode())
                {
                    shaderCode += "        " + code + "\n";
                }
            }
            if (useVertexDataList[13].Count >= 1 || useVertexDataList[14].Count >= 1 || useVertexDataList[15].Count >= 1 || useVertexDataList[16].Count >= 1 ||
                useVertexDataList[17].Count >= 1 || useVertexDataList[18].Count >= 1 || useVertexDataList[19].Count >= 1 || useVertexDataList[20].Count >= 1 ||
                useVertexDataList[21].Count >= 1 || useVertexDataList[22].Count >= 1 || useVertexDataList[23].Count >= 1 || useVertexDataList[24].Count >= 1)
                shaderCode += "        " + Time._Speed + "(\"" + Time._Speed.Replace("_", "") + "\", float) = 0\n";
            if (useVertexDataList[21].Count >= 1 || useVertexDataList[22].Count >= 1 || useVertexDataList[23].Count >= 1 || useVertexDataList[24].Count >= 1)
                shaderCode += "        [NoScaleOffset]" + Time._Tex + "(\"" + Time._Tex.Replace("_", "") + "\", 2D) = \"white\" {}\n";
            shaderCode += "    }\n";
            shaderCode += "    SubShader\n";
            shaderCode += "    {\n";
            shaderCode += "        Tags { \"RenderPipeline\" = \"UniversalPipeline\" \"RenderType\" = \"Transparent\" \"UniversalMaterialType\" = \"Unlit\" \"Queue\" = \"Transparent\" }\n";
            shaderCode += "        Pass\n";
            shaderCode += "        {\n";
            shaderCode += "            Name \"Universal Forward\"\n";
            shaderCode += "            Cull " + SurfaceOptionTransparent._CullMode[material.GetInt(SurfaceOptionTransparent._Cull)] + "\n";
            shaderCode += "            Blend " + SurfaceOptionTransparent._BlendMode[material.GetInt(SurfaceOptionTransparent._SrcBlend)] + " " 
                                            + SurfaceOptionTransparent._BlendMode[material.GetInt(SurfaceOptionTransparent._DstBlend)] + ", "
                                            + SurfaceOptionTransparent._BlendMode[material.GetInt(SurfaceOptionTransparent._SrcBlendAlpha)] + " "
                                            + SurfaceOptionTransparent._BlendMode[material.GetInt(SurfaceOptionTransparent._DstBlendAlpha)] + "\n";
            shaderCode += "            ZTest " + SurfaceOptionTransparent._ZTestMode[material.GetInt(SurfaceOptionTransparent._ZTest)] + "\n";
            shaderCode += "            ZWrite Off\n";
            shaderCode += "\n";

            shaderCode += "            HLSLPROGRAM\n";
            shaderCode += "            #pragma vertex vert\n";
            shaderCode += "            #pragma fragment frag\n";
            shaderCode += "            #include \"Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl\"\n";
            shaderCode += "            #include \"Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl\"\n";
            shaderCode += "            #include \"Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl\"\n";
            shaderCode += "\n";

            shaderCode += "            CBUFFER_START(UnityPerMaterial)\n";

            // CBUFFER定義をここに追加
            foreach (var option in _options)
            {
                foreach (var code in option.GetCBufferCode())
                {
                    shaderCode += "                " + code + "\n";
                }
            }

            if (useVertexDataList[13].Count >= 1 || useVertexDataList[14].Count >= 1 || useVertexDataList[15].Count >= 1 || useVertexDataList[16].Count >= 1 ||
                useVertexDataList[17].Count >= 1 || useVertexDataList[18].Count >= 1 || useVertexDataList[19].Count >= 1 || useVertexDataList[20].Count >= 1 ||
                useVertexDataList[21].Count >= 1 || useVertexDataList[22].Count >= 1 || useVertexDataList[23].Count >= 1 || useVertexDataList[24].Count >= 1)
                shaderCode += "                " + "float " + Time._Speed + ";\n";

            shaderCode += "            CBUFFER_END\n";
            shaderCode += "\n";
            shaderCode += "            SAMPLER(SamplerState_Linear_Clamp);\n";
            shaderCode += "            SAMPLER(SamplerState_Linear_Repeat);\n";
            shaderCode += "            SAMPLER(SamplerState_Linear_Mirror);\n";
            shaderCode += "            SAMPLER(SamplerState_Linear_MirrorOnce);\n";
            if (useVertexDataList[21].Count >= 1 || useVertexDataList[22].Count >= 1 || useVertexDataList[23].Count >= 1 || useVertexDataList[24].Count >= 1)
                shaderCode += "            TEXTURE2D(" + Time._Tex + ");\n";

            // Texture定義をここに追加
            foreach (var option in _options)
            {
                foreach (var code in option.GetTextureCode())
                {
                    shaderCode += "            " + code + "\n";
                }
            }
            shaderCode += "\n";

            shaderCode += "            struct appdata\n";
            shaderCode += "            {\n";
            shaderCode += "                float4 vertex : POSITION;\n";
            if(useUVChannelList[5].Count >= 1 || VertexAnimation.IsActive(material) || SurfaceFade.IsActive(material) || FakeLight.IsActive(material) || UVParallax.IsActive(material))
                shaderCode += "                float3 normal : NORMAL;\n";
            if(VertexAnimation.IsActive(material) || UVParallax.IsActive(material))            
                shaderCode += "                float4 tangent : TANGENT;\n";
            shaderCode += "                float4 texCoord0 : TEXCOORD0;\n";
            shaderCode += "                float4 texCoord1 : TEXCOORD1;\n";
            shaderCode += "                float4 texCoord2 : TEXCOORD2;\n";
            shaderCode += "                half4 vertexColor : COLOR;\n";
            shaderCode += "            };\n";
            shaderCode += "\n";


            shaderCode += "            struct v2f\n";
            shaderCode += "            {\n";
            shaderCode += "                float4 positionCS : SV_POSITION;\n";
            shaderCode += "                float4 texCoord0 : TEXCOORD0;\n";
            shaderCode += "                float4 texCoord1 : TEXCOORD1;\n";
            shaderCode += "                float4 texCoord2 : TEXCOORD2;\n";
            shaderCode += "                half4 vertexColor : COLOR;\n";
            var v2fCount = 2;
            foreach (var option in _options)
            {
                foreach (var code in option.GetUseV2fCode())
                {
                    v2fCount++;
                    shaderCode += "                " + code + " : TEXCOORD" + v2fCount + ";\n";
                }
            }
            if (SurfaceFade.IsActive(material) || FakeLight.IsActive(material) || UVParallax.IsActive(material))
            {
                v2fCount++;
                shaderCode += "                float3 worldPos : TEXCOORD" + v2fCount + ";\n";
            }
            if (SurfaceFade.IsActive(material) || FakeLight.IsActive(material) || UVParallax.IsActive(material))
            {
                v2fCount++;
                shaderCode += "                float3 normal : TEXCOORD" + v2fCount + ";\n";
            }
            if(UVParallax.IsActive(material))
            {
                v2fCount++;
                shaderCode += "                float4 tangent : TEXCOORD" + v2fCount + ";\n";
            }
            shaderCode += "            };\n";
            shaderCode += "\n";


            shaderCode += "            v2f vert (appdata v)\n";
            shaderCode += "            {\n";
            shaderCode += "                v2f o;\n";
            shaderCode += "\n";

            shaderCode += "                o.texCoord0 = v.texCoord0;\n";
            shaderCode += "                o.texCoord1 = v.texCoord1;\n";
            shaderCode += "                o.texCoord2 = v.texCoord2;\n";
            shaderCode += "                o.vertexColor = v.vertexColor;\n";
            shaderCode += "                float4 texCoord0 = v.texCoord0;\n";
            shaderCode += "                float4 texCoord1 = v.texCoord1;\n";
            shaderCode += "                float4 texCoord2 = v.texCoord2;\n";
            shaderCode += "                half4 vertexColor = v.vertexColor;\n";
            shaderCode += "\n";

            if (useUVChannelList[1].Count >= 1 || useUVChannelList[2].Count >= 1 || useUVChannelList[3].Count >= 1 || SurfaceFade.IsActive(material) || FakeLight.IsActive(material) || UVParallax.IsActive(material))
                shaderCode += "                float3 worldPos = TransformObjectToWorld(v.vertex.xyz);\n";
            if (useUVChannelList[4].Count >= 1)
            {
                shaderCode += "                float4 screenPos = ComputeScreenPos(TransformObjectToHClip(v.vertex.xyz));\n";
                shaderCode += "                screenPos.xy = screenPos.xy / screenPos.w;\n";
            }
            if (useUVChannelList[5].Count >= 1)
                shaderCode += "                float3 viewNormal = TransformWorldToViewDir(TransformObjectToWorldDir(v.normal.xyz));\n";
            if (SurfaceFade.IsActive(material) || FakeLight.IsActive(material) || UVParallax.IsActive(material))
                shaderCode += "                o.worldPos = worldPos;\n";
            if (SurfaceFade.IsActive(material) || FakeLight.IsActive(material) || UVParallax.IsActive(material))
                shaderCode += "                o.normal = v.normal;\n";
            if (UVParallax.IsActive(material))
                shaderCode += "                o.tangent = v.tangent;\n";
            shaderCode += "\n";

            if (useVertexDataList[13].Count >= 1 || useVertexDataList[14].Count >= 1 || useVertexDataList[15].Count >= 1 || useVertexDataList[16].Count >= 1 ||
                useVertexDataList[17].Count >= 1 || useVertexDataList[18].Count >= 1 || useVertexDataList[19].Count >= 1 || useVertexDataList[20].Count >= 1 ||
                useVertexDataList[21].Count >= 1 || useVertexDataList[22].Count >= 1 || useVertexDataList[23].Count >= 1 || useVertexDataList[24].Count >= 1)
            {
                shaderCode += "                float4 time = _Time * " + Time._Speed + ";\n";
                if (useVertexDataList[21].Count >= 1 || useVertexDataList[22].Count >= 1 || useVertexDataList[23].Count >= 1 || useVertexDataList[24].Count >= 1)
                    shaderCode += "                float4 timeMap = SAMPLE_TEXTURE2D(" + Time._Tex + ", SamplerState_Linear_Clamp, frac(time.yy));\n";
            }

            // 頂点シェーダー早期実行する処理をここに追加
            shaderCode += "//早期計算\n";
            foreach (var option in _options)
            {
                foreach (var code in option.GetVertexHeadCode())
                {
                    shaderCode += "                " + code + "\n";
                }
            }

            // 頂点シェーダー処理をここに追加
            shaderCode += "//メイン計算\n";
            foreach (var option in _options)
            {
                foreach (var code in option.GetVertexCode())
                {
                    shaderCode += "                " + code + "\n";
                }
            }

            shaderCode += "                o.positionCS = TransformObjectToHClip(v.vertex.xyz);\n";
            shaderCode += "                return o;\n";
            shaderCode += "            }\n";
            shaderCode += "\n";

            shaderCode += "            half4 frag (v2f i" + (FaceColor.IsActive(material) ? ", half face : VFACE" : "") + ") : SV_Target\n";
            shaderCode += "            {\n";
            shaderCode += "                half4 col = half4(1,1,1,1);\n";
            shaderCode += "                float4 texCoord1 = i.texCoord1;\n";
            shaderCode += "                float4 texCoord2 = i.texCoord2;\n";
            shaderCode += "                half4 vertexColor = i.vertexColor;\n";
            if (UVParallax.IsActive(material))
            {
                shaderCode += "                float4 tangentWS = float4(TransformObjectToWorldDir(i.tangent.xyz), i.tangent.w);\n";
                shaderCode += "                float3 worldNormal = (TransformObjectToWorldDir(i.normal));\n";
                shaderCode += "                float3 unnormalizedNormalWS = worldNormal;\n";
                shaderCode += "                float renormFactor = 1.0 / length(unnormalizedNormalWS);\n";
                shaderCode += "                worldNormal = normalize(unnormalizedNormalWS);\n";
                shaderCode += "                float crossSign = (tangentWS.w > 0.0 ? 1.0 : -1.0)* GetOddNegativeScale();\n";
                shaderCode += "                float3 bitang = crossSign * cross(worldNormal, tangentWS.xyz);\n";
                shaderCode += "                float3 worldSpaceNormal = renormFactor * worldNormal;\n";
                shaderCode += "                float3 worldSpaceTangent = renormFactor * tangentWS.xyz;\n";
                shaderCode += "                float3 worldSpaceBiTangent = renormFactor * bitang;\n";
                shaderCode += "                float3 worldSpaceViewDirection = GetWorldSpaceNormalizeViewDir(i.worldPos);\n";
                shaderCode += "                float3x3 tangentSpaceTransform = float3x3(worldSpaceTangent, worldSpaceBiTangent, worldSpaceNormal);\n";
                shaderCode += "                float3 tangentSpaceViewDirection = mul(tangentSpaceTransform, worldSpaceViewDirection);\n";
            }
            shaderCode += "\n";

            // フラグメントシェーダーの早期実行処理をここに追加
            shaderCode += "//早期計算\n";
            foreach (var option in _options)
            {
                foreach (var code in option.GetFragmentHeadCode())
                {
                    shaderCode += "                " + code + "\n";
                }
            }

            // フラグメントシェーダー処理をここに追加
            shaderCode += "//メイン計算\n";
            foreach (var option in _options)
            {
                foreach (var code in option.GetFragmentCode())
                {
                    shaderCode += "                " + code + "\n";
                }
            }

            if(material.GetInt(SurfaceOptionTransparent._ColorMultiplAlpha) == 1)
            {
                shaderCode += "                col.rgb *= col.a;\n";
            }


            shaderCode += "                return col;\n";
            shaderCode += "            }\n";
            shaderCode += "\n";

            shaderCode += "            ENDHLSL\n";
            shaderCode += "        }\n";
            shaderCode += "    }\n";
            shaderCode += "}\n";

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            File.WriteAllText(path, shaderCode);
            AssetDatabase.ImportAsset(path);

            var asset = AssetDatabase.LoadAssetAtPath<Shader>(path);
            material.shader = asset;
        }
    }
}