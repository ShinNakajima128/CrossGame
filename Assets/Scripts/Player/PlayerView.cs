using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;

/// <summary>
/// �v���C���[�̏���UI�ɕ\������@�\�����R���|�[�l���g
/// </summary>
public class PlayerView : MonoBehaviour
{
    #region serialize
    [SerializeField]
    private TextMeshProUGUI _scoreTMP = default;

    [SerializeField]
    private TextMeshProUGUI _comboTMP = default;
    #endregion

    #region public method
    /// <summary>
    /// �X�R�A���X�V����
    /// </summary>
    /// <param name="score">���݃X�R�A�̒l</param>
    public void UpdateScoreView(float score)
    {
        _scoreTMP.text = $"Score : {score}";
    }

    /// <summary>
    /// �R���{�����X�V����
    /// </summary>
    /// <param name="combo">���݂̃R���{��</param>
    public void UpdateComboView(int combo)
    {
        if (combo > 0)
        {
            _comboTMP.text = $"{combo}Combo!!";
        }
        else
        {
            _comboTMP.text = "";
        }
    }
    #endregion
}
