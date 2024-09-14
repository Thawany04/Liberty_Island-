using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dois : MonoBehaviour
{
    public float speed = 2f; // Velocidade de movimento do inimigo
    public float followRange = 5f; // Distância em que o inimigo começa a seguir o jogador
    public float attackRange = 1f; // Distância em que o inimigo começa a atacar o jogador
    public LayerMask playerLayer; // Layer do jogador
    public float groundCheckRadius = 0.2f; // Raio para verificar se o inimigo está no chão
    public Transform groundCheck; // Ponto de verificação do chão
    public float attackDelay = 0.3f; // Tempo de espera antes de atacar
    public GameObject bulletPrefab; // Prefab da bala
    public Transform firePoint; // Ponto de disparo da bala
    public float bulletSpeed = 5f; // Velocidade da bala
    public float shootCooldown = 2f; // Tempo de espera entre os tiros

    private GameObject player; // Referência ao jogador
    private Rigidbody2D rb; // Referência ao Rigidbody2D do inimigo
    public int currentHealth = 10; // Vida atual do inimigo
    private bool isGrounded; // Verifica se o inimigo está no chão
    private bool canShoot = true; // Controla quando o inimigo pode atirar
    public bool IsAtacking = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Encontrar o jogador pela tag
        rb = GetComponent<Rigidbody2D>(); // Obter o componente Rigidbody2D
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, LayerMask.GetMask("chao"));

        if (player != null && IsPlayerVisible())
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= attackRange && !IsAtacking)
            {
                StartCoroutine(AttackPlayerCoroutine()); // Ataca o jogador se estiver próximo
            }
            else if (distanceToPlayer <= followRange)
            {
                FollowPlayer(); // Segue o jogador se estiver dentro do alcance
                if (canShoot) // Adiciona a lógica para disparar se o inimigo estiver dentro do alcance e pode atirar
                {
                    StartCoroutine(Shoot());
                }
            }
            else
            {
                StopMovement(); // Para o movimento quando o jogador estiver fora do alcance
            }
        }
        else
        {
            StopMovement(); // Para o movimento se o jogador não estiver visível
        }

        if (!isGrounded)
        {
            StopMovement(); // Para o movimento se o inimigo não estiver no chão
        }
    }

    void FollowPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized; // Calcula a direção para o jogador
        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y); // Define a velocidade do Rigidbody2D para seguir o jogador, apenas no eixo X
    }

    void StopMovement()
    {
        rb.velocity = new Vector2(0, rb.velocity.y); // Para o movimento do inimigo no eixo X
    }

    IEnumerator AttackPlayerCoroutine()
    {
        IsAtacking = true;
        yield return new WaitForSeconds(attackDelay);
        // Animação do ataque aqui
        move_pulo playerScript = player.GetComponent<move_pulo>();
        if (playerScript != null)
        {
            playerScript.Damager(1); // Certifique-se de que o método Damager no script do jogador aplica dano
        }
        yield return new WaitForSeconds(1f);
        IsAtacking = false;
    }

    bool IsPlayerVisible()
    {
        return (player.gameObject.layer == Mathf.Log(playerLayer.value, 2)); // Verifica se o jogador está na Layer correta
    }

    IEnumerator Shoot()
    {
        canShoot = false;

        // Calcula a direção para o jogador
        Vector2 direction = (player.transform.position - firePoint.position).normalized;

        // Cria a bala no firePoint
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Ajusta a velocidade da bala
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = direction * bulletSpeed;

        // Espera pelo tempo de cooldown
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("bala"))
        {
            TakeDamage(2);
            Destroy(col.gameObject);
        }
    }

    void Die()
    {
        Destroy(gameObject); // Destroi o inimigo
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, followRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}


