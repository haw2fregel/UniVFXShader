using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


namespace UniVFX.Editor
{
    public class MaskTexture : UniVFXOption
    {
        static bool _viewGUI = false;
        static bool _viewUVGUI = false;
        static bool _viewTargetGUI = false;
        protected const string _IsActive = "_MASKTEXTURE";
        protected const string _Tex = "_MaskTex";
        protected const string _UV = "_MaskUV";
        protected const string _Offset = "_MaskOffset";
        protected const string _Repeat = "_MaskRepeat";
        public const string _TargetMainTex = "_MainTexMask";
        public const string _TargetBlendTex = "_BlendTexMask";
        public const string _TargetGradation = "_GradationMask";
        public const string _TargetDistortionTex = "_DistortionMask";
        public const string _TargetDissolveTex = "_DissolveMask";
        public const string _TargetSurfaceFade = "_SurfaceFadeMask";
        public const string _TargetHSVShift = "_HSVShiftMask";
        public const string _TargetFakeLight = "_FakeLightMask";
        public const string _TargetParallax = "_ParallaxMask";
        public const string _ResultValue = "maskResult";

        public MaskTexture(bool isCanvas, bool isBRP) : base(isCanvas, isBRP)
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
                    _viewGUI = EditorGUILayout.Foldout(_viewGUI, "Mask Texture");
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
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Offset, "Value Offset", -2, 2);
                                    UniVFXGUILayout.OptionBoolField(ref _mat, _Repeat, "Repeat");
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
                                            var gradation = UniVFXGUILayout.OptionBoolField(ref _mat, _TargetGradation, "Gradation");
                                            var distortion = UniVFXGUILayout.OptionBoolField(ref _mat, _TargetDistortionTex, "Distortion");
                                            var dissolve = UniVFXGUILayout.OptionBoolField(ref _mat, _TargetDissolveTex, "Dissolve");
                                            var surfafeFade = UniVFXGUILayout.OptionBoolField(ref _mat, _TargetSurfaceFade, "Surface Fade");
                                            var hsv = UniVFXGUILayout.OptionBoolField(ref _mat, _TargetHSVShift, "HSV Shift");
                                            var fakeLight = UniVFXGUILayout.OptionBoolField(ref _mat, _TargetFakeLight, "Fake Light");
                                            var parallax = UniVFXGUILayout.OptionBoolField(ref _mat, _TargetParallax, "Parallax");

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

