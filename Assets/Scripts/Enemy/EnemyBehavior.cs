using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Tarif Khan, Grace Calianese
// This script serves for zombies to chase our player and attack them
public class EnemyBehavior : MonoBehaviour
{

    public enum FSMStates
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Die
    }

    // Start is called before the first frame update
    public Transform player;
    public float moveSpeed = 5f;
    public float idleDistance = 35f;
    public float chaseDistance = 15f;
    public int startingHealth = 100;
    public FSMStates currentState;

    public AudioClip zombieHitSFX;
    public AudioClip playerHitSFX;
    public AudioClip zombieDiesSFX;
    public GameObject zombieDiesFX;

    int currentHealth;
    int damageAmount = 20;
    Animator anim;
    bool isDead;
    Vector3 nextDestination;
    float distanceToPlayer;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        isDead = false;
        nextDestination = player.transform.position;
        currentState = FSMStates.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        switch (currentState)
        {
            case FSMStates.Idle:
                UpdateIdleState();
                break;
            case FSMStates.Patrol:
                UpdatePatrolState();
                break;
            case FSMStates.Chase:
                UpdateChaseState();
                break;
            case FSMStates.Die:
                UpdateDieState();
                break;
        }
    }

    void UpdateIdleState()
    {
        anim.SetInteger("animState", 0);
        if(distanceToPlayer <= idleDistance)
        {
            currentState = FSMStates.Patrol;
        }
    }


    void UpdatePatrolState()
    {
        print("Patrol state");
        anim.SetInteger("animState", 1);
        float step = moveSpeed * Time.deltaTime;

        if (!isDead)
        {
            if(distanceToPlayer > idleDistance)
            {
                currentState = FSMStates.Idle;
            }
            else if (distanceToPlayer <= chaseDistance)
            {
                currentState = FSMStates.Chase;
            }
            FaceTarget(nextDestination);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(nextDestination.x, 0, nextDestination.z), step);
        }
        else
        {
            currentState = FSMStates.Die;
        }
    }

    void UpdateChaseState()
    {
        print("Chase state");
        if (!isDead)
        {
            if (distanceToPlayer > chaseDistance)
            {
                currentState = FSMStates.Patrol;
            }
            else if (distanceToPlayer <= chaseDistance)
            {
                float step = (moveSpeed + 1) * Time.deltaTime;
                anim.SetInteger("animState", 2);
                FaceTarget(player.transform.position);
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.position.x, 0, player.position.z), step);
            }
        }
        else
        {
            currentState = FSMStates.Die;
        }
    }

    void UpdateDieState()
    {
        print("Die state");
        StartCoroutine("PlayDeathAnimation");
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !LevelManager.isGameOver)
        {
            Debug.Log("Collision with player");
            var playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damageAmount);
            AudioSource.PlayClipAtPoint(playerHitSFX, Camera.main.transform.position);
        }

        if (other.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("Projectile collision detected");
            AudioSource.PlayClipAtPoint(zombieHitSFX, transform.position);
            Destroy(other.gameObject);
            TakeDamage(FindObjectOfType<ShootProjectile>().projectileDamage);
        }
    }

    void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        damageAmount = Mathf.Clamp(currentHealth, 0, 100);
        Debug.Log("Zombie health: " + currentHealth);
        if(currentHealth <= 0)
        {
            isDead = true;
            currentState = FSMStates.Die;
        }
    }

    void DestroyZombie()
    {
        gameObject.SetActive(false);
        GameObject particleEffect = Instantiate(zombieDiesFX, transform.position, transform.rotation) as GameObject;
        particleEffect.transform.SetParent(transform.parent);
        AudioSource.PlayClipAtPoint(zombieDiesSFX, Camera.main.transform.position);
        Destroy(gameObject, .5f);
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }

    private IEnumerator PlayDeathAnimation()
    {
        Debug.Log("Zombie die");
        anim.SetInteger("animState", 4);
        yield return new WaitForSeconds(2.7f);
        DestroyZombie();
    }
}
