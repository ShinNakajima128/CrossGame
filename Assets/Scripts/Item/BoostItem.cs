using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class BoostItem : ItemBase
{
    #region property
    #endregion

    #region serialize
    [SerializeField]
    private float _boostAmount = 15f;

    [SerializeField]
    private float _boostTime = 3f;
    #endregion

    #region private
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    #endregion

    #region public method
    public override void Use(PlayerModel model)
    {
        model.OnBoost(_boostAmount, _boostTime);
        AudioManager.PlaySE(SEType.Boost);
    }
    #endregion

    #region private method
    #endregion
    
    #region coroutine method
    #endregion
}
