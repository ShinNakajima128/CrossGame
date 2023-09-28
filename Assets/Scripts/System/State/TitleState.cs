using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public partial class GameStateMachine : MonoBehaviour
{
    /// <summary>
    /// タイトル画面での振る舞いを行うStateクラス
    /// </summary>
    public class TitleState : StateBase
    {
        #region public method
        public override void OnEnter()
        {
            AudioManager.PlayBGM(BGMType.Title);
        }
        public override void OnUpdate()
        {
            if (Input.anyKeyDown)
            {
                GameManager.Instance.OnGameStart();
            }
        }
        public override void OnExit()
        {
        }
        #endregion
    }
}
