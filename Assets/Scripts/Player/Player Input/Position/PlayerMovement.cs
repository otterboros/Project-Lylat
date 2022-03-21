// PlayerMovement.cs - Update player ship transform based on input
//                     & check if ship has collided with an object
//                     that doesn't impart damage but isn't passthru
//------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private ProcessMovementInput m_Input;

    private PlayerData _data;
    private Rigidbody _rb;

    private void Awake()
    {
        m_Input = GetComponent<ProcessMovementInput>();
        _data = GetComponent<PlayerData>();
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        ProcessShipPosition();
    }

    private void ProcessShipPosition()
    {
        // Get values from player inputs
        Vector2 playerInput = new Vector2(m_Input.xThrow, -m_Input.yThrow);

        // Physics-based movement
        Vector3 desiredVelocity = new Vector3(playerInput.x * _data.xMoveSpeed, playerInput.y * _data.yMoveSpeed, 0);

        // Get current PlayerShip2 velocity
        Vector3 velocity = _rb.velocity;

        velocity.x =
            Mathf.MoveTowards(velocity.x, desiredVelocity.x, _data.xMaxSpeedChange);
        velocity.y =
            Mathf.MoveTowards(velocity.y, desiredVelocity.y, _data.yMaxSpeedChange);
        velocity.z = 0;

        _rb.velocity = velocity;
    }
}