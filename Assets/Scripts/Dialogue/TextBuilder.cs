// TextBuilder.cs - Coroutine for revealing text one character at a time
//----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBuilder : MonoBehaviour
{
    private TMP_Text m_TextComponent;

    public bool isTxtRevealing { get { return revealingTxt != null; } }
    Coroutine revealingTxt;

    /// <summary>
    /// Text Reveal Speed
    /// </summary>
    [SerializeField] float txtSpd;

    void Awake()
    {
        m_TextComponent = gameObject.GetComponent<TMP_Text>();
    }
    #region Start and Stop Text Reveal Coroutines
    /// <summary>
    /// Start Reveal Characters coroutine which reveals the current text one character at a time based on text speed.
    /// Modified from the TMPro.Examplse TextConsoleSimulator.cs
    /// </summary>
    public void StartTextReveal()
    {
        StopTextReveal();
        revealingTxt = StartCoroutine(RevealCharacters(m_TextComponent, txtSpd));
    }

    public void StopTextReveal()
    {
        if (isTxtRevealing)
        {
            StopCoroutine(revealingTxt);
        }
        revealingTxt = null;
    }

    IEnumerator RevealCharacters(TMP_Text textComponent, float txtSpd)
    {
        textComponent.ForceMeshUpdate();

        TMP_TextInfo textInfo = textComponent.textInfo;

        int totalVisibleCharacters = textInfo.characterCount; // Get # of Visible Character in text object
        int visibleCount = 0;

        while (visibleCount <= totalVisibleCharacters)
        {
            textComponent.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?
            visibleCount += 1;

            yield return new WaitForSeconds(txtSpd);
        }
        StopTextReveal();
        DialogueManager.instance.StartClosingDialogue();
    }
    #endregion
}
