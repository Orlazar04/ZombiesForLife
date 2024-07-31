// Beta Version
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is for the exit of the level
// Main Contributors: Grace Calianese
public class ExitBehavior : MonoBehaviour
{
    private LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            levelManager.LevelFinishAttempt();
        }
    }
}
