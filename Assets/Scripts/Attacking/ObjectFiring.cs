using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFiring : MonoBehaviour
{
    private bool _hasBegunFiring = false;
    // Change this to overload parameters!
    public void SetFiringMode(string firingMode, GameObject target, float speed)
    {
        switch (firingMode)
        {
            case ("FireObjectForward"):
                FireObjectForward(speed);
                break;
            case ("FireObjectAtTarget"):
                FireObjectAtTarget(target.transform, speed);
                break;
            case ("FireHomingObjectAtTarget"):
                FireHomingObjectAtTarget(target.transform, speed);
                break;
            default:
                Debug.Log("Error! This is not a listed firing mode.");
                break;
        }
    }

    public void FireObjectForward(float speed)
    {
        transform.Translate(Vector3.forward * speed);
    }

    public void FireObjectAtTarget(Transform target, float speed)
    {
        // Aim at the target for only one frame
        if(!_hasBegunFiring)
            transform.LookAt(target);

        _hasBegunFiring = true;
        transform.Translate(Vector3.forward * speed);
    }

    public void FireHomingObjectAtTarget(Transform target, float speed)
    {
        // Object will continually home in on target, chasing it from behind if it overshoots
        transform.LookAt(target);
        transform.Translate(Vector3.forward * speed);
    }

    public void FireHomingObjectAtTarget(Transform target, Transform thisObject, float speed)
    {
        // Overloaded version that moves something this isn't attached to
        thisObject.transform.LookAt(target);
        thisObject.transform.Translate(Vector3.forward * speed);
    }
}
