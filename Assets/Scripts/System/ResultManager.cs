using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// リザルト画面の機能を管理するManager
/// </summary>
public class ResultManager : MonoBehaviour
{
    #region property
    public static ResultManager Instance { get; private set; }
    #endregion

    #region serialize
    [Tooltip("リザルト画面の表示機能")]
    [SerializeField]
    private ResultView _resultView = default;
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
        Instance = this;
    }

    private void Start()
    {

    }
    #endregion

    #region public method
    /// <summary>
    /// リザルトを表示する
    /// </summary>
    public void OnResult()
    {
        _resultView.OnResultView();
        AudioManager.PlayBGM(BGMType.Result);
    }
    #endregion

    #region private method
    #endregion
    
    #region coroutine method
    #endregion
}
