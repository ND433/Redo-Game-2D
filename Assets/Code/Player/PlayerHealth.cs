using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Values")]
    public float health = 1f;
    public float maxHealth = 2f;

    [Header("UI Elements")]
    public TextMeshProUGUI healthText;
    public GameObject[] healthImages;

    private static int totalDeaths = 0;
    private bool isImmune = false;

    public void TakeDamage(float amount)
    {
        if (isImmune) return;

        health -= amount;
        if (health < 0) health = 0;

        UpdateHealthUI();

        if (health <= 0)
        {
            HandleDeath();
        }
        else
        {
            StartCoroutine(ImmunityRoutine());
        }
    }

    void HandleDeath()
    {
        totalDeaths++;
        if (totalDeaths >= 10)
        {
            totalDeaths = 0;
            SceneManager.LoadScene(5);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private IEnumerator ImmunityRoutine()
    {
        isImmune = true;
        yield return new WaitForSeconds(0.5f);
        isImmune = false;
    }

    public void Heal(float amount) { health += amount; if (health > maxHealth) health = maxHealth; UpdateHealthUI(); }
    void UpdateHealthUI() { }
}