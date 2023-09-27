using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class InfiltratorItem : ItemBase
{
    
    #region property
    #endregion

    #region serialize

    [SerializeField]
    private float _infiltratorTime = 5f;
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
        model.OnInfiltrator(_infiltratorTime);
    }
    #endregion

    #region private method
    #endregion

    #region coroutine method
    #endregion
}
