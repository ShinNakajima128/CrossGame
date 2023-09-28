using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

/// <summary>
/// �v���C���[�̓���̋@�\�����R���|�[�l���g
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public partial class PlayerModel : MonoBehaviour, IDamagable
{
    #region property
    public IObservable<Unit> DamageObserber => _damageSubject;
    public bool IsInvincible => _isInvincibled;
    public PlayerStatus Status => _status;
    #endregion

    #region serialize
    [Header("�e���l")]
    [SerializeField]
    private int _maxHP = 3;

    [Tooltip("�ړ����x")]
    [SerializeField]
    private float _moveSpeed = 5.0f;

    [Tooltip("��]���x")]
    [SerializeField]
    private float _rotateSpeed = 5.0f;

    [Tooltip("�_���[�W��̖��G����")]
    [SerializeField]
    private float _afterDamageInvincibleTime = 2.0f;

    [Tooltip("�v���C���[�̃I�u�W�F�N�g���f����Transform")]
    [SerializeField]
    private Transform _playerModelTrans = default;

    [Tooltip("�����G�t�F�N�g�̃I�u�W�F�N�g")]
    [SerializeField]
    private GameObject _accelEffect = default;

    [Header("�e�X�e�[�^�X���̃��f���̐F")]
    [SerializeField]
    private Color _slowStateColor = Color.black;
    #endregion

    #region private
    /// <summary>�v���C���[�̓��͏��</summary>
    private PlayerInput _input;
    /// <summary>���������p�̃R���|�[�l���g</summary>
    private Rigidbody _rb;
    /// <summary>�v���C���[�̏�Ԃ����R���|�[�l���g</summary>
    private PlayerStatus _status;

    /// <summary>���͍��W</summary>
    private Vector2 _inputAxis;
    /// <summary>���݂̈ړ����x</summary>
    private float _currentMoveSpeed;
    /// <summary>���X�̈ړ����x�B�����A�C�e���̌��ʏI�����ȂǂɊ��p</summary>
    private float _originSpeed;
    /// <summary>����\���ǂ���</summary>
    private bool _isCanOperate = false;
    /// <summary>�_���[�W���������ǂ���</summary>
    private bool _isDamaged = false;
    /// <summary>���G�����ǂ���</summary>
    private bool _isInvincibled = false;

    /// <summary>�v���C���[�I�u�W�F�N�g��Renderer</summary>
    private Renderer _playerModelRenderer;
    /// <summary>����������Coroutine</summary>
    private Coroutine _boostCoroutine;
    /// <summary>���蔲��������Coroutine</summary>
    private Coroutine _InfiltratorCoroutine;
    /// <summary>�_���[�W������Coroutine</summary>
    private Coroutine _damageCoroutine;
    #endregion

    #region Event
    private Subject<Unit> _damageSubject = new Subject<Unit>();
    #endregion

    #region unity methods
    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
        _status = new PlayerStatus();

        _playerModelRenderer = _playerModelTrans.gameObject.GetComponent<Renderer>();
        _currentMoveSpeed = _moveSpeed;
    }

    private void Start()
    {
        //�v���C���[�̓��͏�����o�^
        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Where(_ => _isCanOperate)
            .Subscribe(_ =>
            {
                if (!_isDamaged)
                {
                    SetRotateInput();
                    OnMoving();
                }
                DebugInput();
            });
    }

    private void OnEnable()
    {
        _input.actions["Move"].performed += OnRotate;
        _input.actions["Move"].canceled += OnResetInput;
    }
    private void OnDisable()
    {
        _input.actions["Move"].performed -= OnRotate;
        _input.actions["Move"].canceled -= OnResetInput;
    }
    #endregion

    #region public method
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="boostAmount">��������l</param>
    /// <param name="boostTime">�������Ă��鎞��</param>
    public void OnBoost(float boostAmount, float boostTime)
    {
        BreakBoost();
       _boostCoroutine = StartCoroutine(BoostCoroutine(boostAmount, boostTime));
    }

    /// <summary>
    /// �����𒆒f����
    /// </summary>
    public void BreakBoost()
    {
        if (_boostCoroutine != null)
        {
            StopCoroutine(_boostCoroutine);
            _boostCoroutine = null;
            _currentMoveSpeed = _originSpeed;
            _accelEffect.SetActive(false);
        }
    }
    /// <summary>
    /// �ݑ�������
    /// </summary>
    /// <param name="slowTime">�ݑ��ɂȂ��Ă��鎞��</param>
    public void OnSlow(float slowTime)
    {
        StartCoroutine(SlowCoroutine(slowTime));
    }

    /// <summary>
    /// ����������
    /// </summary>
    /// <param name="infiltratorTime">���߂��Ă��鎞��</param>
    public void OnInfiltrator(float infiltratorTime)
    {
        if (_InfiltratorCoroutine != null)
        {
            StopCoroutine(_InfiltratorCoroutine);
            _InfiltratorCoroutine = null;
            _status.ChangeState(this, PlayerState.Normal);
        }
        _InfiltratorCoroutine = StartCoroutine(InfiltratorCoroutine(infiltratorTime));
    }
    /// <summary>
    /// �_���[�W���󂯂�
    /// </summary>
    public void Damage()
    {
        if (_isDamaged || _status.CurrentState == PlayerState.Invincible || _status.CurrentState == PlayerState.Infiltrator)
        {
            return;
        }
        BreakBoost();
        _damageCoroutine = StartCoroutine(DamageCoroutine());
        AudioManager.PlaySE(SEType.Damage_Player);
    }
    /// <summary>
    /// �q�b�`�n�C�J�[�𑝂₷
    /// </summary>
    public void AddHitchhiker()
    {
        _status.ChangeSpeedAccordingHitchhiker(this);
    }
    /// <summary>
    /// ��Q����ʉ߂����R���{���𑝂₷
    /// </summary>
    public void AddCombo()
    {
        _status.AddComboNum();
    }
    /// <summary>
    /// �R���{�������Z�b�g����
    /// </summary>
    public void ResetCombo()
    {
        _status.ResetCombo();
    }
    /// <summary>
    /// �v���C���[����̉\/�s��؂�ւ���
    /// </summary>
    /// <param name="value">�\/�s��</param>
    public void ChangeIsCanOperation(bool value)
    {
        _isCanOperate = value;
    }
    /// <summary>
    /// �v���C���[�̏�Ԃ����Z�b�g����
    /// </summary>
    public void PlayerReset()
    {
        _status.ChangeTransparency(this, 1f);
        _status.ChangeState(this, PlayerState.Normal);
        _status.ResetStatus();
        _currentMoveSpeed = _moveSpeed;
        _playerModelTrans.eulerAngles = Vector3.zero;
    }

    /// <summary>
    /// �v���C���[�̑��x�����Z�b�g����
    /// </summary>
    public void ResetVelocity()
    {
        _rb.velocity = Vector3.zero;
        _accelEffect.SetActive(false);
    }
    #endregion

    #region private method
    /// <summary>
    /// �ړ�����
    /// </summary>
    private void OnMoving()
    {
        if (_input.actions["Throttle"].IsPressed())
        {
            _rb.velocity = _playerModelTrans.forward * _currentMoveSpeed;
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }

        if (_input.actions["Throttle"].WasPressedThisFrame())
        {
            AudioManager.PlaySE(SEType.Accel);
        }
        //Debug.Log($"���݂̑��x{_rb.velocity.magnitude}");
    }
    /// <summary>
    /// ���͕������Z�b�g����
    /// </summary>
    /// <param name="dir">���͕���</param>
    private void SetDirection(Vector2 dir)
    {
        _inputAxis = dir;
    }
    /// <summary>
    /// ��]����
    /// </summary>
    /// <param name="obj"></param>
    private void OnRotate(InputAction.CallbackContext obj)
    {
        var value = obj.ReadValue<Vector2>();
        value.y = 0;
        SetDirection(value);
    }
    /// <summary>
    /// ��]�����Z�b�g����
    /// </summary>
    private void SetRotateInput()
    {
        if (_inputAxis != Vector2.zero && _rb.velocity.magnitude != 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_inputAxis);
            _playerModelTrans.rotation = Quaternion.Slerp(_playerModelTrans.rotation, targetRotation, Time.deltaTime * _rotateSpeed);
        }
    }
    /// <summary>
    /// ���͏������Z�b�g����
    /// </summary>
    /// <param name="obj"></param>
    private void OnResetInput(InputAction.CallbackContext obj)
    {
        SetDirection(Vector2.zero);
    }
    /// <summary>
    /// �f�o�b�O����
    /// </summary>
    private void DebugInput()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _status.ChangeState(this, PlayerState.Normal);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            _status.ChangeState(this, PlayerState.Slowing);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            _status.ChangeState(this, PlayerState.Infiltrator);
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            OnBoost(15, 5);
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            Damage();
        }
