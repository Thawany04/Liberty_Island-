using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tirano : MonoBehaviour
{
    private Animator animator;
    private bool isDead = false;
    private bool facingRight = true; // Para controlar o lado que o inimigo está olhando

    [Header("Configurações do Chefe")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Referências")]
    public Transform player; // Referência ao jogador
    public GameObject knifePrefab; // Prefab da faca
    public Transform knifeSpawnPoint; // Ponto de spawn da faca
    public float knifeSpeed = 10f;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isDead) return;

        // Controle de movimento (Idle ou Run)
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer > 3f)
        {
            animator.SetBool("IsRunning", true);
            MoveTowardsPlayer();
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
    }

    void MoveTowardsPlayer()
    {
        // Movimenta o inimigo em direção ao jogador
        transform.position = Vector2.MoveTowards(transform.position, player.position, 2f * Time.deltaTime);

        // Verifica a direção do jogador
        if (player.position.x < transform.position.x && facingRight)
        {
            Flip();
        }
        else if (player.position.x > transform.position.x && !facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        // Inverte a direção que o inimigo está olhando
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= 1; // Inverte a escala no eixo X
        transform.localScale = scale;
    }

    public void MeleeAttack()
    {
        if (isDead) return;

        animator.SetTrigger("MeleeAttackTrigger");
        // Lógica do ataque corpo a corpo aqui
    }

    public void ThrowKnife()
    {
        if (isDead) return;

        animator.SetTrigger("ThrowKnifeTrigger");
        // Criação e disparo da faca
        GameObject knife = Instantiate(knifePrefab, knifeSpawnPoint.position, Quaternion.identity);
        Vector2 direction = (player.position - knifeSpawnPoint.position).normalized;
        knife.GetComponent<Rigidbody2D>().velocity = direction * knifeSpeed;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        animator.SetTrigger("HitTrigger");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetBool("IsDead", true);
        // Outras lógicas de morte (desativar o chefe, recompensa, etc.)
    }
}
