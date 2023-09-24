using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// アイテム関連の機能を管理するManagerクラス
/// </summary>
public class ItemManager : MonoBehaviour
{
    #region property
    public static ItemManager Instance { get; private set; }
    #endregion

    #region serialize
    [SerializeField]
    private ItemBase[] _items = default;
    #endregion

    #region private
    private Dictionary<ItemType, ObjectPool<ItemBase>> _itemPoolDic = new Dictionary<ItemType, ObjectPool<ItemBase>>();
    #endregion

    #region Constant
    private const int ALL_ITEM_NUM = 3;
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {
        Instance = this;
        Initialize();
    }

    private void Start()
    {
    }
    #endregion

    #region public method
    /// <summary>
    /// ランダムなアイテムを生成する
    /// </summary>
    /// <returns>アイテム</returns>
    public ItemBase RandomItemGenerate()
    {
        //ItemType randomType = (ItemType)Random.Range(0, ALL_ITEM_NUM);
        ItemType randomType = ItemType.Boost;
        var item = _itemPoolDic[randomType].Rent();

        return item;
    }
    #endregion

    #region private method
    private void Initialize()
    {
        for (int i = 0; i < _items.Length; i++)
        {
            _itemPoolDic.Add((ItemType)i, new ObjectPool<ItemBase>(_items[i], transform));
        }
    }
    #endregion
    
    #region coroutine method
    #endregion
}
