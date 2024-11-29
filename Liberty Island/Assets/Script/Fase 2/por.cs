using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class por : MonoBehaviour
{
    [SerializeField] private string nomeDaProximaFase; // Nome da próxima fase (definido no Inspector ou no código)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o objeto que colidiu tem a tag "Player"
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Jogador colidiu, carregando próxima fase: " + nomeDaProximaFase);
            SceneManager.LoadScene(nomeDaProximaFase); // Carrega a cena especificada
        }
    }
}
