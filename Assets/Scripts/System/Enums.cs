/// <summary>
/// ゲーム中に使用するEnumをまとめたScript
/// </summary>
public enum GameState
{
    Title,
    InGame,
    Result
}

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

public enum BuildingType
{
    Building1,
    Building2,
    Building3,
    Building4,
}

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
public enum ItemType
{
    Boost,
    Infiltrator,
    /// <summary>
    /// 無敵
    /// </summary>
    Invincible
}

public enum GeneratePointType
{
    Left,
    Right
}

public enum BGMType
{
    /// <summary> タイトル画面 </summary>
    Title,
    InGame,
    Result
}
public enum SEType
{
    Title_GameStart,
    InGame_CarHorn,
    Crash,
    CountDown,
    Damage_Player
}
public enum StageLevel
{
    Easy,
    Normal,
    Hard
}
public enum HitchhikerType
{
    Female_G,
}
public enum HitchhikerState
{
    Idle,
    Dancing,
    BlowOff
}

public enum CameraType
{
    Title,
    InGame
}