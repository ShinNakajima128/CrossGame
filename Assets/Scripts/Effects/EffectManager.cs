using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    /// <summary>
    /// 使用しない
    /// </summary>
    None,
    /// <summary>
    /// 獲得
    /// </summary>
    Get,
    /// <summary>
    /// 妨害
    /// </summary>
    Obstacle,
    Swap,
    DorMouse_Find,
    /// <summary>
    /// ロビー時計が12時になった時のハート
    /// </summary>
    Heart,
    /// <summary>
    /// 女王の着地時の衝撃波
    /// </summary>
    ShockWave,
    Stage2_WarpStart,
    Stage2_WarpEnd,
    Stage3_Goal,
    Debris_Landing,
    Player_Heal,
    FallPole
}

public class EffectManager : SingletonMonoBehaviour<EffectManager>
{
    // オブジェクトのデータをファイルに保存する
    [System.Serializable]
    class EffectData
    {
        /// <summary> EffectのPrefab </summary>
        public GameObject EffectPrefab = default;
        /// <summary> 最大表示数 </summary>
        public int MaxCount = 1;
    }

    [SerializeField] 
    EffectData[] m_effectDatas = default;

    #region property
    public Dictionary<EffectType, List<EffectControl>> EffectDic => m_effectDic;

    protected override bool IsDontDestroyOnLoad => true;
    #endregion

    #region private
    private Dictionary<EffectType, List<EffectControl>> m_effectDic = new Dictionary<EffectType, List<EffectControl>>();
    #endregion

    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < m_effectDatas.Length; i++)
        {
            EffectType effectType = (EffectType)(i + 1);
            m_effectDic.Add(effectType, new List<EffectControl>());
            for (int k = 0; k < m_effectDatas[i].MaxCount; k++)
            {
                var instance = Instantiate(m_effectDatas[i].EffectPrefab, this.transform);
                var eControl = instance.AddComponent<EffectControl>();
                m_effectDic[effectType].Add(eControl);
            }
        }
    }

    public static void PlayEffect(EffectType effectType, Vector3 pos)
    {
        foreach (var effect in Instance.m_effectDic[effectType])
        {
            if (effect.IsActive())
            {
                continue;
            }
            effect.Play(pos);
            return;
        }
    }

    public static void PlayEffect(EffectType effectType, Transform parent)
    {
        foreach (var effect in Instance.m_effectDic[effectType])
        {
            if (effect.IsActive())
            {
                continue;
            }
            effect.Play(parent);
            return;
        }
    }

    public static void ReturnEffect()
    {
        print("エフェクトを停止");
        foreach (var effect in Instance.m_effectDic)
        {
            foreach (var e in effect.Value)
            {
                if (!e.IsActive())
                {
                    continue;
                }
                e.Stop();
                print($"再生中のエフェクトを停止:{e.name}");
            }
        }
    }
}
