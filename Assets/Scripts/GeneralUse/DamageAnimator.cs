// DamageAnimator.cs - Coroutine to pingpong a color change for this game object
//---------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAnimator : MonoBehaviour
{
    Renderer rend;
    Material mat;
    [SerializeField]  float duration = 0.2f;
    Color colorStart = Color.black;
    [SerializeField] Color colorEnd = Color.red;

    Coroutine animatingDamage;
    bool isAnimatingDamage { get { return animatingDamage != null; } }

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        mat = rend.material;
    }

    public void StartAnimatingDamage()
    {
        StopAnimatingDamage(); //If this ship is already animating damage, stop it.
        animatingDamage = StartCoroutine(AnimateDamage());
    }

    public void StopAnimatingDamage()
    {
        if (isAnimatingDamage)
        {
            StopCoroutine(animatingDamage);
        }
        animatingDamage = null;
    }

    IEnumerator AnimateDamage()
    {
        float ctr = duration;
        mat.EnableKeyword("_EMISSION");

        while (ctr > 0)
        {
            ctr -= Time.deltaTime; 

            float lerp = Mathf.PingPong(Time.time, duration) / duration;
            Color newColor = Color.Lerp(colorStart, colorEnd, lerp);
            mat.SetColor("_EmissionColor", newColor);

            yield return new WaitForEndOfFrame();
        }

        mat.SetColor("_EmissionColor", colorStart);
        mat.DisableKeyword("_EMISSION");


        StopAnimatingDamage();
    }
}
