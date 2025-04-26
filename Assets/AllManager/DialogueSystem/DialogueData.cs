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
[Serializable]
public class DialogueLine
{
    public int Id;                      // 唯一識別ID
    public DialogueLineType LineType;   // 行類型
    public string CharacterName;        // 角色名稱（僅對話行使用）
    public string Position;             // 角色位置（僅對話行使用）
    public string Text;                 // 顯示文本（對話內容/選項文字）
    public int NextLineId;              // 下一行ID（用於跳轉）
    public string Effect;               // 選項效果（僅選項行使用）
    public string Condition;            // 條件 (僅選項行使用)
}