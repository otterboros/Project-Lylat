using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] GameObject playerShipCollider;
    [SerializeField] GameObject playerShipMesh;

    public void DisablePlayerControls()
    {
        GetComponent<PlayerInput>().enabled = false;
        playerShipCollider.SetActive(false);
        playerShipMesh.SetActive(false);

        // Add GameOver Screen sequence
    }
}
