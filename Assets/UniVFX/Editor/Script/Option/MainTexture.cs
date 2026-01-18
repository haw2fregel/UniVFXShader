using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


namespace UniVFX.Editor
{
    public class MainTexture : UniVFXOption
    {
        protected const string _Tex = "_MainTex";
        protected const string _UV = "_MainUV";
        protected const string _Color = "_MainColor";
        protected const string _ColorMultiple = "_MainColorMultiple";
        protected const string _AlphaMultiple = "_MainAlphaMultiple";

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
                                UniVFXGUILayout.OptionTextureField(ref _mat, _Tex, "Texture");
                                UniVFXGUILayout.OptionColorField(ref _mat, _Color, "Color");
                                UniVFXGUILayout.OptionBoolField(ref _mat, _ColorMultiple, "Color Multiple");
                                UniVFXGUILayout.OptionBoolField(ref _mat, _AlphaMultiple, "Alpha Multiple");
                                UniVFXGUILayout.UVGUILayout(ref _mat, ref _viewUVGUI, _UV);
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
            UniVFXGUILayout.VaridateCustomDataVector(ref _mat, _UV + "Transform");
            UniVFXGUILayout.VaridateArrayIndex(ref _mat, _UV + "Transform_Index", UniVFXGUILayout._UVChannelOption);
            UniVFXGUILayout.VaridateCustomColorDataInt(ref _mat, _Color);
        }

        public override List<string> GetPropertyCode()
        {
            var code = new List<string>();
            code.Add("[NoScaleOffset]" + _Tex + "(\"" + _Tex.Replace("_", "") + "\", 2D) = \"white\" {}");
            code.Add(_UV + "Transform(\"" + _UV.Replace("_", "") + "Transform\", Vector) = (0,0,1,1)");
            if(_mat.GetInt(_Color + "_Data") == 0)
                code.Add(_Color + "(\"" + _Color.Replace("_", "") + "\", Color) = (1,1,1,1)");
            return code;
        }

        public override List<string> GetCBufferCode()
        {
            var code = new List<string>();
            code.Add("float4 " + _UV + "Transform;");
            if(_mat.GetInt(_Color + "_Data") == 0)
                code.Add("half4 " + _Color + ";");
            return code;
        }

        public override List<string> GetTextureCode()
        {
            var code = new List<string>();
            code.Add("TEXTURE2D(" + _Tex + ");");
            return code;
        }

        public override List<string> GetUseV2fCode()
        {
            var code = new List<string>();
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
            var uvName = UniVFXGUILayout.GetVertUVName(_mat.GetInt(_UV + "Transform_Index"), _mat);
            var transform = "st_" + _Tex.Replace("_", "");
            var transformX = VertexDataConvert.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").x, _UV + "Transform.x");
            var transformY = VertexDataConvert.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").y, _UV + "Transform.y");
            var transformZ = VertexDataConvert.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").z, _UV + "Transform.z");
            var transformW = VertexDataConvert.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").w, _UV + "Transform.w");
            

            code.Add("//MainTex");

            var uv = "o.uv_" + _Tex.Replace("_", "");
            code.Add(uv + " = " + uvName + ";");
            if(UVBend.IsActive(_mat) && UniVFXGUILayout._UVChannelOption[_mat.GetInt(_UV + "Transform_Index")] == "BendUV" && _mat.GetInt(UVBend._Polar) == 1)
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
            var sampler = UniVFXGUILayout.GetSamplerName(_mat.GetInt(_UV + "Transform_Sampler"));
            var color = VertexColorDataConvert.VertexColorDataToCode(_mat.GetInt(_Color + "_Data"), _Color);
            var colorMultiple = _mat.GetInt(_ColorMultiple) == 1;
            var alphaMultiple = _mat.GetInt(_AlphaMultiple) == 1;
            var uv = "uv_" + _Tex.Replace("_", "");
            var tex = "tex_" + _Tex.Replace("_", "");

            code.Add("//MainTex");
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
            if (UVDistortion.IsActive(_mat) && _mat.GetInt(UVDistortion._TargetMainTex) == 1)
                code.Add(uv + " += " + UVDistortion._ResultValue + ";");
            if (UVParallax.IsActive(_mat) && _mat.GetInt(UVParallax._TargetMainTex) == 1)
                code.Add(uv + " += " + UVParallax._ResultValue + ";");
            code.Add("half4 " + tex + " = SAMPLE_TEXTURE2D(" + _Tex + ", " + sampler + ", " + uv + ");");
            if (MaskTexture.IsActive(_mat) && _mat.GetInt(MaskTexture._TargetMainTex) == 1)
                code.Add(tex + ".a *= " + MaskTexture._ResultValue + ";");
            if (SurfaceFade.IsActive(_mat) && _mat.GetInt(SurfaceFade._TargetMainTex) == 1)
                code.Add(tex + ".a *= " + SurfaceFade._ResultValue + ";");
            if (colorMultiple)
                code.Add(tex + ".rgb *= " + color + ".rgb;");
            if (alphaMultiple)
                code.Add(tex + ".a *= " + color + ".a;");
            code.Add("col = " + tex + ";");
            code.Add("");
            return code;
        }
        
    }

}
