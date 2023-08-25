using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TapGameController : MonoBehaviour
{
    [SerializeField] private TapGameCore TapGameCore;

    private void Awake()
    {
        TapGameCore.onTapGameStateChange += TapGameStateChange;
        GameManager.Instance.SceneController.ActionPass = true;
    }

    private void OnDisable()
    {
        TapGameCore.onTapGameStateChange -= TapGameStateChange;
    }

    private void Start()
    {
        TapGameCore.CurrentTapGameState = TapGameCore.TapGameStates.INSTRUCTION;
    }

    private void TapGameStateChange(object sender, EventArgs e)
    {
        switch (TapGameCore.CurrentTapGameState)
        {
            case TapGameCore.TapGameStates.INSTRUCTION:
                TapGameCore.InitializeScene();
                break;
            case TapGameCore.TapGameStates.GAMEPLAY:
                TapGameCore.ToggleStartGame(false);
                break;
            case TapGameCore.TapGameStates.TAPPED:
                TapGameCore.HandleOnTap();
                break;
        }
    }

    private void Update()
    {
        if (TapGameCore.CurrentTapGameState != TapGameCore.TapGameStates.GAMEPLAY)
            return;

        TapGameCore.HandleMovement();

        if(Application.isEditor)
        {
            if (!Mouse.current.leftButton.isPressed)
                return;

            TapGameCore.CurrentTapGameState = TapGameCore.TapGameStates.TAPPED;
        }
        else
        {
            if (!Touchscreen.current.primaryTouch.press.isPressed)
                return;

            TapGameCore.CurrentTapGameState = TapGameCore.TapGameStates.TAPPED;
        }
    }
}
