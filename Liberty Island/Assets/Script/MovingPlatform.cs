using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] waypoints; // Array de pontos de movimento
    public float speed = 2f; // Velocidade da plataforma
    private int currentTargetIndex = 0; // Índice do ponto atual
    private Transform currentTarget; // Ponto de destino atual

    void Start()
    {
        if (waypoints.Length > 0)
        {
            currentTarget = waypoints[currentTargetIndex];
        }
    }

    void Update()
    {
        if (waypoints.Length > 0)
        {
            // Move a plataforma em direção ao ponto de destino atual
            transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

            // Verifica se a plataforma alcançou o ponto de destino
            if (Vector3.Distance(transform.position, currentTarget.position) < 0.1f)
            {
                // Passa para o próximo ponto
                currentTargetIndex = (currentTargetIndex + 1) % waypoints.Length;
                currentTarget = waypoints[currentTargetIndex];
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Desenha linhas no editor para visualizar o caminho
        if (waypoints != null && waypoints.Length > 0)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < waypoints.Length; i++)
            {
                if (i + 1 < waypoints.Length)
                {
                    Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                }
                else
                {
                    // Se desejar que a plataforma retorne ao ponto inicial, desenhe uma linha para o primeiro ponto
                    Gizmos.DrawLine(waypoints[i].position, waypoints[0].position);
                }
            }
        }
    }
}


