using System;
using System.Collections.Generic;

[Serializable]
public class GameSaveData
{
    // 背包數據
    public InventoryRuntimeData InventoryData = new();

    // 商店數據
    
    public AllStoresRuntimeData AllStoresData = new();

    // 其他系統的數據...
}

[Serializable]
public class GameSettingsData
{
    public AudioSettingsData  AudioData = new();
    
}

[Serializable]
public class AudioSettingsData
{
    // 音訊設定
    public float MasterVolume = 0.5f;
    public float AmbientVolume = 0.5f;
    public float MusicVolume = 0.5f;
    public float SFXVolume = 0.5f;
    public float UIVolume = 0.5f;
}



// // 圖形設定
// public int QualityLevel = 0; // Unity QualitySettings 等級
// public int ResolutionIndex = 0; // 解析度選項索引
// public bool IsFullscreen = true;
//
// // 遊戲設定
// public float MouseSensitivity = 1.0f;
// public bool InvertYAxis = false;
// public string LanguageCode = "en"; // 語言代碼

// // 控制設定（可擴展）
// public Dictionary<string, KeyCode> KeyBindings = new()
// {
//     {"MoveForward", KeyCode.W},
//     {"MoveBack", KeyCode.S}
// };