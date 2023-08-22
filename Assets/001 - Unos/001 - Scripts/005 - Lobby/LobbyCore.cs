using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCore : MonoBehaviour, I_Dialogue
{
    #region VARIABLES
    //================================================================================================================
    [Header("VISUAL VARIABLES")]
    [SerializeField] private SpriteRenderer BackgroundSprite;
    [SerializeField] private SpriteRenderer PlayerCharacterSprite;
    [SerializeField] private Sprite MaleSprite;
    [SerializeField] private Sprite FemaleSprite;

    [Header("DIALOGUE VARIABLES")]
    [SerializeField] private DialogueData StartingDialogue;
    [ReadOnly] public DialogueData CurrentDialogue;
    [SerializeField] private TextMeshProUGUI LobbyDialogueTMP;
    [SerializeField][ReadOnly][TextArea(minLines: 5, maxLines: 10)] private string ModifiedDialogue;
    [SerializeField] private GameObject Proceed;
    [ReadOnly] public bool DialogueFinished;

    [Header("CHOICE VARIABLES")]
    [SerializeField] private GameObject TyphoonChoice;
    [SerializeField] private GameObject EarthquakeChoice;
    [SerializeField][ReadOnly] private int SelectedChoiceIndex;

    [Header("SUCCESS VARIABLES")]
    [SerializeField] private Button StartBtn;

    Coroutine currentCoroutine;
    //================================================================================================================
    #endregion

    #region DIALOGUE

    public void PlayStartingDialogue()
    {
        if (GameManager.Instance.PlayerGender == GameManager.Gender.MALE)
            PlayerCharacterSprite.sprite = MaleSprite;
        else if (GameManager.Instance.PlayerGender == GameManager.Gender.FEMALE)
            PlayerCharacterSprite.sprite = FemaleSprite;
        TyphoonChoice.SetActive(false);
        EarthquakeChoice.SetActive(false);
        StartBtn.gameObject.SetActive(false);
        CurrentDialogue = StartingDialogue;
        currentCoroutine = StartCoroutine(PlayDialogueText());
    }

    public IEnumerator PlayDialogueText()
    {
        DialogueFinished = false;
        Proceed.SetActive(false);
        ModifiedDialogue = CurrentDialogue.DialogueContent;
        ModifiedDialogue = ReplaceSubstring(ModifiedDialogue, "PLAYER", GameManager.Instance.Username);
        ModifiedDialogue = ReplaceSubstring(ModifiedDialogue, "POS", GameManager.Instance.PlayerGender == GameManager.Gender.MALE ? "his" : "her");
        ModifiedDialogue = ReplaceSubstring(ModifiedDialogue, "OBJ", GameManager.Instance.PlayerGender == GameManager.Gender.MALE ? "him" : "her");

        LobbyDialogueTMP.text = "";
        foreach (char c in ModifiedDialogue)
        {
            LobbyDialogueTMP.text += c;
            yield return new WaitForSeconds(0.05f);
        }

        DialogueFinished = true;
        //  Display the proper interactable buttons based on the current dialogue type
        if (CurrentDialogue.ThisDialogueType == DialogueData.DialogueType.TALKING)
            Proceed.SetActive(true);
        else if (CurrentDialogue.ThisDialogueType == DialogueData.DialogueType.CHOOSING)
        {
            if (!GameManager.Instance.FinishedCalamities.Contains(GameManager.Calamity.TYPHOON))
                TyphoonChoice.SetActive(true);
            EarthquakeChoice.SetActive(true);
        }
        else if (CurrentDialogue.ThisDialogueType == DialogueData.DialogueType.ENDING_SUCCESS)
            StartBtn.gameObject.SetActive(true);
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
                GameManager.Instance.CurrentCalamity = (GameManager.Calamity)SelectedChoiceIndex + 1;
                CurrentDialogue = CurrentDialogue.OptionDialogues[SelectedChoiceIndex];
                SelectedChoiceIndex = 0;
                currentCoroutine = StartCoroutine(PlayDialogueText());
                break;
            case DialogueData.DialogueType.ENDING_SUCCESS:
                GameManager.Instance.SceneController.CurrentScene = CurrentDialogue.NextScene;
                break;
        }
        if (GameManager.Instance.PlayerGender == GameManager.Gender.MALE)
            BackgroundSprite.sprite = CurrentDialogue.DialogueBackgroundMale;
        if (GameManager.Instance.PlayerGender == GameManager.Gender.FEMALE)
            BackgroundSprite.sprite = CurrentDialogue.DialogueBackgroundFemale;
    }

    public void MakeChoice(int choice)
    {
        SelectedChoiceIndex = choice;
        TyphoonChoice.SetActive(false);
        EarthquakeChoice.SetActive(false);
        LoadNextDialogue();
    }

    public void HandleMinigame()
    {

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
