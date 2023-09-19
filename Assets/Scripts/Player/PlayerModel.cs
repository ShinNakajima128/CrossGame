using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public partial class PlayerModel : MonoBehaviour, IDamagable
{
    #region property
    public IObservable<Unit> DamageObserber => _damageSubject;
    public bool IsInvincible => _isInvincibled;
    #endregion

    #region serialize
    [Header("各数値")]
    [SerializeField]
    private int _maxHP = 3;

    [SerializeField]
    private float _moveSpeed = 5.0f;

    [SerializeField]
    private float _rotateSpeed = 5.0f;

    [SerializeField]
    private float _maxSpeed = 20f;

    [SerializeField]
    private float _afterDamageInvincibleTime = 2.0f;

    [SerializeField]
    private Transform _playerModelTrans = default;

    [Header("各ステータス時のモデルの色")]
    [SerializeField]
    private Color _slowStateColor = Color.black;

    [SerializeField]
    private Transform _explosionPoint = default;
    #endregion

    #region private
    private PlayerInput _input;
    private Rigidbody _rb;
    private PlayerStatus _status;

    private Vector2 _inputAxis;
    private float _currentMoveSpeed;
    private bool _isCanOperate = false;
    private bool _isDamaged = false;
    private bool _isInvincibled = false;

    private Renderer _playerModelRenderer;
    private Coroutine _boostCoroutine;
    private Coroutine _damageCoroutine;

    #endregion

    #region Constant
    #endregion

    #region Event
    private Subject<Unit> _damageSubject = new Subject<Unit>();
    #endregion

    #region unity methods
    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
        _status = new PlayerStatus(_maxHP);

        _playerModelRenderer = _playerModelTrans.gameObject.GetComponent<Renderer>();
        _currentMoveSpeed = _moveSpeed;
    }

    private void Start()
    {
        GameManager.Instance.IsInGameObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(value =>
                            {
                                _isCanOperate = value;
                            });

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
    public void OnBoost(float boostAmount, float boostTime)
    {
        if (_boostCoroutine != null)
        {
            StopCoroutine(_boostCoroutine);
            _boostCoroutine = null;
            _currentMoveSpeed = _moveSpeed;
        }

        _boostCoroutine = StartCoroutine(BoostCoroutine(boostAmount, boostTime));
    }

    public void Damage()
    {
        if (_isDamaged)
        {
            return;
        }
        _damageCoroutine = StartCoroutine(DamageCoroutine());
        AudioManager.PlaySE(SEType.Damage_Player);
    }

    public void ChangeIsCanOperation(bool value)
    {
        _isCanOperate = value;
    }
    #endregion

    #region private method
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
    }
    private void SetDirection(Vector2 dir)
    {
        _inputAxis = dir;
    }
    private void OnRotate(InputAction.CallbackContext obj)
    {
        var value = obj.ReadValue<Vector2>();
        value.y = 0;
        SetDirection(value);
    }
    private void SetRotateInput()
    {
        if (_inputAxis != Vector2.zero && _rb.velocity.magnitude != 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_inputAxis);
            _playerModelTrans.rotation = Quaternion.Slerp(_playerModelTrans.rotation, targetRotation, Time.deltaTime * _rotateSpeed);
        }
    }
    private void OnResetInput(InputAction.CallbackContext obj)
    {
        SetDirection(Vector2.zero);
    }
    private void DebugInput()
    {
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
    }
    #endregion

    #region coroutine method
    private IEnumerator BoostCoroutine(float boostAmount, float boostTime)
    {
        var originSpeed = _currentMoveSpeed;
        var moveSpeed = _currentMoveSpeed + boostAmount;

        DOTween.To(() =>
               _currentMoveSpeed,
               x => _currentMoveSpeed = x,
               moveSpeed,
               0.5f)
               .WaitForCompletion();

        yield return new WaitForSeconds(boostTime);

        DOTween.To(() =>
               _currentMoveSpeed,
               x => _currentMoveSpeed = x,
               originSpeed,
               0.5f)
               .WaitForCompletion();
    }

    private IEnumerator DamageCoroutine()
    {
        _isDamaged = true;
        _rb.velocity = Vector2.zero;
        _currentMoveSpeed = _moveSpeed;
        _damageSubject.OnNext(Unit.Default);

        yield return _playerModelTrans.DOLocalRotate(new Vector3(0f, _playerModelTrans.localRotation.y + 720f, 0f),
                                                    1.0f, RotateMode.FastBeyond360)
                                                    .SetEase(Ease.Linear)
                                                    .WaitForCompletion();
        Debug.Log("ダメージ後無敵時間");
        _isDamaged = false;
        _isInvincibled = true;
        _status.ChangeTransparency(this, 0.5f);

        yield return new WaitForSeconds(_afterDamageInvincibleTime);

        _status.ChangeTransparency(this, 1f);
        _isInvincibled = false;
    }
    #endregion
}
