using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAnimator : MonoBehaviour
{
    Renderer rend;
    Material mat;
    float duration = 1f;
    Color colorStart;
    Color colorEnd;

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
        Debug.Log("Starting damage animation!");
        StopAnimatingDamage(); //If this ship is already animating damage, stop it.
        animatingDamage = StartCoroutine(AnimateDamage());
    }

    public void StopAnimatingDamage()
    {
        Debug.Log("Stoping previous damage animation!");
        if (isAnimatingDamage)
        {
            StopCoroutine(animatingDamage);
        }
        animatingDamage = null;
    }

    IEnumerator AnimateDamage()
    {
        float ctr = duration;
        while (ctr > 0)
        {
            Debug.Log($"Damage animation counter is now {ctr}!");
            ctr -= Time.deltaTime;

            //colorStart = mat.GetColor("_Color");
            colorStart = Color.blue;

            //float emission = Mathf.PingPong(Time.deltaTime, 1.0f);
            //Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);

            float lerp = Mathf.PingPong(Time.time, duration) / duration;
            mat.color = Color.Lerp(colorStart, colorEnd, lerp);

            //mat.SetColor("_Color", finalColor);

            yield return new WaitForEndOfFrame();
        }

        StopAnimatingDamage();
    }
}
