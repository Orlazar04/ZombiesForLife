using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Main Contributors: Trin Rist
public class ArmorBehavior : MonoBehaviour
{
    public int armorStrength = 5;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // detects the health of the player
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                // increases the amount of protection the player has from zombies
                playerHealth.protectionAmount = armorStrength;
                // destroys the armor after collection
                Destroy(gameObject, 2);
            }
        }
    }
}
