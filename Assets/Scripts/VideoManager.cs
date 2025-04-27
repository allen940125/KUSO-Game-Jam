using UnityEngine;
using UnityEngine.Video;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class VideoManager : MonoBehaviour
{
    [System.Serializable]
    public class VideoEntry
    {
        public string videoName;     // 影片名稱（自己取）
        public VideoClip clip;       // 對應的影片
    }

    public VideoPlayer videoPlayer;     // 拖進來的VideoPlayer
    public List<VideoEntry> videos;      // 影片列表


    private Dictionary<string, VideoClip> videoDict;

    private void Awake()
    {
        // 建立影片字典（快速查找用）
        videoDict = new Dictionary<string, VideoClip>();
        foreach (var entry in videos)
        {
            videoDict[entry.videoName] = entry.clip;
        }
    }

    private void Start()
    {
        videoPlayer.targetCameraAlpha = 0f;
    }

    /// <summary>
    /// 播放影片
    /// </summary>
    public void PlayVideo(string videoName, bool withFlick)
    {
        if (videoDict.TryGetValue(videoName, out VideoClip clip))
        {
            if (withFlick == false)
            {
                StartCoroutine(PlayNormalVideo(clip));
            }
            else if (withFlick == true)
            {
                StartCoroutine(PlayWithFlicker(clip));
            }
            
        }
        else
        {
            Debug.LogWarning($"找不到影片：{videoName}");
        }
    }

    private IEnumerator PlayNormalVideo(VideoClip clip)
    {
        videoPlayer.clip = clip;
        videoPlayer.targetCameraAlpha = 1f;
        videoPlayer.Play();

        // 等待影片播放結束
        yield return new WaitForSeconds((float)clip.length);

        videoPlayer.targetCameraAlpha = 0f; // 播完自動隱藏
        
        SceneManager.LoadScene("MainMenu");
    }   

    private IEnumerator PlayWithFlicker(VideoClip clip)
    {
        videoPlayer.clip = clip;
        videoPlayer.Play();
    
        float flickerTime = 2f;         // 閃爍總時長
        float elapsed = 0f;
        float flickerInterval = 0.1f;   // 每次閃爍間隔

        while (elapsed < flickerTime)
        {
            videoPlayer.targetCameraAlpha = Random.value > 0.5f ? 1f : 0f;  // 隨機出現或消失
            elapsed += flickerInterval;
            yield return new WaitForSeconds(flickerInterval);
        }

        // 閃爍完，正常播放
        videoPlayer.targetCameraAlpha = 1f;

        yield return new WaitForSeconds((float)clip.length - flickerTime);

        videoPlayer.targetCameraAlpha = 0f; // 隱藏
        PlayVideo("POPOPO",false);
        
    }
}
