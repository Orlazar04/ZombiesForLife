using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Grace Calianese
// This script controls health pickup behavior
public class HealthPickup : MonoBehaviour
{
    public int healthAmount = 10;
    public AudioClip healthPickupSFX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(healthPickupSFX, transform.position);
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.TakeHealth(healthAmount);
            Destroy(gameObject);
        }
    }
}
