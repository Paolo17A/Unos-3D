using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private MainMenuCore MainMenuCore;
    private void Awake()
    {
        MainMenuCore.onMainMenuStateChange += MainMenuStateChange;
        GameManager.Instance.SceneController.ActionPass = true;
    }

    private void OnDisable()
    {
        MainMenuCore.onMainMenuStateChange -= MainMenuStateChange;
    }

    private void Start()
    {
        MainMenuCore.CurrentMainMenuState = MainMenuCore.MainMenuStates.MAINMENU;
    }

    private void MainMenuStateChange(object sender, EventArgs e)
    {
        switch (MainMenuCore.CurrentMainMenuState)
        {
            case MainMenuCore.MainMenuStates.MAINMENU:
                MainMenuCore.ShowMainMenuPanel();
                break;
            case MainMenuCore.MainMenuStates.ABOUT:
                MainMenuCore.ShowAboutPanel();
                break;
            case MainMenuCore.MainMenuStates.SETTINGS:
                MainMenuCore.ShowSettingsPanel();
                break;
            case MainMenuCore.MainMenuStates.QUIT:
                MainMenuCore.ShowQuitPanel();
                break;
        }
    }

}
