using Gamemanager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class StoreMenu : BasePanel
    {
        [Header("設定通用按鈕")]
        [SerializeField] private Button useButton;
        [SerializeField] private Button closeButton;
        
        [Header("物品分類標籤頁")]
        [SerializeField] private Button equipmentTabButton; 
        [SerializeField] private Button consumableTabButton; 
        [SerializeField] private Button materialTabButton;
        [SerializeField] private Button keyItemTabButton;

        [Header("商店資訊")]
        [SerializeField] GameObject prefabSlotStoreItem;
        [SerializeField] GameObject scrollViewContentStoreItemList;
        [SerializeField] TMP_Text textMoneyVolue;

        [Header("當前選中物品資訊")]
        [SerializeField] private Image selectedItemIcon;
        [SerializeField] private TMP_Text selectedItemName;
        [SerializeField] private TMP_Text selectedItemDescription;
        
        
        protected override void Awake()
        {
            base.Awake();

            //設定通用按鈕
            InitializeCommonButtons();
            //設定類別按鈕
            //InitializeCategoryButtons();

            GameManager.Instance.MainGameEvent.Send(new CursorToggledEvent() { ShowCursor = true});  
            
            // 訂閱事件並保存 IDisposable
            GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnStoreItemClickedEvent, OnStoreItemClickedEvent);
            GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPurchaseItemClickedEvent, OnPurchaseItemClickedEvent);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            GameManager.Instance.UIManager.OpenPanel<GameHUD>(UIType.GameHUD);
            
            // 取消訂閱事件，避免記憶體洩漏
            GameManager.Instance.MainGameEvent.Unsubscribe<StoreItemClickedEvent>(OnStoreItemClickedEvent);
            GameManager.Instance.MainGameEvent.Unsubscribe<PurchaseItemClickedEvent>(OnPurchaseItemClickedEvent);
        }
        
        protected override void Start()
        {
            base.Start();
            Update_StoreItemDataInGrid();
            UpdateMoneyVolueText();
        }
        
        #region 事件訂閱

        void OnStoreItemClickedEvent(StoreItemClickedEvent cmd)
        {
            UpdateClickItemInfo(cmd.StoreItemData);
        }

        void OnPurchaseItemClickedEvent(PurchaseItemClickedEvent cmd)
        {
            UpdateMoneyVolueText();
        }

        #endregion
        
        #region 註冊按鈕事件

        /// <summary>
        /// 初始化通用按鈕
        /// </summary>
        void InitializeCommonButtons()
        {
            useButton.onClick.AddListener(OnUseButtonClicked);
            closeButton.onClick.AddListener(OnCloseButtonClicked);
        }
        /// <summary>
        /// 初始化設定類別按鈕
        /// </summary>
        void InitializeCategoryButtons()
        {
            equipmentTabButton.onClick.AddListener(OnEquipmentTabButtonClicked);
            consumableTabButton.onClick.AddListener(OnConsumableTabButtonClicked);
            materialTabButton.onClick.AddListener(OnMaterialTabButtonClicked);
            keyItemTabButton.onClick.AddListener(OnKeyItemTabButtonClicked);
        }

        //設定通用按鈕
        void OnUseButtonClicked()
        {
            if (InventoryManager.Instance.curClickInventoryItemRuntimeData != null)
            {
                StoreManager.Instance.PurchaseItem(StoreManager.Instance.curClickStoreItemData, 1);
            }
        }

        void OnCloseButtonClicked()
        {
            Debug.Log("Click Btn_Cancel");
            ClosePanel();
        }

        //設定類別按鈕
        void OnEquipmentTabButtonClicked()
        {
            Debug.Log("Click OnEquipmentTabButtonClicked");
            GameManager.Instance.MainGameEvent.Send(new PlayerBagRefreshedEvent() { ItemControllerType = ItemControllerType.Equipment});  
        }

        void OnConsumableTabButtonClicked()
        {
            Debug.Log("Click OnConsumableTabButtonClicked");
            GameManager.Instance.MainGameEvent.Send(new PlayerBagRefreshedEvent() { ItemControllerType = ItemControllerType.Consumable});  
        }
        
        void OnMaterialTabButtonClicked()
        {
            Debug.Log("Click OnMaterialTabButtonClicked");
            GameManager.Instance.MainGameEvent.Send(new PlayerBagRefreshedEvent() { ItemControllerType = ItemControllerType.Material});  
        }

        void OnKeyItemTabButtonClicked()
        {
            Debug.Log("Click OnKeyItemTabButtonClicked");
            GameManager.Instance.MainGameEvent.Send(new PlayerBagRefreshedEvent() { ItemControllerType = ItemControllerType.KeyItem});  
        }

        #endregion

        /// <summary>
        /// 添加商店物品數據至ScrollView_Content
        /// </summary>
        /// <param name="recipeData"></param>
        void Update_StoreItemDataInGrid()
        {
            // 清空現有物品
            foreach (Transform child in scrollViewContentStoreItemList.transform)
            {
                Destroy(child.gameObject);
            }

            // 根據當前商店ID獲取資料
            var targetStore = StoreManager.Instance.AllStoresRuntimeData.stores
                .Find(s => s.storeId == StoreManager.Instance.CurrentStoreId);

            // 安全檢查
            if (targetStore == null || 
                targetStore.categoryGroups.Count == 0 || 
                targetStore.categoryGroups[0].items == null)
            {
                Debug.LogWarning($"找不到商店ID {StoreManager.Instance.CurrentStoreId} 的資料");
                return;
            }

            // 動態生成物品
            foreach (StoreItemRuntimeData storeItemData in targetStore.categoryGroups[0].items)
            {
                Debug.Log(storeItemData.storeItemId);
                GameObject newSlot = Instantiate(prefabSlotStoreItem, scrollViewContentStoreItemList.transform);
                Debug.Log("表格內增加的商店格子資料ID" + storeItemData.storeItemId + "商店物品初始化的StoreItemBaseTemplete為" + storeItemData.BaseTemplete.ItemId);
                newSlot.GetComponent<SlotStoreItem>().Initialize(storeItemData, gameObject);
            }
        }

        /// <summary>
        /// 更新點擊的物品資訊
        /// </summary>
        /// <param name="itemData"></param>
        void UpdateClickItemInfo(StoreItemRuntimeData itemData)
        {
            Debug.Log($"UpdateClickItemInfo called with itemData: {itemData}");
            Debug.Log($"Image component: {selectedItemIcon}");
            Debug.Log($"Text component (Name): {selectedItemName}");
            Debug.Log($"Text component (Description): {selectedItemDescription}");

            if (itemData == null || selectedItemIcon == null || selectedItemName == null || selectedItemDescription == null)
            {
                Debug.LogWarning("Item data or UI elements are null!");
                return;
            }

            selectedItemIcon.sprite = itemData.ItemBaseTemplete.ItemIconPath;
            selectedItemName.text = itemData.ItemBaseTemplete.Name;
            selectedItemDescription.text = itemData.ItemBaseTemplete.ItemDescription;
        }
        
        /// <summary>
        /// 更新金錢文字UI
        /// </summary>
        void UpdateMoneyVolueText()
        {
            textMoneyVolue.text = InventoryManager.Instance.PlayerMoney.quantity.ToString();
        }
    }
}