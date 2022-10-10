using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFiring : MonoBehaviour
{
    private bool _hasBegunFiring = false;
    // Change this to overload parameters!
    public void SetFiringMode(BaseData.FiringModes firingMode, GameObject target, float speed)
    {
        switch (firingMode)
        {
            case BaseData.FiringModes.FireObjectForward:
                FireObjectForward(speed);
                break;
            case BaseData.FiringModes.FireObjectAtTarget:
                FireObjectAtTarget(target.transform, speed);
                break;
            case BaseData.FiringModes.FireHomingObjectAtTarget:
                FireHomingObjectAtTarget(target.transform, speed);
                break;
            case BaseData.FiringModes.None:
                Debug.Log("No firing mode was set!");
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
    /// <summary>
    /// Move this object at target at speed. 
    /// Object will continually home in on target, chasing it from behind if it overshoots.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="speed"></param>
    public void FireHomingObjectAtTarget(Transform target, float speed)
    {
        transform.LookAt(target);
        transform.Translate(Vector3.forward * speed);
    }
    /// <summary>
    /// Move thisObject at target at speed.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="thisObject"></param>
    /// <param name="speed"></param>
    public void FireHomingObjectAtTarget(Transform target, Transform thisObject, float speed)
    {
        thisObject.transform.LookAt(target);
        thisObject.transform.Translate(Vector3.forward * speed);
    }
}
