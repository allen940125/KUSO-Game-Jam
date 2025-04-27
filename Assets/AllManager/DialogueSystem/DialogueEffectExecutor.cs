using UnityEngine;

/// <summary>
/// 對話效果執行器
/// </summary>
public class DialogueEffectExecutor
{
    /// <summary>
    /// 執行效果
    /// </summary>
    /// <param name="effect"></param>
    public static void ExecuteEffect(string effect)
    {
        Debug.Log("效果值" + effect);
        if (string.IsNullOrEmpty(effect)) return;

        var commands = effect.Split('|');
        foreach (var cmd in commands)
        {
            var parts = cmd.Split(':');
            if (parts.Length < 2) continue;

            string type = parts[0];
            string param = parts[1];

            Debug.Log("抓到的類型" + type.ToLower());
            
            switch (type.ToLower())
            {
                case "fell":
                    var itemData = param.Split('*');
                    string fall = itemData[0];
                    int value = itemData.Length > 1 ? int.Parse(itemData[1]) : 1;
                    
                    Debug.Log("目標心情為" + fall + "值是" + value);
                    
                    if (fall == "Excitement")
                    {
                        GameManager.Instance.MainGameMediator.RealTimePlayerData.ExcitementValue += value;
                    }
                    else if (fall == "Suspicion")
                    {
                        GameManager.Instance.MainGameMediator.RealTimePlayerData.SuspicionValue += value;
                    }
                    break;
                case "method":
                {
                    int dialogueEventID = int.Parse(param);
                    if (dialogueEventID == 1)
                    {
                        SpecialEvents.Instance.TriggerEvent1();
                    }
                    else if (dialogueEventID == 2)
                    {
                        SpecialEvents.Instance.TriggerEvent2();
                    }
                    else if (dialogueEventID == 3)
                    {
                        SpecialEvents.Instance.TriggerEvent3();
                    }
                    else if (dialogueEventID == 4)
                    {
                        SpecialEvents.Instance.TriggerEvent4();
                    }
                    else if (dialogueEventID == 5)
                    {
                        SpecialEvents.Instance.TriggerEvent5();
                    }
                    break;
                }
                // case "give":
                //     var itemData = param.Split('x');
                //     int itemId = int.Parse(itemData[0]);
                //     int quantity = itemData.Length > 1 ? int.Parse(itemData[1]) : 1;
                //     InventoryManager.Instance.AddItem(itemId, quantity);
                //     break;
                //
                // case "complete":
                //     int questId = int.Parse(param);
                //     QuestManager.Instance.CompleteQuest(questId);
                //     break;
                //
                // case "unlock":
                //     switch (param)
                //     {
                //         case "shop":
                //             GameManager.Instance.UnlockShop();
                //             break;
                //     }
                //     break;
            }
        }
    }
}