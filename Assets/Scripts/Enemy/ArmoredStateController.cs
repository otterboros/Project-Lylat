// ArmoredStateController.cs - Set armored state by emission color and
//                             destroy this class instance when armor is
//                             deactivated
//-------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmoredStateController : MonoBehaviour
{
    private EnemyData _data;

    private Renderer rend;
    private Material mat;
    private Color colorArmored = Color.yellow;

    private void Awake()
    {
        _data = GetComponent<EnemyData>();

        rend = GetComponent<Renderer>();
        mat = rend.material;
    }

    private void Start()
    {
        ActivateArmor();
    }

    private void ActivateArmor()
    {
        if (_data.isArmored == true)
        {
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", colorArmored);
        }
    }

    public void DeactivateArmor()
    {
        mat.SetColor("_EmissionColor", Color.black);
        mat.DisableKeyword("_EMISSION");

        Destroy(this);
    }
}
