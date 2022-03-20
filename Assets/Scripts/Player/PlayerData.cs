// PlayerData.cs - Store Data Values for the player ship
//                 and draw x,y movement range and max z firing range of ship
//---------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [Header("Player Health Settings")]
    [Tooltip("The maximum health the player can have")]
    public int maxHealth = 20;
    [Tooltip("How many invincibility frames does the player get?")]
    public int numOfIFrames = 15;

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

    [Header("Player Blocked by Environment")]
    [Tooltip("If the character is blocked or not.")]
    public bool blocked = false;
    [Tooltip("What layers the character uses as blocked")]
    public LayerMask blockedLayers;

    private void OnDrawGizmos()
    {
        // Color a box to show player's max movement and attack range
        Gizmos.color = Color.yellow;

        // The Cube's x and y ranges are the player's max movement range
        // The Cube's z range is the max distance the player's shot can travel
        Gizmos.DrawWireCube(transform.position + new Vector3(0,0,distToDestroy/2), new Vector3(2*xRange, 2*yRange, distToDestroy));
    }
}
