using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ScoreManager : MonoBehaviour
{
    #region property
    public static ScoreManager Instance { get; private set; }
    public IObservable<int> CurrentScoreRP => _currentScore;
    #endregion

    #region serialize
    #endregion

    #region private
    private ReactiveProperty<int> _currentScore = new ReactiveProperty<int>();
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
        GameManager.Instance.AddScoreObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(value => AddScore(value));
    }
    #endregion

    #region public method
    public void AddScore(int score)
    {
        _currentScore.Value += score;
    }
    #endregion

    #region private method
    private void ResetScore()
    {
        _currentScore.Value = 0;
    }
    #endregion
    
    #region coroutine method
    #endregion
}
