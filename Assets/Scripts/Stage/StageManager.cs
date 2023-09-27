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
    private StageView _stageView = default;

    [SerializeField]
    private Transform _playerTrans = default;

    [SerializeField]
    private Transform _startTrans = default;

    [SerializeField]
    private Transform _goalTrans = default;

    [SerializeField]
    private Obstacle[] _movingCars = default;

    [SerializeField]
    private Transform _carsParent = default;
    #endregion

    #region private
    private Dictionary<int, ObjectPool<Obstacle>> _movingCarsPoolDic = new Dictionary<int, ObjectPool<Obstacle>>();

    private bool _isInGame = false;
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

        GameManager.Instance.GameResetObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(_ =>
                            {
                                ResetPlayerPosition();
                                _stageView.ResetView();
                            });

        HitchhikerManager.Instance.CurrentHitchhikerAmountObserver
                                  .TakeUntilDestroy(this)
                                  .Subscribe(value => OnHitchhikerAmountView(value, HitchhikerManager.Instance.MaxHitchhikerNum));

        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Where(_ => _isInGame)
            .Subscribe(_ =>
            {
                float distance = Vector3.Distance(_playerTrans.position, _goalTrans.position);
                _stageView.DistanceView(distance);

                if (distance <= 10f)
                {
                    GameManager.Instance.OnGameEnd();
                    _stageView.FinishView();
                    StartCoroutine(FinishGameCoroutine());
                    AudioManager.PlaySE(SEType.Goal);
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
    /// <summary>
    /// プレイヤーの位置をスタート地点に戻す
    /// </summary>
    private void ResetPlayerPosition()
    {
        _playerTrans.position = _startTrans.position;
    }
    /// <summary>
    /// 現在のヒッチハイカーの数を表示する
    /// </summary>
    /// <param name="amount">現在の人数</param>
    /// <param name="maxAmount">最大人数</param>
    private void OnHitchhikerAmountView(int amount, int maxAmount)
    {
        _stageView.HitchhikerAmountView(amount, maxAmount);
    }
    #endregion

    #region coroutine method
    /// <summary>
    /// ゲーム終了時のコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator FinishGameCoroutine()
    {
        yield return new WaitForSeconds(2.5f);

        FadeManager.Fade(FadeType.Out, () =>
        {
            GameManager.Instance.ChangeGameState(GameState.Result);
            FadeManager.Fade(FadeType.In);
        });
    }
    #endregion
}
