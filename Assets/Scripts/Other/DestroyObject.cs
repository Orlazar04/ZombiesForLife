using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Tarif Khan
// This script is meant for shooting projectiles and it doing damage.
public class DestroyObject : MonoBehaviour
{
    // Start is called before the first frame update
    public float destroyDuration = 3;
    void Start()
    {
        Destroy(gameObject, destroyDuration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
