using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EulerQuaternionProcessor
{
    /// <summary>
    /// Convert Quaternion local Rotation into Euler Angles, process input for pitch, yaw, and roll, and output the new angles converted back into Quaternion local Rotation
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="shipRatios"></param>
    /// <param name="playerInput"></param>
    /// <param name="interpDuration"></param>
    /// <returns></returns>
    public static Quaternion ProcessPitchYawAndRoll(GameObject gameObject, Vector3 shipRatios, Vector2 playerInput, float interpDuration, bool yInversion = true)
    {
        // pitch, yaw, roll
        float xThrow = playerInput.x;
        float yThrow;

        if (yInversion)
            yThrow = -playerInput.y; // Find a way to make this toggleable later...
        else
            yThrow = playerInput.y;

        float currentAngle;
        float rawAngle;

        float Interp = Time.deltaTime / interpDuration;

        Vector3 currentEulerAngles = gameObject.transform.localRotation.eulerAngles;
        Vector3 targetAngles = new Vector3(yThrow * shipRatios.x, xThrow * shipRatios.y, xThrow * shipRatios.z);
        Vector3 outputAngles = Vector3.zero;

        for (int i = 0; i < 3; i++)
        {
            if (i != 1)
            {
                if (currentEulerAngles[i] > 180) // Convert to usable values for yThrow input
                    currentAngle = 360 - currentEulerAngles[i];
                else
                    currentAngle = -currentEulerAngles[i];

                rawAngle = Mathf.Lerp(currentAngle, targetAngles[i], Interp);

                if (rawAngle > 0) // Convert back to euler Angles
                    outputAngles[i] = 360 - rawAngle;
                else
                    outputAngles[i] = -rawAngle;
            }

            else if (i == 1) // Is there a way to contain this in the generic equation above?
            {
                if (currentEulerAngles[i] > 180) // Convert to usable values for yThrow input
                    currentAngle = -(360 - currentEulerAngles[i]);
                else
                    currentAngle = currentEulerAngles[i];

                rawAngle = Mathf.Lerp(currentAngle, targetAngles[i], Interp);

                if (rawAngle > 0) // Convert back to euler Angles
                    outputAngles[i] = 360 + rawAngle; // This is the only part that isn't just a negative of the ones in the generic equation above!
                else
                    outputAngles[i] = rawAngle;
            }
        }
        return Quaternion.Euler(outputAngles);
    }
}
