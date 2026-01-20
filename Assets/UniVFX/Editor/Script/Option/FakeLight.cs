using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


namespace UniVFX.Editor
{
    public class FakeLight : UniVFXOption
    {
        static bool _viewGUI = false;
        const string _IsActive = "_FAKELIGHT";
        const string _Tex = "_FakeLightMap";
        const string _Param = "_FakeLightParam";
        const string _Intensity = "_FakeLightIntensity";
        const string _LightColor = "_FakeLightColor";
        const string _ShadowColor = "_FakeShadowColor";
        readonly static string _Type = "_FakeLightType";
        readonly static string[] _TypeOption = { "Direction", "Point" };

        public FakeLight(bool isCanvas, bool isBRP) : base(isCanvas, isBRP)
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
            return 31;
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
                    _viewGUI = EditorGUILayout.Foldout(_viewGUI, "Fake Light");
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
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Intensity, "Intensity", 0, 5);
                                    UniVFXGUILayout.OptionColorField(ref _mat, _LightColor, "Light Color");
                                    UniVFXGUILayout.OptionColorField(ref _mat, _ShadowColor, "Shadow Color");
                                    EditorGUILayout.Space();

                                    var type = UniVFXGUILayout.OptionPopupField(ref _mat, _Type, "Light Type", _TypeOption);
                                    if (type == 0)
                                    {
                                        UniVFXGUILayout.OptionSlider(ref _mat, _Param, "X", 0, -1, 1);
                                        UniVFXGUILayout.OptionSlider(ref _mat, _Param, "Y", 1, -1, 1);
                                        UniVFXGUILayout.OptionSlider(ref _mat, _Param, "Z", 2, -1, 1);
                                    }
                                    else
                                    {
                                        UniVFXGUILayout.OptionSlider(ref _mat, _Param, "X", 0, -10, 10);
                                        UniVFXGUILayout.OptionSlider(ref _mat, _Param, "Y", 1, -10, 10);
                                        UniVFXGUILayout.OptionSlider(ref _mat, _Param, "Z", 2, -10, 10);
                                        UniVFXGUILayout.OptionSlider(ref _mat, _Param, "Distance", 3, 0.001f, 10);
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
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").x].Add("FakeLight X");
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").y].Add("FakeLight Y");
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").z].Add("FakeLight Z");
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").w].Add("FakeLight Distance");
            useCustomDataList[_mat.GetInt(_Intensity + "_Data")].Add("FakeLight Intensity");
        }

        public override void CollectCustomColorData(ref List<List<string>> useCustomDataList)
        {
            if (!IsActive())
                return;
            useCustomDataList[_mat.GetInt(_LightColor + "_Data")].Add("FakeLight Color");
            useCustomDataList[_mat.GetInt(_ShadowColor + "_Data")].Add("FakeShadow Color");
        }

        public override void CollectUVChannel(ref List<List<string>> useUVChannelList)
        {
        }

        public override void VaridateCustomData()
        {
            UniVFXGUILayout.VaridateCustomDataInt(ref _mat, _Intensity, _isCanvas);
            UniVFXGUILayout.VaridateCustomDataVector(ref _mat, _Param, _isCanvas);
            UniVFXGUILayout.VaridateCustomColorDataInt(ref _mat, _LightColor, _isCanvas);
            UniVFXGUILayout.VaridateCustomColorDataInt(ref _mat, _ShadowColor, _isCanvas);
        }

