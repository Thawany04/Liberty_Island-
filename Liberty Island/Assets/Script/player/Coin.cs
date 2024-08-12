using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int scoreValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Gamer_Controler.instace.Updatescore(scoreValue);
         Destroy(gameObject);
    }
}
