// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;
//
// public class CraftingMaterial : MonoBehaviour
// {
//     [Header("�һݧ���")]
//     [SerializeField] Image itemIcon;
//     [SerializeField] TMP_Text itemName;
//     [SerializeField] TMP_Text itemQuantity;
//
//     /// <summary>
//     /// ��l��CraftingMaterial���Ω�UI
//     /// </summary>
//     /// <param name="itemIcon">���~Icon</param>
//     /// <param name="itemName">���~�W��</param>
//     /// <param name="itemQuantity">���~�ƶq</param>
//     public void Initialize(int id, Sprite itemIcon, string itemName, int itemQuantity)
//     {
//         Debug.Log(id);
//         BagItemData curBagItem = InventoryManager.Instance.GetItemsInBag(id);
//         Debug.Log(curBagItem);
//         int curBagItemQuantity = curBagItem?.Quantity ?? 0;
//
//         this.itemIcon.sprite = itemIcon;
//         this.itemName.text = itemName;
//         this.itemQuantity.text = $"{curBagItemQuantity}/{itemQuantity}";
//     }
//
// }
