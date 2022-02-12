using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiator : MonoBehaviour
{
    public static Instantiator instance;

    void Awake()
    {
        instance = this;
    }

    public void InstantiateByString(string resourcesPathName, Transform targetTransform, Quaternion targetRotation, Transform parentTransform, int numOfObj)
    {
        int i = 0;

        while (i < numOfObj)
        {
            Debug.Log($"{resourcesPathName}");
            GameObject fx = Instantiate(Resources.Load<GameObject>($"{resourcesPathName}"), targetTransform.position, targetRotation, parentTransform);
            i += 1;
        }

    }
}
