using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GenderSelectCore : MonoBehaviour
{
    //====================================================================================================================
    [SerializeField][ReadOnly] private GameManager.Gender SelectedGender;

    [Header("MALE VARIABLES")]
    [SerializeField] private Button MaleBtn;
    [SerializeField] private Sprite SelectedMaleSprite;
    [SerializeField] private Sprite UnselectedMaleSprite;
    [SerializeField] private GameObject MaleCharacter;

    [Header("FEMALE VARIABLES")]
    [SerializeField] private Button FemaleBtn;
    [SerializeField] private Sprite SelectedFemaleSprite;
    [SerializeField] private Sprite UnselectedFemaleSprite;
    [SerializeField] private GameObject FemaleCharacter;

    [Header("USERNAME VARIABLES")]
    [SerializeField] private TMP_InputField UsernameTMPInput;
    //====================================================================================================================

    #region GENDER SELECTION
    public void SelectMaleCharacter()
    {
        if (SelectedGender == GameManager.Gender.MALE)
            return;
        SelectedGender = GameManager.Gender.MALE;
        MaleCharacter.SetActive(true);
        MaleBtn.GetComponent<Image>().sprite = SelectedMaleSprite;
        DeselectFemaleCharacter();
    }

    private void DeselectMaleCharacter()
    {
        MaleCharacter.SetActive(false);
        MaleBtn.GetComponent<Image>().sprite = UnselectedMaleSprite;
    }

    public void SelectFemaleCharacter()
    {
        if (SelectedGender == GameManager.Gender.FEMALE)
            return;
        SelectedGender = GameManager.Gender.FEMALE;
        FemaleCharacter.SetActive(true);
        FemaleBtn.GetComponent<Image>().sprite = SelectedFemaleSprite;
        DeselectMaleCharacter();
    }

    private void DeselectFemaleCharacter()
    {
        FemaleCharacter.SetActive(false);
        FemaleBtn.GetComponent<Image>().sprite = UnselectedFemaleSprite;
    }
    #endregion

    #region USERNAME
    public void SubmitUserDetails()
    {
        if (UsernameTMPInput.text == "")
            return;

        GameManager.Instance.PlayerGender = SelectedGender;
        GameManager.Instance.Username = UsernameTMPInput.text;
        StartCoroutine(GameManager.Instance.APIClient.AddUser(GameManager.Instance.PlayerGender == GameManager.Gender.MALE ? "male": "female"));
        GameManager.Instance.SceneController.CurrentScene = "LobbyScene";
    }
    #endregion
}
