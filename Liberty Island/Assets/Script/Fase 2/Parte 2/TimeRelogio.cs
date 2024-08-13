using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRelogio : MonoBehaviour
{
    public float extraTime = 5f; // Tempo extra em segundos

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TimerManager timerManager = FindObjectOfType<TimerManager>();
            if (timerManager != null)
            {
                timerManager.AddTime(extraTime);
            }
            Destroy(gameObject); // Destroi o objeto após ser coletado
        }
    }
}
