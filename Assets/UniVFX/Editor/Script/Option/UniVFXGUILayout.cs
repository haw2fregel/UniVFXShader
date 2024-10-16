﻿using UnityEngine;
using UnityEditor;
using System;

namespace UniVFX.Editor
{
    public static class UniVFXGUILayout
    {
        public readonly static string[] _CanvasUVChannelOption = { "TEXCOORD", "TEXCOORD1", "ScreenPosition", "RotateUV", "FripBookUV", "BendUV" };
        public readonly static string[] _CanvasUVChannelOptionVert = { "TEXCOORD", "TEXCOORD1", "ScreenPosition", "RotateUV", "FripBookUV" };
        public readonly static string[] _UVChannelOption = { "TEXCOORD", "PositionZFace", "PositionYFace", "PositionXFace", "ScreenPosition", "ViewNormal", "RotateUV", "FripBookUV", "BendUV" };
        public readonly static string[] _UVChannelOptionVert = { "TEXCOORD", "PositionZFace", "PositionYFace", "PositionXFace", "ScreenPosition", "ViewNormal", "RotateUV", "FripBookUV" };
        public readonly static string[] _WrapMode = { "Clamp", "Repeat", "Mirror", "MirrorOnce" };

        /// MARK: UVGUILayout
        /// <summary>
        /// UVField表示のテンプレ
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="viewGUI"></param>
        /// <param name="property">uv name, "+Transform" "+Transform_Index" "+Transform_Sampler"</param>
        public static void UVGUILayout(ref Material mat, ref bool viewGUI, string property)
        {
            viewGUI = EditorGUILayout.Foldout(viewGUI, "UV");
            if (viewGUI)
            {
                GUI.color = new Color(0f, 0f, 0f, 0.8f);
                using (new EditorGUILayout.VerticalScope("Box"))
                {
                    GUI.color = new Color(1f, 1f, 1f, 1.0f);

                    var index = OptionPopupField(ref mat, property + "Transform_Index", "UV Channel", _UVChannelOption);
                    OptionPopupField(ref mat, property + "Transform_Sampler", "Wrap Mode", _WrapMode);
                    OptionFloatField(ref mat, property + "Transform", "Tile X", 0);
                    OptionFloatField(ref mat, property + "Transform", "Tile Y", 1);
                    OptionFloatField(ref mat, property + "Transform", "Offset X", 2);
                    OptionFloatField(ref mat, property + "Transform", "Offset Y", 3);

                    if (index == 6 && !UVRotate.IsActive(mat))
                    {
                        var rect = EditorGUILayout.GetControlRect();
                        rect.height = 40;
                        rect.xMin += 30;
                        EditorGUI.HelpBox(rect, "Please UV Rotate Active", MessageType.Warning);
                        rect.xMin = rect.xMax - 70;
                        if (GUI.Button(rect, "Fix now"))
                            UVRotate.SetActive(mat, true);
                        EditorGUILayout.GetControlRect();
                    }

                    if (index == 7 && !UVFripBook.IsActive(mat))
                    {
                        var rect = EditorGUILayout.GetControlRect();
                        rect.height = 40;
                        rect.xMin += 30;
                        EditorGUI.HelpBox(rect, "Please UV FripBook Active", MessageType.Warning);
                        rect.xMin = rect.xMax - 70;
                        if (GUI.Button(rect, "Fix now"))
                            UVFripBook.SetActive(mat, true);
                        EditorGUILayout.GetControlRect();
                    }

                    if (index == 8 && !UVBend.IsActive(mat))
                    {
                        var rect = EditorGUILayout.GetControlRect();
                        rect.height = 40;
                        rect.xMin += 30;
                        EditorGUI.HelpBox(rect, "Please UV Bend Active", MessageType.Warning);
                        rect.xMin = rect.xMax - 70;
                        if (GUI.Button(rect, "Fix now"))
                            UVBend.SetActive(mat, true);
                        EditorGUILayout.GetControlRect();
                    }
                }
            }
        }

