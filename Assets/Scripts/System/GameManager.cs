using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    #region property
    public IObservable<bool> IsInGameObserver => _isInGameSubject;
    public IObservable<Unit> GameStartObserver => _gameStartSubject;
    public IObservable<bool> GamePauseObserver => _gamePauseSubject;
    public IObservable<Unit> GameEndObserver => _gameEndSubject;
    public IObservable<Unit> GameResetObserver => _gameResetSubject;
    #endregion

    #region serialize
    #endregion

    #region private
    #endregion

    #region Constant
    #endregion

    #region Event
    /// <summary>�C���Q�[�������ǂ�����؂�ւ���Subject</summary>
    private Subject<bool> _isInGameSubject = new Subject<bool>();
    /// <summary>�Q�[���J�n����Subject</summary>
    private Subject<Unit> _gameStartSubject = new Subject<Unit>();
    /// <summary>�Q�[�����f����Subject</summary>
    private Subject<bool> _gamePauseSubject = new Subject<bool>();
    /// <summary>�Q�[���I������Subject</summary>
    private Subject<Unit> _gameEndSubject = new Subject<Unit>();
    /// <summary>�Q�[���̓��e�����Z�b�g����Subject</summary>
    private Subject<Unit> _gameResetSubject = new Subject<Unit>();
    #endregion

    #region unity methods
    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {

    }
    #endregion

    #region public method
    /// <summary>
    /// �Q�[�����J�n����
    /// </summary>
    public void OnGameStart()
    {
        _gameStartSubject.OnNext(Unit.Default);
        _isInGameSubject.OnNext(true);
    }

    /// <summary>
    /// �Q�[���𒆒f����
    /// </summary>
    /// /// <param name="value">�|�[�Y���邩�ǂ���</param>
    public void OnGamePause(bool value)
    {
        _gamePauseSubject.OnNext(value);
        _isInGameSubject.OnNext(!value);
    }
    /// <summary>
    /// �Q�[�����I������
    /// </summary>
    public void OnGameEnd()
    {
        _gameEndSubject.OnNext(Unit.Default);
        _isInGameSubject.OnNext(false);
    }

    /// <summary>
    /// �Q�[�������Z�b�g����
    /// </summary>
    public void OnGameReset()
    {
        _gameResetSubject.OnNext(Unit.Default);
    }
    #endregion

    #region private method
    #endregion

    #region coroutine method
    #endregion
}
