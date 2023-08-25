using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WorldCore : MonoBehaviour
{
    //=================================================================================================
    [Header("SETTINGS VARIABLES")]
    [SerializeField] private GameObject SettingsPanel;

    [Header("QUESET VARIABLES")]
    #region TYPHOON VARIABLES
    [SerializeField] private GameObject TyphoonQuestsContainer;
    [SerializeField] private QuestData DrugstoreQuest;
    [SerializeField] private Toggle DrugstoreToggle;
    [SerializeField] private QuestData MarketQuest;
    [SerializeField] private Toggle MarketToggle;
    [SerializeField] private QuestData HardwareStoreQuest;
    [SerializeField] private Toggle HardwareStoreToggle;
    #endregion
    [SerializeField] private OptionHandler MainQuestContainer;
    [SerializeField][ReadOnly] private bool showingQuest;

    [Header("ZONE VARIABLES")]
    [SerializeField] private ZoneHandler DrugstoreZone;
    [SerializeField] private ZoneHandler MarketZone;
    [SerializeField] private ZoneHandler HardwareStoreZone;
    [SerializeField] private ZoneHandler HouseZone;
    [SerializeField] private ZoneHandler SchoolZone;
    [SerializeField] private DialogueData StartingSchoolDialogue;
    [SerializeField] private GameObject ZonePopUp;
    [SerializeField] private TextMeshProUGUI ZoneTMP;
    [ReadOnly] public ZoneHandler EnteredZone;
    bool zoneEntered;

    [Header("PLAYER VARIABLES")]
    [SerializeField] private GameObject PlayerCharacter;

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

    public bool IsSettingsOpen()
    {
        return SettingsPanel.activeInHierarchy;
    }

    public void ReturnToMainMenu()
    {
        MarketQuest.IsAccomplised = false;
        HardwareStoreQuest.IsAccomplised = false;
        DrugstoreQuest.IsAccomplised = false;
        GameManager.Instance.SceneController.CurrentScene = "MainMenuScene";
    }
    #endregion

    #region QUESTS
    public void InitializeQuests()
    {
        if(GameManager.Instance.CurrentCalamity == GameManager.Calamity.TYPHOON)
        {
            MainQuestContainer.SetOptionText("Go back to your house.");
            if (AllTyphoonQuestsDone())
            {
                TyphoonQuestsContainer.SetActive(false);
                MainQuestContainer.gameObject.SetActive(true);
            }
            else
            {
                TyphoonQuestsContainer.SetActive(true);
                MainQuestContainer.gameObject.SetActive(false);
                DrugstoreToggle.isOn = DrugstoreQuest.IsAccomplised;
                MarketToggle.isOn = MarketQuest.IsAccomplised;
                HardwareStoreToggle.isOn = HardwareStoreQuest.IsAccomplised;
            }
        }
        else if (GameManager.Instance.CurrentCalamity == GameManager.Calamity.EARTHQUAKE)
        {
            TyphoonQuestsContainer.SetActive(false);
            MainQuestContainer.gameObject.SetActive(true);
            MainQuestContainer.SetOptionText("Go to the school.");
        }
        showingQuest = true;
    }

    private bool AllTyphoonQuestsDone()
    {
        return DrugstoreQuest.IsAccomplised && MarketQuest.IsAccomplised && HardwareStoreQuest.IsAccomplised;
    }

    public void ToggleQuestPanel()
    {
        //  The quest is currently being displayed and we want to hide it
        if(showingQuest)
        {
            if (GameManager.Instance.CurrentCalamity == GameManager.Calamity.TYPHOON)
            {
                if(AllTyphoonQuestsDone())
                {
                    TyphoonQuestsContainer.SetActive(false);
                    MainQuestContainer.gameObject.SetActive(false);
                }
                else
                {
                    TyphoonQuestsContainer.SetActive(false);
                    MainQuestContainer.gameObject.SetActive(false);
                }
            }
            else if (GameManager.Instance.CurrentCalamity == GameManager.Calamity.EARTHQUAKE)
                MainQuestContainer.gameObject.SetActive(false);

        }
        //  The quest is currently being hidden and we want to display it
        else
        {
            if (GameManager.Instance.CurrentCalamity == GameManager.Calamity.TYPHOON)
            {
                if (AllTyphoonQuestsDone())
                {
                    TyphoonQuestsContainer.SetActive(false);
                    MainQuestContainer.gameObject.SetActive(true);
                }
                else
                {
                    TyphoonQuestsContainer.SetActive(true);
                    MainQuestContainer.gameObject.SetActive(false);
                }
            }
            else if (GameManager.Instance.CurrentCalamity == GameManager.Calamity.EARTHQUAKE)
                MainQuestContainer.gameObject.SetActive(true);
        }
        showingQuest = !showingQuest;
    }
    #endregion

    #region ZONE
    public void SetProperZones()
    {
        if(GameManager.Instance.CurrentCalamity == GameManager.Calamity.TYPHOON)
        {
            if (DrugstoreQuest.IsAccomplised)
                DrugstoreZone.gameObject.SetActive(false);
            else
                DrugstoreZone.gameObject.SetActive(true);
            
            if(MarketQuest.IsAccomplised)
                MarketZone.gameObject.SetActive(false);
            else
                MarketZone.gameObject.SetActive(true);

            if (HardwareStoreQuest.IsAccomplised)
                HardwareStoreZone.gameObject.SetActive(false);
            else
                HardwareStoreZone.gameObject.SetActive(true);

            if (DrugstoreQuest.IsAccomplised && MarketQuest.IsAccomplised && HardwareStoreQuest.IsAccomplised)
                HouseZone.gameObject.SetActive(true);
            else
                HouseZone.gameObject.SetActive(false);

            SchoolZone.gameObject.SetActive(false);
        }
        else if (GameManager.Instance.CurrentCalamity == GameManager.Calamity.EARTHQUAKE)
        {
            DrugstoreZone.gameObject.SetActive(false);
            MarketZone.gameObject.SetActive(false);
            HardwareStoreZone.gameObject.SetActive(false);
            HouseZone.gameObject.SetActive(false);
            SchoolZone.gameObject.SetActive(true);
        }
    }

    public void DisplayZonePopUp()
    {
        ZonePopUp.SetActive(true);
        ZoneTMP.text = "Enter the " + EnteredZone.QuestData.ZoneName + "?";
    }

    public void HideZonePopUp()
    {
        ZonePopUp.SetActive(false);
    }

    public void EnterZone()
    {
        if (zoneEntered) return;
        zoneEntered = true;
        GameManager.Instance.CurrentQuest = EnteredZone.QuestData;
        if (EnteredZone.QuestData.SceneName == "SchoolScene")
            GameManager.Instance.CurrentEarthquakeDialogue = StartingSchoolDialogue;
        GameManager.Instance.SceneController.CurrentScene = EnteredZone.QuestData.SceneName;
    }
    #endregion

    #region PLAYER
    public void SetPlayerSpawnPosition()
    {
        PlayerCharacter.GetComponent<CharacterController>().enabled = false;
        if (GameManager.Instance.SceneController.LastScene == "DrugstoreScene")
            PlayerCharacter.transform.position = new Vector3(DrugstoreZone.transform.position.x, PlayerCharacter.transform.position.y, DrugstoreZone.transform.position.z);
        else if (GameManager.Instance.SceneController.LastScene == "MarketScene")
            PlayerCharacter.transform.position = new Vector3(MarketZone.transform.position.x, PlayerCharacter.transform.position.y, MarketZone.transform.position.z);
        else if (GameManager.Instance.SceneController.LastScene == "HardwareStoreScene")
            PlayerCharacter.transform.position = new Vector3(HardwareStoreZone.transform.position.x, PlayerCharacter.transform.position.y, HardwareStoreZone.transform.position.z);
        else if (GameManager.Instance.SceneController.LastScene == "SchoolScene")
            PlayerCharacter.transform.position = new Vector3(SchoolZone.transform.position.x, PlayerCharacter.transform.position.y, SchoolZone.transform.position.z);
        
        PlayerCharacter.GetComponent<CharacterController>().enabled = true;
    }
    #endregion
}
