// Alpha Version
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is for the buildings getting hit
// Main Contributors: Grace Calianese
public class BuildingBehavior : MonoBehaviour
{
    public AudioClip hitBuildingSFX;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Building"))
        {
            Debug.Log("Hit Building");
            AudioSource.PlayClipAtPoint(hitBuildingSFX, transform.position);
            Destroy(gameObject);
        }
    }
}
