using UnityEngine;
/// <summary>
/// ダメージを受ける機能のインターフェース
/// </summary>
public interface IDamagable
{
    #region property
    /// <summary>無敵状態かどうか</summary>
    bool IsInvincible { get; }
    #endregion

    #region public method
    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="amount">ダメージ量</param>
    void Damage(float amount);
    void Knockback(Transform other);
    #endregion
}
