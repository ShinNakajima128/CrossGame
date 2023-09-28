using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// �v���C���[��Model��View�̏������q����@�\�����R���|�[�l���g
/// </summary>
public class PlayerPresenter : MonoBehaviour
{
    #region serialize
    [Tooltip("�v���C���[�̓��쏈�����s���R���|�[�l���g")]
    [SerializeField]
    private PlayerModel _model = default;

    [Tooltip("�v���C���[����\������R���|�[�l���g")]
    [SerializeField]
    private PlayerView _view = default;
    #endregion

    #region unity methods
    private void Start()
    {
        //�Q�[���̏�Ԃɂ���ăv���C���[����\����؂�ւ���C�x���g������o�^
        GameManager.Instance.IsInGameObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(value => _model.ChangeIsCanOperation(value));

        //�q�b�`�n�C�J�[��Ԏ��̃C�x���g������o�^
        GameManager.Instance.AddHitchhikerObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(_ => _model.AddHitchhiker());

        //�R���{���ύX���̃C�x���g������o�^
        GameManager.Instance.UpdateComboObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(_ => _model.AddCombo());

        //�R���{�����Z�b�g���̃C�x���g������o�^
        GameManager.Instance.ResetComboObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(_ => _model.ResetCombo());

        //�Q�[���I�����̃C�x���g������o�^
        GameManager.Instance.GameEndObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(_ =>
                            {
                                _model.ResetVelocity();
                                _model.PlayerReset();
                            });

        //�Q�[�����������̃C�x���g������o�^
        GameManager.Instance.GameResetObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(_ => _model.PlayerReset());

        //�X�R�A���X�V����ĉ�ʂ̕\�����X�V����C�x���g������ScoreManager�ɓo�^
        ScoreManager.Instance.CurrentScoreObserver
                             .TakeUntilDestroy(this)
                             .Subscribe(value => _view.UpdateScoreView(value));

        //�v���C���[�_���[�W���̃C�x���g������GameManager��o�^
        _model.DamageObserber
              .TakeUntilDestroy(this)
              .Subscribe(_ => GameManager.Instance.OnPlayerDamage());

        //�R���{���X�V���̃C�x���g������o�^
        _model.Status.CurrentComboObserver
                     .TakeUntilDestroy(this)
                     .Subscribe(value => _view.UpdateComboView(value));
    }
    #endregion
}
