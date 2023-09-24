using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

/// <summary>
/// プレイヤーがダメージを受けた際に使用するラグドールの機能を持つコンポーネント
/// </summary>
public class Ragdoll : MonoBehaviour, IPoolable
{
    public IObservable<Unit> InactiveObserver => _inactiveSubject;
    #region property
    #endregion

    #region serialize
    [Tooltip("出現してから消えるまでの時間")]
    [SerializeField]
    private float _intctiveTime = 5.0f;
    #endregion

    #region private
    #endregion

    #region Constant
    #endregion

    #region Event
    private Subject<Unit> _inactiveSubject = new Subject<Unit>();
    #endregion

    #region unity methods
    private void OnEnable()
    {
        StartCoroutine(InactiveCoroutine());
    }

    private void OnDisable()
    {
        ReturnPool();
    }
    #endregion

    #region public method
    public void ReturnPool()
    {
        _inactiveSubject.OnNext(Unit.Default);
    }
    #endregion

    #region private method
    #endregion

    #region coroutine method
    private IEnumerator InactiveCoroutine()
    {
        yield return new WaitForSeconds(_intctiveTime);

        gameObject.SetActive(false);
    }
    #endregion
}
