using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HouseCore : MonoBehaviour, I_Dialogue
{
    #region VARIABLES
    //======================================================================================================================
    [Header("VISUAL VARIABLES")]
    [SerializeField] private SpriteRenderer BackgroundSprite;
    [SerializeField] private SpriteRenderer ThirdPartySprite;
    [SerializeField] private SpriteRenderer PlayerCharacterSprite;
    [SerializeField] private GameObject ExtraSprite;
    [SerializeField] private GameObject Boat;

    [Header("DIALOGUE VARIABLES")]
    [SerializeField] private DialogueData StartingDialogue;
    [ReadOnly] public DialogueData CurrentDialogue;
    [SerializeField] private GameObject HouseDialogueContainer;
    [SerializeField] private TextMeshProUGUI HouseDialogueTMP;
    [SerializeField][ReadOnly][TextArea(minLines: 3, maxLines: 5)] private string ModifiedDialogue;
    [SerializeField] private GameObject Proceed;
    [ReadOnly] public bool DialogueFinished;

    [Header("CHOICE VARIABLES")]
    [SerializeField] private List<GameObject> Options;
    [SerializeField][ReadOnly] private int SelectedChoiceIndex;

    [Header("MINI GAME VARIABLES")]
    [SerializeField] private List<GameObject> SelectableObjects;
    [SerializeField][ReadOnly] private int SelectedItemsCount;

    [Header("ENDING FAIL VARIABLES")]
    [SerializeField] private GameObject TriviaPanel;
    [SerializeField] private TextMeshProUGUI TriviaPanelTMP;
    [SerializeField] private GameObject GameOverHeader;

    [Header("BACKGROUND MUSIC")]
    [SerializeField] private AudioClip Rain;

    Coroutine currentCoroutine;
    bool alreadySelected;
    //======================================================================================================================
    #endregion

    #region DIALOGUE
    public void PlayStartingDialogue()
    {
        GameManager.Instance.AudioManager.KillBackgroundMusic();
        GameManager.Instance.AudioManager.SetBackgroundMusic(Rain);
        CurrentDialogue = StartingDialogue;
        if (GameManager.Instance.DebugMode)
            GameManager.Instance.PlayerGender = GameManager.Gender.FEMALE;
        if (GameManager.Instance.PlayerGender == GameManager.Gender.MALE)
        {
            PlayerCharacterSprite.sprite = CurrentDialogue.AddedMale;
            BackgroundSprite.sprite = CurrentDialogue.DialogueBackgroundMale;
        }
        else if (GameManager.Instance.PlayerGender == GameManager.Gender.FEMALE)
        {
            PlayerCharacterSprite.sprite = CurrentDialogue.AddedFemale;
            BackgroundSprite.sprite = CurrentDialogue.DialogueBackgroundFemale;
        }
        PlayerCharacterSprite.gameObject.transform.position = CurrentDialogue.AddedLocation;
        ThirdPartySprite.gameObject.SetActive(false);
        GameOverHeader.SetActive(false);
        foreach (GameObject option in Options)
            option.SetActive(false);
        foreach (GameObject selectable in SelectableObjects)
            selectable.SetActive(false);
        currentCoroutine = StartCoroutine(PlayDialogueText());
    }

    public IEnumerator PlayDialogueText()
    {
        DialogueFinished = false;

        //  Activate and deactivate some visuals as needed
        if (CurrentDialogue.HasExtraSprite)
            ExtraSprite.SetActive(true);
        else
            ExtraSprite.SetActive(false);
        if (CurrentDialogue.WillDisplayBoat)
            Boat.SetActive(true);
        else
            Boat.SetActive(false);
        if(CurrentDialogue.MustShowSelectables)
            foreach (GameObject selectable in SelectableObjects)
                selectable.SetActive(true);
        if (CurrentDialogue.ThirdPartySprite != null)
        {
            ThirdPartySprite.sprite = CurrentDialogue.ThirdPartySprite;
            ThirdPartySprite.gameObject.SetActive(true);
        }
        else
            ThirdPartySprite.gameObject.SetActive(false);
        if (CurrentDialogue.ThisDialogueType == DialogueData.DialogueType.ENDING_SUCCESS || CurrentDialogue.ThisDialogueType == DialogueData.DialogueType.ENDING_FAIL)
            GameOverHeader.SetActive(true);
        else
            GameOverHeader.SetActive(false);
        Proceed.SetActive(false);
        TriviaPanel.SetActive(false);

        //  Modify the dialogue to include what's needed based on username and gender
        ModifiedDialogue = CurrentDialogue.DialogueContent;
        ModifiedDialogue = ReplaceSubstring(ModifiedDialogue, "PLAYER", GameManager.Instance.Username);
        ModifiedDialogue = ReplaceSubstring(ModifiedDialogue, "POS", GameManager.Instance.PlayerGender == GameManager.Gender.MALE ? "his" : "her");
        ModifiedDialogue = ReplaceSubstring(ModifiedDialogue, "OBJ", GameManager.Instance.PlayerGender == GameManager.Gender.MALE ? "him" : "her");

        //  Display the dialogue text over time
        HouseDialogueContainer.SetActive(true);
        HouseDialogueTMP.text = "";
        foreach (char c in ModifiedDialogue)
        {
            HouseDialogueTMP.text += c;
            yield return new WaitForSeconds(0.025f);
        }

        //  Activate and deactivate some visuals as needed
        if (CurrentDialogue.ThisDialogueType == DialogueData.DialogueType.TALKING)
            Proceed.SetActive(true);
        else if (CurrentDialogue.ThisDialogueType == DialogueData.DialogueType.CHOOSING)
        {
            for(int i = 0; i < Options.Count; i++)
            {
                Options[i].GetComponent<OptionHandler>().SetOptionText(CurrentDialogue.OptionContent[i]);
            }
        }
        else if (CurrentDialogue.ThisDialogueType == DialogueData.DialogueType.ENDING_SUCCESS || CurrentDialogue.ThisDialogueType == DialogueData.DialogueType.ENDING_FAIL)
        {
            yield return new WaitForSeconds(0.025f);
            TriviaPanel.SetActive(true);
            TriviaPanelTMP.text = CurrentDialogue.EndingTrivia;
            Proceed.SetActive(true);

            if (CurrentDialogue.ThisDialogueType == DialogueData.DialogueType.ENDING_SUCCESS)
                GameManager.Instance.FinishedCalamities.Add(GameManager.Calamity.TYPHOON);
        }

        DialogueFinished = true;
    }

    public void DetectItem(Vector3 inputPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(inputPosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider == null) return;

        if(hit.collider.tag == "selectable")
        {
            hit.collider.gameObject.SetActive(false);
            HandleMinigame();
        }
    }

    public void HandleMinigame()
    {
        SelectedItemsCount++;
        if (SelectedItemsCount == SelectableObjects.Count)
            LoadNextDialogue();
    }

    public void LoadNextDialogue()
    {
        StopCoroutine(currentCoroutine);
        switch (CurrentDialogue.ThisDialogueType)
        {
            case DialogueData.DialogueType.TALKING:
                CurrentDialogue = CurrentDialogue.NextDialogue;
                currentCoroutine = StartCoroutine(PlayDialogueText());
                break;
            case DialogueData.DialogueType.CHOOSING:
                CurrentDialogue = CurrentDialogue.OptionDialogues[SelectedChoiceIndex];
                SelectedChoiceIndex = 0;
                currentCoroutine = StartCoroutine(PlayDialogueText());
                break;
            case DialogueData.DialogueType.ENDING_SUCCESS:
                LoadSuccessScene();
                break;
            case DialogueData.DialogueType.ENDING_FAIL:
                CurrentDialogue = CurrentDialogue.SavePointDialogue;
                currentCoroutine = StartCoroutine(PlayDialogueText());
                break;
            case DialogueData.DialogueType.MINIGAME:
                CurrentDialogue = CurrentDialogue.ReturningPointDialogue;
                currentCoroutine = StartCoroutine(PlayDialogueText());
                break;
        }

        //  Set background visuals as needed
        if (GameManager.Instance.PlayerGender == GameManager.Gender.MALE)
        {
            if (CurrentDialogue.AddedMale != null)
            {
                PlayerCharacterSprite.gameObject.SetActive(true);
                PlayerCharacterSprite.sprite = CurrentDialogue.AddedMale;
            }
            else
                PlayerCharacterSprite.gameObject.SetActive(false);
            BackgroundSprite.sprite = CurrentDialogue.DialogueBackgroundMale;
        }
        else if (GameManager.Instance.PlayerGender == GameManager.Gender.FEMALE)
        {
            if (CurrentDialogue.AddedFemale != null)
            {
                PlayerCharacterSprite.gameObject.SetActive(true);
                PlayerCharacterSprite.sprite = CurrentDialogue.AddedFemale;
            }
            else
                PlayerCharacterSprite.gameObject.SetActive(false);
            BackgroundSprite.sprite = CurrentDialogue.DialogueBackgroundFemale;
        }
        if(CurrentDialogue.AddedLocation != Vector3.zero)
            PlayerCharacterSprite.gameObject.transform.position = CurrentDialogue.AddedLocation;
    }

    public void MakeChoice(int choice)
    {
        SelectedChoiceIndex = choice;
        StartCoroutine(GameManager.Instance.APIClient.MakeDisasterChoice(CurrentDialogue.ScenarioIndex, GameManager.Instance.PlayerGender == GameManager.Gender.MALE ? "male" : "female", SelectedChoiceIndex == 0 ? "a" : "b"));
        foreach (GameObject option in Options)
            option.SetActive(false);
        LoadNextDialogue();
    }


    

    private void LoadSuccessScene()
    {
        if (alreadySelected) return;

        alreadySelected = true;
        GameManager.Instance.AudioManager.KillBackgroundMusic();
        GameManager.Instance.CurrentCalamity = GameManager.Calamity.NONE;
        if (GameManager.Instance.FinishedCalamities.Count == 2)
        {
            GameManager.Instance.FinishedCalamities.Clear();
            GameManager.Instance.SceneController.CurrentScene = "MainMenuScene";
        }
        else
            GameManager.Instance.SceneController.CurrentScene = CurrentDialogue.NextScene;
        foreach (QuestData quest in GameManager.Instance.TyphoonQuests)
            quest.IsAccomplised = false;
    }
    #endregion

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
