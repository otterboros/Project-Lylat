using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraData : MonoBehaviour
{
    // Any way to convert these to ints to save memory?

    [Header("Camera Movement Settings")]
    [Tooltip("How fast camera moves in response to player input")]
    [SerializeField] public int xMoveSpeed = 15;
    [SerializeField] public int yMoveSpeed = 15;

    [Tooltip("How far camera can move from 0,0 in response to player input")]
    // Old values couple to ship movement were 0.3, 0.3
    [SerializeField] public int xRange = 10;
    [SerializeField] public int yRange = 5;

    [Header("Camera Rotation Settings")]
    // Old values coupled to ship movement are 0.25, 0.1, 0.25
    [Tooltip("How fast the camera rotates in response to player input")]
    [SerializeField] public int cameraPitchRatio = 11;
    [SerializeField] public int cameraYawRatio = 6;
    [SerializeField] public int cameraRollRatio = 7;

    [Tooltip("Rate of change from current to desired rotation")]
    [SerializeField] public float interpDuration = 0.2f;
}
