// Beta Version
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSpace;

// This script is meant for managing the player's weapons
// Main Contributors: Olivia Lazar, Tarif Khan
public class WeaponManager : MonoBehaviour
{ 
    public static WeaponType currentWeaponType;

    // Melee Weapon Stats
    public static GameObject meleeWeapon;
    public static int meleeDamage;
    public static float swingRate;
    public static Transform contactPoint;
    public static Animator meleeAnimator;

    // Ranged Weapon Stats
    public static GameObject rangedWeapon;
    public static GameObject projectile;
    public static int projectileDamage;
    public static float projectileSpeed;
    public static float fireRate;
    public static Transform launchPoint;
    public static Animator rangedAnimator;

    // Start is called before the first frame update
    void Start()
    {
        currentWeaponType = WeaponType.Ranged;
        UpdateRangedWeapon("Pistol");
    }

    private static void UpdateMellee() 
    {
    
    }

    // Updates the behavior of the current ranged weapon
    private static void UpdateRangedWeapon(string weapon) 
    {
        if(weapon == "None")
        {
            SetRangeValues(0, 0, 0);
        }
        else if (weapon == "Pistol")
        {
            SetRangeValues(5, 60, 0.2f);
        }
        else if (weapon == "AK")
        {
            SetRangeValues(45, 90, 0.1f);
        }
        else if (weapon == "M4")
        {
            SetRangeValues(30, 110, 0.05f);
        }
        else if (weapon == "SMG")
        {
            SetRangeValues(20, 160, 0.02f);
        }
        else if (weapon == "Sniper")
        {
            SetRangeValues(500, 300, 0.5f);
        }
    }

    // Sets the visuals of the current ranged weapon
    private static void SetRangeVisuals(GameObject weapon, GameObject bullet, Transform point, Animator anim)
    {
        rangedWeapon = weapon;
        projectile = bullet;
        launchPoint = point;
        rangedAnimator = anim;
    }

    // Sets the values of the current ranged weapon
    private static void SetRangeValues(int damage, float speed, float rate)
    {
        projectileDamage = damage;
        projectileSpeed = speed;
        fireRate = rate;
    }
}