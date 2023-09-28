using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// プレイヤーのModelとViewの処理を繋げる機能を持つコンポーネント
/// </summary>
public class PlayerPresenter : MonoBehaviour
{
    #region serialize
    [Tooltip("プレイヤーの動作処理を行うコンポーネント")]
    [SerializeField]
    private PlayerModel _model = default;

    [Tooltip("プレイヤー情報を表示するコンポーネント")]
    [SerializeField]
    private PlayerView _view = default;
    #endregion

    #region unity methods
    private void Start()
    {
        //ゲームの状態によってプレイヤー操作可能かを切り替えるイベント処理を登録
        GameManager.Instance.IsInGameObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(value => _model.ChangeIsCanOperation(value));

        //ヒッチハイカー乗車時のイベント処理を登録
        GameManager.Instance.AddHitchhikerObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(_ => _model.AddHitchhiker());

        //コンボ数変更時のイベント処理を登録
        GameManager.Instance.UpdateComboObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(_ => _model.AddCombo());

        //コンボ数リセット時のイベント処理を登録
        GameManager.Instance.ResetComboObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(_ => _model.ResetCombo());

        //ゲーム終了時のイベント処理を登録
        GameManager.Instance.GameEndObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(_ =>
                            {
                                _model.ResetVelocity();
                                _model.PlayerReset();
                            });

        //ゲーム初期化時のイベント処理を登録
        GameManager.Instance.GameResetObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(_ => _model.PlayerReset());

        //スコアが更新されて画面の表示を更新するイベント処理をScoreManagerに登録
        ScoreManager.Instance.CurrentScoreObserver
                             .TakeUntilDestroy(this)
                             .Subscribe(value => _view.UpdateScoreView(value));

        //プレイヤーダメージ時のイベント処理をGameManagerを登録
        _model.DamageObserber
              .TakeUntilDestroy(this)
              .Subscribe(_ => GameManager.Instance.OnPlayerDamage());

        //コンボ数更新時のイベント処理を登録
        _model.Status.CurrentComboObserver
                     .TakeUntilDestroy(this)
                     .Subscribe(value => _view.UpdateComboView(value));
    }
    #endregion
}
