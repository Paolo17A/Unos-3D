using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyController : MonoBehaviour
{
    [SerializeField] private LobbyCore LobbyCore;

    private void Awake()
    {
        GameManager.Instance.SceneController.ActionPass = true;
    }

    private void Start()
    {
        LobbyCore.PlayStartingDialogue();
    }

    private void Update()
    {
        if(Application.isEditor)
        {
            if (Mouse.current.leftButton.isPressed && LobbyCore.CurrentDialogue.ThisDialogueType == DialogueData.DialogueType.TALKING && LobbyCore.DialogueFinished)
            {
                LobbyCore.LoadNextDialogue();
            }
        }
        else
        {
            if (Touchscreen.current.primaryTouch.press.isPressed && LobbyCore.CurrentDialogue.ThisDialogueType == DialogueData.DialogueType.TALKING && LobbyCore.DialogueFinished)
            {
                LobbyCore.LoadNextDialogue();
            }
        }
    }

}
