// using System.Collections.Generic;
// using System.IO;
// using UnityEngine;
// using UnityEngine.Events;
//
// //[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
// [System.Serializable]
// public class BaseItemData : ScriptableObject
// {
//     [Header("物品詳情")]
//     [SerializeField] int itemID;
//     [SerializeField] ItemType itemType;
//     [SerializeField] ItemRarity itemRarity;
//     [SerializeField] Sprite itemIcon;
//
//     [Header("物品資訊")]
//     [SerializeField] string itemName;
//     [SerializeField] int quantity;
//     [SerializeField] int maxQuantity;
//     [TextArea][SerializeField] string itemDescription;
//
//     // 事件：當物品數量變化時觸發
//     [Header("事件")]
//     public UnityEvent<int> onQuantityChanged;
//
//     // 公共屬性來訪問私有字段
//
//     //物品詳情
//     /// <summary>
//     /// 該物品的ID
//     /// </summary>
//     public int ID => itemID;
//     /// <summary>
//     /// 該物品的類型
//     /// </summary>
//     public ItemType Type => itemType;
//     /// <summary>
//     /// 該物品的品質
//     /// </summary>
//     public ItemRarity Rarity => itemRarity;
//     /// <summary>
//     /// 該物品的圖標
//     /// </summary>
//     public Sprite Icon => itemIcon;
//
//     //物品資訊
//     /// <summary>
//     /// 該物品的名稱
//     /// </summary>
//     public string Name => itemName;
//     /// <summary>
//     /// 該物品當前堆疊數量
//     /// </summary>
//     public int Quantity
//     {
//         get => quantity;
//         set
//         {
//             quantity = Mathf.Clamp(value, 0, maxQuantity);
//             onQuantityChanged?.Invoke(quantity);
//         }
//     }
//     /// <summary>
//     /// 該物品可以達到的最滿堆疊數量
//     /// </summary>
//     public int MaxQuantity => maxQuantity;
//     /// <summary>
//     /// 該物品的描述
//     /// </summary>
//     public string Description => itemDescription;
//
//     protected virtual void OnValidate()
//     {
//         // 驗證邏輯
//     }
//
//     private static readonly Dictionary<string, ItemType> itemTypeMap = new Dictionary<string, ItemType>
// {
//     { "Equipment", ItemType.Equipment },
//     { "Materials", ItemType.Materials },
//     { "KeyItems", ItemType.KeyItems }
// };
//
//     private static readonly Dictionary<string, ItemRarity> itemRarityMap = new Dictionary<string, ItemRarity>
// {
//     { "Poor", ItemRarity.Poor },
//     { "Common", ItemRarity.Common },
//     { "Uncommon", ItemRarity.Uncommon },
//     { "Rare", ItemRarity.Rare }
// };
//
//     /// <summary>
//     /// 進行物品初始化
//     /// </summary>
//     /// <param name="itemID"></param>
//     /// <param name="itemType"></param>
//     /// <param name="itemQuality"></param>
//     /// <param name="itemName"></param>
//     /// <param name="maxQuantity"></param>
//     /// <param name="itemDescription"></param>
//     public virtual void Initialize(int itemID, string itemName, string itemType, string itemRarity, int maxQuantity, string itemDescription, int itemValue, int health, int attack, int defense)
//     {
//         this.itemID = itemID;
//         this.itemName = itemName;
//         this.maxQuantity = maxQuantity;
//         this.itemDescription = itemDescription;
//
//         itemIcon = Resources.Load<Sprite>($"Image/{itemName}");
//
//         if (itemTypeMap.TryGetValue(itemType, out var parsedType))
//         {
//             this.itemType = parsedType;
//         }
//
//         if (itemRarityMap.TryGetValue(itemRarity, out var parsedQuality))
//         {
//             this.itemRarity = parsedQuality;
//         }
//     }
//
//
//
//     /// <summary>
//     /// 增加該物品數量
//     /// </summary>
//     /// <param name="amount"></param>
//     public void AddQuantity(int amount)
//     {
//         Quantity += amount;
//     }
//
//     /// <summary>
//     /// 減少該物品數量
//     /// </summary>
//     /// <param name="amount"></param>
//     public void RemoveQuantity(int amount)
//     {
//         Quantity -= amount;
//     }
//
//     /// <summary>
//     /// 克隆該物品
//     /// </summary>
//     /// <returns></returns>
//     public BaseItemData Clone()
//     {
//         BaseItemData clone = Instantiate(this);
//         clone.quantity = this.quantity;
//         return clone;
//     }
// }
//
// /// <summary>
// /// 物品類型枚舉
// /// </summary>
// public enum ItemType
// {
//     Equipment,    //裝備 100001 ~ 100999
//     Materials,    //材料 200001 ~ 200999
//     KeyItems,     //重要物品 300001 ~ 300999
// }
//
// /// <summary>
// /// 物品品質枚舉
// /// </summary>
// public enum ItemRarity
// {
//     Poor,      // 粗劣
//     Common,    // 一般
//     Uncommon,  // 優良
//     Rare       // 稀有
// }
//
//
// [System.Serializable]
// public class BaseItemDataSerializable
// {
//     public int itemID;
//     public ItemType itemType;
//     public ItemRarity itemRarity;
//     public string itemIconPath; // 假設你用路徑來保存圖標
//     public string itemName;
//     public int quantity;
//     public int maxQuantity;
//     public string itemDescription;
//
//     public BaseItemDataSerializable(BaseItemData itemData)
//     {
//         itemID = itemData.ID;
//         itemType = itemData.Type;
//         itemRarity = itemData.Rarity;
//         itemIconPath = itemData.Icon != null ? itemData.Icon.name : null; // 假設用圖標名稱作為路徑
//         itemName = itemData.Name;
//         quantity = itemData.Quantity;
//         maxQuantity = itemData.MaxQuantity;
//         itemDescription = itemData.Description;
//     }
// }