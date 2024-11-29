using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI; // Necessário para manipular UI

public class pause : MonoBehaviour
{
    public Text messageText; // Referência ao texto na tela
    public float displayTime = 2f; // Tempo que o texto será exibido

    private bool isPaused = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isPaused) // Verifica colisão com o jogador
        {
            StartCoroutine(PauseGame());
        }
    }

    private IEnumerator PauseGame()
    {
        // Pausa o jogo
        isPaused = true;
        Time.timeScale = 0; // Congela o tempo no jogo

        // Exibe o texto
        messageText.enabled = true;

        // Espera o tempo definido (corrigido para Time.timeScale = 0)
        float elapsedTime = 0f;
        while (elapsedTime < displayTime)
        {
            elapsedTime += Time.unscaledDeltaTime; // Usa UnscaledDeltaTime para ignorar Time.timeScale
            yield return null;
        }

        // Esconde o texto e despausa o jogo
        messageText.enabled = false;
        Time.timeScale = 1; // Retoma o tempo do jogo
        isPaused = false;
    }
}
