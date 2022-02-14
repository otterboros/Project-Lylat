using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProcessMovementInput : MonoBehaviour
{
    [HideInInspector] public float xThrow, yThrow;

    public void Move(InputAction.CallbackContext context)
    {
        xThrow = context.ReadValue<Vector2>().x;
        yThrow = context.ReadValue<Vector2>().y;
    }
}
