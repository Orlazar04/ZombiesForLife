using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Author : Tarif Khan
// This script checks for if a zombie is hit with a projectile
public class ZombieHit : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject zombieDies;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            DestroyZombie();
        }
    }

    void DestroyZombie()
    {
       // This will be the particle system
       // Instantiate(zombieDies, transform.position, transform.rotation);
        gameObject.SetActive(false);
        Destroy(gameObject, .5f);
    }
}