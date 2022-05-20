using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yarn.Unity
{
    /// <summary>
    /// A version of the component DialogueAdvancedInput that
    /// checks for collision or triggering from a game object that
    /// the user wishes to advance dialogue.
    ///<para>When the configured input occurs, this component calls the <see
    /// cref="DialogueViewBase.UserRequestedViewAdvancement"/> method on its
    /// <see cref="dialogueView"/>.
    /// </para>
    /// </summary>
    public class DialogueAdvanceOnTrigger : MonoBehaviour
    {
        /// <summary>
        /// The dialogue view that will be notified when the user performs the
        /// advance input (as configured by <see cref="continueActionType"/> and
        /// related fields.)
        /// </summary>
        /// <remarks>
        /// When the input is performed, this dialogue view will have its <see
        /// cref="DialogueViewBase.UserRequestedViewAdvancement"/> method
        /// called.
        /// </remarks>
        [SerializeField] public DialogueViewBase dialogueView;

        internal void Start()
        {
            if (dialogueView == null)
            {
                dialogueView = GetComponent<DialogueViewBase>();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "DialogueTrigger")
            {
                Debug.Log("Loading next linear script line!");

                dialogueView.UserRequestedViewAdvancement();
            }
        }
    }
}