using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [SerializeField] GameObject playerShip;

    private CameraData _data;

    private ProcessMovementInput _mInput;
    private Vector2 playerInput;

    private void Awake()
    {
        _data = GetComponent<CameraData>();
         _mInput = playerShip.GetComponent<ProcessMovementInput>();
    }

    private void FixedUpdate()
    {
        ProcessCameraPosition();
    }

    private void ProcessCameraPosition()
    {
        playerInput = new Vector2(_mInput.xThrow, _mInput.yThrow);

        // from player movement
        Vector2 offset = new Vector2(playerInput.x * _data.xMoveSpeed * Time.deltaTime, playerInput.y * _data.yMoveSpeed * Time.deltaTime);
        Vector2 rawPosition = new Vector2(transform.localPosition.x + offset.x, transform.localPosition.y - offset.y);

        transform.localPosition = new Vector3
            (Mathf.Clamp(rawPosition.x, -_data.xRange, _data.xRange),
            Mathf.Clamp(rawPosition.y, -_data.yRange, _data.yRange),
            transform.localPosition.z);
    }
}
