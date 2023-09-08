using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Obstacle : MonoBehaviour, IPoolable
{
    #region property
    public IObservable<Unit> InactiveObserver => throw new NotImplementedException();
    #endregion

    #region serialize
    [SerializeField]
    private ObstacleData _data = default;
    #endregion

    #region private
    private Renderer _obstacleRenderer;

    private bool _isVanished = false;
    #endregion

    #region Constant
    #endregion

    #region Event
    private Subject<Unit> _inactiveSubject = new Subject<Unit>();
    #endregion

    #region unity methods
    private void Awake()
    {
        _obstacleRenderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        this.OnCollisionEnterAsObservable()
            .TakeUntilDestroy(this)
            .Where(x => x.gameObject.CompareTag(GameTag.Player))
            .Subscribe(x =>
            {
                if (!_isVanished)
                {
                    OnVanish();
                    _isVanished = true;
                }
            });
    }
    #endregion

    #region public method
    public void ReturnPool()
    {
        _inactiveSubject.OnNext(Unit.Default);
    }
    #endregion

    #region private method
    private void OnVanish()
    {
        StartCoroutine(OnVanishCoroutine());
    }
    #endregion

    #region coroutine method
    private IEnumerator OnVanishCoroutine()
    {
        yield return new WaitForSeconds(1.5f);

        float ditherAmount = 1.0f;

        DOTween.To(() =>
                ditherAmount,
                x => ditherAmount = x,
                0f,
                1.5f)
               .OnUpdate(() =>
               {
                   _obstacleRenderer.material.SetFloat("_Opacity", ditherAmount);
               });
    }
    #endregion
}
