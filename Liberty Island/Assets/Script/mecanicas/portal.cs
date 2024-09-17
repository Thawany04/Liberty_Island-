using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class portal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Chama o método 'LoadScene' após 0,5 segundos de atraso
            Invoke("LoadScene", 0f);
        }
    }
    
    // Método que carrega a cena após o atraso
    private void LoadScene()
    {
        SceneManager.LoadScene("part 2 fase 2");
    }
}


