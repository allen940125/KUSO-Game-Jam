using System;
using Gamemanager;

namespace Game.UI
{
    public class TransitionUIManager
    {
        public void Initialize()
        {
            GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSceneLoadedEvent, OnSceneLoadedEvent);
            GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSceneTransitionStartedEvent, OnSceneTransitionStartedEvent);
        }

        public void Cleanup()
        {
            GameManager.Instance.MainGameEvent.Unsubscribe<SceneLoadedEvent>(OnSceneLoadedEvent);
            GameManager.Instance.MainGameEvent.Unsubscribe<SceneTransitionStartedEvent>(OnSceneTransitionStartedEvent);
        }

        #region 事件訂閱

        private void OnSceneLoadedEvent(SceneLoadedEvent cmd)
        {
            HideTransitionUI();
        }

        private void OnSceneTransitionStartedEvent(SceneTransitionStartedEvent cmd)
        {
            ShowTransitionUI();
        }
        
        #endregion
        
        private void ShowTransitionUI()
        {
            GameManager.Instance.UIManager.OpenPanel<SceneLoadingTransitionPanel>(UIType.SceneLoadingTransitionPanel);
        }

        private void HideTransitionUI()
        {
            //GameManager.Instance.UIManager.CloseAllPanels();
        }
    }
}