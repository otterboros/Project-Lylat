// DownedTextController.cs - Update "Downed +" text position, text, and reset when done
//-------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DownedTextController : MonoBehaviour
{
    private Camera gameCamera;
    private TMP_Text dtcText;

    [SerializeField] int resetDelay;

    private Coroutine resettingText;
    private bool isResetting { get { return resettingText != null; } }

    private void Awake()
    {
        gameCamera = Camera.main;
        dtcText = GetComponent<TMP_Text>();
        dtcText.outlineWidth = 0.5f;
        dtcText.outlineColor = new Color32(0, 0, 0, 255);
    }

    public void UpdateDownedTextPosition(Transform lastEnemyTransform)
    {
        transform.position = gameCamera.WorldToScreenPoint(lastEnemyTransform.position);
    }

    public void UpdateDownedText(int comboValue)
    {
        dtcText.text = "Downed + " + (comboValue - 1);
    }

    public void StartResettingDownedText()
    {
        StopResettingDownedText();
        resettingText = StartCoroutine(ResetDownedText());
    }

    public void StopResettingDownedText()
    {
        if (isResetting)
        {
            StopCoroutine(resettingText);
        }
        resettingText = null;
    }

    private IEnumerator ResetDownedText()
    {
        yield return new WaitForSeconds(resetDelay);
        dtcText.text = "";
    }
}
