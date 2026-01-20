using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


namespace UniVFX.Editor
{
    public class UVParallax : UniVFXOption
    {
        static bool _viewGUI = false;
        static bool _viewTargetGUI = false;
        public const string _IsActive = "_PARALLAXMAPPING";
        public const string _Tex = "_ParallaxTex";
        public const string _Intensity = "_ParallaxAmplitude";
        public const string _TargetMainTex = "_MainUVParallax";
        public const string _TargetBlendTex = "_BlendUVParallax";
        public const string _TargetDissolveTex = "_DissolveUVParallax";
        public const string _ResultValue = "parallaxResult";

        public UVParallax(bool isCanvas, bool isBRP) : base(isCanvas, isBRP)
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
                    _viewGUI = EditorGUILayout.Foldout(_viewGUI, "UV Parallax");
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
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Intensity, "Intensity", -1, 1);
                                    _viewTargetGUI = EditorGUILayout.Foldout(_viewTargetGUI, "Target");
                                    if (_viewTargetGUI)
                                    {
                                        GUI.color = new Color(0f, 0f, 0f, 0.8f);
                                        using (new EditorGUILayout.VerticalScope("Box"))
                                        {
                                            GUI.color = new Color(1f, 1f, 1f, 1.0f);
                                            UniVFXGUILayout.OptionBoolField(ref _mat, _TargetMainTex, "Main Texture");
                                            UniVFXGUILayout.OptionBoolField(ref _mat, _TargetBlendTex, "Blend Texture");
                                            UniVFXGUILayout.OptionBoolField(ref _mat, _TargetDissolveTex, "Dissolve Texture");
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
            useCustomDataList[(int)_mat.GetFloat(_Intensity + "_Data")].Add("Parallax Intensity");
        }

        public override void CollectCustomColorData(ref List<List<string>> useCustomDataList)
        {

        }

        public override void CollectUVChannel(ref List<List<string>> useUVChannelList)
        {
        }
        

        public override void VaridateCustomData()
        {
            UniVFXGUILayout.VaridateCustomDataInt(ref _mat, _Intensity, _isCanvas);
        }

        public override List<string> GetPropertyCode(RefactOption refactOption)
        {
            var code = new List<string>();
            if (!IsActive())
                return code;
            var intensity = UniVFXGUILayout.VertexDataToCode(_mat.GetInt(_Intensity + "_Data"), _mat.GetFloat(_Intensity) + "", _isCanvas);
            if(intensity == "0")
            {
                code.Add("//UV Parallax Skipped, Intensity is 0");
                return code;
            }
            code.Add("[NoScaleOffset]" + _Tex + "(\"" + _Tex.Replace("_", "") + "\", 2D) = \"white\" {}");
            return code;
        }
        public override List<string> GetCBufferCode(RefactOption refactOption)
        {
            var code = new List<string>();
            return code;
        }
        public override List<string> GetTextureCode(RefactOption refactOption)
        {
            var code = new List<string>();
            if (!IsActive())
                return code;
            var intensity = UniVFXGUILayout.VertexDataToCode(_mat.GetInt(_Intensity + "_Data"), _mat.GetFloat(_Intensity) + "", _isCanvas);
            if(intensity == "0")
            {
                code.Add("//UV Parallax Skipped, Intensity is 0");
                return code;
            }
            code.Add("TEXTURE2D(" + _Tex + ");");
            return code;
        }
        public override List<string> GetUseV2fCode(RefactOption refactOption)
        {
            var code = new List<string>();
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
            return code;
        }
        public override List<string> GetFragmentHeadCode(RefactOption refactOption)
        {
            var code = new List<string>();
            if (!IsActive())
                return code;

            var intensity = UniVFXGUILayout.VertexDataToCode(_mat.GetInt(_Intensity + "_Data"), _mat.GetFloat(_Intensity) + "", _isCanvas);
            var uv = "uv_" + _Tex.Replace("_", "");
            var tex = "tex_" + _Tex.Replace("_", "");

            code.Add("//UVParallax");
            if(intensity == "0")
            {
                code.Add("float2 " + _ResultValue + " = 0;");
                code.Add("//UV Parallax Skipped, Intensity is 0");
                code.Add("");
                return code;
            }

            code.Add("float2 " + uv + " = i.texCoord0.xy;");
            code.Add("half4 " + tex + " = SAMPLE_TEXTURE2D(" + _Tex + ", SamplerState_Linear_Clamp, " + uv + ");");
            code.Add(tex + ".x -= 0.5;");
            code.Add("float2 parallax = tangentSpaceViewDirection.xy * " + tex + ".x * " + intensity + " / tangentSpaceViewDirection.z;");
            if (MaskTexture.IsActive(_mat) && _mat.GetInt(MaskTexture._TargetParallax) == 1)
                code.Add( "parallax *= " + MaskTexture._ResultValue + ";");

            code.Add("float2 " + _ResultValue + " = parallax;");
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
