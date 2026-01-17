using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


namespace UniVFX.Editor
{
    public class Time : UniVFXOption
    {
        public const string _Speed = "_TimeSpeed";
        public const string _Tex = "_TimeMap";

        public override bool IsActive()
        {
            return true;
        }

        public override void SetActive(bool active)
        {
        }
        public override int HeatValue()
        {
            return 0;
        }

        public override void GetHeatValue(ref int value, ref int max)
        {
            max += HeatValue();
            if (IsActive())
                value += HeatValue();
        }

        public override void OptionGUI()
        {
            GUI.color = new Color(5f, 5f, 5f, 1.0f);
            using (new EditorGUILayout.VerticalScope("Box"))
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    GUI.color = new Color(1f, 1f, 1f, 1.0f);
                    _viewGUI = EditorGUILayout.Foldout(_viewGUI, "Time");
                    if (_viewGUI)
                    {
                        GUI.color = new Color(0f, 0f, 0f, 0.6f);
                        using (new EditorGUILayout.VerticalScope("Box"))
                        {
                            using (new EditorGUI.IndentLevelScope())
                            {
                                GUI.color = new Color(1f, 1f, 1f, 1f);
                                UniVFXGUILayout.FloatField(ref _mat, _Speed, "Speed");
                                UniVFXGUILayout.OptionTextureField(ref _mat, _Tex, "Texture");
                            }
                        }
                    }
                }
            }
            GUI.color = new Color(1f, 1f, 1f, 1f);
        }

        public override void CollectCustomData(ref List<List<string>> useCustomDataList)
        {
        }

        public override void CollectCustomColorData(ref List<List<string>> useCustomDataList)
        {
        }

        public override void CollectUVChannel(ref List<List<string>> useUVChannelList)
        {
        }

        public override void VaridateCustomData()
        {
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
            return code;
        }
    }
}
