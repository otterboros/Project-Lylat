using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedShotReset : MonoBehaviour
{
    private ChargedShotData _data;
    private ColorChanger _cc;

    private void Awake()
    {
        _data = GetComponent<ChargedShotData>();
        _cc = GetComponent<ColorChanger>();

        ResetChargedShotSequence(); // Reset at start of scene
    }

    private void Update()
    {
        if(ChargedShotData.isChargedShotSequenceEnded)
        {
            Debug.Log("Reset CS due to sequence end, at end of explosion combo count.");
            ResetChargedShotSequence();
        }


        if(_data.isEnemyTargeted)
            if (ChargedShotData.enemyTargeted.transform.position.z < transform.position.z - 1)
            {
                Debug.Log("Reset CS due target enemy falling behind player.");
                ResetChargedShotSequence();
            }
    }

    public void ResetChargedShotSequence()
    {
        Destroy(ChargedShotData.chargedShot);
        ChargedShotData.enemyTargeted = null;
        Destroy(ChargedShotData.enemyTargetedReticle);
        ChargedShotData.isChargedShotFired = false;
        ChargedShotData.isChargedShotSequenceEnded = false;
        
        _cc.ChangeReticleColor(new Color(0.9058824f, 0.1058823f, 0.6814225f, 1f));
    }
}
