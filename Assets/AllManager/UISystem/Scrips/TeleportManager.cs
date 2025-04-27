using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Game.UI;

public class TeleportManager : Singleton<TeleportManager>
{
    [System.Serializable]
    public class TeleportData
    {
        public int id;
        public Transform teleportPosition;
    }

    public List<TeleportData> teleportDataList;

    public void TeleportWithFade(int id)
    {
        StartCoroutine(TeleportCoroutine(id));
    }

    private IEnumerator TeleportCoroutine(int id)
    {
        // 開啟淡入淡出畫面
        GameManager.Instance.UIManager.OpenPanel<FadeInOutWindow>(UIType.FadeInOutWindow);
        var fadeWindow = GameManager.Instance.UIManager.GetPanel<FadeInOutWindow>(UIType.FadeInOutWindow);
        fadeWindow.EnterStory(1, 4);

        // 等淡入時間
        yield return new WaitForSeconds(1f);

        // 找到對應ID的TeleportData
        TeleportData data = teleportDataList.Find(x => x.id == id);
        if (data != null)
        {
            GameManager.Instance.Player.transform.position = data.teleportPosition.position;
        }
        else
        {
            Debug.LogWarning($"找不到ID為 {id} 的傳送資料！");
        }

        fadeWindow.EnterStory(0, 4);

        // 可以選擇這時候馬上淡出，也可以交給FadeInOutWindow自己處理
        // 這裡先不手動 ExitStory，因為 EnterStory(1,4)裡應該自己管理淡出動畫
    }
}
