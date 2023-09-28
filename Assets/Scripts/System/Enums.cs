// �Q�[�����Ɏg�p����Enum���܂Ƃ߂�Script

/// <summary>
/// �Q�[���̏��
/// </summary>
public enum GameState
{
    Title,
    InGame,
    Result
}

/// <summary>
/// �v���C���[�̏��
/// </summary>
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

/// <summary>
/// ��Q���̎��
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
/// �A�C�e���̎��
/// </summary>
public enum ItemType
{
    Boost,
    Infiltrator,
}

/// <summary>
/// �����̐����ʒu
/// </summary>
public enum GeneratePointType
{
    Left,
    Right
}
/// <summary>
/// BGM�̎��
/// </summary>
public enum BGMType
{
    /// <summary> �^�C�g����� </summary>
    Title,
    /// <summary> �C���Q�[����� </summary>
    InGame,
    /// <summary> ���U���g��� </summary>
    Result
}
/// <summary>
/// SE�̎��
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
/// �q�b�`�n�C�J�[�̎��
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
/// �q�b�`�n�C�J�[�̏��
/// </summary>
public enum HitchhikerState
{
    Idle,
    Dancing,
    BlowOff
}
/// <summary>
/// �J�����̎��
/// </summary>
public enum CameraType
{
    Title,
    InGame
}
/// <summary>
/// ���U���g��ʂ̃{�^���̎��
/// </summary>
public enum ResultButtonType
{
    Retry,
    Title
}