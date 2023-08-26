using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TapGameCore : MonoBehaviour, I_MiniGame
{
    #region STATE MACHINE
    //======================================================================================================================
    public enum TapGameStates
    {
        NONE,
        INSTRUCTION,
        GAMEPLAY,
        TAPPED
    }

    private event EventHandler tapGameStateChange;
    public event EventHandler onTapGameStateChange
    {
        add 
        { 
            if(tapGameStateChange == null || !tapGameStateChange.GetInvocationList().Contains(value))
            tapGameStateChange += value; 
        }
        remove { tapGameStateChange -= value; }
    }

    public TapGameStates CurrentTapGameState
    {
        get => tapGameState;
        set
        {
            tapGameState = value;
            tapGameStateChange?.Invoke(this, EventArgs.Empty);
        }
    }

    [SerializeField] private TapGameStates tapGameState;
    //======================================================================================================================
    #endregion

    #region VARIABLES
    //======================================================================================================================
    [Header("PLAYER VARIABLES")]
    [SerializeField] private SpriteRenderer PlayerSprite;

    [Header("DIALOGUE VARIABLES")]
    [SerializeField] private GameObject DialogueContainer;
    [SerializeField] private TextMeshProUGUI DialogueTMP;
    [SerializeField] private GameObject StartGame;
    [ReadOnly] public bool DialogueFinished;

    [Header("TAP GAMEPLAY VARIABLES")]
    [SerializeField] private GameObject SliderContainer;
    [SerializeField] private GameObject MovingTarget;
    [SerializeField] private GameObject StationaryTarget;
    [SerializeField] private float MovementSpeed;
    [SerializeField] private Transform StartingPoint;
    [SerializeField] private Transform EndingPoint;
    [SerializeField][ReadOnly] private bool movingTowardsEnd = true;

    Coroutine currentCoroutine;
    bool alreadyPressed;
    //======================================================================================================================
    #endregion

    public void InitializeScene()
    {
        if (GameManager.Instance.PlayerGender == GameManager.Gender.MALE)
            PlayerSprite.sprite = GameManager.Instance.CurrentEarthquakeDialogue.AddedMale;
        else if (GameManager.Instance.PlayerGender == GameManager.Gender.FEMALE)
            PlayerSprite.sprite = GameManager.Instance.CurrentEarthquakeDialogue.AddedFemale;
        else
            PlayerSprite.gameObject.SetActive(false);

        DialogueContainer.SetActive(true);
        ToggleStartGame(false);
        SliderContainer.SetActive(false);

        currentCoroutine = StartCoroutine(DisplayInstructionText());
        
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

    public void InitializeGameplay()
    {
        StopCoroutine(currentCoroutine);
        DialogueContainer.SetActive(false);
        SliderContainer.SetActive(true);
        MovingTarget.transform.localPosition = StartingPoint.localPosition;
        CurrentTapGameState = TapGameStates.GAMEPLAY;
    }

    public void ToggleStartGame(bool isActive)
    {
        StartGame.SetActive(isActive);
    }

    public void HandleMovement()
    {
        //  Move towards the specified endpoint
        if (movingTowardsEnd)
        {
            MovingTarget.transform.localPosition = Vector3.MoveTowards(MovingTarget.transform.localPosition, EndingPoint.localPosition, MovementSpeed * Time.deltaTime);
            if (Mathf.Abs(Vector3.Distance(MovingTarget.transform.localPosition, EndingPoint.localPosition)) <= Mathf.Epsilon)
                movingTowardsEnd = false;
        }
        else
        {
            MovingTarget.transform.localPosition = Vector3.MoveTowards(MovingTarget.transform.localPosition, StartingPoint.localPosition, MovementSpeed * Time.deltaTime);
            if (Mathf.Abs(Vector3.Distance(MovingTarget.transform.localPosition, StartingPoint.localPosition)) <= Mathf.Epsilon)
                movingTowardsEnd = true;
        }
    }

    public void HandleOnTap()
    {
        if (alreadyPressed) return;

        alreadyPressed = true;

        // Calculate the distance between the centers of the colliders
        float distance = Vector2.Distance(MovingTarget.transform.localPosition, StationaryTarget.transform.localPosition);

        // Calculate the sum of the radii of the colliders
        float radiiSum = MovingTarget.GetComponent<CircleCollider2D>().radius + StationaryTarget.GetComponent<CircleCollider2D>().radius;

        // Calculate the overlap amount
        float overlapAmount = 1.0f - Mathf.Clamp01(distance / radiiSum);

        Debug.Log(overlapAmount);
        if (overlapAmount > 0.25f)
            HandleSuccess();
        else
            HandleFailure();
    }

    public void HandleSuccess()
    {
        GameManager.Instance.CurrentEarthquakeDialogue = GameManager.Instance.CurrentEarthquakeDialogue.ReturningPointDialogue;
        GameManager.Instance.SceneController.CurrentScene = "SchoolScene";
    }


    public void HandleFailure()
    {
        GameManager.Instance.CurrentEarthquakeDialogue = GameManager.Instance.CurrentEarthquakeDialogue.SavePointDialogue;
        GameManager.Instance.SceneController.CurrentScene = "SchoolScene";
    }
}
