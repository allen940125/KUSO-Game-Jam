// using System;
// using System.Collections.Generic;
// using UnityEngine;
//
// [CreateAssetMenu(menuName = "Inventory/Recipe/RecipeData", fileName = "RecipeData")]
// public class RecipeData : ScriptableObject
// {
//     [Header(("物品合成的所需材料"))]
//     [SerializeField] List<CraftingMaterialItem> ingredients;
//     [Header("物品合成的結果")]
//     [SerializeField] int result_ItemID;
//     [Header("物品合成完後給的數量")]
//     [SerializeField] int quantity;
//
//     /// <summary>
//     /// 物品合成的所需材料
//     /// </summary>
//     public List<CraftingMaterialItem> Ingredients => ingredients;
//     /// <summary>
//     /// 物品合成的結果
//     /// </summary>
//     public BaseItemData Result => InventoryManager.Instance.GetItemInDatabase(result_ItemID);
//     /// <summary>
//     /// 物品合成完後給的數量
//     /// </summary>
//     public int Quantity => quantity;
//
//     public void Initialize(List<CraftingMaterialItem> ingredients, int result_ItemID, int quantity)
//     {
//         this.ingredients = ingredients;
//         this.result_ItemID = result_ItemID;
//         this.quantity = quantity;
//     }
// }
//
// [Serializable]
// public class CraftingMaterialItem
// {
//     [SerializeField] int itemID;
//     [SerializeField] int quantity;
//
//     public BaseItemData ItemData => InventoryManager.Instance.GetItemInDatabase(itemID);
//     public int Quantity => quantity;
//
//     public CraftingMaterialItem(int itemID, int quantity)
//     {
//         this.itemID = itemID;
//         this.quantity = quantity;
//     }
// }