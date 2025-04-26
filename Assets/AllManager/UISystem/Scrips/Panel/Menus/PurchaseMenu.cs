using Gamemanager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class PurchaseMenu : BasePanel
    {
        [Header("設定通用按鈕")]
        [SerializeField] Button Btn_Purchase;
        [SerializeField] Button Btn_Close;

        [Header("商店物品資訊")]
        [SerializeField] StoreItemRuntimeData storeItemData;

        [SerializeField] Image Image_StoreItem_Icon;
        [SerializeField] TMP_Text Text_StoreItem_Name;
        [SerializeField] TMP_Text Text_StoreItem_Description;
        [SerializeField] TMP_Text Text_StoreItme_PurchaseInfo;
        [SerializeField] TMP_Text Text_StoreItem_Price;
        [SerializeField] TMP_Text Text_StoreItem_PriceSum;

        private int purchase_Quantity = 1;
        [SerializeField] TMP_Text Text_Purchase_Quantity;
        [SerializeField] Button Btn_Min;
        [SerializeField] Button Btn_Decrease;
        [SerializeField] Button Btn_Add;
        [SerializeField] Button Btn_Max;

        const int MaxPurchaseLimit = 100;

        protected override void Awake()
        {
            base.Awake();

            //設定通用按鈕
            InitializeCommonButtons();

            InitializePurchaseQuantity();
        }

        /// <summary>
        /// 介面初始化
        /// </summary>
        /// <param name="storeItemData"></param>
        public void Initialize(StoreItemRuntimeData storeItemData)
        {
            this.storeItemData = storeItemData;

            Image_StoreItem_Icon.sprite = storeItemData.ItemBaseTemplete.ItemIconPath;

            Text_StoreItem_Name.text = storeItemData.ItemBaseTemplete.Name;
            Text_StoreItem_Description.text = storeItemData.ItemBaseTemplete.ItemDescription;
            Text_StoreItme_PurchaseInfo.text = storeItemData.ItemBaseTemplete.Name+ "     *" + storeItemData.BaseTemplete.ItemQuantity;
            Text_StoreItem_Price.text = storeItemData.BaseTemplete.ItemBasePrice.ToString();

            SetPurchaseQuantity(1);
            
        }

        /// <summary>
        /// 當購買數量變更時觸發，用於調整UI的設置
        /// </summary>
        void UpdatePurchase_Quantity()
        {
            Text_Purchase_Quantity.text = purchase_Quantity.ToString();
            Text_StoreItem_PriceSum.text = (storeItemData.BaseTemplete.ItemBasePrice * purchase_Quantity).ToString();

            Btn_Purchase.interactable = StoreManager.Instance.CanPurchase(storeItemData, purchase_Quantity);
        }

        /// <summary>
        /// 初始化通用按鈕
        /// </summary>
        void InitializeCommonButtons()
        {
            Btn_Purchase.onClick.AddListener(() => OnBtn_Purchase());
            Btn_Close.onClick.AddListener(() => OnBtn_Cancel());
        }

        /// <summary>
        /// 初始化購買選單的物品數量介面
        /// </summary>
        void InitializePurchaseQuantity()
        {
            Btn_Min.onClick.AddListener(() => OnBtn_Min());
            Btn_Decrease.onClick.AddListener(() => OnBtn_Decrease());
            Btn_Add.onClick.AddListener(() => OnBtn_Add());
            Btn_Max.onClick.AddListener(() => OnBtn_Max());
        }
        //設定通用按鈕
        void OnBtn_Purchase()
        {
            Debug.Log("Click Btn_Purchase");

            StoreManager.Instance.PurchaseItem(storeItemData, purchase_Quantity);
        }

        void OnBtn_Cancel()
        {
            Debug.Log("Click Btn_Cancel");
            gameObject.SetActive(false);
        }

        //設置購買選單的物品數量介面按鈕
        void OnBtn_Min()
        {
            Debug.Log("Click Btn_Min");
            SetPurchaseQuantity(1);
        }

        void OnBtn_Decrease()
        {
            Debug.Log("Click Btn_Decrease");
            if (purchase_Quantity > 1)
            {
                AdjustPurchaseQuantity(-1);
            }
        }

        void OnBtn_Add()
        {
            Debug.Log("Click Btn_Add");
            if (StoreManager.Instance.CanPurchase(storeItemData, purchase_Quantity + 1))
            {
                AdjustPurchaseQuantity(1);
            }
        }

        void OnBtn_Max()
        {
            Debug.Log("Click Btn_Max");
            int playerMoney = InventoryManager.Instance.PlayerMoney.quantity;
            int MaxAffordableQuantity = playerMoney / storeItemData.BaseTemplete.ItemBasePrice;

            int maxQuantity = Mathf.Min(MaxPurchaseLimit, MaxAffordableQuantity);
            SetPurchaseQuantity(maxQuantity);
        }

        /// <summary>
        /// 設定購買數量
        /// </summary>
        /// <param name="quantity">設定的數量</param>
        void SetPurchaseQuantity(int quantity)
        {
            purchase_Quantity = Mathf.Clamp(quantity, 1, MaxPurchaseLimit);
            UpdatePurchase_Quantity();
        }

        /// <summary>
        /// 調整採購數量
        /// </summary>
        /// <param name="adjustment">增加或減少購買的數量</param>
        void AdjustPurchaseQuantity(int adjustment)
        {
            int newQuantity = purchase_Quantity + adjustment;
            SetPurchaseQuantity(newQuantity);
        }
    }
}