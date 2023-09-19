using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class StageManager : MonoBehaviour
{
    #region property
    public static StageManager Instance { get; private set; }
    #endregion

    #region serialize
    [SerializeField]
    private StageView _stageview = default;

    [SerializeField]
    private Transform _playerTrans = default;

    [SerializeField]
    private Transform _goalTrans = default;

    [SerializeField]
    private Obstacle[] _movingCars = default;

    [SerializeField]
    private Transform _carsParent = default;
    #endregion

    #region private
    private bool _isInGame = false;

    private Dictionary<int, ObjectPool<Obstacle>> _movingCarsPoolDic = new Dictionary<int, ObjectPool<Obstacle>>();
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {
        Instance = this;
     
        for (int i = 0; i < _movingCars.Length; i++)
        {
            _movingCarsPoolDic.Add(i, new ObjectPool<Obstacle>(_movingCars[i], _carsParent));
        }
    }

    private void Start()
    {
        GameManager.Instance.IsInGameObserver
                            .Subscribe(value =>
                            {
                                _isInGame = value;
                            });

        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Subscribe(_ =>
            {
                if (_isInGame)
                {
                    float distance = Vector3.Distance(_playerTrans.position, _goalTrans.position);
                    _stageview.DistanceView(distance);

                    if (distance <= 5f)
                    {
                        GameManager.Instance.OnGameEnd();
                        _stageview.ResetView();
                    }
                }
            });
    }
    #endregion

    #region public method
    public Obstacle RentRandomMovingCar()
    {
        int randomCarIndex = Random.Range(0, _movingCars.Length);
        return _movingCarsPoolDic[randomCarIndex].Rent();
    }
    #endregion

    #region private method
    #endregion

    #region coroutine method
    #endregion
}
