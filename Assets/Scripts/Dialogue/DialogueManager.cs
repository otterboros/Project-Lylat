// DialogueManager.cs -  (Unfinished) Manager passes new text into TextBuilder
// To-Do: Break up into separate files
//        Add logic to parse script file
//        Load barks and scripts under different conditions
//        Preload scripts, consider static this file
//        Delete testing update function
//--------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.IO;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    private TMP_Text dialogueText;
    private TMP_Text characterText;
    private TextBuilder _tb;

    private string cachedLastSpeaker;

    List<string> data = new List<string>(); // Lines from the chapter file.

    #region Initialization (Awake)
    private void Awake()
    {
        instance = this;

        GameObject dialogueParent = GameObject.Find("Dialogue");
        _tb = dialogueParent.GetComponentInChildren<TextBuilder>();
        dialogueText = dialogueParent.transform.GetChild(0).GetComponent<TMP_Text>();
        characterText = dialogueParent.transform.GetChild(1).GetComponent<TMP_Text>();
    }
    #endregion

    #region Testing DELETE WHEN DONE
    /// <summary>
    /// For testing, delete when done!
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            LoadScriptDoc("test.txt");
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            Next();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            StartClosingDialogue();
        }
    }
    #endregion

    #region Load Script File
    public bool isHandlingScriptFile { get { return handlingScriptFile != null; } }
    Coroutine handlingScriptFile = null;
    int scriptProgress = 0;
    public void LoadScriptDoc(string fileName)
    {
        cachedLastSpeaker = "";

        data = FileManager.LoadFile(FileManager.savPath + "Resources/Dialogue/LevelScripts/" + fileName);
        Debug.Log($"Loading file {fileName}");

        if (handlingScriptFile != null)
        {
            StopCoroutine(handlingScriptFile);
        }
        handlingScriptFile = StartCoroutine(HandlingScriptDoc());
    }
    #endregion

    #region Break Script into Lines
    IEnumerator HandlingScriptDoc()
    {
        scriptProgress = 0; // progress for this chapter file

        while (scriptProgress < data.Count)
        {
            if (_next)
            {
                string line = data[scriptProgress];
               
                ReadLine(data[scriptProgress]);
                scriptProgress++;

                while (_tb.isTxtRevealing)
                {
                    yield return new WaitForEndOfFrame();
                }
                _next = false;
            }
            yield return new WaitForEndOfFrame();
        }
        handlingScriptFile = null;
    }
    #endregion

    #region Process and Reveal Lines with TextBuilder.cs StartTextReveal

    /// <summary>
    /// Read line by creating new class LINE and running TextBuilder TextReveal
    /// </summary>
    /// <param name="rawLine"></param>
    void ReadLine(string rawLine)
    {
        LINE line = Interpret(rawLine);
        
        characterText.text = line.speaker; // Update naming, previous speaker is always current speaker after a Line is processed.
        dialogueText.text = line.currentLine;
        _tb.StartTextReveal();
    }

    public static LINE Interpret(string rawLine)
    {
        return new LINE(rawLine);
    }

    /// <summary>
    /// Used to break lines into speaker and dialogue.
    /// TO-DO: Add commands for barks and script forking
    /// </summary>
    public class LINE
    {
        /// <summary> Determine who is speaking on this line. </summary>
        public string speaker = "";
        public string currentLine = "";

        public LINE(string rawLine)
        {
            string[] speakerAndDialogue = rawLine.Split('"'); // split line along quotations

            if(speakerAndDialogue[0] == "")
            {
                speaker = DialogueManager.instance.cachedLastSpeaker;
            }
            else 
            {
                speaker = speakerAndDialogue[0];
                if (speaker[speaker.Length - 1] == ' ')
                {
                    speaker = speaker.Remove(speaker.Length - 1);
                }
                DialogueManager.instance.cachedLastSpeaker = speaker;
            }
            currentLine = speakerAndDialogue[1];
        }
    }
    #endregion

    #region Next Field & Method
    /// <summary>
    /// Trigger that advances progress through a chapter file.
    /// </summary>
    static bool _next = false;
    /// <summary>
    /// Move to the next line of the chapter when _next = true;
    /// </summary>
    public void Next()
    {
        _next = true;
    }
    #endregion

    #region Close Dialogue

    /// <summary>
    /// Dialogue Closing Speed
    /// </summary>
    [SerializeField] float closingSpeed;
    bool isDialogueClosing { get { return closingDialogue != null; } }
    Coroutine closingDialogue;
    /// <summary>
    /// Close Dialouge box
    /// TO-DO: Update with animation rather than just erasing text
    /// </summary>
    public void StartClosingDialogue()
    {
        StopClosingDialogue();
        closingDialogue = StartCoroutine(CloseDialogue());
    }

    public void StopClosingDialogue()
    {
        if (isDialogueClosing)
        {
            StopCoroutine(closingDialogue);
        }
        closingDialogue = null;
    }

    IEnumerator CloseDialogue()
    {
        while (_tb.isTxtRevealing) // If text is still revealing, wait for it to finish
        {
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(closingSpeed); // Wait closingSpeed seconds
        // Play closing animation & sound
        dialogueText.text = "";
        characterText.text = "";
        StopClosingDialogue();
    }
    #endregion
}