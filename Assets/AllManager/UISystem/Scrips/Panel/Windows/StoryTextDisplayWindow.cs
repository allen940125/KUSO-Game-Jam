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
        private string fullText;
        public float typeSpeed = 0.005f;

        [SerializeField] private float waitSecondsAfterStory = 3f;
        
        private string currentText = "";

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            StartCoroutine(StartAllStories());
        }  

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        IEnumerator ShowText()
        {
            for (int i = 0; i < fullText.Length; i++)
            {
                currentText = fullText.Substring(0, i + 1);
                storyText.text = currentText;
                yield return new WaitForSeconds(typeSpeed * 100);
            }
        }

        public void StartStory(String text)
        {
            fullText = text;
            StartCoroutine(ShowText());
        }
        
        public void StartStory(StoryData storyData)
        {
            storyText.color = storyData.color;
            fullText = storyData.storyText;
            StartCoroutine(ShowText());
        }
        
        IEnumerator StartAllStories()
        {
            List<StoryData> stories = DialogueManager.Instance.stories;
            Debug.Log(stories.Count);
            foreach (StoryData story in stories)
            {
                StartStory(story);
                Debug.Log("顯示第" + story.storyID + "個故事:" + story.storyText);
                yield return new WaitForSeconds(waitSecondsAfterStory); // 等待5秒
            }
        }
    }
}