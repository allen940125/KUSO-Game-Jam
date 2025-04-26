using System;
using System.Collections.Generic;
using Datamanager;
using Gamemanager;
using UnityEngine;
using UnityEngine.Serialization;

public class InventoryManager : Singleton<InventoryManager>
{
    public static InventoryPanelController InventoryPanelController { get; private set; }

    [Header("物品、角色資料")]
    public InventoryItemRuntimeData curClickInventoryItemRuntimeData;
    public InventoryItemRuntimeData PlayerMoney => GetInventoryData(100);

    [Header("玩家背包資料")]
    public ItemControllerType curCategoryTypeName;

    public int initializeMoney;

    // 直接從 SaveManager 獲取背包數據
    private InventoryRuntimeData CurrentInventory => SaveManager.Instance.CurrentSaveData.InventoryData;
    
    protected override void Awake()
    {
        base.Awake(); // 先執行 Singleton 的檢查

        // 如果當前物件已被標記銷毀，直接返回
        if (_instance != this) return;

        InitializeManagers();
        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        InventoryPanelController?.Dispose(); // 手動觸發清理
        UnsubscribeFromEvents();
    }

    void Start()
    {
        PrintInventory();
    }

    #region 初始化
    
    private void SubscribeToEvents()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnItemAddedToBagEvent, OnItemAddedToBagEvent);
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnInventoryItemClickedEvent, OnInventoryItemClickedEvent);
    }

    private void UnsubscribeFromEvents()
    {
        if (GameManager.Instance != null && GameManager.Instance.MainGameEvent != null)
        {
            GameManager.Instance.MainGameEvent.Unsubscribe<ItemAddedToBagEvent>();
            GameManager.Instance.MainGameEvent.Unsubscribe<InventoryItemClickedEvent>();
        }
    }
    
    /// <summary>
    /// 初始化分支管理器
    /// </summary>
    private void InitializeManagers()
    {
        InventoryPanelController = new InventoryPanelController();
    }
    
    #endregion
    
    
    #region 事件訂閱

    private void OnItemAddedToBagEvent(ItemAddedToBagEvent cmd)
    {
        AddItem(cmd.ItemID, cmd.Quantity);
    }

    private void OnInventoryItemClickedEvent(InventoryItemClickedEvent cmd)
    {
        SetCurrentSelectedItem(cmd.StoredInventoryItemRuntimeData);
    }

    
    #endregion
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            AddItem(100, initializeMoney);
            AddItem(101, 120);  // 測試加入 120 個 ID 為 101 的物品
            AddItem(102, 50);   // 測試加入 50 個 ID 為 102 的物品
            PrintInventory();
        }
    }

    /// <summary>
    /// 設置當前選擇物品
    /// </summary>
    /// <param name="slotInventoryItemRuntime"></param>
    private void SetCurrentSelectedItem(InventoryItemRuntimeData slotInventoryItemRuntime)//更新物品資訊
    {
        curClickInventoryItemRuntimeData = slotInventoryItemRuntime;
    }
    
    /// <summary>
    /// 取得背包內的物品數據
    /// </summary>
    public InventoryItemRuntimeData GetInventoryData(int itemId)
    {
        ItemControllerType itemType = GameContainer.Get<DataManager>().GetDataByID<ItemDataBaseTemplete>(itemId).ItemControllerType;
        
        foreach (var category in CurrentInventory.categoryGroups)
        {
            if (category.categoryName == itemType)
            {
                return category.items.Find(item => item.itemId == itemId);
            }
        }

        if (itemId == 100)
        {
            InventoryItemRuntimeData inventoryItemRuntimeData = new InventoryItemRuntimeData();
            inventoryItemRuntimeData.itemId = itemId;
            inventoryItemRuntimeData.quantity = 0;
            return inventoryItemRuntimeData;
        }
        
        return null; // 找不到物品
    }

    /// <summary>
    /// 新增物品到背包（依照分類）
    /// </summary>
    public void AddItem(int itemId, int count)
    {
        ItemControllerType itemType = GameContainer.Get<DataManager>().GetDataByID<ItemDataBaseTemplete>(itemId).ItemControllerType;

        // 先找到對應類別的背包
        InventoryCategoryGroup category = CurrentInventory.categoryGroups.Find(c => c.categoryName == itemType);

        // 如果該類別不存在，則創建一個新分類
        if (category == null)
        {
            category = new InventoryCategoryGroup { categoryName = itemType };
            CurrentInventory.categoryGroups.Add(category);
        }

        Debug.Log(itemType);
        
        // 在對應的分類中查找該物品
        InventoryItemRuntimeData foundInventoryItemRuntime = category.items.Find(item => item.itemId == itemId);

        if (foundInventoryItemRuntime != null)
        {
            foundInventoryItemRuntime.quantity += count;
        }
        else
        {
            category.items.Add(new InventoryItemRuntimeData { itemId = itemId, quantity = count });
        }
    }

    /// <summary>
    /// 從背包移除物品（依照分類）
    /// </summary>
    public void RemoveItem(int itemId, int count)
    {
        ItemControllerType itemType = GameContainer.Get<DataManager>().GetDataByID<ItemDataBaseTemplete>(itemId).ItemControllerType;

        // 找到對應類別
        InventoryCategoryGroup category = CurrentInventory.categoryGroups.Find(c => c.categoryName == itemType);
        if (category == null)
        {
            Debug.LogWarning("背包內沒有該分類的物品");
            return;
        }

        // 在分類內查找物品
        for (int i = 0; i < category.items.Count; i++)
        {
            if (category.items[i].itemId == itemId)
            {
                if (category.items[i].quantity > count)
                {
                    category.items[i].quantity -= count;
                    return;
                }
                else
                {
                    count -= category.items[i].quantity;
                    category.items.RemoveAt(i);
                    i--; // 刪除後要修正索引
                }

                if (count <= 0)
                {
                    return;
                }
            }
        }

        Debug.LogWarning("背包內沒有足夠的物品可供刪除");
    }

    /// <summary>
    /// Debug 用來顯示背包內容（按分類顯示）
    /// </summary>
    void PrintInventory()
    {
        Debug.Log("========== 目前背包內容 ==========");
        if (CurrentInventory.categoryGroups.Count == 0)
        {
            Debug.Log("背包是空的");
        }
        else
        {
            foreach (var category in CurrentInventory.categoryGroups)
            {
                Debug.Log($"--- 類別: {category.categoryName} ---");
                if (category.items.Count == 0)
                {
                    Debug.Log("  (空)");
                }
                else
                {
                    foreach (var item in category.items)
                    {
                        Debug.Log($"  物品 ID: {item.itemId}, 數量: {item.quantity}");
                    }
                }
            }
        }
        Debug.Log("==================================");
    }

    /// <summary>
    /// 消除該父類中所有的子類
    /// </summary>
    public void ClearChildObjects(Transform parent)
    {
        Debug.Log(parent.name + "ClearChildObjects");
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.childCount == 0)
            {
                break;
            }
            Destroy(parent.GetChild(i).gameObject);
        }
    }
}

[Serializable]
public class InventoryItemRuntimeData
{
    public int itemId;
    public int quantity;
    
    public ItemDataBaseTemplete BaseTemplete => GameContainer.Get<DataManager>().GetDataByID<ItemDataBaseTemplete>(itemId);
}

[Serializable]
public class InventoryCategoryGroup
{
    public ItemControllerType categoryName;
    public List<InventoryItemRuntimeData> items =  new();
}

[Serializable]
public class InventoryRuntimeData
{
    public List<InventoryCategoryGroup> categoryGroups = new();
}