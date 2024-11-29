using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class carregacena : MonoBehaviour
{
    public string cenaParacarrega;
    
    void Start()
    {
        SceneManager.LoadScene("Tutorial");
    }

    void Update()
    {
        
    }
}
