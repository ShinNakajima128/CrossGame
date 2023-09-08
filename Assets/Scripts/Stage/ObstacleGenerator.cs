using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ObstacleGenerator : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [SerializeField]
    private GameObject[] _obstacleSets = default;

    [SerializeField]
    private Transform[] _generatePoints = default;

    [SerializeField]
    private Transform _obstacleParent = default;
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
        if (_generatePoints.Length > 0)
        {
            OnRandomGenerate();
        }
    }
    #endregion

    #region public method
    #endregion

    #region private method
    private void OnRandomGenerate()
    {
        for (int i = 0; i < _generatePoints.Length; i++)
        {
            int randomIndex = Random.Range(0, _obstacleSets.Length);

            var obj = Instantiate(_obstacleSets[randomIndex], _obstacleParent);
            obj.transform.position = _generatePoints[i].position;
        }
    }
    #endregion

    #region coroutine method
    #endregion
}
