using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class CutsceneManager : MonoBehaviour
{

    private TextMeshProUGUI componenteTexTo;
    private AudioSource _audioSource;
    private string mensagemOriginal;
    public bool imprimindo;
    public float tempoentreletras = 0.08f;

    private void Awake()
    {
        TryGetComponent(out componenteTexTo);
        TryGetComponent(out _audioSource);
        mensagemOriginal = componenteTexTo.text;
        componenteTexTo.text = "";
    }
    
    private void OnEnable()
    {
        ImprimndoMensagem(mensagemOriginal);
    }
    
    private void OnDisable()
    {
        componenteTexTo.text = mensagemOriginal;
        StopAllCoroutines();
    }

    public void ImprimndoMensagem(string mensagem)
    {
        if (gameObject.activeInHierarchy)
        {
            if (imprimindo) return;
            imprimindo = true;
            StartCoroutine(letraporletras(mensagem));
        }
    }

    IEnumerator letraporletras(string mensagem)
    {
        string msg = "";
        foreach (var letra in mensagem)
        {
            msg += letra;
            componenteTexTo.text = msg;
            _audioSource.Play();
            yield return new WaitForSeconds(tempoentreletras);

        }

        imprimindo = false;
        StopAllCoroutines();
    }
    
}
