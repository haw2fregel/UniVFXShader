using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System;
using System.IO;


namespace UniVFX.Editor
{
    public class UniVFXCanvasInspector : ShaderGUI
    {
        bool _viewVertexData = false;
        static RefactOption _refactOption = (RefactOption)Enum.ToObject(typeof(RefactOption), -1);
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
                _options.Add(new MainTexture(true, false));
                _options.Add(new MaskTexture(true, false));
                _options.Add(new BlendTexture(true, false));
                _options.Add(new GradationColor(true, false));
                _options.Add(new Dissolve(true, false));
                _options.Add(new UVDistortion(true, false));
                _options.Add(new UVBend(true, false));
                _options.Add(new UVRotate(true, false));
                _options.Add(new UVFripBook(true, false));
                _options.Add(new HSVShift(true, false));
                _options.Add(new Time(true, false));
                _options.Add(new SurfaceOptionCanvas(true, false));
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

                GUILayout.Space(5);
                EditorGUILayout.LabelField("最適化", EditorStyles.boldLabel);
                _refactOption = (RefactOption)EditorGUILayout.EnumFlagsField("最適化設定", _refactOption);
                if (GUILayout.Button("専用シェーダーに変換"))
                {
                    ConvertUniqueShader(materialEditor);
                }
            }
            EditorUtility.SetDirty(material);
        }

        public void ConvertUniqueShader(MaterialEditor materialEditor)
        {
            Material material = materialEditor.target as Material;
            material.parent = null;
            var dir = AssetDatabase.GetAssetPath(material.shader);
            dir = System.IO.Path.GetDirectoryName(dir) + "/Canvas";
            var path = dir + "/" + material.name + ".shader";

            var useVertexDataList = new List<List<string>>();
            for (int i = 0; i < Enum.GetValues(typeof(CanvasVertexData)).Length; i++)
            {
                useVertexDataList.Add(new List<string>());
            }
            foreach (var option in _options)
            {
                option.CollectCustomData(ref useVertexDataList);
            }
            
            var useUVChannelList = new List<List<string>>();
            for (int i = 0; i < UniVFXGUILayout._CanvasUVChannelOption.Length; i++)
            {
                useUVChannelList.Add(new List<string>());
            }
            foreach (var option in _options)
            {
                option.CollectUVChannel(ref useUVChannelList);
            }

            

            var shaderCode = "";
            shaderCode += "Shader \"UniVFX/Canvas/" + material.name + " \" \n";
            shaderCode += "{\n";
            shaderCode += "    Properties\n";
            shaderCode += "    {\n";

            // Properties定義をここに追加
            foreach (var option in _options)
            {
                foreach (var code in option.GetPropertyCode(_refactOption))
                {
                    shaderCode += "        " + code + "\n";
                }
            }
            if (useVertexDataList[9].Count >= 1 || useVertexDataList[10].Count >= 1 || useVertexDataList[11].Count >= 1 || useVertexDataList[12].Count >= 1 ||
                useVertexDataList[13].Count >= 1 || useVertexDataList[14].Count >= 1 || useVertexDataList[15].Count >= 1 || useVertexDataList[16].Count >= 1 ||
                useVertexDataList[17].Count >= 1 || useVertexDataList[18].Count >= 1 || useVertexDataList[19].Count >= 1 || useVertexDataList[20].Count >= 1)
            {
                if (_refactOption.HasFlag(RefactOption.PropertiesToFixedValue))
                {
                    shaderCode += "                //Skipped TimeSpeed Property\n";
                }else
                {
                    shaderCode += "        " + Time._Speed + "(\"" + Time._Speed.Replace("_", "") + "\", float) = 0\n";
                }
            }
                
            if (useVertexDataList[17].Count >= 1 || useVertexDataList[18].Count >= 1 || useVertexDataList[19].Count >= 1 || useVertexDataList[20].Count >= 1)
                shaderCode += "        [NoScaleOffset]" + Time._Tex + "(\"" + Time._Tex.Replace("_", "") + "\", 2D) = \"white\" {}\n";
            shaderCode += "        [HideInInspector]_StencilComp (\"Stencil Comparison\", Float) = 8\n";
            shaderCode += "        [HideInInspector]_Stencil (\"Stencil ID\", Float) = 0\n";
            shaderCode += "        [HideInInspector]_StencilOp (\"Stencil Operation\", Float) = 0\n";
            shaderCode += "        [HideInInspector]_StencilWriteMask (\"Stencil Write Mask\", Float) = 255\n";
            shaderCode += "        [HideInInspector]_StencilReadMask (\"Stencil Read Mask\", Float) = 255\n";
            shaderCode += "        [HideInInspector]_ColorMask (\"ColorMask\", Float) = 15\n";
            shaderCode += "    }\n";
            shaderCode += "    SubShader\n";
            shaderCode += "    {\n";
            shaderCode += "        Tags { \"RenderPipeline\" = \"UniversalPipeline\" \"RenderType\" = \"Transparent\" \"UniversalMaterialType\" = \"Unlit\" \"Queue\" = \"Transparent\" }\n";
            shaderCode += "        Pass\n";
            shaderCode += "        {\n";
            shaderCode += "            Name \"Universal Forward\"\n";
            shaderCode += "            Cull Back\n";
            if(material.HasProperty(SurfaceOptionCanvas._SrcBlendAlpha) && material.HasProperty(SurfaceOptionCanvas._DstBlendAlpha))
            {
                shaderCode += "            Blend " + SurfaceOptionCanvas._BlendMode[material.GetInt(SurfaceOptionCanvas._SrcBlend)] + " " 
                                                + SurfaceOptionCanvas._BlendMode[material.GetInt(SurfaceOptionCanvas._DstBlend)] + ", "
                                                + SurfaceOptionCanvas._BlendMode[material.GetInt(SurfaceOptionCanvas._SrcBlendAlpha)] + " "
                                                + SurfaceOptionCanvas._BlendMode[material.GetInt(SurfaceOptionCanvas._DstBlendAlpha)] + "\n";
            }
            else
            {
                shaderCode += "            Blend " + SurfaceOptionCanvas._BlendMode[material.GetInt(SurfaceOptionCanvas._SrcBlend)] + " " 
                                                + SurfaceOptionCanvas._BlendMode[material.GetInt(SurfaceOptionCanvas._DstBlend)] + "\n";
            }
            shaderCode += "            ZTest [unity_GUIZTestMode]\n";
            shaderCode += "            ZWrite Off\n";
            shaderCode += "            ColorMask [_ColorMask]\n";
            shaderCode += "            Stencil\n";
            shaderCode += "            {\n";
            shaderCode += "                ReadMask [_StencilReadMask]\n";
            shaderCode += "                WriteMask [_StencilWriteMask]\n";
            shaderCode += "                Ref [_Stencil]\n";
            shaderCode += "                CompFront [_StencilComp]\n";
            shaderCode += "                PassFront [_StencilOp]\n";
            shaderCode += "                CompBack [_StencilComp]\n";
            shaderCode += "                PassBack [_StencilOp]\n";
            shaderCode += "            }\n";
            shaderCode += "\n";

            shaderCode += "            HLSLPROGRAM\n";
            shaderCode += "            #pragma vertex vert\n";
            shaderCode += "            #pragma fragment frag\n";
            shaderCode += "            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP\n";
            shaderCode += "            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT\n";
            shaderCode += "            #pragma multi_compile_local _ _ALPHATEST_ON\n";
            shaderCode += "            #include \"Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl\"\n";
            shaderCode += "            #include \"Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl\"\n";
            shaderCode += "            #include \"Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl\"\n";
            shaderCode += "\n";

            shaderCode += "            CBUFFER_START(UnityPerMaterial)\n";

            // CBUFFER定義をここに追加
            foreach (var option in _options)
            {
                foreach (var code in option.GetCBufferCode(_refactOption))
                {
                    shaderCode += "                " + code + "\n";
                }
            }

            if (useVertexDataList[9].Count >= 1 || useVertexDataList[10].Count >= 1 || useVertexDataList[11].Count >= 1 || useVertexDataList[12].Count >= 1 ||
                useVertexDataList[13].Count >= 1 || useVertexDataList[14].Count >= 1 || useVertexDataList[15].Count >= 1 || useVertexDataList[16].Count >= 1 ||
                useVertexDataList[17].Count >= 1 || useVertexDataList[18].Count >= 1 || useVertexDataList[19].Count >= 1 || useVertexDataList[20].Count >= 1)
            {
                if (_refactOption.HasFlag(RefactOption.PropertiesToFixedValue))
                {
                    shaderCode += "                //Skipped TimeSpeed Property\n";
                }else
                {
                    shaderCode += "                " + "float " + Time._Speed + ";\n";
                }
            }

            shaderCode += "            CBUFFER_END\n";
            shaderCode += "\n";
            shaderCode += "            float4 _ClipRect;\n";
            shaderCode += "            float _UIMaskSoftnessX;\n";
            shaderCode += "            float _UIMaskSoftnessY;\n";

            shaderCode += "            SAMPLER(SamplerState_Linear_Clamp);\n";
            shaderCode += "            SAMPLER(SamplerState_Linear_Repeat);\n";
            shaderCode += "            SAMPLER(SamplerState_Linear_Mirror);\n";
            shaderCode += "            SAMPLER(SamplerState_Linear_MirrorOnce);\n";
            if (useVertexDataList[17].Count >= 1 || useVertexDataList[18].Count >= 1 || useVertexDataList[19].Count >= 1 || useVertexDataList[20].Count >= 1)
                shaderCode += "            TEXTURE2D(" + Time._Tex + ");\n";

            // Texture定義をここに追加
            foreach (var option in _options)
            {
                foreach (var code in option.GetTextureCode(_refactOption))
                {
                    shaderCode += "            " + code + "\n";
                }
            }
            shaderCode += "\n";

            shaderCode += "            struct appdata\n";
            shaderCode += "            {\n";
            shaderCode += "                float4 vertex : POSITION;\n";
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
                foreach (var code in option.GetUseV2fCode(_refactOption))
                {
                    v2fCount++;
                    shaderCode += "                " + code + " : TEXCOORD" + v2fCount + ";\n";
                }
            }
            v2fCount++;
            shaderCode += "                #ifdef UNITY_UI_CLIP_RECT\n";
            shaderCode += "                    float4  mask : TEXCOORD" + v2fCount + ";\n";
            shaderCode += "                #endif\n";
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

            shaderCode += "                #ifdef UNITY_UI_CLIP_RECT\n";
            shaderCode += "                    float2 pixelSize = v.vertex.w;\n";
            shaderCode += "                    pixelSize /= float2(1, 1) * abs(mul((float2x2)UNITY_MATRIX_P, _ScreenParams.xy));\n";
            shaderCode += "                    float4 clampedRect = clamp(_ClipRect, -2e10, 2e10);\n";
            shaderCode += "                    float2 maskUV = (v.vertex.xy - clampedRect.xy) / (clampedRect.zw - clampedRect.xy);\n";
            shaderCode += "                    o.mask = float4(v.vertex.xy * 2 - clampedRect.xy - clampedRect.zw, 0.25 / (0.25 * half2(_UIMaskSoftnessX, _UIMaskSoftnessY) + abs(pixelSize.xy)));\n";
            shaderCode += "                #endif\n";
            shaderCode += "\n";

            if (useUVChannelList[2].Count >= 1)
            {
                shaderCode += "                float4 screenPos = ComputeScreenPos(TransformObjectToHClip(v.vertex.xyz));\n";
                shaderCode += "                screenPos.xy = screenPos.xy / screenPos.w;\n";
            }
            shaderCode += "\n";

            if (useVertexDataList[9].Count >= 1 || useVertexDataList[10].Count >= 1 || useVertexDataList[11].Count >= 1 || useVertexDataList[12].Count >= 1 ||
                useVertexDataList[13].Count >= 1 || useVertexDataList[14].Count >= 1 || useVertexDataList[15].Count >= 1 || useVertexDataList[16].Count >= 1 ||
                useVertexDataList[17].Count >= 1 || useVertexDataList[18].Count >= 1 || useVertexDataList[19].Count >= 1 || useVertexDataList[20].Count >= 1)
            {
                shaderCode += "                float4 time = _Time * " + (_refactOption.HasFlag(RefactOption.PropertiesToFixedValue) ? material.GetFloat(Time._Speed) : Time._Speed) + ";\n";
                if (useVertexDataList[17].Count >= 1 || useVertexDataList[18].Count >= 1 || useVertexDataList[19].Count >= 1 || useVertexDataList[20].Count >= 1)
                    shaderCode += "                float4 timeMap = SAMPLE_TEXTURE2D_LOD(" + Time._Tex + ", SamplerState_Linear_Clamp, frac(time.yy), 0);\n";
            }

            // 頂点シェーダー早期実行する処理をここに追加
            shaderCode += "//早期計算\n";
            foreach (var option in _options)
            {
                foreach (var code in option.GetVertexHeadCode(_refactOption))
                {
                    shaderCode += "                " + code + "\n";
                }
            }

            // 頂点シェーダー処理をここに追加
            shaderCode += "//メイン計算\n";
            foreach (var option in _options)
            {
                foreach (var code in option.GetVertexCode(_refactOption))
                {
                    shaderCode += "                " + code + "\n";
                }
            }

            shaderCode += "                o.positionCS = TransformObjectToHClip(v.vertex.xyz);\n";
            shaderCode += "                return o;\n";
            shaderCode += "            }\n";
            shaderCode += "\n";

            shaderCode += "            half4 frag (v2f i) : SV_Target\n";
            shaderCode += "            {\n";
            shaderCode += "                half4 col = half4(1,1,1,1);\n";
            shaderCode += "                float4 texCoord1 = i.texCoord1;\n";
            shaderCode += "                float4 texCoord2 = i.texCoord2;\n";
            shaderCode += "                half4 vertexColor = i.vertexColor;\n";
            shaderCode += "\n";

            if (useVertexDataList[9].Count >= 1 || useVertexDataList[10].Count >= 1 || useVertexDataList[11].Count >= 1 || useVertexDataList[12].Count >= 1 ||
                useVertexDataList[13].Count >= 1 || useVertexDataList[14].Count >= 1 || useVertexDataList[15].Count >= 1 || useVertexDataList[16].Count >= 1 ||
                useVertexDataList[17].Count >= 1 || useVertexDataList[18].Count >= 1 || useVertexDataList[19].Count >= 1 || useVertexDataList[20].Count >= 1)
            {
                shaderCode += "                float4 time = _Time * " + (_refactOption.HasFlag(RefactOption.PropertiesToFixedValue) ? material.GetFloat(Time._Speed) : Time._Speed) + ";\n";
                if (useVertexDataList[17].Count >= 1 || useVertexDataList[18].Count >= 1 || useVertexDataList[19].Count >= 1 || useVertexDataList[20].Count >= 1)
                    shaderCode += "                float4 timeMap = SAMPLE_TEXTURE2D(" + Time._Tex + ", SamplerState_Linear_Clamp, frac(time.yy));\n";
            }

            // フラグメントシェーダーの早期実行処理をここに追加
            shaderCode += "//早期計算\n";
            foreach (var option in _options)
            {
                foreach (var code in option.GetFragmentHeadCode(_refactOption))
                {
                    shaderCode += "                " + code + "\n";
                }
            }

            // フラグメントシェーダー処理をここに追加
            shaderCode += "//メイン計算\n";
            foreach (var option in _options)
            {
                foreach (var code in option.GetFragmentCode(_refactOption))
                {
                    shaderCode += "                " + code + "\n";
                }
            }

            

            shaderCode += "                #ifdef UNITY_UI_CLIP_RECT\n";
            shaderCode += "                    half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(i.mask.xy)) * i.mask.zw);\n";
            shaderCode += "                    col.a *= m.x * m.y;\n";
            shaderCode += "                #endif\n";

            if(material.IsKeywordEnabled(SurfaceOptionCanvas._AlphaClipActive))
            {
                shaderCode += "                    clip (col.a - " + material.GetFloat(SurfaceOptionCanvas._AlphaClip) + ");\n";
            }

            shaderCode += "                col.rgb *= col.a;\n";


            shaderCode += "                return col;\n";
            shaderCode += "            }\n";
            shaderCode += "\n";

            shaderCode += "            ENDHLSL\n";
            shaderCode += "        }\n";
            shaderCode += "    }\n";
            shaderCode += "    CustomEditor \"UniVFX.Editor.UniVFXUniqueCanvasInspector\"\n";
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