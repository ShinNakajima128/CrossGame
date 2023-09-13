using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;

public class PlayerView : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [SerializeField]
    private TextMeshProUGUI _scoreTMP = default;

    [SerializeField]
    private Image _hpImage = default;
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
    public void UpdateScoreView(float score)
    {
        _scoreTMP.text = $"Score : {score}";
    }
    #endregion

    #region private method
    #endregion
    
    #region coroutine method
    #endregion
}
