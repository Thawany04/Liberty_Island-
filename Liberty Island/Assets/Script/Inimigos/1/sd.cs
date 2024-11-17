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
    
    
    public GameObject player;  // Referência ao jogador
    public float distanciaDeteccao = 5f;  // Distância para detectar o jogador
    public GameObject projetilPrefab;  // Prefab do projétil
    public Transform pontoDeTiro;  // Ponto de origem do projétil
    public float velocidadeTiro = 10f;  // Velocidade do projétil

    void Start()
    {
        destinoAtual = pontoB.position;  // Inicia patrulhando para o ponto B
        // Armazena a escala inicial do inimigo
        escalaInicial = transform.localScale;
    }

    void Update()
    {
        Patrulhar();
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

        // Troca de direção ao alcançar o destino
        if (Vector3.Distance(transform.position, destinoAtual) < 0.1f)
        {
            destinoAtual = (destinoAtual == pontoA.position) ? pontoB.position : pontoA.position;
        }
    }
    
    void DetectarJogador()
    {
        float distanciaAoJogador = Vector3.Distance(transform.position, player.transform.position);

        if (distanciaAoJogador <= distanciaDeteccao)
        {
            // Atira se o jogador estiver no alcance
            Atirar();
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
}
