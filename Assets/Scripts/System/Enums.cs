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
}