        public override List<string> GetPropertyCode(RefactOption refactOption)
        {
            var code = new List<string>();
            if (!IsActive())
                return code;

            var isFixedValue = refactOption.HasFlag(RefactOption.PropertiesToFixedValue);
            var isTextureNone = _mat.GetTexture(_Tex) == null && refactOption.HasFlag(RefactOption.NoneTextureToFixedValue);
            

            if (isTextureNone)
            {
                code.Add("//FakeLight Skipped, Texture is null");
            }else
            {
                code.Add("[NoScaleOffset]" + _Tex + "(\"" + _Tex.Replace("_", "") + "\", 2D) = \"white\" {}");
            }

            if(isFixedValue)
            {
                code.Add("//FakeLight Property Skipped, FixedValue");
            }
            else
            {
                if (_mat.GetInt(_LightColor + "_Data") == 0)
                    code.Add("[HDR]" + _LightColor + "(\"" + _LightColor.Replace("_", "") + "\", Color) = (1,1,1,1)");
                if (_mat.GetInt(_ShadowColor + "_Data") == 0)
                    code.Add(_ShadowColor + "(\"" + _ShadowColor.Replace("_", "") + "\", Color) = (1,1,1,1)");
                if(_mat.GetInt(_Intensity + "_Data") == 0)
                    code.Add(_Intensity + "(\"" + _Intensity.Replace("_", "") + "\", float) = 1");
                if(_mat.GetVector(_Param + "_Data") == new Vector4(0,0,0,0))
                    code.Add(_Param + "(\"" + _Param.Replace("_", "") + "\", Vector) = (1,0,0,0)");
            }
            
            return code;
        }
        public override List<string> GetCBufferCode(RefactOption refactOption)
        {
            var code = new List<string>();
            if (!IsActive())
                return code;

            var isFixedValue = refactOption.HasFlag(RefactOption.PropertiesToFixedValue);
            

            if(isFixedValue)
            {
                code.Add("//FakeLight Property Skipped, FixedValue");
            }
            else
            {
                if (_mat.GetInt(_LightColor + "_Data") == 0)
                    code.Add("half4 " + _LightColor + ";");
                if (_mat.GetInt(_ShadowColor + "_Data") == 0)
                    code.Add("half4 " + _ShadowColor + ";");
                if(_mat.GetInt(_Intensity + "_Data") == 0)
                    code.Add("half " + _Intensity + ";");
                if(_mat.GetVector(_Param + "_Data") == new Vector4(0,0,0,0))
                    code.Add("half4 " + _Param + ";");
            }
            return code;
        }
        public override List<string> GetTextureCode(RefactOption refactOption)
        {
            var code = new List<string>();

            var isTextureNone = _mat.GetTexture(_Tex) == null && refactOption.HasFlag(RefactOption.NoneTextureToFixedValue);

            if (isTextureNone)
            {
                code.Add("//FakeLight Skipped, Texture is null");
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
            return code;
        }
        public override List<string> GetFragmentCode(RefactOption refactOption)
        {
            var code = new List<string>();

            if (!IsActive())
                return code;

            var intensity = UniVFXGUILayout.VertexDataToFloatCode(_mat, _Intensity, _isCanvas, refactOption);
            var isTextureNone = _mat.GetTexture(_Tex) == null && refactOption.HasFlag(RefactOption.NoneTextureToFixedValue);

            var lightColor = UniVFXGUILayout.VertexDataToColorCode(_mat, _LightColor, false, _isCanvas, refactOption);
            var shadowColor = UniVFXGUILayout.VertexDataToColorCode(_mat, _ShadowColor, true, _isCanvas, refactOption);
            
            var uv = "uv_" + _Tex.Replace("_", "");
            var tex = "tex_" + _Tex.Replace("_", "");

            var param = "param_" + _Tex.Replace("_", "");
            var paramX = UniVFXGUILayout.VertexDataToVectorCode(_mat, _Param, 0, _isCanvas, refactOption);
            var paramY = UniVFXGUILayout.VertexDataToVectorCode(_mat, _Param, 1, _isCanvas, refactOption);
            var paramZ = UniVFXGUILayout.VertexDataToVectorCode(_mat, _Param, 2, _isCanvas, refactOption);
            var paramW = UniVFXGUILayout.VertexDataToVectorCode(_mat, _Param, 3, _isCanvas, refactOption);
                    
            code.Add("//FakeLight");

            code.Add("float4 " + param + " = float4(" + paramX + ", " + paramY + ", " + paramZ + ", " + paramW + ");");

            if (_mat.GetInt(_Type) == 0)
            {
                code.Add("float3 fakeLightDir = " + param + ".xyz;");
                code.Add("float attan = 1;");
            }
            else
            {
                code.Add("float3 fakeLightDir = i.worldPos.xyz - (i.texCoord3.xyz + " + param + ".xyz);");
                code.Add("float attan = min(length(fakeLightDir), " + param + ".w);");
                code.Add("attan = 1 - (attan / " + param + ".w);");
                code.Add("attan *= attan;");
            }

            code.Add("float fakeLightIntensity = saturate(" + intensity + " * attan);");

            if (MaskTexture.IsActive(_mat) && _mat.GetInt(MaskTexture._TargetFakeLight) == 1)
                code.Add("fakeLightIntensity *= " + MaskTexture._ResultValue + ";");

            code.Add("float nDotL = -dot(TransformObjectToWorldDir(i.normal.xyz), normalize(fakeLightDir)) * 0.5 + 0.5;");

            if (isTextureNone)
            {
                code.Add("half4 " + tex + " = nDotL.xxxx;");
                code.Add("//FakeLight Skipped, Texture is null");
            }
            else
            {
                code.Add("half4 " + tex + " = SAMPLE_TEXTURE2D(" + _Tex + ", SamplerState_Linear_Clamp, nDotL.xx);");
            }

            code.Add("col.rgb = lerp(col.rgb, col.rgb * " + shadowColor + ".rgb, (1 - saturate(" + tex + ".x * 2)) * fakeLightIntensity);");
            code.Add("col.rgb = lerp(col.rgb, col.rgb + " + lightColor + ".rgb, saturate((" + tex + ".x - 0.5) * 2) * fakeLightIntensity);");

            code.Add("");

            return code;
        }

    }

}
