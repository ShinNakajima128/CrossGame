using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public partial class GameStateMachine : MonoBehaviour
{
    public class TitleState : StateBase
    {
        #region private
        private bool _isStarted = false;
        #endregion

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
                // = true;
            }
        }
        public override void OnExit()
        {
            //_isStarted = false;
        }
        #endregion
    }
}
