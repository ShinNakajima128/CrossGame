using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerModel : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [SerializeField]
    private float _moveSpeed = 5.0f;

    [SerializeField]
    private float _rotateSpeed = 5.0f;

    [SerializeField]
    private float _maxSpeed = 20f;

    [SerializeField]
    private Transform _playerModelTrans = default;
    #endregion

    #region private
    private PlayerInput _input;
    private Rigidbody _rb;

    private Vector2 _inputAxis;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Subscribe(_ =>
            {
                OnRotate();
            });
        this.FixedUpdateAsObservable()
            .TakeUntilDestroy(this)
            .Subscribe(_ =>
            {
                ApplyMoving();
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
    #endregion

    #region private method
    private void SetDirection(Vector2 dir)
    {
        _inputAxis = dir;
    }

    //private void SetThrottle(InputAction.CallbackContext obj)
    //{
    //    var value = obj.ReadValue<bool>();
    //    _isThrottle = value;
    //}

    private void ApplyMoving()
    {
        if (_input.actions["Throttle"].IsPressed())
        {
            _rb.velocity = _playerModelTrans.forward * _moveSpeed;
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }
    }

    private void OnRotate()
    {
        if (_inputAxis != Vector2.zero && _rb.velocity.magnitude != 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_inputAxis);
            _playerModelTrans.rotation = Quaternion.Slerp(_playerModelTrans.rotation, targetRotation, Time.deltaTime * _rotateSpeed);

            //_playerModelTrans.Rotate(new Vector3(0f, _inputAxis.x * _rotateSpeed, 0f));
        }
    }
    
    private void OnRotate(InputAction.CallbackContext obj)
    {
        var value = obj.ReadValue<Vector2>();
        value.y = 0;
        SetDirection(value);
    }
    private void OnResetInput(InputAction.CallbackContext obj)
    {
        SetDirection(Vector2.zero);
    }
    #endregion

    #region coroutine method
    #endregion
}
