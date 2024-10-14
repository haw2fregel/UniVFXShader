using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


namespace UniVFX.Editor
{
    public class FakeLight : UniVFXOption
    {
        const string _IsActive = "_FAKELIGHT";
        const string _Tex = "_FakeLightMap";
        const string _Param = "_FakeLightParam";
        const string _Intensity = "_FakeLightIntensity";
        const string _LightColor = "_FakeLightColor";
        const string _ShadowColor = "_FakeShadowColor";
        readonly static string _Type = "_FakeLightType";
        readonly static string[] _TypeOption = { "Direction", "Point" };

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
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").x].Add("FakeLight X");
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").y].Add("FakeLight Y");
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").z].Add("FakeLight Z");
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").w].Add("FakeLight Distance");
            useCustomDataList[_mat.GetInt(_Intensity + "_Data")].Add("FakeLight Intensity");
        }

        public override void CollectCustomColorData(ref List<List<string>> useCustomDataList)
        {
            useCustomDataList[_mat.GetInt(_LightColor + "_Data")].Add("FakeLight Color");
            useCustomDataList[_mat.GetInt(_ShadowColor + "_Data")].Add("FakeShadow Color");
        }

        public override void VaridateCustomData()
        {
            UniVFXGUILayout.VaridateCustomDataInt(ref _mat, _Intensity);
            UniVFXGUILayout.VaridateCustomDataVector(ref _mat, _Param);
            UniVFXGUILayout.VaridateCustomColorDataInt(ref _mat, _LightColor);
            UniVFXGUILayout.VaridateCustomColorDataInt(ref _mat, _ShadowColor);
        }

    }

}
