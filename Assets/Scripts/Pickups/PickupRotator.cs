using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupRotator : MonoBehaviour
{
    private PickupData _pd;
    private void Awake()
    {
        _pd = GetComponent<PickupData>();
    }

    private void Update()
    {
        Vector3 eAngles = transform.localRotation.eulerAngles;
        eAngles.z += _pd.rotationRate;
        transform.localRotation = Quaternion.Euler(eAngles);
    }
}
