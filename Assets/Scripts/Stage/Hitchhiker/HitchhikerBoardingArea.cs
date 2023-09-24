using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// �q�b�`�n�C�J�[����Ԃ���G���A�̋@�\�����R���|�[�l���g
/// </summary>
public class HitchhikerBoardingArea : MonoBehaviour, IPoolable
{
    public IObservable<Unit> InactiveObserver => _inactiveSubject;


    #region property
    #endregion

    #region serialize
    [SerializeField]
    private Transform _generatePoint = default;
    #endregion

    #region private
    private Hitchhiker _currentHicthhiker;
    #endregion

    #region Constant
    #endregion

    #region Event
    private Subject<Unit> _inactiveSubject = new Subject<Unit>();
    #endregion

    #region unity methods
    private void Start()
    {
        this.OnTriggerEnterAsObservable()
            .TakeUntilDestroy(this)
            .Where(x => x.gameObject.CompareTag(GameTag.Player))
            .Subscribe(x =>
            {
                GameManager.Instance.OnAddHitchhiker(_currentHicthhiker.HitchhikerType);
            });
    }

    private void OnEnable()
    {
        Initialize();
    }
    private void OnDisable()
    {
        _currentHicthhiker.gameObject.SetActive(false);
        _currentHicthhiker = null;
        ReturnPool();
    }
    #endregion

    #region public method
    public void ReturnPool()
    {
        _inactiveSubject.OnNext(Unit.Default);
    }
    #endregion

    #region private method
    private void Initialize()
    {
        HitchhikerType type = (HitchhikerType)UnityEngine.Random.Range(0, (int)HitchhikerType.MAX_NUM);

        _currentHicthhiker = HitchhikerManager.Instance.RentHitchhiker(type);
        _currentHicthhiker.transform.position = _generatePoint.position;
        _currentHicthhiker.transform.rotation = _generatePoint.rotation;
    }
    #endregion

    #region coroutine method
    #endregion
}
