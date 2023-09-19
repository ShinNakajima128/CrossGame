using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

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
        GameManager.Instance.IsInGameObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(value => _model.ChangeIsCanOperation(value));
        _model.DamageObserber
              .TakeUntilDestroy(this)
              .Subscribe(_ => GameManager.Instance.OnPlayerDamage());
    }
    #endregion

    #region public method
    #endregion

    #region private method
    #endregion
    
    #region coroutine method
    #endregion
}
