using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// ゲームの進行状況、イベント処理を管理するManager
/// </summary>
[RequireComponent(typeof(GameStateMachine))]
public class GameManager : SingletonMonoBehaviour<GameManager>
{
    #region property
    public IObservable<bool> IsInGameObserver => _isInGameSubject;
    public IObservable<Unit> GameStartObserver => _gameStartSubject;
    public IObservable<bool> GamePauseObserver => _gamePauseSubject;
    public IObservable<HitchhikerType> AddHitchhikerObserver => _addHitchhikerSubject;
    public IObservable<Unit> UpdateComboObserver => _updateComboSubject;
    public Subject<Unit> ResetComboObserver => _resetComboSubject;
    public IObservable<Unit> PlayerDamageObserver => _playerDamageSubject;
    public IObservable<Unit> GameEndObserver => _gameEndSubject;
    public IObservable<Unit> GameResetObserver => _gameResetSubject;
    public IObservable<int> UpdateScoreObserver => _updateScoreSubject;

    protected override bool IsDontDestroyOnLoad => true;
    #endregion

    #region serialize
    [Tooltip("タイトル画面のCanvasGroup")]
    [SerializeField]
    private CanvasGroup _titleGroup = default;

    [Tooltip("インゲーム画面のCanvasGroup")]
    [SerializeField]
    private CanvasGroup _inGameGroup = default;

    [Tooltip("リザルト画面のCanvasGroup")]
    [SerializeField]
    private CanvasGroup _resultGroup = default;
    #endregion

    #region private
    /// <summary>現在のゲームの状態に応じた処理を行うコンポーネント</summary>
    private GameStateMachine _stateMachine;
    /// <summary>現在のゲームの状態</summary>
    private GameState _currentState;
    #endregion

    #region Event
    /// <summary>インゲーム中かどうかを切り替えるSubject</summary>
    private Subject<bool> _isInGameSubject = new Subject<bool>();
    /// <summary>ゲーム開始時のSubject</summary>
    private Subject<Unit> _gameStartSubject = new Subject<Unit>();
    /// <summary>ゲーム中断時のSubject</summary>
    private Subject<bool> _gamePauseSubject = new Subject<bool>();
    /// <summar>ヒッチハイカーを増やすSubject</summar></summary>
    private Subject<HitchhikerType> _addHitchhikerSubject = new Subject<HitchhikerType>();
    /// <summary>コンボ数を更新するSubject</summary>
    private Subject<Unit> _updateComboSubject = new Subject<Unit>();
    /// <summary>コンボ数をリセットするSubject</summary>
    private Subject<Unit> _resetComboSubject = new Subject<Unit>();
    /// <summary>プレイヤー被弾時のSubject</summary>
    private Subject<Unit> _playerDamageSubject = new Subject<Unit>();
    /// <summary>ゲーム終了時のSubject</summary>
    private Subject<Unit> _gameEndSubject = new Subject<Unit>();
    /// <summary>ゲームの内容をリセットするSubject</summary>
    private Subject<Unit> _gameResetSubject = new Subject<Unit>();
    /// <summary>スコアを更新するSubject</summary>
    private Subject<int> _updateScoreSubject = new Subject<int>();
    #endregion

    #region unity methods
    protected override void Awake()
    {
        base.Awake();

        _stateMachine = GetComponent<GameStateMachine>();
    }
    private void Start()
    {
        //画面を明転
        FadeManager.Fade(FadeType.In);
        //開始時はタイトル画面を表示
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
            Debug.Log("同じステートです");
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
            CameraManager.Instance.ChangeCamera(CameraType.InGame, 0f); //カメラをインゲーム用に切り替える
            TimeManager.Instance.OnCountDown(); //カウントダウン開始
        });
    }

    /// <summary>
    /// ゲームを再スタートする
    /// </summary>
    public void OnGameReStart()
    {
        CameraManager.Instance.ChangeCamera(CameraType.InGame, 0f);
        TimeManager.Instance.OnCountDown();
    }
    /// <summary>
    /// インゲーム中かどうかを切り替える
    /// </summary>
    /// <param name="value">ON/OFF</param>
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
    /// <summary>
    /// ヒッチハイカーを追加する
    /// </summary>
    public void OnAddHitchhiker(HitchhikerType type)
    {
        _addHitchhikerSubject.OnNext(type);
    }

    /// <summary>
    /// コンボ数を増加する
    /// </summary>
    public void OnAddConbo()
    {
        _updateComboSubject.OnNext(Unit.Default);
    }

    /// <summary>
    /// コンボ数をリセットする
    /// </summary>
    public void OnResetCombo()
    {
        _resetComboSubject.OnNext(Unit.Default);
    }
    /// <summary>
    /// プレイヤーの被弾処理を実行する
    /// </summary>
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

    /// <summary>
    /// ゲーム画面を切り替える
    /// </summary>
    /// <param name="state">ゲームの状態</param>
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
}