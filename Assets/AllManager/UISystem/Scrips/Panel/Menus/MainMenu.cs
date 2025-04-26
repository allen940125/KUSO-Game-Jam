using Game.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class MainMenu : BasePanel
    {
        [Header("設定通用按鈕")]
        [SerializeField] private Button startButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button exitButton;

        [Header("轉換的場景名稱")]
        [SerializeField] private SceneType sceneType;

        protected override void Awake()
        {
            base.Awake();

            //設定通用按鈕
            InitializeCommonButtons();
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        /// <summary>
        /// 初始化通用按鈕
        /// </summary>
        void InitializeCommonButtons()
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
            settingsButton.onClick.AddListener(OnSettingsButtonClicked);
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        //設定通用按鈕
        void OnStartButtonClicked()
        {
            Debug.Log("Click OnStartButtonClicked");
            GameManager.Instance.SceneTransitionManager.LoadScene(sceneType);
        }
        void OnSettingsButtonClicked()
        {
            Debug.Log("Click OnSettingsButtonClicked");
            GameManager.Instance.UIManager.OpenPanel<SettingsWindow>(UIType.SettingsWindow);
        }
        void OnExitButtonClicked()
        {
            Debug.Log("Click OnExitButtonClicked");
#if UNITY_EDITOR
            // 在編輯器中停止播放模式
            UnityEditor.EditorApplication.isPlaying = false;
#else
    // 在執行檔中退出應用程式
    Application.Quit();
#endif
        }

    }
}

