using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{
    // Remove redundancies between this and PlayerControls, if possible

    [SerializeField] InputAction movement;
    [SerializeField] GameObject[] lasers;
    [SerializeField] GameObject chargedShotSphere;
    [SerializeField] GameObject enemyTargetedReticlePrefab;
    [SerializeField] GameObject uiCanvas;
    
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

    float xThrow, yThrow;

    bool isTargetingChargedShot = false;
    bool isChargedTargetAcquired = false;
    bool isChargedShotFired = false;
    GameObject chargedShot;

    [SerializeField] GameObject[] reticles;
    Color defaultReticleColor;

    GameObject enemyTargeted;
    GameObject enemyTargetedReticle;



    private void Awake()
    {
        SetLasersActive(false);
        defaultReticleColor = new Color(0.9058824f, 0.1058823f, 0.6814225f, 1f);

        gameCamera = Camera.main;
    }

    void OnEnable()
    {
        movement.Enable();
    }

    void OnDisable()
    {
        movement.Disable();
    }

    private void ProcessShipPosition()
    {
        xThrow = movement.ReadValue<Vector2>().x;
        yThrow = movement.ReadValue<Vector2>().y;

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

    void Update()
    {
        ProcessShipPosition();
        ProcessShipRotation();
    }
  

    public void Fire(InputAction.CallbackContext context)
    {
        // See "Fire" in Input Action Asset for detail on "Tap" & "Hold" interactions
        if(context.started) 
            SetLasersActive(true); // Fire lasers while button is held above Press Point but beneath Hold time threshold

        if(context.performed) 
            ReadyChargedShot(); // Button has been held on or above Hold time threshold

        if (context.canceled)
        {
            SetLasersActive(false); // Deactivate lasers if button is released before Hold time threshold
            if (isChargedTargetAcquired)
                isChargedShotFired = true; // If Charged shot target is acquired, fire Charged Shot
            else
                CancelChargedShot(); // Otherwise, cancel Charged Shot

            enemyTargeted = null; // Move this to a general CancelCharged that runs after sphere is fired, as well as if its canceled

            // Add a destroy target on enemy, and remove target acquired, if enemy leaves screen


            reticles[0].GetComponent<Image>().color = defaultReticleColor;
            reticles[1].GetComponent<Image>().color = defaultReticleColor;
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
        SetLasersActive(false);

        Vector3 chargedShotPositionAdj = new Vector3(0f, -0.753000021f, 1.30999994f);
        chargedShot = Instantiate(chargedShotSphere, transform.position + chargedShotPositionAdj, Quaternion.identity, transform);

        reticles[0].GetComponent<Image>().color = Color.yellow;
        reticles[1].GetComponent<Image>().color = Color.red;

        // Add ready charged shot sound

        isTargetingChargedShot = true;
    }

    private void FixedUpdate()
    {
        if (isTargetingChargedShot)
            TargetingChargedShot();
        else if (isChargedTargetAcquired)
            PositionEnemyTargetedReticle();
        else if (isChargedShotFired)
            FireChargedShot();
        else
            return;
    }

    void TargetingChargedShot()
    {
        RaycastHit hit;

        int layerMask = 1 << 2;
        layerMask = ~layerMask; // Ignore only objects in the Ignore Raycast layer (2)

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 50, layerMask) && hit.collider.gameObject.tag == "Enemy")
        {
            isTargetingChargedShot = false;
            isChargedTargetAcquired = true;
            enemyTargeted = hit.collider.gameObject;
            Debug.Log("Targeted enemy" + hit.collider.gameObject.transform.name + "!");

            enemyTargetedReticle = Instantiate(enemyTargetedReticlePrefab, gameCamera.WorldToScreenPoint(enemyTargeted.transform.position), Quaternion.identity, uiCanvas.transform);
        }
    }

    void PositionEnemyTargetedReticle()
    {
        enemyTargetedReticle.transform.position = gameCamera.WorldToScreenPoint(enemyTargeted.transform.position);
    }

    void FireChargedShot()
    {
        chargedShot.transform.LookAt(enemyTargeted.transform);
        chargedShot.transform.Translate(Vector3.forward * 1);
    }

    void CancelChargedShot()
    {
        isTargetingChargedShot = false;
        isChargedTargetAcquired = false;
        isChargedShotFired = false;
        Destroy(enemyTargetedReticle);
        Destroy(chargedShot);
    }

    //bool isFiringChargedShot { get { return firingChargedShot != null; } }
    //Coroutine firingChargedShot = null;

    //void FireChargedShot()
    //{
    //    StopFiringChargedShot();
    //    chargedShot.transform.rotation = Quaternion.LookRotation(transform.forward);
    //    firingChargedShot = StartCoroutine(FiringChargedShot());
    //}

    //void StopFiringChargedShot()
    //{
    //    if (isFiringChargedShot)
    //    {
    //        StopCoroutine(firingChargedShot);
    //    }
    //    firingChargedShot = null;
    //}

    //IEnumerator FiringChargedShot()
    //{
    //    // fire sphere that homes in on targetted enemy
    //    // play firing charged shot sound
    //    Collider other;



    //    // run sphere collision method
    //    // destroy targeted reticle on collision
    //    void OnTriggerEnter(Collider other)
    //    {
    //        switch (other.gameObject.tag)
    //        {
    //            case "Friendly":
    //                Debug.Log("This thing is friendly");
    //                break;
    //            case "Enemy":
    //                StartExplosionSequence();
    //                break;
    //            default:
    //                Debug.Log("Somehow, this is neither friendly nor an enemy.");
    //                break;
    //        }
    //    }
    //    yield return new WaitForEndOfFrame();

    //    StopFiringChargedShot();
    //}
}

