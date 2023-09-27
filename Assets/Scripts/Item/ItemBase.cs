using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

public abstract class ItemBase : MonoBehaviour, IPoolable
{
    #region property
    public IObservable<Unit> InactiveObserver => _inactiveSubject;
    #endregion

    #region serialize
    [SerializeField]
    private float _rotateSpeed = 5.0f;

    [SerializeField]
    private Transform _itemObjectTrans = default;
    #endregion

    #region private
    #endregion

    #region Constant
    #endregion

    #region Event
    private Subject<Unit> _inactiveSubject = new Subject<Unit>();
    #endregion

    #region unity methods
    protected virtual void Start()
    {
        this.OnTriggerEnterAsObservable()
            .TakeUntilDestroy(this)
            .Where(x => x.gameObject.CompareTag(GameTag.Player))
            .Subscribe(x =>
            {
                var player = x.gameObject.GetComponent<PlayerModel>();

                if (player != null && player.Status.CurrentState != PlayerState.Slowing)
                {
                    Use(player);
                    gameObject.SetActive(false);
                }
            });
    }
    protected virtual void OnEnable()
    {
        OnItemObjectRotation();
    }

    private void OnDisable()
    {
        
    }
    #endregion

    #region public method
    /// <summary>
    /// 使用する
    /// </summary>
    public abstract void Use(PlayerModel model);
    /// <summary>
    /// プールに戻る
    /// </summary>
    public void ReturnPool()
    {
        _inactiveSubject.OnNext(Unit.Default);
    }
    #endregion

    #region private method
    private void OnItemObjectRotation()
    {
        _itemObjectTrans.DOBlendableLocalRotateBy(new Vector3(0, 360, 0), 2.0f, RotateMode.FastBeyond360)
                        .SetEase(Ease.Linear)
                        .SetLoops(-1)
                        .SetLink(gameObject, LinkBehaviour.CompleteOnDisable);
    }
    #endregion

    #region coroutine method
    #endregion
}
