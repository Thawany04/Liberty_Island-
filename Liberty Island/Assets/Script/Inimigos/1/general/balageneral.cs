using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class balageneral : MonoBehaviour
{
    public float tempoDeVida = 2f;
    public int dano = 1; // Dano causado pelo projétil

    void Start()
    {
        Destroy(gameObject, tempoDeVida); // Destroi o projétil após o tempo de vida
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se o projétil colidiu com o jogador
        if (collision.gameObject.CompareTag("Player"))
        {
            // Acessa o script do jogador e aplica o dano
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Damager(dano); // Aplica o dano ao jogador
            }
        }

        // Destrói o projétil ao colidir com qualquer objeto
        Destroy(gameObject);
    }
}
