// ChargedShotManager.cs - Coroutine manages the stages of spawning, targeting, and firing a charged shot
// TO-DO: Add a reset state that if charged shot leaves the camera it resets
//-------------------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedShotManager : MonoBehaviour
{
    private ChargedShotData _data;
    private GameObject parentGameObject;
    private GameObject playerShip;
    private EnemyTargeter _em;
    private ObjectFiring _of;
    private ColorChanger _cc;
    private Camera gameCamera;
    private GameObject uiCanvas;

    private Coroutine managingChargedShot;
    private bool isChargedShotManagerActive { get { return managingChargedShot != null; } }

    private void Awake()
    {
        parentGameObject = GameObject.FindWithTag("CreateAtRuntime");
        playerShip = GameObject.Find("PlayerShip2");
        _data = GetComponent<ChargedShotData>();
        _em = GetComponent<EnemyTargeter>();
        _of = GetComponent<ObjectFiring>();
        _cc = GetComponent<ColorChanger>();
        gameCamera = Camera.main;
        uiCanvas = GameObject.Find("UI");

        ChargedShotData.chargedShot = Instantiate(Resources.Load<GameObject>("Prefabs/Player/ChargedShot/ChargedShotManaged"),
                                                  transform.position, Quaternion.identity, parentGameObject.transform);
        ChargedShotData.chargedShot.SetActive(false);

        ChargedShotData.enemyTargetedReticle = Instantiate(Resources.Load<GameObject>("Prefabs/Player/ChargedShot/EnemyTargetedReticle"), 
                                                           transform.position, Quaternion.identity, uiCanvas.transform);
        ChargedShotData.enemyTargetedReticle.SetActive(false);
    }

    public void StartChargedShotManager()
    {
        StopChargedShotManager(); //If a charged shot coroutine is running, stop it and reset the charged shot.

        Debug.Log("Starting charged shot manager!");
        SetupChargedShot();
        managingChargedShot = StartCoroutine(ManageChargedShot());
    }

    public void StopChargedShotManager()
    {
        Debug.Log("Stopping charged shot manager!");
        // Stop the coroutine and reset the charged shot.
        if (isChargedShotManagerActive)
        {
            StopCoroutine(managingChargedShot);
        }
        managingChargedShot = null;

        ResetChargedShotSequence();
    }

    private void SetupChargedShot()
    {
        ChargedShotData.chargedShot.SetActive(true);
        PositionUpdater.UpdatePosition(ChargedShotData.chargedShot.transform, playerShip.transform, _data.chargedShotPositionOffset);
        _cc.ChangeReticleColor(Color.yellow, "CloseReticle");
        _cc.ChangeReticleColor(Color.red, "FarReticle");
    }

    IEnumerator ManageChargedShot()
    {
        while(true)
        {
            if (ChargedShotData.isChargedShotSequenceEnded) // last step, reset after ChargedShotExplosion.cs has handled CS damage and combo
            {
                Debug.Log("Reset CS due to sequence end, at end of explosion combo count.");
                StopChargedShotManager();
            }
            else if (!ChargedShotData.isChargedShotFired) // steps 1 & 2 - charged shot position is fixed against player location
            {
                PositionUpdater.UpdatePosition(ChargedShotData.chargedShot.transform, playerShip.transform, _data.chargedShotPositionOffset);

                if (!ChargedShotData.enemyTargeted) // step 1 - player can target an enemy
                {
                    _em.TargetingChargedShot();
                }
                else if (ChargedShotData.enemyTargeted) // step 2 - enemy is targetted
                {
                    ChargedShotData.enemyTargetedReticle.transform.position = gameCamera.WorldToScreenPoint(ChargedShotData.enemyTargeted.transform.position);

                    if (ChargedShotData.enemyTargeted.transform.position.z < transform.position.z - 1)
                    {
                        Debug.Log("Reset CS due target enemy falling behind player.");
                        StopChargedShotManager();
                    }
                }
            }
            else if (ChargedShotData.isChargedShotFired) // step 3 - player fires charged shot at targetted enemy
            {
                ChargedShotData.enemyTargetedReticle.transform.position = gameCamera.WorldToScreenPoint(ChargedShotData.enemyTargeted.transform.position);

                _of.FireHomingObjectAtTarget(ChargedShotData.enemyTargeted.transform, ChargedShotData.chargedShot.transform, _data.chargedShotSpeed);
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public void ResetChargedShotSequence()
    {
        ChargedShotData.chargedShot.SetActive(false);
        ChargedShotData.enemyTargeted = null;
        ChargedShotData.enemyTargetedReticle.SetActive(false);
        ChargedShotData.isChargedShotFired = false;
        ChargedShotData.isChargedShotSequenceEnded = false;

        _cc.ChangeReticleColor(new Color(0.9058824f, 0.1058823f, 0.6814225f, 1f));
    }
}
