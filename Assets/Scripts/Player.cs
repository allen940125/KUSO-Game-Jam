using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    public float speed = 10;
    private float moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    void Start()
    {
         rb = GetComponent<Rigidbody2D>();
         animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        moveInput = Input.GetAxis("Horizontal"); 
        rb.linearVelocityX = moveInput * speed;
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
    

}