#endif
    }
    #endregion

    #region coroutine method
    /// <summary>
    /// ����������Coroutine
    /// </summary>
    /// <param name="boostAmount">������</param>
    /// <param name="boostTime">��������</param>
    /// <returns></returns>
    private IEnumerator BoostCoroutine(float boostAmount, float boostTime)
    {
        _originSpeed = _currentMoveSpeed;
        var moveSpeed = _currentMoveSpeed + boostAmount;
        
        _accelEffect.SetActive(true);

        //���X�ɉ���
        yield return DOTween.To(() =>
                            _currentMoveSpeed,
                            x => _currentMoveSpeed = x,
                            moveSpeed,
                            0.5f)
                            .WaitForCompletion();

        yield return new WaitForSeconds(boostTime);

        //���X�Ɍ���
        yield return DOTween.To(() =>
                             _currentMoveSpeed,
                             x => _currentMoveSpeed = x,
                             _originSpeed,
                             0.5f)
                             .WaitForCompletion();

        _accelEffect.SetActive(false);
        AudioManager.PlaySE(SEType.BackToNormalState);
    }

    /// <summary>
    /// ���蔲��������COroutine
    /// </summary>
    /// <param name="infiltratorTime">���蔲���\����</param>
    /// <returns></returns>
    private IEnumerator InfiltratorCoroutine(float infiltratorTime)
    {
        _status.ChangeState(this, PlayerState.Infiltrator);
        AudioManager.PlaySE(SEType.Infiltrator);

        yield return new WaitForSeconds(infiltratorTime - 2);

        //���蔲����ԏI��2�b�O�ɂȂ�Ɠ_�ł��A�v���C���[�ɏI���ԋ߂�`���鏈�����s��
        for (int i = 0; i < 10; i++)
        {
            if (i % 2 == 0)
            {
                _status.ChangeTransparency(this, 0.8f);
            }
            else
            {
                _status.ChangeTransparency(this, 0.3f);
            }
            yield return new WaitForSeconds(0.2f);
        }

        //�v���C���[���f���̓��ߏ����̏Փ˂����������߁A1�t���[���ҋ@���邱�ƂőΏ�
        yield return null;

        //�v���C���[�̏�Ԃ�ʏ��Ԃɖ߂�
        _status.ChangeState(this, PlayerState.Normal);
        AudioManager.PlaySE(SEType.BackToNormalState);
    }
    /// <summary>
    /// ���x�ቺ������Coroutine
    /// </summary>
    /// <param name="slowTime">���x�ቺ����</param>
    /// <returns></returns>
    private IEnumerator SlowCoroutine(float slowTime)
    {
        _status.ChangeState(this, PlayerState.Slowing);
        AudioManager.PlaySE(SEType.Slow_Player);

        yield return new WaitForSeconds(slowTime);

        //�v���C���[�̏�Ԃ�ʏ��Ԃɖ߂�
        _status.ChangeState(this, PlayerState.Normal);
    }
    /// <summary>
    /// �_���[�W������Coroutine
    /// </summary>
    /// <returns></returns>
    private IEnumerator DamageCoroutine()
    {
        _isDamaged = true;
        _rb.velocity = Vector2.zero;
        _currentMoveSpeed = _moveSpeed;
        _damageSubject.OnNext(Unit.Default);

        //�Փ˂ɂ��X�s����\��
        yield return _playerModelTrans.DOLocalRotate(new Vector3(0f, _playerModelTrans.localRotation.y + 720f, 0f),
                                                    1.0f, RotateMode.FastBeyond360)
                                                    .SetEase(Ease.Linear)
                                                    .WaitForCompletion();
        Debug.Log("�_���[�W�㖳�G����");
        _isDamaged = false;
        _isInvincibled = true;
        _status.ChangeTransparency(this, 0.5f);

        yield return new WaitForSeconds(_afterDamageInvincibleTime);

        _status.ChangeTransparency(this, 1f);
        _isInvincibled = false;
    }
    #endregion
}
