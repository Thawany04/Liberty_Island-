using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sd : MonoBehaviour
{
    public Transform pontoA;  // Primeiro ponto de patrulha
    public Transform pontoB;  // Segundo ponto de patrulha
    public float velocidade = 2f;  // Velocidade do movimento
    private Vector3 destinoAtual;
    private Vector3 escalaInicial;
    private Animator animator;
    private bool morto = false;
    public GameObject player;  // Referência ao jogador
    public float distanciaDeteccao = 5f;  // Distância para detectar o jogador
    public GameObject projetilPrefab;  // Prefab do projétil
    public Transform pontoDeTiro;  // Ponto de origem do projétil
    public float velocidadeTiro = 10f;  // Velocidade do projétil
    public float intervaloAtaque = 1.5f;  // Tempo entre ataques

    private bool atacando = false;         // Indica se o inimigo está no modo de ataque
    private float contadorTempoAtaque = 0f; // Contador de tempo para controlar intervalo entre ataques

    // Variáveis de vida
    public int vida = 100;  // Vida máxima do inimigo
    private int vidaAtual;        // Vida atual do inimigo

    void Start()
    {
        destinoAtual = pontoB.position;  // Inicia patrulhando para o ponto B
        // Armazena a escala inicial do inimigo
        escalaInicial = transform.localScale;
        animator = GetComponent<Animator>();

        // Inicializa a vida
        vidaAtual = vida;
    }

    void Update()
    {
        if (morto) return;
        
        // Atualiza o contador de tempo de ataque
        if (atacando)
        {
            contadorTempoAtaque += Time.deltaTime;
            if (contadorTempoAtaque >= intervaloAtaque)
            {
                // Permite um novo ataque após o intervalo
                atacando = false;
                contadorTempoAtaque = 0f;
            }
        }
        
        // Se não está atacando, continua patrulhando
        if (!atacando)
        {
            Patrulhar();
        }

        DetectarJogador();
    }
    
    void Patrulhar()
    {
        // Move em direção ao destino atual
        transform.position = Vector3.MoveTowards(transform.position, destinoAtual, velocidade * Time.deltaTime);

        // Verifica a direção e ajusta apenas o eixo X da escala
        if (transform.position.x < destinoAtual.x)
        {
            // Indo para a direita
            transform.localScale = new Vector3(Mathf.Abs(escalaInicial.x), escalaInicial.y, escalaInicial.z);
        }
        else
        {
            // Indo para a esquerda
            transform.localScale = new Vector3(-Mathf.Abs(escalaInicial.x), escalaInicial.y, escalaInicial.z);
        }
        animator.SetBool("anda", true);

        // Troca de direção ao alcançar o destino
        if (Vector3.Distance(transform.position, destinoAtual) < 0.1f)
        {
            destinoAtual = (destinoAtual == pontoA.position) ? pontoB.position : pontoA.position;
        }
    }
    
    void DetectarJogador()
    {
        float distanciaAoJogador = Vector3.Distance(transform.position, player.transform.position);

        if (distanciaAoJogador <= distanciaDeteccao && !atacando)
        {
            // Inimigo detecta o jogador e para de patrulhar para atacar
            atacando = true;
            animator.SetBool("anda", false);
            animator.SetBool("atiro", true);
            
            // Ajusta a direção para o jogador
            VirarParaJogador();

            Atirar();
        }
        else if (distanciaAoJogador > distanciaDeteccao)
        {
            // Se o jogador está fora do alcance, continua a patrulhar
            animator.SetBool("atiro", false);
        }
    }
    
    void Atirar()
    {
        // Cria um projétil na posição do pontoDeTiro
        GameObject projetil = Instantiate(projetilPrefab, pontoDeTiro.position, Quaternion.identity);
        Rigidbody2D rb = projetil.GetComponent<Rigidbody2D>();

        // Calcula a direção do tiro em direção ao jogador
        Vector3 direcao = (player.transform.position - pontoDeTiro.position).normalized;
        rb.velocity = direcao * velocidadeTiro;
    }

    public void ReceberDano(int dano)
    {
        if (morto) return;

        // Reduz a vida do inimigo
        vidaAtual -= dano;

        // Se a vida chegar a zero, o inimigo morre
        if (vidaAtual <= 0)
        {
            vidaAtual = 0;
            Morrer();
        }
    }

    void Morrer()
    {
        morto = true;
        animator.SetTrigger("mort");

        // Desativa a colisão e outros componentes
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        // Destrói o objeto após a animação de morte
        Destroy(gameObject, 2f);
    }

    // Método para desenhar a área de detecção de ataque com Gizmos
    void OnDrawGizmos()
    {
        // Define a cor para a área de detecção
        Gizmos.color = Color.red;

        // Desenha uma esfera que representa a área de detecção
        Gizmos.DrawWireSphere(transform.position, distanciaDeteccao);
    }

    void VirarParaJogador()
    {
        // Verifica a posição do jogador em relação ao inimigo e ajusta a escala
        if (player.transform.position.x > transform.position.x)
        {
            // Jogador está à direita do inimigo
            transform.localScale = new Vector3(Mathf.Abs(escalaInicial.x), escalaInicial.y, escalaInicial.z);
        }
        else
        {
            // Jogador está à esquerda do inimigo
            transform.localScale = new Vector3(-Mathf.Abs(escalaInicial.x), escalaInicial.y, escalaInicial.z);
        }
    }
}
