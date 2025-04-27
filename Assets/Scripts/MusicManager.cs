using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance; 
    public AudioClip[] musicClips;        
    private AudioSource audioSource;     

    private void Awake()
    {
        // 保證只有一個MusicManager存在
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 切場景不消失
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (musicClips.Length > 0)
        {
            PlayMusic(0); // 開始時播放第一首
        }
    }

    // 給其他腳本呼叫的API，播放指定編號的音樂
    public void PlayMusic(int index)
    {
        if (index >= 0 && index < musicClips.Length)
        {
            audioSource.clip = musicClips[index];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Music index超出範圍：" + index);
        }
    }

    // 給其他腳本呼叫的API，停止音樂
    public void StopMusic()
    {
        audioSource.Stop();
    }
}