using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCore : MonoBehaviour
{
    //=================================================================================================
    [Header("SETTINGS VARIABLES")]
    [SerializeField] private GameObject SettingsPanel;
    //=================================================================================================

    #region SETTINGS
    public void DisplaySettingsPanel()
    {
        SettingsPanel.SetActive(true);
    }

    public void HideSettingsPanel()
    {
        SettingsPanel.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        GameManager.Instance.SceneController.CurrentScene = "MainMenuScene";
    }
    #endregion
}
