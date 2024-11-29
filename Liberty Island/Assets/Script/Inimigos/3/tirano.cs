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
    public float meleeAttackRange = 3f; // Alcance para ataque corpo a corpo
    public float rangedAttackRange = 10f; // Alcance para ataque à distância

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

        // Se não está atacando, controlar o movimento e decidir atacar
        if (!isAttacking)
        {
            if (distanceToPlayer <= meleeAttackRange)
            {
                // Para e inicia ataque corpo a corpo
                animator.SetBool("IsRunning", false);
                StartCoroutine(MeleeAttack());
            }
            else if (distanceToPlayer <= rangedAttackRange)
            {
                // Para e inicia ataque à distância
                animator.SetBool("IsRunning", false);
                StartCoroutine(ThrowKnife());
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

        GameObject knife = Instantiate(knifePrefab, knifeSpawnPoint.position, Quaternion.identity);
        Vector2 direction = (player.position - knifeSpawnPoint.position).normalized;
        knife.GetComponent<Rigidbody2D>().velocity = direction * knifeSpeed;

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
        Gizmos.DrawWireSphere(transform.position, meleeAttackRange); // Range corpo a corpo

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangedAttackRange); // Range ataque à distância
    }
}
