using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [Header("Player Health Settings")]
    [Tooltip("The maximum health the player can have")]
    public int maxHealth = 20;

    [Header("Ship Movement Settings")]
    [Tooltip("How fast ship moves in response to player input")]
    public int xMoveSpeed = 40;
    public int yMoveSpeed = 40;

    [Tooltip("How far ship can move from 0,0 in response to player input")]
    public int xRange = 30;
    public int yRange = 15;

    [Header("Ship Rotation Settings")]
    [Tooltip("How far ship can rotate in response to player input")]
    public int shipPitchRatio = 45;
    public int shipYawRatio = 60;
    public int shipRollRatio = 30;

    [Tooltip("Rate of change from current to desired rotation")]
    public float interpDuration = 0.2f;

    [Header("Player Attack Settings")]
    //[Tooltip("The attack value for player lasers")]
    public float shotsPerSecond = 0.3f;
    public float shotSpeed = 2;
    public int shotDamage = 1;
    public int distToDestroy = 150; // add the -z of camera to intended dist from player ship
}
