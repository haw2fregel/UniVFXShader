# ChangeLog

## v1.1.1
- 専用シェーダー変換の不具合修正と使い勝手改善（Canvas の UV/RectMask2D・Cutout の RenderType・ロケール非依存の数値出力・出力先フォルダ指定と自動記憶・UPM 導入時の出力先フォールバックと衝突回避）
- シェーダー最適化・修正（SurfaceFade の BRP saturate 修正・Fresnel/UVRotate 最適化・UniVFXCanvas/FakeLight の整理）
- Runtime/Editor 安全化（`UniVFXSetupCanvasUV` 刷新、他マテリアル参照チェック付きの専用シェーダー安全削除、Inspector の変更時のみ `SetDirty` 化）

## v1.1.0
- [最適化機能の実装](https://github.com/haw2fregel/UniVFXShader/blob/v1.1.0/DOCUMENTATION.md#%E6%9C%80%E9%81%A9%E5%8C%96)

## v1.0.3
- TransparentShaderでBlendAlphaを変更できるようにする

## v1.0.2
- 負荷軽減
- TransparentShaderのDepthWriteを強制的にOffにする

## v1.0.1
#### Bug Fix
CanvasShaderの修正
- CanvasOverlayでも正常に表示されるように修正
- CanvasUV1~3にCustomUVを設定するコンポーネントを追加 (UniVFXSetupCanvasUV)
- hader切り替え時にエラーが発生する問題を修正

UVBendの修正
- PolarOnの時に一部テクスチャが途切れる不具合の修正


## v1.0.0
#### Releace
