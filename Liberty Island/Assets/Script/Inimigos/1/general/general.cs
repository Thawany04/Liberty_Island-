using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class General : MonoBehaviour
{
    public float moveSpeed = 3f; // Velocidade de movimento na fase 1
    public float waitTime = 1.5f; // Tempo de espera ao atingir os extremos
    public float attackInterval = 5f; // Intervalo entre ataques na fase 1
    public Transform pointA; // Ponto A (limite esquerdo)
    public Transform pointB; // Ponto B (limite direito)
    public GameObject bulletPrefab; // Prefab do projétil
    public Transform firePoint; // Ponto de origem do tiro
    public Transform player; // Referência ao jogador

    private Vector3 targetPosition; // Posição alvo atual
    private bool isWaiting = false; // Indica se o boss está esperando
    private bool isAttacking = false; // Indica se o boss está atacando
    private Animator animator; // Referência ao Animator
    private float attackTimer; // Temporizador de ataque
    private Vector3 originalScale; // Escala original do General

    public Slider healthBar; // Referência à barra de vida
    public int maxHealth = 100; // Vida máxima do General
    private int currentHealth; // Vida atual do General

    private bool isPhase2 = false; // Indica se o boss está na fase 2
    public float phase2SpeedMultiplier = 1.5f; // Multiplicador de velocidade na fase 2
    public float phase2AttackSpeedMultiplier = 0.5f; // Multiplicador de velocidade de ataque na fase 2

    void Start()
    {
        targetPosition = pointB.position;
        animator = GetComponent<Animator>(); // Pega a referência do Animator
        SetWalking(true); // Começa andando
        attackTimer = attackInterval; // Inicializa o temporizador de ataque

        originalScale = transform.localScale;
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        
    }
    

    void Update()
    {
        // Atualiza o temporizador de ataque
        attackTimer -= Time.deltaTime;

        // Verifica se é hora de atacar e se não está atacando
        if (attackTimer <= 0f && !isAttacking)
        {
            StartCoroutine(AttackPlayer());
            attackTimer = attackInterval; // Reinicia o temporizador de ataque
        }

        // Movimento se não estiver esperando ou atacando
        if (!isWaiting && !isAttacking)
        {
            Move();
        }

        // Ajusta a direção baseado no movimento do eixo X
        FlipBasedOnMovement();
    }

    void Move()
    {
        if (isAttacking || isWaiting) return; // Não se move durante o ataque ou a espera

        // Multiplica a velocidade dependendo da fase
        float currentMoveSpeed = isPhase2 ? moveSpeed * phase2SpeedMultiplier : moveSpeed;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentMoveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            StartCoroutine(WaitBeforeTurning());
        }
    }

    IEnumerator AttackPlayer()
    {
        // Verifica se o boss está olhando para o jogador
        if (!IsFacingPlayer())
        {
            FlipTowardsPlayer(); // Vira o boss para o jogador
            // Espera um pouco para garantir que o boss tenha tempo de virar
            yield return new WaitForSeconds(0.2f); // Ajuste conforme necessário
        }

        // Após virar, atira
        if (IsPlayerInFront())
        {
            isAttacking = true;
            SetWalking(false); // Para o movimento

            SetShooting(true); // Ativa a animação de atirar

            // Espera a animação de atirar iniciar (ajuste o tempo conforme a duração da animação)
            yield return new WaitForSeconds(0.5f);

            // Instancia o projétil
            Shoot();

            // Espera um tempo extra após disparar
            yield return new WaitForSeconds(1f); // Tempo para a bala "sair direito"

            SetShooting(false); // Desativa a animação de atirar
            SetWalking(true); // Volta para a animação de andar
            isAttacking = false;
        }
        else
        {
            // Se o jogador não estiver na frente, apenas continua andando
            isAttacking = false;
        }
    }



    bool IsPlayerInFront()
    {
        if (player != null)
        {
            float directionToPlayer = player.position.x - transform.position.x;

            // Verifica se o jogador está à frente, dependendo da direção do General
            return (directionToPlayer > 0 && transform.localScale.x > 0) || // Jogador à direita e General virado para a direita
                   (directionToPlayer < 0 && transform.localScale.x < 0);   // Jogador à esquerda e General virado para a esquerda
        }
        return false;
    }

    void Shoot()
    {
        // Cria o projétil no ponto de disparo
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Verifica se o projétil tem um Rigidbody2D para aplicar velocidade
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Define a direção do projétil com base na orientação do General
            float bulletDirection = transform.localScale.x > 0 ? 1f : -1f;
            rb.velocity = new Vector2(bulletDirection * 10f, 0f);
        }
    }

    void FlipTowardsPlayer()
    {
        if (player != null)
        {
            Vector3 directionToPlayer = player.position - transform.position;

            // Verifica se o jogador está à direita ou à esquerda e vira o boss na direção dele
            if (directionToPlayer.x > 0 && transform.localScale.x < 0 ||
                directionToPlayer.x < 0 && transform.localScale.x > 0)
            {
                Flip(); // Vira o boss
            }
        }
    }


    bool IsFacingPlayer()
    {
        if (player != null)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            // Verifica se o boss está olhando na direção do jogador
            return (directionToPlayer.x > 0 && transform.localScale.x > 0) ||
                   (directionToPlayer.x < 0 && transform.localScale.x < 0);
        }
        return false;
    }


    IEnumerator WaitBeforeTurning()
    {
        isWaiting = true;
        SetWalking(false); // Troca para a animação de parado
        yield return new WaitForSeconds(waitTime);

        targetPosition = (targetPosition == pointA.position) ? pointB.position : pointA.position;
        isWaiting = false;
        SetWalking(true);
    }

    void Flip()
    {
        transform.localScale = new Vector3(
            -transform.localScale.x,
            transform.localScale.y,
            transform.localScale.z
        );
    }

    void FlipBasedOnMovement()
    {
        float movementDirection = targetPosition.x - transform.position.x;

        if (movementDirection > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        }
        else if (movementDirection < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        }
    }

    void SetWalking(bool isWalking)
    {
        animator.SetBool("isWalking", isWalking);
    }

    void SetShooting(bool isShooting)
    {
        animator.SetBool("isShooting", isShooting);
    }

    public void ReceberDano(int damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;

        // Verifica se a saúde do boss caiu para metade ou menos da vida máxima
        if (currentHealth <= maxHealth / 2 && !isPhase2)
        {
            ActivatePhase2(); // Ativa a fase 2
        }

        if (currentHealth <= 0)
        {
            Die();
            SceneManager.LoadScene("Fase 2");
        }
    }
    
    void Die()
    {
        Debug.Log("General derrotado!");
        Destroy(gameObject);
    }
    

    // Método para ativar a fase 2
    public void ActivatePhase2()
    {
        isPhase2 = true;
        attackInterval *= phase2AttackSpeedMultiplier; // Acelera os ataques
    }

    // Método para desativar a fase 2
    public void DeactivatePhase2()
    {
        isPhase2 = false;
        attackInterval /= phase2AttackSpeedMultiplier; // Restaura o intervalo de ataque
    }

   /* private void OnCollisionEnter2D(Collision2D coll)
    {
        PlayerController player = coll.gameObject.GetComponent<PlayerController>();
        if (coll.gameObject.CompareTag("Player"))
        {
            player.Damager(1);
        }
    }*/
    
    public void DesativarBarraDeVida()
    {
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(false);
        }
    }
    
}
