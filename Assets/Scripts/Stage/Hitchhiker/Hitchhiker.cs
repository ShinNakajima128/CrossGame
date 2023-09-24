using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Hitchhiker : MonoBehaviour
{
    #region property
    public HitchhikerType HitchhikerType => _hitchHikerType;
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
    #endregion

    #region unity methods
    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }
    private void Start()
    {
        //yield return null;

        if (_debugMode)
        {
            ChangeState(_debugState);
        }
        else
        {
            ChangeState(HitchhikerState.Idle);
        }
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
                break;
            case HitchhikerState.BlowOff:
                break;
            default:
                break;
        }
    }
    #endregion

    #region private method
    #endregion
    
    #region coroutine method
    #endregion
}
