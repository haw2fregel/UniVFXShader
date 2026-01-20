using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


namespace UniVFX.Editor
{
    public class UVDistortion : UniVFXOption
    {
        static bool _viewGUI = false;
        static bool _viewUVGUI = false;
        static bool _viewTargetGUI = false;
        protected const string _IsActive = "_DISTORTIONENABLE";
        protected const string _Tex = "_DistortionTex";
        protected const string _UV = "_DistortionUV";
        protected const string _Intensity = "_DistortionIntensity";
        public const string _TargetMainTex = "_MainUVDistortion";
        public const string _TargetBlendTex = "_BlendUVDistortion";
        public const string _TargetDissolveTex = "_DissolveUVDistortion";
        public const string _TargetGradation = "_GradationUVDistortion";
        public const string _ResultValue = "distortionResult";

        public UVDistortion(bool isCanvas, bool isBRP) : base(isCanvas, isBRP)
        {
        }

        public override bool IsActive()
        {
            return _mat.GetInt(_IsActive) == 1;
        }

        public static bool IsActive(Material mat)
        {
            return mat.GetInt(_IsActive) == 1;
        }

        public override void SetActive(bool active)
        {
            _mat.SetInt(_IsActive, active ? 1 : 0);
        }

        public static void SetActive(Material mat, bool active)
        {
            mat.SetInt(_IsActive, active ? 1 : 0);
        }


        public override int HeatValue()
        {
            return 6;
        }

