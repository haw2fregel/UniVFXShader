using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


namespace UniVFX.Editor
{
    public class Dissolve : UniVFXOption
    {
        protected const string _IsActive = "_DISSOLVE";
        protected const string _Tex = "_DissolveTex";
        protected const string _UV = "_DissolveUV";
        protected const string _Color = "_DissolveColor";
        protected const string _Param = "_DissolveParam";

        public override bool IsActive()
        {
            return _mat.IsKeywordEnabled(_IsActive);
        }

        public static bool IsActive(Material mat)
        {
            return mat.IsKeywordEnabled(_IsActive);
        }

        public override void SetActive(bool active)
        {
            if (active)
            {
                if(!_mat.IsKeywordEnabled(_IsActive)) 
                    _mat.EnableKeyword(_IsActive);
            }else{
                if(_mat.IsKeywordEnabled(_IsActive)) 
                    _mat.DisableKeyword(_IsActive);
            }
        }

        public static void SetActive(Material mat, bool active)
        {
            if (active)
            {
                if(!mat.IsKeywordEnabled(_IsActive)) 
                    mat.EnableKeyword(_IsActive);
            }else{
                if(mat.IsKeywordEnabled(_IsActive)) 
                    mat.DisableKeyword(_IsActive);
            }
        }

        public override int HeatValue()
        {
            return 31;
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
                    _viewGUI = EditorGUILayout.Foldout(_viewGUI, "Dissolve");
                    if (_viewGUI)
                    {
                        var active = UniVFXGUILayout.OptionKeywordField(ref _mat, _IsActive, "Active");
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
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Param, "Alpha", 0, 0, 1);
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Param, "Smooth", 1, 0, 1);
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Param, "Emissive Width", 2, 0, 1);
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Param, "Emissive Smooth", 3, 0, 1);
                                    UniVFXGUILayout.UVGUILayout(ref _mat, ref _viewUVGUI, _UV);
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
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").x].Add("DissolveUV Tile X");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").y].Add("DissolveUV Tile Y");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").z].Add("DissolveUV Offset X");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").w].Add("DissolveUV Offset Y");
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").x].Add("Disslve Alpha");
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").y].Add("Disslve Smooth");
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").z].Add("Disslve Emissive Width");
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").w].Add("Disslve Emissive Smooth");
        }

        public override void CollectCustomColorData(ref List<List<string>> useCustomDataList)
        {
            if (!IsActive())
                return;
            useCustomDataList[_mat.GetInt(_Color + "_Data")].Add("Dissolve Color");
        }

        public override void CollectUVChannel(ref List<List<string>> useUVChannelList)
        {
            if (!IsActive())
                return;
            useUVChannelList[_mat.GetInt(_UV + "Transform_Index")].Add(_Tex);
        }

        public override void VaridateCustomData()
        {
            UniVFXGUILayout.VaridateCustomDataVector(ref _mat, _UV + "Transform");
            UniVFXGUILayout.VaridateCustomDataVector(ref _mat, _Param);
            UniVFXGUILayout.VaridateArrayIndex(ref _mat, _UV + "Transform_Index", UniVFXGUILayout._UVChannelOption);
            UniVFXGUILayout.VaridateCustomColorDataInt(ref _mat, _Color);
        }

        public override List<string> GetPropertyCode()
        {
            var code = new List<string>();
            if (!IsActive())
                return code;
            
            code.Add("[NoScaleOffset]" + _Tex + "(\"" + _Tex.Replace("_", "") + "\", 2D) = \"white\" {}");
            code.Add(_UV + "Transform(\"" + _UV.Replace("_", "") + "Transform\", Vector) = (0,0,1,1)");
            if (_mat.GetInt(_Color + "_Data") == 0)
                code.Add("[HDR]" + _Color + "(\"" + _Color.Replace("_", "") + "\", Color) = (1,1,1,1)");
            code.Add(_Param + "(\"" + _Param.Replace("_", "") + "\", Vector) = (1,0,0,0)");
            return code;
        }
        public override List<string> GetCBufferCode()
        {
            var code = new List<string>();
            if (!IsActive())
                return code;
            
            code.Add("float4 " + _UV + "Transform;");
            if (_mat.GetInt(_Color + "_Data") == 0)
                code.Add("half4 " + _Color + ";");
            code.Add("half4 " + _Param + ";");
            return code;
        }
        public override List<string> GetTextureCode()
        {
            var code = new List<string>();
            if (!IsActive())
                return code;
            
            code.Add("TEXTURE2D(" + _Tex + ");");
            return code;
        }
        public override List<string> GetUseV2fCode()
        {
            var code = new List<string>();
            if (!IsActive())
                return code;
            
            code.Add("float2 uv_" + _Tex.Replace("_", ""));
            if (!SurfaceFade.IsActive(_mat) || _mat.GetInt(SurfaceFade._TargetDissolve) == 0)
            {
                code.Add("float4 param_" + _Tex.Replace("_", ""));
            }
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
            
            var uvName = UniVFXGUILayout.GetVertUVName(_mat.GetInt(_UV + "Transform_Index"), _mat);
            var transform = "st_" + _Tex.Replace("_", "");
            var transformX = VertexDataConvert.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").x, _UV + "Transform.x");
            var transformY = VertexDataConvert.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").y, _UV + "Transform.y");
            var transformZ = VertexDataConvert.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").z, _UV + "Transform.z");
            var transformW = VertexDataConvert.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").w, _UV + "Transform.w");

            code.Add("//Dissolve");
            var uv = "o.uv_" + _Tex.Replace("_", "");
            code.Add(uv + " = " + uvName + ";");
            if (UVBend.IsActive(_mat) && UniVFXGUILayout._UVChannelOption[_mat.GetInt(_UV + "Transform_Index")] == "BendUV" && _mat.GetInt(UVBend._Polar) == 1)
            {
                code.Add("");
                return code;
            }

            if (UVBend.IsActive(_mat) && UniVFXGUILayout._UVChannelOption[_mat.GetInt(_UV + "Transform_Index")] == "BendUV")
            {
                UVBend.ApplyBendCode(ref code, _mat, uv);
            }

            code.Add("float4 " + transform + " = float4(" + transformX + ", " + transformY + ", " + transformZ + ", " + transformW + ");");
            code.Add(uv + " = (" + uv + " - float2(0.5, 0.5)) * " + transform + ".xy + float2(0.5, 0.5) + " + transform + ".zw;");


            if (!SurfaceFade.IsActive(_mat) || _mat.GetInt(SurfaceFade._TargetDissolve) == 0)
            {
                var param = "param_" + _Tex.Replace("_", "");
                var paramX = VertexDataConvert.VertexDataToCode((int)_mat.GetVector(_Param + "_Data").x, _Param + ".x");
                var paramY = VertexDataConvert.VertexDataToCode((int)_mat.GetVector(_Param + "_Data").y, _Param + ".y");
                var paramZ = VertexDataConvert.VertexDataToCode((int)_mat.GetVector(_Param + "_Data").z, _Param + ".z");
                var paramW = VertexDataConvert.VertexDataToCode((int)_mat.GetVector(_Param + "_Data").w, _Param + ".w");

                code.Add("float4 " + param + " = float4(" + paramX + ", " + paramY + ", " + paramZ + ", " + paramW + ");");
                code.Add("half dissolveEmissive = " + param + ".z + " + param + ".w;");
                code.Add("half dissolveSmooth = max(0.0001, " + param + ".y);");
                code.Add("half dissolveAlpha = 1 -  " + param + ".x;");
                code.Add("half dissolveAlphaMin = dissolveAlpha - (dissolveSmooth + dissolveEmissive);");
                code.Add("dissolveAlpha = dissolveAlphaMin + dissolveAlpha * (1 - dissolveAlphaMin);");
                code.Add("dissolveSmooth += dissolveAlpha;");
                code.Add("half dissolveEmissiveWidth = dissolveSmooth + " + param + ".z;");
                code.Add("half dissolveEmissiveSmooth = dissolveEmissiveWidth + max(0.0001, " + param + ".w);");
                code.Add("o." + param + " = float4(dissolveAlpha, dissolveSmooth, dissolveEmissiveWidth, dissolveEmissiveSmooth);");
            }
            
            code.Add("");
            return code;
        }

        public override List<string> GetFragmentHeadCode()
        {
            var code = new List<string>();
            return code;
        }
        public override List<string> GetFragmentCode()
        {
            var code = new List<string>();
            if (!IsActive())
                return code;
            
            var sampler = UniVFXGUILayout.GetSamplerName(_mat.GetInt(_UV + "Transform_Sampler"));
            var color = VertexColorDataConvert.VertexColorDataToCode(_mat.GetInt(_Color + "_Data"), _Color);
            var uv = "uv_" + _Tex.Replace("_", "");
            var tex = "tex_" + _Tex.Replace("_", "");
            var param = "param_" + _Tex.Replace("_", "");
            

            code.Add("//Dissolve");
            code.Add("float2 " + uv + " = i." + uv + ";");
            if (UVBend.IsActive(_mat) && UniVFXGUILayout._UVChannelOption[_mat.GetInt(_UV + "Transform_Index")] == "BendUV" && _mat.GetInt(UVBend._Polar) == 1)
            {
                var transform = "st_" + _Tex.Replace("_", "");
                var transformX = VertexDataConvert.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").x, _UV + "Transform.x");
                var transformY = VertexDataConvert.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").y, _UV + "Transform.y");
                var transformZ = VertexDataConvert.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").z, _UV + "Transform.z");
                var transformW = VertexDataConvert.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").w, _UV + "Transform.w");

                UVBend.ApplyBendPolarCode(ref code, uv);
                UVBend.ApplyBendCode(ref code, _mat, uv);

                code.Add("float4 " + transform + " = float4(" + transformX + ", " + transformY + ", " + transformZ + ", " + transformW + ");");
                code.Add(uv + " = (" + uv + " - float2(0.5, 0.5)) * " + transform + ".xy + float2(0.5, 0.5) + " + transform + ".zw;");

            }
            if (UVDistortion.IsActive(_mat) && _mat.GetInt(UVDistortion._TargetDissolveTex) == 1)
                code.Add(uv + " += " + UVDistortion._ResultValue + ";");
            if (UVParallax.IsActive(_mat) && _mat.GetInt(UVParallax._TargetDissolveTex) == 1)
                code.Add(uv + " += " + UVParallax._ResultValue + ";");
            code.Add("half4 " + tex + " = SAMPLE_TEXTURE2D(" + _Tex + ", " + sampler + ", " + uv + ");");
            if (MaskTexture.IsActive(_mat) && _mat.GetInt(MaskTexture._TargetDissolveTex) == 1)
                code.Add(tex + ".x = 1 - (1 - " + tex + ".x) * " + MaskTexture._ResultValue + ";");

            if (!SurfaceFade.IsActive(_mat) || _mat.GetInt(SurfaceFade._TargetDissolve) == 0)
            {
                code.Add("half dissolveResult = smoothstep(i." + param + ".x, i." + param + ".y, " + tex + ".x);");
                code.Add("half3 dissolveEmissiveResult = smoothstep(i." + param + ".w, i." + param + ".z, " + tex + ".x) * step(0.001, i." + param + ".w - + i." + param + ".y) * " + color + ".xyz;");

            }
            else
            {
                var paramX = VertexDataConvert.VertexDataToCode((int)_mat.GetVector(_Param + "_Data").x, _Param + ".x");
                var paramY = VertexDataConvert.VertexDataToCode((int)_mat.GetVector(_Param + "_Data").y, _Param + ".y");
                var paramZ = VertexDataConvert.VertexDataToCode((int)_mat.GetVector(_Param + "_Data").z, _Param + ".z");
                var paramW = VertexDataConvert.VertexDataToCode((int)_mat.GetVector(_Param + "_Data").w, _Param + ".w");

                code.Add("float4 " + param + " = float4(" + paramX + ", " + paramY + ", " + paramZ + ", " + paramW + ");");
                code.Add(param + ".x *= " + SurfaceFade._ResultValue + ";");

                code.Add("half dissolveEmissive = " + param + ".z + " + param + ".w;");
                code.Add("half dissolveSmooth = max(0.0001, " + param + ".y);");
                code.Add("half dissolveAlpha = 1 -  " + param + ".x;");
                code.Add("half dissolveAlphaMin = dissolveAlpha - (dissolveSmooth + dissolveEmissive);");
                code.Add("dissolveAlpha = dissolveAlphaMin + dissolveAlpha * (1 - dissolveAlphaMin);");
                code.Add("dissolveSmooth += dissolveAlpha;");
                code.Add("half dissolveEmissiveWidth = dissolveSmooth + " + param + ".z;");
                code.Add("half dissolveEmissiveSmooth = dissolveEmissiveWidth + max(0.0001, " + param + ".w);");
                code.Add("half dissolveResult = smoothstep(dissolveAlpha, dissolveSmooth, " + tex + ".x);");
                code.Add("half3 dissolveEmissiveResult = smoothstep(dissolveEmissiveSmooth, dissolveEmissiveWidth, " + tex + ".x) * step(0.001, dissolveEmissive) * " + color + ".xyz;");
            }
            code.Add("col.a *= dissolveResult;");
            code.Add("col.rgb += dissolveEmissiveResult;");


            return code;
        }
        
    }
}
