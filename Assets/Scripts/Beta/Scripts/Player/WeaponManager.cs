using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSpace;

// This script is meant for managing the player's weapons
// Dependencies: Level State
// Main Contributors: Olivia Lazar, Tarif Khan
// Beta Version
public class WeaponManager : MonoBehaviour
{
    public static WeaponType equippedWeapon;

    // Melee Weapon Stats
    private static bool hasMelee;
    public static int meleeDamage;
    public static float meleeKnockback;
    public static float swingRate;
    public static float swingRange;

    public static GameObject meleeWeapon;
    public static Transform contactPoint;
    public static Animator meleeAnimator;

    // Ranged Weapon Stats
    private static bool hasRanged;
    public static int projectileDamage;
    public static float projectileSpeed;
    public static float projectileKnockback;
    public static float fireRate;
    public static float fireRange;

    public static GameObject rangedWeapon;
    public static GameObject projectile;
    public static Transform launchPoint;
    public static Animator rangedAnimator;

    private static string currentMeleeWeapon = "None";
    private static string currentRangedWeapon = "None";

    [SerializeField]
    private GameObject[] meleeWeapons;
    [SerializeField]
    private GameObject[] rangedWeapons;

    // Start is called before the first frame update
    private void Start()
    {
        equippedWeapon = WeaponType.None;
        UpdateRangedWeapon("Pistol");
        UpdateMeleeWeapon("Axe");
    }

    // Update is called once per frame
    private void Update()
    {
        // While the level is active
        if (LevelManager.IsLevelActive())
        {
            // Equipping weapons
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.E))
            {
                EquipWeaponType(WeaponType.Melee, hasMelee);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.R))
            {
                EquipWeaponType(WeaponType.Ranged, hasRanged);
            }
            // Dropping weapons
            else if (Input.GetKeyDown(KeyCode.Q) && equippedWeapon != WeaponType.None)
            {
                DropEquippedWeapon();
            }
        }
    }

    // Changes the equipped weapon to the given type if possible
    private static void EquipWeaponType(WeaponType type, bool hasWeapon)
    {
        if (equippedWeapon != type)
        {
            if (hasWeapon)
            {
                //LowerWeapon();
                equippedWeapon = type;
                //RaiseWeapon();
                Debug.Log("Switched to " + type + " Weapon: " + (type == WeaponType.Melee ? currentMeleeWeapon : currentRangedWeapon));
            }
            else if (!hasWeapon && equippedWeapon != WeaponType.None)
            {
                //LowerWeapon();
                equippedWeapon = WeaponType.None;
                Debug.Log("No " + type + " weapon available. Switched to unarmed.");
            }
        }
    }

    // Drops the equipped weapon
    private static void DropEquippedWeapon()
    {
        //LowerWeapon();
        switch (equippedWeapon)
        {
            case WeaponType.Melee:
                UpdateMeleeWeapon("None");
                break;
            case WeaponType.Ranged:
                UpdateRangedWeapon("None");
                break;
        }
        //DropWeapon();
        Debug.Log("Dropped " + equippedWeapon + " weapon.");
    }

    // Changing weapons among types
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MeleeWeapon") && !hasMelee)
        {
            UpdateMeleeWeapon(other.gameObject.name);
        }
        else if (other.CompareTag("RangedWeapon") && !hasRanged)
        {
            UpdateRangedWeapon(other.gameObject.name);
        }
    }

    // Updates the behavior of the current melee weapon
    private static void UpdateMeleeWeapon(string weapon)
    {
        currentMeleeWeapon = weapon;
        hasMelee = weapon != "None";
        if (weapon == "None")
        {
            SetMeleeValues(0, 0, 0, 0);
            if (equippedWeapon == WeaponType.Melee)
            {
                equippedWeapon = WeaponType.None;
            }
        }
        else if (weapon == "Axe")
        {
            SetMeleeValues(30, 2, 1.5f, 5f);
        }
        // Add more melee weapons here
    }

    // Sets the values of the current melee weapon
    private static void SetMeleeValues(int damage, float knock, float rate, float range)
    {
        meleeDamage = damage;
        meleeKnockback = knock;
        swingRate = rate;
        swingRange = range;
    }

    // Sets the visuals of the current melee weapon
    public static void SetMeleeVisuals(GameObject weapon, Transform point, Animator anim)
    {
        meleeWeapon = weapon;
        contactPoint = point;
        meleeAnimator = anim;
    }

    // Updates the behavior of the current ranged weapon
    private static void UpdateRangedWeapon(string weapon)
    {
        currentRangedWeapon = weapon;
        hasRanged = weapon != "None";
        if (weapon == "None")
        {
            SetRangedValues(0, 0, 0, 0, 0);
            if (equippedWeapon == WeaponType.Ranged)
            {
                equippedWeapon = WeaponType.None;
            }
        }
        else if (weapon == "Pistol")
        {
            SetRangedValues(5, 60, 2, 0.2f, 15f);
        }
        else if (weapon == "AK")
        {
            SetRangedValues(45, 90, 2, 0.1f, 15f);
        }
        else if (weapon == "M4")
        {
            SetRangedValues(30, 110, 2, 0.05f, 15f);
        }
        else if (weapon == "SMG")
        {
            SetRangedValues(20, 160, 2, 0.02f, 15f);
        }
        else if (weapon == "Sniper")
        {
            SetRangedValues(500, 300, 2, 0.5f, 15f);
        }
    }

    // Sets the values of the current ranged weapon
    private static void SetRangedValues(int damage, float speed, float knock, float rate, float range)
    {
        projectileDamage = damage;
        projectileSpeed = speed;
        projectileKnockback = knock;
        fireRate = rate;
        fireRange = range;
    }

    // Sets the visuals of the current ranged weapon
    public static void SetRangedVisuals(GameObject weapon, GameObject bullet, Transform point, Animator anim)
    {
        rangedWeapon = weapon;
        projectile = bullet;
        launchPoint = point;
        rangedAnimator = anim;
    }
}