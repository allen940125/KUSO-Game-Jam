using Game.Input;
using Game.Audio;
using UnityEngine;

namespace Game.UI
{
    public class BasePanel : MonoBehaviour
    {
        protected GameObject uiPanel;
        protected bool isRemove = false;
        protected UIType panelType; // 用來儲存傳入的 UIType
        
        public UIGroup Group { get; set; } // 每個 UI Panel 有一個分組

        [Header("當前 UI 是否需要設置成進入 UI 狀態（會調整 InputManagers 的按鍵輸入）")]
        [SerializeField] InputType isSetInputActive;

        [Header("按鈕的基礎音效")]
        [SerializeField] protected AudioData audio_NormalBtn;

        protected virtual void Awake()
        {
            uiPanel = gameObject;
        }

        protected virtual void Start()
        {
            // 設定輸入狀態，可根據需要在面板開啟時調整
            GameManager.Instance.InputManagers.SetInputActive(isSetInputActive);
        }

        protected virtual void OnDestroy()
        {
            // 當 Panel 被銷毀時，通知 UIManager 清除引用
            if (GameManager.Instance != null && GameManager.Instance.UIManager != null)
            {
                GameManager.Instance.UIManager.RemovePanelReference(this);
            }
        }

        /// <summary>
        /// 設定這個 Panel 是否顯示
        /// </summary>
        public virtual void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        /// <summary>
        /// 開啟 Panel 並儲存傳入的 UIType
        /// </summary>
        public virtual void OpenPanel(UIType type)
        {
            panelType = type;
            SetActive(true);
        }

        /// <summary>
        /// 關閉 Panel：
        /// 1. 設置不顯示
        /// 2. 執行銷毀（可改為回收）
        /// 3. 不直接操作 UIManager 的 PanelDict，由 UIManager OnDestroy 處理清除
        /// 4. 如果沒有 Menu 類型的 UI 開啟，恢復 Gameplay 輸入狀態
        /// </summary>
        public virtual void ClosePanel()
        {
            isRemove = true;
            SetActive(false);
            Destroy(gameObject); // 若未來有面板回收系統，可替換成回收邏輯

            // if (!GameManager.Instance.UIManager.HasOpenUIInGroup(UIGroup.Menu))
            // {
            //     Debug.Log("全清除：切回 Gameplay 狀態");
            //     GameManager.Instance.InputManagers.SetInputActive(InputType.Gameplay);
            // }
        }
    }
}
