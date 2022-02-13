using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 playerInput;

    private PlayerData _data;
    private ProcessMovementInput _mInput;

    private void Awake()
    {
        _data = GetComponent<PlayerData>();
        _mInput = GetComponent<ProcessMovementInput>();
    }

    private void FixedUpdate()
    {
        ProcessShipPosition();
    }

    private void ProcessShipPosition()
    {
        playerInput = new Vector2(_mInput.xThrow, _mInput.yThrow);

        Vector2 offset = new Vector2(playerInput.x * _data.xMoveSpeed * Time.deltaTime, playerInput.y * _data.yMoveSpeed * Time.deltaTime);
        Vector2 rawPosition = new Vector2(transform.localPosition.x + offset.x, transform.localPosition.y - offset.y);

        transform.localPosition = new Vector3
            (Mathf.Clamp(rawPosition.x, -_data.xRange, _data.xRange),
            Mathf.Clamp(rawPosition.y, -_data.yRange, _data.yRange),
            transform.localPosition.z);
    }
}