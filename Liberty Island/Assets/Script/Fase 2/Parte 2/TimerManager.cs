using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    public float startTime = 60f; // Tempo inicial em segundos
    private float currentTime;
    public Text timerText; // Referência ao componente de texto na UI

    void Start()
    {
        currentTime = startTime;
    }

    void Update()
    {
        currentTime -= Time.deltaTime;
        UpdateTimerUI();

        if (currentTime <= 0)
        {
            currentTime = 0;
            Explode();
        }
    }

    void UpdateTimerUI()
    {
        // Formata o tempo em minutos e segundos
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void Explode()
    {
        // Carrega a cena de explosão
        SceneManager.LoadScene("ExplosionScene");
    }

    public void AddTime(float extraTime)
    {
        currentTime += extraTime;
    }
}
