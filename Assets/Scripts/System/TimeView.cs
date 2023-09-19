using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;

public class TimeView : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [SerializeField]
    private TextMeshProUGUI _countDownTMP = default;

    [SerializeField]
    private TextMeshProUGUI _limitTimeTMP = default;
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

    }
    #endregion

    #region public method
    public void CountDownView(string value)
    {
        _countDownTMP.text = value;
    }
    public void LimitTimeView(string value)
    {
        _limitTimeTMP.text = value;
    }
    #endregion

    #region private method
    #endregion
    
    #region coroutine method
    #endregion
}
