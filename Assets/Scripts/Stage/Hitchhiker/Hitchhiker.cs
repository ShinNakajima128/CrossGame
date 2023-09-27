using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Hitchhiker : MonoBehaviour, IPoolable
{
    #region property
    public HitchhikerType HitchhikerType => _hitchHikerType;

    public System.IObservable<Unit> InactiveObserver => _inactiveSubject;
    #endregion

    #region serialize
    [SerializeField]
    private HitchhikerType _hitchHikerType = default;

    [SerializeField]
    private bool _debugMode = false;

    [SerializeField]
    private HitchhikerState _debugState = default;
    #endregion

    #region private
    private Animator _anim;
    private HitchhikerState _currentState;
    #endregion

    #region Constant
    #endregion

    #region Event
    private Subject<Unit> _inactiveSubject = new Subject<Unit>();
    #endregion

    #region unity methods
    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }
    private void Start()
    {
        if (_debugMode)
        {
            ChangeState(_debugState);
        }
        else
        {
            ChangeState(HitchhikerState.Idle);
        }
    }

    private void OnDisable()
    {
        ReturnPool();
    }
    #endregion

    #region public method
    public void ChangeState(HitchhikerState newState)
    {
        _currentState = newState;

        switch (_currentState)
        {
            case HitchhikerState.Idle:
                _anim.Play("Idle");
                break;
            case HitchhikerState.Dancing:
                int random = Random.Range(0, 5);
                _anim.Play($"Dancing{random + 1}");
                OnRideSE(HitchhikerType);
                break;
            case HitchhikerState.BlowOff:
                break;
            default:
                break;
        }
    }

    public void ReturnPool()
    {
        _inactiveSubject.OnNext(Unit.Default);
    }
    #endregion

    #region private method
    private void OnRideSE(HitchhikerType type)
    {
        switch (type)
        {
            case HitchhikerType.Female_G:
                AudioManager.PlaySE(SEType.HitchhikersRide_Type1);
                break;
            case HitchhikerType.Elder_Female_G:
                AudioManager.PlaySE(SEType.HitchhikersRide_Type5);
                break;
            case HitchhikerType.Male_G:
                AudioManager.PlaySE(SEType.HitchhikersRide_Type2);
                break;
            case HitchhikerType.Male_K:
                AudioManager.PlaySE(SEType.HitchhikersRide_Type4);
                break;
            case HitchhikerType.Little_Boy:
                AudioManager.PlaySE(SEType.HitchhikersRide_Type3);
                break;
            default:
                break;
        }
    }
    #endregion

    #region coroutine method
    #endregion
}
