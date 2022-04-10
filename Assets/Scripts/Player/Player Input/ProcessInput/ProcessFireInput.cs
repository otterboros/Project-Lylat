using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProcessFireInput : MonoBehaviour
{
    private PlayerLaserFiring _psl;
    private ChargedShotManager _csm;
    private ChargedShotData _data;
    private void Awake()
    {
        _psl = GetComponent<PlayerLaserFiring>();
        _csm = GameObject.Find("ChargedShotManager").GetComponent<ChargedShotManager>();
        _data = _csm.GetComponent<ChargedShotData>();
    }

    /// <summary>
    /// Function used by Player Input to determine Firing state based on stages of the "Hold" interaction
    /// </summary>
    /// <param name="context"></param>
    public void Fire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // Activate Normal Shot Lasers on Fire button press
            _psl.StartSpawningBullets();
        }


        if (context.performed)
        {
            // If Fire button is held beyond Hold time threshold, deactivate Normal Shot Lasers and Ready Charged Shot state
            _psl.StopSpawningBullets();
            _csm.StartChargedShotManager();
        }

        if (context.canceled)
        {
            _psl.StopSpawningBullets();

            if (_data.isEnemyTargeted)
            {
                // If Fire button is released with an enemy targeted, fire Charged Shot
                ChargedShotData.isChargedShotFired = true; 
            }
            else
            {
                // If Fire button is released without targetting an enemy, reset Charged Shot Sequence.
                _csm.StopChargedShotManager();
            }
        }
    }
}
