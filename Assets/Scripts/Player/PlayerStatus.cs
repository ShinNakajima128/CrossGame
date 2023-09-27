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
        /// �R���{���𑝉�����
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

            //��x�f�t�H���g�ɖ߂��ۂ̏������s���B�i��F���G��Ԃ��I�������ꍇ�͖��G�t���O��OFF�ɂ���Ȃǁj
            switch (_currentState)
            {
                case PlayerState.Normal:
                    break;
                case PlayerState.Slowing:
                    model._currentMoveSpeed = model._originSpeed;
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
                    model.BreakBoost();
                    model._originSpeed = model._currentMoveSpeed;
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

        public void ChangeHitchhikerNum(PlayerModel model)
        {
            model._currentMoveSpeed += model._moveSpeed * 0.15f;
        }

        public void ResetCombo()
        {
            _currentComboAmountRP.Value = 0;
        }

        public void ResetStatus()
        {
            _currentState = PlayerState.Normal;
            _currentComboAmountRP.Value = 0;
        }
        #endregion

        #region private method
        #endregion

        #region coroutine method
        #endregion
    }
}