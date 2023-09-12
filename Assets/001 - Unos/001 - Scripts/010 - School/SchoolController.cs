using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SchoolController : MonoBehaviour
{
    [SerializeField] private SchoolCore SchoolCore;

    private void Awake()
    {
        GameManager.Instance.SceneController.ActionPass = true;
    }

    private void Start()
    {
        GameManager.Instance.ProgressContainer.SetActive(true);
        SchoolCore.PlayStartingDialogue();
    }

    private void Update()
    {
        if (Application.isEditor)
        {
            if (!Mouse.current.leftButton.isPressed)
                return;
            if (!SchoolCore.DialogueFinished)
                return;
            if (GameManager.Instance.CurrentEarthquakeDialogue.ThisDialogueType == DialogueData.DialogueType.TALKING || GameManager.Instance.CurrentEarthquakeDialogue.ThisDialogueType == DialogueData.DialogueType.ENDING_SUCCESS || GameManager.Instance.CurrentEarthquakeDialogue.ThisDialogueType == DialogueData.DialogueType.ENDING_FAIL)
                SchoolCore.LoadNextDialogue();
        }

        else if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (!SchoolCore.DialogueFinished)
                return;
            if ((GameManager.Instance.CurrentEarthquakeDialogue.ThisDialogueType == DialogueData.DialogueType.TALKING || GameManager.Instance.CurrentEarthquakeDialogue.ThisDialogueType == DialogueData.DialogueType.ENDING_SUCCESS || GameManager.Instance.CurrentEarthquakeDialogue.ThisDialogueType == DialogueData.DialogueType.ENDING_FAIL))
                SchoolCore.LoadNextDialogue();
        }
    }
}
