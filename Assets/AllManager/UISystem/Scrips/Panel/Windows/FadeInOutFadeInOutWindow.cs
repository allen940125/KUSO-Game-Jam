using UnityEngine.UI;
using UnityEngine;

namespace Game.UI
{
    public class FadeInOutWindow : BasePanel
    {
        [SerializeField] Sprite YajuuSenpai;
        [SerializeField] Image nowImage;
 
        [SerializeField] Color backGroundColor;
        [SerializeField] float changespeed = 4f;

        [SerializeField] float maxAlpha = 1f;
        [SerializeField] float minAlpha = 0f;

        [SerializeField] bool isEntering = false;
        [SerializeField] bool isExiting = false;

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
                    nowImage.color = backGroundColor;   
                    //Debug.Log(backGroundColor.a);
                }
                else
                {
                    backGroundColor.a = maxAlpha;
                    nowImage.color = backGroundColor; 
                    isEntering = false;
                }
            }
            else if(isExiting)
            {
                if(backGroundColor.a >= minAlpha)
                {
                    backGroundColor.a -= changespeed * Time.deltaTime;
                    nowImage.color = backGroundColor;   
                }
                else
                {
                    backGroundColor.a = minAlpha;
                    nowImage.color = backGroundColor; 
                    isExiting = false;
                }
            }
        }
        public void EnterStory(float maxalpha, float speed)
        {
            maxAlpha = maxalpha;
            changespeed = speed;
            isEntering = true;
        }

        public void EnterStory(float speed)
        {
            changespeed = speed;
            isEntering = true;
        }

        public void EnterStory()
        {
            isEntering = true;
        }

        public void ExitStory(float minalpha, float speed)
        {
            minAlpha = minalpha;
            changespeed = speed;
            isExiting = true;
        }   

        public void ExitStory(float speed)
        {
            changespeed = speed;
            isExiting = true;
        }

        public void ExitStory()
        {
            isExiting = true;
        }

        public void SetColor(Color color)
        {
            backGroundColor = color;
        }

        public void Yarimasune()
        {
            nowImage.color = backGroundColor = new Color(Color.white.r, Color.white.g, Color.white.b, 0f);
            nowImage.sprite = YajuuSenpai;
        }

        public void NoYarimasune()
        {
            nowImage.color = backGroundColor = new Color(Color.black.r, Color.black.g, Color.black.b, 0f);
            nowImage.sprite = null;
        }
    }
}