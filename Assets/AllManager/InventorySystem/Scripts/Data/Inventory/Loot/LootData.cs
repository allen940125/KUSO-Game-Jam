// using System;
// using System.Collections.Generic;
// using UnityEngine;
//
// [CreateAssetMenu(fileName = "LootData", menuName = "Inventory/Loot/LootData")]
// public class LootData : ScriptableObject
// {
//     [Header("戰利品詳情")]
//     [SerializeField] private int lootID;  // 獎勵品ID
//     [SerializeField] private List<LootItemData> lootItemDataList = new List<LootItemData>();  // 掉落物品的列表
//
//     /// <summary>
//     /// 戰利品ID
//     /// </summary>
//     public int LootID => lootID;
//
//     /// <summary>
//     /// 掉落物品列表
//     /// </summary>
//     public List<LootItemData> LootItemDataList => lootItemDataList;
//
//     /// <summary>
//     /// 初始化戰利品數據
//     /// </summary>
//     public void Initialize(int lootID, List<LootItemData> lootItems)
//     {
//         this.lootID = lootID;
//         lootItemDataList = lootItems;
//     }
// }
//
//
// [Serializable]
// public class LootItemData
// {
//     [Header("觀察用數據")]
//     [SerializeField] ItemDataBaseTemplete itemData;
//
//     [Header("戰利品必備數值")]
//     [SerializeField] private string itemName;  // 物品名稱
//     [SerializeField] private int minQuantity;  // 最小掉落數量
//     [SerializeField] private int maxQuantity;  // 最大掉落數量
//     [SerializeField] private float dropRate;   // 掉落概率
//
//     /// <summary>
//     /// 戰利品物品數據
//     /// </summary>
//     public ItemDataBaseTemplete ItemData => itemData;
//
//     /// <summary>
//     /// 物品名稱
//     /// </summary>
//     public string ItemName => itemName;
//
//     /// <summary>
//     /// 最小掉落數量
//     /// </summary>
//     public int MinQuantity => minQuantity;
//
//     /// <summary>
//     /// 最大掉落數量
//     /// </summary>
//     public int MaxQuantity => maxQuantity;
//
//     /// <summary>
//     /// 掉落概率
//     /// </summary>
//     public float DropRate => dropRate;
//
//     /// <summary>
//     /// 初始化掉落物品數據
//     /// </summary>
//     public LootItemData(string itemName, int minQuantity, int maxQuantity, float dropRate)
//     {
//         Debug.Log(itemName);
//         itemData = InventoryManager.Instance.GetItemInDatabase(itemID);
//
//         this.itemName = itemName;
//         this.minQuantity = minQuantity;
//         this.maxQuantity = maxQuantity;
//         this.dropRate = dropRate;
//     }
// }