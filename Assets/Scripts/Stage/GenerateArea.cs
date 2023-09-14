using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class GenerateArea : MonoBehaviour
{
    #region property
    public IObservable<bool> IsInAreaObserver => _isInArea;
    #endregion

    #region serialize
    #endregion

    #region private
    #endregion

    #region Constant
    #endregion

    #region Event
    private ReactiveProperty<bool> _isInArea = new ReactiveProperty<bool>(false);
    #endregion

    #region unity methods
    private void Awake()
    {

    }

    private void Start()
    {
        this.OnTriggerEnterAsObservable()
            .TakeUntilDestroy(this)
            .Where(x => x.gameObject.CompareTag(GameTag.Player))
            .Subscribe(x =>
            {
                _isInArea.Value = true;
            });

        this.OnTriggerExitAsObservable()
            .TakeUntilDestroy(this)
            .Where(x => x.gameObject.CompareTag(GameTag.Player))
            .Subscribe(x =>
            {
                _isInArea.Value = false;
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