        /// MARK: UVGUILayout
        /// <summary>
        /// UVField表示のテンプレ
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="viewGUI"></param>
        /// <param name="property">uv name, "+Transform" "+Transform_Index" "+Transform_Sampler"</param>
        public static void CanvasUVGUILayout(ref Material mat, ref bool viewGUI, string property)
        {
            viewGUI = EditorGUILayout.Foldout(viewGUI, "UV");
            if (viewGUI)
            {
                GUI.color = new Color(0f, 0f, 0f, 0.8f);
                using (new EditorGUILayout.VerticalScope("Box"))
                {
                    GUI.color = new Color(1f, 1f, 1f, 1.0f);

                    var index = OptionPopupField(ref mat, property + "Transform_Index", "UV Channel", _CanvasUVChannelOption);
                    OptionPopupField(ref mat, property + "Transform_Sampler", "Wrap Mode", _WrapMode);
                    CanvasOptionFloatField(ref mat, property + "Transform", "Tile X", 0);
                    CanvasOptionFloatField(ref mat, property + "Transform", "Tile Y", 1);
                    CanvasOptionFloatField(ref mat, property + "Transform", "Offset X", 2);
                    CanvasOptionFloatField(ref mat, property + "Transform", "Offset Y", 3);

                    if (index == 6 && !UVRotate.IsActive(mat))
                    {
                        var rect = EditorGUILayout.GetControlRect();
                        rect.height = 40;
                        rect.xMin += 30;
                        EditorGUI.HelpBox(rect, "Please UV Rotate Active", MessageType.Warning);
                        rect.xMin = rect.xMax - 70;
                        if (GUI.Button(rect, "Fix now"))
                            UVRotate.SetActive(mat, true);
                        EditorGUILayout.GetControlRect();
                    }

                    if (index == 7 && !UVFripBook.IsActive(mat))
                    {
                        var rect = EditorGUILayout.GetControlRect();
                        rect.height = 40;
                        rect.xMin += 30;
                        EditorGUI.HelpBox(rect, "Please UV FripBook Active", MessageType.Warning);
                        rect.xMin = rect.xMax - 70;
                        if (GUI.Button(rect, "Fix now"))
                            UVFripBook.SetActive(mat, true);
                        EditorGUILayout.GetControlRect();
                    }

                    if (index == 8 && !UVBend.IsActive(mat))
                    {
                        var rect = EditorGUILayout.GetControlRect();
                        rect.height = 40;
                        rect.xMin += 30;
                        EditorGUI.HelpBox(rect, "Please UV Bend Active", MessageType.Warning);
                        rect.xMin = rect.xMax - 70;
                        if (GUI.Button(rect, "Fix now"))
                            UVBend.SetActive(mat, true);
                        EditorGUILayout.GetControlRect();
                    }
                }
            }
        }

        /// MARK: UVGUILayout
        /// <summary>
        /// UVField表示のテンプレ
        /// 頂点シェーダー上でサンプルするテクスチャ用 
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="viewGUI"></param>
        /// <param name="property">uv name, "+Transform" "+Transform_Index" "+Transform_Sampler"</param>
        public static void UVGUILayoutVert(ref Material mat, ref bool viewGUI, string property)
        {
            viewGUI = EditorGUILayout.Foldout(viewGUI, "UV");
            if (viewGUI)
            {
                GUI.color = new Color(0f, 0f, 0f, 0.8f);
                using (new EditorGUILayout.VerticalScope("Box"))
                {
                    GUI.color = new Color(1f, 1f, 1f, 1.0f);

                    var index = OptionPopupField(ref mat, property + "Transform_Index", "UV Channel", _UVChannelOptionVert);
                    OptionPopupField(ref mat, property + "Transform_Sampler", "Wrap Mode", _WrapMode);
                    OptionFloatField(ref mat, property + "Transform", "Tile X", 0);
                    OptionFloatField(ref mat, property + "Transform", "Tile Y", 1);
                    OptionFloatField(ref mat, property + "Transform", "Offset X", 2);
                    OptionFloatField(ref mat, property + "Transform", "Offset Y", 3);


                    if (index == 6 && !UVRotate.IsActive(mat))
                    {
                        var rect = EditorGUILayout.GetControlRect();
                        rect.height = 40;
                        rect.xMin += 30;
                        EditorGUI.HelpBox(rect, "Please UV Rotate Active", MessageType.Warning);
                        rect.xMin = rect.xMax - 70;
                        if (GUI.Button(rect, "Fix now"))
                            UVRotate.SetActive(mat, true);
                        EditorGUILayout.GetControlRect();
                    }

                    if (index == 7 && !UVFripBook.IsActive(mat))
                    {
                        var rect = EditorGUILayout.GetControlRect();
                        rect.height = 40;
                        rect.xMin += 30;
                        EditorGUI.HelpBox(rect, "Please UV FripBook Active", MessageType.Warning);
                        rect.xMin = rect.xMax - 70;
                        if (GUI.Button(rect, "Fix now"))
                            UVFripBook.SetActive(mat, true);
                        EditorGUILayout.GetControlRect();
                    }
                }
            }
        }

        /// MARK: CanvasUVGUILayoutVert
        /// <summary>
        /// UVField表示のテンプレ
        /// 頂点シェーダー上でサンプルするテクスチャ用 
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="viewGUI"></param>
        /// <param name="property">uv name, "+Transform" "+Transform_Index" "+Transform_Sampler"</param>
        public static void CanvasUVGUILayoutVert(ref Material mat, ref bool viewGUI, string property)
        {
            viewGUI = EditorGUILayout.Foldout(viewGUI, "UV");
            if (viewGUI)
            {
                GUI.color = new Color(0f, 0f, 0f, 0.8f);
                using (new EditorGUILayout.VerticalScope("Box"))
                {
                    GUI.color = new Color(1f, 1f, 1f, 1.0f);

                    var index = OptionPopupField(ref mat, property + "Transform_Index", "UV Channel", _CanvasUVChannelOptionVert);
                    OptionPopupField(ref mat, property + "Transform_Sampler", "Wrap Mode", _WrapMode);
                    CanvasOptionFloatField(ref mat, property + "Transform", "Tile X", 0);
                    CanvasOptionFloatField(ref mat, property + "Transform", "Tile Y", 1);
                    CanvasOptionFloatField(ref mat, property + "Transform", "Offset X", 2);
                    CanvasOptionFloatField(ref mat, property + "Transform", "Offset Y", 3);


                    if (index == 6 && !UVRotate.IsActive(mat))
                    {
                        var rect = EditorGUILayout.GetControlRect();
                        rect.height = 40;
                        rect.xMin += 30;
                        EditorGUI.HelpBox(rect, "Please UV Rotate Active", MessageType.Warning);
                        rect.xMin = rect.xMax - 70;
                        if (GUI.Button(rect, "Fix now"))
                            UVRotate.SetActive(mat, true);
                        EditorGUILayout.GetControlRect();
                    }

                    if (index == 7 && !UVFripBook.IsActive(mat))
                    {
                        var rect = EditorGUILayout.GetControlRect();
                        rect.height = 40;
                        rect.xMin += 30;
                        EditorGUI.HelpBox(rect, "Please UV FripBook Active", MessageType.Warning);
                        rect.xMin = rect.xMax - 70;
                        if (GUI.Button(rect, "Fix now"))
                            UVFripBook.SetActive(mat, true);
                        EditorGUILayout.GetControlRect();
                    }
                }
            }
        }

