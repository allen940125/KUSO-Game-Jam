using System;
using Game.UI;
using UnityEngine;

public class EndDoor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance.MainGameMediator.RealTimePlayerData.SuspicionValue >= 50)
            {
                GameManager.Instance.UIManager.OpenPanel<StoryTextDisplayWindow>(UIType.StoryTextDisplayWindow);
                StartCoroutine(GameManager.Instance.UIManager.GetPanel<StoryTextDisplayWindow>(UIType.StoryTextDisplayWindow).EndStories());
            }
            else
            {
                FindFirstObjectByType<VideoManager>().PlayVideo("TVError",true);
                
            }
        }
    }
}
