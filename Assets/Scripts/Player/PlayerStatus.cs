using System;
using UnityEngine;
using UniRx;

public partial class PlayerModel : MonoBehaviour
{
    /// <summary>
    /// プレイヤーの状態を管理するクラス
    /// </summary>
    public class PlayerStatus
    {
        #region property
        public IObservable<int> CurrentComboObserver => _currentComboAmountRP;
        public PlayerState CurrentState => _currentState;
        public int CurrentComboAmount => _currentComboAmountRP.Value;
        #endregion

        #region private
        /// <summary>現在の状態</summary>
        private PlayerState _currentState;
        #endregion

        #region const
        /// <summary>ヒッチハイカーの人数に応じた速度の係数</summary>
        private const float SPEED_COEFFICIENT_HITCHHIKER = 0.15f;
        #endregion

        #region Event
        /// <summary>コンボ数の更新を通知するReactiveProperty</summary>
        private ReactiveProperty<int> _currentComboAmountRP = new ReactiveProperty<int>();
        #endregion

        #region public method
        /// <summary>
        /// コンストラクタ
        /// </summary>
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
        /// <summary>
        /// 状態を変更する
        /// </summary>
        /// <param name="model">プレイヤーモデル</param>
        /// <param name="newState">次の状態</param>
        public void ChangeState(PlayerModel model, PlayerState newState)
        {
            //同じ状態の場合を処理を終了
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

            //新しいステータスの処理を行う
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
        /// <summary>
        /// プレイヤーオブジェクトの透過状態を変更する
        /// </summary>
        /// <param name="model">プレイヤーモデル</param>
        /// <param name="amount">透過値</param>
        public void ChangeTransparency(PlayerModel model, float amount)
        {
            //ディザリングシェーダーの透過プロパティに数値を代入
            model._playerModelRenderer.material.SetFloat("_Opacity", amount);
        }
        /// <summary>
        /// ヒッチハイカーの人数に応じた移動速度に変更する
        /// </summary>
        /// <param name="model">プレイヤーモデル</param>
        public void ChangeSpeedAccordingHitchhiker(PlayerModel model)
        {
            model._currentMoveSpeed += model._moveSpeed * SPEED_COEFFICIENT_HITCHHIKER;
        }
        /// <summary>
        /// コンボ数をリセットする
        /// </summary>
        public void ResetCombo()
        {
            _currentComboAmountRP.Value = 0;
        }
        /// <summary>
        /// プレイヤーの状態をリセットする
        /// </summary>
        public void ResetStatus()
        {
            _currentState = PlayerState.Normal;
            _currentComboAmountRP.Value = 0;
        }
        #endregion
    }
}