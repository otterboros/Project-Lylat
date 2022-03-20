// PlayerMovement.cs - Update player ship transform based on input
//                     & check if ship has collided with an object
//                     that doesn't impart damage but isn't passthru
//------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 playerInput;

    private PlayerData _data;
    private ProcessMovementInput _mInput;

    private bool isBlocked = false;
    private Rigidbody _rb;
    [SerializeField] GameObject _playerBoxCollider;

    private void Awake()
    {
        _data = GetComponent<PlayerData>();
        _mInput = GetComponent<ProcessMovementInput>();

        _rb = GetComponent<Rigidbody>();
        //_playerBoxCollider = GetComponentInChildren<BoxCollider>().gameObject;
    }

    private void FixedUpdate()
    {
        ProcessShipPosition();
        BlockedCheck();
    }

    private void ProcessShipPosition()
    {
        playerInput = new Vector2(_mInput.xThrow, _mInput.yThrow);

        Vector2 offset = new Vector2(playerInput.x * _data.xMoveSpeed * Time.deltaTime, playerInput.y * _data.yMoveSpeed * Time.deltaTime);
        Vector2 rawPosition = new Vector2(transform.localPosition.x + offset.x, transform.localPosition.y - offset.y);

        if (!isBlocked)
        {
            // This allows for the ship to be pushed away in "Blocked" state but still keep velocities to a zero.
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;

            // Regular movement, frame by frame
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
        isBlocked = Physics.CheckBox(_playerBoxCollider.transform.position, _playerBoxCollider.GetComponent<BoxCollider>().size/2, 
                                     Quaternion.identity, _data.blockedLayers, QueryTriggerInteraction.Collide);
        _data.blocked = isBlocked;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_playerBoxCollider.transform.position, _playerBoxCollider.GetComponent<BoxCollider>().size);
    }
}