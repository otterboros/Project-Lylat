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
    [SerializeField] DialogueRunner dialogueRunner;
    [SerializeField] ScriptView scriptView;
    [SerializeField] string sceneNode;

    internal void Start()
    {
        if (dialogueRunner == null)
        {
            dialogueRunner = FindObjectOfType<DialogueRunner>();
        }
        
        if (scriptView == null)
        {
            scriptView = FindObjectOfType<ScriptView>();
        }

        if (sceneNode == "")
        {
            sceneNode = "StartScript";
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "DialogueTrigger")
        {
            if(!dialogueRunner.IsDialogueRunning)
            {
                // start node
                dialogueRunner.startNode = sceneNode;
                dialogueRunner.StartDialogue(dialogueRunner.startNode);
            }
            else
            {
                Debug.Log("Loading next linear script line!");
                scriptView.UserRequestedViewAdvancement();
            }
        }
    }
}