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

    [SerializeField]
    private TextMeshProUGUI _currentHitchhikerAmountTMP = default;

    [SerializeField]
    private TextMeshProUGUI _finishGameTMP = default;
    #endregion

    #region public method
    public void DistanceView(float currentDistance)
    {
        _distanceTMP.text = $"�S�[���܂�\n{(currentDistance - 10).ToString("F0")}m";
    }

    public void HitchhikerAmountView(int amount, int maxAmount)
    {
        _currentHitchhikerAmountTMP.text = $"�q�b�`�n�C�J�[ {amount}/{maxAmount}";
    }

    public void FinishView()
    {
        _finishGameTMP.text = "GOAL!!";
        _distanceTMP.text = "";
    }
    public void ResetView()
    {
        _finishGameTMP.text = "";
    }
    #endregion
}
