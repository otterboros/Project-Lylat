using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool isBlocked = false;

    private Vector2 playerInput;

    private PlayerData _data;
    private ProcessMovementInput _mInput;

    private Rigidbody _rb;

    private void Awake()
    {
        _data = GetComponent<PlayerData>();
        _mInput = GetComponent<ProcessMovementInput>();
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        ProcessShipPosition();
        BlockedCheck();
        //VelocityCap();
    }

    private void ProcessShipPosition()
    {
        playerInput = new Vector2(_mInput.xThrow, _mInput.yThrow);

        Vector2 offset = new Vector2(playerInput.x * _data.xMoveSpeed * Time.deltaTime, playerInput.y * _data.yMoveSpeed * Time.deltaTime);
        Vector2 rawPosition = new Vector2(transform.localPosition.x + offset.x, transform.localPosition.y - offset.y);

        if (!isBlocked)
        {
            transform.localPosition = new Vector3
                (Mathf.Clamp(rawPosition.x, -_data.xRange, _data.xRange),
                Mathf.Clamp(rawPosition.y, -_data.yRange, _data.yRange),
                transform.localPosition.z);
        }
        else
        {
            Debug.Log("Blocked!!");
        }
      
    }

    private void BlockedCheck()
    {
        // set box position, with offset
        Vector3 cubePosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 blockedDimensions = new Vector3(3f, 1.5f, 3f);
        isBlocked = Physics.CheckBox(cubePosition, blockedDimensions, Quaternion.identity, _data.blockedLayers, QueryTriggerInteraction.Collide);
        _data.blocked = isBlocked;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(6f, 3f, 6f));
    }

    //private void VelocityCap()
    //{
    //    if(_rb.velocity.x > 1)
    //    {
    //        _rb.AddForce(new Vector3 (1,0,0), ForceMode.VelocityChange);
    //    }

    //    if (_rb.velocity.x < -1)
    //    {
    //        _rb.AddForce(new Vector3(-1, 0, 0), ForceMode.VelocityChange);
    //    }

    //    if (_rb.velocity.y > 1)
    //    {
    //        _rb.AddForce(new Vector3(0, 1, 0), ForceMode.VelocityChange);
    //    }

    //    if (_rb.velocity.y < -1)
    //    {
    //        _rb.AddForce(new Vector3(0, -1, 0), ForceMode.VelocityChange);
    //    }

    //}
}