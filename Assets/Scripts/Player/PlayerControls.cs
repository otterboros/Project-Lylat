using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] GameObject uiCanvas;

    private List<GameObject> lasers = new List<GameObject>();
    private List<GameObject> reticles = new List<GameObject>();

    Camera gameCamera;

    [Header("Ship Movement Settings")]
    [Tooltip("How fast ship moves in response to player input")]
    [SerializeField] float xDodgeSpeed;
    [SerializeField] float yDodgeSpeed;

    [Tooltip("How far ship can move from 0,0 in response to player input")]
    public float xShipRange;
    public float yShipRange;

    [Header("Ship Rotation Settings")]
    [Tooltip("How far ship can rotate in response to player input")]
    public float shipPitchRatio;
    public float shipYawRatio;
    [SerializeField] float shipRollRatio;

    [Tooltip("Rate of change from current to desired rotation")]
    [SerializeField] float InterpDuration;

    // Used to store input from movement controls
    private float xThrow, yThrow;
    
    // Variables used by Charged Shot state that are shared with other scripts
    private static GameObject chargedShot;
    private static GameObject enemyTargeted = null;
    private static GameObject enemyTargetedReticle;

    private static bool isTargetingChargedShot = false;
    private static bool isChargedTargetAcquired = false;
    private static bool isChargedShotFired = false;

    // Variables used by Charged Shot state
    private GameObject parentGameObject;
    private Color defaultReticleColor;
    private Vector3 chargedShotPositionAdj;

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

    /// <summary>
    /// Function used by Player Input to log changes in Movement
    /// </summary>
    /// <param name="context"></param>
    public void Move(InputAction.CallbackContext context)
    {
        xThrow = context.ReadValue<Vector2>().x;
        yThrow = context.ReadValue<Vector2>().y;
    }
  
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
            if (isChargedTargetAcquired) // If Fire button is released after acquiring Charged Shot target, fire Charged Shot
            {
                isChargedShotFired = true; 
                isChargedTargetAcquired = false;
            }
            else
                ResetChargedShotStates(); // Otherwise, cancel Charged Shot State

            foreach (GameObject reticle in reticles)
                reticle.GetComponent<Image>().color = defaultReticleColor; // Reset reticle colors upon leaving Charged Shot state
        }
    }
    private void FixedUpdate()
    {
        // Functions to Process Movement
        ProcessShipPosition();
        ProcessShipRotation();

        // Functions to Process Charged Shot stages
        if (isTargetingChargedShot)
            TargetingChargedShot();

        if (isTargetingChargedShot || isChargedTargetAcquired)
            PositionChargedShot();

        if (isChargedTargetAcquired || isChargedShotFired)
            PositionEnemyTargetedReticle();

        if (isChargedShotFired)
            FireChargedShot();
    }

    private void ProcessShipPosition()
    {
        float xOffset = xThrow * xDodgeSpeed * Time.deltaTime;
        float yOffset = yThrow * yDodgeSpeed * Time.deltaTime;

        float rawXPosition = transform.localPosition.x + xOffset;
        float newXPosition = Mathf.Clamp(rawXPosition, -xShipRange, xShipRange);

        float rawYPosition = transform.localPosition.y - yOffset; // negative offset here to invert vertical controls
        float newYPosition = Mathf.Clamp(rawYPosition, -yShipRange, yShipRange);

        transform.localPosition = new Vector3
            (newXPosition,
            newYPosition,
            transform.localPosition.z);
    }

    private void ProcessShipRotation()
    {
        float currentPitch, currentYaw, currentRoll;
        float rawPitch, rawYaw, rawRoll;
        float pitch, yaw, roll;
        Vector3 currentEulerAngles = transform.localRotation.eulerAngles;
        float Interp = Time.deltaTime / InterpDuration;

        float targetPitch = -yThrow * shipPitchRatio;
        float targetYaw = xThrow * shipYawRatio;
        float targetRoll = xThrow * shipRollRatio;

        if (currentEulerAngles.x > 180) // Convert to usable values for yThrow input
            currentPitch = 360 - currentEulerAngles.x;
        else
            currentPitch = -currentEulerAngles.x;
        rawPitch = Mathf.Lerp(currentPitch, targetPitch, Interp);
        if (rawPitch > 0) // Convert back to euler Angles
            pitch = 360 - rawPitch;
        else
            pitch = -rawPitch;

        if (currentEulerAngles.y > 180) // Convert to usable values for xThrow input
            currentYaw = -360 + currentEulerAngles.y;
        else
            currentYaw = currentEulerAngles.y;

        rawYaw = Mathf.Lerp(currentYaw, targetYaw, Interp);
        if (rawYaw < 0) // Convert back to euler Angles
            yaw = 360 + rawYaw;
        else
            yaw = rawYaw;

        if (currentEulerAngles.z > 180) // Convert to usable values for xThrow input
            currentRoll = 360 - currentEulerAngles.z;
        else
            currentRoll = -currentEulerAngles.z;
        rawRoll = Mathf.Lerp(currentRoll, targetRoll, Interp);
        if (rawRoll > 0) // Convert back to euler Angles
            roll = 360 - rawRoll;
        else
            roll = -rawRoll;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    void SetLasersActive(bool isLaserActive)
    {
        var emission = lasers[0].GetComponent<ParticleSystem>().emission; // Stores the module in a local variable
        emission.enabled = isLaserActive; // Applies the new value directly to the Particle System
        // play firing laser sound (ideally, this will be attached to emission system)
    }
    void ReadyChargedShot()
    {
        SetLasersActive(false);

        chargedShotPositionAdj = new Vector3(0f, -0.753000021f, 1.30999994f);
        chargedShot = Instantiate(Resources.Load<GameObject>("Prefabs/ChargedShot"), transform.position + chargedShotPositionAdj, Quaternion.identity, parentGameObject.transform);

        reticles[0].GetComponent<Image>().color = Color.yellow;
        reticles[1].GetComponent<Image>().color = Color.red;

        // Add ready charged shot sound

        isTargetingChargedShot = true;
    }

    void TargetingChargedShot()
    {
        RaycastHit hit;

        int layerMask = 1 << 2;
        layerMask = ~layerMask; // Ignore only objects in the Ignore Raycast layer (2)

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 100, layerMask) && hit.collider.gameObject.tag == "Enemy")
        {
            isTargetingChargedShot = false;
            isChargedTargetAcquired = true;
            enemyTargeted = hit.collider.gameObject;

            // Add sound

            enemyTargetedReticle = Instantiate(Resources.Load<GameObject>("Prefabs/EnemyTargetedReticle"), gameCamera.WorldToScreenPoint(enemyTargeted.transform.position), Quaternion.identity, uiCanvas.transform);
        }
    }

    void PositionEnemyTargetedReticle()
    {
        enemyTargetedReticle.transform.position = gameCamera.WorldToScreenPoint(enemyTargeted.transform.position);
    }

    void PositionChargedShot()
    {
        chargedShot.transform.position = transform.position + chargedShotPositionAdj;
    }

    void FireChargedShot()
    {
        // Add sound
        chargedShot.transform.LookAt(enemyTargeted.transform);
        chargedShot.transform.Translate(Vector3.forward * 1);
    }

    public static void ResetChargedShotStates()
    {
        isTargetingChargedShot = false;
        isChargedTargetAcquired = false;
        isChargedShotFired = false;

        enemyTargeted = null;

        Destroy(enemyTargetedReticle);
        Destroy(chargedShot);
    }
}