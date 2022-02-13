using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    // Make this into a serializable class, maybe?
    // Can we make this static and just call it anywhere?

    [Header("Player Health Settings")]
    [Tooltip("The maximum health the player can have")]
    [SerializeField] public int maxHealth = 20;

    [Header("Ship Movement Settings")]
    [Tooltip("How fast ship moves in response to player input")]
    [SerializeField] public int xMoveSpeed = 40;
    [SerializeField] public int yMoveSpeed = 40;

    [Tooltip("How far ship can move from 0,0 in response to player input")]
    [SerializeField] public int xRange = 30;
    [SerializeField] public int yRange = 15;

    [Header("Ship Rotation Settings")]
    [Tooltip("How far ship can rotate in response to player input")]
    [SerializeField] public int shipPitchRatio = 45;
    [SerializeField] public int shipYawRatio = 60;
    [SerializeField] public int shipRollRatio = 30;

    [Tooltip("Rate of change from current to desired rotation")]
    [SerializeField] public float interpDuration = 0.2f;
}
