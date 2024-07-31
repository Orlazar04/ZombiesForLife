// Beta Version
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is meant for the behavior of a projectile
// Dependencies: Weapon Manager
// Main Contributors: Olivia Lazar
public class ProjectileBehavior : MonoBehaviour
{
    [SerializeField]
    private float duration = 2;     // The amount of time before the projectile despawns
    private float timer;            // The current amount of time of the projectile's existence

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Destroy object after some time
        timer += Time.deltaTime;
        if(timer > duration)
        {
            Destroy(gameObject);
        }
    }

    // On collision enter
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
