using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


namespace UniVFX.Editor
{
    public class MaskTexture : UniVFXOption
    {
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

        public override List<string> GetPropertyCode()
        {
            var code = new List<string>();
            if (!IsActive())
                return code;
            if (_mat.GetInt(_TargetMainTex) == 0 && _mat.GetInt(_TargetBlendTex) == 0 && _mat.GetInt(_TargetGradation) == 0 && _mat.GetInt(_TargetDistortionTex) == 0 && _mat.GetInt(_TargetDissolveTex) == 0
                && _mat.GetInt(_TargetSurfaceFade) == 0 && _mat.GetInt(_TargetHSVShift) == 0 && _mat.GetInt(_TargetFakeLight) == 0 && _mat.GetInt(_TargetParallax) == 0)
                return code;
             
            code.Add("[NoScaleOffset]" + _Tex + "(\"" + _Tex.Replace("_", "") + "\", 2D) = \"white\" {}");

            return code;
        }
        public override List<string> GetCBufferCode()
        {
            var code = new List<string>();
            if (!IsActive())
                return code;

            return code;
        }
        public override List<string> GetTextureCode()
        {
            var code = new List<string>();
            if (!IsActive())
                return code;
            if (_mat.GetInt(_TargetMainTex) == 0 && _mat.GetInt(_TargetBlendTex) == 0 && _mat.GetInt(_TargetGradation) == 0 && _mat.GetInt(_TargetDistortionTex) == 0 && _mat.GetInt(_TargetDissolveTex) == 0
                && _mat.GetInt(_TargetSurfaceFade) == 0 && _mat.GetInt(_TargetHSVShift) == 0 && _mat.GetInt(_TargetFakeLight) == 0 && _mat.GetInt(_TargetParallax) == 0)
                return code;
             
            code.Add("TEXTURE2D(" + _Tex + ");");
            return code;
        }
        public override List<string> GetUseV2fCode()
        {
            var code = new List<string>();
            if (!IsActive())
                return code;
            if (_mat.GetInt(_TargetMainTex) == 0 && _mat.GetInt(_TargetBlendTex) == 0 && _mat.GetInt(_TargetGradation) == 0 && _mat.GetInt(_TargetDistortionTex) == 0 && _mat.GetInt(_TargetDissolveTex) == 0
                && _mat.GetInt(_TargetSurfaceFade) == 0 && _mat.GetInt(_TargetHSVShift) == 0 && _mat.GetInt(_TargetFakeLight) == 0 && _mat.GetInt(_TargetParallax) == 0)
                return code;
             
            code.Add("float2 uv_" + _Tex.Replace("_", ""));
            return code;
        }
        public override List<string> GetVertexHeadCode()
        {
            var code = new List<string>();
            return code;
        }
        public override List<string> GetVertexCode()
        {
            var code = new List<string>();
            if (!IsActive())
                return code;
            if (_mat.GetInt(_TargetMainTex) == 0 && _mat.GetInt(_TargetBlendTex) == 0 && _mat.GetInt(_TargetGradation) == 0 && _mat.GetInt(_TargetDistortionTex) == 0 && _mat.GetInt(_TargetDissolveTex) == 0
                && _mat.GetInt(_TargetSurfaceFade) == 0 && _mat.GetInt(_TargetHSVShift) == 0 && _mat.GetInt(_TargetFakeLight) == 0 && _mat.GetInt(_TargetParallax) == 0)
                return code;
             
            var uvName = UniVFXGUILayout.GetVertUVName(_mat.GetInt(_UV + "Transform_Index"), _mat);
            var transform = "st_" + _Tex.Replace("_", "");
            var transformX = UniVFXGUILayout.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").x, _mat.GetVector(_UV + "Transform").x.ToString(), _isCanvas);
            var transformY = UniVFXGUILayout.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").y, _mat.GetVector(_UV + "Transform").y.ToString(), _isCanvas);
            var transformZ = UniVFXGUILayout.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").z, _mat.GetVector(_UV + "Transform").z.ToString(), _isCanvas);
            var transformW = UniVFXGUILayout.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").w, _mat.GetVector(_UV + "Transform").w.ToString(), _isCanvas);

            code.Add("//MaskTex");
            var uv = "o.uv_" + _Tex.Replace("_", "");
            code.Add(uv + " = " + uvName + ";");
            if(UVBend.IsActive(_mat) && UniVFXGUILayout._UVChannelOption[_mat.GetInt(_UV + "Transform_Index")] == "BendUV" && _mat.GetInt(UVBend._Polar) == 1)
            {
                code.Add("");
                return code;
            }

            if (UVBend.IsActive(_mat) && UniVFXGUILayout._UVChannelOption[_mat.GetInt(_UV + "Transform_Index")] == "BendUV")
            {
                UVBend.ApplyBendCode(ref code, _mat, uv, _isCanvas);
            }

            code.Add("float4 " + transform + " = float4(" + transformX + ", " + transformY + ", " + transformZ + ", " + transformW + ");");
            code.Add(uv + " = (" + uv + " - float2(0.5, 0.5)) * " + transform + ".xy + float2(0.5, 0.5) + " + transform + ".zw;");

            code.Add("");
            return code;
        }
        public override List<string> GetFragmentHeadCode()
        {
            var code = new List<string>();
            if (!IsActive())
                return code;
            if (_mat.GetInt(_TargetMainTex) == 0 && _mat.GetInt(_TargetBlendTex) == 0 && _mat.GetInt(_TargetGradation) == 0 && _mat.GetInt(_TargetDistortionTex) == 0 && _mat.GetInt(_TargetDissolveTex) == 0
                && _mat.GetInt(_TargetSurfaceFade) == 0 && _mat.GetInt(_TargetHSVShift) == 0 && _mat.GetInt(_TargetFakeLight) == 0 && _mat.GetInt(_TargetParallax) == 0)
                return code;
             
            var sampler = UniVFXGUILayout.GetSamplerName(_mat.GetInt(_UV + "Transform_Sampler"));
            var offset = UniVFXGUILayout.VertexDataToCode(_mat.GetInt(_Offset + "_Data"), _mat.GetFloat(_Offset).ToString(), _isCanvas);
           
            var uv = "uv_" + _Tex.Replace("_", "");
            var tex = "tex_" + _Tex.Replace("_", "");

            code.Add("//MaskTex");
            code.Add("float2 " + uv + " = i." + uv + ";");
            if (UVBend.IsActive(_mat) && UniVFXGUILayout._UVChannelOption[_mat.GetInt(_UV + "Transform_Index")] == "BendUV" && _mat.GetInt(UVBend._Polar) == 1)
            {
                var transform = "st_" + _Tex.Replace("_", "");
                var transformX = UniVFXGUILayout.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").x, _mat.GetVector(_UV + "Transform").x.ToString(), _isCanvas);
                var transformY = UniVFXGUILayout.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").y, _mat.GetVector(_UV + "Transform").y.ToString(), _isCanvas);
                var transformZ = UniVFXGUILayout.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").z, _mat.GetVector(_UV + "Transform").z.ToString(), _isCanvas);
                var transformW = UniVFXGUILayout.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").w, _mat.GetVector(_UV + "Transform").w.ToString(), _isCanvas);

                UVBend.ApplyBendPolarCode(ref code, uv);
                UVBend.ApplyBendCode(ref code, _mat, uv, _isCanvas);

                code.Add("float4 " + transform + " = float4(" + transformX + ", " + transformY + ", " + transformZ + ", " + transformW + ");");
                code.Add(uv + " = (" + uv + " - float2(0.5, 0.5)) * " + transform + ".xy + float2(0.5, 0.5) + " + transform + ".zw;");

            }
            code.Add("half " + tex + " = SAMPLE_TEXTURE2D(" + _Tex + ", " + sampler + ", " + uv + ").x;");
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
        public override List<string> GetFragmentCode()
        {
            var code = new List<string>();
            return code;
        }


    }
}
