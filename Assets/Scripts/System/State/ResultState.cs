using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public partial class GameStateMachine : MonoBehaviour
{
    /// <summary>
    /// リザルト画面での振る舞いを行うStateクラス
    /// </summary>
    public class ResultState : StateBase
    {
        #region public method
        public override void OnEnter()
        {
            ResultManager.Instance.OnResult();
        }
        public override void OnUpdate()
        {
        }
        public override void OnExit()
        {
        }
        #endregion
    }
}