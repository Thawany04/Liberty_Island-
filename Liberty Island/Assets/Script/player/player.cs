using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_pulo : MonoBehaviour
{
    public float speed;
    public float forcejump;
    private bool isJump;
    public int vida;

    private Rigidbody2D rig;
    private Animator anim;
    public Transform groundCheck;  // Ponto para verificar se está no chão
    public float groundCheckRadius = 0.2f;  // Raio para verificar o chão
    public LayerMask whatIsGround;  // Layer do chão

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
        Mover();
        CheckGrounded();
        Jump();
    }

    void Mover()
    {
        float movement = Input.GetAxis("Horizontal");
        rig.velocity = new Vector2(movement * speed, rig.velocity.y);

        if (movement > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (movement < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    void CheckGrounded()
    {
        // Raycast ou OverlapCircle para verificar se está no chão
        isJump = !Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && !isJump)
        {
            rig.AddForce(new Vector2(0, forcejump), ForceMode2D.Impulse);
            isJump = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isJump = false;
        }

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
}
