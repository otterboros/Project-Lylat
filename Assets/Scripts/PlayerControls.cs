using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    // Remove redundancies between this and PlayerControls, if possible

    [SerializeField] InputAction movement;
    [SerializeField] InputAction fire;
    [SerializeField] GameObject[] lasers;

    [Header("Ship Movement Settings")]
    [Tooltip("How fast ship moves in response to player input")]
    [SerializeField] float xDodgeSpeed;
    [SerializeField] float yDodgeSpeed;

    [Tooltip("How far ship can move from 0,0 in response to player input")]
    [SerializeField] float xShipRange;
    [SerializeField] float yShipRange;

    [Header("Ship Rotation Settings")]
    [Tooltip("How far ship can rotate in response to player input")]
    [SerializeField] float shipPitchRatio;
    [SerializeField] float shipYawRatio;
    [SerializeField] float shipRollRatio;

    [Tooltip("Rate of change from current to desired rotation")]
    [SerializeField] float InterpDuration;

    float xThrow, yThrow;

    void OnEnable()
    {
        movement.Enable();
        fire.Enable();
    }

    void OnDisable()
    {
        movement.Disable();
        fire.Disable();
    }

    private void ProcessShipPosition()
    {
        xThrow = movement.ReadValue<Vector2>().x;
        yThrow = movement.ReadValue<Vector2>().y;

        float xOffset = xThrow * xDodgeSpeed * Time.deltaTime;
        float yOffset = yThrow * yDodgeSpeed * Time.deltaTime;

        float rawXPosition = transform.localPosition.x + xOffset;
        float newXPosition = Mathf.Clamp(rawXPosition, -xShipRange, xShipRange);

        float rawYPosition = transform.localPosition.y - yOffset; // negative offset here to invert vertical controls
        float newYPosition = Mathf.Clamp(rawYPosition, -yShipRange, yShipRange);

        transform.localPosition = new Vector3
            (newXPosition,
            newYPosition,
            transform.localPosition.z);
    }

    private void ProcessShipRotation()
    {
        float currentPitch, currentYaw, currentRoll;
        float rawPitch, rawYaw, rawRoll;
        float pitch, yaw, roll;
        Vector3 currentEulerAngles = transform.localRotation.eulerAngles;
        float Interp = Time.deltaTime / InterpDuration;

        float targetPitch = -yThrow * shipPitchRatio;
        float targetYaw = xThrow * shipYawRatio;
        float targetRoll = xThrow * shipRollRatio;

        if (currentEulerAngles.x > 180) // Convert to usable values for yThrow input
            currentPitch = 360 - currentEulerAngles.x;
        else
            currentPitch = -currentEulerAngles.x;
        rawPitch = Mathf.Lerp(currentPitch, targetPitch, Interp);
        if (rawPitch > 0) // Convert back to euler Angles
            pitch = 360 - rawPitch;
        else
            pitch = -rawPitch;

        if (currentEulerAngles.y > 180) // Convert to usable values for xThrow input
            currentYaw = -360 + currentEulerAngles.y;
        else
            currentYaw = currentEulerAngles.y;

        rawYaw = Mathf.Lerp(currentYaw, targetYaw, Interp);
        if (rawYaw < 0) // Convert back to euler Angles
            yaw = 360 + rawYaw;
        else
            yaw = rawYaw;

        if (currentEulerAngles.z > 180) // Convert to usable values for xThrow input
            currentRoll = 360 - currentEulerAngles.z;
        else
            currentRoll = -currentEulerAngles.z;
        rawRoll = Mathf.Lerp(currentRoll, targetRoll, Interp);
        if (rawRoll > 0) // Convert back to euler Angles
            roll = 360 - rawRoll;
        else
            roll = -rawRoll;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    void Update()
    {
        ProcessShipPosition();
        ProcessShipRotation();
        ProcessFire();
    }

    void ProcessFire()
    {
        if (fire.ReadValue<float>() > 0.05)
        {
            SetLasersActive(true);
        }
        else
        {
            SetLasersActive(false);
        }
    }

    void SetLasersActive(bool isLaserActive)
    {
        var emission = lasers[0].GetComponent<ParticleSystem>().emission; // Stores the module in a local variable
        emission.enabled = isLaserActive; // Applies the new value directly to the Particle System
    }
}
