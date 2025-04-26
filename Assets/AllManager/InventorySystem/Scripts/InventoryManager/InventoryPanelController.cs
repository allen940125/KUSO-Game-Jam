using System;
using Game.UI;
using Gamemanager;
using UnityEngine;

public class InventoryPanelController
{
    [Header("背包UI位置")]
    private GameObject _uiPanel;
    private GameObject _slotGrid;

    private GameObject _emptySlot;//空位
    
    public InventoryPanelController()
    {
        SubscribeEvents(); // 抽成獨立方法
    }

    public void Dispose()
    {
        UnsubscribeEvents(); // 手動呼叫解除訂閱
        GC.SuppressFinalize(this); // 避免重複清理
    }
    
    #region 初始化
    
    private void SubscribeEvents()
    {
        Debug.Log("設置訂閱");
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerBagRefreshedEvent, OnPlayerBagRefreshedEvent);
    }

    private void UnsubscribeEvents()
    {
        Debug.Log("解除訂閱");
        GameManager.Instance.MainGameEvent.Unsubscribe<PlayerBagRefreshedEvent>();
    }

    #endregion

    private void OnPlayerBagRefreshedEvent(PlayerBagRefreshedEvent cmd)
    {
        RefreshPlayerBagItem(cmd.ItemControllerType);
    }
    
    #region 背包更新

    /// <summary>
    /// 設置BagMenu的背包資訊
    /// </summary>
    /// <param name="uiPanel"></param>
    /// <param name="grid"></param>
    public void SetBagInfo(GameObject uiPanel, GameObject grid, GameObject emptySlot)
    {
        this._uiPanel = uiPanel;
        _slotGrid = grid;
        _emptySlot = emptySlot;
        
        Debug.Log(_slotGrid.name + "跟" + _emptySlot.name);
    }
    
    /// <summary>
    /// 重新整理玩家背包物品
    /// </summary>
    /// <param name="itemControllerType"></param>
    private void RefreshPlayerBagItem(ItemControllerType itemControllerType)
    {
        Debug.Log(_slotGrid);
        InventoryManager.Instance.ClearChildObjects(_slotGrid.transform);

        // 使用 LINQ 或 Find 方法直接取得符合分類
        var category = SaveManager.Instance.CurrentSaveData.InventoryData.categoryGroups
            .Find(c => c.categoryName == itemControllerType);

        if (category == null)
        {
            Debug.LogWarning("沒有找到符合的背包分類: " + itemControllerType);
            return;
        }
    
        // 更新當前分類名稱（如果需要）
        InventoryManager.Instance.curCategoryTypeName = itemControllerType;

        Debug.Log(category.items.Count + " 生成格子");
        
        // 根據該分類的 Items 數量生成格子
        for (int i = 0; i < category.items.Count; i++)
        {
            GameObject curGameObject = GameManager.Instance.InstantiateFromManager(_emptySlot);
            curGameObject.transform.SetParent(_slotGrid.transform);
            Debug.Log(category.items[i].itemId);
            curGameObject.GetComponent<SlotItem>().Initialize(category.items[i]);
        }
    }
    #endregion
}
