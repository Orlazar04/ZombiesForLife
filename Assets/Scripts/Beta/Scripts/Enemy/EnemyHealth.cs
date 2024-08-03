using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth;
    public Slider healthSlider;
    private AudioSource zombieHitSFX;


    int currentHealth;

    // Decreases the health of the enemy by the given amount
    void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, startingHealth);
        healthSlider.value = currentHealth;
        zombieHitSFX.Play();

        if (currentHealth <= 0)
        {
            GetComponent<EnemyBehavior>().ZombieDies();
        }
    }

    public void SetStartingHealth(int startHealth)
    {
        startingHealth = startHealth;
        currentHealth = startingHealth;
        healthSlider.value = startingHealth;
        zombieHitSFX = gameObject.GetComponent<AudioSource>();
    }
}
