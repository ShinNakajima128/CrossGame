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
    private TextMeshProUGUI _progressTimeTMP = default;
    #endregion

    #region private
    private int _currentMinuteAmount = 0;
    private int _currentSecondAmount = 0;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    #endregion

    #region public method
    public void CountDownView(string value)
    {
        _countDownTMP.text = value;
    }
    public void ProgressTimeView(float value)
    {
        _progressTimeTMP.text = $"{((int)value / 60).ToString().PadLeft(2, '0')}:{((int)value % 60).ToString().PadLeft(2, '0')}:{((value - Mathf.FloorToInt(value)) * 100).ToString("F0").PadLeft(2, '0')}";
    }
    #endregion

    #region private method
    #endregion
    
    #region coroutine method
    #endregion
}
