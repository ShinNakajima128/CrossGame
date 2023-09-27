using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

/// <summary>
/// ステージ上にヒッチハイカーを生成する機能を持つコンポーネント
/// </summary>
public class HitchhikerBoardingGenerator : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [SerializeField]
    private int _maxGenerateNum = 3;

    [SerializeField]
    private Transform[] _generatePoints = default;
    #endregion

    #region private
    private List<int> _alreadyGenerateIndexList = new List<int>();
    private bool _init = false;
    #endregion

    #region Event
    private Subject<Unit> _resetSubject = new Subject<Unit>();
    #endregion

    #region unity methods
    private void Start()
    {
        Initialize();

        GameManager.Instance.GameResetObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(_ => Initialize());
    }
    #endregion

    #region public method
    #endregion

    #region private method
    private void Initialize()
    {
        //初期化処理を既に行っている場合
        if (_init)
        {
            _resetSubject.OnNext(Unit.Default);
            _alreadyGenerateIndexList.Clear();
        }

        int generateNum = Random.Range(1, _maxGenerateNum);

        for (int i = 0; i < generateNum; i++)
        {
            int randomIndex = Random.Range(0, _generatePoints.Length);

            //同じ生成位置にならないように判定を行う
            if (_alreadyGenerateIndexList.Any(x => x == randomIndex))
            {
                i--;
                continue;
            }

            var area = HitchhikerManager.Instance.RentBoardingArea();

            area.transform.position = _generatePoints[randomIndex].position;
            _alreadyGenerateIndexList.Add(randomIndex);
        }
        _init = true;
    }
    #endregion

    #region coroutine method
    #endregion
}
