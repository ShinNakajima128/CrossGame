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
        private int _currentHitchhikerAmount = 0;
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
            //���蔲����Ԃ܂��͖��G��Ԃł͂Ȃ��ꍇ�̓_���[�W���󂯂�
            if (_currentState != PlayerState.Infiltrator ||
                _currentState != PlayerState.Invincible)
            {
                _currentHP -= damageAmount;
                _currentHitchhikerAmount = 0;

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

            //��x�f�t�H���g�ɖ߂��ۂ̏������s���B�i��F���G��Ԃ��I�������ꍇ�͖��G�t���O��OFF�ɂ���Ȃǁj
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

            //�V�����X�e�[�^�X�̏������s��
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
                    model.gameObject.layer = 13; //Layer�́u13�v�Ɋ��蓖�ĂĂ��铧�ߏ�ԂɕύX����
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

        public void ResetStatus(int maxHP)
        {
            _currentHP = maxHP;
            _currentHitchhikerAmount = 0;
        }
        #endregion

        #region private method
        #endregion

        #region coroutine method
        #endregion
    }
}