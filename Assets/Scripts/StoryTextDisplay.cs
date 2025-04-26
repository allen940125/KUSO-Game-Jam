using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class StoryTextDisplay : BasePanel
    {
        [SerializeField] private Text storyText;
        private string fullText;
        public float typeSpeed = 0.005f;

        private string currentText = "";

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
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
    }
}