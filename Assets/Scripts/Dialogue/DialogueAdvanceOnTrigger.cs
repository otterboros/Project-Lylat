using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

/// <summary>
/// Trigger UserRequestedViewAdvancement from a Dialogue View
/// when this Game Object collides with a Game Object with tag
/// DialogueTrigger.
/// </summary>
public class DialogueAdvanceOnTrigger : MonoBehaviour
{
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