using System;
using UnityEngine;

public class EndDoor : MonoBehaviour
{
    [SerializeField] private int targetLevel;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
        }
    }
}
