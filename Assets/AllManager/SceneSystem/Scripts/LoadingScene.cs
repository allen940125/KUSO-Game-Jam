using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Game.SceneManagement
{
    /// <summary>
    /// 負責在 Loading 畫面時加載目標場景，建議命名為 LoadingScreenController
    /// </summary>
    public class LoadingScreenController : MonoBehaviour
    {
        private void Start()
        {
            Debug.Log("LoadingScreenController: 當前要載入的目標場景：" + SceneTransitionManager.NextTargetScene);
            StartCoroutine(LoadNextScene());
        }

        private IEnumerator LoadNextScene()
        {
            // 非同步載入目標場景
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneTransitionManager.NextTargetScene.ToString());
            // 初始不允許自動切換，讓 Loading 畫面保持一段時間
            asyncOperation.allowSceneActivation = false;

            while (!asyncOperation.isDone)
            {
                // 當加載進度達到 0.9 時，代表場景加載基本完成
                if (asyncOperation.progress >= 0.9f)
                {
                    // 延遲播放完成動畫或其他效果
                    yield return new WaitForSeconds(0.5f);
                    asyncOperation.allowSceneActivation = true;
                }
                yield return null;
            }
        }
    }
}