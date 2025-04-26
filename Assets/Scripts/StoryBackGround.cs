using UnityEngine.UI;
using UnityEngine;

namespace Game.UI
{
    public class FadeInOutPanel : BasePanel
    {
        [SerializeField] GameObject BackGroundObject;

        [SerializeField] Color BackGroundColor;
        [SerializeField] float speed = 4f;

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

        private void Update()
        {
            if(IsEntering)
            {
                if(BackGroundColor.a <= maxAlpha)
                {
                    BackGroundColor.a += speed * Time.deltaTime;
                    BackGroundObject.GetComponent<Image>().color = BackGroundColor;   
                    Debug.Log(BackGroundColor.a);
                }
                else
                {
                    IsEntering = false;
                }
            }
            else if(IsExiting)
            {
                if(BackGroundColor.a >= 0)
                {
                    BackGroundColor.a -= speed * Time.deltaTime;
                    BackGroundObject.GetComponent<Image>().color = BackGroundColor;   
                }
                else
                {
                    IsExiting = false;
                }
            }
        }
        public void EnterStory(float alpha)
        {
            maxAlpha = alpha;
            IsEntering = true;
        }

        public void ExitStory()
        {
            IsExiting = true;
        }
    }
}