using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    public int damage = 1; // Dano causado pela faca

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Aplicar dano ao jogador
            collision.GetComponent<PlayerController>().Damager(1);
            Destroy(gameObject);
        }

        // Destruir faca ao colidir com algo
        if (collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Destruir a faca após certo tempo para evitar desperdício de memória
        Destroy(gameObject, 5f);
    }
}
