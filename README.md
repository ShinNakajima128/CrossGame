## タイトル
「JOYRIDE」
…「スリルを味わうために無謀な運転をすること」「車を乱暴に乗り回す」などの意味を持ったスラングとして使われているこの言葉がイメージに合ったため、これをタイトルにしました。

## テーマ「交わる」を表現した点
・「交わる」をイメージしながらアセットを探していた際に、流れてきた町のアセットを見て「交差点」が思い浮かび、直線の道路を走りながら障害となる交差点を突破する「ランゲーム」を企画しました。

・もう一つのテーマの要素として、プレイヤーの車に乗車する「ヒッチハイカー」を用意し、「ヒッチハイカーとの交流(交わる)」という表現も行いました。

## 適用したデザインパターン
・Singleton

・State

・ObjectPool

・Observer

・MVP(Model-View-Presenter)

## ゲームの説明
プレイヤーは「トラック」を運転し、「障害物」や「交差点」を乗り越え、ゴールにたどり着くまでの間で多くのスコア獲得を目指すランゲームとなっています。

「バリケード」を超えることでコンボ数が増加し、コンボ数に応じてスコアが伸びやすくなりますが、障害物にぶつかるとコンボがリセットされます。

道路の左右に立っている「ヒッチハイカー」は、プレイヤーが近づくと自動的に乗車します。

一人につき、走行速度が10%上昇。最大6人まで乗車可能。

「交差点」では、ここを走っている車に当たると「一定時間行動不能」となる他、乗車しているヒッチハイカーが全員車から落下し、走行速度が元に戻ります。

道路の途中に「工事中の道路」があり、ここの上を走ると数秒間走行速度が低下します。

取得することでプレイヤーが有益な効果を得る「アイテム」が道路上にランダムで出現します。
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
 
