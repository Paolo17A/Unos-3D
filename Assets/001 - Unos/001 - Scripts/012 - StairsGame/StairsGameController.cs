using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsGameController : MonoBehaviour
{
    [SerializeField] private StairsGameCore StairsGameCore;
    [SerializeField] private AudioClip Ouch;
    [SerializeField] private AudioClip Yay;
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
        gameObject.GetComponent<AudioSource>().volume = GameManager.Instance.AudioManager.GetBGMVolume();
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
                GameManager.Instance.AudioManager.PlayAudioClip(Ouch);
                StairsGameCore.HandleFailure();
                break;
            case StairsGameCore.StairsGameStates.SAFE:
                GameManager.Instance.AudioManager.PlayAudioClip(Yay);
                StairsGameCore.HandleSuccess();
                break;
        }
    }
}
