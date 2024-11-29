using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoComMachado : MonoBehaviour
{
    public AudioSource Atack;
    public Transform pontoA;  // Primeiro ponto de patrulha
    public Transform pontoB;  // Segundo ponto de patrulha
    public float velocidade = 2f;  // Velocidade do movimento
    private Vector3 destinoAtual;
    private Vector3 escalaInicial;
    private Animator animator;
    private bool morto = false;
    public GameObject player;  // Referência ao jogador
    public float distanciaDeteccao = 5f;  // Distância para detectar o jogador
    public float intervaloAtaque = 1.5f;  // Tempo entre ataques

    private bool atacando = false;         // Indica se o inimigo está no modo de ataque
    private float contadorTempoAtaque = 0f; // Contador de tempo para controlar intervalo entre ataques

    // Variáveis de vida
    public int vida = 100;  // Vida máxima do inimigo
    public int vidaAtual;        // Vida atual do inimigo

    // Variável de dano que pode ser ajustada no Inspector
    public int dano = 10;  // Dano causado pelo inimigo

    void Start()
    {
        Atack = GetComponent<AudioSource>();
        destinoAtual = pontoB.position;  // Inicia patrulhando para o ponto B
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
        animator.SetBool("Andando", true);

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
            animator.SetBool("Andando", false);
            animator.SetBool("atac", true);

            // Ajusta a direção para o jogador
            VirarParaJogador();

            Atacar();
            Atack.Play();
        }
        else if (distanciaAoJogador > distanciaDeteccao && atacando)
        {
            // Se o jogador sair da área de detecção, interrompe o ataque
            atacando = false;
            animator.SetBool("atac", false);  // Interrompe animação de ataque
            animator.SetBool("Andando", true); // Retorna à animação de patrulha
        }
    }

    void Atacar()
    {
        
        // Lógica para ataque com machado
        animator.SetTrigger("atac");  // Chama animação de ataque com machado
        // Verificar se o jogador está na área de ataque do inimigo
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, distanciaDeteccao);
        foreach (Collider2D col in hitPlayers)
        {
            if (col.CompareTag("Player"))
            {
                
                Debug.Log("Jogador atingido pelo inimigo!");
                PlayerController playerScript = col.GetComponent<PlayerController>();
                if (playerScript != null)
                {
                    playerScript.Damager(dano);  // Aplica o dano ao jogador (usando a variável dano ajustável)
                }
            }
        }

        // Aguardando o intervalo entre os ataques
        atacando = true;
    }

    public void ReceberDano(int dano)
    {
        if (morto) return;

        // Reduz a vida do inimigo
        vidaAtual -= dano;
        Debug.Log("Vida do inimigo: " + vidaAtual);  // Exibe a vida no console para debugar

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
        Destroy(gameObject, 0.8f);
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
