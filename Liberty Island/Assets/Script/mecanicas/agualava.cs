using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class agualava : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Chama o método 'LoadScene' após 1 segundo de atraso
            Invoke("LoadScene", 0.5f);
        }
    }
    
    // Método que carrega a cena após o atraso
    private void LoadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("ExplosionScene");
    }
}
