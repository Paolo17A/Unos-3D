using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsGameController : MonoBehaviour
{
    [SerializeField] private StairsGameCore StairsGameCore;

    private void Awake()
    {
        StairsGameCore.onStairsGameStateChange += StairsGameStateChange;
        GameManager.Instance.SceneController.ActionPass = true;
    }

    private void OnDisable()
    {
        StairsGameCore.onStairsGameStateChange -= StairsGameStateChange;
    }

    private void Start()
    {
        StairsGameCore.CurrentStairsGameState = StairsGameCore.StairsGameStates.INSTRUCTION;
    }

    private void StairsGameStateChange(object sender, EventArgs e)
    {
        switch(StairsGameCore.CurrentStairsGameState)
        {
            case StairsGameCore.StairsGameStates.INSTRUCTION:
                StairsGameCore.InitializeScene();
                break;
            case StairsGameCore.StairsGameStates.GAMEPLAY:
                StairsGameCore.HandleMovement();
                break;
            case StairsGameCore.StairsGameStates.STONED:
                StairsGameCore.HandleFailure();
                break;
            case StairsGameCore.StairsGameStates.SAFE:
                StairsGameCore.HandleSuccess();
                break;
        }
    }
}