        /// MARK: UVGUILayoutClamp
        /// <summary>
        /// UVField表示のテンプレ　WrapModeなし
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="viewGUI"></param>
        /// <param name="property">uv name, "+Transform" "+Transform_Index"</param>
        public static void UVGUILayoutClamp(ref Material mat, ref bool viewGUI, string property)
        {
            viewGUI = EditorGUILayout.Foldout(viewGUI, "UV");
            if (viewGUI)
            {
                GUI.color = new Color(0f, 0f, 0f, 0.8f);
                using (new EditorGUILayout.VerticalScope("Box"))
                {
                    GUI.color = new Color(1f, 1f, 1f, 1.0f);

                    var index = OptionPopupField(ref mat, property + "Transform_Index", "UV Channel", _UVChannelOption);
                    OptionFloatField(ref mat, property + "Transform", "Tile X", 0);
                    OptionFloatField(ref mat, property + "Transform", "Tile Y", 1);
                    OptionFloatField(ref mat, property + "Transform", "Offset X", 2);
                    OptionFloatField(ref mat, property + "Transform", "Offset Y", 3);

                    if (index == 6 && !UVRotate.IsActive(mat))
                    {
                        var rect = EditorGUILayout.GetControlRect();
                        rect.height = 40;
                        rect.xMin += 30;
                        EditorGUI.HelpBox(rect, "Please UV Rotate Active", MessageType.Warning);
                        rect.xMin = rect.xMax - 70;
                        if (GUI.Button(rect, "Fix now"))
                            UVRotate.SetActive(mat, true);
                        EditorGUILayout.GetControlRect();
                    }

                    if (index == 7 && !UVFripBook.IsActive(mat))
                    {
                        var rect = EditorGUILayout.GetControlRect();
                        rect.height = 40;
                        rect.xMin += 30;
                        EditorGUI.HelpBox(rect, "Please UV FripBook Active", MessageType.Warning);
                        rect.xMin = rect.xMax - 70;
                        if (GUI.Button(rect, "Fix now"))
                            UVFripBook.SetActive(mat, true);
                        EditorGUILayout.GetControlRect();
                    }

                    if (index == 8 && !UVBend.IsActive(mat))
                    {
                        var rect = EditorGUILayout.GetControlRect();
                        rect.height = 40;
                        rect.xMin += 30;
                        EditorGUI.HelpBox(rect, "Please UV Bend Active", MessageType.Warning);
                        rect.xMin = rect.xMax - 70;
                        if (GUI.Button(rect, "Fix now"))
                            UVBend.SetActive(mat, true);
                        EditorGUILayout.GetControlRect();
                    }

                }
            }
        }

        /// MARK: CanvasUVGUILayoutClamp
        /// <summary>
        /// UVField表示のテンプレ　WrapModeなし
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="viewGUI"></param>
        /// <param name="property">uv name, "+Transform" "+Transform_Index"</param>
        public static void CanvasUVGUILayoutClamp(ref Material mat, ref bool viewGUI, string property)
        {
            viewGUI = EditorGUILayout.Foldout(viewGUI, "UV");
            if (viewGUI)
            {
                GUI.color = new Color(0f, 0f, 0f, 0.8f);
                using (new EditorGUILayout.VerticalScope("Box"))
                {
                    GUI.color = new Color(1f, 1f, 1f, 1.0f);

                    var index = OptionPopupField(ref mat, property + "Transform_Index", "UV Channel", _CanvasUVChannelOption);
                    CanvasOptionFloatField(ref mat, property + "Transform", "Tile X", 0);
                    CanvasOptionFloatField(ref mat, property + "Transform", "Tile Y", 1);
                    CanvasOptionFloatField(ref mat, property + "Transform", "Offset X", 2);
                    CanvasOptionFloatField(ref mat, property + "Transform", "Offset Y", 3);

                    if (index == 6 && !UVRotate.IsActive(mat))
                    {
                        var rect = EditorGUILayout.GetControlRect();
                        rect.height = 40;
                        rect.xMin += 30;
                        EditorGUI.HelpBox(rect, "Please UV Rotate Active", MessageType.Warning);
                        rect.xMin = rect.xMax - 70;
                        if (GUI.Button(rect, "Fix now"))
                            UVRotate.SetActive(mat, true);
                        EditorGUILayout.GetControlRect();
                    }

                    if (index == 7 && !UVFripBook.IsActive(mat))
                    {
                        var rect = EditorGUILayout.GetControlRect();
                        rect.height = 40;
                        rect.xMin += 30;
                        EditorGUI.HelpBox(rect, "Please UV FripBook Active", MessageType.Warning);
                        rect.xMin = rect.xMax - 70;
                        if (GUI.Button(rect, "Fix now"))
                            UVFripBook.SetActive(mat, true);
                        EditorGUILayout.GetControlRect();
                    }

                    if (index == 8 && !UVBend.IsActive(mat))
                    {
                        var rect = EditorGUILayout.GetControlRect();
                        rect.height = 40;
                        rect.xMin += 30;
                        EditorGUI.HelpBox(rect, "Please UV Bend Active", MessageType.Warning);
                        rect.xMin = rect.xMax - 70;
                        if (GUI.Button(rect, "Fix now"))
                            UVBend.SetActive(mat, true);
                        EditorGUILayout.GetControlRect();
                    }

                }
            }
        }

