using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupLaserRotator : MonoBehaviour
{
    private PickupData _pd;
    private void Awake()
    {
        _pd = GetComponent<PickupData>();
    }

    private void Update()
    {
        Vector3 eAngles = transform.rotation.eulerAngles;
        eAngles.y += _pd.rotationRate;
        transform.rotation = Quaternion.Euler(eAngles);
    }
}
