using UnityEngine;
using System.Collections.Generic;

namespace UniVFX.Editor
{
    public abstract class UniVFXOption
    {
        protected bool _viewGUI = false;
        protected bool _viewUVGUI = false;
        protected bool _viewTargetGUI = false;
        protected Material _mat;

        /// <summary>
        /// 対象マテリアルをセット
        /// </summary>
        /// <param name="mat"></param>
        public void SetMaterial(Material mat) { _mat = mat; }

        /// <summary>
        /// maxにはHeatValue()をそのまま加算
        /// valueにActive状況を判別した値を加算
        /// </summary>
        /// <param name="value"></param>
        /// <param name="max"></param>
        public abstract void GetHeatValue(ref int value, ref int max);

        /// <summary>
        /// Activeを取得
        /// </summary>
        /// <returns></returns>
        public abstract bool IsActive();

        /// <summary>
        /// Active制御する用
        /// Boolプロパティかキーワードか 
        /// </summary>
        /// <param name="active"></param>
        public abstract void SetActive(bool active);

        /// <summary>
        /// オプションをActiveにした時の描画負荷を取得
        /// 値はShaderコンパイル時の命令数を参考に決める
        /// </summary>
        /// <returns></returns>
        public abstract int HeatValue();

        /// <summary>
        /// Inspectorに表示 
        /// </summary>
        public abstract void OptionGUI();

        /// <summary>
        /// 使用しているCustomFloatDataをリストに追加する
        /// </summary>
        /// <param name="useCustomDataList"></param>
        public abstract void CollectCustomData(ref List<List<string>> useCustomDataList);

        /// <summary>
        /// 使用しているCustomColorDataをリストに追加する
        /// </summary>
        /// <param name="useCustomDataList"></param>
        public abstract void CollectCustomColorData(ref List<List<string>> useCustomDataList);

    }
}