using UnityEngine;
using TMPro;

/// <summary>
/// プレイヤーの情報をUIに表示する機能を持つコンポーネント
/// </summary>
public class PlayerView : MonoBehaviour
{
    #region serialize
    [Tooltip("スコア表示のTMP")]
    [SerializeField]
    private TextMeshProUGUI _scoreTMP = default;

    [Tooltip("コンボ数表示のTMP")]
    [SerializeField]
    private TextMeshProUGUI _comboTMP = default;
    #endregion

    #region public method
    /// <summary>
    /// スコアを更新する
    /// </summary>
    /// <param name="score">現在スコアの値</param>
    public void UpdateScoreView(float score)
    {
        _scoreTMP.text = $"Score : {score}";
    }

    /// <summary>
    /// コンボ数を更新する
    /// </summary>
    /// <param name="combo">現在のコンボ数</param>
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
