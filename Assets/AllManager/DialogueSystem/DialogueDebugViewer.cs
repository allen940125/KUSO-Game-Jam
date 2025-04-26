using UnityEngine;

public class DialogueDebugViewer : MonoBehaviour
{
    [SerializeField] private DialogueManager _dialogueManager;
    [SerializeField] private DialogueLine[] debugLines;

    [ContextMenu("更新目前所有對話行")]
    public void RefreshDebugLines()
    {
        if (_dialogueManager == null) _dialogueManager = DialogueManager.Instance;
        debugLines = _dialogueManager.GetAllLinesAsArray();
    }
}