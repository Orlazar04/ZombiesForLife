// Beta Version
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSpace;

// This script is meant for the managing of the target pickup
// Dependencies: Level State
// Main Contributors: Grace Calianese, Olivia Lazar 
public class TargetPickupManager : MonoBehaviour
{
    public string itemName = "Undefined";
    public float integrity = 60f;
    public PickupState status;
    
    private int nearbyZombies;
    private int attackingZombies = 1;

    // Start is called before the first frame update
    void Start()
    {
        status = PickupState.Safe;
        nearbyZombies = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // While the level is active
        if(LevelManager.IsLevelActive())
        {
            UpdatePickupState();
            
            // While being attacked
            if(status == PickupState.Attacked)
            {
                DecreaseIntegrity();
            }
        }
    }

    // Updates the state of the target pickup
    private void UpdatePickupState()
    {
        if(status != PickupState.Attacked && nearbyZombies > 0)
        {
            status = PickupState.Threatened;
        }
        else if (status != PickupState.Attacked && nearbyZombies == 0)
        {
            status = PickupState.Safe;
        }
    }

    // Updates the amount of zombies approaching the target pickup
    public void UpdateNearbyZombies(int val)
    {
        nearbyZombies += val;
    }

    private void OnColliderStay(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy1") || other.gameObject.CompareTag("Enemy2") || other.gameObject.CompareTag("Enemy3"))
        {
            status = PickupState.Attacked;
        }
    }

    private void OnColliderExit(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy1") || other.gameObject.CompareTag("Enemy2") || other.gameObject.CompareTag("Enemy3"))
        {
            status = PickupState.Threatened;
        }
    }

    // Decreases the integrity of the target pickup
    public void DecreaseIntegrity()
    {
        integrity -= Time.deltaTime * attackingZombies;
        integrity = Mathf.Clamp(integrity, 0f, 60f);
        if(integrity <= 0)
        {
            status = PickupState.Destroyed;
            gameObject.SetActive(false);
        }
    }
}
