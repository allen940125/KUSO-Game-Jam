using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Game.UI;
using Unity.Cinemachine;

public class TeleportManager : Singleton<TeleportManager>
{
    public CinemachineCamera vCam;
    
    [System.Serializable]
    public class TeleportData
    {
        public int id;
        public Transform teleportPosition;
        public Collider2D boundingShape;
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
            Debug.Log("玩家位置" + GameManager.Instance.Player.transform.position);
            GameManager.Instance.Player.transform.position = data.teleportPosition.position;
            UpdateConfiner(data);
        }
        else
        {
            Debug.LogWarning($"找不到ID為 {id} 的傳送資料！");
        }

        fadeWindow.ExitStory(0, 4);

        // 可以選擇這時候馬上淡出，也可以交給FadeInOutWindow自己處理
        // 這裡先不手動 ExitStory，因為 EnterStory(1,4)裡應該自己管理淡出動畫
    }
    
    private void UpdateConfiner(TeleportData data)
    {
        var confiner = vCam.GetComponent<CinemachineConfiner2D>();
        if (confiner != null)
        {
            confiner.BoundingShape2D = data.boundingShape;
            confiner.InvalidateLensCache();

            // 強制將相機位置設置到玩家當前位置（立即更新）
            Vector3 playerPos = GameManager.Instance.Player.transform.position;
            vCam.ForceCameraPosition(playerPos, Quaternion.identity);
        }
    }
}
