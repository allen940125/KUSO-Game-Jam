using Game.UI;
using UnityEngine;

public class StoryIntro : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.UIManager.OpenPanel<FadeInOutWindow>(UIType.FadeInOutWindow);
        GameManager.Instance.UIManager.GetPanel<FadeInOutWindow>(UIType.FadeInOutWindow).FadeIn(1, 1000000000);
        GameManager.Instance.UIManager.OpenPanel<StoryTextDisplayWindow>(UIType.StoryTextDisplayWindow);
    }
}
