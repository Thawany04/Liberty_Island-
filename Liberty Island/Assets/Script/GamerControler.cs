using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gamer_Controler : MonoBehaviour
{
    public Text vidatext;
    public static Gamer_Controler instace;

    public int score, quantidade;
    public Text scoreText;
    public TextMeshProUGUI scoreTextPro;
    
    
    public void Updatescore(int value)
    {
        score += value;
        scoreText.text = $"{score} / {quantidade }";
    }
    void Start()
    {
        instace = this;
        quantidade = GameObject.FindGameObjectsWithTag("coletavel").Length;
        Updatescore(0);
    }

    public void UpdateLives(int value)
    {
        vidatext.text = "x " + value.ToString();
    }
    private void Update()
    {
        if (score>= 3)
        {
            SceneManager.LoadScene(1);
        }
    }
}


