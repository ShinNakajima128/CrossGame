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
    private float rotateSpeed = 5.0f;
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
                ApplyInput();
            });

        this.FixedUpdateAsObservable()
            .TakeUntilDestroy(this)
            .Subscribe(_ =>
            {
                ApplyMoving();
            });
    }
    #endregion

    #region public method
    #endregion

    #region private method
    private void ApplyInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        _inputAxis = new Vector2(h, v);
    }

    private void ApplyMoving()
    {
        var dir = new Vector3(_inputAxis.x, 0f, _inputAxis.y);
        _rb.AddForce(dir.normalized * _moveSpeed, ForceMode.Force);
    }

    private void ApplyRotate()
    {

    }
    #endregion

    #region coroutine method
    #endregion
}
