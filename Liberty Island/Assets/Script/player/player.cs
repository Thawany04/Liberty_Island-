using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movimento e pulo
    public float speed;
    public float forcejump;
    public bool isJump;
    public int vida;

    private Rigidbody2D rig;
    private Animator anim;
    public Transform groundCheck; // Ponto para verificar se está no chão
    public float groundCheckRadius = 0.1f; // Raio para verificar o chão
    public LayerMask whatIsGround; // Layer do chão
    public bool isGrounded;

    // Tiro
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public float bulletLifeTime = 2f; // Tempo de vida da bala em segundos
    public float attackDelay = 0.5f;  // Tempo entre o início do ataque e o final
    private bool isAttacking = false; // Para marcar quando o jogador está atacando

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

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

        // Atirar com a tecla F, só se não estiver atacando
        if (Input.GetKeyDown(KeyCode.F) && !isAttacking)
        {
            StartCoroutine(ShootCoroutine());
        }

        // Controlar o movimento
        if (!isAttacking)
        {
            Mover();
            Jump();
        }
        else
        {
            // Manter a animação de ataque enquanto ataca
            anim.SetInteger("Transition", 3);  // Animação de ataque
        }
    }

    void Mover()
    {
        float movement = Input.GetAxis("Horizontal");

        // Evitar movimento se estiver atacando
        if (isAttacking)
        {
            return; // Não permitir movimento durante o ataque
        }

        // Atualiza a velocidade do jogador
        rig.velocity = new Vector2(movement * speed, rig.velocity.y);

        // Controla a animação de movimento
        if (movement > 0 && isGrounded)
        {
            anim.SetInteger("Transition", 1);  // Movimento para a direita
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (movement < 0 && isGrounded)
        {
            anim.SetInteger("Transition", 1);  // Movimento para a esquerda
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (movement == 0 && isGrounded)
        {
            anim.SetInteger("Transition", 0);  // Parado
        }
    }

    void Jump()
    {
        // Só permite o pulo se o jogador estiver no chão (isGrounded) e não estiver atacando
        if (Input.GetButtonDown("Jump") && isGrounded && !isAttacking)
        {
            rig.AddForce(new Vector2(0, forcejump), ForceMode2D.Impulse);
            isJump = true; // Marcar como verdadeiro
            anim.SetInteger("Transition", 4); // Ativar animação de pulo
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        // Verifica se o objeto colidido tem a tag "inimigo"
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
            UnityEngine.SceneManagement.SceneManager.LoadScene("ExplosionScene");
        }
    }

    void CheckGround()
    {
        // Verifica se o jogador está no chão
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        // Atualiza a variável de pulo (isJump) com base no estado do chão
        if (!isGrounded)
        {
            isJump = true;  // Marca como verdadeiro se não estiver no chão
            anim.SetInteger("Transition", 4);  // Ativa a animação de pulo
        }
        else
        {
            isJump = false; // Reseta para falso se estiver no chão
            anim.SetInteger("Transition", 0);  // Retorna para a animação de parada ou movimento
        }
    }

    // Coroutine para gerenciar o ataque (tiro)
    IEnumerator ShootCoroutine()
    {
        isAttacking = true; // Marca que o jogador está atacando
        anim.SetInteger("Transition", 3); // Ativa animação de ataque
        rig.velocity = Vector2.zero; // Para o jogador enquanto ataca
        yield return new WaitForSeconds(0.3f); // Tempo da animação de ataque

        // Criar uma bala na posição de disparo
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = firePoint.right * bulletSpeed;
        }

        // Destruir a bala após bulletLifeTime segundos
        Destroy(bullet, bulletLifeTime);

        // Aguarda o tempo de delay antes de permitir outro ataque
        yield return new WaitForSeconds(attackDelay);

        isAttacking = false; // Marca que o jogador não está mais atacando
        anim.SetInteger("Transition", 0);  // Retorna para a animação de parada após o ataque
    }
}