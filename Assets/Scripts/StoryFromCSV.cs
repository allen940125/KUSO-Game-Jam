using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class StoryData
{
    public int storyID;
    public string storyText;
    public Color color;
    
    public StoryData(int id, string text)
    {
        storyID = id;
        storyText = text;
        color = color;
    }
}

public class StoryFromCSV : MonoBehaviour
{
    [SerializeField] private TextAsset csvFile; // 將CSV檔拖進來
    [SerializeField] private List<StoryData> stories = new List<StoryData>();

    private void Start()
    {
        LoadCSV();
        DialogueManager.Instance.stories = stories;
    }

    void LoadCSV()
    {
        if (csvFile == null)
        {
            Debug.LogError("CSV檔沒有指定！");
            return;
        }

        string[] lines = csvFile.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None);

        for (int i = 1; i < lines.Length; i++) // 從1開始，跳過標題行
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue; // 忽略空行

            string[] parts = lines[i].Split(',');
            if (parts.Length >= 2)
            {
                int id;
                if (int.TryParse(parts[0], out id))
                {
                    string text = parts[1].Trim();
                    stories.Add(new StoryData(id, text));
                }
                else
                {
                    Debug.LogWarning($"第{i + 1}行ID解析失敗: {parts[0]}");
                }
            }
        }
        
        DialogueManager.Instance.stories = stories;
    }
}