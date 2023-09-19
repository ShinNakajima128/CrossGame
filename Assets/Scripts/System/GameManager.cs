using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(GameStateMachine))]
public class GameManager : SingletonMonoBehaviour<GameManager>
{
    #region property
    public IObservable<bool> IsInGameObserver => _isInGameSubject;
    public IObservable<Unit> GameStartObserver => _gameStartSubject;
    public IObservable<bool> GamePauseObserver => _gamePauseSubject;
    public IObservable<Unit> PlayerDamageObserver => _playerDamageSubject;
    public IObservable<HitchHikerType> AddHitchhikerObserver => _addHitchhikerSubject;
    public IObservable<Unit> GameEndObserver => _gameEndSubject;
    public IObservable<Unit> GameResetObserver => _gameResetSubject;
    public IObservable<int> AddScoreObserver => _addScoreSubject;
    #endregion

    #region serialize
    [SerializeField]
    private CanvasGroup _titleGroup = default;

    [SerializeField]
    private CanvasGroup _inGameGroup = default;

    [SerializeField]
    private CanvasGroup _resultGroup = default;
    #endregion

    #region private
    private GameStateMachine _stateMachine;

    private GameState _currentState;
    private StageLevel _currentStageLevel;
    #endregion

    #region Constant
    #endregion

    #region Event
    /// <summary>インゲーム中かどうかを切り替えるSubject</summary>
    private Subject<bool> _isInGameSubject = new Subject<bool>();
    /// <summary>ゲーム開始時のSubject</summary>
    private Subject<Unit> _gameStartSubject = new Subject<Unit>();
    /// <summary>ゲーム中断時のSubject</summary>
    private Subject<bool> _gamePauseSubject = new Subject<bool>();
    /// <summar>ヒッチハイカーを増やすSubject</summar></summary>
    private Subject<HitchHikerType> _addHitchhikerSubject = new Subject<HitchHikerType>();
    /// <summary>プレイヤー被弾時のSubject</summary>
    private Subject<Unit> _playerDamageSubject = new Subject<Unit>();
    /// <summary>ゲーム終了時のSubject</summary>
    private Subject<Unit> _gameEndSubject = new Subject<Unit>();
    /// <summary>ゲームの内容をリセットするSubject</summary>
    private Subject<Unit> _gameResetSubject = new Subject<Unit>();
    /// <summary>ゲームの内容をリセットするSubject</summary>
    private Subject<int> _addScoreSubject = new Subject<int>();
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

        _stateMachine = GetComponent<GameStateMachine>();
    }
    private void Start()
    {
        FadeManager.Fade(FadeType.In);
        AudioManager.PlayBGM(BGMType.Title);
        ChangeViewGroup(GameState.Title);
    }
    #endregion

    #region public method
    /// <summary>
    /// ゲームの状態を更新する
    /// </summary>
    /// <param name="nextState"></param>
    public void ChangeGameState(GameState nextState)
    {
        if (_currentState == nextState)
        {
            return;
        }
        _stateMachine.ChangeState(nextState);
        _currentState = nextState;
        ChangeViewGroup(_currentState);
    }
    /// <summary>
    /// ゲームを開始する
    /// </summary>
    public void OnGameStart()
    {
        _gameStartSubject.OnNext(Unit.Default);
        AudioManager.PlaySE(SEType.Title_GameStart);

        FadeManager.Fade(FadeType.Out, () =>
        {
            FadeManager.Fade(FadeType.In);
            CameraManager.Instance.ChangeCamera(CameraType.InGame, 0f);
            TimeManager.Instance.OnCountDown();
        });
    }

    public void OnChangeIsInGame(bool value)
    {
        _isInGameSubject.OnNext(value);
    }
    /// <summary>
    /// ゲームを中断する
    /// </summary>
    /// /// <param name="value">ポーズするかどうか</param>
    public void OnGamePause(bool value)
    {
        _gamePauseSubject.OnNext(value);
        _isInGameSubject.OnNext(!value);
    }

    public void OnPlayerDamage()
    {
        _playerDamageSubject.OnNext(Unit.Default);
    }
    /// <summary>
    /// ゲームを終了する
    /// </summary>
    public void OnGameEnd()
    {
        _gameEndSubject.OnNext(Unit.Default);
        _isInGameSubject.OnNext(false);
    }

    /// <summary>
    /// ゲームをリセットする
    /// </summary>
    public void OnGameReset()
    {
        _gameResetSubject.OnNext(Unit.Default);
    }

    public void ChangeViewGroup(GameState state)
    {
        switch (state)
        {
            case GameState.Title:
                _titleGroup.alpha = 1;
                _inGameGroup.alpha = 0;
                _resultGroup.alpha = 0;
                break;
            case GameState.InGame:
                _titleGroup.alpha = 0;
                _inGameGroup.alpha = 1;
                _resultGroup.alpha = 0;
                break;
            case GameState.Result:
                _titleGroup.alpha = 0;
                _inGameGroup.alpha = 0;
                _resultGroup.alpha = 1;
                break;
            default:
                break;
        }
    }
    #endregion

    #region private method
    #endregion

    #region coroutine method
    #endregion
}