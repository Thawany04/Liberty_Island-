using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Tirano : MonoBehaviour
{
    private Animator animator;
    private bool isDead = false;
    private bool facingRight = true;
    private bool isPhaseTwo = false; // Controle da fase dois

    [Header("Configurações do Chefe")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("UI")]
    public Slider healthBar; // Referência ao Slider de vida

    [Header("Referências")]
    public Transform player;
    public GameObject knifePrefab;
    public Transform knifeSpawnPoint;
    public float knifeSpeed = 10f;
    public Collider2D meleeCollider;
    public int meleeDamage = 10;

    [Header("Configurações de Ataque")]
    public float meleeAttackRange = 3f;
    public float rangedAttackRange = 10f;
    public float rangedAttackCooldown = 3f;
    private float rangedAttackCooldownTimer = 0f;

    private bool isAttacking = false;
    private bool alternateAttack = true;

    [Header("Movimento")]
    public float moveSpeed = 2f; // Velocidade normal
    public float phaseTwoMoveSpeed = 3.5f; // Velocidade na fase dois

    [Header("Ataques")]
    public float normalAttackCooldown = 3f;
    public float phaseTwoAttackCooldown = 2f;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        // Configura o slider de vida
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
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

    // Verifica se deve mudar para a fase dois
    if (!isPhaseTwo && currentHealth <= maxHealth / 2)
    {
        EnterPhaseTwo();
    }

    // Verifica a direção do jogador antes de agir
    CheckDirection();

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
            rangedAttackCooldownTimer = isPhaseTwo ? phaseTwoAttackCooldown : normalAttackCooldown;
        }
        else
        {
            // Move em direção ao jogador
            animator.SetBool("IsRunning", true);
            MoveTowardsPlayer();
        }
    }
}

void CheckDirection()
{
    // Gira para o jogador se necessário
    if ((player.position.x < transform.position.x && facingRight) ||
        (player.position.x > transform.position.x && !facingRight))
    {
        Flip();
    }
}

IEnumerator MeleeAttack()
{
    isAttacking = true;
    CheckDirection(); // Garante que o chefe esteja virado para o jogador
    animator.SetTrigger("MeleeAttackTrigger");

    yield return new WaitForSeconds(0.5f);
    ApplyMeleeDamage();

    yield return new WaitForSeconds(0.5f);
    ApplyMeleeDamage();

    yield return new WaitForSeconds(1f);
    isAttacking = false;
}

IEnumerator ThrowKnife()
{
    isAttacking = true;
    CheckDirection(); // Garante que o chefe esteja virado para o jogador
    animator.SetTrigger("ThrowKnifeTrigger");

    GameObject knife = Instantiate(knifePrefab, knifeSpawnPoint.position, Quaternion.identity);

    float direction = Mathf.Sign(transform.localScale.x);
    Vector3 knifeScale = knife.transform.localScale;
    knifeScale.x = Mathf.Abs(knifeScale.x) * direction;
    knife.transform.localScale = knifeScale;

    knife.GetComponent<Rigidbody2D>().velocity = new Vector2(direction * knifeSpeed, 0);

    yield return new WaitForSeconds(1f);
    isAttacking = false;
}

    void MoveTowardsPlayer()
    {
        float speed = isPhaseTwo ? phaseTwoMoveSpeed : moveSpeed;

        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

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
    
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (healthBar != null)
        {
            healthBar.value = currentHealth; // Atualiza o slider de vida
        }

        animator.SetTrigger("HitTrigger");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void EnterPhaseTwo()
    {
        isPhaseTwo = true;
        animator.SetTrigger("PhaseTwoTrigger");
        // Outras mudanças visuais ou de comportamento podem ser adicionadas aqui
    }

    void Die()
    {
        isDead = true;
        animator.SetBool("IsDead", true);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeAttackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangedAttackRange);
    }
}
