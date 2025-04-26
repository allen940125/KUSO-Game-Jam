using UnityEngine;

/// <summary>
/// 對話條件檢查器
/// </summary>
public class DialogueConditionChecker
{
    /// <summary>
    /// 檢查條件
    /// </summary>
    /// <param name="condition"></param>
    /// <returns></returns>
    public static bool CheckCondition(string condition)
    {
        //沒有條件的欄位直接過
        if (string.IsNullOrEmpty(condition)) return true;
        
        var parts = condition.Split(':');
        if (parts.Length < 2) return false;

        string type = parts[0];
        string param = parts[1];

        switch (type.ToLower())
        {
            case "item":
                var itemParts = param.Split('x');
                int itemId = int.Parse(itemParts[0]);
                int requiredCount = itemParts.Length > 1 ? int.Parse(itemParts[1]) : 1;
            
                var item = InventoryManager.Instance.GetInventoryData(itemId);
                return item != null && item.quantity >= requiredCount;
            
            // case "quest":
            //     int questId = int.Parse(param);
            //     return QuestManager.Instance.IsQuestCompleted(questId);
            //
            // case "level":
            //     int requiredLevel = int.Parse(param);
            //     return Player.Instance.Level >= requiredLevel;
            
            default:
                Debug.LogError($"未知條件類型: {type}");
                return false;
        }
    }
}