using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Datamanager;
using Gamemanager;
using UnityEngine;
using UnityEngine.Serialization;

public class StoreManager : Singleton<StoreManager>
{
    public static StorePanelController StorePanelController { get; private set; }
    
    [Header("商店當前資訊")]
    public int CurrentStoreId {get; private set;}
    
    public StoreItemRuntimeData curClickStoreItemData;
    
    [Header("商店所需資料")]
    public AllStoresRuntimeData AllStoresRuntimeData;
    
    protected override void Awake()
    {
        base.Awake();
        
        // 如果當前物件已被標記銷毀，直接返回
        if (_instance != this) return;

        InitializeManagers();
        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        StorePanelController?.Dispose();
        UnsubscribeFromEvents();
    }

    private void Update()
    {
        // 開啟商店時
        if (Input.GetKeyDown(KeyCode.K))
        {
            LoadStoreData(1001);
        }
        else if(Input.GetKeyDown(KeyCode.L))
        {
            LoadStoreData(1002);
        }
    }

    #region 初始化
    
    private void SubscribeToEvents()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnStoreItemClickedEvent, OnStoreItemClickedEvent);
    }

    private void UnsubscribeFromEvents()
    {
        if (GameManager.Instance != null && GameManager.Instance.MainGameEvent != null)
        {
            GameManager.Instance.MainGameEvent.Unsubscribe<StoreItemClickedEvent>();
        }
    }
    
    /// <summary>
    /// 初始化分支管理器
    /// </summary>
    private void InitializeManagers()
    {
        StorePanelController = new StorePanelController();
    }
    
    #endregion

    #region 事件訂閱

    private void OnStoreItemClickedEvent(StoreItemClickedEvent cmd)
    {
        SetCurrentSelectedStoreItem(cmd.StoreItemData);
    }

    
    #endregion
    
    #region 讀取資料初始化
    
    /// <summary>
    /// 載入該商店ID的資訊
    /// </summary>
    /// <param name="storeId"></param>
    private void LoadStoreData(int storeId)
{
    CurrentStoreId = storeId;
    
    // 從 CSV 獲取該商店所有商品
    var csvStoreItems = GameContainer.Get<DataManager>()
        .DataGroup.StoreDataBase.DataArray
        .Cast<StoreDataBaseTemplete>()
        .Where(item => item.StoreId == storeId)
        .ToList();

    // 查找現有商店數據
    var existingStore = AllStoresRuntimeData.stores.Find(s => s.storeId == storeId);

    if (existingStore == null)
    {
        // 創建新商店數據
        var newStore = CreateNewStore(storeId, csvStoreItems);
        AllStoresRuntimeData.stores.Add(newStore);
    }
    else
    {
        // 合併數據
        MergeStoreData(existingStore, csvStoreItems);
    }
}

private StoreRuntimeData CreateNewStore(int storeId, List<StoreDataBaseTemplete> csvItems)
{
    var newStore = new StoreRuntimeData
    {
        storeId = storeId,
        categoryGroups = new List<StoreCategoryGroup>()
    };

    var defaultCategory = new StoreCategoryGroup
    {
        categoryName = "Default",
        items = new List<StoreItemRuntimeData>()
    };

    foreach (var item in csvItems)
    {
        defaultCategory.items.Add(new StoreItemRuntimeData
        {
            entryId = item.Id,
            storeID = item.StoreId,
            storeItemId = item.ItemId,
            remainingPurchases = item.MaxPurchase,
            lastPurchaseTime = DateTime.MinValue
        });
    }

    newStore.categoryGroups.Add(defaultCategory);
    return newStore;
}

