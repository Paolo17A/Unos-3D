using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Dialogue
{
    void PlayStartingDialogue();
    IEnumerator PlayDialogueText();
    void LoadNextDialogue();
    void MakeChoice(int choice);
    void HandleMinigame();
}
