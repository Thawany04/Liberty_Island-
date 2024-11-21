using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int dano = 10; // Dano causado pela bala

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se a bala colidiu com um inimigo
        if (collision.gameObject.CompareTag("inimigo"))
        {
            // Pega o script do inimigo e aplica dano
            sd inimigo = collision.gameObject.GetComponent<sd>();
            if (inimigo != null)
            {
                inimigo.ReceberDano(dano);
            }
        }

        // Verifica se a bala colidiu com um boss
        if (collision.gameObject.CompareTag("boss"))
        {
            // Pega o script do boss e aplica dano
            boss2 boss = collision.gameObject.GetComponent<boss2>();
            if (boss != null)
            {
                boss.TakeDamage(dano);
            }
        }

        // Destruir a bala ao colidir com qualquer objeto
        Destroy(gameObject);
    }
}