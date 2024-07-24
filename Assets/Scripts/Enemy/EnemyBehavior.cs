using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Tarif Khan
// This script serves for zombies to chase our player and attack them
public class EnemyBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    public float moveSpeed = 10;
    public float minDistance = 2;
    int damageAmount = 20;
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float step = moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > minDistance)
        {
            transform.LookAt(player);
            transform.position = Vector3.MoveTowards(transform.position, player.position, step);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damageAmount);
        }
    }
}
