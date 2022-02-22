using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{
    private GameObject uiCanvas;
    private List<GameObject> reticles = new List<GameObject>();

    private void Start()
    {
        uiCanvas = GameObject.Find("UI");

        reticles.Add(uiCanvas.transform.Find("CloseReticle").gameObject);
        reticles.Add(uiCanvas.transform.Find("FarReticle").gameObject);
    }

    public void ChangeImageColor(GameObject gameObject, Color newColor)
    {
        gameObject.GetComponent<Image>().color = newColor;
    }

    public void ChangeReticleColor(Color newColor, string reticleName = "")
    {
        foreach (GameObject reticle in reticles)
        {
            if (reticleName == "")
            {
                reticle.GetComponent<Image>().color = newColor;
            }
            else
            {
                if (Equals(reticle.transform.name, reticleName))
                    reticle.GetComponent<Image>().color = newColor;
            }
        }
    }
}
