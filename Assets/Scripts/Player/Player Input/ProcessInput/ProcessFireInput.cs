using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProcessFireInput : MonoBehaviour
{
    // Variables used by Charged Shot state that are shared with other scripts
    

    private GameObject parentGameObject;

    private PlayerLaserFiring _psl;
    private ColorChanger _cc;
    private ChargedShotData _data;
    private ChargedShotReset _csr;

    private Camera gameCamera;
    private GameObject uiCanvas;
    private void Awake()
    {
        parentGameObject = GameObject.FindWithTag("CreateAtRuntime");
        gameCamera = Camera.main;
        uiCanvas = GameObject.Find("UI");

        _psl = GetComponent<PlayerLaserFiring>();
        _cc = GetComponent<ColorChanger>();
        _data = GetComponent<ChargedShotData>();
        _csr = GetComponent<ChargedShotReset>();

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

            Debug.Log("Readying CS!");
            ChargedShotData.chargedShot = Instantiate(Resources.Load<GameObject>("Prefabs/Player/ChargedShot/ChargedShot"), transform.position + _data.chargedShotPositionOffset, Quaternion.identity, parentGameObject.transform);

            _cc.ChangeReticleColor(Color.yellow, "CloseReticle");
            _cc.ChangeReticleColor(Color.red, "FarReticle");
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
                _csr.ResetChargedShotSequence();
            }
        }
    }
}
