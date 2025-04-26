using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class ItemOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [FormerlySerializedAs("curItemRuntimeData")] [FormerlySerializedAs("curItemData")] public InventoryItemRuntimeData curInventoryItemRuntimeData;
    public Transform originalParent; // 原始父級

    private void Start()
    {
        //curItemData = transform.parent.gameObject.GetComponent<Slot>().SlotItemData;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        return;
        Debug.Log("OnBeginDrag");
        // 紀錄原始拖動物品的父級
        originalParent = transform.parent;
        // 把Item變成跟Slot一樣的父級，如果只有一個parent代表變為父類的子級而已，而兩個parent則是父類的父類的子級
        transform.SetParent(transform.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        return;
        Debug.Log("OnDrag");
        // 把物品的位子設為鼠標的位子
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        return;
        Debug.Log("OnEndDrag");
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);

        Transform targetSlot = eventData.pointerCurrentRaycast.gameObject.transform.parent?.parent;
        Transform originalSlot = originalParent;

        //Debug.Log(originalSlot.gameObject.name);
        //if (targetSlot.GetComponent<Slot>().slotItem.Item != InventoryManager.instance.curBagType)
        //{
        //    Debug.Log("當前物品類別不匹配背包");
        //    return;
        //}


        //if (eventData.pointerCurrentRaycast.gameObject == null || eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>() == null || originalSlot.GetComponent<Slot>().slotItemData.Type != InventoryManager.Instance.curBagType)
        {
            Debug.Log("沒有找到有效的Slot，將物品返回原位");
            // 如果沒有點擊到有效的Slot，將物品返回原位
            transform.SetParent(originalParent);
            transform.position = originalParent.position;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            return;
        }


        //物品兌換時觸發
        if (eventData.pointerCurrentRaycast.gameObject.name == "Item Image" && targetSlot != null)
        {
            //Debug.Log("進入這裡");
            // 暫存目標Slot的slotItem
            //var targetSlotItem = targetSlot.GetComponent<Slot>().SlotItemData;

            // 設置該Item作為目標Slot的子類
            transform.SetParent(targetSlot);
            transform.position = targetSlot.position;

            // 設置目標Slot的Item位置為原始Slot的位置
            eventData.pointerCurrentRaycast.gameObject.transform.parent.position = originalSlot.position;
            eventData.pointerCurrentRaycast.gameObject.transform.parent.SetParent(originalSlot);

            // 將原始Slot的slotItem設為目標Slot的slotItem
            //originalSlot.GetComponent<Slot>().SlotItemData = targetSlotItem;
        }
        else
        {
            // 設置該Item作為目標Slot的子類
            transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
            transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;

            // 將原始Slot的slotItem設為null
            //originalSlot.GetComponent<Slot>().SlotItemData = null;
        }

        GetComponent<CanvasGroup>().blocksRaycasts = true;

        // 更新目標Slot的slotItem
        //transform.parent.GetComponent<Slot>().SlotItemData = curItemData;

        // 刷新背包
        //InventoryManager.instance.RefreshInventoryInGridSlot();
    }

    private int CalculateGlobalIndex(Transform to)
    {
        int globalIndex = 0;

        // 獲取父物件
        Transform parent = to.parent;

        // 確認父物件是否為 "強化裝備"
        if (parent.name == "強化裝備")
        {
            Debug.Log("該Slot父類為強化裝備");
            globalIndex += parent.GetSiblingIndex() + to.GetSiblingIndex();
        }
        // 確認父物件是否為 "強化裝備所需圖紙"
        else if (parent.name == "強化裝備所需圖紙")
        {
            Debug.Log("該Slot父類為強化裝備所需圖紙");
            globalIndex += parent.GetSiblingIndex() + to.GetSiblingIndex();
        }
        // 確認父物件是否為 "強化裝備素材"
        else if (parent.name == "強化裝備素材")
        {
            Debug.Log("該Slot父類為強化裝備素材");
            globalIndex += parent.GetSiblingIndex() + to.GetSiblingIndex();
        }

        Debug.Log("最終全局索引: " + globalIndex);
        return globalIndex;
    }

    // private void SwapItems(Transform from, Transform to)
    // {
    //     int fromIndex = from.GetSiblingIndex();
    //     int toIndex = to.GetSiblingIndex();
    //
    //     // 假設你的背包數據結構是 List<BaseItemData>
    //     var temp = InventoryManager.Instance.curmyBagItems[fromIndex];
    //     InventoryManager.Instance.curmyBagItems[fromIndex] = InventoryManager.Instance.curmyBagItems[toIndex];
    //     InventoryManager.Instance.curmyBagItems[toIndex] = temp;
    // }
}
