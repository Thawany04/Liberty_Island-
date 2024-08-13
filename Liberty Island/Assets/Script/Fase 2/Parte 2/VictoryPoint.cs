using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Victory();
        }
    }

    void Victory()
    {
        // Carrega a cena de vitória
        SceneManager.LoadScene("SuccessScene");
    }
}
