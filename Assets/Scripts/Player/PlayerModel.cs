using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

/// <summary>
/// プレイヤーの動作の機能を持つコンポーネント
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
    [Header("各数値")]
    [SerializeField]
    private int _maxHP = 3;

    [Tooltip("移動速度")]
    [SerializeField]
    private float _moveSpeed = 5.0f;

    [Tooltip("回転速度")]
    [SerializeField]
    private float _rotateSpeed = 5.0f;

    [Tooltip("ダメージ後の無敵時間")]
    [SerializeField]
    private float _afterDamageInvincibleTime = 2.0f;

    [Tooltip("プレイヤーのオブジェクトモデルのTransform")]
    [SerializeField]
    private Transform _playerModelTrans = default;

    [Tooltip("加速エフェクトのオブジェクト")]
    [SerializeField]
    private GameObject _accelEffect = default;

    [Header("各ステータス時のモデルの色")]
    [SerializeField]
    private Color _slowStateColor = Color.black;
    #endregion

    #region private
    /// <summary>プレイヤーの入力情報</summary>
    private PlayerInput _input;
    /// <summary>物理挙動用のコンポーネント</summary>
    private Rigidbody _rb;
    /// <summary>プレイヤーの状態を持つコンポーネント</summary>
    private PlayerStatus _status;

    /// <summary>入力座標</summary>
    private Vector2 _inputAxis;
    /// <summary>現在の移動速度</summary>
    private float _currentMoveSpeed;
    /// <summary>元々の移動速度。加速アイテムの効果終了時などに活用</summary>
    private float _originSpeed;
    /// <summary>操作可能かどうか</summary>
    private bool _isCanOperate = false;
    /// <summary>ダメージ処理中かどうか</summary>
    private bool _isDamaged = false;
    /// <summary>無敵中かどうか</summary>
    private bool _isInvincibled = false;

    /// <summary>プレイヤーオブジェクトのRenderer</summary>
    private Renderer _playerModelRenderer;
    /// <summary>加速処理のCoroutine</summary>
    private Coroutine _boostCoroutine;
    /// <summary>すり抜け処理のCoroutine</summary>
    private Coroutine _InfiltratorCoroutine;
    /// <summary>ダメージ処理のCoroutine</summary>
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
        //プレイヤーの入力処理を登録
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
    /// 加速する
    /// </summary>
    /// <param name="boostAmount">加速する値</param>
    /// <param name="boostTime">加速している時間</param>
    public void OnBoost(float boostAmount, float boostTime)
    {
        BreakBoost();
       _boostCoroutine = StartCoroutine(BoostCoroutine(boostAmount, boostTime));
    }

    /// <summary>
    /// 加速を中断する
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
    /// 鈍足化する
    /// </summary>
    /// <param name="slowTime">鈍足になっている時間</param>
    public void OnSlow(float slowTime)
    {
        StartCoroutine(SlowCoroutine(slowTime));
    }

    /// <summary>
    /// 透明化する
    /// </summary>
    /// <param name="infiltratorTime">透過している時間</param>
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
    /// ダメージを受ける
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
    /// ヒッチハイカーを増やす
    /// </summary>
    public void AddHitchhiker()
    {
        _status.ChangeSpeedAccordingHitchhiker(this);
    }
    /// <summary>
    /// 障害物を通過したコンボ数を増やす
    /// </summary>
    public void AddCombo()
    {
        _status.AddComboNum();
    }
    /// <summary>
    /// コンボ数をリセットする
    /// </summary>
    public void ResetCombo()
    {
        _status.ResetCombo();
    }
    /// <summary>
    /// プレイヤー操作の可能/不可を切り替える
    /// </summary>
    /// <param name="value">可能/不可</param>
    public void ChangeIsCanOperation(bool value)
    {
        _isCanOperate = value;
    }
    /// <summary>
    /// プレイヤーの状態をリセットする
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
    /// プレイヤーの速度をリセットする
    /// </summary>
    public void ResetVelocity()
    {
        _rb.velocity = Vector3.zero;
        _accelEffect.SetActive(false);
    }
    #endregion

    #region private method
    /// <summary>
    /// 移動処理
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
        //Debug.Log($"現在の速度{_rb.velocity.magnitude}");
    }
    /// <summary>
    /// 入力方向をセットする
    /// </summary>
    /// <param name="dir">入力方向</param>
    private void SetDirection(Vector2 dir)
    {
        _inputAxis = dir;
    }
    /// <summary>
    /// 回転処理
    /// </summary>
    /// <param name="obj"></param>
    private void OnRotate(InputAction.CallbackContext obj)
    {
        var value = obj.ReadValue<Vector2>();
        value.y = 0;
        SetDirection(value);
    }
    /// <summary>
    /// 回転情報をセットする
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
    /// 入力情報をリセットする
    /// </summary>
    /// <param name="obj"></param>
    private void OnResetInput(InputAction.CallbackContext obj)
    {
        SetDirection(Vector2.zero);
    }
    /// <summary>
    /// デバッグ処理
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
    /// 加速処理のCoroutine
    /// </summary>
    /// <param name="boostAmount">加速量</param>
    /// <param name="boostTime">加速時間</param>
    /// <returns></returns>
    private IEnumerator BoostCoroutine(float boostAmount, float boostTime)
    {
        _originSpeed = _currentMoveSpeed;
        var moveSpeed = _currentMoveSpeed + boostAmount;
        
        _accelEffect.SetActive(true);

        //徐々に加速
        yield return DOTween.To(() =>
                            _currentMoveSpeed,
                            x => _currentMoveSpeed = x,
                            moveSpeed,
                            0.5f)
                            .WaitForCompletion();

        yield return new WaitForSeconds(boostTime);

        //徐々に減速
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
    /// すり抜け処理のCOroutine
    /// </summary>
    /// <param name="infiltratorTime">すり抜け可能時間</param>
    /// <returns></returns>
    private IEnumerator InfiltratorCoroutine(float infiltratorTime)
    {
        _status.ChangeState(this, PlayerState.Infiltrator);
        AudioManager.PlaySE(SEType.Infiltrator);

        yield return new WaitForSeconds(infiltratorTime - 2);

        //すり抜け状態終了2秒前になると点滅し、プレイヤーに終了間近を伝える処理を行う
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

        //プレイヤーモデルの透過処理の衝突があったため、1フレーム待機することで対処
        yield return null;

        //プレイヤーの状態を通常状態に戻す
        _status.ChangeState(this, PlayerState.Normal);
        AudioManager.PlaySE(SEType.BackToNormalState);
    }
    /// <summary>
    /// 速度低下処理のCoroutine
    /// </summary>
    /// <param name="slowTime">速度低下時間</param>
    /// <returns></returns>
    private IEnumerator SlowCoroutine(float slowTime)
    {
        _status.ChangeState(this, PlayerState.Slowing);
        AudioManager.PlaySE(SEType.Slow_Player);

        yield return new WaitForSeconds(slowTime);

        //プレイヤーの状態を通常状態に戻す
        _status.ChangeState(this, PlayerState.Normal);
    }
    /// <summary>
    /// ダメージ処理のCoroutine
    /// </summary>
    /// <returns></returns>
    private IEnumerator DamageCoroutine()
    {
        _isDamaged = true;
        _rb.velocity = Vector2.zero;
        _currentMoveSpeed = _moveSpeed;
        _damageSubject.OnNext(Unit.Default);

        //衝突によるスピンを表現
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
