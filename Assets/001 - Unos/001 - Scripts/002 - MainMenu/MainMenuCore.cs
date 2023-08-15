using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MainMenuCore : MonoBehaviour
{
    #region STATE MACHINE
    //================================================================================================================
    public enum MainMenuStates
    {
        NONE,
        MAINMENU,
        ABOUT,
        SETTINGS,
        QUIT,
    }

    private event EventHandler mainMenuStateChange;
    public event EventHandler onMainMenuStateChange
    {
        add
        {
            if (mainMenuStateChange == null || !mainMenuStateChange.GetInvocationList().Contains(value))
                mainMenuStateChange += value;
        }
        remove { mainMenuStateChange -= value; }
    }

    public MainMenuStates CurrentMainMenuState
    {
        get => mainMenuStates;
        set
        {
            mainMenuStates = value;
            mainMenuStateChange?.Invoke(this, EventArgs.Empty);
        }
    }
    [SerializeField][ReadOnly] private MainMenuStates mainMenuStates;
    //=============================================================================================================
    #endregion

    #region VARIABLES
    //=============================================================================================================
    [Header("PANELS")]
    [SerializeField] private RectTransform MainMenuRT;
    [SerializeField] private CanvasGroup MainMenuCG;
    [SerializeField] private RectTransform AboutRT;
    [SerializeField] private CanvasGroup AboutCG;
    [SerializeField] private RectTransform SettingsRT;
    [SerializeField] private CanvasGroup SettingsCG;
    [SerializeField] private RectTransform QuitRT;
    [SerializeField] private CanvasGroup QuitCG;
    //=============================================================================================================
    #endregion

    #region PANELS
    public void ShowMainMenuPanel()
    {
        GameManager.Instance.AnimationsLT.FadePanel(MainMenuRT, null, MainMenuCG, 0, 1, () => { });
    }

    public void HideMainMenuPanel()
    {
        GameManager.Instance.AnimationsLT.FadePanel(MainMenuRT, MainMenuRT, MainMenuCG, 1, 0, () => { });
    }

    public void ShowAboutPanel()
    {
        GameManager.Instance.AnimationsLT.FadePanel(AboutRT, null, AboutCG, 0, 1, () => { });
    }

    public void HideAboutPanel()
    {
        GameManager.Instance.AnimationsLT.FadePanel(AboutRT, AboutRT, AboutCG, 1, 0, () => { });
    }

    public void ShowSettingsPanel()
    {
        GameManager.Instance.AnimationsLT.FadePanel(SettingsRT, null, SettingsCG, 0, 1, () => { });
    }

    public void HideSettingsPanel()
    {
        GameManager.Instance.AnimationsLT.FadePanel(SettingsRT, SettingsRT, SettingsCG, 1, 0, () => { });
    }

    public void ShowQuitPanel()
    {
        GameManager.Instance.AnimationsLT.FadePanel(QuitRT, null, QuitCG, 0, 1, () => { });
    }

    public void HideQuitPanel()
    {
        GameManager.Instance.AnimationsLT.FadePanel(QuitRT, QuitRT, QuitCG, 1, 0, () => { });
    }

    public void MainMenuStateToIndex(int index)
    {
        switch (index)
        {
            case (int)MainMenuStates.MAINMENU:
                CurrentMainMenuState = MainMenuStates.MAINMENU;
                break;
            case (int)MainMenuStates.ABOUT:
                CurrentMainMenuState = MainMenuStates.ABOUT;
                break;
            case (int)MainMenuStates.SETTINGS:
                CurrentMainMenuState = MainMenuStates.SETTINGS;
                break;
            case (int)MainMenuStates.QUIT:
                CurrentMainMenuState = MainMenuStates.QUIT;
                break;
        }
    }
    #endregion

    public void GoToGenderSelectScene()
    {
        GameManager.Instance.SceneController.CurrentScene = "GenderSelectScene";
    }
}
