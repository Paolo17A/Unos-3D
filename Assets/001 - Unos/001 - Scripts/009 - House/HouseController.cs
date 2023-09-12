using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HouseController : MonoBehaviour
{
    [SerializeField] private HouseCore HouseCore;

    private void Awake()
    {
        GameManager.Instance.SceneController.ActionPass = true;
    }

    private void Start()
    {
        GameManager.Instance.ProgressContainer.SetActive(true);
        HouseCore.PlayStartingDialogue();
    }

    private void Update()
    {
        if(Application.isEditor)
        {
            if (!Mouse.current.leftButton.isPressed)
                return;
            if (!HouseCore.DialogueFinished)
                return;
            if (HouseCore.CurrentDialogue.ThisDialogueType == DialogueData.DialogueType.TALKING || HouseCore.CurrentDialogue.ThisDialogueType == DialogueData.DialogueType.ENDING_SUCCESS || HouseCore.CurrentDialogue.ThisDialogueType == DialogueData.DialogueType.ENDING_FAIL)
                HouseCore.LoadNextDialogue();
            else if (HouseCore.CurrentDialogue.ThisDialogueType == DialogueData.DialogueType.MINIGAME)
                HouseCore.DetectItem(Input.mousePosition);
        }

        else if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            if ((HouseCore.CurrentDialogue.ThisDialogueType == DialogueData.DialogueType.TALKING || HouseCore.CurrentDialogue.ThisDialogueType == DialogueData.DialogueType.ENDING_SUCCESS || HouseCore.CurrentDialogue.ThisDialogueType == DialogueData.DialogueType.ENDING_FAIL) && HouseCore.DialogueFinished)
                HouseCore.LoadNextDialogue();
            else if (HouseCore.CurrentDialogue.ThisDialogueType == DialogueData.DialogueType.MINIGAME)
                HouseCore.DetectItem(Touchscreen.current.primaryTouch.position.ReadValue());
        }
    }
}
