using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.UI
{
    public class FadeInOutPanel : BasePanel
    {
        [SerializeField] GameObject backGroundImage;

        [SerializeField] Color backGroundColor;
        [SerializeField] float changespeed = 4f;

        [SerializeField] float maxAlpha = 1f;

        [SerializeField] bool isEntering = false;
        [SerializeField] bool isExiting = false;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            backGroundColor = backGroundImage.GetComponent<Image>().color;
            backGroundColor.a = 1f;
            backGroundImage.GetComponent<Image>().color = backGroundColor;
            base.Start();
        }  

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        // private void Start()
        // {
        //     BackGroundColor = BackGroundObject.GetComponent<Image>().color;
        //     BackGroundColor.a = 1f;
        //     BackGroundObject.GetComponent<Image>().color = BackGroundColor;
        // }
    
        private void Update()
        {
            if(isEntering)
            {
                if(backGroundColor.a <= maxAlpha)
                {
                    
                    backGroundColor.a += changespeed * Time.deltaTime;
                    backGroundImage.GetComponent<Image>().color = backGroundColor;   
                    Debug.Log(backGroundColor.a);
                }
                else
                {
                    backGroundColor.a = maxAlpha;
                    backGroundImage.GetComponent<Image>().color = backGroundColor; 
                    isEntering = false;
                }
            }
            else if(isExiting)
            {
                if(backGroundColor.a >= 0)
                {
                    backGroundColor.a -= changespeed * Time.deltaTime;
                    backGroundImage.GetComponent<Image>().color = backGroundColor;   
                }
                else
                {
                    backGroundColor.a = 0;
                    backGroundImage.GetComponent<Image>().color = backGroundColor; 
                    isExiting = false;
                }
            }
        }
        public void EnterStory(float alpha, float speed)
        {
            maxAlpha = alpha;
            changespeed = speed;
            isEntering = true;
        }

        public void ExitStory(float speed)
        {
            changespeed = speed;
            isExiting = true;
        }
    }
}