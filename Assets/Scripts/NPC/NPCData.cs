using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCData : MonoBehaviour
{
    [Header("NPC Name")]
    public string npcName;

    [Header("NPC Health Settings")]
    [Tooltip("The maximum health the player can have")]
    public int maxHealth = 10;
    //[Tooltip("How many invincibility frames does the NPC get?")]
    //public int numOfIFrames = 15;

    [Header("NPC Attack Settings")]
    //[Tooltip("The attack value for player lasers")]
    public float shotsPerSecond = 0.3f;
    public float shotSpeed = 2;
    public int shotDamage = 1;
    //public int distToDestroy = 150; // add the -z of camera to intended dist from player ship
}
