using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public abstract class ItemBase : MonoBehaviour, IPoolable
{
    #region property
    public IObservable<Unit> InactiveObserver => _inactiveSubject;
    #endregion

    #region serialize
    #endregion

    #region private
    #endregion

    #region Constant
    #endregion

    #region Event
    private Subject<Unit> _inactiveSubject = new Subject<Unit>();
    #endregion

    #region unity methods
    private void Awake()
    {

    }

    private void Start()
    {

    }
    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
    }
    #endregion

    #region public method
    /// <summary>
    /// 使用する
    /// </summary>
    public abstract void Use(PlayerModel model);
    /// <summary>
    /// プールに戻る
    /// </summary>
    public void ReturnPool()
    {
        _inactiveSubject.OnNext(Unit.Default);
    }
    #endregion

    #region private method
    #endregion

    #region coroutine method
    #endregion
}
