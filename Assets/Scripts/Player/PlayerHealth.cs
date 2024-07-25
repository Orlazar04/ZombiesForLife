using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Author : Tarif Khan, Grace Calianese
// This script is meant for the zombies to attack our player and the player loses health
public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public int startingHealth = 100;
    int currentHealth;
    public int damageAmount = 20;
    //public AudioClip deadSfx;
    public Slider healthSlider;
    void Start()
    {
        currentHealth = startingHealth;
        healthSlider.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damageAmount)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damageAmount;
            currentHealth = Mathf.Clamp(currentHealth, 0, 100);
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            PlayerDies();
        }

        Debug.Log("Current Health: " + currentHealth);
    }

    void PlayerDies()
    {
        Debug.Log("Player is dead");
        //AudioSource.PlayClipAtPoint(deadSfx, transform.position);
        transform.Rotate(-90, 0, 0, Space.Self);
        FindObjectOfType<LevelManager>().LevelLost();
    }


    //Add the given health amount to the player's current health,
    //between 0 and 100, update health slider
    public void TakeHealth(int healthAmount)
    {
        if (currentHealth < 100)
        {
            currentHealth += healthAmount;
            currentHealth = Mathf.Clamp(currentHealth, 0, 100);
            healthSlider.value = currentHealth;
        }
        Debug.Log("Current Health with loot: " + currentHealth);
    }
}
