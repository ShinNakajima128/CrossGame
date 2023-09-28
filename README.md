## 概要
制作にあたってのお題「交わる」をテーマとして、約一ヵ月の期間で制作した作品です。

## タイトル
「JOYRIDE」
…「スリルを味わうために無謀な運転をすること」「車を乱暴に乗り回す」などの意味を持ったスラングとして使われているこの言葉がイメージに合ったため、これをタイトルにしました。

以下のURLからプレイ可能となっています。

https://shinnakajima128.github.io/CrossGame/

## テーマ「交わる」を表現した点
・「交わる」をイメージしながらアセットを探していた際に、流れてきた町のアセットを見て「交差点」が思い浮かび、直線の道路を走りながら障害となる交差点を突破する「ランゲーム」を企画しました。

・もう一つのテーマの要素として、プレイヤーの車に乗車する「ヒッチハイカー」を用意し、「ヒッチハイカーとの交流(交わる)」という表現も行いました。

## 適用したデザインパターン
・Singleton

https://github.com/ShinNakajima128/CrossGame/blob/master/Assets/Scripts/DesignPattern/SingletonMonoBehaviour.cs

・State

https://github.com/ShinNakajima128/CrossGame/tree/master/Assets/Scripts/System/State

・ObjectPool

https://github.com/ShinNakajima128/CrossGame/blob/master/Assets/Scripts/DesignPattern/ObjectPool.cs

・Observer

https://github.com/ShinNakajima128/CrossGame/blob/master/Assets/Scripts/System/GameManager.cs

・MVP(Model-View-Presenter)

https://github.com/ShinNakajima128/CrossGame/tree/master/Assets/Scripts/Player

## ゲームの説明
プレイヤーは「トラック」を運転し、「障害物」や「交差点」を乗り越え、ゴールにたどり着くまでの間で多くのスコア獲得を目指すランゲームとなっています。

<img src="./JOYRIDE_SS/スクリーンショット 2023-09-27 154408.jpg" width="px480" height="270px"></img>
<img src="./JOYRIDE_SS/スクリーンショット 2023-09-27 153834.jpg" width="px480" height="270px"></img>

「バリケード」を超えることでコンボ数が増加し、コンボ数に応じてスコアが伸びやすくなりますが、障害物にぶつかるとコンボがリセットされます。

<img src="./JOYRIDE_SS/スクリーンショット 2023-09-27 141457.jpg" width="200px" height="200px"></img>

道路の左右に立っている「ヒッチハイカー」は、プレイヤーが近づくと自動的に乗車します。

一人につき、走行速度が15%上昇。最大6人まで乗車可能。

<img src="./JOYRIDE_SS/スクリーンショット 2023-09-27 141231.jpg" width="200px" height="200px"></img>
<img src="./JOYRIDE_SS/スクリーンショット 2023-09-27 153451.jpg" width="200px" height="200px"></img>

「交差点」では、ここを走っている車に当たると「一定時間行動不能」となる他、乗車しているヒッチハイカーが全員車から落下し、走行速度が元に戻ります。

<img src="./JOYRIDE_SS/スクリーンショット 2023-09-27 153640.jpg" width="px200" height="200px"></img>

道路の途中に「工事中の道路」があり、ここの上を走ると数秒間走行速度が低下します。

<img src="./JOYRIDE_SS/スクリーンショット 2023-09-27 141255.jpg" width="200px" height="200px"></img>
<img src="./JOYRIDE_SS/スクリーンショット 2023-09-27 154829.jpg" width="200px" height="200px"></img>

取得することでプレイヤーが様々な効果を得る「アイテム」が道路上にランダムで出現します。

【加速アイテム】一定時間、速度が上昇します。

<img src="./JOYRIDE_SS/スクリーンショット 2023-09-27 141158.jpg" width="200px" height="200px"></img>
<img src="./JOYRIDE_SS/スクリーンショット 2023-09-27 141632.jpg" width="200px" height="200px"></img>

【すり抜けアイテム】一定時間、すり抜け状態となり、障害物に当たらなくなります。

<img src="./JOYRIDE_SS/スクリーンショット 2023-09-27 141003.jpg" width="200px" height="200px"></img>
<img src="./JOYRIDE_SS/スクリーンショット 2023-09-27 141125.jpg" width="200px" height="200px"></img>

## 操作方法
・アクセル　　　左クリック(押し続けている間)

・移動（左右）　Aキー、Dキー

## 仕様書
・ステージは直線の道路で、道幅以上には移動不可。

・ランダムに生成されるオブジェクトは、「建物」「障害物」「ヒッチハイカー」「アイテム」の4種。

・道中の障害物「バリケード」を超えるとスコア加算の他、コンボ数が増加。障害物に当たるとコンボ数はリセット。

・時間制限は無し。ゴールにたとりつくまでの時間を計測。

・プレイヤーとゴールまでの距離をUIに表示し、現在の進行度を可視化。

・アイテムは「加速アイテム」「すり抜けアイテム」の2種類。

 ・「ヒッチハイカー」は6人まで乗車可能で、一人乗車する毎に「15%」走行速度が上昇。
 
・「交差点」を走っている車に当たると一定時間行動不能、乗車している「ヒッチハイカー」は全てリセットされ、走行速度も元に戻る。

・「工事中エリア」に侵入すると、一定時間速度が低下。

・ゴールにたどり着くとゲーム終了し、リザルト画面が表示される。

・リザルト画面では、「ゲーム中に獲得したスコア」「ゴールまで乗車していたヒッチハイカーの人数に応じたスコア」「クリアタイムに応じたスコア」を合算したトータルスコアを計測。

・リザルト表示終了後、「リトライ」「タイトル画面に戻る」を選択可能。

## その他試したこと
・オブジェクトのディザリング透過処理をShaderGraphにて作成しました。

・一部クラスを「Partialクラス」として作成し、コードの肥大化を防止する試みを行いました。
