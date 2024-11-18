using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class general : MonoBehaviour
{
   
    public float moveSpeed = 3f; // Velocidade de movimento do boss
    public float waitTime = 1.5f; // Tempo de espera ao atingir os extremos
    public float attackInterval = 5f; // Intervalo entre ataques
    public Transform pointA; // Ponto A (limite esquerdo)
    public Transform pointB; // Ponto B (limite direito)
    public GameObject bulletPrefab; // Prefab do projétil
    public Transform firePoint; // Ponto de origem do tiro
    public Transform player; // Referência ao jogador

    private Vector3 targetPosition; // Posição alvo atual
    private bool facingRight = true; // Para controlar a direção
    private bool isWaiting = false; // Indica se o boss está esperando
    private bool isAttacking = false; // Indica se o boss está atacando
    private Animator animator; // Referência ao Animator
    private float attackTimer; // Temporizador de ataque

    void Start()
    {
        targetPosition = pointB.position;
        animator = GetComponent<Animator>(); // Pega a referência do Animator
        SetWalking(true); // Começa andando
        attackTimer = attackInterval; // Inicializa o temporizador de ataque
    }

    void Update()
    {
        // Atualiza o temporizador de ataque
        attackTimer -= Time.deltaTime;

        // Verifica se é hora de atacar
        if (attackTimer <= 0f && !isAttacking)
        {
            StartCoroutine(AttackPlayer());
            attackTimer = attackInterval; // Reinicia o temporizador de ataque
        }

        // Movimento se não estiver esperando ou atacando
        if (!isWaiting && !isAttacking)
        {
            // Move o boss em direção à posição alvo
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Verifica se chegou ao ponto alvo
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                // Inicia a espera e troca o ponto alvo
                StartCoroutine(WaitBeforeTurning());
            }
        }
    }

    // Coroutine para atacar o jogador
    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        SetWalking(false); // Para o movimento
        SetShooting(true); // Ativa a animação de atirar

        // Gira o general para o lado do jogador
        FlipTowardsPlayer();

        // Espera a animação de atirar iniciar (ajuste o tempo conforme a duração da animação)
        yield return new WaitForSeconds(0.5f);

        // Instancia o projétil
        Shoot();

        // Espera a animação de atirar finalizar (ajuste o tempo conforme a duração da animação)
        yield return new WaitForSeconds(0.5f);

        SetShooting(false); // Desativa a animação de atirar
        SetWalking(true); // Volta para a animação de andar
        isAttacking = false;
    }

    void Shoot()
    {
        // Cria o projétil no ponto de disparo
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Verifica se o projétil tem um Rigidbody2D para aplicar velocidade
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Se estiver virado para a direita, a velocidade será positiva, caso contrário negativa
            rb.velocity = facingRight ? firePoint.right * 10f : -firePoint.right * 10f;
        }
    }




    // Método para girar o general para o lado do jogador
    void FlipTowardsPlayer()
    {
        if (player != null)
        {
            // Verifica a posição do jogador em relação ao general
            if (player.position.x > transform.position.x && !facingRight)
            {
                Flip(); // Gira o general para a direita
            }
            else if (player.position.x < transform.position.x && facingRight)
            {
                Flip(); // Gira o general para a esquerda
            }
        }
    }

    // Coroutine para esperar antes de inverter a direção
    IEnumerator WaitBeforeTurning()
    {
        isWaiting = true;
        SetWalking(false); // Troca para a animação de parado
        yield return new WaitForSeconds(waitTime); // Espera pelo tempo definido

        // Troca o ponto alvo quando terminar a espera
        if (targetPosition == pointA.position)
        {
            targetPosition = pointB.position;
        }
        else
        {
            targetPosition = pointA.position;
        }

        // Inverte a direção
        Flip();
        SetWalking(true); // Volta para a animação de andar
        isWaiting = false;
    }

    
    // Método para inverter a direção
    void Flip()
    {
        facingRight = !facingRight;

        // Inverte a escala no eixo X do general
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        // O firePoint agora já está ajustado corretamente com base na inversão da escala
    }


    // Método para definir o estado de movimento no Animator
    void SetWalking(bool isWalking)
    {
        animator.SetBool("isWalking", isWalking);
    }

    // Método para definir o estado de ataque no Animator
    void SetShooting(bool isShooting)
    {
        animator.SetBool("isShooting", isShooting);
    }

}





