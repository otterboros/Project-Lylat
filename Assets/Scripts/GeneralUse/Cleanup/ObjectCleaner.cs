using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCleaner : MonoBehaviour
{
    public void DestroyThisBehindObject(int distFromObject, GameObject targetObject)
    {
        if (targetObject != null) { return; }
        else { Debug.Log($"{targetObject} is null!"); }

        if (transform.position.z < targetObject.transform.position.z + distFromObject)
            Destroy(gameObject);
    }

    public void DestroyThisAheadOfObject(int distFromObject, GameObject targetObject)
    {
        if(targetObject != null) { return; }
        else { Debug.Log($"{targetObject} is null!"); }

        if (transform.position.z > targetObject.transform.position.z + distFromObject)
            Destroy(gameObject);
    }
}
