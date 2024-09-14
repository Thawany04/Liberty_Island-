using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class move_pulo : MonoBehaviour
{
    public float speed;
    public float forcejump;
    private bool isJump;
    public int vida;

    private Rigidbody2D rig;
    private Animator anim;
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Gamer_Controler.instace.UpdateLives(vida);
        Mover();
        Jump();
    }

    void Mover()
    {
        float movement = Input.GetAxis("Horizontal");
        rig.velocity= new Vector2(movement * speed, rig.velocity.y);

        if (movement > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        
        if (movement < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump")) 
        {
            if (!isJump)
            {
                rig.AddForce(new Vector2(0, forcejump), ForceMode2D.Impulse);
                isJump = true;
            }
            
        }
    }
    
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == 3)
        {
            isJump = false;
        }

        // Verifica se o objeto colidido tem a tag "inimigo"
        if (coll.gameObject.CompareTag("inimigo"))
        {
            Damager(1); //  Aplica dano ao player; você pode ajustar a quantidade de dano conforme necessário
            
        }
    }


    public void Damager(int dmg)
    {
        vida -= dmg;
        Gamer_Controler.instace.UpdateLives(vida);

        if (vida <= 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("ExplosionScene");
        }
    }
}
