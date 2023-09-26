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

障害物にぶつからずに「バリケード」を超えることでコンボが増え、スコアが伸びやすくなります。

道路の左右に立っている「ヒッチハイカー」は、プレイヤーが近づくと自動的に乗車します。

一人につき、走行速度が10%上昇。最大6人まで乗車可能。

道路の途中に「交差点」が存在し、ここを走っている車に当たると「一定時間行動不能」となる他、乗車しているヒッチハイカーが全員車から落下し、走行速度が元に戻ります。


## 素材アセットリスト