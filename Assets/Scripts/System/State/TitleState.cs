using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public partial class GameStateMachine : MonoBehaviour
{
    public class TitleState : StateBase
    {
        #region public method
        public override void OnEnter()
        {
            GameManager.Instance.ChangeGameState(GameState.Title);
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
