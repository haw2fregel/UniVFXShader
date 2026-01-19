using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


namespace UniVFX.Editor
{
    public class SurfaceFade : UniVFXOption
    {
        const string _IsActive = "_SURFACEFADE";
        const string _Frenel = "_Frenel";
        const string _Pow = "_FrenelPow";
        const string _Reverce = "_FrenelReverce";
        const string _FadeIn = "_SurfaceFadeIn";
        const string _FadeOut = "_SurfaceFadeOut";
        const string _Type = "_SurfaceFadeType";
        readonly static string[] _TypeOption = { "Screen Z", "Height", "SoftParticle", "Off" };

        public const string _TargetFinalColor = "_FinalColorSurfaceFade";
        public const string _TargetFinalAlpha = "_FinalAlphaSurfaceFade";
        public const string _TargetMainTex = "_MainTexSurfaceFade";
        public const string _TargetBlendColor = "_BlendColorSurfaceFade";
        public const string _TargetBlendTex = "_BlendTexSurfaceFade";
        public const string _TargetHSVShift = "_HSVShiftSurfaceFade";
        public const string _TargetDistortion = "_DistortionSurfaceFade";
        public const string _TargetDissolve = "_DissolveSurfaceFade";
        public const string _ResultValue = "surfaceFadeResult";

        public SurfaceFade(bool isCanvas, bool isBRP) : base(isCanvas, isBRP)
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
            return 30;
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
                    _viewGUI = EditorGUILayout.Foldout(_viewGUI, "Surface Fade");
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

                                    var frenel = UniVFXGUILayout.OptionBoolField(ref _mat, _Frenel, "Frenel");
                                    if(frenel)
                                    {
                                        UniVFXGUILayout.OptionSlider(ref _mat, _Pow, "Power", 0.001f, 50);
                                        UniVFXGUILayout.OptionBoolField(ref _mat, _Reverce, "Reverce");
                                    }

                                    var type = UniVFXGUILayout.OptionPopupField(ref _mat, _Type, "Position Fade Type", _TypeOption);
                                    if (type < 3)
                                    {
                                        UniVFXGUILayout.OptionSlider(ref _mat, _FadeIn, "FadeIn", -10, 10);
                                        UniVFXGUILayout.OptionSlider(ref _mat, _FadeOut, "FadeOut", -10, 10);
                                    }

                                    

