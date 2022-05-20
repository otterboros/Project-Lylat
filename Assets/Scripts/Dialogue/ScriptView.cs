// ScriptView.cs - A Custom subclass of DialogueViewBase for Starfox 64-like
//                 linear script-based dialogue.
// To-DO: Edit Dismiss, Run, and Interupt to work with what we want!
//--------------------------------------------------------------

using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

using Yarn.Unity;


public class ScriptView : DialogueViewBase
{
    // The canvas group that contains the UI elements used by this Line
    [SerializeField] internal CanvasGroup canvasGroup;

    // The object that displays the text of dialogue lines.
    [SerializeField] internal TextMeshProUGUI lineText = null;

    // The amount of time that lines will take to appear.
    [SerializeField] internal float appearanceTime = 0.5f;

    // The amount of time that lines will take to disappear.
    [SerializeField] internal float disappearanceTime = 0.5f;

    // The current <see cref="LocalizedLine"/> that this line view is displaying.
    LocalizedLine currentLine = null;

    /// <summary>
    /// Controls whether the <see cref="lineText"/> object will show the
    /// character name present in the line or not.
    /// </summary>
    /// <remarks>
    /// <para style="note">This value is only used if <see
    /// cref="characterNameText"/> is <see langword="null"/>.</para>
    /// <para>If this value is <see langword="true"/>, any character names
    /// present in a line will be shown in the <see cref="lineText"/>
    /// object.</para>
    /// <para>If this value is <see langword="false"/>, character names will
    /// not be shown in the <see cref="lineText"/> object.</para>
    /// </remarks>
    [SerializeField]
    [UnityEngine.Serialization.FormerlySerializedAs("showCharacterName")]
    internal bool showCharacterNameInLineView = true;

    // The object that displays the character names found in dialogue lines.
    [SerializeField] internal TextMeshProUGUI characterNameText = null;

    /// <summary>
    /// A stop token that is used to interrupt the current animation.
    /// </summary>
    Effects.CoroutineInterruptToken currentStopToken = new Effects.CoroutineInterruptToken();

    public override void DismissLine(Action onDismissalComplete)
    {
        currentLine = null;

        var interactable = canvasGroup.interactable;
        canvasGroup.interactable = false;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        // turning interaction back on, if it needs it
        canvasGroup.interactable = interactable;
        onDismissalComplete();
    }

    /// <inheritdoc/>
    public override void InterruptLine(LocalizedLine dialogueLine, Action onInterruptLineFinished)
    {
        currentLine = dialogueLine;

        // Cancel all coroutines that we're currently running. This will
        // stop the RunLineInternal coroutine, if it's running.
        StopAllCoroutines();

        // for now we are going to just immediately show everything
        // later we will make it fade in
        lineText.gameObject.SetActive(true);
        canvasGroup.gameObject.SetActive(true);

        int length;

        if (characterNameText == null)
        {
            if (showCharacterNameInLineView)
            {
                lineText.text = dialogueLine.Text.Text;
                length = dialogueLine.Text.Text.Length;
            }
            else
            {
                lineText.text = dialogueLine.TextWithoutCharacterName.Text;
                length = dialogueLine.TextWithoutCharacterName.Text.Length;
            }
        }
        else
        {
            characterNameText.text = dialogueLine.CharacterName;
            lineText.text = dialogueLine.TextWithoutCharacterName.Text;
            length = dialogueLine.TextWithoutCharacterName.Text.Length;
        }

        // Show the entire line's text immediately.
        lineText.maxVisibleCharacters = length;

        // Make the canvas group fully visible immediately, too.
        canvasGroup.alpha = 1;

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        onInterruptLineFinished();
    }

    ///// <inheritdoc/>
    //public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    //{
    //    // Stop any coroutines currently running on this line view (for
    //    // example, any other RunLine that might be running)
    //    StopAllCoroutines();

    //    // Begin running the line as a coroutine.
    //    StartCoroutine(RunLineInternal(dialogueLine, onDialogueLineFinished));
    //}

    //private IEnumerator RunLineInternal(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    //{
    //    IEnumerator PresentLine()
    //    {
    //        lineText.gameObject.SetActive(true);
    //        canvasGroup.gameObject.SetActive(true);

    //        if (characterNameText != null)
    //        {
    //            // If we have a character name text view, show the character
    //            // name in it, and show the rest of the text in our main
    //            // text view.
    //            characterNameText.text = dialogueLine.CharacterName;
    //            lineText.text = dialogueLine.TextWithoutCharacterName.Text;
    //        }
    //        else
    //        {
    //            // We don't have a character name text view. Should we show
    //            // the character name in the main text view?
    //            if (showCharacterNameInLineView)
    //            {
    //                // Yep! Show the entire text.
    //                lineText.text = dialogueLine.Text.Text;
    //            }
    //            else
    //            {
    //                // Nope! Show just the text without the character name.
    //                lineText.text = dialogueLine.TextWithoutCharacterName.Text;
    //            }
    //        }

        
    //        // Ensure that the max visible characters is effectively
    //        // unlimited.
    //        lineText.maxVisibleCharacters = int.MaxValue;
    //    }
    //    currentLine = dialogueLine;

    //    // Run any presentations as a single coroutine. If this is stopped,
    //    // which UserRequestedViewAdvancement can do, then we will stop all
    //    // of the animations at once.
    //    yield return StartCoroutine(PresentLine());

    //    currentStopToken.Complete();

    //    // All of our text should now be visible.
    //    lineText.maxVisibleCharacters = int.MaxValue;

    //    // Our view should at be at full opacity.
    //    canvasGroup.alpha = 1f;
    //    canvasGroup.blocksRaycasts = true;


    //    // If we have a hold time, wait that amount of time, and then
    //    // continue.
    //    if (holdTime > 0)
    //    {
    //        yield return new WaitForSeconds(holdTime);
    //    }

    //    if (autoAdvance == false)
    //    {
    //        // The line is now fully visible, and we've been asked to not
    //        // auto-advance to the next line. Stop here, and don't call the
    //        // completion handler - we'll wait for a call to
    //        // UserRequestedViewAdvancement, which will interrupt this
    //        // coroutine.
    //        yield break;
    //    }

    //    // Our presentation is complete; call the completion handler.
    //    onDialogueLineFinished();
    //}

    /// <inheritdoc/>
    public override void UserRequestedViewAdvancement()
    {
        // We received a request to advance the view. If we're in the middle of
        // an animation, skip to the end of it. If we're not current in an
        // animation, interrupt the line so we can skip to the next one.

        // we have no line, so the user just mashed randomly
        if (currentLine == null)
        {
            return;
        }

        // we may want to change this later so the interrupted
        // animation coroutine is what actually interrupts
        // for now this is fine.
        // Is an animation running that we can stop?
        if (currentStopToken.CanInterrupt)
        {
            // Stop the current animation, and skip to the end of whatever
            // started it.
            currentStopToken.Interrupt();
        }
        // No animation is now running. Signal that we want to
        // interrupt the line instead.
        requestInterrupt?.Invoke();
    }
}