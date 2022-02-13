using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{
    // SOLID
    // Single Responsiblity
    // Open-Closed
    // Liskov Substitution (code interfaces so they can be replaced with any subtype)
    // Interface Separation Principle (many client-specific if better than one general)
    // Dependency Inversion (classes shud be abstraction, not concretion)

    // Responsiblities
    // Handle Movement input
    // Handle firing input
    // FIxedUpdate with all movement and firing functions (this is a great controller/selector)
    // Convert player movement input into ship position
    // A TON OF MATH to convert player movement input into ship rotation, this is also duplicated 
    // Many steps for the Charged Shot, including targetting enemy, positioning reticle, and reseting the states

    [SerializeField] GameObject uiCanvas;

    private List<GameObject> lasers = new List<GameObject>();
    private List<GameObject> reticles = new List<GameObject>();

    Camera gameCamera;

    [Header("Charged Shot Settings")]
    [Tooltip("Maximum Z range of Charged Shot lock-on targeting")]
    [SerializeField] float maximumLockOnRange;

    
    // Variables used by Charged Shot state that are shared with other scripts
    private GameObject chargedShot;
    private GameObject enemyTargeted = null;
    private GameObject enemyTargetedReticle;

    private bool isCShotCreated { get { return chargedShot != null; } }
    private bool isReticleCreated { get { return enemyTargetedReticle != null; } }
    private bool isEnemyTargeted { get { return enemyTargeted != null; } }

    private bool isChargedShotFired = false;

    // Variables used by Charged Shot state
    private GameObject parentGameObject;
    private Color defaultReticleColor;
    private Vector3 chargedShotPositionAdj;

    // A bunch of assignments!
    private void Awake()
    {
        gameCamera = Camera.main;

        lasers.Add(transform.Find("Laser One").gameObject);
        lasers.Add(transform.Find("Laser Two Port").gameObject);
        lasers.Add(transform.Find("Laser Two Starb").gameObject);

        SetLasersActive(false);

        reticles.Add(uiCanvas.transform.Find("CloseReticle").gameObject);
        reticles.Add(uiCanvas.transform.Find("FarReticle").gameObject);

        defaultReticleColor = new Color(0.9058824f, 0.1058823f, 0.6814225f, 1f);

        parentGameObject = GameObject.FindWithTag("CreateAtRuntime");
    }

    // Let's break out player movement and firing into two seperate classes

    /// <summary>
    /// Function used by Player Input to determine Firing state based on stages of the "Hold" interaction
    /// </summary>
    /// <param name="context"></param>
    public void Fire(InputAction.CallbackContext context)
    {
        if(context.started) 
            SetLasersActive(true); // Activate Normal Shot Lasers on Fire button press

        if(context.performed)
        {
            // If Fire button is held beyond Hold time threshold, deactivate Normal Shot Lasers and Ready Charged Shot state
            SetLasersActive(false);
            ReadyChargedShot(); 
        }

        if (context.canceled)
        {
            SetLasersActive(false);
            if (isReticleCreated) // or, if enemyTargeted != null
            {
                isChargedShotFired = true; // If Fire button is released after targeting reticle is created, fire Charged Shot
            }
            else
                ResetChargedShotStates(); // Otherwise, cancel Charged Shot State

            foreach (GameObject reticle in reticles)
                reticle.GetComponent<Image>().color = defaultReticleColor; // Reset reticle colors upon leaving Charged Shot state
        }
    }
    private void FixedUpdate()
    {

        // Functions to Process Charged Shot stages
        if (isCShotCreated)
        {
            if (!isChargedShotFired)
                PositionChargedShot();
            else if (isChargedShotFired)
                FireChargedShot();

            if (!isReticleCreated)
                TargetingChargedShot();
            else if (isReticleCreated)
                PositionEnemyTargetedReticle();

            if (isReticleCreated && !isEnemyTargeted)
            {
                // So far, this case has not showed up often enough to test! Keep debug so I know to check on it if I see the message.
                Debug.Log("Enemy destroyed without firing CS. Retargetting!");
                ResetChargedShotStates(true);
            }
        }
    }

    void SetLasersActive(bool isLaserActive)
    {
        var emission = lasers[0].GetComponent<ParticleSystem>().emission; // Stores the module in a local variable
        emission.enabled = isLaserActive; // Applies the new value directly to the Particle System
        // play firing laser sound (ideally, this will be attached to emission system)
    }
    void ReadyChargedShot()
    {
        chargedShotPositionAdj = new Vector3(0f, -0.753000021f, 1.30999994f);
        chargedShot = Instantiate(Resources.Load<GameObject>("Prefabs/Player/ChargedShot/ChargedShot"), transform.position + chargedShotPositionAdj, Quaternion.identity, parentGameObject.transform);

        reticles[0].GetComponent<Image>().color = Color.yellow;
        reticles[1].GetComponent<Image>().color = Color.red;

        // Add ready charged shot sound
    }

    void TargetingChargedShot()
    {
        RaycastHit hit;

        int layerMask = 1 << 2;
        layerMask = ~layerMask; // Ignore only objects in the Ignore Raycast layer (2)

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, maximumLockOnRange, layerMask) && hit.collider.gameObject.tag == "Enemy")
        {
            enemyTargeted = hit.collider.gameObject;
            // Add sound
            enemyTargetedReticle = Instantiate(Resources.Load<GameObject>("Prefabs/Player/ChargedShot/EnemyTargetedReticle"), gameCamera.WorldToScreenPoint(enemyTargeted.transform.position), Quaternion.identity, uiCanvas.transform);
        }
    }

    void PositionEnemyTargetedReticle()
    {
        enemyTargetedReticle.transform.position = gameCamera.WorldToScreenPoint(enemyTargeted.transform.position);

        if (enemyTargeted.transform.position.z < transform.position.z + 1)
            ResetChargedShotStates();
    }

    void PositionChargedShot()
    {
        chargedShot.transform.position = lasers[0].transform.position;
    }

    void FireChargedShot()
    {
        // Add sound
        chargedShot.transform.LookAt(enemyTargeted.transform);
        chargedShot.transform.Translate(Vector3.forward * 1);
    }

    public void ResetChargedShotStates(bool retarget = false)
    {
        if (retarget == true)
            Destroy(enemyTargetedReticle);
        else
        {
            Destroy(chargedShot);
            Destroy(enemyTargetedReticle);

            enemyTargeted = null;

            isChargedShotFired = false;
        }
    }
}