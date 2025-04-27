using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Cysharp.Threading.Tasks;               // 假設你用 UniTask 做 async
using Datamanager;                          // DataGroup、UIDataBaseTemplete
using Gamemanager;                          // GameManager

namespace Game.UI
{
    public class UIManager
    {
        private Transform _uiRoot;
        public Transform UIRoot => _uiRoot;

        // 快取從 DataManager 拿到的 UI 模板（包含 prefab、UIGroup 等資料）
        private Dictionary<UIType, UIDataBaseTemplete> _templateCache = new();

        // 當前打開的 Panel 實例
        public Dictionary<UIType, BasePanel> PanelDict { get; private set; } = new();

        #region 初始化與清理

        public void Initialize()
        {
            Debug.Log("UIManager + Initialize");
            EnsureUIRootExists();
            PanelDict.Clear();
            _templateCache.Clear();
            // 假設 DataManager 已在更早階段，透過 Addressables 填好所有 UIDataBaseTemplete.PrefabPath

            // 訂閱場景與輸入事件
            GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSceneLoadedEvent, OnSceneLoadedEvent);
            GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnEscapeKeyPressedEvent, OnEscapeKeyPressedEvent);
            GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnOpenBackpackKeyPressedEvent, OnOpenBackpackKeyPressedEvent);
        }

        public void Cleanup()
        {
            Debug.Log("UIManager + Cleanup");
            CloseAllPanels();
            PanelDict.Clear();
            _templateCache.Clear();

            // 取消訂閱事件
            GameManager.Instance.MainGameEvent.Unsubscribe<SceneLoadedEvent>(OnSceneLoadedEvent);
            GameManager.Instance.MainGameEvent.Unsubscribe<EscapeKeyPressedEvent>(OnEscapeKeyPressedEvent);
            GameManager.Instance.MainGameEvent.Unsubscribe<OpenBackpackKeyPressedEvent>(OnOpenBackpackKeyPressedEvent);
        }

        public void Update()
        {
            // 測試用而已：按 S 開啟商店，B 顯示快取與 panel 狀態，D 關閉所有 UI
            if (UnityEngine.Input.GetKeyDown(KeyCode.S))
            {
                OpenPanel<StoreMenu>(UIType.StoreMenu);
            }
        }

        /// <summary>
        /// 確保UIRoot存在
        /// </summary>
        private void EnsureUIRootExists()
        {
            if (_uiRoot == null)
            {
                var canvas = GameObject.Find("Canvas") ?? new GameObject("Canvas");
                _uiRoot = canvas.transform;
            }
        }
        
        #endregion

        #region 事件訂閱與處理

        private void OnSceneLoadedEvent(SceneLoadedEvent cmd)
        {
            // 當新場景載入完成時先關閉所有 UI
            CloseAllPanels();
            LoadSceneUIConfig();
        }

        private void OnEscapeKeyPressedEvent(EscapeKeyPressedEvent cmd)
        {
            OnEscapePressed();
        }

        private void OnOpenBackpackKeyPressedEvent(OpenBackpackKeyPressedEvent cmd)
        {
            OnOpenBackPackPressed();
        }

        /// <summary>
        /// 當按 Escape 時：
        ///     1. 如果有開啟 Menu 群組的 UI，先關閉這些 UI
        ///     2. 否則開啟設置選單
        /// </summary>
        private void OnEscapePressed()
        {
            if (HasOpenUIInGroup(UIGroup.Menu))
            {
                CloseAllPanels(UIGroup.Menu);
            }
            else
            {
                OpenPanel<SettingsWindow>(UIType.SettingsWindow).Forget();
            }
        }

        private void OnOpenBackPackPressed()
        {
            //OpenPanel<BagMenu>(UIType.BagMenu).Forget();
        }

        /// <summary>
        /// 載入場景時的 UI 配置（可根據需求調整）
        /// </summary>
        private void LoadSceneUIConfig()
        {
            // 此處如果你有根據場景配置要預設打開的 UI，可以從配置檔或 DataGroup 中獲取
            // 例如：
            // string currentSceneName = SceneManager.GetActiveScene().name;
            // var uiList = SceneConfigManager.Instance.GetUIPanelsForScene(currentSceneName);
            // foreach (var ui in uiList)
            // {
            //     OpenPanel(ui).Forget();
            // }
        }

        #endregion

        #region 打開與關閉 UI Panel

        /// <summary>
        /// 非同步開啟一個 UI Panel
        /// </summary>
        public async UniTask<T> OpenPanel<T>(UIType uiType) where T : BasePanel
        {
            if (PanelDict.ContainsKey(uiType))
            {
                Debug.LogWarning($"UI already open: {uiType}");
                return PanelDict[uiType] as T;
            }

            var prefab = LoadPanelPrefab(uiType);
            if (prefab == null)
            {
                Debug.LogError($"Cannot load prefab for {uiType}");
                return null;
            }

            // 使用 GameManager 提供的 Instantiate 方法進行實例化
            var go = GameManager.Instance.InstantiateFromManager(prefab, _uiRoot, false);
            if (go == null)
            {
                Debug.LogError("Instantiate failed.");
                return null;
            }

            var panel = go.GetComponent<BasePanel>();
            if (panel == null)
            {
                Debug.LogError($"BasePanel component not found on instantiated prefab for {uiType}");
                return null;
            }
            
            panel.Group = GetUIGroup(uiType);
            PanelDict[uiType] = panel;
            panel.OpenPanel(uiType);
            return panel as T;
        }

        /// <summary>
        /// 關閉指定 UI Panel
        /// </summary>
        public bool ClosePanel(UIType uiType)
        {
            if (!PanelDict.TryGetValue(uiType, out var panel))
                return false;

            panel.ClosePanel();
            // 從 PanelDict 中安全移除該 panel
            RemovePanelReference(panel);
            return true;
        }

        /// <summary>
        /// 關閉所有 UI Panel，會檢查並移除已被銷毀的引用
        /// </summary>
        public void CloseAllPanels()
        {
            // 以安全方式遍歷 PanelDict
            foreach (var kv in PanelDict.Values.ToList())
            {
                if (kv == null || kv.gameObject == null)
                {
                    // 已被銷毀的 panel 直接移除
                    RemovePanelReference(kv);
                }
                else
                {
                    kv.ClosePanel();
                }
            }
            PanelDict.Clear();
        }

        /// <summary>
        /// 關閉指定 UIGroup 中的所有 Panel
        /// </summary>
        public void CloseAllPanels(UIGroup group)
        {
            var panelsToClose = new List<UIType>();

            foreach (var kvp in PanelDict)
            {
                if (kvp.Value.Group == group)
                {
                    panelsToClose.Add(kvp.Key);
                }
            }

            foreach (var uiType in panelsToClose)
            {
                ClosePanel(uiType);
            }
        }

        /// <summary>
        /// 從 PanelDict 中安全移除指定 Panel 的引用
        /// </summary>
        public void RemovePanelReference(BasePanel panel)
        {
            if (panel == null) return;

            var keysToRemove = PanelDict
                .Where(kv => kv.Value == null || kv.Value == panel)
                .Select(kv => kv.Key)
                .ToList();

            foreach (var key in keysToRemove)
            {
                PanelDict.Remove(key);
            }
        }

        #endregion

        #region 載入 Prefab 與 UI 模板

        /// <summary>
        /// 從 DataManager 拿取 UI 模板（UIDataBaseTemplete），並快取結果
        /// </summary>
        private UIDataBaseTemplete LoadPanelTemplate(UIType uiType)
        {
            if (_templateCache.TryGetValue(uiType, out var tpl))
                return tpl;

            // 用 uiType.ToString() 作為 key 從 DataManager 獲取模板
            string key = uiType.ToString();
            tpl = GameContainer.Get<DataManager>().GetDataByName<UIDataBaseTemplete>(key);
            if (tpl == null)
            {
                Debug.LogError($"UI template not found: {key}");
                return null;
            }

            _templateCache[uiType] = tpl;
            return tpl;
        }

        /// <summary>
        /// 從 UI 模板中取得已預載好的 Prefab
        /// </summary>
        private GameObject LoadPanelPrefab(UIType uiType)
        {
            var tpl = LoadPanelTemplate(uiType);
            if (tpl == null || tpl.PrefabPath == null)
            {
                Debug.LogError($"PrefabPath missing for UI {uiType}");
                return null;
            }
            return tpl.PrefabPath;
        }

        #endregion

        #region 幫助方法
        
        /// <summary>
        /// 根據 UI 模板中儲存的字串轉換成 UIGroup 枚舉（若無或錯誤則回傳默認值）
        /// </summary>
        private UIGroup GetUIGroup(UIType uiType)
        {
            var tpl = LoadPanelTemplate(uiType);
            if (tpl == null)
            {
                Debug.LogError($"UI template not found for {uiType}");
                return UIGroup.Menu;   // 預設值
            }

            if (string.IsNullOrEmpty(tpl.UIGroup))
            {
                Debug.LogWarning($"UIGroup is empty for {uiType}, fallback to Menu");
                return UIGroup.Menu;
            }

            if (Enum.TryParse<UIGroup>(tpl.UIGroup, ignoreCase: true, out var group))
            {
                return group;
            }
            else
            {
                Debug.LogError($"Invalid UIGroup '{tpl.UIGroup}' for {uiType}, fallback to Menu");
                return UIGroup.Menu;
            }
        }

        public string GetOpenedPanelsDetailedInfo()
        {
            if (PanelDict == null || PanelDict.Count == 0)
            {
                return "目前沒有開啟的 UI Panel";
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=== UI Panel 狀態報告 ===");
            sb.AppendLine($"總數: {PanelDict.Count}");
            sb.AppendLine("------------------------");

            foreach (var kvp in PanelDict)
            {
                sb.AppendLine($"Key: {kvp.Key}");
                sb.AppendLine($"Type: {kvp.Value?.GetType().FullName ?? "null"}");
                sb.AppendLine($"Active: {kvp.Value?.gameObject.activeSelf.ToString() ?? "null"}");
                sb.AppendLine("------------------------");
            }

            return sb.ToString();
        }

        public string GetTemplateCacheDetailedInfo()
        {
            if (_templateCache == null || _templateCache.Count == 0)
            {
                return "目前沒有已快取的 UI Template";
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=== UI Template 狀態報告 ===");
            sb.AppendLine($"總數: {_templateCache.Count}");
            sb.AppendLine("------------------------");

            foreach (var kvp in _templateCache)
            {
                sb.AppendLine($"Key: {kvp.Key}");
                sb.AppendLine($"Type: {kvp.Value?.GetType().FullName ?? "null"}");
                sb.AppendLine("------------------------");
            }

            return sb.ToString();
        }

        #endregion

        #region UI 狀態檢測

        /// <summary>
        /// 檢查指定 UIGroup 是否有開啟的 Panel
        /// </summary>
        public bool HasOpenUIInGroup(UIGroup group)
        {
            return PanelDict.Values.Any(panel => panel != null && panel.Group == group);
        }

        #endregion
        
        public bool IsPanelOpen(UIType type) => PanelDict.ContainsKey(type);

        public T GetPanel<T>(UIType type) where T : BasePanel
        {
            PanelDict.TryGetValue(type, out var panel);
            return panel as T;
        }

        public IEnumerable<BasePanel> GetPanelsByGroup(UIGroup group)
        {
            return PanelDict.Values.Where(p => p.Group == group);
        }
    }
}
