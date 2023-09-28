using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// ゲームの状態を管理するコンポーネント
/// </summary>
public partial class GameStateMachine : MonoBehaviour
{
    #region private
    /// <summary>現在の状態</summary>
    private StateBase _currentState;
    /// <summary>タイトル画面の状態</summary>
    private TitleState _titleState;
    /// <summary>インゲーム画面の状態</summary>
    private InGameState _inGameState;
    /// <summary>リザルト画面の状態</summary>
    private ResultState _resultState;
    #endregion

    #region unity methods
    private void Awake()
    {
        Initialize();
    }
    private IEnumerator Start()
    {
        //初期化のために1フレーム待機
        yield return null;

        //初期化時はタイトル状態に移行
        ChangeState(GameState.Title);

        //現在のゲーム状態の処理を実行する処理を登録
        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Subscribe(_ =>
            {
                _currentState.OnUpdate();
            });
    }
    #endregion

    #region public method
    /// <summary>
    /// 状態を変更する
    /// </summary>
    /// <param name="nextState">次の状態</param>
    public void ChangeState(GameState nextState)
    {
        //現在の状態の終了処理を実行
        _currentState?.OnExit();

        switch (nextState)
        {
            case GameState.Title:
                _currentState = _titleState;
                break;
            case GameState.InGame:
                _currentState = _inGameState;
                break;
            case GameState.Result:
                _currentState = _resultState;
                break;
            default:
                break;
        }

        //次の状態の開始処理を実行
        _currentState.OnEnter();
    }
    #endregion

    #region private method
    /// <summary>
    /// 初期化
    /// </summary>
    private void Initialize()
    {
        _titleState = new TitleState();
        _inGameState = new InGameState();
        _resultState = new ResultState();
    }
    #endregion
}