                                            if (distortion && !UVDistortion.IsActive(_mat))
                                            {
                                                var rect = EditorGUILayout.GetControlRect();
                                                rect.height = 40;
                                                rect.xMin += 30;
                                                EditorGUI.HelpBox(rect, "Please Distortion Active", MessageType.Warning);
                                                rect.xMin = rect.xMax - 70;
                                                if (GUI.Button(rect, "Fix now"))
                                                    UVDistortion.SetActive(_mat, true);
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

                                            if (surfafeFade && !SurfaceFade.IsActive(_mat))
                                            {
                                                var rect = EditorGUILayout.GetControlRect();
                                                rect.height = 40;
                                                rect.xMin += 30;
                                                EditorGUI.HelpBox(rect, "Please SurfaceFade Active", MessageType.Warning);
                                                rect.xMin = rect.xMax - 70;
                                                if (GUI.Button(rect, "Fix now"))
                                                    SurfaceFade.SetActive(_mat, true);
                                                EditorGUILayout.GetControlRect();
                                            }

                                            if (hsv && !HSVShift.IsActive(_mat))
                                            {
                                                var rect = EditorGUILayout.GetControlRect();
                                                rect.height = 40;
                                                rect.xMin += 30;
                                                EditorGUI.HelpBox(rect, "Please HSVShift Active", MessageType.Warning);
                                                rect.xMin = rect.xMax - 70;
                                                if (GUI.Button(rect, "Fix now"))
                                                    HSVShift.SetActive(_mat, true);
                                                EditorGUILayout.GetControlRect();
                                            }

                                            if (fakeLight && !FakeLight.IsActive(_mat))
                                            {
                                                var rect = EditorGUILayout.GetControlRect();
                                                rect.height = 40;
                                                rect.xMin += 30;
                                                EditorGUI.HelpBox(rect, "Please FakeLight Active", MessageType.Warning);
                                                rect.xMin = rect.xMax - 70;
                                                if (GUI.Button(rect, "Fix now"))
                                                    FakeLight.SetActive(_mat, true);
                                                EditorGUILayout.GetControlRect();
                                            }

                                            if (parallax && !UVParallax.IsActive(_mat))
                                            {
                                                var rect = EditorGUILayout.GetControlRect();
                                                rect.height = 40;
                                                rect.xMin += 30;
                                                EditorGUI.HelpBox(rect, "Please UVParallax Active", MessageType.Warning);
                                                rect.xMin = rect.xMax - 70;
                                                if (GUI.Button(rect, "Fix now"))
                                                    UVParallax.SetActive(_mat, true);
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
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").x].Add("MaskUV Tile X");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").y].Add("MaskUV Tile Y");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").z].Add("MaskUV Offset X");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").w].Add("MaskUV Offset Y");
            useCustomDataList[(int)_mat.GetFloat(_Offset + "_Data")].Add("Mask Offset");
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
            if (!IsActive())
                return;

            UniVFXGUILayout.VaridateCustomDataVector(ref _mat, _UV + "Transform", _isCanvas);
            UniVFXGUILayout.VaridateArrayIndex(ref _mat, _UV + "Transform_Index", UniVFXGUILayout._UVChannelOption);
            UniVFXGUILayout.VaridateCustomDataInt(ref _mat, _Offset, _isCanvas);
        }

        public override List<string> GetPropertyCode(RefactOption refactOption)
        {
            var code = new List<string>();
            if (!IsActive())
                return code;

            var isInvalid = _mat.GetFloat(_TargetBlendTex) == 0 && _mat.GetFloat(_TargetGradation) == 0 && _mat.GetFloat(_TargetDistortionTex) == 0 && _mat.GetFloat(_TargetDissolveTex) == 0
                && _mat.GetFloat(_TargetSurfaceFade) == 0 && _mat.GetFloat(_TargetHSVShift) == 0 && _mat.GetFloat(_TargetFakeLight) == 0 && _mat.GetFloat(_TargetParallax) == 0;
            var isFixedValue = refactOption.HasFlag(RefactOption.PropertiesToFixedValue);
            var isTextureNone = _mat.GetTexture(_Tex) == null && refactOption.HasFlag(RefactOption.NoneTextureToFixedValue);
            

            if(isInvalid)
            {
                code.Add("//MaskTex Skipped, Target None");
                return code;
            }

            if (isTextureNone)
            {
                code.Add("//MaskTex Skipped, Texture is null");
            }else
            {
                code.Add("[NoScaleOffset]" + _Tex + "(\"" + _Tex.Replace("_", "") + "\", 2D) = \"white\" {}");
            }

            if(isFixedValue)
            {
                code.Add("//MaskTex Property Skipped, FixedValue");
            }
            else
            {
                if(_mat.GetInt(_Offset + "_Data") == 0)
                    code.Add(_Offset + "(\"" + _Offset.Replace("_", "") + "\", float) = 0");
                
                if (isTextureNone)
                {
                    code.Add("//MaskTex Transform Skipped, Texture is null");
                }
                else
                {
                    if(_mat.GetVector(_UV + "Transform_Data") == new Vector4(0,0,0,0))
                        code.Add(_UV + "Transform(\"" + _UV.Replace("_", "") + "Transform\", Vector) = (0,0,1,1)");
                }
            }

            return code;
        }
        public override List<string> GetCBufferCode(RefactOption refactOption)
        {
            var code = new List<string>();
            if (!IsActive())
                return code;

            var isInvalid = _mat.GetFloat(_TargetBlendTex) == 0 && _mat.GetFloat(_TargetGradation) == 0 && _mat.GetFloat(_TargetDistortionTex) == 0 && _mat.GetFloat(_TargetDissolveTex) == 0
                && _mat.GetFloat(_TargetSurfaceFade) == 0 && _mat.GetFloat(_TargetHSVShift) == 0 && _mat.GetFloat(_TargetFakeLight) == 0 && _mat.GetFloat(_TargetParallax) == 0;
            var isFixedValue = refactOption.HasFlag(RefactOption.PropertiesToFixedValue);
            var isTextureNone = _mat.GetTexture(_Tex) == null && refactOption.HasFlag(RefactOption.NoneTextureToFixedValue);
            
            if(isInvalid)
            {
                code.Add("//MaskTex Skipped, Target None");
                return code;
            }

            if(isFixedValue)
            {
                code.Add("//MaskTex Property Skipped, FixedValue");
            }
            else
            {
                if (isTextureNone)
                {
                    code.Add("//MaskTex Transform Skipped, Texture is null");
                }else
                {
                    if(_mat.GetVector(_UV + "Transform_Data") == new Vector4(0,0,0,0))
                        code.Add("float4 " + _UV + "Transform;");
                }

                if(_mat.GetInt(_Offset + "_Data") == 0)
                    code.Add("half " + _Offset + ";");
            }

            return code;
        }
        public override List<string> GetTextureCode(RefactOption refactOption)
        {
            var code = new List<string>();
            if (!IsActive())
                return code;

            var isInvalid = _mat.GetFloat(_TargetBlendTex) == 0 && _mat.GetFloat(_TargetGradation) == 0 && _mat.GetFloat(_TargetDistortionTex) == 0 && _mat.GetFloat(_TargetDissolveTex) == 0
                && _mat.GetFloat(_TargetSurfaceFade) == 0 && _mat.GetFloat(_TargetHSVShift) == 0 && _mat.GetFloat(_TargetFakeLight) == 0 && _mat.GetFloat(_TargetParallax) == 0;
            var isTextureNone = _mat.GetTexture(_Tex) == null && refactOption.HasFlag(RefactOption.NoneTextureToFixedValue);


            if(isInvalid)
            {
                code.Add("//MaskTex Skipped, Target None");
                return code;
            }

            if (isTextureNone)
            {
                code.Add("//MaskTex Skipped, Texture is null");
            }
            else
            {
                code.Add("TEXTURE2D(" + _Tex + ");");
            }

            return code;
        }
        public override List<string> GetUseV2fCode(RefactOption refactOption)
        {
            var code = new List<string>();
            if (!IsActive())
                return code;
            
            var isInvalid = _mat.GetFloat(_TargetBlendTex) == 0 && _mat.GetFloat(_TargetGradation) == 0 && _mat.GetFloat(_TargetDistortionTex) == 0 && _mat.GetFloat(_TargetDissolveTex) == 0
                && _mat.GetFloat(_TargetSurfaceFade) == 0 && _mat.GetFloat(_TargetHSVShift) == 0 && _mat.GetFloat(_TargetFakeLight) == 0 && _mat.GetFloat(_TargetParallax) == 0;
            var isTextureNone = _mat.GetTexture(_Tex) == null && refactOption.HasFlag(RefactOption.NoneTextureToFixedValue);

            if(isInvalid)
            {
                code.Add("//MaskTex Skipped, Target None");
                return code;
            }

            if (isTextureNone)
            {
                code.Add("//MaskTex Skipped, Texture is null");
            }
            else
            {
                code.Add("float2 uv_" + _Tex.Replace("_", ""));
            }
            
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

            var isInvalid = _mat.GetFloat(_TargetBlendTex) == 0 && _mat.GetFloat(_TargetGradation) == 0 && _mat.GetFloat(_TargetDistortionTex) == 0 && _mat.GetFloat(_TargetDissolveTex) == 0
                && _mat.GetFloat(_TargetSurfaceFade) == 0 && _mat.GetFloat(_TargetHSVShift) == 0 && _mat.GetFloat(_TargetFakeLight) == 0 && _mat.GetFloat(_TargetParallax) == 0;
            var isTextureNone = _mat.GetTexture(_Tex) == null && refactOption.HasFlag(RefactOption.NoneTextureToFixedValue);

            var uvName = UniVFXGUILayout.GetVertUVName(_mat.GetInt(_UV + "Transform_Index"), _mat);
            var transform = "st_" + _Tex.Replace("_", "");
            var transformX = UniVFXGUILayout.VertexDataToVectorCode(_mat, _UV + "Transform", 0, _isCanvas, refactOption);
            var transformY = UniVFXGUILayout.VertexDataToVectorCode(_mat, _UV + "Transform", 1, _isCanvas, refactOption);
            var transformZ = UniVFXGUILayout.VertexDataToVectorCode(_mat, _UV + "Transform", 2, _isCanvas, refactOption);
            var transformW = UniVFXGUILayout.VertexDataToVectorCode(_mat, _UV + "Transform", 3, _isCanvas, refactOption);

            code.Add("//MaskTex");

            if(isInvalid)
            {
                code.Add("//MaskTex Skipped, Target None");
                return code;
            }

            if (isTextureNone)
            {
                code.Add("//MaskTex Skipped, Texture is null");
            }
            else
            {
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
            }

            code.Add("");
            return code;
        }
        public override List<string> GetFragmentHeadCode(RefactOption refactOption)
        {
            var code = new List<string>();
            if (!IsActive())
                return code;

            var isInvalid = _mat.GetFloat(_TargetBlendTex) == 0 && _mat.GetFloat(_TargetGradation) == 0 && _mat.GetFloat(_TargetDistortionTex) == 0 && _mat.GetFloat(_TargetDissolveTex) == 0
                && _mat.GetFloat(_TargetSurfaceFade) == 0 && _mat.GetFloat(_TargetHSVShift) == 0 && _mat.GetFloat(_TargetFakeLight) == 0 && _mat.GetFloat(_TargetParallax) == 0;
            var isTextureNone = _mat.GetTexture(_Tex) == null && refactOption.HasFlag(RefactOption.NoneTextureToFixedValue);
             
            var sampler = UniVFXGUILayout.GetSamplerName(_mat.GetInt(_UV + "Transform_Sampler"));
            var offset = UniVFXGUILayout.VertexDataToFloatCode(_mat, _Offset, _isCanvas, refactOption);
           
            var uv = "uv_" + _Tex.Replace("_", "");
            var tex = "tex_" + _Tex.Replace("_", "");

            code.Add("//MaskTex");

            if(isInvalid)
            {
                code.Add("//MaskTex Skipped, Target None");
                return code;
            }

            if (isTextureNone)
            {
                code.Add("half4 " + tex + " = half4(1,1,1,1);");
                code.Add("//MaskTex Skipped, Texture is null");
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
                code.Add("half " + tex + " = SAMPLE_TEXTURE2D(" + _Tex + ", " + sampler + ", " + uv + ").x;");
            }
            if(offset != "0")
                code.Add(tex + " += " + offset + ";");
            if (_mat.GetInt(_Repeat) == 1)
            {
                code.Add("half maskMinus = " + tex + " < 0 ? -1 : 0;");
                code.Add("half maskRepeat = abs((" + tex + " + maskMinus)) % 2 >= 1 ? 1 - frac(" + tex + ") : frac(" + tex + ");");
                code.Add("half " + _ResultValue + " = maskRepeat;");
            }
            else
            {
                code.Add("half " + _ResultValue + " = saturate(" + tex + ");");
            }
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
