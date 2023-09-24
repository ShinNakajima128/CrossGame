using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// プレイヤーのModelとViewの処理を繋げる機能を持つコンポーネント
/// </summary>
public class PlayerPresenter : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [SerializeField]
    private PlayerModel _model = default;

    [SerializeField]
    private PlayerView _view = default;
    #endregion

    #region private
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {

    }

    private void Start()
    {
        //ゲームの状態によってプレイヤー操作可能かを切り替えるイベント処理をGameManagerに登録
        GameManager.Instance.IsInGameObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(value => _model.ChangeIsCanOperation(value));
        
        GameManager.Instance.UpdateComboObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(_ => _model.AddCombo());

        GameManager.Instance.PlayerDamageObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(_ => _model.Status.ResetStatus());

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

    #region public method
    #endregion

    #region private method
    #endregion
    
    #region coroutine method
    #endregion
}
