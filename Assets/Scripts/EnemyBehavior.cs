using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSpace;

// Enemy momevent and attacking behaviors
// Main Contributors: Olivia Lazar, Grace 
public class EnemyBehavior : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private int damageAmount;
    [SerializeField]
    private float maxDistance;

    private int difficulty;
    private ZombieTarget currentTarget;
    private bool isBlocked;                 // Whether there is an obstacle in its path
    private Transform playerTF;
    private Transform pickupTF;
    
    private PlayerHealth playerHealth;
    private TargetPickupBehavior targetScript;
    
    // Start is called before the first frame update
    void Start()
    {
        currentTarget = ZombieTarget.None;
        isBlocked = false;

        // Initialize level manager values
        difficulty = LevelManager.levelDifficulty;
        lm = FindObjectOfType<LevelManager>();

        // Initialize player components
        var player =  GameObject.FindGameObjectWithTag("Player");
        playerTF = player.transform;
        playerHealth = player.GetComponent<PlayerHealth>();

        // Initialize pickup components;
        var pickup = GameObject.FindGameObjectWithTag("Target Pickup");
        pickupTF = pickup.transform;

        // Initialize enemy stats, affected by difficulty and enemy type
        if(gameObject.CompareTag("Enemy1"))
        {
            moveSpeed = 3;
            damageAmount = 5 * difficulty;
            maxDistance = 15;
        }
        else if(gameObject.CompareTag("Enemy2"))
        {
            moveSpeed = 2;
            damageAmount = 10 * difficulty;
            maxDistance = 20;
        }
        else if(gameObject.CompareTag("Enemy3"))
        {
            moveSpeed = 1;
            damageAmount = 15 * difficulty;
            maxDistance = 30;
        }        
    }

    // Update is called once per frame
    void Update()
    {
        // While the level is active
        if(LevelManager.IsLevelActive())
        {
            // Move to target if the path is clear
            if(!isBlocked)
            {
                MoveToTarget();
            }
            else
            {
                GoAround();
            }
        }
    }

    // Update for physics based attributes
    private void FixedUpdate()
    {   
        // Update whether there is an object blocking the enemies path
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 2))
        {
            isBlocked = hit.collider.CompareTag("Obstacle") || hit.collider.CompareTag("Wall"));
        }
    }

    // Moves the enemy towards an updated target
    private void MoveToTarget()
    {
        float distancePlayer = Vector3.Distance(transform.position, playerTF.position);
        float distancePickup = Vector3.Distance(transform.position, pickupTF.position);

        // Update enemy's target destination
        if(distancePlayer <= maxDistance)
        {
            currentTarget = ZombieTarget.Player;
            MoveEnemy(playerTF);
        }
        else if(distancePickup <= maxDistance)
        {
            currentTarget = ZombieTarget.Player;
            MoveEnemy(pickupTF);
            targetScript.EnemyApproaching();
        }
        else
        {
            currentTarget = ZombieTarget.None;
            MoveRandomly();
        }
    }

    // Moves the enemy towards the given target
    private void MoveEnemy(Transform target)
    {
        // Update values with respect to target's location
        float step = moveSpeed * Time.deltaTime;

        // Look and move
        transform.LookAt(target);
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }

    // Moves the enemy randomly
    private void MoveRandomly()
    {

    }

    // Moves the enemy away from an object blocking its path
    private void GoAround()
    {
        Vector3 newPostion = transform.position + ;
        transform.position = Vector3.MoveTowards(transform.position, newPostion, step);
    }
        
    // On collision enter
    private void OnCollisionEnter(Collision collision)
    {
        // Attack player on collision
        if(collision.gameObject.CompareTag("Player"))
        {
            playerHealth.TakeDamage(damageAmount);
        }
        // Attack target pick up on collision
        if(collision.gameObject.CompareTag("Target Pickup"))
        {
            
        }
    }
}
