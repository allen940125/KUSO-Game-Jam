using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class StoryTextDisplayWindow : BasePanel
    {
        [SerializeField] private Text storyText;
        [SerializeField] private Text storyText2; 
        private string fullText;
        public float typeSpeed = 0.005f;

        private bool NeedText2;

        private float textOffset = 120f;
        private float text2Y = -470f;

        private bool isShowText = false;

        private bool isStoryEnd = false;

        [SerializeField] private float waitSecondsAfterStory = 2f;
        private float realWaitSecondsAfterStory = 2f;
        
        private string currentText = "";

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            StartCoroutine(StartStories());
        }  

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        IEnumerator ShowText(Color color)
        {
            isShowText = false; // 開始顯示時設為false

            if(!NeedText2)
            {
                storyText2.text = "";
                for (int i = 0; i < fullText.Length; i++)
                {
                    currentText = fullText.Substring(0, i + 1);
                    storyText.text = currentText;
                    storyText.color = color;
                    yield return new WaitForSeconds(typeSpeed * 100);
                }
            }
            else
            {
                for (int i = 0; i < fullText.Length; i++)
                {
                    currentText = fullText.Substring(0, i + 1);
                    storyText2.text = currentText;
                    storyText2.color = color;
                    yield return new WaitForSeconds(typeSpeed * 100);
                }
                NeedText2 = false;
            }

            isShowText = true; // 顯示完畢設為true
        }

        public void StartStory(String text)
        {
            fullText = text;
            StartCoroutine(ShowText(Color.white));
        }
        

        public void StartStory(StoryData storyData)
        {
            fullText = storyData.storyText;
            StartCoroutine(ShowText(storyData.color));
        }

        public void EndStory(StoryData storyData)
        {
            fullText = storyData.storyText;
            StartCoroutine(ShowText(storyData.color));
        }
        
        IEnumerator StartStories()
        {
            List<StoryData> stories = DialogueManager.Instance.stories;
            Debug.Log(stories.Count);
            GameManager.Instance.MainGameMediator.RealTimePlayerData.CanPlayerMove = false;
            Debug.Log("故事開始，玩家無法移動");
            foreach (StoryData story in stories)
            {
                if(story.storyID <= 5)
                {
                    NeedText2 = story.storyID == 3; // 如果ID是3，啟動NeedText2
                    if(story.storyID == 2)
                    {
                        storyText.transform.position += new Vector3(0, textOffset, 0);
                        storyText2.transform.position += new Vector3(0, text2Y, 0);
                        realWaitSecondsAfterStory = 0f;
                    }
                    else if(story.storyID == 4)
                    {
                        storyText.transform.position -= new Vector3(0, textOffset, 0);
                        realWaitSecondsAfterStory = waitSecondsAfterStory;
                    }
                    else
                    {
                        realWaitSecondsAfterStory = waitSecondsAfterStory;
                    }
                    StartStory(story);
                    Debug.Log("顯示第" + story.storyID + "個故事:" + story.storyText);
                    yield return new WaitUntil(() => isShowText); // 等到文字顯示完畢
                    yield return new WaitForSeconds(realWaitSecondsAfterStory); // 根據ID決定等待時間
                }
            }
            storyText.text = "";
            storyText2.text = "";
            GameManager.Instance.UIManager.GetPanel<FadeInOutWindow>(UIType.FadeInOutWindow).FadeOut(0, 4);
            GameManager.Instance.MainGameMediator.RealTimePlayerData.CanPlayerMove = true;
            GameManager.Instance.MainGameMediator.RealTimePlayerData.IsListeningYajuuSenpai = false;
            Debug.Log("故事結束，淡出後玩家可以移動");
        }

        public IEnumerator EndStories()
        {
            GameManager.Instance.UIManager.OpenPanel<FadeInOutWindow>(UIType.FadeInOutWindow);
            GameManager.Instance.UIManager.GetPanel<FadeInOutWindow>(UIType.FadeInOutWindow).FadeIn(1, 4);
            yield return new WaitForSeconds(0.35f);
            List<StoryData> stories = DialogueManager.Instance.stories;
            Debug.Log(stories.Count);
            GameManager.Instance.MainGameMediator.RealTimePlayerData.CanPlayerMove = false;
            Debug.Log("GoodEnd結尾故事開始，玩家無法移動");
            foreach (StoryData story in stories)
            {
                if(story.storyID >= 6)
                {
                EndStory(story);
                Debug.Log("顯示第" + story.storyID + "個故事:" + story.storyText);
                yield return new WaitUntil(() => isShowText); // 等到文字顯示完畢
                yield return new WaitForSeconds(realWaitSecondsAfterStory); // 根據ID決定等待時間
                }
            }
            storyText.text = "";
            storyText2.text = "";
            Debug.Log("GoodEnd結尾故事結束");
        }
    }
}