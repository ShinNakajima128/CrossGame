using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ItemGenerator : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [SerializeField]
    private int _maxGenerateNum = 3;

    //[SerializeField]
    //private Transform[] _generatePoints = default;
    #endregion

    #region private
    private List<int> _alreadyGenerateIndexList = new List<int>();
    private bool _init = false;
    #endregion

    #region Constant
    private const int STAGE_DEPTH = 50;
    private const int STAGE_WIDTH = 4;
    #endregion

    #region Event
    private Subject<Unit> _resetSubject = new Subject<Unit>();
    #endregion

    #region unity methods
    private void Awake()
    {

    }

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
    /// <summary>
    /// ‰Šú‰»
    /// </summary>
    private void Initialize()
    {
        //‰Šú‰»ˆ—‚ğŠù‚És‚Á‚Ä‚¢‚éê‡
        if (_init)
        {
            _resetSubject.OnNext(Unit.Default);
            _alreadyGenerateIndexList.Clear();
        }

        int generateNum = Random.Range(0, _maxGenerateNum);

        for (int i = 0; i < generateNum; i++)
        {
            //int randomIndex = Random.Range(0, _generatePoints.Length);

            ////“¯‚¶¶¬ˆÊ’u‚É‚È‚ç‚È‚¢‚æ‚¤‚É”»’è‚ğs‚¤
            //if (_alreadyGenerateIndexList.Any(x => x == randomIndex))
            //{
            //    i--;
            //    continue;
            //}

            var item = ItemManager.Instance.RandomItemGenerate();
            int depth = Random.Range(-STAGE_DEPTH, STAGE_DEPTH);
            int width = Random.Range(-STAGE_WIDTH, STAGE_WIDTH);

            item.transform.position = new Vector3(transform.position.x + width, 1f, transform.position.z + depth);
            //_alreadyGenerateIndexList.Add(randomIndex);
        }
        _init = true;
    }
    #endregion
    
    #region coroutine method
    #endregion
}
