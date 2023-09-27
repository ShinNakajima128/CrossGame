/// <summary>
/// �Q�[�����Ɏg�p����Enum���܂Ƃ߂�Script
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
    /// �ʏ�
    /// </summary>
    Normal,
    /// <summary>
    /// �ݑ�
    /// </summary>
    Slowing,
    /// <summary>
    /// ���߁B�|�P�����̓����u����ʂ��v������p
    /// </summary>
    Infiltrator,
    /// <summary>
    /// ���G
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
}

public enum GeneratePointType
{
    Left,
    Right
}

public enum BGMType
{
    /// <summary> �^�C�g����� </summary>
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

public enum StageLevel
{
    Easy,
    Normal,
    Hard
}
public enum HitchhikerType
{
    Female_G,
    Elder_Female_G,
    Male_G,
    Male_K,
    Little_Boy,
    MAX_NUM
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

public enum ResultButtonType
{
    Retry,
    Title
}