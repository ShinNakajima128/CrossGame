using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;

public class StageView : MonoBehaviour
{
    #region serialize
    [SerializeField]
    private TextMeshProUGUI _distanceTMP = default;
    #endregion

    #region public method
    public void DistanceView(float currentDistance)
    {
        _distanceTMP.text = $"ÉSÅ[ÉãÇ‹Ç≈\n{currentDistance.ToString("F0")}m";
    }
    public void ResetView()
    {
        _distanceTMP.text = "";
    }
    #endregion
}
