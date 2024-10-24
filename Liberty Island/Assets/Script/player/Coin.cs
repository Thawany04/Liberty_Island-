using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int scoreValue;
    private AudioSource sound;

    private void Awake()
    {
        sound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
             CoinObs.OnCoin(1);
             sound.Play();
             Destroy(gameObject, 0.2f);
        }
       
    }
}
