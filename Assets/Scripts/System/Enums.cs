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
    /// <summary>
    /// ���G
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