using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movimento e pulo
    public float speed;
    public float forcejump;
    public bool isJump;
    public  int vida;

    private Rigidbody2D rig;
    private Animator anim;
    public Transform groundCheck; // Ponto para verificar se está no chão
    public float groundCheckRadius = 0.1f; // Raio para verificar o chão
    public LayerMask whatIsGround; // Layer do chão
    public bool isGrounded;

    // Ataque com espada
    public Vector2 attackRange = new Vector2(1.0f, 0.5f); // Ajuste conforme o tamanho da espada
    public Vector2 attackOffset = new Vector2(0.5f, 0f); // Posição da área de colisão em relação ao jogador
    
    // Tiro
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public float bulletLifeTime = 2f; // Tempo de vida da bala em segundos
    public float attackDelay = 0.5f;  // Tempo entre o início do ataque e o final
    private bool isAttacking = false; // Para marcar quando o jogador está atacando

    // Som
    public AudioSource audioSource; // Referência ao AudioSource
    public AudioClip jumpSound; // Som do pulo
    public AudioClip faca;
    public AudioClip corre;
    public AudioClip spada;
    
    public General general; // Referência ao General


    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>(); // Pega o AudioSource do jogador

        // Inscrever no evento de dano de fogo
        FireJetObs.OnFireDamage += Damager;
    }

    void OnDestroy()
    {
        // Cancelar a inscrição no evento quando o objeto for destruído
        FireJetObs.OnFireDamage -= Damager;
    }

    void Update()
    {
        Gamer_Controler.Instance.UpdateLives(vida);
        CheckGround();

        // Se não estiver atacando, permite movimento e pulo
        if (!isAttacking)
        {
            Mover();
            Jump();
        }

        // Atirar com a tecla F
        if (Input.GetKeyDown(KeyCode.L) && !isAttacking)
        {
            StartCoroutine(ShootCoroutine());
        }

        // Atacar com a espada ao pressionar "C"
        if (Input.GetKeyDown(KeyCode.K) && !isAttacking)
        {
            StartCoroutine(SwordAttack());
        }

        // Controlar animação de idle se não estiver atacando e estiver parado
        if (!isAttacking && rig.velocity.x == 0 && isGrounded)
        {
            anim.SetInteger("Transition", 0); // Parado
        }
    }

    void Mover()
    {
        // Permitir movimento apenas se a vida for maior que zero
        if (vida <= 0) return;

        float movement = Input.GetAxis("Horizontal");

        // Evitar movimento se estiver atacando
        if (isAttacking) return;

        // Atualiza a velocidade do jogador
        rig.velocity = new Vector2(movement * speed, rig.velocity.y);

        // Controla a animação de movimento e o som de correr
        if (movement != 0 && isGrounded)
        {
            anim.SetInteger("Transition", 1);  // Movendo (para qualquer direção)

            // Define a rotação do jogador com base no movimento
            transform.eulerAngles = movement > 0 ? new Vector3(0, 0, 0) : new Vector3(0, 180, 0);

            // Tocar som de correr se o jogador estiver se movendo e no chão
            if (!audioSource.isPlaying)
            {
                audioSource.clip = corre;
                audioSource.Play();
            }
        }
        else if (movement == 0 && isGrounded)
        {
            anim.SetInteger("Transition", 0);  // Parado
            // Parar o som de correr quando o jogador parar
            if (audioSource.isPlaying && audioSource.clip == corre)
            {
                audioSource.Stop();
            }
        }
    }


    void Jump()
    {
        // Só permite o pulo se o jogador estiver no chão (isGrounded), não estiver atacando e tiver vida maior que zero
        if (vida > 0 && Input.GetButtonDown("Jump") && isGrounded && !isAttacking)
        {
            rig.AddForce(new Vector2(0, forcejump), ForceMode2D.Impulse);
            anim.SetInteger("Transition", 4); // Ativar animação de pulo
            audioSource.PlayOneShot(jumpSound); // Tocar som de pulo
        }
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if (!isGrounded)
        {
            anim.SetInteger("Transition", 4);  // Animação de pulo se não estiver no chão
        }
    }

   IEnumerator SwordAttack()
{
    isAttacking = true; // Marca que o jogador está atacando
    anim.SetInteger("Transition", 2); // Ativa animação de ataque
    audioSource.PlayOneShot(spada); // Toca o som da espada
    rig.velocity = Vector2.zero; // Para o jogador enquanto ataca

    // Espera um pouco para garantir que a animação de ataque seja visível
    yield return new WaitForSeconds(0.3f);

    // Verifica a direção do jogador
    Vector2 adjustedAttackOffset = attackOffset;

    // Se o jogador estiver virado para a esquerda (rotação ou escala negativa no eixo Y)
    if (transform.eulerAngles.y == 180)  // Isso verifica se o jogador está virado para a esquerda
    {
        adjustedAttackOffset.x = -attackOffset.x;  // Inverte o offset horizontalmente
    }

    // Posição e alcance do ataque de espada
    Vector2 attackPosition = (Vector2)transform.position + adjustedAttackOffset;
    Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPosition, attackRange, 0f);

    // Verifica se atingiu algum inimigo
    foreach (Collider2D enemy in hitEnemies)
    {
        if (enemy.CompareTag("inimigo"))
        {
            Debug.Log("Inimigo atingido pela espada!");
            InimigoComMachado enemyScript = enemy.GetComponent<InimigoComMachado>(); // Altere aqui para InimigoComMachado
            if (enemyScript != null)
            {
                enemyScript.ReceberDano(3); // Aplica dano ao inimigo (mudei de TakeDamage para ReceberDano)
            }
        }
        // Adicionando lógica para o boss
         if (enemy.CompareTag("boss")) // Verifica se o objeto atingido é o boss
        {
            Debug.Log("Chefe atingido pela espada!");
            boss2 bossScript = enemy.GetComponent<boss2>();
            if (bossScript != null)
            {
                bossScript.TakeDamage(3); // Aplica dano ao chefe
            }
        }
        
        if (enemy.CompareTag("inimigo"))
        {
            Debug.Log("Inimigo atingido pela espada!");
            sd enemyScript = enemy.GetComponent<sd>(); // Altere aqui para InimigoComMachado
            if (enemyScript != null)
            {
                enemyScript.ReceberDano(3); // Aplica dano ao inimigo (mudei de TakeDamage para ReceberDano)
            }
        }
        
        if (enemy.CompareTag("boss"))
        {
            Debug.Log("Inimigo atingido pela espada!");
            General enemyScript = enemy.GetComponent<General>(); // Altere aqui para InimigoComMachado
            if (enemyScript != null)
            {
                enemyScript.ReceberDano(6); // Aplica dano ao inimigo (mudei de TakeDamage para ReceberDano)
            }
        }
        
    }

    // Aguarda o tempo de delay antes de permitir outro ataque
    yield return new WaitForSeconds(attackDelay);

    isAttacking = false; // Marca que o jogador não está mais atacando
}


    IEnumerator ShootCoroutine()
    {
        isAttacking = true; // Marca que o jogador está atacando
        anim.SetInteger("Transition", 3); // Ativa animação de tiro
        audioSource.PlayOneShot(faca); // Toca o som de tiro

        // Para o jogador enquanto atira
        rig.velocity = Vector2.zero;

        // Espera para garantir que a animação de tiro seja visível
        yield return new WaitForSeconds(0.3f);

        // Criar uma bala na posição de disparo
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = firePoint.right * bulletSpeed;
        }

        // Destruir a bala após bulletLifeTime segundos
        Destroy(bullet, bulletLifeTime);

        // Aguarda o tempo de delay antes de permitir outro tiro
        yield return new WaitForSeconds(attackDelay);

        isAttacking = false; // Marca que o jogador não está mais atirando
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("inimigo"))
        {
            Damager(1); // Aplica dano ao player; ajuste a quantidade de dano conforme necessário
        }
    }

    public void Damager(int dmg)
    {
        vida -= dmg;
        Gamer_Controler.Instance.UpdateLives(vida);

        if (vida <= 0)
        {
            Gamer_Controler.Instance.GameOver();
           
            
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 attackPosition = (Vector2)transform.position + attackOffset;
        Gizmos.DrawWireCube(attackPosition, attackRange);
    }
    
}
