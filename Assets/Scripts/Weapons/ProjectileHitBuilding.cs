using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHitBuilding : MonoBehaviour
{
    public AudioClip hitBuildingSFX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Building"))
        {
            Debug.Log("Hit Building");
            AudioSource.PlayClipAtPoint(hitBuildingSFX, transform.position);
            Destroy(gameObject);
        }
    }
}
