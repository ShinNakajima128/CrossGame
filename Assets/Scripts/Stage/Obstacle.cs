using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Obstacle : MonoBehaviour, IPoolable
{
    #region property
    public IObservable<Unit> InactiveObserver => throw new NotImplementedException();
    #endregion

    #region serialize
    [SerializeField]
    private ObstacleData _data = default;
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
        this.OnCollisionEnterAsObservable()
            .TakeUntilDestroy(this)
            .Where(x => x.gameObject.CompareTag(GameTag.Player))
            .Subscribe(x =>
            {
                
            });
    }
    #endregion

    #region public method
    public void ReturnPool()
    {
        throw new NotImplementedException();
    }
    #endregion

    #region private method
    #endregion
    
    #region coroutine method
    #endregion
}
