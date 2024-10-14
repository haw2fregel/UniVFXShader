using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;


namespace UniVFX.Editor
{
    public class VertexAnimation : UniVFXOption
    {
        const string _IsActive = "_VERTEXANIMATION";
        const string _Tex = "_VertexAnimTex";
        const string _UV = "_VertexAnimUV";
        const string _Param = "_VertexAnimParam";

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
            return 0;
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
                    _viewGUI = EditorGUILayout.Foldout(_viewGUI, "Vertex Animation");
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
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Param, "X", 0, -1, 1);
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Param, "Y", 1, -1, 1);
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Param, "Z", 2, -1, 1);
                                    UniVFXGUILayout.OptionSlider(ref _mat, _Param, "Intensity", 3, -1, 1);
                                    UniVFXGUILayout.UVGUILayoutVert(ref _mat, ref _viewUVGUI, _UV);
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
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").x].Add("VertexAnim Tile X");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").y].Add("VertexAnim Tile Y");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").z].Add("VertexAnim Offset X");
            useCustomDataList[(int)_mat.GetVector(_UV + "Transform_Data").w].Add("VertexAnim Offset Y");
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").x].Add("VertexAnim X");
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").y].Add("VertexAnim Y");
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").z].Add("VertexAnim Z");
            useCustomDataList[(int)_mat.GetVector(_Param + "_Data").w].Add("VertexAnim Intensity");

        }

        public override void CollectCustomColorData(ref List<List<string>> useCustomDataList)
        {

        }


        public override void VaridateCustomData()
        {
            UniVFXGUILayout.VaridateCustomDataVector(ref _mat, _UV + "Transform");
            UniVFXGUILayout.VaridateArrayIndex(ref _mat, _UV + "Transform_Index", UniVFXGUILayout._UVChannelOptionVert);
            UniVFXGUILayout.VaridateCustomDataVector(ref _mat, _Param);
        }


    }

}
