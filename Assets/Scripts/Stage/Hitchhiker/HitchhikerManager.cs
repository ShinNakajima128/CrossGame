using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// ヒッチハイカー関連の処理を管理するManager
/// </summary>
public class HitchhikerManager : MonoBehaviour
{
    #region property
    public static HitchhikerManager Instance { get; private set; }
    public int CurrentHitchhikerAmount => _onHitchhikersList.Count;
    public int MaxHitchhikerNum => MAX_HITCHHIKER_NUM;
    public IObservable<int> CurrentHitchhikerAmountObserver => _currentHitchhikerAmountRP;
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
    private HitchhikerBoardingArea _bordingArea = default;

    [SerializeField]
    private Transform _hitchhikersParent = default;

    [SerializeField]
    private Transform _ragdollParent = default;

    [SerializeField]
    private Transform _areaParent = default;
    #endregion

    #region private
    private Rigidbody _rb_explosionPoint;

    private List<Hitchhiker> _onHitchhikersList = new List<Hitchhiker>();

    private Dictionary<HitchhikerType, ObjectPool<Hitchhiker>> _hitchhikersPoolDic = new Dictionary<HitchhikerType, ObjectPool<Hitchhiker>>();
    private Dictionary<HitchhikerType, ObjectPool<Ragdoll>> _ragdollsPoolDic = new Dictionary<HitchhikerType, ObjectPool<Ragdoll>>();

    private ObjectPool<HitchhikerBoardingArea> _boadingAreaPool;
    #endregion

    #region Constant
    private const int MAX_HITCHHIKER_NUM = 6;
    #endregion

    #region Event
    private ReactiveProperty<int> _currentHitchhikerAmountRP = new ReactiveProperty<int>(0);
    #endregion

    #region unity methods
    private void Awake()
    {
        Instance = this;
        _rb_explosionPoint = _explosionPoint.GetComponent<Rigidbody>();

        Initialize();
    }

    private void Start()
    {
        GameManager.Instance.PlayerDamageObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(_ => OnBlowoff());

        GameManager.Instance.AddHitchhikerObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(type => AddHitchhiker(type));

        GameManager.Instance.GameResetObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(_ => ResetHitchhiker());
    }
    #endregion

    #region public method
    /// <summary>
    /// ヒッチハイカーを使用する
    /// </summary>
    /// <param name="type">ヒッチハイカーの種類</param>
    /// <returns>ヒッチハイカー</returns>
    public Hitchhiker RentHitchhiker(HitchhikerType type)
    {
        var hitchhiker = _hitchhikersPoolDic[type].Rent();

        return hitchhiker;
    }
    /// <summary>
    /// ヒッチハイカーが待機するエリアを使用する
    /// </summary>
    /// <returns>エリア</returns>
    public HitchhikerBoardingArea RentBoardingArea()
    {
        return _boadingAreaPool.Rent();
    }
    /// <summary>
    /// ヒッチハイカーを乗車させる
    /// </summary>
    /// <param name="type">ヒッチハイカーの種類</param>
    public void AddHitchhiker(HitchhikerType type)
    {
        if (_onHitchhikersList.Count >= MAX_HITCHHIKER_NUM)
        {
            return;
        }

        var hitchhiker = RentHitchhiker(type);
        hitchhiker.transform.position = _generateHitchhikerPoint[_onHitchhikersList.Count].position;
        hitchhiker.ChangeState(HitchhikerState.Dancing);
        hitchhiker.gameObject.transform.SetParent(_generateHitchhikerPoint[_onHitchhikersList.Count]);
        _onHitchhikersList.Add(hitchhiker);
        _currentHitchhikerAmountRP.Value++;
    }

    /// <summary>
    /// 乗車可能か判定する
    /// </summary>
    /// <returns>乗車可能かどうか</returns>
    public bool IsCanRiding()
    {
        return CurrentHitchhikerAmount < MaxHitchhikerNum;
    }
    #endregion

    #region private method
    private void OnBlowoff()
    {
        //乗車しているヒッチハイカーがいない場合は処理を終了
        if (_onHitchhikersList.Count < 0)
        {
            return;
        }

        for (int i = 0; i < _onHitchhikersList.Count; i++)
        {
            //乗車中のヒッチハイカーの種類を調べ、そのラグドールを生成する
            var type = _onHitchhikersList[i].HitchhikerType;
            var ragdoll = _ragdollsPoolDic[type].Rent();
            switch (type)
            {
                case HitchhikerType.Female_G:
                    AudioManager.PlaySE(SEType.Scream_Type1);
                    break;
                case HitchhikerType.Elder_Female_G:
                    AudioManager.PlaySE(SEType.Scream_Type5);
                    break;
                case HitchhikerType.Male_G:
                    AudioManager.PlaySE(SEType.Scream_Type2);
                    break;
                case HitchhikerType.Male_K:
                    AudioManager.PlaySE(SEType.Scream_Type4);
                    break;
                case HitchhikerType.Little_Boy:
                    AudioManager.PlaySE(SEType.Scream_Type3);
                    break;
                default:
                    break;
            }
            ragdoll.transform.position = _generateHitchhikerPoint[i].position;

            //乗車用のヒッチハイカーのオブジェクトをプールに戻す
            _onHitchhikersList[i].gameObject.SetActive(false);
            _onHitchhikersList[i].gameObject.transform.SetParent(_hitchhikersParent);
        }
        _onHitchhikersList.Clear();
        _currentHitchhikerAmountRP.Value = 0;
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

        _boadingAreaPool = new ObjectPool<HitchhikerBoardingArea>(_bordingArea, _areaParent);
    }

    private void ResetHitchhiker()
    {
        if (_onHitchhikersList.Count > 0)
        {
            for (int i = 0; i < _onHitchhikersList.Count; i++)
            {
                //乗車用のヒッチハイカーのオブジェクトをプールに戻す
                _onHitchhikersList[i].gameObject.SetActive(false);
                _onHitchhikersList[i].gameObject.transform.SetParent(_hitchhikersParent);
            }
            _onHitchhikersList.Clear();
            _currentHitchhikerAmountRP.Value = 0;
        }
    }
    #endregion

    #region coroutine method
    #endregion
}
