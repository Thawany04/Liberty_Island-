using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int scoreValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
             Gamer_Controler.Instance.Updatescore(scoreValue);
             Destroy(gameObject);
        }
       
    }
}
