using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public ParticleSystem explosionPrefab; // Use a prefab instead of a reference to an in-scene object
    public bool explode; 

    void OnCollisionEnter(Collision collision)
    {
        if (explosionPrefab != null && explode)
        {
            // Instantiate explosion effect at collision point and set rotation
            ParticleSystem explosionInstance = Instantiate(explosionPrefab, collision.contacts[0].point, Quaternion.identity);
            
            // Play the explosion effect
            explosionInstance.Play();

            // Destroy explosion after its duration to clean up
            Destroy(explosionInstance.gameObject, explosionInstance.main.duration + explosionInstance.main.startLifetime.constantMax);
        }

        // Destroy bullet after collision
        Destroy(gameObject);
    }
}
