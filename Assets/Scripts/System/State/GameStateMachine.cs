using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public partial class GameStateMachine : MonoBehaviour
{
    #region property
    #endregion

    #region private
    private StateBase _currentState;
    private TitleState _titleState = new TitleState();
    private InGameState _inGameState = new InGameState();
    private ResultState _resultState = new ResultState();
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Start()
    {
        ChangeState(GameState.Title);

        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Subscribe(_ =>
            {
                _currentState.OnUpdate();
            });
    }
    #endregion

    #region public method
    public void ChangeState(GameState nextState)
    {
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
        _currentState.OnEnter();
    }
    #endregion
}
