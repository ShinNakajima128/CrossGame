using System;
using UnityEngine;
using UniRx;

public partial class PlayerModel : MonoBehaviour
{
    /// <summary>
    /// �v���C���[�̏�Ԃ��Ǘ�����N���X
    /// </summary>
    public class PlayerStatus
    {
        #region property
        public IObservable<int> CurrentComboObserver => _currentComboAmountRP;
        public PlayerState CurrentState => _currentState;
        public int CurrentComboAmount => _currentComboAmountRP.Value;
        #endregion

        #region private
        /// <summary>���݂̏��</summary>
        private PlayerState _currentState;
        #endregion

        #region const
        /// <summary>�q�b�`�n�C�J�[�̐l���ɉ��������x�̌W��</summary>
        private const float SPEED_COEFFICIENT_HITCHHIKER = 0.15f;
        #endregion

        #region Event
        /// <summary>�R���{���̍X�V��ʒm����ReactiveProperty</summary>
        private ReactiveProperty<int> _currentComboAmountRP = new ReactiveProperty<int>();
        #endregion

        #region public method
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
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
        /// <summary>
        /// ��Ԃ�ύX����
        /// </summary>
        /// <param name="model">�v���C���[���f��</param>
        /// <param name="newState">���̏��</param>
        public void ChangeState(PlayerModel model, PlayerState newState)
        {
            //������Ԃ̏ꍇ���������I��
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
        /// <summary>
        /// �v���C���[�I�u�W�F�N�g�̓��ߏ�Ԃ�ύX����
        /// </summary>
        /// <param name="model">�v���C���[���f��</param>
        /// <param name="amount">���ߒl</param>
        public void ChangeTransparency(PlayerModel model, float amount)
        {
            //�f�B�U�����O�V�F�[�_�[�̓��߃v���p�e�B�ɐ��l����
            model._playerModelRenderer.material.SetFloat("_Opacity", amount);
        }
        /// <summary>
        /// �q�b�`�n�C�J�[�̐l���ɉ������ړ����x�ɕύX����
        /// </summary>
        /// <param name="model">�v���C���[���f��</param>
        public void ChangeSpeedAccordingHitchhiker(PlayerModel model)
        {
            model._currentMoveSpeed += model._moveSpeed * SPEED_COEFFICIENT_HITCHHIKER;
        }
        /// <summary>
        /// �R���{�������Z�b�g����
        /// </summary>
        public void ResetCombo()
        {
            _currentComboAmountRP.Value = 0;
        }
        /// <summary>
        /// �v���C���[�̏�Ԃ����Z�b�g����
        /// </summary>
        public void ResetStatus()
        {
            _currentState = PlayerState.Normal;
            _currentComboAmountRP.Value = 0;
        }
        #endregion
    }
}