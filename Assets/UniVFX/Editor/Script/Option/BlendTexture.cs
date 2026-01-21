using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


namespace UniVFX.Editor
{
    public class BlendTexture : UniVFXOption
    {
        static bool _viewGUI = false;
        static bool _viewUVGUI = false;
        protected const string _IsActive = "_BLENDTEXTURE";
        protected const string _Tex = "_BlendTex";
        protected const string _Color = "_BlendTexColor";
        protected const string _Intensity = "_BlendIntensity";
        protected const string _UV = "_BlendUV";
        protected const string _BlendMode = "_BlendTexBlendMode";
        protected readonly static string[] _BlendModeOption = { "Overwrite", "Add", "Multiply", "Subtract", "Overlay" };

        public BlendTexture(bool isCanvas, bool isBRP) : base(isCanvas, isBRP)
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
            return 11;
        }

        public override void GetHeatValue(ref int value, ref int max)
        {
            max += HeatValue();
            if(IsActive())
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
                    _viewGUI = EditorGUILayout.Foldout(_viewGUI, "Blend Texture");
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
                                    UniVFXGUILayout.OptionColorField(ref _mat, _Color, "Color");
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Intensity, "Intensity", 0, 1);
                                    UniVFXGUILayout.OptionPopupField(ref _mat, _BlendMode, "Blend Mode", _BlendModeOption);
                                    UniVFXGUILayout.UVGUILayout(ref _mat, ref _viewUVGUI, _UV, _isCanvas);
                                }
                            }
                        }
                    }
                }
                GUI.color = new Color(1f, 1f, 1f, 1f);
            }
        }

        public override void CollectCustomData(ref List<List<string>> useCustomDataList)
        {
            if (!IsActive())
                return;
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").x].Add("BlendUV Offset X");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").y].Add("BlendUV Offset Y");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").z].Add("BlendUV Tile X");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").w].Add("BlendUV Tile Y");
            useCustomDataList[_mat.GetInt(_Intensity + "_Data")].Add("BlendTexture Intensity");
        }

        public override void CollectCustomColorData(ref List<List<string>> useCustomDataList)
        {
            if (!IsActive())
                return;
            useCustomDataList[_mat.GetInt(_Color + "_Data")].Add("BlendTex Color");
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
            UniVFXGUILayout.VaridateCustomColorDataInt(ref _mat, _Color, _isCanvas);
        }


        public override List<string> GetPropertyCode(RefactOption refactOption)
        {
            var code = new List<string>();
            if (!IsActive())
                return code;

            var intensity = UniVFXGUILayout.VertexDataToFloatCode(_mat, _Intensity, _isCanvas, refactOption);
            var isInvalid = intensity == "0" && refactOption.HasFlag(RefactOption.PropertiesToFixedValue);
            var isFixedValue = refactOption.HasFlag(RefactOption.PropertiesToFixedValue);
            var isTextureNone = _mat.GetTexture(_Tex) == null && refactOption.HasFlag(RefactOption.NoneTextureToFixedValue);
            

            if(isInvalid)
            {
                code.Add("//BlendTex Skipped, Intensity is 0");
                return code;
            }

            if (isTextureNone)
            {
                code.Add("//BlendTex Skipped, Texture is null");
            }else
            {
                code.Add("[NoScaleOffset]" + _Tex + "(\"" + _Tex.Replace("_", "") + "\", 2D) = \"white\" {}");
            }

            if(isFixedValue)
            {
                code.Add("//BlendTex Property Skipped, FixedValue");
            }
            else
            {
                if (_mat.GetInt(_Color + "_Data") == 0)
                    code.Add(_Color + "(\"" + _Color.Replace("_", "") + "\", Color) = (1,1,1,1)");
                if(_mat.GetInt(_Intensity + "_Data") == 0)
                    code.Add(_Intensity + "(\"" + _Intensity.Replace("_", "") + "\", float) = 1");
                
                if (isTextureNone)
                {
                    code.Add("//BlendTex Transform Skipped, Texture is null");
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

            var intensity = UniVFXGUILayout.VertexDataToFloatCode(_mat, _Intensity, _isCanvas, refactOption);
            var isInvalid = intensity == "0" && refactOption.HasFlag(RefactOption.PropertiesToFixedValue);
            var isFixedValue = refactOption.HasFlag(RefactOption.PropertiesToFixedValue);
            var isTextureNone = _mat.GetTexture(_Tex) == null && refactOption.HasFlag(RefactOption.NoneTextureToFixedValue);
            
            if(isInvalid)
            {
                code.Add("//BlendTex Skipped, Intensity is 0");
                return code;
            }

            if(isFixedValue)
            {
                code.Add("//BlendTex Property Skipped, FixedValue");
            }
            else
            {
                if (isTextureNone)
                {
                    code.Add("//BlendTex Transform Skipped, Texture is null");
                }else
                {
                    if(_mat.GetVector(_UV + "Transform_Data") == new Vector4(0,0,0,0))
                        code.Add("float4 " + _UV + "Transform;");
                }

                if (_mat.GetInt(_Color + "_Data") == 0)
                    code.Add("half4 " + _Color + ";");
                if(_mat.GetInt(_Intensity + "_Data") == 0)
                    code.Add("half " + _Intensity + ";");
            }
            
            
            return code;
        }
        public override List<string> GetTextureCode(RefactOption refactOption)
        {
            var code = new List<string>();
            if (!IsActive())
                return code;

            var intensity = UniVFXGUILayout.VertexDataToFloatCode(_mat, _Intensity, _isCanvas, refactOption);
            var isInvalid = intensity == "0" && refactOption.HasFlag(RefactOption.PropertiesToFixedValue);
            var isTextureNone = _mat.GetTexture(_Tex) == null && refactOption.HasFlag(RefactOption.NoneTextureToFixedValue);

            if(isInvalid)
            {
                code.Add("//BlendTex Skipped, Intensity is 0");
                return code;
            }

            if (isTextureNone)
            {
                code.Add("//BlendTex Skipped, Texture is null");
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

            var intensity = UniVFXGUILayout.VertexDataToFloatCode(_mat, _Intensity, _isCanvas, refactOption);
            var isInvalid = intensity == "0" && refactOption.HasFlag(RefactOption.PropertiesToFixedValue);
            var isTextureNone = _mat.GetTexture(_Tex) == null && refactOption.HasFlag(RefactOption.NoneTextureToFixedValue);

            if(isInvalid)
            {
                code.Add("//BlendTex Skipped, Intensity is 0");
                return code;
            }

            if (isTextureNone)
            {
                code.Add("//BlendTex Skipped, Texture is null");
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

            var intensity = UniVFXGUILayout.VertexDataToFloatCode(_mat, _Intensity, _isCanvas, refactOption);
            var isInvalid = intensity == "0" && refactOption.HasFlag(RefactOption.PropertiesToFixedValue);
            var isTextureNone = _mat.GetTexture(_Tex) == null && refactOption.HasFlag(RefactOption.NoneTextureToFixedValue);

            var uvName = UniVFXGUILayout.GetVertUVName(_mat.GetInt(_UV + "Transform_Index"), _mat);
            var transform = "st_" + _Tex.Replace("_", "");
            var transformX = UniVFXGUILayout.VertexDataToVectorCode(_mat, _UV + "Transform", 0, _isCanvas, refactOption);
            var transformY = UniVFXGUILayout.VertexDataToVectorCode(_mat, _UV + "Transform", 1, _isCanvas, refactOption);
            var transformZ = UniVFXGUILayout.VertexDataToVectorCode(_mat, _UV + "Transform", 2, _isCanvas, refactOption);
            var transformW = UniVFXGUILayout.VertexDataToVectorCode(_mat, _UV + "Transform", 3, _isCanvas, refactOption);

            code.Add("//BlendTex");

            if(isInvalid)
            {
                code.Add("//BlendTex Skipped, Intensity is 0");
                code.Add("");
                return code;
            }

            if (isTextureNone)
            {
                code.Add("//BlendTex Skipped, Texture is null");
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
            return code;
        }
        public override List<string> GetFragmentCode(RefactOption refactOption)
        {
            var code = new List<string>();
            if (!IsActive())
                return code;

            var intensity = UniVFXGUILayout.VertexDataToFloatCode(_mat, _Intensity, _isCanvas, refactOption);
            var isInvalid = intensity == "0" && refactOption.HasFlag(RefactOption.PropertiesToFixedValue);
            var isTextureNone = _mat.GetTexture(_Tex) == null && refactOption.HasFlag(RefactOption.NoneTextureToFixedValue);
            
            var sampler = UniVFXGUILayout.GetSamplerName(_mat.GetInt(_UV + "Transform_Sampler"));
            var color = UniVFXGUILayout.VertexDataToColorCode(_mat, _Color, true, _isCanvas, refactOption);
            var blendMode = _mat.GetInt(_BlendMode);
            var uv = "uv_" + _Tex.Replace("_", "");
            var tex = "tex_" + _Tex.Replace("_", "");

            code.Add("//BlendTex");
            if(isInvalid)
            {
                code.Add("//BlendTex Skipped, Intensity is 0");
                code.Add("");
                return code;
            }

            if (isTextureNone)
            {
                code.Add("half4 " + tex + " = half4(1,1,1,1);");
                code.Add("//BlendTex Skipped, Texture is null");
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
                if(UVDistortion.IsActive(_mat) && _mat.GetInt(UVDistortion._TargetBlendTex) == 1)
                    code.Add(uv + " += " + UVDistortion._ResultValue + ";");
                if (UVParallax.IsActive(_mat) && _mat.GetInt(UVParallax._TargetBlendTex) == 1)
                    code.Add(uv + " += " + UVParallax._ResultValue + ";");
                code.Add("half4 " + tex + " = SAMPLE_TEXTURE2D(" + _Tex + ", " + sampler + ", " + uv + ");");
            }
            if(color != "half4(1.000, 1.000, 1.000, 1.000)")
                code.Add(tex + " *= " + color + ";");
            if(intensity != "1")
                code.Add(tex + ".a *= " + intensity + ";");
            if (MaskTexture.IsActive(_mat) && _mat.GetInt(MaskTexture._TargetBlendTex) == 1)
                code.Add(tex + ".a *= " + MaskTexture._ResultValue + ";");
            if (SurfaceFade.IsActive(_mat) && _mat.GetInt(SurfaceFade._TargetBlendTex) == 1)
                code.Add(tex + ".a *= " + SurfaceFade._ResultValue + ";");
            switch (_BlendModeOption[blendMode])
            {
                case "Overwrite":
                    code.Add("col.rgb = lerp(col.rgb, " + tex + ".rgb, " + tex + ".a);");
                    break;
                case "Add":
                    code.Add("col.rgb = col.rgb + " + tex + ".rgb * " + tex + ".a;");
                    break;
                case "Multiply":
                    code.Add("col.rgb = col.rgb * (1 - (1 -" + tex + ".rgb) * " + tex + ".a);");
                    break;
                case "Subtract":
                    code.Add("col.rgb = col.rgb - " + tex + ".rgb * " + tex + ".a;");
                    break;
                case "Overlay":
                    code.Add("col.rgb = lerp(col.rgb, col.rgb < 0.5 ? 2.0 * " + tex + ".rgb * col.rgb : 1.0 - (1.0 - " + tex + ".rgb) * (1.0 - col.rgb), " + tex + ".a);");
                    break;
                default:
                    break;
            }
            code.Add("");
            return code;
        }


    }
}
