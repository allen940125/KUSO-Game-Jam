using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    [SerializeField] private float multiplier;
    private float moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    
    private void Awake()
    {
        GameManager.Instance.Player = this.gameObject;
    }

    void Start()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
         rb = GetComponent<Rigidbody2D>();
         animator = GetComponentInChildren<Animator>();
         GameManager.Instance.MainGameMediator.RealTimePlayerData.SuspicionValue = 0;
    }

    void Update()
    {
        if(GameManager.Instance.MainGameMediator.RealTimePlayerData.CanPlayerMove)
        {
            moveInput = Input.GetAxis("Horizontal"); 
        }
        else
        {
            moveInput = 0;
        }

        if (Input.GetKey(KeyCode.W))
        {
            Debug.Log("興憤值" + GameManager.Instance.MainGameMediator.RealTimePlayerData.ExcitementValue);
            Debug.Log("懷疑值" + GameManager.Instance.MainGameMediator.RealTimePlayerData.SuspicionValue);
        }
        
        rb.linearVelocityX = moveInput * speed * multiplier;
        if (UnityEngine.Input.GetKeyDown(KeyCode.L))
        {
            FindFirstObjectByType<VideoManager>().PlayVideo("TVError",true);
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.O))
        {
            FindFirstObjectByType<VideoManager>().PlayVideo("JumpScare",false);
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.P))
        {
            FindFirstObjectByType<VideoManager>().PlayVideo("POPOPO",false);
        }
    }

    void FixedUpdate()
    {
         
        if(Mathf.Abs(moveInput) > 0.01f)
        {
            animator.SetBool("walking",true);
        }
        else
        {
            animator.SetBool("walking",false);
        }
        
    }
    
    public void SetMultiplier(float target)
    {
        multiplier = target;
    }
}
