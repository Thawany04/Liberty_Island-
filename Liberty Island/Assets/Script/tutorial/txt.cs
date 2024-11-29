using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class txt : MonoBehaviour
{
    public Text messageText; // Referência ao componente Text
    public string message = "Você atingiu o objeto!"; // Mensagem a ser exibida
    public float displayDuration = 2f; // Tempo que o texto ficará visível

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o objeto que colidiu é o Player
        if (collision.CompareTag("Player"))
        {
            ShowMessage();
        }
    }

    private void ShowMessage()
    {
        if (messageText != null)
        {
            messageText.text = message;
            messageText.enabled = true; // Torna o texto visível
            Invoke(nameof(HideMessage), displayDuration); // Oculta após o tempo especificado
        }
    }

    private void HideMessage()
    {
        if (messageText != null)
        {
            messageText.enabled = false; // Torna o texto invisível novamente
        }
    }
}
