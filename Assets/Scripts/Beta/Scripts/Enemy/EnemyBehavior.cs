// Alpha Version
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSpace;

// This script is for enemy momevent and attacking behaviors
// Dependencies: Level State, Level Difficulty
// Main Contributors: Olivia Lazar, Grace Calianese
public class EnemyBehavior : MonoBehaviour
{
    [SerializeField]
    private int currentHealth;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private int damageAmount;
    [SerializeField]
    private float chaseRange;
    [SerializeField]
    private float attackRange;

    public AudioClip zombieDiesSFX;
    public GameObject zombieDiesFX;

    private bool isPathBlocked;                 // Whether there is an obstacle in its path
    private ZombieTarget currentTarget;         // The current movement target
    private ZombieState currentState;           // The current behavior state
    private Transform playerTF;
    private Transform pickupTF;
    private Vector3 randomLocation;             // Random nearby location to wander towards
    
    private PlayerHealth playerHealth;
    private TargetPickupManager targetPickup;
    private AudioSource zombieHitSFX;
    private Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        // Initiliaze enemy values
        currentTarget = ZombieTarget.None;
        currentState = ZombieState.Wander;
        isPathBlocked = false;
        RandomDestination();

        InitializeEnemyType();

        // Initialize player components
        var player =  GameObject.FindGameObjectWithTag("Player");
        playerTF = player.transform;
        playerHealth = player.GetComponent<PlayerHealth>();

        // Initialize pickup components
        var pickup = GameObject.FindGameObjectWithTag("TargetPickup");
        pickupTF = pickup.transform;
        targetPickup = pickup.GetComponent<TargetPickupManager>();
        
        // Initialize effects
        zombieHitSFX = gameObject.GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    // Initialize enemy stats, affected by difficulty and enemy type
    private void InitializeEnemyType()
    {
        int difficulty = LevelManager.levelDifficulty;
        attackRange = 2;

        if(gameObject.CompareTag("Enemy1"))
        {
            currentHealth = 25 * difficulty;
            moveSpeed = 3;
            damageAmount = 5 * difficulty;
            chaseRange = 15;
        }
        else if(gameObject.CompareTag("Enemy2"))
        {
            currentHealth = 50 * difficulty;
            moveSpeed = 2;
            damageAmount = 10 * difficulty;
            chaseRange = 20;
        }
        else if(gameObject.CompareTag("Enemy3"))
        {
            currentHealth = 100 * difficulty;
            moveSpeed = 1;
            damageAmount = 15 * difficulty;
            chaseRange = 30;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // While the level is active and the enemy is alive
        if(LevelManager.IsLevelActive() && IsAlive())
        {
            UpdateZombieState();
            
            switch(currentState)
            {
                case ZombieState.Wander:
                    Wander();
                    break;
                case ZombieState.Chase:
                    Chase();
                    break;
                case ZombieState.Attack:
                    anim.SetInteger("animState", 3);
                    break;
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
            isPathBlocked = hit.collider.CompareTag("Obstacle") || hit.collider.CompareTag("Wall");
        }
    }

    // Whether the enemy is alive
    private bool IsAlive()
    {
        return (currentState != ZombieState.Dead);
    }

    // Updates the zombie's state and target
    private void UpdateZombieState()
    {
        float distancePlayer = Vector3.Distance(transform.position, playerTF.position);
        float distancePickup = Vector3.Distance(transform.position, pickupTF.position);

        if(distancePlayer <= attackRange)
        {
            SetZombieState(ZombieState.Attack, ZombieTarget.Player);
            
        }
        else if(distancePlayer <= chaseRange)
        {
            SetZombieState(ZombieState.Chase, ZombieTarget.Player);
        }
        else if(distancePickup <= attackRange)
        {
            SetZombieState(ZombieState.Attack, ZombieTarget.Pickup);
        }
        else if(distancePickup <= chaseRange)
        {   
            SetZombieState(ZombieState.Chase, ZombieTarget.Pickup);
        }
        else
        {
            SetZombieState(ZombieState.Wander, ZombieTarget.None);
        }
    }

    // Sets the zombie's state and target
    private void SetZombieState(ZombieState state, ZombieTarget target)
    {
        currentState = state;
        currentTarget = target;
    }

    // Moves the enemy randomly
    private void Wander()
    {
        float distanceLocation = Vector3.Distance(transform.position, randomLocation);

        // If target location has been reached or is too far
        if(distanceLocation < 1 || distanceLocation > 25)
        {
            RandomDestination();
        }

        MoveEnemyTowards(randomLocation);

        anim.SetInteger("animState", 1);
    }

    // Establish a nearby random position for the enemy to move towards
    private void RandomDestination()
    {
        Vector3 pos = transform.position;
        float xPos = Random.Range(pos.x - 10f, pos.x + 10f);
        float yPos = pos.y;
        float zPos = Random.Range(pos.z - 10f, pos.z + 10f);
        randomLocation = new Vector3(xPos, yPos, zPos);
    }

    // Makes the enemy chase the current target
    private void Chase()
    {
        anim.SetInteger("animState", 2);

        switch(currentTarget)
        {
            case ZombieTarget.Player:
                MoveEnemyTowards(playerTF.position);
                break;
            case ZombieTarget.Pickup:
                MoveEnemyTowards(pickupTF.position);
                break;
        }
        /*
        // Move to target if the path is clear
        if(!isBlocked)
        {
            switch(currentTarget)
            {
                case(ZombieTarget.Player):
                    MoveEnemyTowards(playerTF.position);
                    break;
                case(ZombieTarget.Pickup):
                    MoveEnemyTowards(pickupTF.position);
                    pickupBehavior.Endanger();
                    break;
            }
        }
        else
        {
            GoAround();
        }*/
    }

    // Moves the enemy towards the given target
    private void MoveEnemyTowards(Vector3 target)
    {
        // Update values with respect to target's location
        float step = moveSpeed * Time.deltaTime;

        // Look and move
        FaceTarget(target);
        transform.position = Vector3.MoveTowards(transform.position, target, step);
    }

    // Makes the enemy look at the given target
    private void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }

    // Moves the enemy away from an object blocking its path
    //TODO
    /*
    private void GoAround()
    {
        Vector3 newPostion = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, newPostion, step);
    }*/

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && IsAlive())
        {
            playerHealth.TakeDamage(damageAmount);
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            zombieHitSFX.Play();
            TakeDamage(WeaponManager.projectileDamage);
        }
    }

    // Decreases the health of the enemy by the given amount
    void TakeDamage(int amount)
    {
        currentHealth -= amount;
        amount = Mathf.Clamp(currentHealth, 0, 100);
        
        if(currentHealth <= 0)
        {
            currentState = ZombieState.Dead;
            anim.SetInteger("animState", 4);
            AudioSource.PlayClipAtPoint(zombieDiesSFX, transform.position);
            Invoke("DestoryZombie", anim.GetCurrentAnimatorStateInfo(0).length);
        }
    }

    // Destroys the zombie
    void DestroyZombie()
    {
        gameObject.SetActive(false);

        GameObject particleEffect = Instantiate(zombieDiesFX, transform.position, transform.rotation) as GameObject;
        particleEffect.transform.SetParent(transform.parent);

        Destroy(gameObject, .5f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
