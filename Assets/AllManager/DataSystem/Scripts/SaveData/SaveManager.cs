using System;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    private readonly string progressFileName = "game_progress";  // 遊戲進度存檔
    private readonly string settingsFileName = "game_settings";  // 設定檔
    
    // 當前存檔數據
    public GameSaveData CurrentSaveData = new();
    public GameSettingsData CurrentSettings = new();
    
    [Header("功能")]

    [SerializeField] private bool restartJson;
    
    private void OnValidate()
    {
        if (restartJson)
        {
            JsonSaveManager.Delete(progressFileName); // 刪除舊存檔
            JsonSaveManager.Delete(settingsFileName); // 刪除舊存檔
        }
    }
    
    private void Awake()
    {
        LoadGame();
        LoadSettings();
        
        // 初始化商店資料
        //ToDo 因為執行順序問題而導致她會有問題
        StoreManager.Instance.LoadFromSave(CurrentSaveData.AllStoresData);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveGame(); // 統一保存存檔
            SaveSettings();
            Debug.Log("已儲存遊戲");
        }
    }

    // 保存存檔
    public void SaveGame()
    {
        // 儲存前先取得最新資料
        CurrentSaveData.AllStoresData = StoreManager.Instance.AllStoresRuntimeData;
        JsonSaveManager.Save(progressFileName, CurrentSaveData);
        Debug.Log("存檔已保存");
    }
    
    // 加載存檔
    public void LoadGame()
    {
        if (JsonSaveManager.SaveFileExists(progressFileName))
        {
            CurrentSaveData = JsonSaveManager.Load<GameSaveData>(progressFileName);
            Debug.Log("存檔加載成功");
        }
        else
        {
            CurrentSaveData = new GameSaveData();
            Debug.Log("新存檔已建立");
        }
    }
    
    // 保存設定檔
    public void SaveSettings()
    {
        JsonSaveManager.Save(settingsFileName, CurrentSettings);
    }
    
    // 加載設定檔
    public void LoadSettings()
    {
        if (JsonSaveManager.SaveFileExists(settingsFileName))
        {
            CurrentSettings = JsonSaveManager.Load<GameSettingsData>(settingsFileName);
            Debug.Log("存檔加載成功");
        }
        else
        {
            CurrentSaveData = new GameSaveData();
            Debug.Log("新存檔已建立");
        }
    }
}