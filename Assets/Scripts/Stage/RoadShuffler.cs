using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class RoadShuffler : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [SerializeField]
    private Transform[] _roads = default;
    #endregion

    #region private
    #endregion

    #region Constant
    #endregion

    #region Event
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
    private void Initialize()
    {
        int randomIndex = Random.Range(1, _roads.Length);

        var pos = _roads[0].position;
        _roads[0].position = _roads[randomIndex].position;
        _roads[randomIndex].position = pos;

        int randomType = Random.Range(0, 2);

        if (randomType == 0)
        {
            _roads[0].eulerAngles = new Vector3(0f, 180f, 0f);
        }
        else
        {
            _roads[0].eulerAngles = new Vector3(0f, 0f, 0f);
        }
    }
    #endregion
    
    #region coroutine method
    #endregion
}
