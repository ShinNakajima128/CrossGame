using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// 移動する障害物(車)の機能を持つコンポーネント
/// </summary>
public class MovingCar : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    #endregion

    #region private
    private float _moveSpeed = 20.0f;
    private Rigidbody _rb;
    private Collider _collider;

    private bool _isCrashed = false;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();

        _rb.useGravity = false;
        _collider.isTrigger = true;
    }

    private void Start()
    {
        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Where(_ => !_isCrashed)
            .Subscribe(_ =>
            {
                _rb.velocity = transform.forward * _moveSpeed;
            });

        this.OnTriggerEnterAsObservable()
            .TakeUntilDestroy(this)
            .Where(x => x.gameObject.CompareTag(GameTag.Player))
            .Subscribe(x =>
            {
                var player = x.gameObject.GetComponent<IDamagable>();

                if (player != null)
                {
                    if (!player.IsInvincible)
                    {
                        player?.Damage();
                        GameManager.Instance.OnPlayerDamage();
                        GameManager.Instance.OnResetCombo();
                        AudioManager.PlaySE(SEType.InGame_CarHorn);
                    }
                }
            });
    }

    private void OnEnable()
    {
        StartCoroutine(VanishCoroutine());
    }
    #endregion

    #region public method
    #endregion

    #region private method
    #endregion

    #region coroutine method
    private IEnumerator VanishCoroutine()
    {
        yield return new WaitForSeconds(5.0f);

        gameObject.SetActive(false);
    }
    #endregion
}