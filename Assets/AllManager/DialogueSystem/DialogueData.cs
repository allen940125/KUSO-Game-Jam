using System;

/// <summary>
/// 對話行類型列舉
/// </summary>
public enum DialogueLineType
{
    Dialogue,   // 普通對話
    Option,     // 分支選項
    End,        // 對話結束
    Store       // 商店觸發
}

/// <summary>
/// 對話行數據類別
/// </summary>
[System.Serializable]
public class DialogueLine
{
    public int Id;
    public DialogueLineType LineType;
    public string CharacterName;
    public string Position;
    public string Text;
    public int NextLineId;
    public string Effect;
    public string Condition;
    public string Background; // <<< 👈 新增這行
}
