using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// 道に出現するバリケードの機能を持つコンポーネント
/// </summary>
public class Barricade : MonoBehaviour
{
    #region serialize
    [SerializeField]
    private int _scoreAmount = 100;
    #endregion

    #region private
    private PlayerModel _player;
    #endregion

    #region unity methods
    private void Start()
    {
        this.OnTriggerExitAsObservable()
            .TakeUntilDestroy(this)
            .Where(x => x.gameObject.CompareTag(GameTag.Player))
            .Subscribe(x =>
            {
                GameManager.Instance.OnAddConbo();

                _player = _player ?? x.GetComponent<PlayerModel>();
                int score;
                int currentCombo = _player.Status.CurrentComboAmount;

                score = _scoreAmount * currentCombo;

                ScoreManager.Instance.AddScore(score);
                AudioManager.PlaySE(SEType.Result_TextView);
            });
    }
    #endregion
}
