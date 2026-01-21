using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


namespace UniVFX.Editor
{
    public class MainTexture : UniVFXOption
    {
        static bool _viewGUI = false;
        static bool _viewUVGUI = false;
        protected const string _Tex = "_MainTex";
        protected const string _UV = "_MainUV";
        protected const string _Color = "_MainColor";
        protected const string _ColorMultiple = "_MainColorMultiple";
        protected const string _AlphaMultiple = "_MainAlphaMultiple";

        public MainTexture(bool isCanvas, bool isBRP) : base(isCanvas, isBRP)
        {
        }

        public override bool IsActive()
        {
            return true;
        }

        public override void SetActive(bool active)
        {
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
                    _viewGUI = EditorGUILayout.Foldout(_viewGUI, "Main Texture");
                    if (_viewGUI)
                    {
                        GUI.color = new Color(0f, 0f, 0f, 0.6f);
                        using (new EditorGUILayout.VerticalScope("Box"))
                        {
                            using (new EditorGUI.IndentLevelScope())
                            {
                                GUI.color = new Color(1f, 1f, 1f, 1f);
                                if(!_isCanvas) UniVFXGUILayout.OptionTextureField(ref _mat, _Tex, "Texture");
                                UniVFXGUILayout.OptionColorField(ref _mat, _Color, "Color", _isCanvas);
                                UniVFXGUILayout.OptionBoolField(ref _mat, _ColorMultiple, "Color Multiple");
                                UniVFXGUILayout.OptionBoolField(ref _mat, _AlphaMultiple, "Alpha Multiple");
                                UniVFXGUILayout.UVGUILayout(ref _mat, ref _viewUVGUI, _UV, _isCanvas);
                            }
                        }
                    }
                }
            }
            GUI.color = new Color(1f, 1f, 1f, 1f);
        }

        public override void CollectCustomData(ref List<List<string>> useCustomDataList)
        {
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").x].Add("MainUV Offset X");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").y].Add("MainUV Offset Y");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").z].Add("MainUV Tile X");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").w].Add("MainUV Tile Y");
        }

        public override void CollectCustomColorData(ref List<List<string>> useCustomDataList)
        {
            useCustomDataList[_mat.GetInt(_Color + "_Data")].Add("Main Color");
        }

        public override void CollectUVChannel(ref List<List<string>> useUVChannelList)
        {
            useUVChannelList[_mat.GetInt(_UV + "Transform_Index")].Add(_Tex);
        }

        public override void VaridateCustomData()
        {
            UniVFXGUILayout.VaridateCustomDataVector(ref _mat, _UV + "Transform", _isCanvas);
            UniVFXGUILayout.VaridateArrayIndex(ref _mat, _UV + "Transform_Index", UniVFXGUILayout._UVChannelOption);
            UniVFXGUILayout.VaridateCustomColorDataInt(ref _mat, _Color, _isCanvas);
        }

        public override List<string> GetPropertyCode(RefactOption refactOption)
        {
            var code = new List<string>();

            var isFixedValue = refactOption.HasFlag(RefactOption.PropertiesToFixedValue);
            var isTextureNone = _mat.GetTexture(_Tex) == null && refactOption.HasFlag(RefactOption.NoneTextureToFixedValue) && !_isCanvas;

            if (isTextureNone)
            {
                code.Add("//MainTex Skipped, Texture is null");
            }else
            {
                code.Add("[NoScaleOffset]" + _Tex + "(\"" + _Tex.Replace("_", "") + "\", 2D) = \"white\" {}");
            }

            if(isFixedValue)
            {
                code.Add("//MainTex Property Skipped, FixedValue");
            }
            else
            {
                if (_mat.GetInt(_Color + "_Data") == 0)
                    code.Add(_Color + "(\"" + _Color.Replace("_", "") + "\", Color) = (1,1,1,1)");
                
                if (isTextureNone)
                {
                    code.Add("//MainTex Transform Skipped, Texture is null");
                }
                else
                {
                    var transformData = _mat.GetVector(_UV + "Transform_Data");
                    if(transformData.x == 0 || transformData.y == 0 || transformData.z == 0 || transformData.w == 0)
                        code.Add(_UV + "Transform(\"" + _UV.Replace("_", "") + "Transform\", Vector) = (0,0,1,1)");
                }
            }
            return code;
        }

        public override List<string> GetCBufferCode(RefactOption refactOption)
        {
            var code = new List<string>();

            var isFixedValue = refactOption.HasFlag(RefactOption.PropertiesToFixedValue);
            var isTextureNone = _mat.GetTexture(_Tex) == null && refactOption.HasFlag(RefactOption.NoneTextureToFixedValue) && !_isCanvas;
            

            if(isFixedValue)
            {
                code.Add("//MainTex Property Skipped, FixedValue");
            }
            else
            {
                if (isTextureNone)
                {
                    code.Add("//MainTex Transform Skipped, Texture is null");
                }else
                {
                    var transformData = _mat.GetVector(_UV + "Transform_Data");
                    if(transformData.x == 0 || transformData.y == 0 || transformData.z == 0 || transformData.w == 0)
                        code.Add("float4 " + _UV + "Transform;");
                }

                if (_mat.GetInt(_Color + "_Data") == 0)
                    code.Add("half4 " + _Color + ";");
            }

            
            return code;
        }

        public override List<string> GetTextureCode(RefactOption refactOption)
        {
            var code = new List<string>();

            var isTextureNone = _mat.GetTexture(_Tex) == null && refactOption.HasFlag(RefactOption.NoneTextureToFixedValue) && !_isCanvas;

            if (isTextureNone)
            {
                code.Add("//MainTex Skipped, Texture is null");
            }
            else
            {
                if(_isBRP)
                {
                    code.Add("Texture2D " + _Tex + ";");
                }else
                {
                    code.Add("TEXTURE2D(" + _Tex + ");");
                }
            }
            
            
            return code;
        }

        public override List<string> GetUseV2fCode(RefactOption refactOption)
        {
            var code = new List<string>();

            var isTextureNone = _mat.GetTexture(_Tex) == null && refactOption.HasFlag(RefactOption.NoneTextureToFixedValue) && !_isCanvas;

            if (isTextureNone)
            {
                code.Add("//MainTex Skipped, Texture is null");
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

            var isTextureNone = _mat.GetTexture(_Tex) == null && refactOption.HasFlag(RefactOption.NoneTextureToFixedValue) && !_isCanvas;
            
            var uvName = UniVFXGUILayout.GetVertUVName(_mat.GetInt(_UV + "Transform_Index"), _mat, _isCanvas);
            var transform = "st_" + _Tex.Replace("_", "");
            var transformX = UniVFXGUILayout.VertexDataToVectorCode(_mat, _UV + "Transform", 0, _isCanvas, refactOption);
            var transformY = UniVFXGUILayout.VertexDataToVectorCode(_mat, _UV + "Transform", 1, _isCanvas, refactOption);
            var transformZ = UniVFXGUILayout.VertexDataToVectorCode(_mat, _UV + "Transform", 2, _isCanvas, refactOption);
            var transformW = UniVFXGUILayout.VertexDataToVectorCode(_mat, _UV + "Transform", 3, _isCanvas, refactOption);


            code.Add("//MainTex");

            if (isTextureNone)
            {
                code.Add("//MainTex Skipped, Texture is null");
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

            var isTextureNone = _mat.GetTexture(_Tex) == null && refactOption.HasFlag(RefactOption.NoneTextureToFixedValue) && !_isCanvas;

            var sampler = UniVFXGUILayout.GetSamplerName(_mat.GetInt(_UV + "Transform_Sampler"));
            var color = UniVFXGUILayout.VertexDataToColorCode(_mat, _Color, true, _isCanvas, refactOption);
            var colorMultiple = _mat.GetInt(_ColorMultiple) == 1;
            var alphaMultiple = _mat.GetInt(_AlphaMultiple) == 1;
            var uv = "uv_" + _Tex.Replace("_", "");
            var tex = "tex_" + _Tex.Replace("_", "");

            code.Add("//MainTex");
            if (isTextureNone)
            {
                code.Add("half4 " + tex + " = half4(1,1,1,1);");
                code.Add("//MainTex Skipped, Texture is null");
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
                if (UVDistortion.IsActive(_mat) && _mat.GetInt(UVDistortion._TargetMainTex) == 1)
                    code.Add(uv + " += " + UVDistortion._ResultValue + ";");
                if (UVParallax.IsActive(_mat) && _mat.GetInt(UVParallax._TargetMainTex) == 1)
                    code.Add(uv + " += " + UVParallax._ResultValue + ";");

                if(_isBRP)
                {
                    code.Add("half4 " + tex + " = " + _Tex + ".Sample(" + sampler + ", " + uv + ");");
                }
                else
                {
                    code.Add("half4 " + tex + " = SAMPLE_TEXTURE2D(" + _Tex + ", " + sampler + ", " + uv + ");");
                }
            }
            if (MaskTexture.IsActive(_mat) && _mat.GetInt(MaskTexture._TargetMainTex) == 1)
                code.Add(tex + " *= " + MaskTexture._ResultValue + ";");
            if (SurfaceFade.IsActive(_mat) && _mat.GetInt(SurfaceFade._TargetMainTex) == 1)
                code.Add(tex + " *= " + SurfaceFade._ResultValue + ";");
            if (colorMultiple && color != "half4(1.000, 1.000, 1.000, 1.000)")
                code.Add(tex + ".rgb *= " + color + ".rgb;");
            if (alphaMultiple && color != "half4(1.000, 1.000, 1.000, 1.000)")
                code.Add(tex + ".a *= " + color + ".a;");
            code.Add("col = " + tex + ";");
            code.Add("");
            return code;
        }
        
    }

}
