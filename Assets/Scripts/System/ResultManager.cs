using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// ���U���g��ʂ̋@�\���Ǘ�����Manager
/// </summary>
public class ResultManager : MonoBehaviour
{
    #region property
    public static ResultManager Instance { get; private set; }
    #endregion

    #region serialize
    [Tooltip("���U���g��ʂ̕\���@�\")]
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
    /// ���U���g��\������
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
