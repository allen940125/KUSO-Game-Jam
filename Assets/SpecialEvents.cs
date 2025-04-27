using Game.Audio;
using UnityEngine;
using System.Collections;
using Game.UI;
using UnityEngine.SceneManagement;

public class SpecialEvents : Singleton<SpecialEvents>
{
    [Header("特別事件Prefab們")]
    [SerializeField] private GameObject eventPrefab1;
    [SerializeField] private GameObject eventPrefab2;
    [SerializeField] private GameObject eventPrefab3;
    [SerializeField] private GameObject eventPrefab4;
    [SerializeField] private GameObject eventPrefab5;

    [Header("特別事件Audio們")]
    [SerializeField] private AudioData eventAudio1;
    [SerializeField] private AudioData eventAudio2;
    [SerializeField] private AudioData eventAudio3;
    [SerializeField] private AudioData eventAudio4;
    [SerializeField] private AudioData eventAudio5;

    private Transform spawnRoot;

    private void Awake()
    {
        base.Awake(); // 確保Singleton正常運作
        spawnRoot = new GameObject("SpecialEventsRoot").transform;
    }

    public void TriggerEvent1()
    {
        StartCoroutine(TriggerEvent1Coroutine());
    }

    public void TriggerEvent2()
    {
        StartCoroutine(TriggerEvent2Coroutine());
    }

    public void TriggerEvent3()
    {
        StartCoroutine(TriggerEvent3Coroutine());
    }

    public void TriggerEvent4()
    {
        StartCoroutine(TriggerEvent4Coroutine());
    }

    public void TriggerEvent5()
    {
        StartCoroutine(TriggerEvent5Coroutine());
    }

    private IEnumerator TriggerEvent1Coroutine()
    {
        Instantiate(eventPrefab1, spawnRoot);
        AudioManager.Instance.PlayRandomSFX(eventAudio1);

        yield return new WaitForSeconds(3.5f); // Event1特別長一點

        Debug.Log("事件1音效結束，執行後續操作（例如切到MainMenu）");
        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator TriggerEvent2Coroutine()
    {
        Instantiate(eventPrefab2, spawnRoot);
        AudioManager.Instance.PlayRandomSFX(eventAudio2);

        yield return new WaitForSeconds(1.5f);

        Debug.Log("事件2音效結束，可以做自己的後續操作");
        // 例如：開啟某個UI
        // GameManager.Instance.UIManager.OpenPanel<SomeUIPanel>();
    }

    private IEnumerator TriggerEvent3Coroutine()
    {
        Instantiate(eventPrefab3, spawnRoot);
        AudioManager.Instance.PlayRandomSFX(eventAudio3);

        yield return new WaitForSeconds(1.5f);

        Debug.Log("事件3音效結束，可以做自己的後續操作");
    }

    private IEnumerator TriggerEvent4Coroutine()
    {
        Instantiate(eventPrefab4, spawnRoot);
        AudioManager.Instance.PlayRandomSFX(eventAudio4);

        yield return new WaitForSeconds(1.5f);

        Debug.Log("事件4音效結束，可以做自己的後續操作");
    }

    private IEnumerator TriggerEvent5Coroutine()
    {
        Instantiate(eventPrefab5, spawnRoot);
        AudioManager.Instance.PlayRandomSFX(eventAudio5);

        yield return new WaitForSeconds(1.5f);

        Debug.Log("事件5音效結束，可以做自己的後續操作");
    }
}
