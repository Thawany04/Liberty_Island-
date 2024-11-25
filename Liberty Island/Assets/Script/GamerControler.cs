using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gamer_Controler : MonoBehaviour
{
    public Text vidatext;
    public static Gamer_Controler Instance { get; private set; }

    public int score, quantidade;
    public Text scoreText;

    public AudioSource audioSource;
    public GameObject PainelGameOver;
    void Awake()
    {
        // Verifica se já existe uma instância do Gamer_Controler
        if (Instance == null)
        {
            // Se não houver, define a instância atual como a única
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantém o GameObject entre cenas
        }
        else
        {
            // Se já houver uma instância, destrói a nova
            Destroy(gameObject);
        }
    }

    public void Updatescore(int value)
    {
        score += value;
        scoreText.text = $"{score}";
    }

    void Start()
    {
        quantidade = GameObject.FindGameObjectsWithTag("coletavel").Length;
        Updatescore(0);
    }

    public void UpdateLives(int value)
    {
        vidatext.text = "x " + value.ToString();
    }

    public void voltamenu()
    {
        SceneManager.LoadScene("Menu");
        PainelGameOver. SetActive(false);
    }
    
    public void reiniciafase()
    {
        // Obtém o índice da cena atual
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Recarrega a cena atual
        SceneManager.LoadScene(currentSceneIndex);
        PainelGameOver. SetActive(false);
    }

    public void GameOver()
    {
        PainelGameOver.SetActive(true);  // Ativa o painel de Game Over
        if (audioSource != null)
        {
            audioSource.Pause();  // Para o som
        }
    }
}