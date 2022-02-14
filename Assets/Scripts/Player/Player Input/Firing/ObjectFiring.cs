using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFiring : MonoBehaviour
{
    public void FireObjectForward(float speed)
    {
        transform.Translate(Vector3.forward * speed);
    }

    public void FireHomingObjectAtTarget(Transform target, float speed)
    {
        transform.LookAt(target.transform);
        transform.Translate(Vector3.forward * speed);
    }
}
