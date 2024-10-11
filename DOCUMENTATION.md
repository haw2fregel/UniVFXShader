# Inspector

![image](https://github.com/user-attachments/assets/1155362e-4d52-4310-b4d3-9278e5f2cdfd)




## Main Texture
<details>
<summary>--- メインテクスチャ</summary>

<br>
  
| ![image](https://github.com/user-attachments/assets/e7d4f7a6-d515-412f-8899-b83b527c9119)|![image](https://github.com/user-attachments/assets/7bc6a52d-dfb4-41be-9717-de426ca08efd)|
| ------------- | ------------- |
| Texture  | メインテクスチャ  |
| Color  | メインテクスチャに乗算する色  |
| Color　Multiply  | チェックを外すとColorのRGBを無視する  |
| Alpha　Multiply  | チェックを外すとColorのAlphaを無視する  |
| UV |[UV](https://github.com/haw2fregel/UniVFXShader/blob/v1.0.0/DOCUMENTATION.md#uv) |

</details>

## Mask Texture
<details>
<summary>--- マスクテクスチャ</summary>

<br>
  
|![image](https://github.com/user-attachments/assets/d967194a-be6b-4094-b1b9-9482d2c1e0e2)|![image](https://github.com/user-attachments/assets/1911cea0-dcab-4414-856d-7e393c648032)|
| ------------- | ------------- |
| Texture  | テクスチャ <br>Rチャンネルを参照 |
| Value Offset  | テクスチャの値をずらす |
| Repeat  | Offsetで0~1からはみ出た場合に繰り返す  |
| UV |[UV](https://github.com/haw2fregel/UniVFXShader/blob/v1.0.0/DOCUMENTATION.md#uv) |
| Target |チェックを付けた機能に適用します。<br><br>![image](https://github.com/user-attachments/assets/1b2c1591-d497-4c62-a84b-b0f9283eeada)|

</details>

## Blend Texture
<details>
<summary>--- ブレンドテクスチャ</summary>

<br>

|![image](https://github.com/user-attachments/assets/caa7a712-4cb8-42ae-beb3-b9e132d3290c)|![image](https://github.com/user-attachments/assets/46e69324-c18d-4f54-949f-8f1b0de38842)|
| ------------- | ------------- |
| Texture  | テクスチャ  |
| Color  | テクスチャに乗算 |
| Intensity | 合成強度 |
| BlendMode | 合成方法<br><br> ![image](https://github.com/user-attachments/assets/98fe1483-c1d5-410b-90f1-84c85b233fed)|
| UV |[UV](https://github.com/haw2fregel/UniVFXShader/blob/v1.0.0/DOCUMENTATION.md#uv) |

</details>

## Gradation Color
<details>
<summary>--- グラデーション</summary>

<br>

|![image](https://github.com/user-attachments/assets/5c9a6b7a-1d86-4ed9-98bb-c0ef463fa7d1)|![image](https://github.com/user-attachments/assets/1585846b-ba5e-4d35-9c9a-636d25840d97)|
| ------------- | ------------- |
| Color00  | 左下|
| Color01  | 左上 |
| Color10 | 右下 |
| Color11 | 右上|
| BlendMode | 合成方法<br><br>![image](https://github.com/user-attachments/assets/b93c0db0-02a8-4ca6-935f-11d57a67feae)|
| UV |[UV](https://github.com/haw2fregel/UniVFXShader/blob/v1.0.0/DOCUMENTATION.md#uv)|

</details>


## Dissolve
<details>
<summary>--- ディゾルブ</summary>

<br>

|![image](https://github.com/user-attachments/assets/9611a530-6577-4c95-a3ea-ad18f528b704)|![image](https://github.com/user-attachments/assets/f67dc342-ed22-40cb-9c4a-07d16e182edb)|
| ------------- | ------------- |
| Color  | エミッシブカラー|
| Alpha  | ディゾルブアルファ<br>0で完全に消える |
| Smooth | 境界ぼかし |
| Emissive Width | エミッシブの大きさ|
| Emissive Smooth | エミッシブのぼかし|
| UV |[UV](https://github.com/haw2fregel/UniVFXShader/blob/v1.0.0/DOCUMENTATION.md#uv) |

</details>

## UV Distortion
<details>
<summary>--- ディストーション</summary>

<br>

|![image](https://github.com/user-attachments/assets/147b515a-47d1-4cb3-b233-b8000c628aee)|![image](https://github.com/user-attachments/assets/93223c9e-9946-42ce-8218-f3c542205d08)|
| ------------- | ------------- |
| Texture  | テクスチャ|
| Intensity  | 強度 |
| UV |[UV](https://github.com/haw2fregel/UniVFXShader/blob/v1.0.0/DOCUMENTATION.md#uv)|
| Target |チェックを付けた機能に適用します。<br><br>![image](https://github.com/user-attachments/assets/88b759b5-b020-4bf0-9630-762d3dc71f78)|

</details>


## UV Bend
<details>
<summary>--- ベンド</summary>

<br>

|![image](https://github.com/user-attachments/assets/e3384e7b-b10e-4833-b5e3-dcadf2a0eece)|![image](https://github.com/user-attachments/assets/d7bdf549-43bb-433b-9982-a8ffb48b0c00)|
| ------------- | ------------- |
| Polar  | 極座標|
| Center　X  | Bendの中央座標 |
| Center　Y |Bendの中央座標|
| Bend　X |Bendの強度|
| Bend　Y |Bendの強度|

</details>

## UV Parallax
<details>
<summary>--- パララックス</summary>

<br>

|![image](https://github.com/user-attachments/assets/5c69dd49-544e-4809-adfe-0fc7e858327a)|![image](https://github.com/user-attachments/assets/bab671ad-d6e1-488f-9ee9-20bce2dfb037)|
| ------------- | ------------- |
| Texture  | テクスチャ|
| Intensity  | 強度 |
| Target |チェックを付けた機能に適用します。<br><br>![image](https://github.com/user-attachments/assets/cfe6fb71-4f34-4158-aba3-4e611dbfae51)|

</details>


## UV Rotate
<details>
<summary>--- ローテーション</summary>

<br>

|![image](https://github.com/user-attachments/assets/0a1315d5-c35c-4f20-9add-b95ae173c2dc)|![image](https://github.com/user-attachments/assets/88237ff8-20cd-443e-acd8-dc4179cd83e3)|
| ------------- | ------------- |
| Rotate  | 角度 |

</details>

## UV FripBook
<details>
<summary>--- フリップブック</summary>

<br>

|![image](https://github.com/user-attachments/assets/4e8c995a-cc78-44c1-9afc-a6f086488105)|![image](https://github.com/user-attachments/assets/4ed456d4-99f3-43d8-a756-54f70f0e863d)|
| ------------- | ------------- |
| Row  | 横の分割数 |
| Column  | 縦の分割数 |
| Index  | 表示位置 |

</details>

## HSV Shift
<details>
<summary>--- HSVシフト</summary>

<br>

|![image](https://github.com/user-attachments/assets/30ab9678-3576-4e05-8120-f7b917710191)|![image](https://github.com/user-attachments/assets/5d995594-01b9-496b-8a8e-0722da233ea3)|
| ------------- | ------------- |
| Hue  | 色相 |
| Sat  | 彩度 |
| Val  | 明度 |

</details>

## Surface Fade
<details>
<summary>--- サーフェイスフェード</summary>

<br>

|![image](https://github.com/user-attachments/assets/54959aa8-5a76-4169-b019-00dab9ef9269)|![image](https://github.com/user-attachments/assets/de5a3673-10cd-44f0-ae9e-f53f1b9c7864)|
| ------------- | ------------- |
| Frenel  | フレネル効果のOnOff |
| Power  | フレネルのグラデ調整 |
| Reverce  | フレネルの反転 |
| Position Fade Type  | 座標を利用したフェード機能<br>SoftParticleを使用する場合はカメラのDepthTextureが必要になります。<br><br>![image](https://github.com/user-attachments/assets/d0d446dc-ec87-4ba5-9f56-9ec79f230c1b)|
| Fade In  | フェードイン閾値 |
| Fade Out  | フェードアウト閾値 |
| Target |チェックを付けた機能に適用します。<br><br>![image](https://github.com/user-attachments/assets/01bf3664-91d2-40de-aa51-485775f8134f)|

</details>


## Fake Light
<details>
<summary>--- フェイクライト</summary>

<br>

|![image](https://github.com/user-attachments/assets/c57e4f7a-5440-447c-834b-e5318b679a77)|![image](https://github.com/user-attachments/assets/06e48891-0b2f-4e77-b7ce-8ca8c306a436)|
| ------------- | ------------- |
| Texture  | 陰影のグラデーションを制御 |
| Intensity  | 強度 |
| Light Color  | ライト領域に加算 |
| Shadow Color  |影領域に乗算|
| Light Type  | 疑似ライトの種類を選択<br><br> ![image](https://github.com/user-attachments/assets/92681309-37d9-4677-82e8-9b005f7ceb48)|
| X  | Direction:向き<br>Point:座標 |
| Y |Direction:向き<br>Point:座標 |
| Z |Direction:向き<br>Point:座標 |

</details>


## Vertex Animation
<details>
<summary>--- 頂点アニメーション</summary>

<br>

|![image](https://github.com/user-attachments/assets/c52d93ff-c425-4007-b674-b75f6557fefc)|![image](https://github.com/user-attachments/assets/52283e25-4d56-40d2-8d70-109c269d0260)|
| ------------- | ------------- |
| Texture  | テクスチャ |
| X  | 変形方向 |
| Y  | 変形方向 |
| Z  | 変形方向<br>法線方向 |
| Intensity  | 強度|
| UV |[UV](https://github.com/haw2fregel/UniVFXShader/blob/v1.0.0/DOCUMENTATION.md#uv) |

</details>


## Face Color
<details>
<summary>--- フェイスカラー</summary>

<br>

|![image](https://github.com/user-attachments/assets/54c95e53-c2c0-479c-9b70-1e454135eb84)|![image](https://github.com/user-attachments/assets/8decb11e-2bcf-4187-ad2b-e5c08ff318cb)|
| ------------- | ------------- |
| FrontFace Color  | 表面に乗算する色 |
| BackFace Color  | 裏面に乗算する色 |


</details>

## Time
<details>
<summary>--- タイム</summary>

<br>

|![image](https://github.com/user-attachments/assets/16fd02cf-99e0-4c67-8425-c4a770acb6cf)||
| ------------- | ------------- |
| Speed  | CustomDataで使用するTime関連の速度 |
| Texture  | CustomDataでTimeMap使用時に参照。<br>Timeの数値でサンプリングした数値を返す。 |


</details>

## Surface Option
<details>
<summary>--- サーフェイスオプション</summary>

<br>

|![image](https://github.com/user-attachments/assets/cb9891de-708b-40d3-9b04-bf6d6c53c773)||
| ------------- | ------------- |
| Color Multiple Alpha  | Colorに対してAlphaを乗算する |
| Src Blend  | BlendModeの変更。<br>![image](https://github.com/user-attachments/assets/6427f1ba-3973-4024-80ed-557524225890)|
| Dst Blend  | BlendModeの変更。<br>![image](https://github.com/user-attachments/assets/64076a22-c488-404f-b99f-7f3ec1546f0c)|
| ZTest  | ZTestの変更。<br>![image](https://github.com/user-attachments/assets/0e5b617a-ea3b-4262-b409-5e15344469d0)|
| Cull  | CullModeの変更。<br>![image](https://github.com/user-attachments/assets/96ed8510-de80-4966-a8b7-89c48b86932c)|


</details>



## UV
<details>
<summary>--- ユーブイ</summary>

<br>

| ![image](https://github.com/user-attachments/assets/d3b88cc7-6d3c-41ba-b37b-c2167c7b6e74)  ||
| ------------- | ------------- |
| UV Channel  | UVの種類　  |
| Wrap Mode  | リピート処理の変更  |
| Tile X  | タイリング  |
| Tile Y  | タイリング  |
| Offset X  | オフセット  |
| Offset Y  | オフセット  |



<details>
<summary>UV Channel</summary>

<br>

| ![image](https://github.com/user-attachments/assets/7a501e1a-1853-4593-9012-de62099f1287)||
| ------------- | ------------- |
| TEXCOORD  | 通常のUV  |
| Position Z Face  | X,Y座標を利用  |
| Position Y Face  | X,Z座標を利用  |
| Position X Face  | Z,Y座標を利用  |
| Screen　Position  |画面上の座標を利用。  |
| View Normal  | カメラから見た法線を利用。<br> MatCapのような処理  |
| Rotate UV  | UV Rotate のActiveが必要  |
| FripBook UV  | UV FripBook のActiveが必要  |
| Bend UV  | UV Bend のActiveが必要  |
|![image](https://github.com/user-attachments/assets/9423257a-c529-44b3-b662-bb7132e3270f)| 非Activeな機能を指定している場合、警告が表示されます。<br>「Fix now」を押すと対象の機能をActiveに変更できます。|

</details>
</details>

## Custom Data
<details>
<summary>--- カスタムデータ</summary>

<br>

|![image](https://github.com/user-attachments/assets/17b55d44-26c8-4ad7-a42e-84055277acd6)![image](https://github.com/user-attachments/assets/06e24198-84f8-483f-b65b-17853126bbd3) | プルダウンが表示されているパラメーターはCustomDataの利用が可能です。<br> CustomDataを選択した場合は、マテリアル上でのパラメーターは無視されます。 |
| ------------- | ------------- | 

</details>

## Heat Gage
<details>
<summary>--- ヒートゲージ</summary>

<br>

|![image](https://github.com/user-attachments/assets/ec183ded-dd4d-4f46-9466-02d9b6c314bc)![image](https://github.com/user-attachments/assets/5bd8eb74-9161-46e7-a74a-5b967e6aa4e7)| Activeにしている数が多いほど描画負荷が高くなります。<br>ゲージが赤いほど負荷が高いです。 |
| ------------- | ------------- | 

</details>

<br><br>

https://github.com/user-attachments/assets/2e154bc4-968c-4121-96cd-8a3b8e414963
