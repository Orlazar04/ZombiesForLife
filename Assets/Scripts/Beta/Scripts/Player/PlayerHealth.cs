// Beta Version
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZombieSpace;

// This script is meant for the player's health management
// Dependencies: Level Difficulty
// Main Contributors: Olivia Lazar
public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private int startHealth = 200;      // The maximum amount of starting health
    [SerializeField]
    private int reduceHealth = 50;      // The factor by which maximum health is reduced for a level

    public Slider healthSlider;
    public AudioClip deadSFX;

    private int maxHealth;              // The maximum amount of health the player can have
    private int currentHealth;          // The current amount of health the player has
    private AudioSource hitSFX;

    private LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        // Initiate health and GUI
        maxHealth = startHealth - (reduceHealth * (LevelManager.levelDifficulty - 1));
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;

        // Initialize sound
        hitSFX = gameObject.GetComponent<AudioSource>();

        // Initialize level manager
        levelManager = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Decrease current health
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

    // Increase current health
    public void TakeHealth(int healthAmount)
    {
        if(currentHealth < maxHealth)
        {
            currentHealth += healthAmount;
            healthSlider.value = Mathf.Clamp(currentHealth, 0, maxHealth);
        }
    }

    // Updates the maximum health the player can have
    public void UpdateHealthRange(int amount)
    {
        maxHealth += amount;
    }

    // Initiates procedure for when player dies
    private void PlayerDies()
    {
        levelManager.LevelLost(DefeatType.PlayerKilled);
        AudioSource.PlayClipAtPoint(deadSFX, transform.position);
        transform.Rotate(-90, 0, 0, Space.Self);
    }
}
