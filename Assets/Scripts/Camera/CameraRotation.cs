using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] GameObject playerShip;

    private CameraData _data;

    private Vector3 shipRotationRatios;

    private ProcessMovementInput _mInput;
    private Vector2 playerInput;

    private void Awake()
    {
        _data = GetComponent<CameraData>();
        shipRotationRatios = new Vector3(_data.cameraPitchRatio, _data.cameraYawRatio, _data.cameraRollRatio);

        _mInput = playerShip.GetComponent<ProcessMovementInput>();
    }

    // Break out Camera Position to a new class
    private void FixedUpdate()
    {
        ProcessCameraRotation();
    }


    private void ProcessCameraRotation()
    {
        playerInput = new Vector2(_mInput.xThrow, _mInput.yThrow);

        // Process Camera Rotation
        transform.localRotation = EulerQuaternionProcessor.ProcessPitchYawAndRoll(gameObject, shipRotationRatios, playerInput, _data.interpDuration);
    }
}
