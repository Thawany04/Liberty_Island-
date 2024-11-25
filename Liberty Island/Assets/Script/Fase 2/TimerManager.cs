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

    private PlayerController playerController; // Referência ao script do PlayerController

    void Start()
    {
        currentTime = startTime;
        UpdateTimerUI();

        // Encontrar o jogador na cena e obter o PlayerController
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }
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

        // Verifica a vida do jogador e desativa o timer
        if (playerController != null && playerController.vida <= 0)
        {
            timerText.gameObject.SetActive(false);
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
        Gamer_Controler.Instance.GameOver();
    }

    public void AddTime(float extraTime)
    {
        currentTime += extraTime;
    }

    private void OnEnable()
    {
        // Registra o método AddTime para escutar o evento de tempo extra
        TimeObs.timeEvent += AddTime;
    }

    private void OnDisable()
    {
        // Desregistra o método quando o objeto for desativado
        TimeObs.timeEvent -= AddTime;
    }
}