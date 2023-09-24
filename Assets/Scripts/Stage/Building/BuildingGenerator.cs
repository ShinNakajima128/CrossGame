using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class BuildingGenerator : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [SerializeField]
    private GameObject[] _buildings = default;

    [SerializeField]
    private Transform[] _northGeneratePoints = default;

    [SerializeField]
    private Transform[] _eastGeneratePoints = default;

    [SerializeField]
    private Transform[] _westGeneratePoints = default;

    [SerializeField]
    private Transform[] _southGeneratePoints = default;

    [SerializeField]
    private Transform _buildingParent = default;
    #endregion

    #region private
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Start()
    {
        OnRandomGenerate();
    }
    #endregion

    #region public method
    #endregion

    #region private method
    private void OnRandomGenerate()
    {
        for (int i = 0; i < _eastGeneratePoints.Length; i++)
        {
            var randomIndex = UnityEngine.Random.Range(0, _buildings.Length);

            var building = Instantiate(_buildings[randomIndex], _buildingParent);
            building.transform.position = _eastGeneratePoints[i].position;
            building.transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }

        for (int i = 0; i < _westGeneratePoints.Length; i++)
        {
            var randomIndex = UnityEngine.Random.Range(0, _buildings.Length);

            var building = Instantiate(_buildings[randomIndex], _buildingParent);
            building.transform.position = _westGeneratePoints[i].position;
        }
    }
    #endregion

    #region coroutine method
    #endregion
}
