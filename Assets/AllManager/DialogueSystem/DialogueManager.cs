using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 對話管理器（單例模式）
/// 負責解析對話數據、管理對話流程和事件通知
/// </summary>
public class DialogueManager : Singleton<DialogueManager>
{
    // 事件系統用於與UI解耦
    public event Action<DialogueLine> OnLineChanged;         // 當對話行變化時觸發
    public event Action<List<DialogueLine>> OnOptionsAvailable; // 當有選項可用時觸發
    public event Action OnDialogueEnd;                       // 當對話結束時觸發
    public event Action OnStoreOpened;                       // 當商店開啟時觸發

    private Dictionary<int, DialogueLine> _linesById;        // 用ID索引所有對話行的字典
    private Dictionary<int, List<DialogueLine>> _optionsByParent; // 用父ID索引選項列表的字典
    [SerializeField] private int _currentLineId;                              // 當前對話行的ID

    [SerializeField] private TextAsset _csvFile;             // 對話數據的CSV文件

    [Header("故事旁白資料")]
    public List<StoryData> stories;
    
    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            PrintLinesById();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("下一行");
            Proceed();
        }
    }

    /// <summary>
    /// 繼續進行對話（用於普通對話的推進）
    /// </summary>
    public void Proceed()
    {
        var line = _linesById[_currentLineId];
    
        if (line.LineType != DialogueLineType.Dialogue) 
            return;

        // 明確分兩種情況處理
        if (_optionsByParent.ContainsKey(line.Id))
        {
            Debug.Log("顯示選項，等待玩家選擇");
            OnOptionsAvailable?.Invoke(_optionsByParent[line.Id]);
        }
        else 
        {
            Debug.Log("無選項，自動跳轉至:" + line.NextLineId);
            JumpTo(line.NextLineId);
        }
    }

    /// <summary>
    /// 跳轉到指定ID的對話行
    /// </summary>
    /// <param name="nextId">目標行ID</param>
    private void JumpTo(int nextId)
    {
        Debug.Log("對話ID從" + _currentLineId + "到" + nextId);
        if (!_linesById.ContainsKey(nextId))
        {
            Debug.LogError($"跳轉ID {nextId} 不存在！");
            OnDialogueEnd?.Invoke();
            return;
        }
        _currentLineId = nextId;
        DispatchCurrent();
    }
    
    /// <summary>
    /// 分派當前對話行（根據類型觸發相應事件）
    /// </summary>
    private void DispatchCurrent()
    {
        var line = _linesById[_currentLineId];
    
        // 修改這裡：只有當背景不為空且非空字串時才換背景
        if (!string.IsNullOrWhiteSpace(line.Background))
        {
            ChangeBackground(line.Background.Trim());
        }
        else
        {
            // 明確傳入 null 來關閉背景
            GameManager.Instance.UIManager.GetPanel<DialogueWindow>(UIType.DialogueWindow)?.SetBackground(null);
        }
        
        // 對話類型需先檢查條件
        if (line.LineType == DialogueLineType.Dialogue)
        {
            // 條件未通過就自動跳過這一行
            if (!DialogueConditionChecker.CheckCondition(line.Condition))
            {
                Debug.Log($"條件未滿足，跳過對話ID: {line.Id}，跳轉到: {line.NextLineId}");
                JumpTo(line.NextLineId); // 遞迴繼續向下找滿足的對話
                return;
            }
        }

        switch (line.LineType)
        {
            case DialogueLineType.Dialogue:
                OnLineChanged?.Invoke(line);
                DialogueEffectExecutor.ExecuteEffect(line.Effect);
                break;

            case DialogueLineType.Option:
                JumpTo(line.NextLineId);
                break;

            case DialogueLineType.Store:
                OnStoreOpened?.Invoke();
                break;

            case DialogueLineType.End:
                GameManager.Instance.MainGameMediator.RealTimePlayerData.CanPlayerMove = true;
                OnDialogueEnd?.Invoke();
                break;
        }
    }
    
    
    private void ChangeBackground(string backgroundName)
    {
        // 再次防禦性檢查
        if (string.IsNullOrWhiteSpace(backgroundName))
        {
            GameManager.Instance.UIManager.GetPanel<DialogueWindow>(UIType.DialogueWindow)?.SetBackground(null);
            return;
        }

        backgroundName = backgroundName.Trim();
        var sprite = Resources.Load<Sprite>($"Backgrounds/{backgroundName}");
    
        if (sprite == null)
        {
            Debug.LogError($"找不到背景圖片: {backgroundName}，請確認Resources/Backgrounds資料夾內有對應的Sprite");
            return;
        }

        GameManager.Instance.UIManager.GetPanel<DialogueWindow>(UIType.DialogueWindow)?.SetBackground(sprite);
    }



    #region DialogueUI

    /// <summary>
    /// 選擇指定選項
    /// </summary>
    /// <param name="optionIndex">選項索引</param>
    public void SelectOption(int optionIndex)
    {
        var opts = _optionsByParent[_currentLineId]
            .Where(opt => DialogueConditionChecker.CheckCondition(opt.Condition)) // 過濾有效選項
            .ToList();

        if (optionIndex < 0 || optionIndex >= opts.Count) return;
    
        var selected = opts[optionIndex];
        DialogueEffectExecutor.ExecuteEffect(selected.Effect); // 執行選項效果
        JumpTo(selected.NextLineId);
    }

    #endregion
    
    #region LoadDialogueCsv
    
    /// <summary>
    /// 解析CSV文件並構建數據結構
    /// </summary>
    /// <param name="csv">CSV文本資源</param>
    private void ParseCsv(TextAsset csv)
    {
        _linesById = new Dictionary<int, DialogueLine>();
        _optionsByParent = new Dictionary<int, List<DialogueLine>>();

        var rows = csv.text
                      .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        int lastDialogueId = -1;

        foreach (var raw in rows)
        {
            var cells = raw.Split(',');
            var tag = cells[0];

            switch (tag)
            {
                case "#": // 普通對話行
                    var id = int.Parse(cells[1]);
                    var line = new DialogueLine {
                        Id = id,
                        LineType = DialogueLineType.Dialogue,
                        CharacterName = cells[2],
                        Position = cells[3],
                        Text = cells[4],
                        NextLineId = int.TryParse(cells[5], out var nxt) ? nxt : -1,
                        Effect = cells[6],
                        Condition = cells[7],
                        Background = cells.Length > 10 ? cells[10] : "", // <<< 注意是 index 10 !!
                    };
                    _linesById[id] = line;
                    lastDialogueId = id;
                    break;

                case "&": // 分支選項行
                    var optId = int.Parse(cells[1]);
                    var opt = new DialogueLine {
                        Id = optId,
                        LineType = DialogueLineType.Option,
                        Text = cells[4],
                        NextLineId = int.TryParse(cells[5], out var optNxt) ? optNxt : -1,
                        Effect = cells[6],
                        Condition = cells[7],
                        Background = cells.Length > 10 ? cells[10] : "", // <<< 注意是 index 10 !!
                    };
                    _linesById[optId] = opt;
                    if (!_optionsByParent.ContainsKey(lastDialogueId))
                        _optionsByParent[lastDialogueId] = new List<DialogueLine>();
                    _optionsByParent[lastDialogueId].Add(opt);
                    break;

                case "END": // 結束標記
                    var endId = int.Parse(cells[1]);
                    _linesById[endId] = new DialogueLine {
                        Id = endId,
                        LineType = DialogueLineType.End
                    };
                    break;

                case "STORE": // 商店標記
                    var storeId = int.Parse(cells[1]);
                    _linesById[storeId] = new DialogueLine {
                        Id = storeId,
                        LineType = DialogueLineType.Store
                    };
                    break;
            }
        }
    }
    
    /// <summary>
    /// 載入並解析CSV對話數據
    /// </summary>
    /// <param name="csv">包含對話數據的文本資源</param>
    public void LoadDialogueCsv(TextAsset csv)
    {
        ParseCsv(csv);
        Debug.Log(_linesById.Keys.Min());
        _currentLineId = _linesById.Keys.Min();
        //Debug.Log($"初始載入ID:{_currentLineId}"); // 應為0
        DispatchCurrent(); // 確保此處觸發OnLineChanged
    }
    
    #endregion
    
    #region Toolkit

    public void PrintLinesById() {
        Debug.Log("===== 對話行字典（按ID排序） =====");
        foreach (var pair in _linesById.OrderBy(x => x.Key)) {
            var line = pair.Value;
            string log = $"[ID {line.Id.ToString().PadLeft(3)}] {line.LineType}";
        
            // 對話行詳細信息
            if (line.LineType == DialogueLineType.Dialogue) {
                log += $" | 角色:{line.CharacterName.PadRight(5)} 位置:{line.Position.PadRight(3)}";
                log += $" | 內容:「{line.Text}」";
                log += $" → 跳轉ID:{line.NextLineId}";
            }
            // 選項行詳細信息
            else if (line.LineType == DialogueLineType.Option) {
                var parentLine = _optionsByParent.FirstOrDefault(x => x.Value.Any(o => o.Id == line.Id));
                log += $" (父ID:{parentLine.Key}) | 選項:「{line.Text}」";
                log += $" → 跳轉ID:{line.NextLineId}";
            }
        
            Debug.Log(log);
        }
        Debug.Log("===== 打印完畢 =====");
    }

    /// <summary>
    /// 載入資源並開始對話
    /// </summary>
    /// <param name="csvfile"></param>
    public void LoadAndStartDialogue(TextAsset csvfile)
    {
        // 先開啟UI面板再載入數據
        GameManager.Instance.UIManager.OpenPanel<DialogueWindow>(UIType.DialogueWindow);
    
        // 確保UI訂閱事件後再初始化對話
        _csvFile = csvfile;
        LoadDialogueCsv(_csvFile); // 此時UI面板已啟用，事件已訂閱
    }
    
    /// <summary>
    /// 將所有對話行序列化為陣列供Debug用（Inspector不可見，純用來Debug.Log）
    /// </summary>
    public DialogueLine[] GetAllLinesAsArray()
    {
        return _linesById.Values.OrderBy(line => line.Id).ToArray();
    }

    #endregion
}