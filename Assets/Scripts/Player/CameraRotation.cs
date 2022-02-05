using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] GameObject playerSpaceship;


    [Header("Camera Movement Settings")]
    [Tooltip("How fast the camera moves in response to ship movement")]
    [SerializeField] float cameraPositionYRatio;
    [SerializeField] float cameraPositionXRatio;

    [Header("Camera Rotation Settings")]
    [Tooltip("How fast the camera rotates in response to ship movement")]
    [SerializeField] float cameraPitchRatio;
    [SerializeField] float cameraYawRatio;
    [SerializeField] float cameraRollRatio;

    // Remove redundancies between this and PlayerControls, if possible

    private void ProcessCameraPosition()
    {
        float newXPosition = playerSpaceship.transform.localPosition.x * cameraPositionXRatio;
        float newYPosition = playerSpaceship.transform.localPosition.y * cameraPositionYRatio;

        transform.localPosition = new Vector3
            (newXPosition,
            newYPosition,
            transform.localPosition.z);
    }

    private void ProcessCameraRotation()
    {
        float currentPitch, currentYaw, currentRoll;
        float rawPitch, rawYaw, rawRoll;
        float pitch, yaw, roll;
        Vector3 currentEulerAngles = playerSpaceship.transform.localRotation.eulerAngles;

        if (currentEulerAngles.x > 180) // Convert to usable values for yThrow input
            currentPitch = 360 - currentEulerAngles.x;
        else
            currentPitch = -currentEulerAngles.x;
        rawPitch = currentPitch * cameraPitchRatio;
        if (rawPitch > 0) // Convert back to euler Angles
            pitch = 360 - rawPitch;
        else
            pitch = -rawPitch;

        if (currentEulerAngles.y > 180) // Convert to usable values for xThrow input
            currentYaw = -360 + currentEulerAngles.y;
        else
            currentYaw = currentEulerAngles.y;
        rawYaw = currentYaw * cameraYawRatio;
        if (rawYaw < 0) // Convert back to euler Angles
            yaw = 360 + rawYaw;
        else
            yaw = rawYaw;

        if (currentEulerAngles.z > 180) // Convert to usable values for xThrow input
            currentRoll = 360 - currentEulerAngles.z;
        else
            currentRoll = -currentEulerAngles.z;
        rawRoll = currentRoll * cameraRollRatio;
        if (rawRoll > 0) // Convert back to euler Angles
            roll = 360 - rawRoll;
        else
            roll = -rawRoll;

        transform.localRotation = Quaternion.Euler(pitch,yaw,roll); 
    }

    void Update()
    {
        ProcessCameraPosition();
        ProcessCameraRotation();
    }
}
