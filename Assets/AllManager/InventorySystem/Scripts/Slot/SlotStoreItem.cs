using Gamemanager;
using TMPro;
using UnityEngine;

public class SlotStoreItem : Slot
{
    public StoreItemRuntimeData storeItemData;
    
    [Header("子類必備資訊")]
    [SerializeField] TMP_Text text_ItemPrice;
    [SerializeField] GameObject storeMenu;
    
    /// <summary>
    /// 進行Slot_StoreItem的初始化設定
    /// </summary>
    /// <param name="recipeData"></param>
    public void Initialize(StoreItemRuntimeData storeItemData, GameObject storeMenu)
    {
        this.storeItemData = storeItemData;
        this.storeMenu = storeMenu;

        Debug.Log("商店物品初始化的StoreItemBaseTemplete為" + storeItemData.BaseTemplete.ItemId + "ItemBaseTemplete物品ID為" + storeItemData.ItemBaseTemplete.Id);
        base.Initialize(storeItemData.ItemBaseTemplete);
        
        //把slotNum的文字改成該item物品數量
        textItemQuantity.text = storeItemData.BaseTemplete.ItemQuantity.ToString();
        text_ItemPrice.text = storeItemData.BaseTemplete.ItemBasePrice.ToString();
    }

    //當按鈕被按時執行
    protected override void ItemOnClicked()
    {
        base.ItemOnClicked();
        GameManager.Instance.MainGameEvent.Send(new StoreItemClickedEvent() { StoreItemData = storeItemData });
    }
}
