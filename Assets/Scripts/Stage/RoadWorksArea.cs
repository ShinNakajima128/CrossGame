using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// 工事中の道路の機能を持つコンポーネント
/// </summary>
public class RoadWorksArea : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [SerializeField]
    private float _slowTime = 3.0f;
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
        this.OnTriggerEnterAsObservable()
            .TakeUntilDestroy(this)
            .Where(x => x.gameObject.CompareTag(GameTag.Player))
            .Subscribe(x =>
            {
                var player = x.gameObject.GetComponent<PlayerModel>();

                if (player != null)
                {
                    if (!player.IsInvincible)
                    {
                        player.OnSlow(_slowTime);
                    }
                }
            });
    }
    #endregion

    #region public method
    #endregion

    #region private method
    #endregion
    
    #region coroutine method
    #endregion
}
