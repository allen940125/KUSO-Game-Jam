using Gamemanager;
using UnityEngine.Serialization;

public class SlotItem : Slot
{
    [FormerlySerializedAs("storedItemRuntimeData")] [FormerlySerializedAs("storedItemData")] public InventoryItemRuntimeData storedInventoryItemRuntimeData;
    
    /// <summary>
    /// 進行ItemSlot的初始化設定
    /// </summary>
    /// <param name="inventoryItemRuntimeData"></param>
    public void Initialize(InventoryItemRuntimeData inventoryItemRuntimeData)
    {
        storedInventoryItemRuntimeData = inventoryItemRuntimeData;
        
        base.Initialize(inventoryItemRuntimeData.BaseTemplete);
        
        //把slotNum的文字改成該item物品數量
        textItemQuantity.text = inventoryItemRuntimeData.quantity.ToString();
    }

    protected override void ItemOnClicked()
    {
        base.ItemOnClicked();
        GameManager.Instance.MainGameEvent.Send(new InventoryItemClickedEvent() { StoredInventoryItemRuntimeData = storedInventoryItemRuntimeData });
    }
}
