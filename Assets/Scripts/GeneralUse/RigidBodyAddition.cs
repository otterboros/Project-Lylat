using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class RigidBodyAddition : MonoBehaviour
{
    public void AddRigidBody(bool useGravity = false)
    {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = useGravity;
    }
}
