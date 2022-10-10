// EnemyAI.cs - AI for enemies, derived from BaseAI
//-----------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : BaseAI
{
    protected GameObject enemyTargetParent;
    protected GameObject player;

    protected EnemyData _eData;

    protected override void Awake()
    {
        base.Awake();
        if (_data.TryGetComponent<EnemyData>(out EnemyData eData)) { _eData = eData; }
        else { Debug.Log("Error! This enemy object is missing enemy data."); }

        enemyTargetParent = GameObject.Find("EnemyTargets");
        player = GameObject.Find("PlayerShip2");
    }

#region Spawn Bullets
    protected override IEnumerator SpawnBullet()
    {
        while(true)
        {
            Debug.Log("Spawning Bullets!");
            _bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/EnemyAttack/EnemyLaserBullet"), transform.position, Quaternion.identity, createAtRuntimeObj.transform);
            SetBulletProperties();
            yield return new WaitForSeconds(1/_data.shotsPerSecond);
        }
    }
    #endregion

    #region Rotate to Face Target

    private Coroutine rotating;
    private bool isRotating { get{ return rotating != null; } }

    public void SelectTarget(string targetName)
    {
        Debug.Log("Rotating to face target!");
        switch (targetName)
        {
            case "player": case "Player":
                currentTarget = player.gameObject;
                break;
            case "10":
                currentTarget = enemyTargetParent.transform.GetChild(1).gameObject;
                break;
            default:
                Debug.Log("Error! Target was not assigned.");
                break;
        }
        StartRotatingToTarget();
    }

    public void StartRotatingToTarget()
    {
        StopRotatingToTarget(); //If this ship is rotating, stop rotating.
        rotating = StartCoroutine(RotateToTarget());
    }

    public void StopRotatingToTarget()
    {
        if (isRotating)
        {
            StopCoroutine(rotating);
        }
        rotating = null;
    }

    IEnumerator RotateToTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(currentTarget.transform.position - transform.position);
        while (transform.rotation != targetRotation)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _eData.shipRotationSpeed);
            yield return new WaitForEndOfFrame();
        }
        StopRotatingToTarget();
    }
    #endregion
}
