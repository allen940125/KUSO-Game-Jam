using System;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private TextAsset _csvFile;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueManager.Instance.LoadAndStartDialogue(_csvFile);
        }
    }
}

//玩家正常白色
//內心戲黃色
//別人紅色