        /// MARK: OptionFloatField
        /// <summary>
        /// Vector４プロパティ
        /// indexで指定した項目のみ編集
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="property">vector4 name, "+_Data"</param>
        /// <param name="label">display name</param>
        /// <param name="index">0~3 vector index</param>
        public static void OptionFloatField(ref Material mat, string property, string label, int index)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var value = mat.GetVector(property);
                var customData = mat.GetVector(property + "_Data");

                EditorGUI.BeginChangeCheck();

                switch (index)
                {
                    case 0:
                        customData.x = Convert.ToInt32(EditorGUILayout.EnumPopup(label, (VertexData)customData.x));
                        if (customData.x == (int)VertexData.Input)
                            value.x = EditorGUILayout.FloatField("", value.x, GUILayout.Width(100));
                        break;

                    case 1:
                        customData.y = Convert.ToInt32(EditorGUILayout.EnumPopup(label, (VertexData)customData.y));
                        if (customData.y == (int)VertexData.Input)
                            value.y = EditorGUILayout.FloatField("", value.y, GUILayout.Width(100));
                        break;

                    case 2:
                        customData.z = Convert.ToInt32(EditorGUILayout.EnumPopup(label, (VertexData)customData.z));
                        if (customData.z == (int)VertexData.Input)
                            value.z = EditorGUILayout.FloatField("", value.z, GUILayout.Width(100));
                        break;

                    case 3:
                        customData.w = Convert.ToInt32(EditorGUILayout.EnumPopup(label, (VertexData)customData.w));
                        if (customData.w == (int)VertexData.Input)
                            value.w = EditorGUILayout.FloatField("", value.w, GUILayout.Width(100));
                        break;

                    default:
                        break;
                }

