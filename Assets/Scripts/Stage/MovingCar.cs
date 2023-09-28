using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// �ړ������Q��(��)�̋@�\�����R���|�[�l���g
/// </summary>
public class MovingCar : MonoBehaviour
{
    #region private
    /// <summary>�ړ����x</summary>
    private float _moveSpeed = 20.0f;
    /// <summary>���������̃R���|�[�l���g</summary>
    private Rigidbody _rb;
    /// <summary>�����蔻��</summary>
    private Collider _collider;
    #endregion

    #region const
    /// <summary>��A�N�e�B�u�ƂȂ�܂ł̎���</summary>
    private const float VANISH_TIME = 5.0f;
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
        //�ړ�������o�^
        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Subscribe(_ =>
            {
                _rb.velocity = transform.forward * _moveSpeed;
            });

        //�v���C���[�ƏՓ˂����ۂ̏�����o�^
        this.OnTriggerEnterAsObservable()
            .TakeUntilDestroy(this)
            .Where(x => x.gameObject.CompareTag(GameTag.Player))
            .Subscribe(x =>
            {
                var player = x.gameObject.GetComponent<IDamagable>();

                if (player != null)
                {
                    //�v���C���[�����G��Ԃł͂Ȃ��ꍇ
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

    #region coroutine method
    /// <summary>
    /// ��莞�Ԍo�ߌ�A��A�N�e�B�u�Ƃ���Coroutine
    /// </summary>
    /// <returns></returns>
    private IEnumerator VanishCoroutine()
    {
        yield return new WaitForSeconds(VANISH_TIME);

        gameObject.SetActive(false);
    }
    #endregion
}