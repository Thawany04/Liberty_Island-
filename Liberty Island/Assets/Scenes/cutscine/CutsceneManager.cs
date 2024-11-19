
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public PlayableDirector playableDirector;

    void Start()
    {
        playableDirector.stopped += OnCutsceneEnd;
    }

    private void OnCutsceneEnd(PlayableDirector director)
    {
        // Carregar a nova cena
        SceneManager.LoadScene("Tutorial"); // Substitua pelo nome da sua cena
    }

    void OnDestroy()
    {
        playableDirector.stopped -= OnCutsceneEnd;
    }
}
