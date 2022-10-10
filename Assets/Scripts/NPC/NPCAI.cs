using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAI : BaseAI
{
    protected NPCData _NPCData;

    protected override void Awake()
    {
        base.Awake();

        if (_data.TryGetComponent<NPCData>(out NPCData NPCData)) { _NPCData = NPCData; }
        else { Debug.Log("Error! This NPC object is missing NPC data."); }
    }

    protected void Update()
    {
        if(_NPCData.target != null) // if current target is destroyed.
        {
            OnTargetDestroy();
        }
    }

    protected override IEnumerator SpawnBullet()
    {
        while (true)
        {
            _bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/EnemyAttack/EnemyLaserBullet"), transform.position, Quaternion.identity, createAtRuntimeObj.transform);
            SetBulletProperties();
            yield return new WaitForSeconds(1 / _data.shotsPerSecond);
        }
    }

    protected virtual void OnTargetDestroy()
    {
        // Play bark if killed by self
        // Play bark if killed by player
        var enemyDamage = _NPCData.target.GetComponent<EnemyDamage>();

        if(enemyDamage.currentAttacker.transform.tag == "Player")
        {
            Debug.Log("Bark for player killing enemy's target.");
        }
        else if (enemyDamage.currentAttacker == this.gameObject)
        {
            Debug.Log("Bark for NPC killing an enemy");
        }

        // Acquire new target
        // Disengage
    }
}