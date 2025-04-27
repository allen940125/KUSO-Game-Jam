using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private int targetLevel;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TeleportManager.Instance.TeleportWithFade(targetLevel);
        }
    }
}
