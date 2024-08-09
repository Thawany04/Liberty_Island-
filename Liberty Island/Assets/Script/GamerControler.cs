using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gamer_Controler : MonoBehaviour
{
    public Text vidatext;
    public static Gamer_Controler instace;
    
    
    void Start()
    {
        instace = this;
    }

   
    void Update()
    {
        
    }

    public void UpdateLives(int value)
    {
        vidatext.text = "x " + value.ToString();
    }
}