private void MergeStoreData(StoreRuntimeData existingStore, List<StoreDataBaseTemplete> csvItems)
{
    // 確保有 Default 分類
    var existingCategory = existingStore.categoryGroups.FirstOrDefault(c => c.categoryName == "Default");
    if (existingCategory == null)
    {
        existingCategory = new StoreCategoryGroup { categoryName = "Default", items = new List<StoreItemRuntimeData>() };
        existingStore.categoryGroups.Add(existingCategory);
    }

    // 建立 CSV 條目字典
    var csvItemDict = csvItems.ToDictionary(item => item.Id);

    // 處理現有項目：更新或移除
    var existingItemsCopy = existingCategory.items.ToList(); // 避免遍歷時修改集合
    foreach (var existingItem in existingItemsCopy)
    {
        if (csvItemDict.TryGetValue(existingItem.entryId, out var csvItem))
        {
            // 同步基礎屬性
            existingItem.storeItemId = csvItem.ItemId;
            
            // 調整剩餘購買次數
            if (csvItem.MaxPurchase != -1)
            {
                // 若 CSV 的 MaxPurchase 比存檔的剩餘次數小，則調整
                existingItem.remainingPurchases = Math.Min(
                    existingItem.remainingPurchases,
                    csvItem.MaxPurchase
                );
            }
        }
        else
        {
            // 移除 CSV 中不存在的項目
            existingCategory.items.Remove(existingItem);
        }
    }

    // 添加新項目
    foreach (var csvItem in csvItems)
    {
        if (!existingCategory.items.Any(item => item.entryId == csvItem.Id))
        {
            existingCategory.items.Add(new StoreItemRuntimeData
            {
                entryId = csvItem.Id,
                storeID = csvItem.StoreId,
                storeItemId = csvItem.ItemId,
                remainingPurchases = csvItem.MaxPurchase,
                lastPurchaseTime = DateTime.MinValue
            });
        }
    }
}
    
    /// <summary>
    /// 從存檔載入資料
    /// </summary>
    /// <param name="savedData"></param>
    public void LoadFromSave(AllStoresRuntimeData savedData)
    {
        AllStoresRuntimeData = savedData;
        
        // // 驗證資料完整性
        // foreach (var store in AllStoresRuntimeData.stores)
        // {
        //     foreach (var category in store.categoryGroups)
        //     {
        //         category.items.RemoveAll(item => 
        //             GameContainer.Get<DataManager>()
        //                 .GetDataByID<StoreDataBaseTemplete>(item.storeItemId) == null);
        //     }
        // }
    }
    
    #endregion
    
    /// <summary>
    /// 設置當前選擇商店物品
    /// </summary>
    /// <param name="storeItemData"></param>
    private void SetCurrentSelectedStoreItem(StoreItemRuntimeData storeItemData)
    {
        curClickStoreItemData = storeItemData;
    }
    
    /// <summary>
    /// 檢查是否有足夠的金錢購買物品
    /// </summary>
    /// <param name="storeItemData">商店物品資料</param>
    /// <param name="quantity">購買數量</param>
    /// <returns>是否可以購買</returns>
    public bool CanPurchase(StoreItemRuntimeData storeItemData, int quantity)
    {
        // 基本 null 檢查（保持不變）
        if (storeItemData == null || storeItemData.BaseTemplete == null)
        {
            Debug.LogError("Invalid store item data");
            return false;
        }

        // 計算總成本
        int totalCost = storeItemData.BaseTemplete.ItemBasePrice * quantity;

        // 條件 1：剩餘購買次數檢查
        bool isPurchaseLimitValid = storeItemData.remainingPurchases == -1 // 無限購買
                                    || (storeItemData.remainingPurchases >= quantity); // 有限且足夠

        // 條件 2：玩家貨幣足夠
        bool isCurrencyEnough = InventoryManager.Instance.PlayerMoney.quantity >= totalCost;

        // 需同時滿足兩個條件
        return isPurchaseLimitValid && isCurrencyEnough;
    }

    /// <summary>
    /// 購買物品（可以是單一或複數）
    /// </summary>
    /// <param name="storeItemData">商店物品資料</param>
    /// <param name="quantity">購買數量</param>
    public void PurchaseItem(StoreItemRuntimeData storeItemData, int quantity = 1)
    {
        if (storeItemData == null)
        {
            Debug.LogWarning("無效的物品資料");
            return;
        }

        // 檢查是否可以購買
        if (CanPurchase(storeItemData, quantity))
        {
            int totalCost = storeItemData.BaseTemplete.ItemBasePrice * quantity;

            // 扣除金錢並添加物品
            InventoryManager.Instance.RemoveItem(InventoryManager.Instance.PlayerMoney.itemId, totalCost);
            InventoryManager.Instance.AddItem(storeItemData.ItemBaseTemplete.Id, storeItemData.BaseTemplete.ItemQuantity * quantity);

            GameManager.Instance.MainGameEvent.Send(new PurchaseItemClickedEvent());  
            Debug.Log("購買成功！");
            
            // 更新剩餘次數（若非無限）
            if (storeItemData.remainingPurchases != -1)
            {
                storeItemData.remainingPurchases -= quantity;
            }
            
            storeItemData.lastPurchaseTime = DateTime.Now;
        }
        else
        {
            Debug.Log("金錢不足無法購買");
        }
    }

    // 售卖物品
    public void SellItem(string itemName, int sellPrice)
    {
        // 假设售卖的物品都存在，不需要在商店列表中进行验证
        InventoryManager.Instance.RemoveItem(100, sellPrice);
        Debug.Log($"Sold {itemName} for {sellPrice} gold. New currency: {InventoryManager.Instance.PlayerMoney.quantity}");

        // 更新UI或者玩家的库存 (inventory)，移除已出售的物品
    }
}

[Serializable]
public class StoreItemRuntimeData
{
    public int entryId;
    public int storeID;
    public int storeItemId;     // 對應 StoreDataBaseTemplete.Id
    public int remainingPurchases;// 剩餘可購買次數（動態）
    public DateTime lastPurchaseTime; // 最後購買時間（用於補貨）

    public StoreDataBaseTemplete BaseTemplete => GameContainer.Get<DataManager>().GetDataByID<StoreDataBaseTemplete>(entryId);
    public ItemDataBaseTemplete ItemBaseTemplete => GameContainer.Get<DataManager>().GetDataByID<ItemDataBaseTemplete>(BaseTemplete.ItemId);
}

[Serializable]
public class StoreCategoryGroup
{
    public string categoryName;                          // 商品分類名稱（如 "武器", "道具"）
    public List<StoreItemRuntimeData> items = new();     // 該分類下的商品們
}

[Serializable]
public class StoreRuntimeData
{
    public int storeId;                                // 商店 ID
    public List<StoreCategoryGroup> categoryGroups = new(); // 該商店下的所有分類商品
}

[Serializable]
public class AllStoresRuntimeData
{
    public List<StoreRuntimeData> stores = new();      // 所有商店的狀態資料
}