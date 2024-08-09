using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mov_pul_atak : MonoBehaviour
{
    public int vida = 10;
    public int velocidade;
    public int forca_pulo;

    public float movimento;
    private bool isjump;
    private bool isfiri, olhandoDireita;
    private Animator anim;
    private Rigidbody2D rig;


    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }
    void Mover()
    {
        movimento = Input.GetAxis("Horizontal");
        rig.velocity = new Vector2(movimento * velocidade, rig.velocity.y);

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
           
        }

        if (movimento > 0 && !isjump)
        { 
            anim.SetInteger("transition", 1);
            transform.eulerAngles = new Vector3(0, 0, 0);
            
            olhandoDireita = true;
           
        }

        if (movimento < 0 && !isjump)
        {
            anim.SetInteger("transition", 1);
            transform.eulerAngles = new Vector3(0, 180, 0);
            olhandoDireita = false;
           
        }

        if (movimento == 0 && !isjump)
        {
            anim.SetInteger("transition", 0);
           
        }
    }
    
    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!isjump)
            {
                anim.SetInteger("transition", 2);
                rig.AddForce(new Vector2(0, forca_pulo), ForceMode2D.Impulse);
                isjump = true;
               
            }
        }
    }
}
