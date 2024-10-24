using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class espinho : MonoBehaviour
{
    public int dano = 10; // Define o valor do dano que o espinho causa
    public float intervaloDano = 1.0f; // Intervalo entre os danos causados pelo espinho

    private bool jogadorEmCima = false; // Verifica se o jogador está em cima do espinho
    private float tempoUltimoDano = 0.0f; // Tempo desde o último dano

    void Update()
    {
        // Verifica se o jogador está em cima do espinho e o intervalo de dano
        if (jogadorEmCima && Time.time - tempoUltimoDano >= intervaloDano)
        {
            AplicarDano();
            
            tempoUltimoDano = Time.time;
            
        }
    }

    private void OnCollisionEnter2D(Collision2D colisao)
    {
        // Verifica se o objeto que entrou em contato é o jogador
        if (colisao.gameObject.CompareTag("Player"))
        {
            jogadorEmCima = true;
            AplicarDano(); // Aplica dano imediatamente se o jogador entrar em contato
        }
    }

    private void OnCollisionExit2D(Collision2D colisao)
    {
        // Verifica se o objeto que saiu de contato é o jogador
        if (colisao.gameObject.CompareTag("Player"))
        {
            jogadorEmCima = false;
        }
    }

    private void AplicarDano()
    {
        // Obtém o componente `move_pulo` do jogador e aplica dano
        GameObject jogador = GameObject.FindGameObjectWithTag("Player");
        if (jogador != null)
        {
            PlayerController jogadorMovePulo = jogador.GetComponent<PlayerController>();
            if (jogadorMovePulo != null)
            {
                jogadorMovePulo.Damager(dano); // Aplica o dano usando o método Damager
               
            }
        }
    }
}
