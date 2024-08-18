using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class danofogo : MonoBehaviour
{
    public float fireDuration = 2f; // Duração do jato de fogo
    public float fireInterval = 3f; // Intervalo entre os jatos de fogo
    public int damage = 1; // Dano causado ao jogador

    private bool isFiring = false;
    private bool hasDamaged = false; // Flag para verificar se o dano já foi aplicado

    private void Start()
    {
        // Iniciar o loop de ativação e desativação do jato de fogo
        StartCoroutine(FireJetLoop());
    }

    private IEnumerator FireJetLoop()
    {
        while (true)
        {
            // Ativar o jato de fogo
            isFiring = true;
            GetComponent<SpriteRenderer>().enabled = true; // Exibe o sprite de fogo
            hasDamaged = false; // Reseta a flag de dano ao ativar o jato
            yield return new WaitForSeconds(fireDuration);

            // Desativar o jato de fogo
            isFiring = false;
            GetComponent<SpriteRenderer>().enabled = false; // Oculta o sprite de fogo
            yield return new WaitForSeconds(fireInterval);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isFiring && collision.CompareTag("Player") && !hasDamaged)
        {
            // Aplique dano ao jogador
            collision.gameObject.GetComponent<move_pulo>().Damager(damage);
            hasDamaged = true; // Marca que o dano já foi aplicado
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hasDamaged = false; // Permite que o dano seja aplicado novamente se o jogador entrar novamente
        }
    }
}
