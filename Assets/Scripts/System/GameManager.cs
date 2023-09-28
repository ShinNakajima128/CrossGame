using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// �Q�[���̐i�s�󋵁A�C�x���g�������Ǘ�����Manager
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
    [Tooltip("�^�C�g����ʂ�CanvasGroup")]
    [SerializeField]
    private CanvasGroup _titleGroup = default;

    [Tooltip("�C���Q�[����ʂ�CanvasGroup")]
    [SerializeField]
    private CanvasGroup _inGameGroup = default;

    [Tooltip("���U���g��ʂ�CanvasGroup")]
    [SerializeField]
    private CanvasGroup _resultGroup = default;
    #endregion

    #region private
    /// <summary>���݂̃Q�[���̏�Ԃɉ������������s���R���|�[�l���g</summary>
    private GameStateMachine _stateMachine;
    /// <summary>���݂̃Q�[���̏��</summary>
    private GameState _currentState;
    #endregion

    #region Event
    /// <summary>�C���Q�[�������ǂ�����؂�ւ���Subject</summary>
    private Subject<bool> _isInGameSubject = new Subject<bool>();
    /// <summary>�Q�[���J�n����Subject</summary>
    private Subject<Unit> _gameStartSubject = new Subject<Unit>();
    /// <summary>�Q�[�����f����Subject</summary>
    private Subject<bool> _gamePauseSubject = new Subject<bool>();
    /// <summar>�q�b�`�n�C�J�[�𑝂₷Subject</summar></summary>
    private Subject<HitchhikerType> _addHitchhikerSubject = new Subject<HitchhikerType>();
    /// <summary>�R���{�����X�V����Subject</summary>
    private Subject<Unit> _updateComboSubject = new Subject<Unit>();
    /// <summary>�R���{�������Z�b�g����Subject</summary>
    private Subject<Unit> _resetComboSubject = new Subject<Unit>();
    /// <summary>�v���C���[��e����Subject</summary>
    private Subject<Unit> _playerDamageSubject = new Subject<Unit>();
    /// <summary>�Q�[���I������Subject</summary>
    private Subject<Unit> _gameEndSubject = new Subject<Unit>();
    /// <summary>�Q�[���̓��e�����Z�b�g����Subject</summary>
    private Subject<Unit> _gameResetSubject = new Subject<Unit>();
    /// <summary>�X�R�A���X�V����Subject</summary>
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
        //��ʂ𖾓]
        FadeManager.Fade(FadeType.In);
        //�J�n���̓^�C�g����ʂ�\��
        ChangeViewGroup(GameState.Title);
    }
    #endregion

    #region public method
    /// <summary>
    /// �Q�[���̏�Ԃ��X�V����
    /// </summary>
    /// <param name="nextState"></param>
    public void ChangeGameState(GameState nextState)
    {
        if (_currentState == nextState)
        {
            Debug.Log("�����X�e�[�g�ł�");
            return;
        }
        _stateMachine.ChangeState(nextState);
        _currentState = nextState;
        ChangeViewGroup(_currentState);
    }
    /// <summary>
    /// �Q�[�����J�n����
    /// </summary>
    public void OnGameStart()
    {
        _gameStartSubject.OnNext(Unit.Default);
        AudioManager.PlaySE(SEType.Title_GameStart);

        FadeManager.Fade(FadeType.Out, () =>
        {
            FadeManager.Fade(FadeType.In);
            CameraManager.Instance.ChangeCamera(CameraType.InGame, 0f); //�J�������C���Q�[���p�ɐ؂�ւ���
            TimeManager.Instance.OnCountDown(); //�J�E���g�_�E���J�n
        });
    }

    /// <summary>
    /// �Q�[�����ăX�^�[�g����
    /// </summary>
    public void OnGameReStart()
    {
        CameraManager.Instance.ChangeCamera(CameraType.InGame, 0f);
        TimeManager.Instance.OnCountDown();
    }
    /// <summary>
    /// �C���Q�[�������ǂ�����؂�ւ���
    /// </summary>
    /// <param name="value">ON/OFF</param>
    public void OnChangeIsInGame(bool value)
    {
        _isInGameSubject.OnNext(value);
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
    /// �q�b�`�n�C�J�[��ǉ�����
    /// </summary>
    public void OnAddHitchhiker(HitchhikerType type)
    {
        _addHitchhikerSubject.OnNext(type);
    }

    /// <summary>
    /// �R���{���𑝉�����
    /// </summary>
    public void OnAddConbo()
    {
        _updateComboSubject.OnNext(Unit.Default);
    }

    /// <summary>
    /// �R���{�������Z�b�g����
    /// </summary>
    public void OnResetCombo()
    {
        _resetComboSubject.OnNext(Unit.Default);
    }
    /// <summary>
    /// �v���C���[�̔�e���������s����
    /// </summary>
    public void OnPlayerDamage()
    {
        _playerDamageSubject.OnNext(Unit.Default);
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

    /// <summary>
    /// �Q�[����ʂ�؂�ւ���
    /// </summary>
    /// <param name="state">�Q�[���̏��</param>
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