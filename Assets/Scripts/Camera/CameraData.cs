using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraData
{
    // Any way to convert these to ints to save memory?

    [Header("Camera Movement Settings")]
    [Tooltip("How fast camera moves in response to player input")]
    [SerializeField] public static int xMoveSpeed = 15;
    [SerializeField] public static int yMoveSpeed = 15;

    [Tooltip("How far camera can move from 0,0 in response to player input")]
    // Old values couple to ship movement were 0.3, 0.3
    [SerializeField] public static int xRange = 10;
    [SerializeField] public static int yRange = 5;

    [Header("Camera Rotation Settings")]
    // Old values coupled to ship movement are 0.25, 0.1, 0.25
    [Tooltip("How fast the camera rotates in response to player input")]
    [SerializeField] public static int cameraPitchRatio = 11;
    [SerializeField] public static int cameraYawRatio = 6;
    [SerializeField] public static int cameraRollRatio = 7;

    [Tooltip("Rate of change from current to desired rotation")]
    [SerializeField] public static float interpDuration = 0.2f;
}
