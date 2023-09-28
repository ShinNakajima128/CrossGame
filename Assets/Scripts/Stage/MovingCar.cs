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
    #region private
    /// <summary>移動速度</summary>
    private float _moveSpeed = 20.0f;
    /// <summary>物理挙動のコンポーネント</summary>
    private Rigidbody _rb;
    /// <summary>当たり判定</summary>
    private Collider _collider;
    #endregion

    #region const
    /// <summary>非アクティブとなるまでの時間</summary>
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
        //移動処理を登録
        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Subscribe(_ =>
            {
                _rb.velocity = transform.forward * _moveSpeed;
            });

        //プレイヤーと衝突した際の処理を登録
        this.OnTriggerEnterAsObservable()
            .TakeUntilDestroy(this)
            .Where(x => x.gameObject.CompareTag(GameTag.Player))
            .Subscribe(x =>
            {
                var player = x.gameObject.GetComponent<IDamagable>();

                if (player != null)
                {
                    //プレイヤーが無敵状態ではない場合
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
    /// 一定時間経過後、非アクティブとするCoroutine
    /// </summary>
    /// <returns></returns>
    private IEnumerator VanishCoroutine()
    {
        yield return new WaitForSeconds(VANISH_TIME);

        gameObject.SetActive(false);
    }
    #endregion
}