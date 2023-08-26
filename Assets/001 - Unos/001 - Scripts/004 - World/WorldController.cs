using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    [SerializeField] private WorldCore WorldCore;

    private void Awake()
    {
        //GameManager.Instance.MainCamera.clearFlags = CameraClearFlags.Skybox;
        GameManager.Instance.SceneController.ActionPass = true;
    }

    private void Start()
    {
        WorldCore.SetProperZones();
        WorldCore.InitializeQuests();
        //WorldCore.ToggleQuestPanel();
        WorldCore.HideZonePopUp();
        WorldCore.SetPlayerSpawnPosition();
    }
}
