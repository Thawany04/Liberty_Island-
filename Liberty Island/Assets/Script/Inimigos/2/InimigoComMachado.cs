using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoComMachado : MonoBehaviour
{
     public Transform pontoA, pontoB; // Pontos A e B para o movimento
    public float velocidade = 3f;
    public float distanciaDeAtaque = 1.5f; // Distância de ataque (raio de ataque)
    public float tempoEntreAtaques = 2f; // Intervalo entre os ataques
    private Transform jogador;
    private Animator animator;
    private bool estaAtacando = false;
    private float tempoUltimoAtaque = 0f; // Para controlar o tempo entre os ataques

    // A área de ataque será um Trigger Collider
    public Collider2D areaDeAtaque; // Referência à área de ataque

    void Start()
    {
        jogador = GameObject.FindGameObjectWithTag("Jogador").transform;
        animator = GetComponent<Animator>();

        // Se não atribuir manualmente no Inspector, podemos buscar automaticamente
        if (areaDeAtaque == null)
        {
            areaDeAtaque = GetComponent<Collider2D>();
        }
    }

    void Update()
    {
        float distancia = Vector3.Distance(transform.position, jogador.position);

        // Movimenta entre ponto A e B
        if (!estaAtacando)
        {
            if (transform.position == pontoA.position)
                transform.position = Vector3.MoveTowards(transform.position, pontoB.position, velocidade * Time.deltaTime);
            else if (transform.position == pontoB.position)
                transform.position = Vector3.MoveTowards(transform.position, pontoA.position, velocidade * Time.deltaTime);

            animator.SetBool("Andando", true); // Define que está andando
        }
        else
        {
            animator.SetBool("Andando", false); // Se está atacando, não anda
        }

        // Verifica se o jogador está dentro da área de ataque e se passou o intervalo entre os ataques
        if (areaDeAtaque.IsTouchingLayers(LayerMask.GetMask("player")) && Time.time - tempoUltimoAtaque >= tempoEntreAtaques)
        {
            IniciarAtaque();
        }
        else
        {
            animator.SetBool("Andando", true); // Se não está atacando, continua andando
        }
    }

    void IniciarAtaque()
    {
        estaAtacando = true;
        animator.SetTrigger("atac"); // Ativa a animação de ataque com machado
        tempoUltimoAtaque = Time.time; // Registra o tempo do último ataque
        StartCoroutine(AguardarFimDoAtaque());
    }

    IEnumerator AguardarFimDoAtaque()
    {
        yield return new WaitForSeconds(1f); // Espera o tempo da animação do golpe
        estaAtacando = false;
    }

    // Método para tomar dano
    public void TomarDano(int dano)
    {
        // Lógica para reduzir vida e verificar morte
    }
}


