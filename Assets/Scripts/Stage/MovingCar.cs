using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public partial class Obstacle : MonoBehaviour
{
    public class MovingCar : MonoBehaviour
    {
        #region property
        #endregion

        #region serialize
        [SerializeField]
        private float _moveSpeed = 10.0f;
        #endregion

        #region private
        private Rigidbody _rb;
        private Obstacle _obstacle;

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
            _obstacle = GetComponent<Obstacle>();

            _rb.useGravity = false;
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

            this.OnCollisionEnterAsObservable()
                .TakeUntilDestroy(this)
                .Where(x => x.gameObject.CompareTag(GameTag.Player))
                .Subscribe(x =>
                {
                    var player = x.gameObject.GetComponent<IDamagable>();
                    player?.Damage();

                    //_isCrashed = true;
                    //_obstacle.OnVanish();
                });
        }
        #endregion

        #region public method
        #endregion

        #region private method
        #endregion

        #region coroutine method
        #endregion
    }
}