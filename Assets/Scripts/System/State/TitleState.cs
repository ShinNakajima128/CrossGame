using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public partial class GameStateMachine : MonoBehaviour
{
    /// <summary>
    /// �^�C�g����ʂł̐U�镑�����s��State�N���X
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
