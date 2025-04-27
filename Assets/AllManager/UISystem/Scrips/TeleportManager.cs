using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Game.UI;


public class TeleportManager : MonoBehaviour
{
    
    public Vector3[] teleportPositions;    
    public void TeleportWithFade(int index)
    {
        if (index >= 0 && index < teleportPositions.Length)
        {
            StartCoroutine(TeleportCoroutine(index));
        }
        else
        {
            Debug.LogWarning("傳送位置索引錯誤！");
        }
    }

    private IEnumerator TeleportCoroutine(int index)
    {
        GameManager.Instance.UIManager.OpenPanel<FadeInOutWindow>(UIType.FadeInOutWindow);

        transform.position = teleportPositions[index];

        UIManager.GetPanel<FadeInOutWindow>(UIType.FadeInOutWindow).EnterStory(1,4);
    }
}
