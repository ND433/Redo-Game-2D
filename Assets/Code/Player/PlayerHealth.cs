using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Values")]
    //health
    public float health = 1f;
    //max health
    public float maxHealth = 2f;

    [Header("UI Elements")]
    //health text drag inspector
    public TextMeshProUGUI healthText;
    //health images drag inspector
    public GameObject[] healthImages; 

    void Start()
    {
        //if not freezed update GUI
        Time.timeScale = 1f;
        UpdateHealthUI();
    }
    //if hp<0 load death scene
    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health < 0) health = 0;

        UpdateHealthUI();

        //if (health <= 0)
        //{
        //    SceneManager.LoadScene(5);
        //}
    }
    //health text updater
    public void Heal(float amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;
        UpdateHealthUI();
    }
    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = (int)health + " / " + (int)maxHealth;
        }
        //healthbar sprite rendering
        if (healthImages != null && healthImages.Length > 0)
        {
            int spriteIndex = Mathf.Clamp((int)((health / maxHealth) * healthImages.Length) - 1, 0, healthImages.Length - 1);

            for (int i = 0; i < healthImages.Length; i++)
            {
                if (healthImages[i] != null)
                {
                    if (health <= 0)
                    {
                        healthImages[i].SetActive(false);
                    }
                    else
                    {
                        healthImages[i].SetActive(i == spriteIndex);
                    }
                }
            }
        }
    }
}