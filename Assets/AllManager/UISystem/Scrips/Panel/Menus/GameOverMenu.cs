using Game.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class GameOverMenu : BasePanel
    {
        [Header("設定通用按鈕")]
        [SerializeField] Button Btn_PlayAgain;
        [SerializeField] Button Btn_GoMainMenu;

        Animator animator;

        protected override void Awake()
        {
            base.Awake();

            //設定通用按鈕
            InitializeCommonButtons();
        }

        /// <summary>
        /// 初始化通用按鈕
        /// </summary>
        void InitializeCommonButtons()
        {
            Btn_PlayAgain.onClick.AddListener(() => OnBtn_PlayAgain());
            Btn_GoMainMenu.onClick.AddListener(() => OnBtn_GoMainMenu());
        }

        //通用按鈕

        void OnBtn_PlayAgain()
        {
            Debug.Log("Click Btn_PlayAgain");
            
            GameManager.Instance.SceneTransitionManager.LoadScene(SceneType.TestGameScene1);
        }

        void OnBtn_GoMainMenu()
        {
            Debug.Log("Click Btn_Cancel");
            GameManager.Instance.SceneTransitionManager.LoadScene(SceneType.MainMenuScene);
        }
    }
}