// using UnityEngine;
//
// [CreateAssetMenu(fileName = "100000_ItemData", menuName = "Inventory/Item/EquipmentData/EquipmentData")]
// public class BaseEquipmentData : BaseItemData
// {
//     [Header("裝備數值")]
//
//     [SerializeField] int equipmentLevel;
//     [SerializeField] float durability;
//     [SerializeField] float maxDurability;
//
//     [SerializeField] float health;
//     [SerializeField] float attack;
//     [SerializeField] float defense;
//     [SerializeField] float speed;
//
//     [Header("武器專用數據")]
//     [SerializeField] GameObject attackCollisionPrefab;
//
//     // 公共屬性來訪問私有字段
//     public int EquipmentLevel => equipmentLevel;
//     public float Durability
//     {
//         get => durability;
//         set => durability = Mathf.Clamp(value, 0, maxDurability);
//     }
//     public float MaxDurability => maxDurability;
//
//     public float Health => health;
//     public float Attack => attack;
//     public float Defense => defense;
//     public float Speed => speed;
//
//     public GameObject AttackCollisionPrefab => attackCollisionPrefab;
//
//     // 初始化方法
//     public override void Initialize(int itemID, string itemName, string itemType, string itemRarity, int maxQuantity, string itemDescription, int itemValue, int health, int attack, int defense)
//     {
//         base.Initialize(itemID, itemName, itemType, itemRarity, maxQuantity, itemDescription, itemValue, health, attack, defense);
//
//         this.health = health;
//         this.attack = attack;
//         this.defense = defense;
//     }
//
//
//     public string EquipmentItemInfo()
//     {
//         string equipmentItemInfo = "";
//
//         if (health > 0) equipmentItemInfo += "生命值: " + health + "\n";
//         if (attack > 0) equipmentItemInfo += "攻擊力: " + attack + "\n";
//         if (defense > 0) equipmentItemInfo += "防禦力: " + defense + "\n";
//         if (speed > 0) equipmentItemInfo += "速度: " + speed + "\n";
//         
//         return equipmentItemInfo;
//     }
//
//     // 使用武器的方法
//     public void UseWeapon(float amount)
//     {
//         Durability -= amount;
//         if (Durability <= 0)
//         {
//             // 處理武器損壞的邏輯
//             Debug.Log($"{Name} is broken!");
//         }
//     }
//
//     // 修復武器的方法
//     public void RepairWeapon(float amount)
//     {
//         Durability += amount;
//     }
// }
