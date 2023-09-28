using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ScoreManager : MonoBehaviour
{
    #region property
    public static ScoreManager Instance { get; private set; }
    public IObservable<int> CurrentScoreObserver => _currentScoreRP;
    public int CurrentScore => _currentScoreRP.Value;
    #endregion

    #region serialize
    [SerializeField]
    private ScoreView _scoreView = default;
    #endregion

    #region private
    private ReactiveProperty<int> _currentScoreRP = new ReactiveProperty<int>();
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
        GameManager.Instance.UpdateScoreObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(value => AddScore(value));

        GameManager.Instance.GameResetObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(_ => ResetScore());
    }
    #endregion

    #region public method
    /// <summary>
    /// スコアを加算する
    /// </summary>
    /// <param name="score">スコア</param>
    public void AddScore(int score)
    {
        _currentScoreRP.Value += score;
    }
    public void ChangeScoreBoardView(bool value)
    {

    }
    #endregion
    /// <summary>
    /// スコアをリセットする
    /// </summary>
    #region private method
    private void ResetScore()
    {
        _currentScoreRP.Value = 0;
    }
    #endregion
    
    #region coroutine method
    #endregion
}
