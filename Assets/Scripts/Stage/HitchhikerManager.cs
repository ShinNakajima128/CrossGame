using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class HitchhikerManager : MonoBehaviour
{
    #region property
    public static HitchhikerManager Instance { get; private set; }
    public int CurrentHitchhikerAmount => _onHitchhikersList.Count;
    #endregion

    #region serialize
    [SerializeField]
    private float _blowoffForceAmount = 10f;

    [SerializeField]
    private float _blowoffRadius = 3.0f;

    [SerializeField]
    private Transform[] _generateHitchhikerPoint = default;

    [SerializeField]
    private Transform _explosionPoint = default;

    [SerializeField]
    private Hitchhiker[] _hitchhikers = default;

    [SerializeField]
    private Ragdoll[] _ragdolls = default;

    [SerializeField]
    private Transform _hitchhikersParent = default;

    [SerializeField]
    private Transform _ragdollParent = default;
    #endregion

    #region private
    private Rigidbody _rb_explosionPoint;

    private List<Hitchhiker> _onHitchhikersList = new List<Hitchhiker>();
    private Dictionary<HitchhikerType, ObjectPool<Hitchhiker>> _hitchhikersPoolDic = new Dictionary<HitchhikerType, ObjectPool<Hitchhiker>>();
    private Dictionary<HitchhikerType, ObjectPool<Ragdoll>> _ragdollsPoolDic = new Dictionary<HitchhikerType, ObjectPool<Ragdoll>>();
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

        Initialize();
    }
    #endregion

    #region public method
    public Hitchhiker RentHitchhiker(HitchhikerType type)
    {
        var hitchhiker = _hitchhikersPoolDic[type].Rent();

        return hitchhiker;
    }
    public void AddHitchhiker(HitchhikerType type)
    {
        var hitchhiker = RentHitchhiker(type);
        hitchhiker.transform.position = _generateHitchhikerPoint[_onHitchhikersList.Count].position;
        _onHitchhikersList.Add(hitchhiker);
    }
    #endregion

    #region private method

    private void OnBlowoff()
    {
        if (_onHitchhikersList.Count < 0)
        {
            return;
        }

        for (int i = 0; i < _onHitchhikersList.Count; i++)
        {
            var type = _onHitchhikersList[i].HitchhikerType;
            var ragdoll = _ragdollsPoolDic[type].Rent();
            ragdoll.transform.position = _generateHitchhikerPoint[i].position;
        }
        _rb_explosionPoint.AddExplosionForce(_blowoffForceAmount, _explosionPoint.position, _blowoffRadius);
    }

    private void Initialize()
    {
        for (int i = 0; i < _hitchhikers.Length; i++)
        {
            _hitchhikersPoolDic.Add((HitchhikerType)i, new ObjectPool<Hitchhiker>(_hitchhikers[i], _hitchhikersParent));
        }

        for (int i = 0; i < _ragdolls.Length; i++)
        {
            _ragdollsPoolDic.Add((HitchhikerType)i, new ObjectPool<Ragdoll>(_ragdolls[i], _ragdollParent));
        }
    }
    #endregion

    #region coroutine method
    #endregion
}
