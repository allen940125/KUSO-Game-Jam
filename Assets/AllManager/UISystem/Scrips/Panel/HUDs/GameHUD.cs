using System;
using Game.SceneManagement;
using Gamemanager;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class GameHUD : BasePanel
    {
        [Header("轉換的場景名稱")] [SerializeField] SceneType sceneType;

        [SerializeField] private float playerDrunkennessValue;
        
        [SerializeField] private float playerCurWineBottle;
        
        [SerializeField] private float playerCurWineBottleRemainingAlcohol;
        
        [SerializeField] private TMP_Text text_PlayerDrunkennessValue;
        
        [SerializeField] private TMP_Text text_CurWineBottle;
        
        [SerializeField] private TMP_Text text_CurWineBottleRemainingAlcohol;

        protected override void Awake()
        {
            base.Awake();
        }

        void Start()
        {
            GameManager.Instance.MainGameEvent.Send(new CursorToggledEvent() { ShowCursor = false });
        }
        private void Update()
        {
            playerDrunkennessValue = GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerDrunkennessValue;
            playerCurWineBottle = GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerCurWineBottle;
            playerCurWineBottleRemainingAlcohol = GameManager.Instance.MainGameMediator.RealTimePlayerData.PlayerCurWineBottleRemainingAlcohol;
            
            UpdateText();
        }

        void UpdateText()
        {
            // text_PlayerDrunkennessValue.text = "酒精值" + playerDrunkennessValue.ToString();
            // text_CurWineBottle.text = "酒瓶數量" + playerCurWineBottle.ToString();
            // text_CurWineBottleRemainingAlcohol.text = "剩餘酒量" + playerCurWineBottleRemainingAlcohol.ToString();
        }
    }
}

