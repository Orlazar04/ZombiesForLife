using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Tarif Khan, Grace Calianese
// This script serves for zombies to chase our player and attack them
public class EnemyBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    public float moveSpeed = 8;
    public float minDistance = 0;
    public AudioClip zombieHitSFX;
    public AudioClip playerHitSFX;
    //public AudioClip zombieDiesSFX;
    public int startingHealth = 100;
    int currentHealth;

    int damageAmount = 20;
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        currentHealth = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        float step = moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > minDistance)
        {
            transform.LookAt(player);
            transform.position = Vector3.MoveTowards(transform.position, player.position, step);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !LevelManager.isGameOver)
        {
            Debug.Log("Collision with player");
            var playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damageAmount);
            AudioSource.PlayClipAtPoint(playerHitSFX, Camera.main.transform.position);
        }

        if (other.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("Projectile collision detected");
            AudioSource.PlayClipAtPoint(zombieHitSFX, transform.position);
            Destroy(other.gameObject);
            TakeDamage(FindObjectOfType<ShootProjectile>().projectileDamage);
        }
    }

    void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        damageAmount = Mathf.Clamp(currentHealth, 0, 100);
        if(currentHealth <= 0)
        {
            DestroyZombie();
        }
    }

    void DestroyZombie()
    {
        // This will be the particle system
        // Instantiate(zombieDiesSFX, transform.position, transform.rotation);
        gameObject.SetActive(false);
        Destroy(gameObject, .5f);
    }
}
