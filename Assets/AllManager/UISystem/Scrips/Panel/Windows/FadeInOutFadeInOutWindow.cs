using UnityEngine.UI;
using UnityEngine;

namespace Game.UI
{
    public class FadeInOutWindow : BasePanel
    {
        [SerializeField] Sprite newSprite; //要更改的圖片
        [SerializeField] Image nowImage; //現在的圖片
 
        [SerializeField] Color backGroundColor; //顏色
        [SerializeField] float changespeed = 4f; //變化速度

        [SerializeField] float maxAlpha = 1f; //最大透明度
        [SerializeField] float minAlpha = 0f; //最小透明度

        [SerializeField] bool isFadeIn = false; //是否淡入
        [SerializeField] bool isFadeOut = false; //是否淡出

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
            //如果isFadeIn為true就要淡入
            if(isFadeIn)
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
                    isFadeIn = false;
                }
            }
            else if(isFadeOut) //如果isFadeOut為true就要淡出
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
                    isFadeOut = false;
                }
            }
        }

        //淡入，可以更改透明度以及速度
        public void FadeIn(float maxalpha, float speed)
        {
            maxAlpha = maxalpha;
            changespeed = speed;
            isFadeIn = true;
        }

        //淡入，可以更改速度
        public void FadeIn(float speed)
        {
            changespeed = speed;
            isFadeIn = true;
        }

        //淡入
        public void FadeIn()
        {
            isFadeIn = true;
        }

        //淡出，可以更改透明度以及速度
        public void FadeOut(float minalpha, float speed)
        {
            minAlpha = minalpha;
            changespeed = speed;
            isFadeOut = true;
        }   

        //淡出，可以更改速度
        public void FadeOut(float speed)
        {
            changespeed = speed;
            isFadeOut = true;
        }

        //淡出
        public void FadeOut()
        {
            isFadeOut = true;
        }

        //更改顏色
        public void SetColor(Color color)
        {
            backGroundColor = color;
        }

        //更改圖片
        public void ChangeImage()
        {
            nowImage.color = backGroundColor = new Color(Color.white.r, Color.white.g, Color.white.b, 0f);
            nowImage.sprite = newSprite;
        }

        //更改圖片，可以在其他地方放圖片
        public void ChangeImage(Sprite sprite)
        {
            nowImage.color = backGroundColor = new Color(Color.white.r, Color.white.g, Color.white.b, 0f);
            nowImage.sprite = sprite;
        }

        //更改圖片，可以在其他地方放圖片以及透明度
        public void ChangeImage(Sprite sprite, float alpha)
        {
            nowImage.color = backGroundColor = new Color(Color.white.r, Color.white.g, Color.white.b, alpha);
            nowImage.sprite = sprite;
        }


        //更改圖片成Null
        public void ChangeImageToNull()
        {
            nowImage.color = backGroundColor = new Color(Color.black.r, Color.black.g, Color.black.b, 0f);
            nowImage.sprite = null;
        }
    }
}