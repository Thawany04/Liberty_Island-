using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float lifetime = 5f; // Tempo de vida da bala em segundos
    public int damage = 1; // Quantidade de dano causado pela bala

    private void Start()
    {
        // Inicia uma corrotina para destruir a bala após um tempo
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se a bala colidiu com o jogador
        if (other.CompareTag("Player"))
        {
            // Obtém o script do jogador e aplica dano
            move_pulo player = other.GetComponent<move_pulo>();
            if (player != null)
            {
                player.Damager(damage); // Supondo que Damager é o método para aplicar dano ao jogador
            }

            // Destrói a bala após causar dano
            Destroy(gameObject);
        }
    }
}




