using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZombieSpace;

// This script is meant for the zombies to attack our player and the player loses health
// Main Contributors: Olivia Lazar
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;

    public AudioClip deadSFX;
    public Slider healthSlider;

    private int currentHealth;
    private LevelManager lm;

    private AudioSource hitSFX;

    // Start is called before the first frame update
    void Start()
    {
        // Initiate health and GUI
        currentHealth = maxHealth;
        healthSlider.maxValue = currentHealth;
        healthSlider.value = currentHealth;

        // Initialize sound
        hitSFX = gameObject.GetComponent<AudioSource>();

        // Initialize level manager
        lm = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Decrease health
    public void TakeDamage(int damageAmount)
    {
        if(currentHealth > 0)
        {
            currentHealth -= damageAmount;
            healthSlider.value = Mathf.Clamp(currentHealth, 0, maxHealth);
            hitSFX.Play();
        }
        else
        {
            PlayerDies();
        }
    }

    // Increase health
    public void TakeHealth(int healthAmount)
    {
        if(currentHealth < maxHealth)
        {
            currentHealth += healthAmount;
            healthSlider.value = Mathf.Clamp(currentHealth, 0, maxHealth);
        }
    }

    // Initiates procedure for when player dies
    private void PlayerDies()
    {
        lm.LevelLost(DefeatType.PlayerKilled);
        AudioSource.PlayClipAtPoint(deadSFX, transform.position);
        transform.Rotate(-90, 0, 0, Space.Self);
    }
}
