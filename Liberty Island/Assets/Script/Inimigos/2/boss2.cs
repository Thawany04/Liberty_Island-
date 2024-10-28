using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Adicione esta linha para usar o Slider

public class boss2 : MonoBehaviour
{
    public Transform player; 
    [SerializeField] private float attackRange = 3f; 
    public float speed = 2f; 
    public int maxHealth = 100;
    public int damage = 10;
    public int currentHealth;
    
    public Slider healthBarSlider; // Adicione esta linha para referenciar a barra de vida

    private bool isAttacking = false; 
    private bool isEnraged = false; 
    private bool isWalking = false;
    private bool isDead = false; // Nova variável para verificar se o boss está morto

    private Animator animator;
    private AudioSource audioSource;
    public AudioClip enrageSound; 
    public AudioClip attackSound; 
    public AudioClip walkSound; 

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>(); 
        audioSource = GetComponent<AudioSource>();

        healthBarSlider.maxValue = maxHealth; // Define o valor máximo da barra de vida
        healthBarSlider.value = currentHealth; // Define o valor inicial da barra de vida
    }

    private void Update()
    {
        if (isDead) return; // Interrompe a execução se o boss estiver morto

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

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

        healthBarSlider.value = currentHealth; // Atualiza a barra de vida conforme a saúde atual
    }

    private void FollowPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        if (!isWalking)
        {
            StartWalking();
        }

        animator.SetBool("isAttacking", false);
    }

    private void FacePlayer()
    {
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
        animator.SetBool("isWalking", true);
        PlaySoundLoop(walkSound);
    }

    private void StopWalking()
    {
        isWalking = false;
        animator.SetBool("isWalking", false);
        audioSource.Stop();
    }

    private void StartAttack()
    {
        if (isDead) return; // Verifica se o boss está morto antes de atacar

        isAttacking = true;
        StopWalking();
        animator.SetBool("isAttacking", true);
        InvokeRepeating("AttackPlayer", 0f, 1f);
    }

    private void StopAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", false);
        CancelInvoke("AttackPlayer");
        if (Vector2.Distance(transform.position, player.position) > attackRange)
        {
            StartWalking();
        }
    }

    private void AttackPlayer()
    {
        if (!isDead) // Verifica novamente se o boss está morto antes de atacar
        {
            player.GetComponent<PlayerController>().Damager(damage);
            PlaySound(attackSound);
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return; // Ignora dano se o boss já está morto

        currentHealth -= amount;
        healthBarSlider.value = currentHealth; // Atualiza a barra de vida ao receber dano

        if (currentHealth <= maxHealth / 2 && !isEnraged)
        {
            Enrage();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Enrage()
    {
        isEnraged = true;
        speed *= 1.5f;
        damage += 10;
        PlaySound(enrageSound);
    }

    private void Die()
    {
        isDead = true; // Marca o boss como morto
        isAttacking = false;
        CancelInvoke("AttackPlayer"); // Cancela ataques pendentes
        animator.SetTrigger("Die");
        audioSource.Stop(); // Para qualquer som em execução
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
