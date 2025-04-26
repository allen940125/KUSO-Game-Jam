using UnityEngine.UI;
using UnityEngine;

namespace Game.UI
{
    public class FateInOutPanel : BasePanel
    {
        [SerializeField] GameObject BackGroundObject;

        [SerializeField] Color BackGroundColor;
        [SerializeField] float changespeed = 4f;

        [SerializeField] float maxAlpha = 1f;

        [SerializeField] bool IsEntering = false;
        [SerializeField] bool IsExiting = false;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            BackGroundColor = BackGroundObject.GetComponent<Image>().color;
            BackGroundColor.a = 1f;
            BackGroundObject.GetComponent<Image>().color = BackGroundColor;
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
            if(IsEntering)
            {
                if(BackGroundColor.a <= maxAlpha)
                {
                    
                    BackGroundColor.a += changespeed * Time.deltaTime;
                    BackGroundObject.GetComponent<Image>().color = BackGroundColor;   
                    Debug.Log(BackGroundColor.a);
                }
                else
                {
                    BackGroundColor.a = maxAlpha;
                    BackGroundObject.GetComponent<Image>().color = BackGroundColor; 
                    IsEntering = false;
                }
            }
            else if(IsExiting)
            {
                if(BackGroundColor.a >= 0)
                {
                    BackGroundColor.a -= changespeed * Time.deltaTime;
                    BackGroundObject.GetComponent<Image>().color = BackGroundColor;   
                }
                else
                {
                    BackGroundColor.a = 0;
                    BackGroundObject.GetComponent<Image>().color = BackGroundColor; 
                    IsExiting = false;
                }
            }
        }
        public void EnterStory(float alpha, float speed)
        {
            maxAlpha = alpha;
            changespeed = speed;
            IsEntering = true;
        }

        public void ExitStory(float speed)
        {
            changespeed = speed;
            IsExiting = true;
        }
    }
}