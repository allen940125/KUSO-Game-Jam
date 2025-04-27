using System.Collections;
using Game.Audio;
using Game.Input;
using Game.SceneManagement;
using UnityEngine;
using Game.UI;
using Gamemanager;

public class GameManager : Singleton<GameManager>
{
    public MainGameEventPack MainGameEvent { get; private set; } = new MainGameEventPack();
    
    [SerializeField] private MainGameMediator mainGameMediator;
    public MainGameMediator MainGameMediator 
    { 
        get => mainGameMediator; 
        private set => mainGameMediator = value; 
    }
    
    [SerializeField] private GameObject player;
    
    [SerializeField] private AudioData _audioData;
    
    public GameObject Player 
    { 
        get => player; 
        set => player = value; 
    }
    
    public SceneTransitionManager SceneTransitionManager { get; private set; } = new SceneTransitionManager();
    public TransitionUIManager TransitionUIManager { get; private set; } = new TransitionUIManager();
    public UIManager UIManager { get; private set; } = new UIManager();
    public InputManagers InputManagers { get; private set; } = new InputManagers();

    [Header("遊戲資料")]
    [SerializeField] private GameSo gameSo;
    [SerializeField] TextAsset csvFile;
    public GameSo GameSo 
    { 
        get => gameSo; 
        set => gameSo = value; 
    }
    
    protected override void Awake()
    {
        base.Awake();
        
        Debug.Log("GameManager 初始化");
        
        // 如果 MainGameMediator 不需要從 Inspector 設定，可直接 new
        MainGameMediator = new MainGameMediator();
        MainGameMediator.MainGameMediatorInit();
        
        SceneTransitionManager.Initialize();
        TransitionUIManager.Initialize();
        UIManager.Initialize();
        InputManagers.Initialize();
    }
    
    private void Start()
    {
        GameStart();
    }

    private void Update()
    {
        // 若 UIManager 為 null 可做防呆檢查
        UIManager?.Update();
        
        if (UnityEngine.Input.GetKeyDown(KeyCode.G))
        {
            //TeleportManager.Instance.TeleportWithFade(1);
            AudioManager.Instance.PlaySFX(_audioData);
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.H))
        {
            //TeleportManager.Instance.TeleportWithFade(2);
            // UIManager.OpenPanel<FadeInOutWindow>(UIType.FadeInOutWindow);
            //UIManager.GetPanel<FadeInOutWindow>(UIType.FadeInOutWindow).ExitStory(1,4);
            // UIManager.OpenPanel<StoryTextDisplayWindow>(UIType.StoryTextDisplayWindow);
            
            //UIManager.GetPanel<StoryTextDisplayWindow>(UIType.StoryTextDisplayWindow).StartStory("444444");
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(UIManager.GetPanel<StoryTextDisplayWindow>(UIType.StoryTextDisplayWindow).EndStories());
        }
        else if(UnityEngine.Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.OpenPanel<SettingsWindow>(UIType.SettingsWindow);
        }
    }
    public void SetPlayer(GameObject newPlayer)
    {
        if (newPlayer != null)
        {
            Player = newPlayer;
        }
    }

    public void NewGame()
    {
        // TODO: 實作新遊戲初始化邏輯
    }

    public void GameStart()
    {
        // TODO: 實作遊戲開始流程
    }
    
    public void GameOver()
    {
        //UIManager.OpenPanel(UIType.GameOverMenu);
    }
    
    public void StartCoroutineFromManager(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    public GameObject InstantiateFromManager(GameObject prefab, Transform parent = null, bool instantiateInWorldSpace = false)
    {
        return Instantiate(prefab, parent, instantiateInWorldSpace);
    }

    private void OnDestroy()
    {
        // 統一管理各個模組的清理工作
        SceneTransitionManager.Cleanup();
        TransitionUIManager.Cleanup();
        UIManager.Cleanup();
        InputManagers.Cleanup();
    }
}
