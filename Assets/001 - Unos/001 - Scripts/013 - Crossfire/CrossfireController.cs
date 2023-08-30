using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossfireController : MonoBehaviour
{
    [SerializeField] private CrossfireCore CrossfireCore;
    [SerializeField] private AudioClip Ouch;
    [SerializeField] private AudioClip Yay;
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
                GameManager.Instance.AudioManager.PlayAudioClip(Ouch);
                CrossfireCore.HandleFailure();
                break;
            case CrossfireCore.CrossfireGameStates.SAFE:
                GameManager.Instance.AudioManager.PlayAudioClip(Yay);
                CrossfireCore.HandleSuccess();
                break;
        }
    }
}
