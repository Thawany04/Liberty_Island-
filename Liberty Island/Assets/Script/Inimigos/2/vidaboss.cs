using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBar; // A barra de vida
    private boss2 boss; // Referência ao script do boss

    private void Start()
    {
        // Encontra o boss na cena
        boss = FindObjectOfType<boss2>();
        UpdateHealthBar(); // Atualiza a barra de vida no início
    }

    // Método para atualizar a barra de vida
    public void UpdateHealthBar()
    {
        if (boss != null) // Verifica se a referência ao boss é válida
        {
            healthBar.fillAmount = (float)boss.GetCurrentHealth() / boss.maxHealth; // Atualiza a barra de vida
        }
    }
}