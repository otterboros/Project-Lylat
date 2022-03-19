using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeCollider : MonoBehaviour
{
    public void SafeCollision(Collider other)
    {
        // Get object normal
        // Prevent ship from moving any further in that direction
        // To prevent z-problems, this object must always have a normal of (0,0,-/+1)

        // Therefore, we don't need it's normal! 

        // What about the highway though? You can crash into it as it turns...

    }

    private void OnCollisionEnter(Collision other)
    {
        // siwtch to GetContact and GetContacts to avoid memory garbo

        Debug.Log($"Collided with {other.transform.name}");
        // Print how many points are colliding with this transform
        Debug.Log("Points colliding: " + other.contacts.Length);

        // Print the normal of the first point in the collision.
        Debug.Log("Normal of the first point: " + other.contacts[0].normal);

        // Draw a different colored ray for every normal in the collision
        foreach (var item in other.contacts)
        {
            Debug.DrawRay(item.point, item.normal * 100, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), 10f);
        }


    }
}
