using Game.Audio;
using Game.SceneManagement;
using Gamemanager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class SettingsWindow : BasePanel
    {
        [Header("設定通用按鈕")]
        [SerializeField] Button applicationButton;
        [SerializeField] Button closeButton;
        [SerializeField] Button returnToMainMenuButton;

        [Header("設定類別按鈕")]
        [SerializeField] Button graphicsTabButton;
        [SerializeField] Button soundTabButton;
        [SerializeField] Button controlsTabButton;

        [Header("設定音效介面")]
        [SerializeField] GameObject masterVolumeSlider;
        [SerializeField] GameObject ambientVolumeSlider;
        [SerializeField] GameObject musicVolumeSlider;
        [SerializeField] GameObject sfxVolumeSlider;
        [SerializeField] GameObject uiVolumeSlider;

        protected override void Awake()
        {
            base.Awake();

            GameManager.Instance.MainGameEvent.Send(new CursorToggledEvent() { ShowCursor = true });
            
            // 初始化音樂界面數值
            InitializeSoundMenu();

            // 設定通用按鈕
            InitializeCommonButtons();
            // 設定類別按鈕
            InitializeCategoryButtons();
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            GameManager.Instance.MainGameEvent.Send(new CursorToggledEvent() { ShowCursor = false });
        }

        /// <summary>
        /// 初始化音效介面
        /// </summary>
        void InitializeSoundMenu()
        {
            //更新音效數值Text
            OnMasterVolumeChanged(AudioManager.Instance.MasterVolume);
            OnAmbientVolumeChanged(AudioManager.Instance.AmbientVolume);
            OnMusicVolumeChanged(AudioManager.Instance.MusicVolume);
            OnSFXVolumeChanged(AudioManager.Instance.SFXVolume);
            OnUIVolumeChanged(AudioManager.Instance.UIVolume);

            // 添加滑輪變化監聽器
            masterVolumeSlider.GetComponentInChildren<Slider>().onValueChanged.AddListener(OnMasterVolumeChanged);
            ambientVolumeSlider.GetComponentInChildren<Slider>().onValueChanged.AddListener(OnAmbientVolumeChanged);
            musicVolumeSlider.GetComponentInChildren<Slider>().onValueChanged.AddListener(OnMusicVolumeChanged);
            sfxVolumeSlider.GetComponentInChildren<Slider>().onValueChanged.AddListener(OnSFXVolumeChanged);
            uiVolumeSlider.GetComponentInChildren<Slider>().onValueChanged.AddListener(OnUIVolumeChanged);
        }

        /// <summary>
        /// 初始化通用按鈕
        /// </summary>
        void InitializeCommonButtons()
        {
            applicationButton.onClick.AddListener(OnApplicationButtonClicked);
            closeButton.onClick.AddListener(OnCloseButtonClicked);
            returnToMainMenuButton.onClick.AddListener(OnReturnToMainMenuButtonClicked);
        }

        /// <summary>
        /// 初始化類別按鈕
        /// </summary>
        void InitializeCategoryButtons()
        {
            graphicsTabButton.onClick.AddListener(OnGraphicsTabButtonClicked);
            soundTabButton.onClick.AddListener(OnSoundTabButtonClicked);
            controlsTabButton.onClick.AddListener(OnControlsTabButtonClicked);
        }

        //設定通用按鈕

        void OnApplicationButtonClicked()
        {
            Debug.Log("Click OnApplicationButtonClicked");
            AudioManager.Instance.PlayUISound(audio_NormalBtn);
            GameManager.Instance.SceneTransitionManager.LoadScene(SceneType.TestGameScene1);
        }

        void OnCloseButtonClicked()
        {
            Debug.Log("Click OnCloseButtonClicked");
            AudioManager.Instance.PlayUISound(audio_NormalBtn);
            ClosePanel();
        }

        void OnReturnToMainMenuButtonClicked()
        {
            Debug.Log("Click OnReturnToMainMenuButtonClicked");
            AudioManager.Instance.PlayUISound(audio_NormalBtn);
            GameManager.Instance.SceneTransitionManager.LoadScene(SceneType.MainMenuScene);
        }

        //設定類別按鈕
        void OnGraphicsTabButtonClicked()
        {
            Debug.Log("Click OnGraphicsTabButtonClicked");
            AudioManager.Instance.PlayUISound(audio_NormalBtn);
        }

        void OnSoundTabButtonClicked()
        {
            Debug.Log("Click OnSoundTabButtonClicked");
            AudioManager.Instance.PlayUISound(audio_NormalBtn);
        }

        void OnControlsTabButtonClicked()
        {
            Debug.Log("Click OnControlsTabButtonClicked");
            AudioManager.Instance.PlayUISound(audio_NormalBtn);
        }

        //設定音效界面
        void OnMasterVolumeChanged(float value)
        {
            AudioManager.Instance.MasterVolume = value;
            
            masterVolumeSlider.GetComponentInChildren<Slider>().value = value;
            //設置音效數值Text
            TMP_Text volume = masterVolumeSlider.transform.Find("Text_Volume").GetComponent<TMP_Text>();
            // 將 value 轉換為百分比格式
            int percentage = Mathf.RoundToInt(value * 100);
            volume.text = percentage.ToString() + "%";
        }
        
        void OnAmbientVolumeChanged(float value)
        {
            AudioManager.Instance.AmbientVolume = value;
            
            ambientVolumeSlider.GetComponentInChildren<Slider>().value = value;
            //設置音效數值Text
            TMP_Text volume = ambientVolumeSlider.transform.Find("Text_Volume").GetComponent<TMP_Text>();
            // 將 value 轉換為百分比格式
            int percentage = Mathf.RoundToInt(value * 100);
            volume.text = percentage.ToString() + "%";
        }
        
        void OnMusicVolumeChanged(float value)
        {
            AudioManager.Instance.MusicVolume = value;
            
            musicVolumeSlider.GetComponentInChildren<Slider>().value = value;
            //設置音效數值Text
            TMP_Text volume = musicVolumeSlider.transform.Find("Text_Volume").GetComponent<TMP_Text>();
            // 將 value 轉換為百分比格式
            int percentage = Mathf.RoundToInt(value * 100);
            volume.text = percentage.ToString() + "%";
        }
        
        void OnSFXVolumeChanged(float value)
        {
            AudioManager.Instance.SFXVolume = value;

            sfxVolumeSlider.GetComponentInChildren<Slider>().value = value;
            //設置音效數值Text
            TMP_Text volume = sfxVolumeSlider.transform.Find("Text_Volume").GetComponent<TMP_Text>();
            // 將 value 轉換為百分比格式
            int percentage = Mathf.RoundToInt(value * 100);
            volume.text = percentage.ToString() + "%";
        }

        void OnUIVolumeChanged(float value)
        {
            AudioManager.Instance.UIVolume = value;

            uiVolumeSlider.GetComponentInChildren<Slider>().value = value;
            //設置音效數值Text
            TMP_Text volume = uiVolumeSlider.transform.Find("Text_Volume").GetComponent<TMP_Text>();
            // 將 value 轉換為百分比格式
            int percentage = Mathf.RoundToInt(value * 100);
            volume.text = percentage + "%";
        }
    }
}
