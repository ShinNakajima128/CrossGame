using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// プーリングするオブジェクトに継承するインターフェース
/// </summary>
public interface IPoolable
{
    #region property
    IObservable<Unit> InactiveObserver { get; }
    #endregion

    #region public method
    void ReturnPool();
    #endregion
}
