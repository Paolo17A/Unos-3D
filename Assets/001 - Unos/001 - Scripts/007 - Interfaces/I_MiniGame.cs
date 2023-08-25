using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_MiniGame
{
    void InitializeScene();
    IEnumerator DisplayInstructionText();
    void InitializeGameplay();
    void HandleMovement();
    void HandleSuccess();
    void HandleFailure();
}
