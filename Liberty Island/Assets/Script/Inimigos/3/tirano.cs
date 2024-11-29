using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tirano : MonoBehaviour
{
    private Animator animator;
    private bool isDead = false;
    private bool facingRight = true;

    [Header("Configurações do Chefe")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Referências")]
    public Transform player;
    public GameObject knifePrefab;
    public Transform knifeSpawnPoint;
    public float knifeSpeed = 10f;
    public Collider2D meleeCollider;
    public int meleeDamage = 10;

    [Header("Alcances de Ataque")]
    public float meleeAttackRange = 3f;
    public float rangedAttackRange = 10f;

    [Header("Cooldowns")]
    public float rangedAttackCooldown = 3f; // Tempo de cooldown do ataque à distância
    private float rangedAttackCooldownTimer = 0f; // Temporizador do cooldown

    private bool isAttacking = false;
    private bool alternateAttack = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isDead) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Atualiza o temporizador do cooldown
        if (rangedAttackCooldownTimer > 0)
        {
            rangedAttackCooldownTimer -= Time.deltaTime;
        }

        // Se não está atacando, controlar o movimento e decidir atacar
        if (!isAttacking)
        {
            if (distanceToPlayer <= meleeAttackRange)
            {
                // Ataque corpo a corpo
                animator.SetBool("IsRunning", false);
                StartCoroutine(MeleeAttack());
            }
            else if (distanceToPlayer <= rangedAttackRange && rangedAttackCooldownTimer <= 0f)
            {
                // Ataque à distância
                animator.SetBool("IsRunning", false);
                StartCoroutine(ThrowKnife());
                rangedAttackCooldownTimer = rangedAttackCooldown; // Ativa o cooldown
            }
            else
            {
                // Move em direção ao jogador
                animator.SetBool("IsRunning", true);
                MoveTowardsPlayer();
            }
        }
    }

    void MoveTowardsPlayer()
    {
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
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    IEnumerator MeleeAttack()
    {
        isAttacking = true;
        animator.SetTrigger("MeleeAttackTrigger");

        yield return new WaitForSeconds(0.5f);
        ApplyMeleeDamage();

        yield return new WaitForSeconds(0.5f);
        ApplyMeleeDamage();

        yield return new WaitForSeconds(1f); // Pausa após o ataque
        isAttacking = false;
    }

    void ApplyMeleeDamage()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(meleeCollider.bounds.center, meleeCollider.bounds.size, 0);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                hit.GetComponent<PlayerController>().Damager(meleeDamage);
            }
        }
    }

    IEnumerator ThrowKnife()
    {
        isAttacking = true;
        animator.SetTrigger("ThrowKnifeTrigger");

        // Criação da faca
        GameObject knife = Instantiate(knifePrefab, knifeSpawnPoint.position, Quaternion.identity);

        // Determina a direção baseada no lado que o chefe está olhando
        float direction = Mathf.Sign(transform.localScale.x); // Direção: -1 para esquerda, 1 para direita

        // Ajusta a escala da faca para que ela "vire" para o lado correto
        Vector3 knifeScale = knife.transform.localScale;
        knifeScale.x = Mathf.Abs(knifeScale.x) * direction; // Inverte a escala X se necessário
        knife.transform.localScale = knifeScale;

        // Define a velocidade da faca
        knife.GetComponent<Rigidbody2D>().velocity = new Vector2(direction * knifeSpeed, 0);

        yield return new WaitForSeconds(1f); // Pausa após o ataque
        isAttacking = false;
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
    }

    // Gizmos para visualizar os alcances de ataque
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeAttackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangedAttackRange);
    }
}
