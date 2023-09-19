using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[CreateAssetMenu(menuName = "Myscriptable/ObstacleData")]
public class ObstacleData : ScriptableObject
{
    #region property
    public ObstacleType Type => _type;
    public int Score => _score;
    public bool IsImmortaled => _isImmortaled;
    #endregion

    #region serialize
    [SerializeField]
    private string _name = default;

    [SerializeField]
    private ObstacleType _type = default;

    [SerializeField]
    private int _score = 100;

    [SerializeField]
    private bool _isImmortaled = false;
    #endregion

    #region private
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    #endregion

    #region public method
    #endregion

    #region private method
    #endregion

    #region coroutine method
    #endregion
}