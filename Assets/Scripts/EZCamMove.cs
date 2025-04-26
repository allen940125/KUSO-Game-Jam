using UnityEngine;

public class EZCamMove : MonoBehaviour
{
    //Only for Testing this Parallax Script
    public float speed = 5f;
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        transform.Translate(new Vector3(horizontal * speed * Time.deltaTime,0, 0));
    }
}