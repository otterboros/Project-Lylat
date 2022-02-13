using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] GameObject playerShip;

    private Vector3 shipRotationRatios;

    private ProcessMovementInput _mInput;
    private Vector2 playerInput;

    private void Awake()
    {
        shipRotationRatios = new Vector3(CameraData.cameraPitchRatio, CameraData.cameraYawRatio, CameraData.cameraRollRatio);

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
        transform.localRotation = EulerQuaternionProcessor.ProcessPitchYawAndRoll(gameObject, shipRotationRatios, playerInput, CameraData.interpDuration);
    }
}
