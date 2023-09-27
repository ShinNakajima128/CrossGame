using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// ヒッチハイカーが乗車するエリアの機能を持つコンポーネント
/// </summary>
public class HitchhikerBoardingArea : MonoBehaviour, IPoolable
{
    #region property
    public IObservable<Unit> InactiveObserver => _inactiveSubject;
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
                if (HitchhikerManager.Instance.IsCanRiding())
                {
                    GameManager.Instance.OnAddHitchhiker(_currentHicthhiker.HitchhikerType);
                    gameObject.SetActive(false);
                }
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
        StartCoroutine(GenerateCoroutine());
    }
    #endregion

    #region coroutine method
    private IEnumerator GenerateCoroutine()
    {
        yield return null;
        HitchhikerType type = (HitchhikerType)UnityEngine.Random.Range(0, (int)HitchhikerType.MAX_NUM);

        _currentHicthhiker = HitchhikerManager.Instance.RentHitchhiker(type);
        _currentHicthhiker.transform.position = transform.position;
        _currentHicthhiker.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        _currentHicthhiker.ChangeState(HitchhikerState.Idle);
    }
    #endregion
}
