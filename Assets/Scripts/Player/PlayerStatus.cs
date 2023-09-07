using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public partial class PlayerModel : MonoBehaviour
{
    public class PlayerStatus
    {
        #region property
        #endregion

        #region private
        private int _currentHP;
        private PlayerState _currentState;
        #endregion

        #region Constant
        #endregion

        #region Event
        #endregion

        #region public method
        public PlayerStatus(int maxHP)
        {
            _currentHP = maxHP;
            _currentState = PlayerState.Normal;
        }
        public void Damage(int damageAmount)
        {
            //すり抜け状態または無敵状態ではない場合はダメージを受ける
            if (_currentState != PlayerState.Infiltrator ||
                _currentState != PlayerState.Invincible)
            {
                _currentHP -= damageAmount;

                if (_currentHP <= 0)
                {
                    GameManager.Instance.OnGameEnd();
                }
            }
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
                    model._playerModelRenderer.material.SetFloat("DitherLevel", 0);
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
                    model._playerModelRenderer.material.color = Color.white;
                    break;
                case PlayerState.Slowing:
                    model._currentMoveSpeed /= 2;
                    model._playerModelRenderer.material.color = model._slowStateColor;
                    break;
                case PlayerState.Infiltrator:
                    model.gameObject.layer = 13; //Layerの「13」に割り当てている透過状態に変更する
                    model._playerModelRenderer.material.SetFloat("DitherLevel", 8);
                    break;
                case PlayerState.Invincible:
                    break;
                default:
                    break;
            }
        }
        public void ResetStatus(int maxHP)
        {
            _currentHP = maxHP;
        }
        #endregion

        #region private method
        #endregion

        #region coroutine method
        #endregion
    }
}