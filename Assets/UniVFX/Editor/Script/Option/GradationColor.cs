using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


namespace UniVFX.Editor
{
    public class GradationColor : UniVFXOption
    {
        static bool _viewGUI = false;
        static bool _viewUVGUI = false;
        protected const string _IsActive = "_GRADATIONCOLOR";
        protected const string _Color00 = "_GradationColor00";
        protected const string _Color01 = "_GradationColor01";
        protected const string _Color10 = "_GradationColor10";
        protected const string _Color11 = "_GradationColor11";
        protected const string _UV = "_GradationUV";
        protected const string _BlendMode = "_GradationBlendMode";
        protected readonly static string[] _BlendModeOption = { "Overwrite", "Add", "Multiply", "Subtract", "Overlay" };

        public GradationColor(bool isCanvas, bool isBRP) : base(isCanvas, isBRP)
        {
        }

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
            return 25;
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
                    _viewGUI = EditorGUILayout.Foldout(_viewGUI, "Gradation");
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
                                    UniVFXGUILayout.OptionColorField(ref _mat, _Color00, "Color00");
                                    UniVFXGUILayout.OptionColorField(ref _mat, _Color01, "Color01");
                                    UniVFXGUILayout.OptionColorField(ref _mat, _Color10, "Color10");
                                    UniVFXGUILayout.OptionColorField(ref _mat, _Color11, "Color11");
                                    UniVFXGUILayout.OptionPopupField(ref _mat, _BlendMode, "Blend Mode", _BlendModeOption);
                                    UniVFXGUILayout.UVGUILayoutClamp(ref _mat, ref _viewUVGUI, _UV, _isCanvas);
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
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").x].Add("GradationUV Offset X");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").y].Add("GradationUV Offset Y");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").z].Add("GradationUV Tile X");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").w].Add("GradationUV Tile Y");
        }

        public override void CollectCustomColorData(ref List<List<string>> useCustomDataList)
        {
            if (!IsActive())
                return;
            useCustomDataList[_mat.GetInt(_Color00 + "_Data")].Add("Gradation Color00");
            useCustomDataList[_mat.GetInt(_Color01 + "_Data")].Add("Gradation Color01");
            useCustomDataList[_mat.GetInt(_Color10 + "_Data")].Add("Gradation Color10");
            useCustomDataList[_mat.GetInt(_Color11 + "_Data")].Add("Gradation Color11");
        }

        public override void CollectUVChannel(ref List<List<string>> useUVChannelList)
        {
            if (!IsActive())
                return;
            useUVChannelList[_mat.GetInt(_UV + "Transform_Index")].Add("Gradation");
        }

        public override void VaridateCustomData()
        {
            UniVFXGUILayout.VaridateCustomDataVector(ref _mat, _UV + "Transform", _isCanvas);
            UniVFXGUILayout.VaridateArrayIndex(ref _mat, _UV + "Transform_Index", UniVFXGUILayout._UVChannelOption);
            UniVFXGUILayout.VaridateCustomColorDataInt(ref _mat, _Color00, _isCanvas);
            UniVFXGUILayout.VaridateCustomColorDataInt(ref _mat, _Color01, _isCanvas);
            UniVFXGUILayout.VaridateCustomColorDataInt(ref _mat, _Color10, _isCanvas);
            UniVFXGUILayout.VaridateCustomColorDataInt(ref _mat, _Color11, _isCanvas);
        }

        public override List<string> GetPropertyCode()
        {
            var code = new List<string>();
            if (!IsActive())
                return code;
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
            return code;
        }
        public override List<string> GetUseV2fCode()
        {
            var code = new List<string>();
            if (!IsActive())
                return code;
            
            code.Add("float2 uv_Gradation");
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
            var transform = "st_Gradation";
            var transformX = UniVFXGUILayout.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").x, _mat.GetVector(_UV + "Transform").x.ToString(), _isCanvas);
            var transformY = UniVFXGUILayout.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").y, _mat.GetVector(_UV + "Transform").y.ToString(), _isCanvas);
            var transformZ = UniVFXGUILayout.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").z, _mat.GetVector(_UV + "Transform").z.ToString(), _isCanvas);
            var transformW = UniVFXGUILayout.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").w, _mat.GetVector(_UV + "Transform").w.ToString(), _isCanvas);

            code.Add("//Gradation");
            var uv = "o.uv_Gradation";
            code.Add(uv + " = " + uvName + ";");
            if (UVBend.IsActive(_mat) && UniVFXGUILayout._UVChannelOption[_mat.GetInt(_UV + "Transform_Index")] == "BendUV" && _mat.GetInt(UVBend._Polar) == 1)
            {
                code.Add("");
                return code;
            }

            if (UVBend.IsActive(_mat) && UniVFXGUILayout._UVChannelOption[_mat.GetInt(_UV + "Transform_Index")] == "BendUV")
            {
                UVBend.ApplyBendCode(ref code, _mat, uv, _isCanvas);
            }

            if(transformX != "1" || transformY != "1" || transformZ != "0" || transformW != "0")
            {
                code.Add("float4 " + transform + " = float4(" + transformX + ", " + transformY + ", " + transformZ + ", " + transformW + ");");
                code.Add(uv + " = (" + uv + " - float2(0.5, 0.5)) * " + transform + ".xy + float2(0.5, 0.5) + " + transform + ".zw;");
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

            var color00 = UniVFXGUILayout.VertexColorDataToCode(_mat.GetInt(_Color00 + "_Data"), _mat.GetColor(_Color00).linear.ToString().Replace("RGBA", "half4"), _isCanvas);
            var color01 = UniVFXGUILayout.VertexColorDataToCode(_mat.GetInt(_Color01 + "_Data"), _mat.GetColor(_Color01).linear.ToString().Replace("RGBA", "half4"), _isCanvas);
            var color10 = UniVFXGUILayout.VertexColorDataToCode(_mat.GetInt(_Color10 + "_Data"), _mat.GetColor(_Color10).linear.ToString().Replace("RGBA", "half4"), _isCanvas);
            var color11 = UniVFXGUILayout.VertexColorDataToCode(_mat.GetInt(_Color11 + "_Data"), _mat.GetColor(_Color11).linear.ToString().Replace("RGBA", "half4"), _isCanvas);

            var blendMode = _mat.GetInt(_BlendMode);
            var uv = "uv_Gradation";
            var color = "col_Gradation";

            code.Add("//Gradation");
            code.Add("float2 " + uv + " = i." + uv + ";");
            if (UVBend.IsActive(_mat) && UniVFXGUILayout._UVChannelOption[_mat.GetInt(_UV + "Transform_Index")] == "BendUV" && _mat.GetInt(UVBend._Polar) == 1)
            {
                var transform = "st_Gradation";
                var transformX = UniVFXGUILayout.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").x, _mat.GetVector(_UV + "Transform").x.ToString(), _isCanvas);
                var transformY = UniVFXGUILayout.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").y, _mat.GetVector(_UV + "Transform").y.ToString(), _isCanvas);
                var transformZ = UniVFXGUILayout.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").z, _mat.GetVector(_UV + "Transform").z.ToString(), _isCanvas);
                var transformW = UniVFXGUILayout.VertexDataToCode((int)_mat.GetVector(_UV + "Transform_Data").w, _mat.GetVector(_UV + "Transform").w.ToString(), _isCanvas);

                UVBend.ApplyBendPolarCode(ref code, uv);
                UVBend.ApplyBendCode(ref code, _mat, uv, _isCanvas);

                if(transformX != "1" || transformY != "1" || transformZ != "0" || transformW != "0")
                {
                    code.Add("float4 " + transform + " = float4(" + transformX + ", " + transformY + ", " + transformZ + ", " + transformW + ");");
                    code.Add(uv + " = (" + uv + " - float2(0.5, 0.5)) * " + transform + ".xy + float2(0.5, 0.5) + " + transform + ".zw;");
                }

            }
            code.Add("half4 gradationColor0 = lerp(" + color00 + ", " + color10 + ", " + uv + ".x);");
            code.Add("half4 gradationColor1 = lerp(" + color01 + ", " + color11 + ", " + uv + ".x);");
            code.Add("half4 gradationColor = lerp(gradationColor0, gradationColor1, " + uv + ".y);");
            code.Add("half4 " + color + " = gradationColor;");
            if (MaskTexture.IsActive(_mat) && _mat.GetInt(MaskTexture._TargetGradation) == 1)
                code.Add(color + ".a *= " + MaskTexture._ResultValue + ";");
            switch (_BlendModeOption[blendMode])
            {
                case "Overwrite":
                    code.Add("col.rgb = lerp(col.rgb, " + color + ".rgb, " + color + ".a);");
                    break;
                case "Add":
                    code.Add("col.rgb = col.rgb + " + color + ".rgb * " + color + ".a;");
                    break;
                case "Multiply":
                    code.Add("col.rgb = col.rgb * (1 - (1 -" + color + ".rgb) * " + color + ".a);");
                    break;
                case "Subtract":
                    code.Add("col.rgb = col.rgb - " + color + ".rgb * " + color + ".a;");
                    break;
                case "Overlay":
                    code.Add("col.rgb = lerp(col.rgb, col.rgb < 0.5 ? 2.0 * " + color + ".rgb * col.rgb : 1.0 - (1.0 - " + color + ".rgb) * (1.0 - col.rgb), " + color + ".a);");
                    break;
                default:
                    break;
            }
            code.Add("");
            return code;
        }
        

    }

}