                if (EditorGUI.EndChangeCheck())
                {
                    mat.SetVector(property, value);
                    mat.SetVector(property + "_Data", customData);
                }

            }
        }

        /// MARK: OptionFloatField
        /// <summary>
        /// Vector４プロパティ
        /// indexで指定した項目のみ編集
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="property">vector4 name, "+_Data"</param>
        /// <param name="label">display name</param>
        /// <param name="index">0~3 vector index</param>
        public static void CanvasOptionFloatField(ref Material mat, string property, string label, int index)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var value = mat.GetVector(property);
                var customData = mat.GetVector(property + "_Data");

                EditorGUI.BeginChangeCheck();

                switch (index)
                {
                    case 0:
                        customData.x = Convert.ToInt32(EditorGUILayout.EnumPopup(label, (CanvasVertexData)customData.x));
                        if (customData.x == (int)CanvasVertexData.Input)
                            value.x = EditorGUILayout.FloatField("", value.x, GUILayout.Width(100));
                        break;

                    case 1:
                        customData.y = Convert.ToInt32(EditorGUILayout.EnumPopup(label, (CanvasVertexData)customData.y));
                        if (customData.y == (int)CanvasVertexData.Input)
                            value.y = EditorGUILayout.FloatField("", value.y, GUILayout.Width(100));
                        break;

                    case 2:
                        customData.z = Convert.ToInt32(EditorGUILayout.EnumPopup(label, (CanvasVertexData)customData.z));
                        if (customData.z == (int)CanvasVertexData.Input)
                            value.z = EditorGUILayout.FloatField("", value.z, GUILayout.Width(100));
                        break;

                    case 3:
                        customData.w = Convert.ToInt32(EditorGUILayout.EnumPopup(label, (CanvasVertexData)customData.w));
                        if (customData.w == (int)CanvasVertexData.Input)
                            value.w = EditorGUILayout.FloatField("", value.w, GUILayout.Width(100));
                        break;

                    default:
                        break;
                }

                if (EditorGUI.EndChangeCheck())
                {
                    mat.SetVector(property, value);
                    mat.SetVector(property + "_Data", customData);
                }

            }
        }

        /// <summary>
        /// floatプロパティの編集
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="property">float name</param>
        /// <param name="label">display name</param>
        /// <returns></returns>
        public static float FloatField(ref Material mat, string property, string label)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var value = mat.GetFloat(property);

                EditorGUI.BeginChangeCheck();

                value = EditorGUILayout.FloatField(label, value);

                if (EditorGUI.EndChangeCheck())
                {
                    mat.SetFloat(property, value);
                }
                return value;
            }
        }

        /// MARK: IntSlider
        /// <summary>
        /// Intプロパティの編集
        /// CustomDataなし
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="property">int name</param>
        /// <param name="label">display name</param>
        /// <param name="min">slider min</param>
        /// <param name="max">slider max</param>
        /// <returns></returns>
        public static int IntSlider(ref Material mat, string property, string label, int min, int max)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var value = mat.GetInt(property);

                EditorGUI.BeginChangeCheck();

                value = EditorGUILayout.IntSlider(label, value, min, max);

                if (EditorGUI.EndChangeCheck())
                {
                    mat.SetInt(property, value);
                }
                return value;
            }
        }


        /// MARK: OptionIntSlider
        /// <summary>
        /// Intプロパティの編集
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="property">vector4 name, "+_Data"</param>
        /// <param name="label">display name</param>
        /// <param name="min">slider min</param>
        /// <param name="max">slider max</param>
        /// <returns></returns>
        public static int OptionIntSlider(ref Material mat, string property, string label, int min, int max)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var value = mat.GetInt(property);
                var customData = mat.GetInt(property + "_Data");

                EditorGUI.BeginChangeCheck();

                customData = Convert.ToInt32(EditorGUILayout.EnumPopup(label, (VertexData)customData));
                if (customData == (int)VertexData.Input)
                    value = EditorGUILayout.IntSlider(value, min, max);

                if (EditorGUI.EndChangeCheck())
                {
                    mat.SetInt(property, value);
                    mat.SetInt(property + "_Data", customData);
                }

                return (int)value;

            }
        }

        /// MARK: CanvasOptionIntSlider
        /// <summary>
        /// Intプロパティの編集
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="property">vector4 name, "+_Data"</param>
        /// <param name="label">display name</param>
        /// <param name="min">slider min</param>
        /// <param name="max">slider max</param>
        /// <returns></returns>
        public static int CanvasOptionIntSlider(ref Material mat, string property, string label, int min, int max)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var value = mat.GetInt(property);
                var customData = mat.GetInt(property + "_Data");

                EditorGUI.BeginChangeCheck();

                customData = Convert.ToInt32(EditorGUILayout.EnumPopup(label, (CanvasVertexData)customData));
                if (customData == (int)CanvasVertexData.Input)
                    value = EditorGUILayout.IntSlider(value, min, max);

                if (EditorGUI.EndChangeCheck())
                {
                    mat.SetInt(property, value);
                    mat.SetInt(property + "_Data", customData);
                }

                return (int)value;

            }
        }

        /// MARK: OptionIntSlider
        /// <summary>
        /// Intプロパティの編集
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="property">vector4 name, "+_Data"</param>
        /// <param name="label">display name</param>
        /// <param name="index">0~3 vector index</param>
        /// <param name="min">slider min</param>
        /// <param name="max">slider max</param>
        /// <returns></returns>
        public static int OptionIntSlider(ref Material mat, string property, string label, int index, int min, int max)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var value = mat.GetVector(property);
                var customData = mat.GetVector(property + "_Data");

                EditorGUI.BeginChangeCheck();

                switch (index)
                {
                    case 0:
                        customData.x = Convert.ToInt32(EditorGUILayout.EnumPopup(label, (VertexData)customData.x));
                        if (customData.x == (int)VertexData.Input)
                            value.x = EditorGUILayout.IntSlider((int)value.x, min, max);
                        break;

                    case 1:
                        customData.y = Convert.ToInt32(EditorGUILayout.EnumPopup(label, (VertexData)customData.y));
                        if (customData.y == (int)VertexData.Input)
                            value.y = EditorGUILayout.IntSlider((int)value.y, min, max);
                        break;

                    case 2:
                        customData.z = Convert.ToInt32(EditorGUILayout.EnumPopup(label, (VertexData)customData.z));
                        if (customData.z == (int)VertexData.Input)
                            value.z = EditorGUILayout.IntSlider((int)value.z, min, max);
                        break;

                    case 3:
                        customData.w = Convert.ToInt32(EditorGUILayout.EnumPopup(label, (VertexData)customData.w));
                        if (customData.w == (int)VertexData.Input)
                            value.w = EditorGUILayout.IntSlider((int)value.w, min, max);
                        break;

                    default:
                        break;
                }


                if (EditorGUI.EndChangeCheck())
                {
                    mat.SetVector(property, value);
                    mat.SetVector(property + "_Data", customData);
                }

                switch (index)
                {
                    case 0:
                        return (int)value.x;
                    case 1:
                        return (int)value.y;
                    case 2:
                        return (int)value.z;
                    case 3:
                        return (int)value.w;
                    default:
                        return 0;
                }
            }
        }

        /// MARK: CanvasOptionIntSlider
        /// <summary>
        /// Intプロパティの編集
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="property">vector4 name, "+_Data"</param>
        /// <param name="label">display name</param>
        /// <param name="index">0~3 vector index</param>
        /// <param name="min">slider min</param>
        /// <param name="max">slider max</param>
        /// <returns></returns>
        public static int CanvasOptionIntSlider(ref Material mat, string property, string label, int index, int min, int max)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var value = mat.GetVector(property);
                var customData = mat.GetVector(property + "_Data");

                EditorGUI.BeginChangeCheck();

                switch (index)
                {
                    case 0:
                        customData.x = Convert.ToInt32(EditorGUILayout.EnumPopup(label, (CanvasVertexData)customData.x));
                        if (customData.x == (int)CanvasVertexData.Input)
                            value.x = EditorGUILayout.IntSlider((int)value.x, min, max);
                        break;

                    case 1:
                        customData.y = Convert.ToInt32(EditorGUILayout.EnumPopup(label, (CanvasVertexData)customData.y));
                        if (customData.y == (int)CanvasVertexData.Input)
                            value.y = EditorGUILayout.IntSlider((int)value.y, min, max);
                        break;

                    case 2:
                        customData.z = Convert.ToInt32(EditorGUILayout.EnumPopup(label, (CanvasVertexData)customData.z));
                        if (customData.z == (int)CanvasVertexData.Input)
                            value.z = EditorGUILayout.IntSlider((int)value.z, min, max);
                        break;

                    case 3:
                        customData.w = Convert.ToInt32(EditorGUILayout.EnumPopup(label, (CanvasVertexData)customData.w));
                        if (customData.w == (int)CanvasVertexData.Input)
                            value.w = EditorGUILayout.IntSlider((int)value.w, min, max);
                        break;

                    default:
                        break;
                }


                if (EditorGUI.EndChangeCheck())
                {
                    mat.SetVector(property, value);
                    mat.SetVector(property + "_Data", customData);
                }

                switch (index)
                {
                    case 0:
                        return (int)value.x;
                    case 1:
                        return (int)value.y;
                    case 2:
                        return (int)value.z;
                    case 3:
                        return (int)value.w;
                    default:
                        return 0;
                }
            }
        }

        /// MARK: Slider
        /// <summary>
        /// floatプロパティの編集
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="property">float name</param>
        /// <param name="label">display name</param>
        /// <param name="min">slider min</param>
        /// <param name="max">slider max</param>
        public static void Slider(ref Material mat, string property, string label, float min, float max)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var value = mat.GetFloat(property);

                EditorGUI.BeginChangeCheck();

                value = EditorGUILayout.Slider(label, value, min, max);

                if (EditorGUI.EndChangeCheck())
                {
                    mat.SetFloat(property, value);
                }
            }
        }

        /// MARK: OptionSlider
        /// <summary>
        /// floatプロパティの編集
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="property">float name, "+_Data"</param>
        /// <param name="label">display name</param>
        /// <param name="min">slider min</param>
        /// <param name="max">slider max</param>
        public static void OptionSlider(ref Material mat, string property, string label, float min, float max)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var value = mat.GetFloat(property);
                var customData = mat.GetInt(property + "_Data");

                EditorGUI.BeginChangeCheck();

                customData = Convert.ToInt32(EditorGUILayout.EnumPopup(label, (VertexData)customData));
                if (customData == (int)VertexData.Input)
                    value = EditorGUILayout.Slider(value, min, max);

                if (EditorGUI.EndChangeCheck())
                {
                    mat.SetFloat(property, value);
                    mat.SetInt(property + "_Data", customData);
                }
            }
        }

        /// MARK: CanvasOptionSlider
        /// <summary>
        /// floatプロパティの編集
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="property">float name, "+_Data"</param>
        /// <param name="label">display name</param>
        /// <param name="min">slider min</param>
        /// <param name="max">slider max</param>
        public static void CanvasOptionSlider(ref Material mat, string property, string label, float min, float max)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var value = mat.GetFloat(property);
                var customData = mat.GetInt(property + "_Data");

                EditorGUI.BeginChangeCheck();

                customData = Convert.ToInt32(EditorGUILayout.EnumPopup(label, (CanvasVertexData)customData));
                if (customData == (int)CanvasVertexData.Input)
                    value = EditorGUILayout.Slider(value, min, max);

                if (EditorGUI.EndChangeCheck())
                {
                    mat.SetFloat(property, value);
                    mat.SetInt(property + "_Data", customData);
                }
            }
        }

        /// MARK: OptionSlider
        /// <summary>
        /// Vector４プロパティ
        /// indexで指定した項目のみ編集
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="property">vector4 name, "+_Data"</param>
        /// <param name="label">display name</param>
        /// <param name="index">0~3 vector index</param>
        /// <param name="min">slider min</param>
        /// <param name="max">slider max</param>
        public static void OptionSlider(ref Material mat, string property, string label, int index, float min, float max)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var value = mat.GetVector(property);
                var customData = mat.GetVector(property + "_Data");

                EditorGUI.BeginChangeCheck();

                switch (index)
                {
                    case 0:
                        customData.x = Convert.ToInt32(EditorGUILayout.EnumPopup(label, (VertexData)customData.x));
                        if (customData.x == (int)VertexData.Input)
                            value.x = EditorGUILayout.Slider(value.x, min, max);
                        break;

                    case 1:
                        customData.y = Convert.ToInt32(EditorGUILayout.EnumPopup(label, (VertexData)customData.y));
                        if (customData.y == (int)VertexData.Input)
                            value.y = EditorGUILayout.Slider(value.y, min, max);
                        break;

                    case 2:
                        customData.z = Convert.ToInt32(EditorGUILayout.EnumPopup(label, (VertexData)customData.z));
                        if (customData.z == (int)VertexData.Input)
                            value.z = EditorGUILayout.Slider(value.z, min, max);
                        break;

                    case 3:
                        customData.w = Convert.ToInt32(EditorGUILayout.EnumPopup(label, (VertexData)customData.w));
                        if (customData.w == (int)VertexData.Input)
                            value.w = EditorGUILayout.Slider(value.w, min, max);
                        break;

                    default:
                        break;
                }


                if (EditorGUI.EndChangeCheck())
                {
                    mat.SetVector(property, value);
                    mat.SetVector(property + "_Data", customData);
                }
            }
        }

        /// MARK: CanvasOptionSlider
        /// <summary>
        /// Vector４プロパティ
        /// indexで指定した項目のみ編集
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="property">vector4 name, "+_Data"</param>
        /// <param name="label">display name</param>
        /// <param name="index">0~3 vector index</param>
        /// <param name="min">slider min</param>
        /// <param name="max">slider max</param>
        public static void CanvasOptionSlider(ref Material mat, string property, string label, int index, float min, float max)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var value = mat.GetVector(property);
                var customData = mat.GetVector(property + "_Data");

                EditorGUI.BeginChangeCheck();

                switch (index)
                {
                    case 0:
                        customData.x = Convert.ToInt32(EditorGUILayout.EnumPopup(label, (CanvasVertexData)customData.x));
                        if (customData.x == (int)CanvasVertexData.Input)
                            value.x = EditorGUILayout.Slider(value.x, min, max);
                        break;

                    case 1:
                        customData.y = Convert.ToInt32(EditorGUILayout.EnumPopup(label, (CanvasVertexData)customData.y));
                        if (customData.y == (int)CanvasVertexData.Input)
                            value.y = EditorGUILayout.Slider(value.y, min, max);
                        break;

                    case 2:
                        customData.z = Convert.ToInt32(EditorGUILayout.EnumPopup(label, (CanvasVertexData)customData.z));
                        if (customData.z == (int)CanvasVertexData.Input)
                            value.z = EditorGUILayout.Slider(value.z, min, max);
                        break;

                    case 3:
                        customData.w = Convert.ToInt32(EditorGUILayout.EnumPopup(label, (CanvasVertexData)customData.w));
                        if (customData.w == (int)CanvasVertexData.Input)
                            value.w = EditorGUILayout.Slider(value.w, min, max);
                        break;

                    default:
                        break;
                }


                if (EditorGUI.EndChangeCheck())
                {
                    mat.SetVector(property, value);
                    mat.SetVector(property + "_Data", customData);
                }
            }
        }

        /// MARK: OptionColorField
        /// <summary>
        /// Colorプロパティの編集
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="property">color name, "+_Data"</param>
        /// <param name="label">display name</param>
        public static void OptionColorField(ref Material mat, string property, string label)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var color = mat.GetColor(property);
                var customData = mat.GetInt(property + "_Data");

                EditorGUI.BeginChangeCheck();

                customData = Convert.ToInt32(EditorGUILayout.EnumPopup(label, (VertexColorData)customData));
                if (customData == (int)VertexColorData.Input)
                    color = EditorGUILayout.ColorField(new GUIContent(""), color, true, true, true);

                if (EditorGUI.EndChangeCheck())
                {
                    mat.SetColor(property, color);
                    mat.SetInt(property + "_Data", customData);
                }
            }
        }

        /// MARK: CanvasOptionColorField
        /// <summary>
        /// Colorプロパティの編集
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="property">color name, "+_Data"</param>
        /// <param name="label">display name</param>
        public static void CanvasOptionColorField(ref Material mat, string property, string label)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var color = mat.GetColor(property);
                var customData = mat.GetInt(property + "_Data");

                EditorGUI.BeginChangeCheck();

                customData = Convert.ToInt32(EditorGUILayout.EnumPopup(label, (CanvasVertexColorData)customData));
                if (customData == (int)CanvasVertexColorData.Input)
                    color = EditorGUILayout.ColorField(new GUIContent(""), color, true, true, true);

                if (EditorGUI.EndChangeCheck())
                {
                    mat.SetColor(property, color);
                    mat.SetInt(property + "_Data", customData);
                }
            }
        }

        /// MARK: OptionTextureField
        /// <summary>
        /// Textureプロパティの編集
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="property">texture name</param>
        /// <param name="label">display name</param>
        public static void OptionTextureField(ref Material mat, string property, string label)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var tex = mat.GetTexture(property);

                EditorGUI.BeginChangeCheck();

                tex = EditorGUILayout.ObjectField(label, tex, typeof(Texture), false) as Texture;

                if (EditorGUI.EndChangeCheck())
                    mat.SetTexture(property, tex);
            }
        }

        /// MARK: OptionBoolField
        /// <summary>
        /// Boolプロパティの編集
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="property">bool name</param>
        /// <param name="label">display name</param>
        /// <returns></returns>
        public static bool OptionBoolField(ref Material mat, string property, string label)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var value = mat.GetInt(property) == 1;

                EditorGUI.BeginChangeCheck();

                value = EditorGUILayout.Toggle(label, value);

                if (EditorGUI.EndChangeCheck())
                    mat.SetInt(property, value ? 1 : 0);

                return value;
            }
        }

        /// MARK: OptionKeywordField
        /// <summary>
        /// KeyWordの編集
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="property">Keyword name</param>
        /// <param name="label">display name</param>
        /// <returns>result bool</returns>
        public static bool OptionKeywordField(ref Material mat, string property, string label)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var value = mat.IsKeywordEnabled(property);

                EditorGUI.BeginChangeCheck();

                value = EditorGUILayout.Toggle(label, value);

                if (EditorGUI.EndChangeCheck())
                {
                    if (value)
                    {
                        mat.EnableKeyword(property);
                    }
                    else
                    {
                        mat.DisableKeyword(property);
                    }
                }

                return value;
            }
        }

        /// MARK: OptionPopupField
        /// <summary>
        /// intプロパティの編集
        /// optionでPopup項目を指定
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="property">int name</param>
        /// <param name="label">display name</param>
        /// <param name="option">Pupup Options string[]</param>
        /// <returns>result index</returns>
        public static int OptionPopupField(ref Material mat, string property, string label, string[] option)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var value = mat.GetInt(property);

                EditorGUI.BeginChangeCheck();

                value = EditorGUILayout.Popup(label, value, option);

                if (EditorGUI.EndChangeCheck())
                    mat.SetInt(property, value);

                return value;
            }
        }

        /// MARK: VaridateVertexDataVector
        /// <summary>
        /// プロパティがEnumの範囲内かチェック
        /// 範囲外の場合は０に変更
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static Vector4 VaridateCustomDataVector(ref Material mat, string property)
        {
            var value = mat.GetVector(property + "_Data");
            var length = Enum.GetValues(typeof(VertexData)).Length;
            if (value.x >= length)
            {
                value.x = 0;
                Debug.Log("「" + property + ".x」CustomData Reset");
            }
            if (value.y >= length)
            {
                value.y = 0;
                Debug.Log("「" + property + ".y」CustomData Reset");
            }
            if (value.z >= length)
            {
                value.z = 0;
                Debug.Log("「" + property + ".z」CustomData Reset");
            }
            if (value.w >= length)
            {
                value.w = 0;
                Debug.Log("「" + property + ".w」CustomData Reset");
            }
            mat.SetVector(property + "_Data", value);
            return value;
        }

        /// MARK: VaridateVertexDataInt
        /// <summary>
        /// プロパティがEnumの範囲内かチェック
        /// 範囲外の場合は０に変更
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static int VaridateCustomDataInt(ref Material mat, string property)
        {
            var value = mat.GetInt(property + "_Data");
            var length = Enum.GetValues(typeof(VertexData)).Length;
            if (value >= length)
            {
                value = 0;
                Debug.Log("「" + property + "」CustomData Reset");
            }
            mat.SetInt(property + "_Data", value);
            return value;
        }

        /// MARK: VaridateVertexColorDataInt
        /// <summary>
        /// プロパティがEnumの範囲内かチェック
        /// 範囲外の場合は０に変更
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static int VaridateCustomColorDataInt(ref Material mat, string property)
        {
            var value = mat.GetInt(property + "_Data");
            var length = Enum.GetValues(typeof(VertexColorData)).Length;
            if (value >= length)
            {
                value = 0;
                Debug.Log("「" + property + "」CustomData Reset");
            }
            mat.SetInt(property + "_Data", value);
            return value;
        }

        /// MARK: VaridateCanvasVertexDataVector
        /// <summary>
        /// プロパティがEnumの範囲内かチェック
        /// 範囲外の場合は０に変更
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static Vector4 VaridateCanvasCustomDataVector(ref Material mat, string property)
        {
            var value = mat.GetVector(property + "_Data");
            var length = Enum.GetValues(typeof(CanvasVertexData)).Length;
            if (value.x >= length)
            {
                value.x = 0;
                Debug.Log("「" + property + ".x」CustomData Reset");
            }
            if (value.y >= length)
            {
                value.y = 0;
                Debug.Log("「" + property + ".y」CustomData Reset");
            }
            if (value.z >= length)
            {
                value.z = 0;
                Debug.Log("「" + property + ".z」CustomData Reset");
            }
            if (value.w >= length)
            {
                value.w = 0;
                Debug.Log("「" + property + ".w」CustomData Reset");
            }
            mat.SetVector(property + "_Data", value);
            return value;
        }


        /// MARK: VaridateCanvasVertexDataInt
        /// <summary>
        /// プロパティがEnumの範囲内かチェック
        /// 範囲外の場合は０に変更
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static int VaridateCanvasCustomDataInt(ref Material mat, string property)
        {
            var value = mat.GetInt(property + "_Data");
            var length = Enum.GetValues(typeof(CanvasVertexData)).Length;
            if (value >= length)
            {
                value = 0;
                Debug.Log("「" + property + "」CustomData Reset");
            }
            mat.SetInt(property + "_Data", value);
            return value;
        }

        /// MARK: VaridateCanvasVertexColorDataInt
        /// <summary>
        /// プロパティがEnumの範囲内かチェック
        /// 範囲外の場合は０に変更
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static int VaridateCanvasCustomColorDataInt(ref Material mat, string property)
        {
            var value = mat.GetInt(property + "_Data");
            var length = Enum.GetValues(typeof(CanvasVertexColorData)).Length;
            if (value >= length)
            {
                value = 0;
                Debug.Log("「" + property + "」CustomData Reset");
            }
            mat.SetInt(property + "_Data", value);
            return value;
        }

        /// MARK: VaridateCanvasVertexColorDataInt
        /// <summary>
        /// プロパティがoption配列の範囲内かチェック
        /// 範囲外の場合は０に変更
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static int VaridateArrayIndex(ref Material mat, string property, string[] option)
        {
            var value = mat.GetInt(property);
            var length = option.Length;
            if (value >= length)
            {
                value = 0;
                Debug.Log("「" + property + "」ArrayIndex Reset");
            }
            mat.SetInt(property, value);
            return value;
        }

    }
}
