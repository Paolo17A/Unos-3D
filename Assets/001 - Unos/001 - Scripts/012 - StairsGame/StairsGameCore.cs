using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StairsGameCore : MonoBehaviour, I_MiniGame
{
    #region STATE MACHINE
    //======================================================================================================================
    public enum StairsGameStates
    {
        NONE,
        INSTRUCTION,
        GAMEPLAY,
        STONED,
        SAFE
    }

    private event EventHandler stairsGameStateChange;
    public event EventHandler onStairsGameStateChange
    {
        add
        {
            if (stairsGameStateChange == null || !stairsGameStateChange.GetInvocationList().Contains(value))
                stairsGameStateChange += value;
        }
        remove { stairsGameStateChange -= value; }
    }

    public StairsGameStates CurrentStairsGameState
    {
        get => stairsGameState;
        set
        {
            stairsGameState = value;
            stairsGameStateChange?.Invoke(this, EventArgs.Empty);
        }
    }

    [SerializeField] private StairsGameStates stairsGameState;
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
    [SerializeField] private List<Transform> DebrisSpawnPoints;
    [SerializeField] private GameObject DebrisPrefab;
    [SerializeField] private Button LeftBtn;
    [SerializeField] private Button RightBtn;
    [SerializeField] private GameObject Player;

    [Header("FAILURE VARIABLES")]
    [SerializeField] private Button RetryBtn;

    [Header("AUDIO")]
    [SerializeField] private AudioClip GameBackgroundMusic;
    //======================================================================================================================
    #endregion

    public void InitializeScene()
    {
        GameManager.Instance.AudioManager.SetBackgroundMusic(GameBackgroundMusic);
        Player.SetActive(true);
        DialogueContainer.SetActive(true);
        ToggleStartGame(false);
        LeftBtn.gameObject.SetActive(false);
        RightBtn.gameObject.SetActive(false);
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
            yield return new WaitForSeconds(0.025f);
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
        LeftBtn.gameObject.SetActive(true);
        RightBtn.gameObject.SetActive(true);
        CurrentStairsGameState = StairsGameStates.GAMEPLAY;
    }

    //  We'll make use of this implementation for the spawning of debris
    public void HandleMovement()
    {
        InvokeRepeating("SpawnDebris", 1, 1f);
    }

    private void SpawnDebris()
    {
        for(int i = 0; i < 3; i++)
        {
            int randomPoint = UnityEngine.Random.Range(0, DebrisSpawnPoints.Count);
            Instantiate(DebrisPrefab, DebrisSpawnPoints[randomPoint].transform.position, Quaternion.identity);
        }
    }

    public void HandleSuccess()
    {
        LeftBtn.gameObject.SetActive(false);
        RightBtn.gameObject.SetActive(false);
        Player.SetActive(false);
        StartCoroutine(SuccessCoroutine());
    }

    private IEnumerator SuccessCoroutine()
    {
        GameManager.Instance.IncreaseProgress(15);
        yield return new WaitForSeconds(4);
        GameManager.Instance.ProgressContainer.SetActive(false);
        GameManager.Instance.CurrentEarthquakeDialogue = GameManager.Instance.CurrentEarthquakeDialogue.ReturningPointDialogue;
        GameManager.Instance.SceneController.CurrentScene = "SchoolScene";
    }

    public void HandleFailure()
    {
        LeftBtn.gameObject.SetActive(false);
        RightBtn.gameObject.SetActive(false);
        Player.SetActive(false);
        RetryBtn.gameObject.SetActive(true);
    }

    public void RetryGame()
    {
        GameManager.Instance.SceneController.CurrentScene = "StairsGameScene";
    }
}
