// ゲーム中に使用するEnumをまとめたScript

/// <summary>
/// ゲームの状態
/// </summary>
public enum GameState
{
    Title,
    InGame,
    Result
}

/// <summary>
/// プレイヤーの状態
/// </summary>
public enum PlayerState
{
    /// <summary>
    /// 通常
    /// </summary>
    Normal,
    /// <summary>
    /// 鈍足
    /// </summary>
    Slowing,
    /// <summary>
    /// 透過。ポケモンの特性「すりぬけ」から引用
    /// </summary>
    Infiltrator,
    /// <summary>
    /// 無敵
    /// </summary>
    Invincible
}

/// <summary>
/// 障害物の種類
/// </summary>
public enum ObstacleType
{
    Car_Normal,
    Car_Sports,
    Car_Truck,
    Car_Bus,
    Train,
    Box,
    Barricade
}
/// <summary>
/// アイテムの種類
/// </summary>
public enum ItemType
{
    Boost,
    Infiltrator,
}

/// <summary>
/// 建物の生成位置
/// </summary>
public enum GeneratePointType
{
    Left,
    Right
}
/// <summary>
/// BGMの種類
/// </summary>
public enum BGMType
{
    /// <summary> タイトル画面 </summary>
    Title,
    /// <summary> インゲーム画面 </summary>
    InGame,
    /// <summary> リザルト画面 </summary>
    Result
}
/// <summary>
/// SEの種類
/// </summary>
public enum SEType
{
    Title_GameStart,
    InGame_CarHorn,
    Crash,
    CountDown,
    Damage_Player,
    Accel,
    Boost,
    Result_TextView,
    Result_TotalScoreView,
    Scream_Type1,
    Scream_Type2,
    Scream_Type3,
    Scream_Type4,
    Scream_Type5,
    HitchhikersRide_Type1,
    HitchhikersRide_Type2,
    HitchhikersRide_Type3,
    HitchhikersRide_Type4,
    HitchhikersRide_Type5,
    Slow_Player,
    Goal,
    Infiltrator,
    BackToNormalState
}
/// <summary>
/// ヒッチハイカーの種類
/// </summary>
public enum HitchhikerType
{
    Female_G,
    Elder_Female_G,
    Male_G,
    Male_K,
    Little_Boy,
    MAX_NUM
}
/// <summary>
/// ヒッチハイカーの状態
/// </summary>
public enum HitchhikerState
{
    Idle,
    Dancing,
    BlowOff
}
/// <summary>
/// カメラの種類
/// </summary>
public enum CameraType
{
    Title,
    InGame
}
/// <summary>
/// リザルト画面のボタンの種類
/// </summary>
public enum ResultButtonType
{
    Retry,
    Title
}