using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class HitchhikerManager : MonoBehaviour
{
    #region property
    public static HitchhikerManager Instance { get; private set; }
    #endregion

    #region serialize
    [SerializeField]
    private float _blowoffForceAmount = 10f;

    [SerializeField]
    private float _blowoffRadius = 3.0f;

    [SerializeField]
    private Transform[] _generateHitchHikerPoint = default;

    [SerializeField]
    private Transform _explosionPoint = default;

    [SerializeField]
    private Hitchhiker[] _hitchhikers = default;

    [SerializeField]
    private GameObject _ragdoll = default;
    #endregion

    #region private
    private Rigidbody _rb_explosionPoint;

    private List<Hitchhiker> _hitchHikers = new List<Hitchhiker>();
    private Dictionary<HitchHikerType, ObjectPool<Hitchhiker>> _hitchhikersPoolDic = new Dictionary<HitchHikerType, ObjectPool<Hitchhiker>>();
    #endregion

    #region Constant
    private const int MAX_HITCHHIKER_NUM = 6;
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {
        Instance = this;
        _rb_explosionPoint = _explosionPoint.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        GameManager.Instance.PlayerDamageObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(_ => OnBlowoff());
    }
    #endregion

    #region public method
    #endregion

    #region private method
    private void AddHitchhiker(HitchHikerType type)
    {
        switch (type)
        {
            case HitchHikerType.Female_G:
                break;
            default:
                break;
        }
    }

    private void OnBlowoff()
    {
        for (int i = 0; i < _generateHitchHikerPoint.Length; i++)
        {
            var ragdoll = Instantiate(_ragdoll, _generateHitchHikerPoint[i].position, _generateHitchHikerPoint[i].rotation);
            Destroy(ragdoll, 5);
        }
        _rb_explosionPoint.AddExplosionForce(_blowoffForceAmount, _explosionPoint.position, _blowoffRadius);
    }
    #endregion
    
    #region coroutine method
    #endregion
}
