using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


namespace UniVFX.Editor
{
    public class HSVShift : UniVFXOption
    {
        protected const string _IsActive = "_HSVSHIFT";
        protected const string _Param = "_HSVShiftParametors";

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
            return 28;
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
                    _viewGUI = EditorGUILayout.Foldout(_viewGUI, "HSV Shift");
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

                                    UniVFXGUILayout.OptionSlider(ref _mat, _Param, "Hue", 0, -1, 1);
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Param, "Sat", 1, -1, 1);
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Param, "Val", 2, -1, 1);
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
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").x].Add("Hue");
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").y].Add("Sat");
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").z].Add("Val");
        }

        public override void CollectCustomColorData(ref List<List<string>> useCustomDataList)
        {
        }

        public override void CollectUVChannel(ref List<List<string>> useUVChannelList)
        {
        }

        public override void VaridateCustomData()
        {
            UniVFXGUILayout.VaridateCustomDataVector(ref _mat, _Param);
        }

        public override List<string> GetPropertyCode()
        {
            var code = new List<string>();
            return code;
        }
        public override List<string> GetCBufferCode()
        {
            var code = new List<string>();
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


            var param = "param_HSVShift";
            var paramX = VertexDataConvert.VertexDataToCode((int)_mat.GetVector(_Param + "_Data").x, _mat.GetVector(_Param).x + "");
            var paramY = VertexDataConvert.VertexDataToCode((int)_mat.GetVector(_Param + "_Data").y, _mat.GetVector(_Param).y + "");
            var paramZ = VertexDataConvert.VertexDataToCode((int)_mat.GetVector(_Param + "_Data").z, _mat.GetVector(_Param).z + "");
            var paramW = VertexDataConvert.VertexDataToCode((int)_mat.GetVector(_Param + "_Data").w, _mat.GetVector(_Param).w + "");

            code.Add("//HsvShift");

            code.Add("float4 " + param + " = float4(" + paramX + ", " + paramY + ", " + paramZ + ", " + paramW + ");");

            if (MaskTexture.IsActive(_mat) && _mat.GetInt(MaskTexture._TargetHSVShift) == 1)
                code.Add(param + " *= " + MaskTexture._ResultValue + ";");
            if (SurfaceFade.IsActive(_mat) && _mat.GetInt(SurfaceFade._TargetHSVShift) == 1)
                code.Add(param + " *= 1 - " + SurfaceFade._ResultValue + ";");

            code.Add("half4 hsv_k = half4(0.0, -0.3333, 0.6666, -1.0);");
            code.Add("half4 hsv_p = lerp(half4(col.b, col.g, hsv_k.w, hsv_k.z), half4(col.y, col.z, hsv_k.x, hsv_k.y), step(col.b, col.g));");
            code.Add("half4 hsv_q = lerp(half4(hsv_p.x, hsv_p.y, hsv_p.w, col.r), half4(col.r, hsv_p.y, hsv_p.z, hsv_p.x), step(hsv_p.x, col.r));");
            code.Add("half hsv_d = hsv_q.x - min(hsv_q.w, hsv_q.y);");
            code.Add("half3 hsv = half3(abs(hsv_q.z + (hsv_q.w - hsv_q.y) / (6.0 * hsv_d + 0.001)), hsv_d / (hsv_q.x + 0.001), hsv_q.x);");
            code.Add("col.rgb = lerp(half3(1, 1, 1), saturate(3.0 * abs(1.0 - 2.0 * frac((" + param + ".x + hsv.r) + half3(0.0, -0.3333, 0.3333))) - 1), (hsv.g + " + param + ".y)) * (hsv.b + " + param + ".z);");

            code.Add("");

            return code;
        }

    }

}
