using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


namespace UniVFX.Editor
{
    public class UVFripBook : UniVFXOption
    {
        protected const string _IsActive = "_FRIPBOOK";
        protected const string _Row = "_FripBookRow";
        protected const string _Column = "_FripBookColumn";
        protected const string _Index = "_FripBookIndex";

        public UVFripBook(bool isCanvas, bool isBRP) : base(isCanvas, isBRP)
        {
        }


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
                    _viewGUI = EditorGUILayout.Foldout(_viewGUI, "UV FripBook");
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
                                    var row = UniVFXGUILayout.IntSlider(ref _mat, _Row, "Row", 1, 8);
                                    var column = UniVFXGUILayout.IntSlider(ref _mat, _Column, "Column", 1, 8);
                                    UniVFXGUILayout.OptionIntSlider(ref _mat, _Index, "Index", 0, row * column - 1);
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
            useCustomDataList[(int)_mat.GetInt(_Index + "_Data")].Add("FripBook Row");
        }

        public override void CollectCustomColorData(ref List<List<string>> useCustomDataList)
        {

        }

        public override void CollectUVChannel(ref List<List<string>> useUVChannelList)
        {
        }


        public override void VaridateCustomData()
        {
            UniVFXGUILayout.VaridateCustomDataInt(ref _mat, _Index, _isCanvas);
        }

        public override List<string> GetPropertyCode()
        {
            var code = new List<string>();
            if (!IsActive())
                return code;

            code.Add(_Row + "(\"" + _Row.Replace("_", "") + "\", float) = 0");
            code.Add(_Column + "(\"" + _Column.Replace("_", "") + "\", float) = 0");
            if (_mat.GetInt(_Index + "_Data") == 0)
                code.Add(_Index + "(\"" + _Index.Replace("_", "") + "\", float) = 0");
            return code;
        }
        public override List<string> GetCBufferCode()
        {
            var code = new List<string>();
            if (!IsActive())
                return code;

            code.Add("half " + _Row + ";");
            code.Add("half " + _Column + ";");
            if(_mat.GetInt(_Index + "_Data") == 0)
                code.Add("half " + _Index + ";");
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
            if (!IsActive())
                return code;

            var index = VertexDataConvert.VertexDataToCode(_mat.GetInt(_Index + "_Data"), _Index);
            var uv = "fripBookUV";

            code.Add("//FripBookUV");
            code.Add("float2 " + uv + " = 0;");
            code.Add("float invRow = 1.0 / (float)" + _Row + ";");
            code.Add("float invColumn = 1.0 / (float)" + _Column + ";");
            code.Add("int indexColumn = " + index + " / _FripBookRow;");
            code.Add("int indexRow = " + index + " % _FripBookRow;");
            code.Add("float2 fripbook_tiling = float2(invRow, invColumn);");
            code.Add("float2 fripbook_offset = float2(indexRow * invRow, indexColumn * invColumn);");
            code.Add(uv + " = (texCoord0.xy * fripbook_tiling) + fripbook_offset;");
            code.Add("");
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
