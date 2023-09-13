using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class StageManager : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [SerializeField]
    private Transform _playerTrans = default;

    [SerializeField]
    private Transform _goalTrans = default;
    #endregion

    #region private
    private bool _isInGame = false;
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
                            .Subscribe(value =>
                            {
                                _isInGame = value;
                            });
    }
    #endregion

    #region public method
    #endregion

    #region private method
    #endregion
    
    #region coroutine method
    #endregion
}
