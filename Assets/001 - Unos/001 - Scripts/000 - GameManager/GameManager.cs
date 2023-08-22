using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/* The GameManager is the central core of the game. It persists all throughout run-time 
 * and stores universal game objects and variables that need to be used in multiple scenes. */
public class GameManager : MonoBehaviour
{
    #region VARIABLES
    //===========================================================
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null)
                    _instance = new GameObject().AddComponent<GameManager>();
            }

            return _instance;
        }
    }

    public enum Gender { NONE, MALE, FEMALE }
    public enum Calamity { NONE, TYPHOON, EARTHQUAKE}
    [field: SerializeField] public List<GameObject> GameMangerObj { get; set; }

    [field: SerializeField] public bool DebugMode { get; set; }
    [SerializeField] private string SceneToLoad;
    [field: SerializeField][field: ReadOnly] public bool CanUseButtons { get; set; }

    [field: Header("PLAYER VARIABLES")]
    [field: SerializeField][field: ReadOnly] public Gender PlayerGender { get; set; }
    [field: SerializeField][field: ReadOnly] public string Username { get; set; }
    [field: SerializeField][field: ReadOnly] public int Pesos { get; set; }
    [field: SerializeField][field: ReadOnly] public Calamity CurrentCalamity { get; set; }
    [field: SerializeField][field: ReadOnly] public List<Calamity> FinishedCalamities { get; set; }
    [field: SerializeField][field: ReadOnly] public QuestData CurrentQuest { get; set; }
    [field: SerializeField] public List<QuestData> TyphoonQuests { get; set; }

    [field: Header("CAMERA")]
    [field: SerializeField] public Camera MainCamera { get; set; }
    [field: SerializeField] public Camera MyUICamera { get; set; }

    [field: Header("MISCELLANEOUS SCRIPTS")]
    [field: SerializeField] public SceneController SceneController { get; set; }
    [field: SerializeField] public AnimationsLT AnimationsLT { get; set; }

    [field: Header("LOADING")]
    [field: SerializeField] public GameObject LoadingPanel { get; set; }

    [field: Header("ERROR")]
    [field: SerializeField] public GameObject ErrorPanel { get; set; }
    [field: SerializeField] public TextMeshProUGUI ErrorTMP { get; set; }
    //===========================================================
    #endregion

    #region CONTROLLER FUNCTIONS
    private void Awake()
    {
        if (_instance != null)
        {
            for (int a = 0; a < GameMangerObj.Count; a++)
                Destroy(GameMangerObj[a]);
        }

        for (int a = 0; a < GameMangerObj.Count; a++)
            DontDestroyOnLoad(GameMangerObj[a]);
    }

    private void Start()
    {
        if (DebugMode)
            SceneController.CurrentScene = SceneToLoad;
        else
            SceneController.CurrentScene = "MainMenuScene";

        foreach(QuestData quest in TyphoonQuests)
        {
            quest.IsAccomplised = false;
            quest.ItemsToGet.Clear();
        }
    }
    #endregion

    #region PANELS
    public void DisplayErrorPanel(string _message)
    {
        ErrorPanel.SetActive(true);
        ErrorTMP.text = _message;
    }

    public void HideErrorPanel()
    {
        ErrorPanel.SetActive(false);
    }
    #endregion
}
