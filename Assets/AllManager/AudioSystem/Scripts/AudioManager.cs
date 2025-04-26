using Gamemanager;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Game.Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        [Header("必須資料")]
        [SerializeField] AudioSource ambientPlayer;
        [SerializeField] AudioSource musicPlayer;
        [SerializeField] AudioSource sFXPlayer;
        [SerializeField] AudioSource uIPlayer;
        
        [Header("音效數值")]
        public AudioSettingsData CurrentAudioSettingsData => SaveManager.Instance.CurrentSettings.AudioData;
        
        [Header("各場景Audio配置")]
        [SerializeField] AudioConfig audioConfig; // 配置所有UI的ScriptableObject

        const float MIN_PITCH = 0.9f;
        const float MAX_PITCH = 1.1f;

        protected override void Awake()
        {
            base.Awake();
        }

        private void OnEnable()
        {
            GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSceneLoadedEvent, OnSceneLoadedEvent);
        }
        
        private void OnDisable()
        {
            if (GameManager.Instance != null && GameManager.Instance.MainGameEvent != null)
            {
                GameManager.Instance.MainGameEvent.Unsubscribe<SceneLoadedEvent>(OnSceneLoadedEvent);
            }
        }

        #region 事件訂閱

        private void OnSceneLoadedEvent(SceneLoadedEvent cmd)
        {
            LoadSceneAudioConfig();
        }

        #endregion

        #region 封裝的音量控制
        public float MasterVolume
        {
            get => CurrentAudioSettingsData.MasterVolume;
            set
            {
                CurrentAudioSettingsData.MasterVolume = value;
                UpdateAllVolumes();
                SaveManager.Instance.SaveSettings();
            }
        }
        
        public float AmbientVolume
        {
            get => CurrentAudioSettingsData.AmbientVolume;
            set
            {
                CurrentAudioSettingsData.AmbientVolume = value;
                UpdateAllVolumes();
                SaveManager.Instance.SaveSettings();
            }
        }
        
        public float MusicVolume
        {
            get => CurrentAudioSettingsData.MusicVolume;
            set
            {
                CurrentAudioSettingsData.MusicVolume = value;
                UpdateAllVolumes();
                SaveManager.Instance.SaveSettings();
            }
        }
        
        public float SFXVolume
        {
            get =>CurrentAudioSettingsData.SFXVolume;
            set
            {
                CurrentAudioSettingsData.SFXVolume = value;
                UpdateAllVolumes();
                SaveManager.Instance.SaveSettings();
            }
        }
        
        public float UIVolume
        {
            get => CurrentAudioSettingsData.UIVolume;
            set
            {
                CurrentAudioSettingsData.UIVolume = value;
                UpdateAllVolumes();
                SaveManager.Instance.SaveSettings();
            }
        }
        
        #endregion

        /// <summary>
        /// 播放隨機音效，並且加上隨機音調
        /// </summary>
        private void PlayRandomSoundInternal(AudioSource player, AudioData audioData)
        {
            player.pitch = Random.Range(MIN_PITCH, MAX_PITCH);
            PlaySoundInternal(player, audioData);
        }

        /// <summary>
        /// 撥放音效的核心邏輯
        /// </summary>
        private void PlaySoundInternal(AudioSource player, AudioData audioData)
        {
            GameObject audioSourceObj = new GameObject("AudioSource");
            audioSourceObj.transform.SetParent(player.transform);

            AudioSource audioSource = audioSourceObj.AddComponent<AudioSource>();
            audioSource.clip = audioData.audioClip;
            audioSource.volume = player == sFXPlayer ? SFXVolume : (player == uIPlayer ? UIVolume : 1f); // 確保音量設置正確
            audioSource.Play();

            Destroy(audioSourceObj, audioData.audioClip.length);
        }

        #region 音量更新方法
        private void UpdateAllVolumes()
        {
            ambientPlayer.volume = CurrentAudioSettingsData.AmbientVolume * MasterVolume;
            musicPlayer.volume = CurrentAudioSettingsData.MusicVolume * MasterVolume;
            sFXPlayer.volume = CurrentAudioSettingsData.SFXVolume * MasterVolume;
            uIPlayer.volume = CurrentAudioSettingsData.UIVolume * MasterVolume;
        }
        
        #endregion
        
        #region Ambient音效
        
        /// <summary>
        /// 播放環境音效
        /// </summary>
        /// <param name="audioData"></param>
        /// <param name="loop"></param>
        public void PlayAmbient(AudioData audioData, bool loop = true)
        {
            ambientPlayer.clip = audioData.audioClip;
            ambientPlayer.loop = loop;
            ambientPlayer.Play();
        }
        
        #endregion
        
        #region PlayMusic音效
        
        /// <summary>
        /// 撥放Music音效
        /// </summary>
        /// <param name="audioData"></param>
        public void PlayMusic(AudioData audioData)
        {
            musicPlayer.clip = audioData.audioClip;
            musicPlayer.volume = MusicVolume * MasterVolume;
            musicPlayer.Play();
        }
        
        #endregion
        
        #region SFX音效

        /// <summary>
        /// 撥放音效
        /// </summary>
        public void PlaySFX(AudioData audioData)
        {
            PlaySoundInternal(sFXPlayer, audioData);
        }

        /// <summary>
        /// 播放隨機音效（帶音調隨機化）
        /// </summary>
        public void PlayRandomSFX(AudioData audioData)
        {
            PlayRandomSoundInternal(sFXPlayer, audioData);
        }

        /// <summary>
        /// 隨機撥放音效
        /// </summary>
        /// <param name="audioDatas"></param>
        public void PlayRandomSFX(AudioData[] audioDatas)
        {
            AudioData randomAudioData = audioDatas[Random.Range(0, audioDatas.Length)];
            PlayRandomSoundInternal(sFXPlayer, randomAudioData);
        }

        #endregion

        #region UI音效

        /// <summary>
        /// 撥放UI音效
        /// </summary>
        public void PlayUISound(AudioData audioData)
        {
            PlaySoundInternal(uIPlayer, audioData);
        }

        /// <summary>
        /// 播放隨機UI音效（帶音調隨機化）
        /// </summary>
        public void PlayRandomUISound(AudioData audioData)
        {
            PlayRandomSoundInternal(uIPlayer, audioData);
        }

        /// <summary>
        /// 隨機撥放UI音效
        /// </summary>
        /// <param name="audioDatas"></param>
        public void PlayRandomUISound(AudioData[] audioDatas)
        {
            AudioData randomAudioData = audioDatas[Random.Range(0, audioDatas.Length)];
            PlayRandomSoundInternal(uIPlayer, randomAudioData);
        }

        #endregion
        
        /// <summary>
        /// 載入場景時載入Audio
        /// </summary>
        private void LoadSceneAudioConfig()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;

            // 從 InputConfig 中獲取當前場景的輸入配置
            AudioConfig.SceneAudio sceneAudio = audioConfig.GetAudioDataForScene(currentSceneName);

            if (sceneAudio != null)
            {
                foreach (AudioData audioData in sceneAudio.startBGMData)
                {
                    // 根據你的邏輯設置 ActionsMap
                    Debug.Log($"Play BGM Sound: {audioData}");
                    PlayMusic(audioData);
                }

                foreach (AudioData audioData in sceneAudio.startSFXData)
                {
                    // 根據你的邏輯設置 ActionsMap
                    Debug.Log($"Play SFX Sound: {audioData}");
                    PlaySFX(audioData);
                }
            }
            else
            {
                Debug.LogWarning($"No AudioConfig for scene: {currentSceneName}");
            }
        }
    }

    [System.Serializable]
    public class AudioData
    {
        public AudioClip audioClip;
    }
}
