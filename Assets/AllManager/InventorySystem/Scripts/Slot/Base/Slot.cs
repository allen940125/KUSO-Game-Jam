using System;
using Gamemanager;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public abstract class Slot : MonoBehaviour
{
    [Header("格子")]
    public bool canClickSlotItemBtn;

    [Header("物品資訊")]
    public ItemDataBaseTemplete slotItemData;
    
    public GameObject objItemInSlot;
    public Image imageItemIcon;
    public TMP_Text textItemQuantity;

    /// <summary>
    /// 進行Slot_StoreItem的初始化設定
    /// </summary>
    /// <param name="itemData">物品資訊</param>
    protected virtual void Initialize(ItemDataBaseTemplete itemData)
    {
        SetupSlot(itemData);

        if (itemData != null)
        {
            slotItemData = itemData;

            //把slotImage的圖片改成item的圖片
            imageItemIcon.sprite = itemData.ItemIconPath;
        }
    }

    /// <summary>
    /// Item按鈕被按下時觸發
    /// </summary>
    protected virtual void ItemOnClicked()
    {
        //GameManager.Instance.MainGameEvent.Send(new ItemUIClicked() { StoredItemData = slotItemData });
        Debug.Log("ItemOnClicked");
    }

    /// <summary>
    /// 用於設定Slot的顏色與設置ItemOnClicked
    /// </summary>
    /// <param name="item"></param>
    public void SetupSlot(ItemDataBaseTemplete item)
    {
        if (item == null)
        {
            objItemInSlot.SetActive(false);
            return;
        }

        objItemInSlot.SetActive(true);
        
        switch (item.ItemRarityType)
        {
            case ItemRarityType.Poor:
            {
                //白
                SetImageColor(new Color(1f, 1f, 1f));
                break;
            }
            case ItemRarityType.Common:
            {
                //綠
                SetImageColor(new Color(0f, 1f, 0f));
                break;
            }
            case ItemRarityType.Uncommon:
            {
                //藍
                SetImageColor(new Color(0f, 0f, 1f));
                break;
            }
            case ItemRarityType.Rare:
            {
                //紫
                SetImageColor(new Color(0.5f, 1f, 0.5f));
                break;
            }
        }
        //Debug.Log(canClickSlotItemBtn + " 狀態");
        if (canClickSlotItemBtn)
        {
            //Debug.Log("開始加入事件");
            GetComponentInChildren<Button>().onClick.AddListener(ItemOnClicked);
            //Debug.Log("成功加入事件");
        }
    }

    /// <summary>
    /// 設置Slot顏色
    /// </summary>
    /// <param name="newColor"></param>
    private void SetImageColor(Color newColor)
    {
        Image slotImage = GetComponent<Image>();
        if (slotImage != null)
        {
            slotImage.color = newColor;
        }
    }
}
