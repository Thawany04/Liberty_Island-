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
        if (player.position.x >= 0)
        {
            Vector3 folloingn = new Vector3(player.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, folloingn, smooth * Time.deltaTime);
        }
        
    }
}
