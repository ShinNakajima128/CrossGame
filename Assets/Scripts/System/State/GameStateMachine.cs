using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// �Q�[���̏�Ԃ��Ǘ�����R���|�[�l���g
/// </summary>
public partial class GameStateMachine : MonoBehaviour
{
    #region private
    /// <summary>���݂̏��</summary>
    private StateBase _currentState;
    /// <summary>�^�C�g����ʂ̏��</summary>
    private TitleState _titleState;
    /// <summary>�C���Q�[����ʂ̏��</summary>
    private InGameState _inGameState;
    /// <summary>���U���g��ʂ̏��</summary>
    private ResultState _resultState;
    #endregion

    #region unity methods
    private void Awake()
    {
        Initialize();
    }
    private IEnumerator Start()
    {
        //�������̂��߂�1�t���[���ҋ@
        yield return null;

        //���������̓^�C�g����ԂɈڍs
        ChangeState(GameState.Title);

        //���݂̃Q�[����Ԃ̏��������s���鏈����o�^
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
    /// ��Ԃ�ύX����
    /// </summary>
    /// <param name="nextState">���̏��</param>
    public void ChangeState(GameState nextState)
    {
        //���݂̏�Ԃ̏I�����������s
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

        //���̏�Ԃ̊J�n���������s
        _currentState.OnEnter();
    }
    #endregion

    #region private method
    /// <summary>
    /// ������
    /// </summary>
    private void Initialize()
    {
        _titleState = new TitleState();
        _inGameState = new InGameState();
        _resultState = new ResultState();
    }
    #endregion
}
