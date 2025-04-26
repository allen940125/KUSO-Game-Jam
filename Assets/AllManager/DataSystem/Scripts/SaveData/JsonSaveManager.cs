using System.IO;
using UnityEngine;

/// <summary>
/// JsonSaveManager 提供了一組靜態方法，用來儲存、讀取、刪除 JSON 格式的資料檔案。
/// 儲存路徑預設在 Application.persistentDataPath 下的 "Saves" 資料夾中。
/// </summary>
public static class JsonSaveManager
{
    // 儲存存檔的資料夾路徑
    private static string saveFolder = Path.Combine(Application.persistentDataPath, "Saves");

    /// <summary>
    /// 儲存資料到 JSON 檔案中。
    /// </summary>
    /// <typeparam name="T">要儲存的資料類型。</typeparam>
    /// <param name="fileName">檔案名稱（不含副檔名）。</param>
    /// <param name="data">資料內容。</param>
    public static void Save<T>(string fileName, T data)
    {
        // 若存檔資料夾不存在，則先建立
        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
        }

        // 將資料轉換成 JSON 字串（使用 prettyPrint 方便檢查）
        string json = JsonUtility.ToJson(data, prettyPrint: true);
        string filePath = Path.Combine(saveFolder, fileName + ".json");

        try
        {
            File.WriteAllText(filePath, json);
            Debug.Log($"[JsonSaveManager] 資料已儲存至: {filePath}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[JsonSaveManager] 儲存資料時發生錯誤: {ex.Message}");
        }
    }

    /// <summary>
    /// 從 JSON 檔案中讀取資料。
    /// </summary>
    /// <typeparam name="T">要讀取的資料類型。</typeparam>
    /// <param name="fileName">檔案名稱（不含副檔名）。</param>
    /// <returns>讀取到的資料，若檔案不存在或發生錯誤則回傳 default(T)。</returns>
    public static T Load<T>(string fileName)
    {
        string filePath = Path.Combine(saveFolder, fileName + ".json");
        if (File.Exists(filePath))
        {
            try
            {
                string json = File.ReadAllText(filePath);
                T data = JsonUtility.FromJson<T>(json);
                Debug.Log($"[JsonSaveManager] 資料已從: {filePath} 讀取");
                return data;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[JsonSaveManager] 讀取資料時發生錯誤: {ex.Message}");
                return default(T);
            }
        }
        else
        {
            Debug.LogWarning($"[JsonSaveManager] 找不到存檔檔案: {filePath}");
            return default(T);
        }
    }

    /// <summary>
    /// 刪除指定的存檔檔案。
    /// </summary>
    /// <param name="fileName">檔案名稱（不含副檔名）。</param>
    public static void Delete(string fileName)
    {
        string filePath = Path.Combine(saveFolder, fileName + ".json");
        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
                Debug.Log($"[JsonSaveManager] 已刪除存檔檔案: {filePath}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[JsonSaveManager] 刪除檔案時發生錯誤: {ex.Message}");
            }
        }
        else
        {
            Debug.LogWarning($"[JsonSaveManager] 無法刪除，找不到檔案: {filePath}");
        }
    }

    /// <summary>
    /// 檢查指定的存檔檔案是否存在。
    /// </summary>
    /// <param name="fileName">檔案名稱（不含副檔名）。</param>
    /// <returns>存在則回傳 true，否則回傳 false。</returns>
    public static bool SaveFileExists(string fileName)
    {
        string filePath = Path.Combine(saveFolder, fileName + ".json");
        return File.Exists(filePath);
    }
}
