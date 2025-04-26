using UnityEngine.UI;
using UnityEngine;

namespace Game.UI
public class StoryBackGround : BasePanel
{
    [SerializeField] GameObject BackGroundObject;

    private Color BackGroundColor;
    [SerializeField] float speed = 4f;

    [SerializeField] bool IsEntering = false;
    [SerializeField] bool IsExiting = false;

    private void Start()
    {
        BackGroundColor = BackGroundObject.GetComponent<Image>().color;
        BackGroundColor.a = 1f;
        BackGroundObject.GetComponent<Image>().color = BackGroundColor;
    }


    private void Update()
    {
        if(IsEntering)
        {
            if(BackGroundColor.a <= 1)
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

    public void EnterStory()
    {
        IsEntering = true;
    }

    public void ExitStory()
    {
        IsExiting = true;
    }

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
}