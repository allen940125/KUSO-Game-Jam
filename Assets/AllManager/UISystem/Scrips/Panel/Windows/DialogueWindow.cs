using System.Collections.Generic;
using System.Linq;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueWindow : BasePanel
{
    [SerializeField] TMP_Text characterNameText;
    [SerializeField] TMP_Text dialogueBodyText;
    [SerializeField] GameObject optionBtnPrefab;
    [SerializeField] Transform optionBtnContainer;
    
    [SerializeField] Image meshImage;
    
    private void OnEnable()
    {
        var dm = DialogueManager.Instance;
        dm.OnLineChanged      += HandleLine;
        dm.OnOptionsAvailable += ShowOptions;
        dm.OnDialogueEnd      += ClosePanel;
        dm.OnStoreOpened      += OpenStore;
    }

    private void OnDisable()
    {
        var dm = DialogueManager.Instance;
        dm.OnLineChanged      -= HandleLine;
        dm.OnOptionsAvailable -= ShowOptions;
        dm.OnDialogueEnd      -= ClosePanel;
        dm.OnStoreOpened      -= OpenStore;
    }
    
    private void HandleLine(DialogueLine line)
    {
        characterNameText.text = line.CharacterName;

        if (characterNameText.text == "旁白")
        {
            dialogueBodyText.color = Color.yellow;
        }
        else if(characterNameText.text == "心靈教育員")
        {
            dialogueBodyText.color = Color.red;
        }
        else
        {
            dialogueBodyText.color = Color.white;
        }
        
        dialogueBodyText.text = line.Text;
        meshImage.gameObject.SetActive(false);
        
        ClearOptions();
    }

    private void ShowOptions(List<DialogueLine> opts)
    {
        ClearOptions();
        dialogueBodyText.text = "";
        meshImage.gameObject.SetActive(true);
        
        var validOptions = opts.Where(opt => 
            DialogueConditionChecker.CheckCondition(opt.Condition)
        ).ToList();

        for (int i = 0; i < validOptions.Count; i++)
        {
            var opt = validOptions[i];
            var btn = Instantiate(optionBtnPrefab, optionBtnContainer);
            btn.GetComponentInChildren<TMP_Text>().text = opt.Text;
        
            // // 條件不滿足時灰化按鈕（可選）
            // if (!string.IsNullOrEmpty(opt.Condition))
            // {
            //     bool conditionMet = DialogueManager.DialogueConditionChecker.CheckCondition(opt.Condition);
            //     btn.GetComponent<Button>().interactable = conditionMet;
            //     btn.GetComponent<TooltipTrigger>().SetTooltip(
            //         conditionMet ? "" : "條件未滿足: " + opt.Condition
            //     );
            // }
        
            int idx = i;
            btn.GetComponent<Button>().onClick.AddListener(() => 
                DialogueManager.Instance.SelectOption(idx)
            );
        }
    }
    
    private void ClearOptions()
    {
        foreach (Transform t in optionBtnContainer)
            Destroy(t.gameObject);
    }

    private void OpenStore()
    {
        // 打开商店 UI
        ClosePanel();
        
    }
}