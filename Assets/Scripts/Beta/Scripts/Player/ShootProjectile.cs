// Beta Version
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZombieSpace;

// This script is meant for the player shooting a projectile
// Dependencies: Level State, Weapon Manager
// Main Contributors: Olivia Lazar
public class ShootProjectile : MonoBehaviour
{
    private bool canShoot;              // Whether a projectile can be fired
    private float coolDownTimer;        // The current timer for when a projectile can be fired again

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // While the level is active
        if(LevelManager.IsLevelActive())
        {
            // If player has a ranged weapon
            if(WeaponManager.currentWeaponType == WeaponType.Ranged)
            {
                ManageShooting();
            }
            // Otherwise reset shooting
            else if (!canShoot)
            {
                canShoot = true;
            }
        }
    }

    // Manages the player's shooting
    private void ManageShooting()
    {
        // Shoot projectile on button click if the player can shoot
        if (Input.GetButtonDown("Fire1") && canShoot)
        {
            Shoot();
        }
        // Track cooldown before shooting is allowed again
        else if (!canShoot)
        {
            coolDownTimer += Time.deltaTime;
            // Restart shooting
            if (coolDownTimer >= WeaponManager.fireRate)
            {
                canShoot = true;
            }
        }
    }       

    // Initiates process for shooting a projectile
    private void Shoot()
    {
        Transform weaponTF = WeaponManager.launchPoint;

        // Instantiate the projectile
        GameObject projectile = Instantiate(WeaponManager.projectile, weaponTF.position, weaponTF.rotation);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(weaponTF.forward * WeaponManager.projectileSpeed, ForceMode.VelocityChange);

        // Set projectile properties
        projectile.transform.SetParent(GameObject.FindGameObjectWithTag("Projectiles").transform);
        projectile.layer = LayerMask.NameToLayer("Ignore Raycast");

        // Animate ranged weapon
        // TODO

        // Update shooting cooldown
        canShoot = false;
        coolDownTimer = 0;
    }
}
