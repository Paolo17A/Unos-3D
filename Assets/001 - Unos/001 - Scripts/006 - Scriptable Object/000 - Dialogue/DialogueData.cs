using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "Unos/Data/DialogueData")]
public class DialogueData : ScriptableObject
{
    public enum DialogueType { NONE, TALKING, CHOOSING, ENDING_SUCCESS, ENDING_FAIL, MINIGAME }

    [field: Header("DIALOGUE VARIABLES")]
    [field: SerializeField] public DialogueType ThisDialogueType { get; set; }
    [field: SerializeField][field: TextArea(minLines: 5, maxLines: 10)] public string DialogueContent { get; set; }
    [field: SerializeField] public Sprite DialogueBackgroundMale { get; set; }
    [field: SerializeField] public Sprite DialogueBackgroundFemale { get; set; }


    [field: Header("TALKING VARIABLES")]
    [field: SerializeField] public DialogueData NextDialogue { get; set; }

    [field: Header("CHOOSE VARIABLES")]
    [field: SerializeField] public List<DialogueData> OptionDialogues { get; set; }

    [field: Header("ENDING SUCCESS VARIABLES")]
    [field: SerializeField] public string NextScene { get; set; }

    [field: Header("ENDING FAIL VARIABLES")]
    [field: SerializeField] public DialogueData SavePointDialogue { get; set; }

    [field: Header("MINI GAME VARIABLES")]
    [field: SerializeField] public string MiniGameScene { get; set; }
    [field: SerializeField] public DialogueData ReturningPointDialogue {get;set;}
}
