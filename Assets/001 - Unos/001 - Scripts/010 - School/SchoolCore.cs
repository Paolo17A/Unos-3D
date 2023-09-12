using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SchoolCore : MonoBehaviour, I_Dialogue
{
    #region VARIABLES
    //======================================================================================================================
    [Header("VISUAL VARIABLES")]
    [SerializeField] private SpriteRenderer BackgroundSprite;
    [SerializeField] private SpriteRenderer PlayerCharacterSprite;
    [SerializeField] private SpriteRenderer ExtraSprite;

    [Header("DIALOGUE VARIABLES")]
    [SerializeField] private GameObject SchoolDialogueContainer;
    [SerializeField] private TextMeshProUGUI SchoolDialogueTMP;
    [SerializeField][ReadOnly][TextArea(minLines: 3, maxLines: 5)] private string ModifiedDialogue;
    [SerializeField] private GameObject Proceed;
    [ReadOnly] public bool DialogueFinished;

    [Header("CHOICE VARIABLES")]
    [SerializeField] private List<GameObject> Options;
    [SerializeField][ReadOnly] private int SelectedChoiceIndex;

    [Header("ENDING VARIABLES")]
    [SerializeField] private GameObject TriviaPanel;
    [SerializeField] private TextMeshProUGUI TriviaPanelTMP;
    [SerializeField] private GameObject GameOverHeader;

    Coroutine currentCoroutine;
    bool alreadySelected;
    //======================================================================================================================
    #endregion

    public void PlayStartingDialogue()
    {
        GameManager.Instance.AudioManager.KillBackgroundMusic();
        
        currentCoroutine = StartCoroutine(PlayDialogueText());
    }

    public IEnumerator PlayDialogueText()
    {
        DialogueFinished = false;
        if (GameManager.Instance.CurrentEarthquakeDialogue.ExtraAudio != null)
        {
            GameManager.Instance.AudioManager.SetBackgroundMusic(GameManager.Instance.CurrentEarthquakeDialogue.ExtraAudio);
        }

        #region VISUALS AND UI INITIALIZATION
        //  Hide UI Panels as soon as this coroutine starts. We can activate what we need later
        foreach (GameObject option in Options)
            option.SetActive(false);
        Proceed.SetActive(false);
        GameOverHeader.SetActive(false);
        TriviaPanel.SetActive(false);

        //  Set background visuals as needed
        if (GameManager.Instance.PlayerGender == GameManager.Gender.MALE)
        {
            if (GameManager.Instance.CurrentEarthquakeDialogue.AddedMale != null)
            {
                PlayerCharacterSprite.gameObject.SetActive(true);
                PlayerCharacterSprite.sprite = GameManager.Instance.CurrentEarthquakeDialogue.AddedMale;
            }
            else
                PlayerCharacterSprite.gameObject.SetActive(false);
            BackgroundSprite.sprite = GameManager.Instance.CurrentEarthquakeDialogue.DialogueBackgroundMale;
        }
        else if (GameManager.Instance.PlayerGender == GameManager.Gender.FEMALE)
        {
            if (GameManager.Instance.CurrentEarthquakeDialogue.AddedFemale != null)
            {
                PlayerCharacterSprite.gameObject.SetActive(true);
                PlayerCharacterSprite.sprite = GameManager.Instance.CurrentEarthquakeDialogue.AddedFemale;
            }
            else
                PlayerCharacterSprite.gameObject.SetActive(false);
            BackgroundSprite.sprite = GameManager.Instance.CurrentEarthquakeDialogue.DialogueBackgroundFemale;
        }
        if (GameManager.Instance.CurrentEarthquakeDialogue.AddedLocation != Vector3.zero)
            PlayerCharacterSprite.gameObject.transform.position = GameManager.Instance.CurrentEarthquakeDialogue.AddedLocation;

        //  Display any additional sprites needed here
        if (GameManager.Instance.CurrentEarthquakeDialogue.ThirdPartySprite != null)
        {
            ExtraSprite.gameObject.SetActive(true);
            ExtraSprite.gameObject.transform.position = GameManager.Instance.CurrentEarthquakeDialogue.ThirdPartyLocation;
            ExtraSprite.sprite = GameManager.Instance.CurrentEarthquakeDialogue.ThirdPartySprite;
        }
        else
            ExtraSprite.gameObject.SetActive(false);
        #endregion

        //  Modify the dialogue to include what's needed based on username and gender
        ModifiedDialogue = GameManager.Instance.CurrentEarthquakeDialogue.DialogueContent;
        ModifiedDialogue = ReplaceSubstring(ModifiedDialogue, "PLAYER", GameManager.Instance.Username);
        ModifiedDialogue = ReplaceSubstring(ModifiedDialogue, "POS", GameManager.Instance.PlayerGender == GameManager.Gender.MALE ? "his" : "her");
        ModifiedDialogue = ReplaceSubstring(ModifiedDialogue, "OBJ", GameManager.Instance.PlayerGender == GameManager.Gender.MALE ? "him" : "her");

        //  Display the dialogue text over time
        SchoolDialogueContainer.SetActive(true);
        SchoolDialogueTMP.text = "";
        foreach (char c in ModifiedDialogue)
        {
            SchoolDialogueTMP.text += c;
            yield return new WaitForSeconds(0.025f);
        }

        //  Process what needs to be done after the dialogue is done being displayed
        if (GameManager.Instance.CurrentEarthquakeDialogue.ThisDialogueType == DialogueData.DialogueType.TALKING)
            Proceed.SetActive(true);
        else if (GameManager.Instance.CurrentEarthquakeDialogue.ThisDialogueType == DialogueData.DialogueType.CHOOSING)
        {
            for (int i = 0; i < Options.Count; i++)
                Options[i].GetComponent<OptionHandler>().SetOptionText(GameManager.Instance.CurrentEarthquakeDialogue.OptionContent[i]);
        }
        else if (GameManager.Instance.CurrentEarthquakeDialogue.ThisDialogueType == DialogueData.DialogueType.ENDING_SUCCESS || GameManager.Instance.CurrentEarthquakeDialogue.ThisDialogueType == DialogueData.DialogueType.ENDING_FAIL)
        {
            yield return new WaitForSeconds(0.5f);
            TriviaPanel.SetActive(true);
            TriviaPanelTMP.text = GameManager.Instance.CurrentEarthquakeDialogue.EndingTrivia;
            Proceed.SetActive(true);

            if (GameManager.Instance.CurrentEarthquakeDialogue.ThisDialogueType == DialogueData.DialogueType.ENDING_SUCCESS)
                GameManager.Instance.FinishedCalamities.Add(GameManager.Calamity.EARTHQUAKE);
        }
        DialogueFinished = true;
    }

    public void HandleMinigame()
    {
        throw new System.NotImplementedException();
    }

    //  We look at the current dialogue and base out next action on the next dialogue
    public void LoadNextDialogue()
    {
        StopCoroutine(currentCoroutine);
        switch (GameManager.Instance.CurrentEarthquakeDialogue.ThisDialogueType)
        {
            case DialogueData.DialogueType.TALKING:
                GameManager.Instance.CurrentEarthquakeDialogue = GameManager.Instance.CurrentEarthquakeDialogue.NextDialogue;
                if(GameManager.Instance.CurrentEarthquakeDialogue.ThisDialogueType == DialogueData.DialogueType.MINIGAME)
                {
                    GameManager.Instance.ProgressContainer.SetActive(false);
                    GameManager.Instance.SceneController.CurrentScene = GameManager.Instance.CurrentEarthquakeDialogue.MiniGameScene;
                }
                else
                    currentCoroutine = StartCoroutine(PlayDialogueText());
                break;
            case DialogueData.DialogueType.CHOOSING:
                GameManager.Instance.CurrentEarthquakeDialogue = GameManager.Instance.CurrentEarthquakeDialogue.OptionDialogues[SelectedChoiceIndex];
                SelectedChoiceIndex = 0;
                if (GameManager.Instance.CurrentEarthquakeDialogue.ThisDialogueType == DialogueData.DialogueType.QUIT)
                {
                    GameManager.Instance.ProgressContainer.SetActive(false);
                    GameManager.Instance.SceneController.CurrentScene = "WorldScene";
                }
                else if (GameManager.Instance.CurrentEarthquakeDialogue.ThisDialogueType == DialogueData.DialogueType.MINIGAME)
                {
                    GameManager.Instance.ProgressContainer.SetActive(false);
                    GameManager.Instance.SceneController.CurrentScene = GameManager.Instance.CurrentEarthquakeDialogue.MiniGameScene;
                }
                else
                    currentCoroutine = StartCoroutine(PlayDialogueText());
                break;
            case DialogueData.DialogueType.ENDING_FAIL:
                GameManager.Instance.CurrentEarthquakeDialogue = GameManager.Instance.CurrentEarthquakeDialogue.SavePointDialogue;
                currentCoroutine = StartCoroutine(PlayDialogueText());
                break;
            case DialogueData.DialogueType.ENDING_SUCCESS:
                LoadSuccessScene();
                break;
        }
    }

    private void LoadSuccessScene()
    {
        if (alreadySelected) return;
        alreadySelected = true;

        GameManager.Instance.CurrentCalamity = GameManager.Calamity.NONE;
        GameManager.Instance.ProgressContainer.SetActive(false);
        GameManager.Instance.AudioManager.KillBackgroundMusic();
        if (GameManager.Instance.FinishedCalamities.Count == 2)
        {
            GameManager.Instance.FinishedCalamities.Clear();
            GameManager.Instance.ResetProgress();
            GameManager.Instance.SceneController.CurrentScene = "MainMenuScene";
        }
        else
            GameManager.Instance.SceneController.CurrentScene = "LobbyScene";

        foreach (QuestData quest in GameManager.Instance.TyphoonQuests)
            quest.IsAccomplised = false;
    }

    public void MakeChoice(int choice)
    {
        SelectedChoiceIndex = choice;
        StartCoroutine(GameManager.Instance.APIClient.MakeDisasterChoice(GameManager.Instance.CurrentEarthquakeDialogue.ScenarioIndex, GameManager.Instance.PlayerGender == GameManager.Gender.MALE ? "male" : "female", SelectedChoiceIndex == 0 ? "a" : "b"));
        if(GameManager.Instance.CurrentEarthquakeDialogue.ScenarioIndex == "3" && SelectedChoiceIndex == 0)     
            GameManager.Instance.IncreaseProgress(5);
        
        foreach (GameObject option in Options)
            option.SetActive(false);
        LoadNextDialogue();
    }


    #region UTILITY
    private string ReplaceSubstring(string original, string substringToReplace, string replacement)
    {
        int index = original.IndexOf(substringToReplace);
        if (index != -1)
        {
            string modified = original.Substring(0, index) + replacement + original.Substring(index + substringToReplace.Length);
            return modified;
        }
        else
        {
            return original;
        }
    }
    #endregion
}
