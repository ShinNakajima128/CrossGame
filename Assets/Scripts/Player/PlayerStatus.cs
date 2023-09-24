using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public partial class PlayerModel : MonoBehaviour
{
    public class PlayerStatus
    {
        #region property
        public IObservable<int> CurrentComboObserver => _currentComboAmountRP;
        public PlayerState CurrentState => _currentState;
        public int CurrentComboAmount => _currentComboAmountRP.Value;
        #endregion

        #region private
        private PlayerState _currentState;
        private int _currentHitchhikerAmount = 0;
        #endregion

        #region Constant
        #endregion

        #region Event
        private ReactiveProperty<int> _currentComboAmountRP = new ReactiveProperty<int>();
        #endregion

        #region public method
        public PlayerStatus()
        {
            _currentState = PlayerState.Normal;
        }
        /// <summary>
        /// コンボ数を増加する
        /// </summary>
        public void AddComboNum()
        {
            _currentComboAmountRP.Value++;
        }

        public void ChangeState(PlayerModel model, PlayerState newState)
        {
            if (_currentState == newState)
            {
                return;
            }

            //一度デフォルトに戻す際の処理を行う。（例：無敵状態が終了した場合は無敵フラグをOFFにするなど）
            switch (_currentState)
            {
                case PlayerState.Normal:
                    break;
                case PlayerState.Slowing:
                    model._currentMoveSpeed = model._moveSpeed;
                    break;
                case PlayerState.Infiltrator:
                    model.gameObject.layer = 6;
                    ChangeTransparency(model, 1f);
                    break;
                case PlayerState.Invincible:
                    break;
                default:
                    break;
            }

            //新しいステータスの処理を行う
            switch (newState)
            {
                case PlayerState.Normal:
                    model._playerModelRenderer.material.SetColor("_AlbedoColor", Color.white);
                    break;
                case PlayerState.Slowing:
                    model._currentMoveSpeed /= 2;
                    model._playerModelRenderer.material.SetColor("_AlbedoColor", model._slowStateColor);
                    break;
                case PlayerState.Infiltrator:
                    model.gameObject.layer = 13; //Layerの「13」に割り当てている透過状態に変更する
                    ChangeTransparency(model, 0.3f);
                    break;
                case PlayerState.Invincible:
                    break;
                default:
                    break;
            }
            _currentState = newState;
        }

        public void ChangeTransparency(PlayerModel model, float amount)
        {
            model._playerModelRenderer.material.SetFloat("_Opacity", amount);
        }

        public void ChangeHitchhikerNum(PlayerModel model, int amount)
        {
            _currentHitchhikerAmount += amount;
            model._currentMoveSpeed += model._moveSpeed * 1.1f;
        }

        public void ResetStatus()
        {
            _currentHitchhikerAmount = 0;
        }
        public void ResetCombo()
        {
            _currentComboAmountRP.Value = 0;
        }
        #endregion

        #region private method
        #endregion

        #region coroutine method
        #endregion
    }
}