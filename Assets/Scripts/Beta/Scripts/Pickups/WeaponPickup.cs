using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Author: Tarif Khan
// This script controls weapon pickup
public class WeaponPickup : MonoBehaviour
{
    public GameObject weaponPrefab; // Assign this in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Find the main camera (child of the player)
            Camera mainCamera = other.GetComponentInChildren<Camera>();

            if (mainCamera != null)
            {
                // Instantiate the weapon as a child of the main camera
                GameObject weapon = Instantiate(weaponPrefab, mainCamera.transform);

                // You might want to set the local position and rotation here
                weapon.transform.localPosition = Vector3.zero;
                weapon.transform.localRotation = Quaternion.identity;

                // Destroy the pickup object
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("Main camera not found in player object.");
            }
        }
    }
}