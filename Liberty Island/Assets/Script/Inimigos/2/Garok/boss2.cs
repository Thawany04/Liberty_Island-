using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class boss2 : MonoBehaviour
{
    // Referência ao jogador
    public Transform player;
    
    // Variável de alcance do ataque do boss
    [SerializeField] private float attackRange = 3f;

    // Variáveis de velocidade, saúde e dano do boss
    public float speed = 2f;
    public int vida = 100;
    public int dano = 10;
    public int currentHealth;

    // Variável pública para o intervalo de ataque do boss
    public float attackInterval = 1f; // intervalo de ataque em segundos
    
    // Barra de vida do boss
    public Slider barradevida;

    // Flags para controle de estado do boss
    private bool isAttacking = false; 
    private bool isEnraged = false;
    private bool isWalking = false;
    private bool isDead = false;

    // Referências para o Animator e o AudioSource
    private Animator animator;
    private AudioSource audioSource;

    // Áudios para diferentes ações do boss
    public AudioClip enrageSound;
    public AudioClip attackSound;
    public AudioClip walkSound;

    private void Start()
    {
        // Inicializa a saúde do boss com o valor máximo
        currentHealth = vida;
        
        // Obtém o componente Animator e AudioSource
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // Define o valor máximo e inicial da barra de vida
        barradevida.maxValue = vida;
        barradevida.value = currentHealth;
    }

    private void Update()
    {
        // Se o boss está morto, interrompe o Update
        if (isDead) return;

        // Calcula a distância até o jogador
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Faz o boss se virar na direção do jogador
        FacePlayer();

        // Verifica se o boss deve começar ou parar de atacar
        if (distanceToPlayer <= attackRange && !isAttacking)
        {
            StartAttack();
        }
        else if (distanceToPlayer > attackRange && isAttacking)
        {
            StopAttack();
        }

        // Se não está atacando, segue o jogador
        if (!isAttacking)
        {
            FollowPlayer();
        }

        // Atualiza a barra de vida
        barradevida.value = currentHealth;
    }

    // Faz o boss seguir o jogador
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

    // Faz o boss virar em direção ao jogador
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

    // Inicia a animação de caminhada e o som correspondente
    private void StartWalking()
    {
        isWalking = true;
        animator.SetBool("isWalking", true);
        PlaySoundLoop(walkSound);
    }

    // Para a animação e o som de caminhada
    private void StopWalking()
    {
        isWalking = false;
        animator.SetBool("isWalking", false);
        audioSource.Stop();
    }

    // Inicia o ataque ao jogador
    private void StartAttack()
    {
        if (isDead) return;

        isAttacking = true;
        StopWalking();
        animator.SetBool("isAttacking", true);
        InvokeRepeating("AttackPlayer", 0f, attackInterval); // usa o intervalo configurável
    }

    // Para o ataque
    private void StopAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", false);
        CancelInvoke("AttackPlayer");

        // Se o jogador está fora do alcance, volta a andar
        if (Vector2.Distance(transform.position, player.position) > attackRange)
        {
            StartWalking();
        }
    }

    // Aplica dano ao jogador
    private void AttackPlayer()
    {
        if (!isDead)
        {
            player.GetComponent<PlayerController>().Damager(dano);
            PlaySound(attackSound);
        }
    }

    // Método para aplicar dano ao boss
    public void TakeDamage(int amount)
    {
        if (isDead) return;

        // Reduz a saúde e atualiza a barra de vida
        currentHealth -= amount;
        barradevida.value = currentHealth;

        // Ativa o modo "enrage" se a saúde estiver abaixo de 50%
        if (currentHealth <= vida / 2 && !isEnraged)
        {
            Enrage();
        }

        // Se a saúde chegar a zero, o boss morre
        if (currentHealth <= 0)
        {
            Die();
            SceneManager.LoadScene("Scenes/2/part 2 fase 2");
        }
    }

    // Modo "enrage", aumentando a velocidade e o dano do boss
    private void Enrage()
    {
        isEnraged = true;
        speed *= 1.5f;
        dano += 10;
        PlaySound(enrageSound);
    }

    // Executa ações quando o boss morre
    private void Die()
    {
        isDead = true;
        isAttacking = false;
        CancelInvoke("AttackPlayer");
        animator.SetTrigger("Die");
        audioSource.Stop();
        Destroy(gameObject, 2f); // Destroi o boss após 2 segundos
    }

    // Toca um áudio uma vez
    private void PlaySound(AudioClip clip)
    {
        if (audioSource && clip)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    // Toca um áudio em loop
    private void PlaySoundLoop(AudioClip clip)
    {
        if (audioSource && clip)
        {
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    // Exibe um círculo visualizando o alcance do ataque
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    // Método público que causa dano ao boss quando colide com um objeto com a tag "bala"
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bala") && !isDead)
        {
            TakeDamage(2); // Aplica 2 de dano ao boss, ajustável conforme necessário
            Destroy(collision.gameObject); // Destroi o objeto "bala" ao colidir
        }
    }
}