                                    _viewTargetGUI = EditorGUILayout.Foldout(_viewTargetGUI, "Target");
                                    if (_viewTargetGUI)
                                    {
                                        GUI.color = new Color(0f, 0f, 0f, 0.8f);
                                        using (new EditorGUILayout.VerticalScope("Box"))
                                        {
                                            GUI.color = new Color(1f, 1f, 1f, 1.0f);
                                            UniVFXGUILayout.OptionBoolField(ref _mat, _TargetFinalColor, "Final Color");
                                            UniVFXGUILayout.OptionBoolField(ref _mat, _TargetFinalAlpha, "Final Alpha");
                                            UniVFXGUILayout.OptionBoolField(ref _mat, _TargetMainTex, "Main Texture");
                                            var blend = UniVFXGUILayout.OptionBoolField(ref _mat, _TargetBlendTex, "Blend Texture");
                                            var distortion = UniVFXGUILayout.OptionBoolField(ref _mat, _TargetDistortion, "Distortion");
                                            var dissolve = UniVFXGUILayout.OptionBoolField(ref _mat, _TargetDissolve, "Dissolve");
                                            var hsv = UniVFXGUILayout.OptionBoolField(ref _mat, _TargetHSVShift, "HSV Shift");

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
            useCustomDataList[_mat.GetInt(_Pow + "_Data")].Add("Frenel Power");
            useCustomDataList[_mat.GetInt(_FadeIn + "_Data")].Add("Surface FadeIn");
            useCustomDataList[_mat.GetInt(_FadeOut + "_Data")].Add("Surface FadeOut");
        }

        public override void CollectCustomColorData(ref List<List<string>> useCustomDataList)
        {

        }

        public override void CollectUVChannel(ref List<List<string>> useUVChannelList)
        {
        }

        public override void VaridateCustomData()
        {
            if (!IsActive())
                return;
            UniVFXGUILayout.VaridateCustomDataInt(ref _mat, _Pow, _isCanvas);
            UniVFXGUILayout.VaridateCustomDataInt(ref _mat, _FadeIn, _isCanvas);
            UniVFXGUILayout.VaridateCustomDataInt(ref _mat, _FadeOut, _isCanvas);
        }

        public override List<string> GetPropertyCode()
        {
            var code = new List<string>();
            if (!IsActive())
                return code;

            if (_mat.GetInt(_Pow + "_Data") == 0 && _mat.GetInt(_Frenel) == 1)
                code.Add(_Pow + "(\"" + _Pow.Replace("_", "") + "\", float) = 1");
            if (_mat.GetInt(_FadeIn + "_Data") == 0)
                code.Add(_FadeIn + "(\"" + _FadeIn.Replace("_", "") + "\", float) = 1");
            if(_mat.GetInt(_FadeOut + "_Data") == 0)
                code.Add(_FadeOut + "(\"" + _FadeOut.Replace("_", "") + "\", float) = 1");
            return code;
        }
        public override List<string> GetCBufferCode()
        {
            var code = new List<string>();
            if (!IsActive())
                return code;

            if (_mat.GetInt(_Pow + "_Data") == 0 && _mat.GetInt(_Frenel) == 1)
                code.Add("half " + _Pow + ";");
            if (_mat.GetInt(_FadeIn + "_Data") == 0)
                code.Add("float " + _FadeIn + ";");
            if (_mat.GetInt(_FadeOut + "_Data") == 0)
                code.Add("float " + _FadeOut + ";");
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
            if (!IsActive())
                return code;

            var fadeIn = UniVFXGUILayout.VertexDataToCode(_mat.GetInt(_FadeIn + "_Data"), _FadeIn, _isCanvas);
            var fadeOut = UniVFXGUILayout.VertexDataToCode(_mat.GetInt(_FadeOut + "_Data"), _FadeOut, _isCanvas);

            code.Add("//SurfaceFade");
            code.Add("half " + _ResultValue + " = 1;");

            if (_mat.GetInt(_Frenel) == 1)
            {
                var pow = UniVFXGUILayout.VertexDataToCode(_mat.GetInt(_Pow + "_Data"), _Pow, _isCanvas);
                code.Add("float rimFade = saturate(dot(i.normal, GetWorldSpaceNormalizeViewDir(i.worldPos)));");
                code.Add("rimFade = pow(rimFade, " + pow + ");");
                if (_mat.GetInt(_Reverce) == 1)
                    code.Add("rimFade = 1 - rimFade;");
                code.Add(_ResultValue + " = rimFade;");
            }
            
            switch (_TypeOption[_mat.GetInt(_Type)])
            {
                case "Screen Z":
                    code.Add(_ResultValue + " *= smoothstep(" + fadeOut + ", " + fadeIn + ", -TransformWorldToView(i.worldPos).z);");
                    break;
                case "Height":
                    code.Add(_ResultValue + " *= smoothstep(" + fadeOut + ", " + fadeIn + ", i.worldPos.y);");
                    break;
                case "SoftParticle":
                    code.Add("float2 pixelPosition = 0;");
                    code.Add("#if UNITY_UV_STARTS_AT_TOP");
                    code.Add("    pixelPosition = float2(i.positionCS.x, (_ProjectionParams.x < 0) ? (_ScaledScreenParams.y - i.positionCS.y) : i.positionCS.y);");
                    code.Add("#else");
                    code.Add("    pixelPosition = float2(i.positionCS.x, (_ProjectionParams.x > 0) ? (_ScaledScreenParams.y - i.positionCS.y) : i.positionCS.y);");
                    code.Add("#endif");
                    code.Add("float2 ndcPosition = pixelPosition.xy / _ScaledScreenParams.xy;");
                    code.Add("ndcPosition.y = 1.0f - ndcPosition.y;");
                    code.Add("float sceneDepth = Linear01Depth(SampleSceneDepth(ndcPosition), _ZBufferParams) * (_ProjectionParams.z - _ProjectionParams.y);");
                    code.Add("float depthDiff = sceneDepth + TransformWorldToView(i.worldPos).z;");
                    code.Add(_ResultValue + " *= smoothstep(" + fadeOut + ", " + fadeIn + ", depthDiff);");
                    break;
                case "Off":
                    break;
                default:
                    break;
            }

            if (MaskTexture.IsActive(_mat) && _mat.GetInt(MaskTexture._TargetSurfaceFade) == 1)
                code.Add(_ResultValue + "= 1 - (1 - " + _ResultValue + ") * " + MaskTexture._ResultValue + ";");

            code.Add("");
            return code;
        }
        public override List<string> GetFragmentCode()
        {
            var code = new List<string>();
            if (!IsActive())
                return code;

            code.Add("//SurfaceFade");
            if (_mat.GetInt(_TargetFinalColor) == 1)
                code.Add("col.rgb *= " + _ResultValue + ";");
            if (_mat.GetInt(_TargetFinalAlpha) == 1)
                code.Add("col.a *= " + _ResultValue + ";");
            
            return code;
        }
       
    }

}
