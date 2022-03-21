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
    private ProcessMovementInput m_Input;

    private bool isBlocked = false;
    private Rigidbody _rb;
    [SerializeField] GameObject playerBoxCollider;
    private Vector3 contactNormal;
    private Vector3 contactPoint;

    // temp vars for force-based move speed. move to data later
    [SerializeField] int xFMoveSPeed;
    [SerializeField] int yFMoveSPeed;

    [SerializeField] int xFMaxSpeedChange;
    [SerializeField] int yFMaxSpeedChange;

    private void Awake()
    {
        _data = GetComponent<PlayerData>();
        m_Input = GetComponent<ProcessMovementInput>();
        _rb = GetComponent<Rigidbody>();

    }

    private void FixedUpdate()
    {
        ProcessShipPosition();
        //BlockedCheck();
    }

    private void ProcessShipPosition()
    {
        playerInput = new Vector2(m_Input.xThrow, -m_Input.yThrow);

        // Physics-based movement
        Vector3 desiredVelocity = new Vector3(playerInput.x * xFMoveSPeed, playerInput.y * yFMoveSPeed, 0);

        Vector3 velocity = _rb.velocity;
        //float maxSpeedChange = maxAcceleration * Time.deltaTime;
        velocity.x =
            Mathf.MoveTowards(velocity.x, desiredVelocity.x, xFMaxSpeedChange);
        velocity.y =
            Mathf.MoveTowards(velocity.y, desiredVelocity.y, yFMaxSpeedChange);
        velocity.z = 0;
        _rb.velocity = velocity;


        // Direct transform movement
        //Vector2 offset = new Vector2(playerInput.x * _data.xMoveSpeed * Time.deltaTime, playerInput.y * _data.yMoveSpeed * Time.deltaTime);
        //Vector2 rawPosition = new Vector2(transform.localPosition.x + offset.x, transform.localPosition.y - offset.y);

        //if (!isBlocked)
        //{
        //    This allows for the ship to be pushed away in "Blocked" state but still keep velocities to a zero.
        //   _rb.velocity = Vector3.zero;
        //    _rb.angularVelocity = Vector3.zero;

        //    Regular movement, frame by frame
        //    transform.localPosition = new Vector3
        //        (Mathf.Clamp(rawPosition.x, -_data.xRange, _data.xRange),
        //        Mathf.Clamp(rawPosition.y, -_data.yRange, _data.yRange),
        //        transform.localPosition.z);



        //}
        //else
        //{
        //    Debug.Log("Blocked!!");
        //}

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "CollisionSafe")
        {
            isBlocked = true;

            ContactPoint contact = collision.contacts[0];

            print(contact.thisCollider.name + " hit " + contact.otherCollider.name);
            // Visualize the contact point
            Debug.DrawRay(contact.point, contact.normal, Color.red);
            Debug.Log($"contact point is {contact.point}");
            Debug.Log($"contact normal is {contact.normal}");

            contactPoint = contact.point;
            contactNormal = contact.normal;
        }
    }

    //private void BlockedCheck()
    //{
    //    isBlocked = Physics.CheckBox(playerBoxCollider.transform.position, playerBoxCollider.GetComponent<BoxCollider>().size / 2, 
    //                                 playerBoxCollider.transform.rotation, _data.blockedLayers, QueryTriggerInteraction.Collide);
    //    _data.blocked = isBlocked;
    //}

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(playerBoxCollider.transform.position, playerBoxCollider.GetComponent<BoxCollider>().size);
    //}
}