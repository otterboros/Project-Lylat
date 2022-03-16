using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCleaner : MonoBehaviour
{
    public void DestroyThisBehindObject(int distFromObject, GameObject targetObject, GameObject thisObject)
    {
        if (targetObject != null) 
        {
            if (thisObject.transform.position.z < targetObject.transform.position.z + distFromObject)
                Destroy(gameObject);
        }
        else 
        { 
            Debug.Log($"{targetObject} is null!"); 
        }
    }

    public void DestroyThisAheadOfObject(int distFromObject, GameObject targetObject)
    {
        if(targetObject != null) { return; }
        else { Debug.Log($"{targetObject} is null!"); }

        if (transform.position.z > targetObject.transform.position.z + distFromObject)
            Destroy(gameObject);
    }

    public void DestroyAfterDistance(float destroyDistance, float startingDistance, float currentDistance)
    {
        if ((currentDistance - startingDistance) > destroyDistance)
            Destroy(gameObject);
    }
}
