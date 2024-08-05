using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSpace;

// This script is meant for the player performing melee attacks
// Dependencies: Level State, Weapon Manager
// Main Contributors: Olivia Lazar, Tarif Khan
// Version: Beta
public class MeleeHit : MonoBehaviour
{
    private bool canAttack;             // Whether a melee attack can be performed
    private float coolDownTimer;        // The current timer for when a melee attack can be performed again

    // Update is called once per frame
    void Update()
    {
        // While the level is active
        if (LevelManager.IsLevelActive())
        {
            // If player has a melee weapon
            if (WeaponManager.equippedWeapon == WeaponType.Melee)
            {
                ManageMeleeAttack();
            }
            // Otherwise reset attack
            else if (!canAttack)
            {
                canAttack = true;
            }
        }
    }

    // Manages the player's melee attacks
    private void ManageMeleeAttack()
    {
        if (Input.GetButtonDown("Fire1") && canAttack)
        {
            MeleeAttack();
        }
        else if (!canAttack)
        {
            coolDownTimer += Time.deltaTime;
            if (coolDownTimer >= WeaponManager.swingRate)
            {
                canAttack = true;
            }
        }
    }

    // Initiates process for performing a melee attack
    private void MeleeAttack()
    {
        // Trigger melee attack animation
        if (WeaponManager.meleeAnimator != null)
        {
            WeaponManager.meleeAnimator.SetTrigger("Attack");
        }

        // Perform melee attack logic (implemented in MeleeAttackHandler)
        MeleeAttackHandler.PerformMeleeAttack();

        // Update attack cooldown
        canAttack = false;
        coolDownTimer = 0;
    }
}