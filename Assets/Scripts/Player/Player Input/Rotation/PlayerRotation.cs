using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    private Vector3 shipRotationRatios;
    private float interpDuration;

    private Vector2 playerInput;

    private PlayerData _data;
    private ProcessMovementInput _mInput;

    private void Awake()
    {
        _data = GetComponent<PlayerData>();
        _mInput = GetComponent<ProcessMovementInput>();

        shipRotationRatios = new Vector3(_data.shipPitchRatio, _data.shipYawRatio, _data.shipRollRatio);

        interpDuration = _data.interpDuration;
    }

    private void FixedUpdate()
    {
        playerInput = new Vector2(_mInput.xThrow, _mInput.yThrow);

        // Process Ship Rotation
        transform.localRotation = EulerQuaternionProcessor.ProcessPitchYawAndRoll(gameObject, shipRotationRatios, playerInput, interpDuration);
    }
}
