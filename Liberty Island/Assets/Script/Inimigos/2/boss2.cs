using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss2 : MonoBehaviour
{
    public Transform player; // Referência ao jogador
    [SerializeField] private float attackRange = 3f; // Distância de ataque visível no Inspector
    public float speed = 2f; // Velocidade de movimento inicial
    public int maxHealth = 100;
    public int damage = 10;
    public int currentHealth;

    public HealthBar vidaboss; // Referência à barra de vida

    private bool isAttacking = false; // Verifica se está atacando
    private bool isEnraged = false; // Verifica se entrou no modo raivoso
    private bool isWalking = false; // Verifica se está andando

    private Animator animator;
    private AudioSource audioSource;
    public AudioClip enrageSound; // Som para o grito de raiva
    public AudioClip attackSound; // Som para o ataque
    public AudioClip walkSound; // Som para os passos

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>(); // Referência ao Animator
        audioSource = GetComponent<AudioSource>(); // Referência ao AudioSource

        // Atualiza a barra de vida no início
        if (vidaboss != null)
        {
            vidaboss.UpdateHealthBar(); 
        }
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Rotaciona o inimigo para "olhar" para o jogador
        FacePlayer();

        if (distanceToPlayer <= attackRange && !isAttacking)
        {
            StartAttack();
        }
        else if (distanceToPlayer > attackRange && isAttacking)
        {
            StopAttack();
        }

        if (!isAttacking)
        {
            FollowPlayer();
        }
    }

    private void FollowPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        if (!isWalking)
        {
            StartWalking();
        }

        animator.SetBool("isAttacking", false); // Para a animação de ataque enquanto persegue
    }

    private void FacePlayer()
    {
        // Se o jogador está à direita do inimigo, ele se vira para a direita; caso contrário, vira à esquerda
        if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void StartWalking()
    {
        isWalking = true;
        animator.SetBool("isWalking", true); // Ativa a animação de andar
        PlaySoundLoop(walkSound); // Inicia o som de passos em loop
    }

    private void StopWalking()
    {
        isWalking = false;
        animator.SetBool("isWalking", false); // Para a animação de andar
        audioSource.Stop(); // Para o som de passos
    }

    private void StartAttack()
    {
        isAttacking = true;
        StopWalking(); // Para o som de passos ao atacar
        animator.SetBool("isAttacking", true); // Ativa a animação de ataque
        InvokeRepeating("AttackPlayer", 0f, 1f); // Ataca a cada segundo
    }

    private void StopAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", false); // Para a animação de ataque
        CancelInvoke("AttackPlayer");
        if (Vector2.Distance(transform.position, player.position) > attackRange)
        {
            StartWalking();
        }
    }

    private void AttackPlayer()
    {
        player.GetComponent<PlayerController>().Damager(damage);
        PlaySound(attackSound); // Reproduz o som de ataque
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= maxHealth / 2 && !isEnraged)
        {
            Enrage();
        }

        if (currentHealth <= 0)
        {
            Die();
        }

        // Atualiza a barra de vida
        if (vidaboss != null)
        {
            vidaboss.UpdateHealthBar(); // Atualiza a barra de vida sempre que o boss toma dano
        }
    }

    private void Enrage()
    {
        isEnraged = true;
        speed *= 1.5f; // Aumenta a velocidade em 50%
        damage += 10;  // Aumenta o dano
        animator.SetBool("isEnraged", true); // Ativa a animação de raiva/enrage
        PlaySound(enrageSound); // Reproduz o som de raiva
        Debug.Log("Grito de raiva!");
    }

    private void Die()
    {
        animator.SetTrigger("Die"); // Animação de morte
        Destroy(gameObject, 2f); // Destroi o boss após 2 segundos
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource && clip)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void PlaySoundLoop(AudioClip clip)
    {
        if (audioSource && clip)
        {
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    // Desenha o Gizmo para o alcance de ataque no Editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

}
