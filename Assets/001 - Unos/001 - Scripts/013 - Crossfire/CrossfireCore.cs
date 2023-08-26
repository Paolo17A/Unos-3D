using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrossfireCore : MonoBehaviour, I_MiniGame
{
    #region STATE MACHINE
    //======================================================================================================================
    public enum CrossfireGameStates
    {
        NONE,
        INSTRUCTION,
        GAMEPLAY,
        BURNED,
        SAFE
    }

    private event EventHandler crossfireGameStateChange;
    public event EventHandler onCrossfireGameStateChange
    {
        add
        {
            if (crossfireGameStateChange == null || !crossfireGameStateChange.GetInvocationList().Contains(value))
                crossfireGameStateChange += value;
        }
        remove { crossfireGameStateChange -= value; }
    }

    public CrossfireGameStates CurrentCrossFireGameState
    {
        get => crossFireGameState;
        set
        {
            crossFireGameState = value;
            crossfireGameStateChange?.Invoke(this, EventArgs.Empty);
        }
    }

    [SerializeField] private CrossfireGameStates crossFireGameState;
    //======================================================================================================================
    #endregion

    #region VARIABLES
    //======================================================================================================================
    [Header("DIALOGUE VARIABLES")]
    [SerializeField] private GameObject DialogueContainer;
    [SerializeField] private TextMeshProUGUI DialogueTMP;
    [SerializeField] private GameObject StartGame;
    [ReadOnly] public bool DialogueFinished;

    [Header("GAMEPLAY VARIABLES")]
    [SerializeField] private Joystick MovementStick;
    [SerializeField] private GameObject Player;

    [Header("FAILURE VARIABLES")]
    [SerializeField] private Button RetryBtn;
    //======================================================================================================================
    #endregion

    public void InitializeScene()
    {
        Player.SetActive(true);
        DialogueContainer.SetActive(true);
        ToggleStartGame(false);
        MovementStick.gameObject.SetActive(false);
        RetryBtn.gameObject.SetActive(false);
        StartCoroutine(DisplayInstructionText());
    }

    public IEnumerator DisplayInstructionText()
    {
        DialogueFinished = false;
        DialogueTMP.text = "";
        foreach (char c in GameManager.Instance.CurrentEarthquakeDialogue.DialogueContent)
        {
            DialogueTMP.text += c;
            yield return new WaitForSeconds(0.05f);
        }
        StartGame.SetActive(true);
        DialogueFinished = true;
    }

    public void ToggleStartGame(bool isActive)
    {
        StartGame.SetActive(isActive);
    }

    public void InitializeGameplay()
    {
        DialogueContainer.SetActive(false);
        ToggleStartGame(false);
        MovementStick.gameObject.SetActive(true);
        CurrentCrossFireGameState = CrossfireGameStates.GAMEPLAY;
    }

    public void HandleMovement()
    {
        //throw new NotImplementedException();
    }

    public void HandleSuccess()
    {
        GameManager.Instance.CurrentEarthquakeDialogue = GameManager.Instance.CurrentEarthquakeDialogue.ReturningPointDialogue;
        GameManager.Instance.SceneController.CurrentScene = "SchoolScene";
    }

    public void HandleFailure()
    {
        MovementStick.gameObject.SetActive(false);
        Player.SetActive(false);
        RetryBtn.gameObject.SetActive(true);
    }

    public void RetryGame()
    {
        GameManager.Instance.SceneController.CurrentScene = "CrossfireGameScene";
    }
}
