using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam : MonoBehaviour
{
    private Transform player;
    public float smooth;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate()
    {
        // Cria um vetor com a posição x e y do jogador, e mantém a posição z atual da câmera.
        Vector3 targetPosition = new Vector3(player.position.x, player.position.y, transform.position.z);
        
        // Move a câmera suavemente em direção à posição desejada.
        transform.position = Vector3.Lerp(transform.position, targetPosition, smooth * Time.deltaTime);
    }
}


