using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    public GameObject optionsPanel; // Referência ao painel de opções
    public GameObject controlPanel; // Referência ao painel de controle
    public GameObject creditsPanel; // Referência ao painel de créditos

    // Função para iniciar o jogo
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("cutscine 1");
    }

    // Função para abrir as opções
    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
    }

    // Função para fechar as opções
    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        controlPanel.SetActive(false); // Esconde painel de controle
        creditsPanel.SetActive(false); // Esconde painel de créditos
    }

    // Função para abrir o painel de controle
    public void OpenControlPanel()
    {
        controlPanel.SetActive(true);
        optionsPanel.SetActive(false); // Esconde o painel de opções
    }

    // Função para abrir o painel de créditos
    public void OpenCreditsPanel()
    {
        creditsPanel.SetActive(true);
        optionsPanel.SetActive(false); // Esconde o painel de opções
    }

    // Função para voltar ao menu principal a partir de controles ou créditos
    public void ReturnToOptions()
    {
        optionsPanel.SetActive(true);
        controlPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

}

    