        public override void GetHeatValue(ref int value, ref int max)
        {
            max += HeatValue();
            if (IsActive())
                value += HeatValue();
        }

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
                    _viewGUI = EditorGUILayout.Foldout(_viewGUI, "UV Distortion");
                    if (_viewGUI)
                    {
                        var active = UniVFXGUILayout.OptionBoolField(ref _mat, _IsActive, "Active");
                        if (active)
                        {
                            GUI.color = new Color(0f, 0f, 0f, 0.6f);
                            using (new EditorGUILayout.VerticalScope("Box"))
                            {
                                using (new EditorGUI.IndentLevelScope())
                                {
                                    GUI.color = new Color(1f, 1f, 1f, 1f);
                                    UniVFXGUILayout.OptionTextureField(ref _mat, _Tex, "Texture");
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Intensity, "Intensity", -1, 1);
                                    UniVFXGUILayout.UVGUILayout(ref _mat, ref _viewUVGUI, _UV, _isCanvas);
                                    _viewTargetGUI = EditorGUILayout.Foldout(_viewTargetGUI, "Target");
                                    if (_viewTargetGUI)
                                    {
                                        GUI.color = new Color(0f, 0f, 0f, 0.8f);
                                        using (new EditorGUILayout.VerticalScope("Box"))
                                        {
                                            GUI.color = new Color(1f, 1f, 1f, 1.0f);
                                            UniVFXGUILayout.OptionBoolField(ref _mat, _TargetMainTex, "Main Texture");
                                            var blend = UniVFXGUILayout.OptionBoolField(ref _mat, _TargetBlendTex, "Blend Texture");
                                            var dissolve = UniVFXGUILayout.OptionBoolField(ref _mat, _TargetDissolveTex, "Dissolve Texture");
                                            var gradation = UniVFXGUILayout.OptionBoolField(ref _mat, _TargetGradation, "Gradation");

                                            if (blend && !BlendTexture.IsActive(_mat))
                                            {
                                                var rect = EditorGUILayout.GetControlRect();
                                                rect.height = 40;
                                                rect.xMin += 30;
                                                EditorGUI.HelpBox(rect, "Please BlendTexture Active", MessageType.Warning);
                                                rect.xMin = rect.xMax - 70;
                                                if (GUI.Button(rect, "Fix now"))
                                                    BlendTexture.SetActive(_mat, true);
                                                EditorGUILayout.GetControlRect();
                                            }

                                            if (dissolve && !Dissolve.IsActive(_mat))
                                            {
                                                var rect = EditorGUILayout.GetControlRect();
                                                rect.height = 40;
                                                rect.xMin += 30;
                                                EditorGUI.HelpBox(rect, "Please Dissolve Active", MessageType.Warning);
                                                rect.xMin = rect.xMax - 70;
                                                if (GUI.Button(rect, "Fix now"))
                                                    Dissolve.SetActive(_mat, true);
                                                EditorGUILayout.GetControlRect();
                                            }

                                            if (gradation && !GradationColor.IsActive(_mat))
                                            {
                                                var rect = EditorGUILayout.GetControlRect();
                                                rect.height = 40;
                                                rect.xMin += 30;
                                                EditorGUI.HelpBox(rect, "Please GradationColor Active", MessageType.Warning);
                                                rect.xMin = rect.xMax - 70;
                                                if (GUI.Button(rect, "Fix now"))
                                                    GradationColor.SetActive(_mat, true);
                                                EditorGUILayout.GetControlRect();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            GUI.color = new Color(1f, 1f, 1f, 1f);
        }

        public override void CollectCustomData(ref List<List<string>> useCustomDataList)
        {
            if (!IsActive())
                return;
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").x].Add("DistortionUV Tile X");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").y].Add("DistortionUV Tile Y");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").z].Add("DistortionUV Offset X");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").w].Add("DistortionUV Offset Y");
            useCustomDataList[(int)_mat.GetFloat(_Intensity + "_Data")].Add("Distortion Intensity");
        }

        public override void CollectCustomColorData(ref List<List<string>> useCustomDataList)
        {

        }

        public override void CollectUVChannel(ref List<List<string>> useUVChannelList)
        {
            if (!IsActive())
                return;
            useUVChannelList[_mat.GetInt(_UV + "Transform_Index")].Add(_Tex);
        }

        public override void VaridateCustomData()
        {
            UniVFXGUILayout.VaridateCustomDataInt(ref _mat, _Intensity, _isCanvas);
            UniVFXGUILayout.VaridateCustomDataVector(ref _mat, _UV + "Transform", _isCanvas);
            UniVFXGUILayout.VaridateArrayIndex(ref _mat, _UV + "Transform_Index", UniVFXGUILayout._UVChannelOption);
        }

        public override List<string> GetPropertyCode(RefactOption refactOption)
        {
            var code = new List<string>();
            if (!IsActive())
                return code;
            
            var intensity = UniVFXGUILayout.VertexDataToFloatCode(_mat, _Intensity, _isCanvas, refactOption);
            if(intensity == "0")
            {
                code.Add("//UV Distortion Skipped, Intensity is 0");
                return code;
            }

            if (_mat.GetTexture(_Tex) == null)
            {
                code.Add("//DistortionTex Skipped, Texture is null");
                return code;
            }
            
            code.Add("[NoScaleOffset]" + _Tex + "(\"" + _Tex.Replace("_", "") + "\", 2D) = \"white\" {}");

            return code;
        }
        public override List<string> GetCBufferCode(RefactOption refactOption)
        {
            var code = new List<string>();
            if (!IsActive())
                return code;
            return code;
        }
        public override List<string> GetTextureCode(RefactOption refactOption)
        {
            var code = new List<string>();
            if (!IsActive())
                return code;
            
            var intensity = UniVFXGUILayout.VertexDataToFloatCode(_mat, _Intensity, _isCanvas, refactOption);
            if(intensity == "0")
            {
                code.Add("//UV Distortion Skipped, Intensity is 0");
                return code;
            }

            if (_mat.GetTexture(_Tex) == null)
            {
                code.Add("//DistortionTex Skipped, Texture is null");
                return code;
            }
            
            code.Add("TEXTURE2D(" + _Tex + ");");
            return code;
        }
        public override List<string> GetUseV2fCode(RefactOption refactOption)
        {
            var code = new List<string>();
            if (!IsActive())
                return code;
            
            var intensity = UniVFXGUILayout.VertexDataToFloatCode(_mat, _Intensity, _isCanvas, refactOption);
            if(intensity == "0")
            {
                code.Add("//UV Distortion Skipped, Intensity is 0");
                return code;
            }

            if (_mat.GetTexture(_Tex) == null)
            {
                code.Add("//DistortionTex Skipped, Texture is null");
                return code;
            }
            
            code.Add("float2 uv_" + _Tex.Replace("_", ""));
            return code;
        }
        public override List<string> GetVertexHeadCode(RefactOption refactOption)
        {
            var code = new List<string>();
            return code;
        }
        public override List<string> GetVertexCode(RefactOption refactOption)
        {
            var code = new List<string>();
            if (!IsActive())
                return code;

            var uvName = UniVFXGUILayout.GetVertUVName(_mat.GetInt(_UV + "Transform_Index"), _mat);
            var transform = "st_" + _Tex.Replace("_", "");
            var transformX = UniVFXGUILayout.VertexDataToVectorCode(_mat, _UV + "Transform", 0, _isCanvas, refactOption);
            var transformY = UniVFXGUILayout.VertexDataToVectorCode(_mat, _UV + "Transform", 1, _isCanvas, refactOption);
            var transformZ = UniVFXGUILayout.VertexDataToVectorCode(_mat, _UV + "Transform", 2, _isCanvas, refactOption);
            var transformW = UniVFXGUILayout.VertexDataToVectorCode(_mat, _UV + "Transform", 3, _isCanvas, refactOption);

            code.Add("//Distortion");
            var intensity =UniVFXGUILayout.VertexDataToFloatCode(_mat, _Intensity, _isCanvas, refactOption);
            if(intensity == "0")
            {
                code.Add("//UV Distortion Skipped, Intensity is 0");
                code.Add("");
                return code;
            }

            if (_mat.GetTexture(_Tex) == null)
            {
                code.Add("//DistortionTex Skipped, Texture is null");
                code.Add("");
                return code;
            }

            var uv = "o.uv_" + _Tex.Replace("_", "");
            code.Add(uv + " = " + uvName + ";");
            if(UVBend.IsActive(_mat) && UniVFXGUILayout._UVChannelOption[_mat.GetInt(_UV + "Transform_Index")] == "BendUV" && _mat.GetInt(UVBend._Polar) == 1)
            {
                code.Add("");
                return code;
            }

            if (UVBend.IsActive(_mat) && UniVFXGUILayout._UVChannelOption[_mat.GetInt(_UV + "Transform_Index")] == "BendUV")
            {
                UVBend.ApplyBendCode(ref code, _mat, uv, _isCanvas, refactOption);
            }

            if(transformX != "1" || transformY != "1" || transformZ != "0" || transformW != "0")
            {
                code.Add("float4 " + transform + " = float4(" + transformX + ", " + transformY + ", " + transformZ + ", " + transformW + ");");
                code.Add(uv + " = (" + uv + " - float2(0.5, 0.5)) * " + transform + ".xy + float2(0.5, 0.5) + " + transform + ".zw;");
            }

            code.Add("");
            return code;
        }
        public override List<string> GetFragmentHeadCode(RefactOption refactOption)
        {
            var code = new List<string>();
            if (!IsActive())
                return code;
            
            var sampler = UniVFXGUILayout.GetSamplerName(_mat.GetInt(_UV + "Transform_Sampler"));
            var intensity = UniVFXGUILayout.VertexDataToFloatCode(_mat, _Intensity, _isCanvas, refactOption);
            var uv = "uv_" + _Tex.Replace("_", "");
            var tex = "tex_" + _Tex.Replace("_", "");

            code.Add("//Distortion");
            if(intensity == "0")
            {
                code.Add("float2 " + _ResultValue + " = 0;");
                code.Add("//UV Distortion Skipped, Intensity is 0");
                code.Add("");
                return code;
            }
            if (_mat.GetTexture(_Tex) == null)
            {
                code.Add("float4 " + tex + " = float4(0.5,0.5,0.5,0.5);");
                code.Add("//DistortionTex Skipped, Texture is null");
            }
            else
            {
                code.Add("float2 " + uv + " = i." + uv + ";");
                if (UVBend.IsActive(_mat) && UniVFXGUILayout._UVChannelOption[_mat.GetInt(_UV + "Transform_Index")] == "BendUV" && _mat.GetInt(UVBend._Polar) == 1)
                {
                    var transform = "st_" + _Tex.Replace("_", "");
                    var transformX = UniVFXGUILayout.VertexDataToVectorCode(_mat, _UV + "Transform", 0, _isCanvas, refactOption);
                    var transformY = UniVFXGUILayout.VertexDataToVectorCode(_mat, _UV + "Transform", 1, _isCanvas, refactOption);
                    var transformZ = UniVFXGUILayout.VertexDataToVectorCode(_mat, _UV + "Transform", 2, _isCanvas, refactOption);
                    var transformW = UniVFXGUILayout.VertexDataToVectorCode(_mat, _UV + "Transform", 3, _isCanvas, refactOption);

                    UVBend.ApplyBendPolarCode(ref code, uv);
                    UVBend.ApplyBendCode(ref code, _mat, uv, _isCanvas, refactOption);

                    if(transformX != "1" || transformY != "1" || transformZ != "0" || transformW != "0")
                    {
                        code.Add("float4 " + transform + " = float4(" + transformX + ", " + transformY + ", " + transformZ + ", " + transformW + ");");
                        code.Add(uv + " = (" + uv + " - float2(0.5, 0.5)) * " + transform + ".xy + float2(0.5, 0.5) + " + transform + ".zw;");
                    }

                }
                code.Add("half4 " + tex + " = SAMPLE_TEXTURE2D(" + _Tex + ", " + sampler + ", " + uv + ");");
            }
            code.Add(tex + ".xy -= float2(0.5, 0.5);");
            code.Add(tex + ".xy *= " + intensity + ";");
            if (MaskTexture.IsActive(_mat) && _mat.GetInt(MaskTexture._TargetDistortionTex) == 1)
                code.Add(tex + ".xy *= " + MaskTexture._ResultValue + ";");
            if (SurfaceFade.IsActive(_mat) && _mat.GetInt(SurfaceFade._TargetDistortion) == 1)
                code.Add(tex + ".xy *= 1 - " + SurfaceFade._ResultValue + ";");
            code.Add("float2 " + _ResultValue + " = " + tex + ".xy;");
            code.Add("");
            return code;
        }
        public override List<string> GetFragmentCode(RefactOption refactOption)
        {
            var code = new List<string>();
            return code;
        }


    }
}
