using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossfireController : MonoBehaviour
{
    [SerializeField] private CrossfireCore CrossfireCore;

    private void Awake()
    {
        CrossfireCore.onCrossfireGameStateChange += CrossfireGameStateChange;
        GameManager.Instance.SceneController.ActionPass = true;
    }

    private void OnDisable()
    {
        CrossfireCore.onCrossfireGameStateChange -= CrossfireGameStateChange;
    }

    private void Start()
    {
        CrossfireCore.CurrentCrossFireGameState = CrossfireCore.CrossfireGameStates.INSTRUCTION;
    }

    private void CrossfireGameStateChange(object sender, EventArgs e)
    {
        switch (CrossfireCore.CurrentCrossFireGameState)
        {
            case CrossfireCore.CrossfireGameStates.INSTRUCTION:
                CrossfireCore.InitializeScene();
                break;
            case CrossfireCore.CrossfireGameStates.BURNED:
                CrossfireCore.HandleFailure();
                break;
            case CrossfireCore.CrossfireGameStates.SAFE:
                CrossfireCore.HandleSuccess();
                break;
        }
    }
}
