using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Crossroad : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    #endregion

    #region private
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Start()
    {
        this.OnTriggerEnterAsObservable()
            .TakeUntilDestroy(this)
            .Where(x => x.gameObject.CompareTag(GameTag.Player))
            .Subscribe(x =>
            {
                AudioManager.ChangePlayEnviroment(true);
            });

        this.OnTriggerExitAsObservable()
            .TakeUntilDestroy(this)
            .Where(x => x.gameObject.CompareTag(GameTag.Player))
            .Subscribe(x =>
            {
                AudioManager.ChangePlayEnviroment(false);
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
